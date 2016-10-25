using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearnDotNet
{
    /// <summary>
    /// Reflection vs Direct Call vs Delegates
    /// </summary>
    public class Reflection : IRun
    {
        public Methods Methods { get; } = new Methods();

        public void Run(string method, params object[] parameters)
        {
            switch (method)
            {
                case "ReflectionTiming":
                    ReflectionTiming();
                    break;
            }
        }

        public void Run(string method, out object obj, params object[] parameters)
        {
            obj = null;
            switch (method)
            {
                case "FillDataTableFromCollection":
                    obj = FillDataTableFromCollection(parameters);
                    break;
            }
        }

        public void Wait()
        {
            //Doing nothing
        }

        public DataTable FillDataTableFromCollection(params object[] parameters)
        {
            var employees = (IEnumerable<Employee>)parameters[0];

            var properties = typeof(Employee).GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name,
                    Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (var entity in employees)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        private static void ReflectionTiming()
        {
            var stopwatch = new Stopwatch();
            var timing = new Reflection();

            MethodInfo info = typeof(Reflection).GetMethod("Wait");
            //To make sure method is JITed
            timing.Wait();

            //Call using Reflectin Invoke method
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                info.Invoke(timing, null);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks + " for Reflection");
            stopwatch.Reset();

            Action converted = (Action)Delegate.CreateDelegate(typeof(Action), null, info);
            //Call using Delegate
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                converted();
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks + " for Delegate call");

            //Direct Call to the method
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                timing.Wait();
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks + " for Direct call");
            stopwatch.Reset();

            Console.Read();

            /*Result: Performce measured in Ticks, Delegate definitely is faster than reflection invoke :)
              	Direct	Delegate	Reflection
        Run1	19	    7       	418
        Run2	18	    11	        400
        Run3	14	    7	        421

             */
        }
    }

    public class Methods
    {
        public readonly string ReflectionTiming = "ReflectionTiming";
        public readonly string FillDataTableFromCollection = "FillDataTableFromCollection";
    }
}
