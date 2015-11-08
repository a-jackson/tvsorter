using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TVSorter
{
    public static class XmlExtensions
    {
        public static void InsertAt(this XmlNode node, XmlNode insertingNode, int index = 0)
        {
            if (insertingNode == null)
                return;
            if (index < 0)
                index = 0;

            var childNodes = node.ChildNodes;
            var childrenCount = childNodes.Count;

            if (index >= childrenCount)
            {
                node.AppendChild(insertingNode);
                return;
            }

            var followingNode = childNodes[index];

            node.InsertBefore(insertingNode, followingNode);
        }
    }
}
