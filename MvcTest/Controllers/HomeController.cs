using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcTest.Models;
using System.Diagnostics;
using TCMB.CurrencyService;
using TCMB.CurrencyService.Dtos;
using TCMB.CurrencyService.Exporters;
using TCMB.CurrencyService.Models;

namespace MvcTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICurrencyService _currencyService;

        public HomeController(ILogger<HomeController> logger, ICurrencyService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }

        public IActionResult Index(IndexViewModel model)
        {
            model.Currencies = _currencyService.GetCurrencies(new CurrencyFilterDto
            {
                CurrencyCode = model.CurrencyCode,
                MaxBanknoteBuying = model.MaxBanknoteBuying,
                MaxBanknoteSelling = model.MaxBanknoteSelling,
                MaxForexBuying = model.MaxForexBuying,
                MaxForexSelling = model.MaxForexSelling,
                MinBanknoteBuying = model.MinBanknoteBuying,
                MinBanknoteSelling = model.MinBanknoteSelling,
                MinForexBuying = model.MinForexBuying,
                MinForexSelling = model.MinForexSelling,
                SearchKey = model.SearchKey,
                Unit = model.Unit,
                OrderColumn = model.OrderColumn,
                OrderDirection = model.OrderDirection
            });

            model.ExporterNames = ExporterManager.GetAllExporterNames();

            return View(model);
        }

        [HttpPost]
        public IActionResult Export(ExportViewModel model)
        {
            _logger.LogInformation("Data is exporting");

            var exporter = ExporterManager.GetExporterByName<Currency>(model.ExportType);

            var _file = _currencyService.ExportData(exporter, model.Filter);

            return File(_file.Data, _file.MimeType, "Currencies" + _file.FileExtension);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
