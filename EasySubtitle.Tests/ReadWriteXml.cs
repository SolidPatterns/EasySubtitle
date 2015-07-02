using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;

namespace EasySubtitle.Tests
{
    [TestFixture]
    public class ReadWriteXml
    {
        [Test]
        public void test()
        {
            var doc = XDocument.Load(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.GetFiles("AppSettings.config").First().FullName);

            var xelement =
                doc.Descendants("add").FirstOrDefault(x => x.Attributes("key").Any(y => y.Value.Equals("SelectedSubtitleLanguages")));

            Debug.WriteLine(xelement.LastAttribute.Value);
            xelement.LastAttribute.Value = "tur,eng,ger";
            Debug.WriteLine(xelement.Attribute("value").Value);
            doc.Save(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.GetFiles("AppSettings.config").First().FullName);
        }
    }
}
