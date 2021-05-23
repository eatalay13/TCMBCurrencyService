using System.Collections.Generic;
using TCMB.CurrencyService.Dtos;
using TCMB.CurrencyService.Exporters;
using TCMB.CurrencyService.Models;

namespace TCMB.CurrencyService
{
    public abstract class CurrencyManagerBase
    {
        public abstract IEnumerable<Currency> GetCurrencies(CurrencyFilterDto filter = null);

        public virtual ExportResult ExportData(IExporter<Currency> exporter, CurrencyFilterDto filter = null)
        {
            var currencyList = GetCurrencies(filter);

            return exporter.Export(currencyList);
        }
    }
}
