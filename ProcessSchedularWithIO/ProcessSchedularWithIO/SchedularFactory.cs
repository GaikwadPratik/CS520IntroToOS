using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSchedularWithIO
{
    /// <summary>
    /// Factory used for creating instances of runtime loaded <see cref="ProcessSchedularBaseClass"/> plugins.
    /// </summary>
    class SchedularFactory
    {
        /// <summary>
        /// Creates an instance of an <see cref="ProcessSchedularBaseClass"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="ProcessSchedularBaseClass"/> instance if the strategy was found in one of the plugin assemblies; otherwise null.</returns>
        public static ProcessSchedularBaseClass CreateSchedular(string name)
        {
            ProcessSchedularBaseClass _rtnSchedular = null;

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name.Equals(name))
                {
                    _rtnSchedular = Activator.CreateInstance(type) as ProcessSchedularBaseClass;
                    break;
                }
            }
            return _rtnSchedular;
        }
    }
}
