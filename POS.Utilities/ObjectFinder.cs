using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities
{
    public class ObjectFinder
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">YourNamespace.MyClass, YourAssemblyName, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T GetObject<T>(string typeName)
        {
            Type objType = Type.GetType(typeName);
            object obj = Activator.CreateInstance(objType);
            T objT = (T)obj;
            return objT;
        }
    }
}
