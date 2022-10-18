using System.Text;
using System.Xml;
using ThriftBox.Infrastructure.Courses.Abstractions;

namespace ThriftBox.Infrastructure.Courses.Handler;

public class ParseSymbolsHandler : IParseSymbolsHandler
{
    public async Task<CoursesResponse> Handle(string symbol)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Clear();
        string s = null;
        var result = await client.GetAsync("https://www.cbr.ru/scripts/XML_daily_eng.asp");
        using (var sr = new StreamReader(await result.Content.ReadAsStreamAsync(), Encoding.GetEncoding("iso-8859-1")))
        {
            s = sr.ReadToEnd();
        }

        XmlDocument xmltest = new XmlDocument();
        xmltest.LoadXml(s);
        CoursesResponse courses = new CoursesResponse();
        var coursesList = new List<CoursesInfo>();
        foreach (XmlNode node in xmltest.DocumentElement.ChildNodes)
        {
            if (node.SelectSingleNode("./CharCode").InnerXml == symbol)
            {
                var courseInfo = new CoursesInfo();
                courseInfo.CharCode = node.SelectSingleNode("./CharCode").InnerXml;
                courseInfo.Nominal = Int32.Parse(node.SelectSingleNode("./Nominal").InnerXml);
                courseInfo.Value = Decimal.Parse(node.SelectSingleNode("./Value").InnerXml);
                coursesList.Add(courseInfo);
            }
        }
        
        var charList = coursesList.Where(p => p.CharCode == symbol).ToList();
        if (charList.Any())
        {
            courses.IsSymbol = true;
            courses.ListCoursesInfo = charList.Where(p => p.CharCode == symbol).ToList();
        }
        else
        {
            courses.IsSymbol = false;
            courses.ListCoursesInfo = coursesList;
        }

        return courses;
    }
}