using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;

//I'm too lazy to use System.Addin, sue me

namespace PluginLoader
{
    public static class PluginLoader
    {
        public static ICollection<IPlugin> LoadDLL(string Filename)
        {
            List<IPlugin> plugins = new List<IPlugin>();

            Assembly assembly = Assembly.LoadFrom(Filename);

            /*Assembly core = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name.Equals("PluginLoader"));
            Type itype = core.GetType("PluginLoader.IPlugin");*/

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IPlugin).IsAssignableFrom(type) && type.IsAbstract == false)
                {
                    IPlugin b = type.InvokeMember(null,
                                               BindingFlags.CreateInstance,
                                               null, null, null) as IPlugin;
                    plugins.Add(b);
                }
            }

            return plugins;
        }

        public static ICollection<IPlugin> LoadAllDLLs(string Directory)
        {
            List<IPlugin> plugins = new List<IPlugin>();
            DirectoryInfo dir = new DirectoryInfo(Directory);

            foreach (FileInfo file in dir.GetFiles("*.dll"))
                plugins.AddRange(LoadDLL(file.FullName));

            return plugins;
        }
    }
}
