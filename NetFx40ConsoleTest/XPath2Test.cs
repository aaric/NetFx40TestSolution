using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace NetFx40ConsoleTest
{
    /// <summary>
    /// https://github.com/StefH/XPath2.Net
    ///
    /// <code>Install-Package XPath2 -Version 1.1.3</code>
    /// </summary>
    class XPath2Test
    {
        public static void Main1(string[] args)
        {
            // XmlContentTest();
            string xmlFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                                 // + @"\test\test.xml";
                                 + @"\test\2023JZ_1-1.xrdml";
            XmlFileTest(xmlFilePath);
            XmlPath2Test(xmlFilePath);
        }

        public static void XmlContentTest()
        {
            Console.WriteLine("--------------- XmlContentTest ---------------");
            // string xmlContent = "<root><book title='Book 1'><author>Author 1</author></book><book title='Book 2'><author>Author 2</author></book></root>";
            string xmlContent =
                "<root xmlns='http://example.com'><book title='Book 1'><author>Author 1</author></book><book title='Book 2'><author>Author 2</author></book></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            // Console.WriteLine("Xml: " + doc.InnerXml);

            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("ns", "http://example.com");

            // string xpathExpression = "/root/book[@title='Book 1']";
            // string xpathExpression = "/root/book/author";
            // string expression = "//author";
            string expression = "//ns:author";

            // XmlNodeList nodes = doc.SelectNodes(expression);
            XmlNodeList nodes = doc.SelectNodes(expression, nsManager);
            if (null != nodes)
            {
                Console.WriteLine("Count: " + nodes.Count);

                foreach (XmlNode node in nodes)
                {
                    // Console.WriteLine("Title: " + node.Attributes["title"].Value);
                    // Console.WriteLine("Author: " + node.SelectSingleNode("author").InnerText);
                    Console.WriteLine("Text: " + node.InnerText);
                }
            }
        }

        public static void XmlFileTest(string xmlFilePath)
        {
            Console.WriteLine("--------------- XmlFileTest ---------------");
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);
            // Console.WriteLine("Xml: " + doc.InnerXml);

            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("ns", "http://www.xrdml.com/XRDMeasurement/1.5");
            nsManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            string expression = "//ns:intensities";

            XmlNode singleNode = doc.SelectSingleNode(expression, nsManager);
            Console.WriteLine("Data: " + singleNode.InnerXml);

            XmlNodeList nodes = doc.SelectNodes(expression, nsManager);
            if (null != nodes)
            {
                Console.WriteLine("Count: " + nodes.Count);

                foreach (XmlNode node in nodes)
                {
                    Console.WriteLine("Text: " + node.InnerXml);
                }
            }
        }

        public static void XmlPath2Test(string xmlFilePath)
        {
            Console.WriteLine("--------------- XmlPath2Test ---------------");
            XDocument doc = XDocument.Load(xmlFilePath);
            // Console.WriteLine("Xml: " + root);

            XmlNamespaceManager nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("ns", "http://www.xrdml.com/XRDMeasurement/1.5");
            nsManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            string expression = "//ns:intensities";

            IEnumerable<XElement> elems = doc.XPath2SelectElements(expression, nsManager);
            Console.WriteLine(elems.Count());
        }
    }
}