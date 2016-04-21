using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AA2Install
{
    public class ModDL
    {
        public string Name;
        public Uri URL;
        public ModDL(string name, Uri uri)
        {
            Name = name;
            URL = uri;
        }
    }
    public class Modpack
    {
        protected int CurrentRev => 1;

        public string Name;
        public string Description;
        public string Authors;
        public int Version;
        public List<ModDL> Mods = new List<ModDL>();

        private int Revision;

        public Modpack(Stream modpackStream)
        {
            using (Stream stream = modpackStream)
            using (XmlReader xml = XmlReader.Create(stream))
            {

                xml.ReadToFollowing("AA2Modpack");
                xml.MoveToFirstAttribute();

                Revision = xml.ReadContentAsInt();

                if (Revision != CurrentRev)
                    throw new ArgumentException("Modpack supplied is invalid or not supported.");

                xml.ReadToFollowing("title");
                Name = xml.ReadElementContentAsString();
                xml.ReadToFollowing("description");
                Description = xml.ReadElementContentAsString();
                xml.ReadToFollowing("authors");
                Authors = xml.ReadElementContentAsString();
                xml.ReadToFollowing("version");
                Version = xml.ReadElementContentAsInt();
                
                while (xml.ReadToFollowing("mod"))
                {
                    xml.ReadToFollowing("name");
                    string name = xml.ReadElementContentAsString();
                    xml.ReadToFollowing("url");
                    Uri uri = new Uri(xml.ReadElementContentAsString());
                    Mods.Add(new ModDL(name, uri));
                }
            }
        }

        public Modpack(string filename) : this(new FileStream(filename, FileMode.Open))
        {

        }
    }
}
