using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TaskFlowManager.Api.DAL
{
    public class XmlQueryProvider : IQueryProvider
    {
        private readonly Dictionary<string, string> _queries;
        public XmlQueryProvider()
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "database", "queries.xml");
            var xml = XDocument.Load(configPath);
            _queries = xml.Descendants("query")
                .ToDictionary(
                    x => (string)x.Attribute("name"),
                    x => x.Element("sql")?.Value.Trim() ?? ""
                );
        }
        public string GetQuery(string name)
        {
            return _queries.TryGetValue(name, out var query) ? query : string.Empty;
        }
    }
}
