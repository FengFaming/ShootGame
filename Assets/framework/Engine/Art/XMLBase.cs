using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Framework.Engine.Art
{
    /// <summary>
    /// 冯发明
    /// 2016/08/03
    /// 
    ///     配置文件基类
    /// </summary>
    public class XMLBase
    {
        private string m_XmlName;
        public string XmlName
        {
            set { m_XmlName = value; }
            get { return m_XmlName; }
        }

        public XMLBase(string name)
        {
            XmlName = name;
        }

        public virtual bool LoadXml(XmlElement node)
        {
            if (node == null)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string str = base.ToString();
            str += "\n";

            str += XmlName + "\n";

            return str;
        }
    }
}
