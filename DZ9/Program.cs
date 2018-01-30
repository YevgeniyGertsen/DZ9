using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DZ9.Model;
using System.IO;

namespace DZ9
{
    class Program
    {
        private static AreaDB DB = new AreaDB();
        static void Main(string[] args)
        {
            Task12();
        }

        public static void Task1()
        {
            DirectoryInfo info = Directory.CreateDirectory("Task1");
            var qeury = DB.Area
                .Where(w => w.PavilionId == 1);
            foreach (Area area in qeury)
            {
                XDocument xDoc = new XDocument(new XElement("Area",
                    new XElement("AreaId", area.AreaId),
                    new XElement("Name", area.Name),
                    new XElement("IP", area.IP)));
                xDoc.Save(info.Name + @"\" + area.AreaId + ".xml");
            }
        }
        public static void Task2()
        {
            DirectoryInfo dir = Directory.CreateDirectory("Task2");
            foreach (Area area in DB.Area)
            {
                DirectoryInfo info = Directory.CreateDirectory(dir.Name + @"\" + area.Name + "(" + area.AreaId + ")");
                XDocument xDoc = new XDocument(new XElement("Area",
                    new XElement("AreaId", area.AreaId),
                    new XElement("Name", area.Name),
                    new XElement("IP", area.IP)));
                xDoc.Save(info.FullName + @"\" + area.AreaId + ".xml");
            }
        }
        public static void Task3()
        {
            DirectoryInfo info = Directory.CreateDirectory("Task3");
            var qeury = DB.Area
                .Where(w => w.ParentId == 0);

            XElement xDoc = new XElement("root");
            
            foreach (Area area in qeury)
            {
                XElement xElement = new XElement(new XElement("Area",
                    new XElement("AreaId", area.AreaId),
                    new XElement("Name", area.Name),
                    new XElement("FullName",area.FullName),
                    new XElement("IP", area.IP),
                    new XElement("ParentId",area.ParentId)));
                xDoc.Add(xElement);
            }
            xDoc.Save(info.Name + @"\" + "Task3.xml");
        }
        public static void Task4()
        {
            DirectoryInfo info = Directory.CreateDirectory("Task4");
            var query = DB.Area.Where(w => w.IP != null).Join(DB.Timer, a => a.AreaId, t => t.AreaId, (a, t) => new
            {
                t.UserId,
                a.Name,
                t.DateStart
            });
            XElement xDoc = new XElement("root");
            foreach (var area in query)
            {
                XElement xElement = new XElement(new XElement("Area",
                            new XElement("UserID",area.UserId),
                            new XElement("AreaName",area.Name),
                            new XElement("DateStart",area.DateStart)));
                xDoc.Add(xElement);
            }
            xDoc.Save(info.Name + @"\" + "Task4.xml");
        }
        public static void Task5()
        {
            DirectoryInfo info = Directory.CreateDirectory("Task5");
            var query = DB.Timer.Where(w => w.DateFinish == null);

            XElement xDoc = new XElement("root");
            foreach (Timer timer in query)
            {
                XElement xElement = new XElement(new XElement("Timer",
                            new XElement("TimerId",timer.TimerId),
                            new XElement("UserID", timer.UserId),
                            new XElement("DateStart", timer.DateStart),
                            new XElement("DateFinish", timer.DateFinish),
                            new XElement("DurationInSeconds", timer.DurationInSeconds)));
                xDoc.Add(xElement);
            }
            xDoc.Save(info.Name + @"\" + "Task5.xml");
        }
        public static void Task6()
        {
            DirectoryInfo info = Directory.CreateDirectory("Task6");
            var query = DB.Timer.Where(w => w.DateFinish != null && w.DateStart != null);

            XElement xDoc = new XElement("root");
            foreach (Timer timer in query)
            {
                XElement xElement = new XElement(new XElement("Timer",
                            new XElement("AreaId",timer.AreaId),
                            new XElement("TimerId", timer.TimerId),
                            new XElement("UserId", timer.UserId),
                            new XElement("DocumentId", timer.DocumentId),
                            new XElement("DateStart", timer.DateStart),
                            new XElement("DateFinish", timer.DateFinish),
                            new XElement("DurationInSeconds", timer.DurationInSeconds)));
                xDoc.Add(xElement);
            }
            xDoc.Save(info.Name + @"\" + "Task6.xml");
        }
        public static void Task7()
        {
            DirectoryInfo info = Directory.CreateDirectory("Task7");
            XNamespace xNamespace = "http://logbook.itstep.org/";
            
            XElement xDoc = new XElement("root", new XAttribute(XNamespace.Xmlns + "Area",xNamespace));
            foreach (Area area in DB.Area)
            {
                XElement element = new XElement
                    (   "Area",
                        new XElement("AreaId", area.AreaId),
                        new XElement("Name", area.Name),
                        new XElement("IP", area.IP)
                    );
                xDoc.Add(element);
            }
            xDoc.Save(info.Name + @"\" + "Task7.xml");
        }
        public static void Task8()
        {
            XElement element = XElement.Load("Task6/Task6.xml");
            //Здесь вообще даже не обязательно выборку то делать, можно и без нее сразу бежать по документу и выводить нужное значение, но я так делать не захотел
            var query = element.Elements("Timer").Select(s => new
            {
                UserId = s.Element("UserId").Value,
                AreaId = s.Element("AreaId").Value,
                DocumentId = s.Element("DocumentId").Value
            });
            foreach (var timer in query)
            {
                Console.WriteLine(string.Format("\tUserId = {0}; AreaId = {1}; DocumentId = {2};",timer.UserId,timer.AreaId,timer.DocumentId));
            }
        }
        public static void Task9()
        {
            DirectoryInfo info = Directory.CreateDirectory("Task9");
            XElement element = XElement.Load("Task6/Task6.xml");

            foreach (var item in element.Elements("Timer"))
            {
                item.Element("DateFinish").SetValue(DateTime.Now);
            }
            element.Save(info.Name + @"\" + "TimeChangeToday_" + DateTime.Now.ToShortDateString() + ".xml");
        }
        public static void Task10()
        {
            XElement element = XElement.Load("Task3/Task3.xml");

            var query = element.Elements("Area").Select(s => new
            {
                AreaId = s.Element("AreaId").Value,
                Name = s.Element("Name").Value,
                FullName = s.Element("FullName").Value,
                IP = s.Element("IP").Value
            });
            foreach (var area in query)
            {
                Console.WriteLine(string.Format("\tAreaId = {0}; Name = {1}; FullName = {2}; IP = {3};", area.AreaId, area.Name, area.FullName, area.IP));
            }
        }
        public static void Task11()
        {
            XElement element = XElement.Load("Task7/Task7.xml");

            var query = DB.Timer.Where(w => w.UserId != 0 && w.DateFinish == null)
                .Join(element.Elements("Area"), t => t.AreaId, a => int.Parse(a.Element("AreaId").Value), (t, a) => new
                {
                    AreaId = a.Element("AreaId").Value,
                    Name = a.Element("Name").Value,
                    IP = a.Element("IP").Value,
                    t.UserId,
                    t.DateFinish
                });
            //IEnumerable<XElement> list = element.Elements("Area")
            //    .Where(w => w.Element("AreaId").Value == DB.Timer
            //        .Where(w2 => w2.DateFinish == null && w2.UserId == 0)
            //        .Select(s => s.AreaId).ToString());
            foreach (var area in query)
            {
                Console.WriteLine(string.Format("UserId = {0}; DateFinish = {1}; AreaId = {2}; Name = {3}; IP = {4};", area.UserId, area.DateFinish, area.AreaId, area.Name, area.IP));
            }

        }
        public static void Task12()
        {
            XElement element = XElement.Load("Task3/Task3.xml");
            int NotFulfilledCount = DB.Timer
                .Join(element.Elements("Area"), t => (int)t.AreaId, a => Convert.ToInt32(a.Element("AreaId").Value), (t, a) => new
                {
                    t.DateFinish,
                })
                .Where(w=>w.DateFinish == null)
                .Count();
            Console.WriteLine(NotFulfilledCount);

            var query = element.Elements("Area").Select(s => new
            {
                s.Element("Name").Value,
                Count = DB.Timer.Where(w => w.AreaId.ToString() == s.Element("AreaId").Value).Count()
            });

        }
    }
}
