using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace XMLParser.Services
{
    public class TreeViewXML : TreeView
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
                Node.Header = xml.DocumentElement.OuterXml.Replace(xml.DocumentElement.InnerXml, "");
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
            foreach (XmlNode child in xmlElement.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    var childElement = child as XmlElement;
                    TreeViewItem ChildTree = new TreeViewItem();
                    if (child.HasChildNodes)
                    {
                        ChildTree.Header = child.OuterXml.Replace(child.InnerXml, "");
                        
                    }
                    else
                    {
                        ChildTree.Header = child.OuterXml;
                    }
                    if (ChildTree.Header.ToString().Contains("color"))
                    {
                        var str = ChildTree.Header.ToString().Split('=')[1];
                        str = str.Split('/')[0].Trim(' ', '\"');
                        ChildTree.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(str));
                    }
                    treeNode.Items.Add(ChildTree);
                    BuildNodes(ChildTree, childElement);
                }
                if (child.NodeType == XmlNodeType.Text)
                {
                    var childText = child as XmlText;
                    treeNode.Items.Add(new TreeViewItem { Header = childText.Value });
                }
            }
        }
    }
}

