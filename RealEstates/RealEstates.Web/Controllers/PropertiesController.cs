using Microsoft.AspNetCore.Mvc;
using RealEstates.Models;
using RealEstates.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstates.Web.Controllers
{
    public class PropertiesController: Controller
    {
        private readonly IPropertiesService propertiesService;

        public PropertiesController(IPropertiesService propertiesService)
        {
            this.propertiesService = propertiesService;

        }

        public IActionResult Search()
        {
            return this.View();
        }

        public IActionResult DoSearch(int minPrice, int maxPrice)
        {
            var properties = this.propertiesService.SearchByPrice(minPrice, maxPrice);
            return this.View(properties);
        }

        public IActionResult DoSearchBySizeAndYear(int minSize, int maxSize, int year)
        {
            var properties = this.propertiesService.SearchBySizeAndYear(minSize, maxSize, year);
            return this.View(properties);
        }

        public IActionResult SearchByTag(string tag)
        {
            var properties = this.propertiesService.SearchByTag(tag);
            return this.View(properties);
        }
    }
}
