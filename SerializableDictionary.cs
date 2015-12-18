using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

[XmlRoot("dictionary")]
public class SerializableDictionary<TValue>
    : IDictionary<string, TValue>, IXmlSerializable
{
    public Dictionary<string, TValue> baseDict = new Dictionary<string, TValue>();
    public ICollection<string> Keys => baseDict.Keys;

    public ICollection<TValue> Values => baseDict.Values;

    public int Count => baseDict.Count;

    public bool IsReadOnly => false;

    private string getKey(string key) => key.ToLower().Remove(0, key.ToLower().LastIndexOf('\\') + 1);

    public TValue this[string key]
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
        XmlSerializer keySerializer = new XmlSerializer(typeof(string));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

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
        XmlSerializer keySerializer = new XmlSerializer(typeof(string));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        foreach (string key in this.Keys)
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
    public void Add(string key, TValue value)
    {
        baseDict.Add(getKey(key), value);
    }

    public bool ContainsKey(string key) => baseDict.ContainsKey(getKey(key));

    public bool Remove(string key) => baseDict.Remove(getKey(key));

    public bool TryGetValue(string key, out TValue value) => baseDict.TryGetValue(getKey(key), out value);

    public void Add(KeyValuePair<string, TValue> item)
    {
        baseDict.Add(getKey(item.Key), item.Value);
    }

    public void Clear()
    {
        baseDict.Clear();
    }

    public bool Contains(KeyValuePair<string, TValue> item) => (baseDict.ContainsKey(item.Key) && baseDict.ContainsValue(item.Value));

    public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<string, TValue> item)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator() => baseDict.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => baseDict.GetEnumerator();
}