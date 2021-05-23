using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TCMB.CurrencyService.Models;

namespace TCMB.CurrencyService.Exporters
{
    public class CsvExporter<T> : IExporter<T>
    {
        public ExportResult Export(IEnumerable<T> items)
        {
            bool isFirstIteration = true;
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (T item in items)
            {
                string[] propertyNames = item.GetType().GetProperties().Select(p => p.Name).ToArray();
                foreach (var prop in propertyNames)
                {
                    if (isFirstIteration == true)
                    {
                        for (int j = 0; j < propertyNames.Length; j++)
                        {
                            sb.Append("\"" + propertyNames[j] + "\"" + ',');
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("\r\n");
                        isFirstIteration = false;
                    }
                    object propValue = item.GetType().GetProperty(prop).GetValue(item, null);
                    sb.Append("\"" + propValue + "\"" + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("\r\n");
            }

            return new ExportResult
            {
                Data = Encoding.UTF8.GetBytes(sb.ToString()),
                MimeType = "text/csv",
                FileExtension = ".csv"
            };
        }
    }
}
