using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Models;

namespace Models
{
    public abstract class XmlUnit
    {
        protected readonly string PATH;
        protected XmlDocument XML;

        public XmlUnit(string path)
        {
            if (Path.IsPathRooted(path))
            {
                PATH = path;
            }
            else
            {
                PATH = Path.GetFullPath(path);
            }
            if (new FileInfo(PATH).Exists)
            {
                XML = new XmlDocument();
                XML.Load(PATH);
            }
            else
            {
                XML = null;
                PATH = null;
            }
        }

        #region 比对
        private static bool 比对(XmlNode node, string Attribute, string Value)
        {
            string V = ((XmlElement)node).GetAttribute(Attribute).Trim();
            if (V.Equals(null) || V.Equals(string.Empty))
            {
                return false;
            }
            else if(V.Equals(Value.Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool 比对(XmlNode node, Dictionary<string, string> pairs)
        {
            foreach (var p in pairs)
            {
                if (!比对(node, p.Key, p.Value))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 获取节点
        public XmlNode GetNode(string Attribute,string Value)
        {
            foreach(XmlNode node in XML.FirstChild)
            {
                XmlNode n = GetNode(node, Attribute, Value);
                if (n != null)
                {
                    return n;
                }
            }
            return null;
        }
        public XmlNode GetNode(Dictionary<string,string> pairs)
        {
            foreach (XmlNode node in XML.FirstChild)
            {
                XmlNode n = GetNode(node, pairs);
                if (n != null)
                {
                    return n;
                }
            }
            return null;
        }
        public XmlNode GetNode(KeyValuePair<string,string> pair)
        {
            return GetNode(pair.Key, pair.Value);
        }
        public static XmlNode GetNode(XmlNode ParentNode,Dictionary<string,string> pairs)
        {
            if (比对(ParentNode,pairs))
            {
                return ParentNode;
            }
            if (ParentNode.HasChildNodes)
            {
                foreach (XmlNode n in ParentNode.ChildNodes)
                {
                    if (比对(n,pairs))
                    {
                        return n;
                    }
                    if (n.HasChildNodes)
                    {
                        XmlNode xml = GetNode(n, pairs);
                        if (xml != null)
                        {
                            return xml;
                        }
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public static XmlNode GetNode(XmlNode ParentNode,KeyValuePair<string,string> pair)
        {
            return GetNode(ParentNode, pair.Key, pair.Value);
        }
        public static XmlNode GetNode(XmlNode ParentNode,string Attribute,string Value)
        {
            if (比对(ParentNode,Attribute,Value))
            {
                return ParentNode;
            }
            if (ParentNode.HasChildNodes)
            {
                foreach(XmlNode n in ParentNode.ChildNodes)
                {
                    if (比对(n, Attribute, Value))
                    {
                        return n;
                    }
                    if (n.HasChildNodes)
                    {
                        XmlNode xml = GetNode(n, Attribute, Value);
                        if (xml != null)
                        {
                            return xml;
                        }
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public static XmlNode GetDirectChild(XmlNode ParentNode,string Attribute,string Value)
        {
            if (ParentNode.HasChildNodes)
            {
                foreach (XmlNode n in ParentNode.ChildNodes)
                {
                    if (比对(n, Attribute, Value))
                    {
                        return n;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public static XmlNode GetDirectChild(XmlNode ParentNode,KeyValuePair<string,string> pair)
        {
            return GetDirectChild(ParentNode, pair.Key, pair.Value);
        }
        public static XmlNode GetDirectChild(XmlNode ParentNode,Dictionary<string,string> pairs)
        {
            if (ParentNode.HasChildNodes)
            {
                foreach (XmlNode n in ParentNode.ChildNodes)
                {
                    if (比对(n, pairs))
                    {
                        return n;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 创建节点
        public XmlNode CreateNode(string NodeName,string Attribute,string Value)
        {
            XmlElement node = XML.CreateElement(NodeName);
            node.SetAttribute(Attribute, Value);
            return node;
        }
        public XmlNode CreateNode(string NodeName,KeyValuePair<string,string> pair)
        {
            return CreateNode(NodeName, pair.Key, pair.Value);
        }
        public XmlNode CreateNode(string NodeName,Dictionary<string,string> pairs)
        {
            XmlElement node = XML.CreateElement(NodeName);
            foreach (var p in pairs)
            {
                node.SetAttribute(p.Key, p.Value);
            }
            return node;
        }
        #endregion

        #region 移除节点
        public bool RemoveNode(KeyValuePair<string,string> pair)
        {
            return RemoveNode(pair.Key, pair.Value);
        }
        public bool RemoveNode(string Attribute,string Value)
        {
            return RemoveNode(XML.FirstChild, Attribute, Value);
        }
        public static bool RemoveNode(XmlNode ParentNode,string Attribute,string Value)
        {
            if (ParentNode.HasChildNodes)
            {
                XmlNode n = GetDirectChild(ParentNode, Attribute, Value);
                if (n != null)
                {
                    ParentNode.RemoveChild(n);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool RemoveNode(XmlNode ParentNode,KeyValuePair<string,string> pair)
        {
            return RemoveNode(ParentNode, pair.Key, pair.Value);
        }
        public static bool RemoveNode(XmlNode ParentNode,Dictionary<string, string> pairs)
        {
            if (ParentNode.HasChildNodes)
            {
                XmlNode n = GetDirectChild(ParentNode, pairs);
                if (n != null)
                {
                    ParentNode.RemoveChild(n);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region 添加节点
        public bool AddChild(XmlNode Child,string Attribute,string Value)
        {
            XmlNode node = GetNode(Attribute, Value);
            if (node != null)
            {
                node.AppendChild(Child);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        public void 保存()
        {
            XML.Save(PATH);
        }
    }
}
