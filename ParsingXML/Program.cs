using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParsingXML
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("需要处理的文件夹：");
            string imagespath = Console.ReadLine();
            FileInfo[] f = new DirectoryInfo(imagespath).GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            FileInfo[] raw = f.Where(file => System.Text.RegularExpressions.Regex.IsMatch(
        file.Name,
        @"^rawprogram[0-9]+\.xml$",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase))
    .ToArray();
            FileInfo[] patch = f.Where(file => System.Text.RegularExpressions.Regex.IsMatch(
        file.Name,
        @"^patch[0-9]+\.xml$",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase))
    .ToArray();
            List<FileInfo> rawfiles = raw.ToList();
            List<FileInfo> patchfiles = patch.ToList();
            Console.WriteLine("搜寻到的文件：");
            foreach (FileInfo file in rawfiles)
            {
                Console.WriteLine(file.FullName);
            }
            foreach (FileInfo file in patchfiles)
            {
                Console.WriteLine(file.FullName);
            }
            Console.WriteLine("解析rawprogram.xml:");
            Rawxmlinfo rawxmlinfo = new Rawxmlinfo();
            foreach (FileInfo fileinfo in rawfiles)
            {
                
                rawxmlinfo.Rawinfo(fileinfo.FullName);
                for (int i = 0; i < rawxmlinfo.filename.Count; i++)
                {
                    Console.WriteLine("LUN" + rawxmlinfo.physical_partition_number[i] + "     " + i.ToString() + "      " + rawxmlinfo.label[i]  + "      start_sector:" + rawxmlinfo.start_sector[i]+ "      num_partition_sectors:" + rawxmlinfo.num_partition_sectors[i]);

                }


            }

            Console.ReadLine();
            Console.ReadKey();
        }
    }
    public class Rawxmlinfo
    {
        public List<string> filename = new List<string>();
        public List<string> label = new List<string>();
        public List<string> start_sector = new List<string>();
        public List<string> num_partition_sectors = new List<string>();
        public List<string> physical_partition_number = new List<string>();
        public List<string> sparse = new List<string>();
        public List<string> file_sector_offset = new List<string>();

        public void Rawinfo(string xmlpath)
        {

            string xmlFilePath = xmlpath;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlFilePath);
            XmlNode xmlNode = xmldoc.SelectSingleNode("data");
            XmlNodeList xmlnodelist = xmlNode.SelectNodes("program");
            ClearAllLists();
            foreach (XmlNode node in xmlnodelist)
            {
                filename.Add(node.Attributes["filename"]?.InnerText);
                label.Add(node.Attributes["label"]?.InnerText);
                start_sector.Add(node.Attributes["start_sector"]?.InnerText);
                num_partition_sectors.Add(node.Attributes["num_partition_sectors"]?.InnerText);
                physical_partition_number.Add(node.Attributes["physical_partition_number"]?.InnerText);
                sparse.Add(node.Attributes["sparse"]?.InnerText);
                file_sector_offset.Add(node.Attributes["file_sector_offset"]?.InnerText);
            }

        }
        private void ClearAllLists()
        {
            filename.Clear();
            label.Clear();
            start_sector.Clear();
            num_partition_sectors.Clear();
            physical_partition_number.Clear();
            sparse.Clear();
            file_sector_offset.Clear();
        }
    }
}





        
    