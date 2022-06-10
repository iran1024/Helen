using HelenServer.BugEngine.Contracts;
using System.Text.RegularExpressions;
using System.Xml;

namespace HelenServer.BugEngine.Services
{
    internal class ZentaoBugAnalyzer
    {
        private readonly string _xml;

        public ZentaoBugAnalyzer(string xml)
        {
            _xml = xml;
        }

        public IEnumerable<ZentaoBugModel>? Analyze()
        {
            var xmlDoc = ConvertToXmlObject(_xml);

            var root = xmlDoc.SelectSingleNode("xml");

            if (root != null)
            {
                var doc = XmlNodeAnalyzer(root);

                return doc;
            }
            return null;
        }

        private static XmlDocument ConvertToXmlObject(string xml)
        {
            var xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadXml(xml);
                return xmlDoc;
            }
            catch (Exception)
            {
                throw new XmlException("XML文件格式不正确");
            }
        }

        private static IEnumerable<ZentaoBugModel> XmlNodeAnalyzer(XmlNode root)
        {
            var zentao = new ZentaoBugModel();

            var rowsNode = root.SelectSingleNode("rows")!;

            var bugsNode = rowsNode.ChildNodes;

            var bugs = new List<ZentaoBugModel>();

            foreach (XmlNode node in bugsNode)
            {
                var bug = new ZentaoBugModel();

                bug.Id = int.Parse(node.SelectSingleNode("id")!.InnerText);
                bug.Title = node.SelectSingleNode("title")!.InnerText;
                bug.Product = node.SelectSingleNode("product")!.InnerText;
                bug.Module = node.SelectSingleNode("module")!.InnerText;
                bug.Demand = int.TryParse(node.SelectSingleNode("story")!.InnerText, out var _demand) ? _demand : -1;
                bug.Keywords = node.SelectSingleNode("keywords")!.InnerText;
                bug.Severity = int.Parse(node.SelectSingleNode("severity")!.InnerText);
                bug.Priority = int.Parse(node.SelectSingleNode("pri")!.InnerText);
                bug.Type = node.SelectSingleNode("type")!.InnerText;
                bug.Os = node.SelectSingleNode("os")!.InnerText;
                bug.Browser = node.SelectSingleNode("browser")!.InnerText;
                bug.ExpirationTime = DateTime.TryParse(node.SelectSingleNode("deadline")!.InnerText, out var expirationTime) ? expirationTime : null;
                bug.Reproduce = node.SelectSingleNode("steps")!.InnerText;
                bug.Status = node.SelectSingleNode("status")!.InnerText;
                bug.ActivedCount = int.Parse(node.SelectSingleNode("activatedCount")!.InnerText);
                bug.IsConfirmed = node.SelectSingleNode("confirmed")!.InnerText;
                bug.CCList = node.SelectSingleNode("mailto")!.InnerText;
                bug.Creator = node.SelectSingleNode("openedBy")!.InnerText;
                bug.CreatedTime = DateTime.Parse(node.SelectSingleNode("openedDate")!.InnerText);
                bug.AffectVersions = node.SelectSingleNode("openedBuild")!.InnerText; ;
                bug.Assignment = node.SelectSingleNode("assignedTo")!.InnerText;
                bug.AssignmentTime = DateTime.TryParse(node.SelectSingleNode("deadline")!.InnerText, out var asTime) ? asTime : null;
                bug.ResolvedBy = node.SelectSingleNode("resolvedBy")!.InnerText;
                bug.Resolution = node.SelectSingleNode("resolution")!.InnerText;
                bug.ResolvedTime = DateTime.TryParse(node.SelectSingleNode("resolvedDate")!.InnerText, out var resolvedTime) ? resolvedTime : null;
                bug.ResolvedVersion = node.SelectSingleNode("resolvedBuild")!.InnerText;
                bug.ClosedBy = node.SelectSingleNode("closedBy")!.InnerText;
                bug.ClosedTime = DateTime.TryParse(node.SelectSingleNode("closedDate")!.InnerText, out var closedTime) ? closedTime : null;
                bug.LastEditBy = node.SelectSingleNode("lastEditedBy")!.InnerText;
                bug.LastEditTime = DateTime.TryParse(node.SelectSingleNode("lastEditedDate")!.InnerText, out var lastEditTime) ? lastEditTime : null;
                bug.Attachments = node.SelectSingleNode("files")!.InnerText;
                bug.DuplicateId = TryPraseDuplicateId(node.SelectSingleNode("duplicateBug")!.InnerText, out var did) ? did : null;
                bug.LastActivedTime = null;

                bugs.Add(bug);
            }

            return bugs;
        }

        private static readonly Regex _didRegex = new(@"\(\d+\)+");

        private static bool TryPraseDuplicateId(string text, out int id)
        {
            if (text == "0")
            {
                id = 0;
                return false;
            }

            var match = _didRegex.Match(text);

            if (match.Value == "")
            {
                id = 0;
                return false;
            }

            id = int.Parse(match.Value[1..^1]);
            return true;
        }
    }
}