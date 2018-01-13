/*
 *  create file time:1/11/2018 3:01:44 PM
 *  Describe:用来管理配置文件的
* */

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
    ///     配置文件管理类
    /// </summary>
    public class ConfigurationManager : BaseMonoSingleton<ConfigurationManager>
    {
        private Dictionary<string, XMLBase> m_AllXML;

        private string m_XmlSavePath;
        public string XmlSavePath
        {
            get { return m_XmlSavePath; }
            set { m_XmlSavePath = value; }
        }

        #region commonInface

        public override bool Initilize()
        {
            if (!base.Initilize())
                return false;

            m_AllXML = new Dictionary<string, XMLBase>();
            m_AllXML.Clear();

            return true;
        }

        public string GetXmlPath(XMLBase xml)
        {
            return GetXmlPath(xml.XmlName);
        }

        public string GetXmlPath(string name)
        {
            return m_XmlSavePath + "/" + name + ".xml";
        }
        #endregion

        #region inface
        /// <summary>
        /// 读取或者得到一个配置表
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="isForceLoad">是否强制重新读取配置表</param>
        /// <returns></returns>
        public XMLBase LoadXML(XMLBase xml, bool isForceLoad = false)
        {
            if (isForceLoad)
            {
                if (m_AllXML.ContainsKey(xml.XmlName))
                {
                    m_AllXML.Remove(xml.XmlName);
                }
            }

            if (m_AllXML.ContainsKey(xml.XmlName))
            {
                return m_AllXML[xml.XmlName];
            }

            XmlDocument doc = new XmlDocument();
            string path = GetXmlPath(xml.XmlName);
            doc.Load(File.OpenText(path));

            if (doc == null)
            {
                Debug.LogError(string.Format("Load xml 【{0}】 is Error!", xml.XmlName));
                return null;
            }

            XmlElement element = doc.DocumentElement;
            xml.LoadXml(element);
            m_AllXML.Add(xml.XmlName, xml);

            return xml;
        }
        #endregion
    }
}
