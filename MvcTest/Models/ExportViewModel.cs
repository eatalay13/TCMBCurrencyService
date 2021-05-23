using TCMB.CurrencyService.Dtos;

namespace MvcTest.Models
{
    public class ExportViewModel
    {
        public CurrencyFilterDto Filter { get; set; }
        public string ExportType { get; set; }
    }
}
