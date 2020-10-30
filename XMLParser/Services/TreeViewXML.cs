using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace XMLParser.Services
{
    public class TreeViewXML : System.Windows.Controls.TreeView
    {
        public void GetXMLFromURL(string url)
        {
            using (var client = new WebClient())
            {
                BuildTreeFromXML(client.DownloadString(url));
            }
        }
        public void BuildTreeFromXML(string str)
        {
            var xml = new XmlDocument();
            xml.LoadXml(str);
            Items.Clear();
            var Node = new TreeViewItem();
            if (xml.DocumentElement != null)
            {
                Node.Header = xml.DocumentElement.Name;
            }
            else
            {
                Node.Header = "Root";
            }
            Items.Add(Node);
            BuildNodes(Node, xml.DocumentElement);
        }
        public void BuildNodes(TreeViewItem treeNode, XmlElement xmlElement)
        {
            /*int i = 0;
            while (xmlNode.HasChildNodes)
            {
                var Node = xmlNode.ChildNodes[i];
                tree.AddChild(new TreeViewItem(){ Header = Node.Name });
                tree = tree.Ch;
                BuildNodes(Tree, Node);
            }*/
            foreach (XmlNode child in xmlElement.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    var childElement = child as XmlElement;
                    TreeViewItem ChildTree = new TreeViewItem { Header = child.Name };
                    treeNode.Items.Add(ChildTree);
                    BuildNodes(ChildTree, childElement);
                }
                if (child.NodeType == XmlNodeType.Text)
                {
                    var childText = child as XmlText;
                    treeNode.Items.Add(new TreeViewItem { Header = child.Value });
                }

            }

        }
    }
}

