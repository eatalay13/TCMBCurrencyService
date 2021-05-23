using System.Collections.Generic;
using TCMB.CurrencyService.Dtos;
using TCMB.CurrencyService.Models;

namespace MvcTest.Models
{
    public class IndexViewModel
    {
        public string SearchKey { get; set; }
        public string CurrencyCode { get; set; }
        public int? Unit { get; set; }
        public decimal? MinForexBuying { get; set; }
        public decimal? MaxForexBuying { get; set; }
        public decimal? MinForexSelling { get; set; }
        public decimal? MaxForexSelling { get; set; }
        public decimal? MinBanknoteBuying { get; set; }
        public decimal? MaxBanknoteBuying { get; set; }
        public decimal? MinBanknoteSelling { get; set; }
        public decimal? MaxBanknoteSelling { get; set; }

        public string OrderColumn { get; set; }
        public OrderDirection OrderDirection { get; set; }


        public IEnumerable<Currency> Currencies { get; set; }
        public IEnumerable<string> ExporterNames { get; set; }
    }
}
