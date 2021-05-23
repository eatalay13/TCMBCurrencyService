using System.Collections.Generic;
using TCMB.CurrencyService.Dtos;
using TCMB.CurrencyService.Exporters;
using TCMB.CurrencyService.Models;

namespace TCMB.CurrencyService
{
    public interface ICurrencyService
    {
        short DataCacheMinute { get; set; }
        IEnumerable<Currency> GetCurrencies(CurrencyFilterDto filter = null);

        ExportResult ExportData(IExporter<Currency> exporter, CurrencyFilterDto filter = null);
    }
}
