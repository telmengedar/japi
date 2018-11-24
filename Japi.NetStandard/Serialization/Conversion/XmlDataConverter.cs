using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NightlyCode.Japi.Extern;
using NightlyCode.Japi.Serialization.Data;

namespace NightlyCode.Japi.Serialization.Conversion {

    /// <summary>
    /// writes xml from java data structures
    /// </summary>
    public class XmlDataConverter : IDataConverter<XmlDocument> {

        /// <summary>
        /// writes an xml document out of a java data structure
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public XmlDocument Convert(IJavaData data) {
            HashSet<JavaObject> visited = new HashSet<JavaObject>();

            XmlDocument document = new XmlDocument();
            if(data is JavaObject)
                WriteObject(document.CreateAndAppendElement(GetTagName(((JavaObject)data).Type)), (JavaObject)data, visited);
            else if(data is JavaValue)
                document.CreateAndAppendElement("value").InnerText = GetValue((JavaValue)data);
            else if(data is JavaArray)
                WriteArray(document.CreateAndAppendElement("array"), (JavaArray)data, visited);
            return document;
        }

        void WriteArray(XmlNode parent, JavaArray array, HashSet<JavaObject> visited) {
            if(array.Items.Count == 0)
                return;

            IJavaData data = array.Items.First();
            if(data is JavaArray) {
                foreach(JavaArray childarray in array.Items.Cast<JavaArray>())
                    WriteArray(parent.CreateAndAppendElement("array"), childarray, visited);
            }
            else if(data is JavaValue) {
#if FRAMEWORK35
                parent.InnerText = string.Join(";", array.Items.Cast<JavaValue>().Select(GetValue).ToArray());
#else
                parent.InnerText = string.Join(";", array.Items.Cast<JavaValue>().Select(GetValue));
#endif
            }
            else if(data is JavaObject) {
                foreach(JavaObject item in array.Items.Cast<JavaObject>())
                    WriteObject(parent.CreateAndAppendElement(GetTagName(item.Type)), item, visited);
            }
            else throw new InvalidOperationException("Invalid data type in array");
        }

        string GetValue(JavaValue value) {
            object jvalue = value.Value;
            if(jvalue is byte[])
#if FRAMEWORK35
                jvalue = string.Join(",", ((byte[])jvalue).Select(b => b.ToString("x2")).ToArray());
#else
                jvalue = string.Join(",", ((byte[])jvalue).Select(b => b.ToString("x2")));
#endif
            return jvalue?.ToString() ?? "";
        }

        void WriteObject(XmlNode parent, JavaObject data, HashSet<JavaObject> visited) {
            if(visited.Contains(data))
                return;
            visited.Add(data);

            foreach(JavaField field in data.Fields)
                WriteData(parent.CreateAndAppendElement(GetTagName(field.Name)), field.Value, visited);

            foreach(IJavaData custom in data.Custom)
                WriteData(parent.CreateAndAppendElement("data"), custom, visited);

            visited.Remove(data);
        }

        void WriteData(XmlNode parent, IJavaData data, HashSet<JavaObject> visited) {
            if (data is JavaArray) {
                JavaArray array = (JavaArray)data;
                WriteArray(parent, array, visited);
            }
            else if (data is JavaValue) {
                parent.InnerText = GetValue((JavaValue)data);
            }
            else if (data is JavaObject)
            {
                WriteObject(parent.CreateAndAppendAttribute("type", GetTagName(((JavaObject)data).Type)), (JavaObject)data, visited);
            }
            else throw new InvalidOperationException("Invalid data type in array");
        }

        string GetTagName(string name) {
            int indexof = name.LastIndexOf("$", StringComparison.Ordinal);
            if(indexof == -1)
                indexof = name.LastIndexOf(".", StringComparison.Ordinal);
            name = name.Substring(indexof + 1);
            if(name.Length == 0 || !char.IsLetter(name[0]))
                name = "I" + name;
            return name;
        }
    }
}