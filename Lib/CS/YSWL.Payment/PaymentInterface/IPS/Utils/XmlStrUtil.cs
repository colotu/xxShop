using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

 
    public class XmlStrUtil
    {
        private XmlDocument xmlDoc = new XmlDocument();

        public XmlStrUtil()
        {
        }

        /// <summary>
        /// 加载xml文本
        /// </summary>
        /// <param name="xmlText">xml文本</param>
        public XmlStrUtil(string xmlText)
        {
            this.xmlDoc.LoadXml(xmlText);
        }

        private static string RemoveLashChar(string xpath)
        {
            if ((xpath.Contains("/") && (xpath.LastIndexOf('/') > 0)) && ((xpath.LastIndexOf('/') + 1) == xpath.Length))
            {
                xpath = xpath.Remove(xpath.LastIndexOf('/'), 1);
            }
            return xpath;
        }

        /// <summary>
        /// 获取Xml指定节点的值(静态方法)
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static string GetXmlValue(string xml, string xpath)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return string.Empty;
            }
            string xmlValue = string.Empty;
            xpath = RemoveLashChar(xpath);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            XPathNodeIterator ite = xmlDocument.CreateNavigator().Select(xpath);
            ite.MoveNext();
            if (ite.Count > 0)
            {
                xmlValue = ite.Current.Value;
            }
            return xmlValue;
        }


        /// <summary>
        /// 获取Xml的值
        /// </summary>
        public string XmlText
        {
            get
            {
                if (this.xmlDoc != null)
                {
                    return this.xmlDoc.InnerXml;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 报文格式化
        /// </summary>
        /// <param name="rspXMl"></param>
        /// <returns></returns>
        public static string FormatXML(string rspXMl)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rspXMl);
            StringWriter writer1 = new StringWriter();
            XmlTextWriter writer2 = new XmlTextWriter(writer1);
            writer2.Formatting = Formatting.Indented;
            writer2.Indentation = 0;
            writer2.IndentChar = '\t';
            doc.WriteTo(writer2);
            return writer1.ToString();
        }
    }

