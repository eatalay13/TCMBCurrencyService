using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TCMB.CurrencyService.Models;

namespace TCMB.CurrencyService.Exporters
{
    public class JsonExporter<T> : IExporter<T>
    {
        public ExportResult Export(IEnumerable<T> items)
        {
            var jsonData = JsonSerializer.Serialize(items);

            return new ExportResult
            {
                Data = Encoding.UTF8.GetBytes(jsonData),
                MimeType = "application/json",
                FileExtension = ".json"
            };
        }
    }
}
