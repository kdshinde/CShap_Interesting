using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LearnDotNet
{
    interface IRun
    {
        Methods Methods { get;}
        /// <summary>
        /// Executes passed method with no return type
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        void Run(string method, params object[] parameters);
        /// <summary>
        /// Executes passed method and returns value through obj parameter
        /// </summary>
        /// <param name="method"></param>
        /// <param name="obj"></param>
        /// <param name="parameters"></param>
        //void Run(string method, out object obj, params object[] parameters);
    }
}
