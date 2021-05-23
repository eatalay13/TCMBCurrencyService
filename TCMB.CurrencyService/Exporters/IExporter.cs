using System.Collections.Generic;
using TCMB.CurrencyService.Models;

namespace TCMB.CurrencyService.Exporters
{
    public interface IExporter<T>
    {
        ExportResult Export(IEnumerable<T> items);
    }
}
