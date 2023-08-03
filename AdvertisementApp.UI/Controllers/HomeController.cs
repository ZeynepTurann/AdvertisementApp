using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.UI.Extensions;
using AdvertisementApp.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertisementApp.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProvidedServiceService _providedServiceService;
        private readonly IAdvertisementService _advertisementService;
        public HomeController(ILogger<HomeController> logger, IProvidedServiceService providedServiceService, IAdvertisementService advertisementService)
        {
            _logger = logger;
            _providedServiceService = providedServiceService;
            _advertisementService = advertisementService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _providedServiceService.GetAllAsync();
            return this.ResponseView(response);
        }

        public IActionResult Privacy()
        {
            return RedirectToAction();
        }

        public async Task<IActionResult> HumanResource()
        {
            var response =await _advertisementService.GetActiveAsync();
            return this.ResponseView(response);
                
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
