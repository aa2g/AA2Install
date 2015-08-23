using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Install
{
    static class Paths
    {
        /// <summary>
        /// 7Zip standalone location
        /// </summary>
        public static string _7Za
        {
            get
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    return Environment.CurrentDirectory + @"\x64\7za.exe";
                } 
                else 
                {
                    return Environment.CurrentDirectory + @"\x86\7za.exe";
                }
            }
        }
        /// <summary>
        /// AA2Decrypt location
        /// </summary>
        public static string AA2Decrypt
        {
            get
            {
                return Environment.CurrentDirectory + @"\x86\AA2Decrypt.exe";
            }
        }
        /// <summary>
        /// AA2Play data install location
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
                    return dir + @"\data";
                }
            }
        }
        /// <summary>
        /// AA2Edit data install location
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
                    return dir + @"\data";
                }
            }
        }
        /// <summary>
        /// Temporary files location
        /// </summary>
        public static string TEMP
        {
            get
            {
                return Environment.CurrentDirectory + @"\temp";
            }
        }
        /// <summary>
        /// 7z mods location
        /// </summary>
        public static string MODS
        {
            get
            {
                return Environment.CurrentDirectory + @"\mods";
            }
        }
        /// <summary>
        /// PP non-working directory
        /// </summary>
        public static string PP
        {
            get
            {
                return Environment.CurrentDirectory + @"\PP";
            }
        }
        /// <summary>
        /// PP working directory
        /// </summary>
        public static string WORKING
        {
            get
            {
                return Environment.CurrentDirectory + @"\compile";
            }
        }

    }
}
