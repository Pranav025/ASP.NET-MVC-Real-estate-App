using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services
{
    public interface IPropertiesService
    {
        void Create(string district, int size, int? year, int price, string propertyType, string buildingType, int? floor, int? maxFloors);

        void UpdateTags(int propertyId);

        IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSyze);

        IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice);

        IEnumerable<PropertyViewModel> SearchBySizeAndYear(int minSize, int maxSize, int year);

        IEnumerable<PropertyViewModel> SearchByTag(string tag);
    }
}
