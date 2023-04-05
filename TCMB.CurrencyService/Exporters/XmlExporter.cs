using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TCMB.CurrencyService.Models;

namespace TCMB.CurrencyService.Exporters
{
    public class XmlExporter<T> : IExporter<T>
    {
        public ExportResult Export(IEnumerable<T> items)
        {
            XmlSerializer serializer = new(typeof(List<T>));

            var stringwriter = new StringWriter();
            serializer.Serialize(stringwriter, items.ToList());

            return new ExportResult
            {
                Data = Encoding.UTF8.GetBytes(stringwriter.ToString()),
                MimeType = "text/xml",
                FileExtension = ".xml"

            };
        }
    }
}
