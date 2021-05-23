namespace TCMB.CurrencyService.Dtos
{
    public class CurrencyFilterDto
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
    }

    public enum OrderDirection
    {
        ASC,
        DESC
    }
}
