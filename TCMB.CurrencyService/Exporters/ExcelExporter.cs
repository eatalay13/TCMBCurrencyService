using System;
using System.Collections.Generic;
using TCMB.CurrencyService.Models;

namespace TCMB.CurrencyService.Exporters;
public class ExcelExporter<T> : IExporter<T>
{
    public ExportResult Export(IEnumerable<T> items)
    {
        throw new NotImplementedException();
    }
}
