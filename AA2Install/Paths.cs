using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Install
{
    public static class Paths
    {
        /// <summary>
        /// 7Zip standalone location.
        /// </summary>
        public static string _7Za
        {
            get
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    return Environment.CurrentDirectory + @"\7z.dll";
                } 
                else 
                {
                    return Environment.CurrentDirectory + @"\7z.dll";
                }
            }
        }
        /// <summary>
        /// AA2Play data install location.
        /// </summary>
        public static string AA2Play
        {
            get
            {
                if (bool.Parse(Configuration.ReadSetting("AA2PLAY") ?? "False"))
                {
                    return Configuration.ReadSetting("AA2PLAY_Path");
                } 
                else 
                {
                    object dir = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\illusion\AA2Play", "INSTALLDIR", "NULL");
                    return dir + @"data";
                }
            }
        }
        /// <summary>
        /// AA2Edit data install location.
        /// </summary>
        public static string AA2Edit
        {
            get
            {
                if (bool.Parse(Configuration.ReadSetting("AA2EDIT") ?? "False"))
                {
                    return Configuration.ReadSetting("AA2EDIT_Path");
                } 
                else 
                {
                    object dir = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\illusion\AA2Edit", "INSTALLDIR", "NULL");
                    return dir + @"data";
                }
            }
        }
        /// <summary>
        /// Temporary files location.
        /// </summary>
        public static string TEMP => Environment.CurrentDirectory + @"\temp";
        /// <summary>
        /// 7z mods location.
        /// </summary>
        public static string MODS => Environment.CurrentDirectory + @"\mods";
        /// <summary>
        /// PP working directory.
        /// </summary>
        public static string WORKING => Environment.CurrentDirectory + @"\compile";
        /// <summary>
        /// Backup 7z location.
        /// </summary>
        public static string BACKUP => Environment.CurrentDirectory + @"\backup";
        /// <summary>
        /// Image and description cache.
        /// </summary>
        public static string CACHE => Environment.CurrentDirectory + @"\cache";
        /// <summary>
        /// Config file path.
        /// </summary>
        public static string CONFIG => Environment.CurrentDirectory + @"\config.json";
        /// <summary>
        /// Plugins directory.
        /// </summary>
        public static string PLUGINS => Environment.CurrentDirectory + @"\plugins";

    }
}
