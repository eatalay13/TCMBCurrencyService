using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TCMB.CurrencyService.Dtos;
using TCMB.CurrencyService.Extensions;
using TCMB.CurrencyService.Models;

namespace TCMB.CurrencyService
{
    public class TCMBCurrencyService : CurrencyManagerBase, ICurrencyService
    {
        private readonly string _apiUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";

        private IEnumerable<Currency> currencyList;

        private DateTime _lastCheckTime;

        private static readonly object _lock = new();

        private static TCMBCurrencyService _instance = null;
        public static TCMBCurrencyService Instance
        {
            get
            {
                lock (_lock)
                {
                    _instance ??= new TCMBCurrencyService();

                    return _instance;
                }
            }
        }

        public TCMBCurrencyService()
        {
            DataCacheMinute = 15;
        }

        public short DataCacheMinute { get; set; }

        public override IEnumerable<Currency> GetCurrencies(CurrencyFilterDto filter = null)
        {
            var currencyList = GetXmlDataList();

            if (filter == null)
                return currencyList;

            var query = currencyList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.CurrencyCode))
                query = query.Where(e => e.CurrencyCode.ToLower().Contains(filter.CurrencyCode.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.SearchKey))
                query = query.Where(e => e.Isim.ToLower().Contains(filter.SearchKey.ToLower()) || e.CurrenyName.ToLower().Contains(filter.SearchKey.ToLower()));

            if (filter.Unit.HasValue)
                query = query.Where(e => e.Unit == filter.Unit);

            if (filter.MinForexBuying.HasValue)
                query = query.Where(e => e.ForexBuying >= filter.MinForexBuying);

            if (filter.MaxForexBuying.HasValue)
                query = query.Where(e => e.ForexBuying <= filter.MaxForexBuying);

            if (filter.MinForexSelling.HasValue)
                query = query.Where(e => e.ForexSelling >= filter.MinForexSelling);

            if (filter.MaxForexSelling.HasValue)
                query = query.Where(e => e.ForexBuying >= filter.MaxForexSelling);

            if (filter.MinBanknoteBuying.HasValue)
                query = query.Where(e => e.BanknoteBuying >= filter.MinBanknoteBuying);

            if (filter.MaxBanknoteBuying.HasValue)
                query = query.Where(e => e.BanknoteBuying <= filter.MaxBanknoteBuying);

            if (filter.MinBanknoteSelling.HasValue)
                query = query.Where(e => e.BanknoteSelling >= filter.MinBanknoteSelling);

            if (filter.MaxBanknoteSelling.HasValue)
                query = query.Where(e => e.BanknoteBuying >= filter.MaxBanknoteSelling);

            if (!string.IsNullOrWhiteSpace(filter.OrderColumn))
                query = query.DynamicOrder(filter.OrderColumn, filter.OrderDirection);

            return query.AsEnumerable();
        }

        private IEnumerable<Currency> GetXmlDataList()
        {
            if ((currencyList != null && currencyList.Any()) && (DateTime.Now - _lastCheckTime).TotalMinutes < DataCacheMinute)
                return currencyList;

            XDocument xmlDocument = XDocument.Load(_apiUrl);

            currencyList = xmlDocument.Descendants("Currency")
                                   .Select(e => new Currency
                                   {
                                       CurrencyCode = e.Attribute("CurrencyCode").Value,
                                       CurrenyName = e.Element("CurrencyName").Value,
                                       Isim = e.Element("Isim").Value,
                                       Unit = int.Parse(e.Element("Unit").Value),
                                       ForexBuying = decimal.TryParse(e.Element("ForexBuying").Value.Replace(".", ","), out decimal FbPrice) ? FbPrice : 0,
                                       ForexSelling = decimal.TryParse(e.Element("ForexSelling").Value.Replace(".", ","), out decimal FsPrice) ? FsPrice : 0,
                                       BanknoteBuying = decimal.TryParse(e.Element("BanknoteBuying").Value.Replace(".", ","), out decimal BbPrice) ? BbPrice : null,
                                       BanknoteSelling = decimal.TryParse(e.Element("BanknoteSelling").Value.Replace(".", ","), out decimal BsPrice) ? BsPrice : null,
                                   });

            _lastCheckTime = DateTime.Now;

            return currencyList;
        }
    }
}
