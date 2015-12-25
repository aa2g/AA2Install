using AA2Install;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace AA2Install
{
    [XmlRoot("dictionary")]
    [DebuggerDisplay("Count = {Count}")]
    public class ModDictionary
        : IDictionary<string, Mod>, IXmlSerializable
    {
        public Dictionary<string, Mod> baseDict = new Dictionary<string, Mod>();
        public ICollection<string> Keys => baseDict.Keys;

        public ICollection<Mod> Values => baseDict.Values;

        public int Count => baseDict.Count;

        public bool IsReadOnly => false;

        private static string getKey(string key) => key.ToLower().Remove(0, key.ToLower().LastIndexOf('\\') + 1);

        public Mod this[string key]
        {
            get
            {
                return baseDict[getKey(key)];
            }

            set
            {
                baseDict[getKey(key)] = value;
            }
        }
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema() => null;

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader == null)
                return;

            XmlSerializer keySerializer = new XmlSerializer(typeof(string));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(Mod));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                string key = (string)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                Mod value = (Mod)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (writer == null)
                return;

            XmlSerializer keySerializer = new XmlSerializer(typeof(string));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(Mod));

            foreach (string key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                Mod value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
        public void Add(string key, Mod value)
        {
            baseDict.Add(getKey(key), value);
        }

        public bool ContainsKey(string key) => baseDict.ContainsKey(getKey(key));

        public bool Remove(string key) => baseDict.Remove(getKey(key));

        public bool TryGetValue(string key, out Mod value) => baseDict.TryGetValue(getKey(key), out value);

        public void Add(KeyValuePair<string, Mod> item)
        {
            baseDict.Add(getKey(item.Key), item.Value);
        }

        public void Clear()
        {
            baseDict.Clear();
        }

        public bool Contains(KeyValuePair<string, Mod> item) => (baseDict.ContainsKey(item.Key) && baseDict.ContainsValue(item.Value));

        public void CopyTo(KeyValuePair<string, Mod>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, Mod> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, Mod>> GetEnumerator() => baseDict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => baseDict.GetEnumerator();
    }

    [XmlRoot("dictionary")]
    [Serializable()]
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema() => null;

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader == null)
                return;

            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (writer == null)
                return;

            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }
}