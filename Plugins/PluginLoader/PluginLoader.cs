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

        public static bool TryLoadDLL(string filename, out ICollection<IPlugin> plugins)
        {
            try
            {
                plugins = LoadDLL(filename);
            }
            catch (FileLoadException) //blocked dll
            {
                plugins = new List<IPlugin>();
                return false;
            }
            return true;
        }

        public static bool LoadAllDLLs(string Directory, out ICollection<IPlugin> plugins)
        {
            List<IPlugin> plug = new List<IPlugin>();
            DirectoryInfo dir = new DirectoryInfo(Directory);
            ICollection<IPlugin> p;

            foreach (FileInfo file in dir.GetFiles("*.dll"))
            {
                if (TryLoadDLL(file.FullName, out p))
                    plug.AddRange(p);
                else
                {
                    plugins = plug;
                    return false;
                }
            }

            plugins = plug;
            return true;
        }
    }
}
