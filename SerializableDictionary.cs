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
    public ICollection<string> Keys
    {
        get
        {
            return baseDict.Keys;
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            return baseDict.Values;
        }
    }

    public int Count
    {
        get
        {
            return baseDict.Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }

    public TValue this[string key]
    {
        get
        {
            return baseDict[key.ToLower()];
        }

        set
        {
            baseDict[key.ToLower()] = value;
        }
    }
    #region IXmlSerializable Members
    public System.Xml.Schema.XmlSchema GetSchema()
    {
        return null;
    }

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
        baseDict.Add(key.ToLower(), value);
    }

    public bool ContainsKey(string key)
    {
        return baseDict.ContainsKey(key.ToLower());
    }

    public bool Remove(string key)
    {
        return baseDict.Remove(key.ToLower());
    }

    public bool TryGetValue(string key, out TValue value)
    {
        return baseDict.TryGetValue(key.ToLower(), out value);
    }

    public void Add(KeyValuePair<string, TValue> item)
    {
        baseDict.Add(item.Key.ToLower(), item.Value);
    }

    public void Clear()
    {
        baseDict.Clear();
    }

    public bool Contains(KeyValuePair<string, TValue> item)
    {
        return (baseDict.ContainsKey(item.Key) && baseDict.ContainsValue(item.Value));
    }

    public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<string, TValue> item)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        return baseDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return baseDict.GetEnumerator();
    }
}