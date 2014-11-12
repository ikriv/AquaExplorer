using System;
using System.Xml;

namespace IKriv.Azure
{
    static class XmlUtil
    {
        public static string GetNodeValue(this XmlNode root, string path)
        {
            if (root == null) return null;
            var node = root.SelectSingleNode(path);
            if (node == null) return null;
            return node.InnerText;
        }

        public static DateTime GetNodeDate(this XmlNode root, string path, DateTime defaultValue)
        {
            string s = GetNodeValue(root, path);
            if (s == null) return defaultValue;

            DateTime result;
            if (DateTime.TryParse(s, out result)) return result;
            return defaultValue;
        }

        public static int GetNodeInt(this XmlNode root, string path, int defaultValue)
        {
            string s = GetNodeValue(root, path);
            if (s == null) return defaultValue;

            int result;
            if (Int32.TryParse(s, out result)) return result;
            return defaultValue;
        }

        public static long GetNodeLong(this XmlNode root, string path, long defaultValue)
        {
            string s = GetNodeValue(root, path);
            if (s == null) return defaultValue;

            long result;
            if (Int64.TryParse(s, out result)) return result;
            return defaultValue;
        }

    }
}
