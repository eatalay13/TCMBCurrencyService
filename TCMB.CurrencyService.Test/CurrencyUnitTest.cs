using NUnit.Framework;
using System.Linq;
using TCMB.CurrencyService.Dtos;
using TCMB.CurrencyService.Exporters;
using TCMB.CurrencyService.Models;

namespace TCMB.CurrencyService.Test
{
    public class CurrencyUnitTest
    {
        private ICurrencyService _currencyService;

        [SetUp]
        public void Setup()
        {
            _currencyService = TCMBCurrencyService.Instance;
        }

        [Test]
        public void Get_data_test()
        {
            var data = _currencyService.GetCurrencies();

            Assert.IsTrue(data.Any());
        }

        [Test]
        public void Filter_data_by_currencyCode_test()
        {
            var data = _currencyService.GetCurrencies(new CurrencyFilterDto
            {
                CurrencyCode = "USD"
            });

            Assert.IsTrue(data.Count() == 1 && data.First().CurrencyCode == "USD");
        }

        [Test]
        public void Sorting_asc_data_by_currencyCode_test()
        {
            var data = _currencyService.GetCurrencies(new CurrencyFilterDto
            {
                OrderColumn = nameof(Currency.CurrencyCode),
                OrderDirection = OrderDirection.ASC
            });

            Assert.IsTrue(data.Any() && data.First().CurrencyCode.StartsWith("A"));
        }

        [Test]
        public void Sorting_asc_data_by_forexBuying_test()
        {
            var minForexBuying = _currencyService.GetCurrencies().Min(e => e.ForexBuying);

            var sortinData = _currencyService.GetCurrencies(new CurrencyFilterDto
            {
                OrderColumn = nameof(Currency.ForexBuying),
                OrderDirection = OrderDirection.ASC
            });

            Assert.AreEqual(minForexBuying, sortinData.First().ForexBuying);
        }

        [Test]
        public void Sorting_desc_data_by_forexBuying_test()
        {
            var maxForexBuying = _currencyService.GetCurrencies().Max(e => e.ForexBuying);

            var sortinData = _currencyService.GetCurrencies(new CurrencyFilterDto
            {
                OrderColumn = nameof(Currency.ForexBuying),
                OrderDirection = OrderDirection.DESC
            });

            Assert.AreEqual(maxForexBuying, sortinData.First().ForexBuying);
        }

        [Test]
        public void Export_data_test()
        {
            var exporter = ExporterManager.GetExporterByName<Currency>("Json");

            var exportingData = _currencyService.ExportData(exporter, new CurrencyFilterDto
            {
                OrderColumn = nameof(Currency.ForexBuying),
                OrderDirection = OrderDirection.DESC
            });

            Assert.AreEqual(exportingData.FileExtension, ".json");
        }
    }
}