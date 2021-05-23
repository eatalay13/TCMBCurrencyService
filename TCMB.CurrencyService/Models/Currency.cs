namespace TCMB.CurrencyService.Models
{
    public class Currency
    {
        public string CurrencyCode { get; set; }
        public int Unit { get; set; }
        public string CurrenyName { get; set; }
        public string Isim { get; set; }
        public decimal ForexBuying { get; set; }
        public decimal ForexSelling { get; set; }
        public decimal? BanknoteBuying { get; set; }
        public decimal? BanknoteSelling { get; set; }
    }
}
