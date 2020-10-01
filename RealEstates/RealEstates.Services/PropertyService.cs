using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RealEstates.Services
{
    public class PropertyService: IPropertiesService
    {
        private RealEstateDbContext db;

        public PropertyService(RealEstateDbContext db)
        {
            this.db = db;
        }

        public void Create(string district, int size, int? year, int price, string propertyType, string buildingType, int? floor, int? maxFloors)
        {
            if (district == null)
            {
                throw new ArgumentNullException(nameof(district));
            }

            var property = new RealEstateProperty
            {
                Size = size,
                Price = price,
                Year = year,
                Floor = floor,
                TotalNumberOfFloors = maxFloors,
            };

            if (property.Year < 1800)
            {
                property.Year = null;
            }

            if (property.Floor <= 0)
            {
                property.Floor = null;
            }

            if (property.TotalNumberOfFloors <= 0)
            {
                property.TotalNumberOfFloors = null;
            }

            //DistrictType
            var districtEntity = this.db.Districts
                .FirstOrDefault(x => x.Name.Trim() == district.Trim());
            if (districtEntity == null)
            {
                districtEntity = new District { Name = district };
            }
            property.District = districtEntity;

            //BuildingType
            var buildingTypeEntity = this.db.BuildingTypes
                .FirstOrDefault(x => x.Name.Trim() == buildingType.Trim());
            if (buildingTypeEntity == null)
            {
                buildingTypeEntity = new BuildingType{ Name = buildingType };
            }
            property.BuildingType = buildingTypeEntity;

            //PropertyType
            var propertyTypeEntity = this.db.PropertyTypes
                .FirstOrDefault(x => x.Name.Trim() == propertyType.Trim());
            if (propertyTypeEntity == null)
            {
                propertyTypeEntity = new PropertyType { Name = propertyType };
            }
            property.PropertyType = propertyTypeEntity;

            this.db.RealEstateProperties.Add(property);
            this.db.SaveChanges();

            this.UpdateTags(property.Id);
        }

        public IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize)
        {
            return db.RealEstateProperties
               .Where(x => x.Year >= minYear && x.Year <= maxYear && x.Size >= minSize && x.Size <= maxSize)
               .Select(MapToPropertyViewModel())
               .OrderBy(x => x.Price)
               .ToList();
        }

        public IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice)
        {
            return this.db.RealEstateProperties
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice)
                .Select(MapToPropertyViewModel())
                .OrderBy(x => x.Price)
                .ToList();
        }

        public IEnumerable<PropertyViewModel> SearchBySizeAndYear(int minSize, int maxSize, int year)
        {
            return this.db.RealEstateProperties
                .Where(x => x.Year == year && x.Size >= minSize && x.Size <= maxSize)
                .Select(MapToPropertyViewModel())
                .OrderBy(x => x.Price)
                .ToList();
        }

        public IEnumerable<PropertyViewModel> SearchByTag(string tagName)
        {
            return this.db.RealEstateProperties
                .Where(x => x.Tags.Any(x => x.Tag.Name == tagName))
                .Select(MapToPropertyViewModel())
                .OrderBy(x => x.Price)
                .ToList();
                
        }

        public void UpdateTags(int propertyId)
        {
            var property = this.db.RealEstateProperties
                .FirstOrDefault(x => x.Id == propertyId);

            property.Tags.Clear();

            if (property.Year.HasValue && property.Year < 1990)
            {
                AddTag(property, "OldBuilding");
            }

            if (property.Year.HasValue && property.Year > 1990)
            {
                AddTag(property, "NewBuilding");
            }

            if (property.Size > 120)
            {
                AddTag(property, "HugeProperty");
            }

            if (property.Size < 120 && property.Size >= 60)
            {
                AddTag(property, "LargeProperty");
            }

            if (property.Size < 60)
            {
                AddTag(property, "SmallProperty");
            }

            if (property.Year > 2010)
            {
                AddTag(property, "NewProperty");
            }

            if (property.Year < 2010)
            {
                AddTag(property, "OldProperty");
            }

            if (property.TotalNumberOfFloors > 5 && property.TotalNumberOfFloors > 5)
            {
                AddTag(property, "WithElevator");
            }

            if (property.Floor == property.TotalNumberOfFloors)
            {
                AddTag(property, "LastFloor");
            }

            if (property.Floor != 1 && property.Floor != property.TotalNumberOfFloors)
            {
                AddTag(property, "NotLastFloor");
            }

            if ((double)(property.Price / property.Size) < 750)
            {
                AddTag(property, "Bestdeal");
            }

            if ((double)(property.Price / property.Size) > 1000)
            {
                AddTag(property, "LuxuryProperty");
            }

            this.db.SaveChanges();
        }

        private void AddTag(RealEstateProperty property, string tagName)
        {
            property.Tags.Add(
                                new RealEstatePropertyTag
                                {
                                    Tag = this.GetOrCreateTag(tagName)
                                });
        }

        private Tag GetOrCreateTag(string tag)
        {
           var tagEntity =  this.db.Tags.FirstOrDefault(x => x.Name.Trim() == tag.Trim());

            if (tagEntity == null)
            {
                tagEntity = new Tag { Name = tag };
            }

            return tagEntity;
        }


        private static Expression<Func<RealEstateProperty, PropertyViewModel>> MapToPropertyViewModel()
        {
            return x => new PropertyViewModel
            {
                District = x.District.Name,
                Price = x.Price,
                Floor = (x.Floor ?? 0).ToString() + "/" + (x.TotalNumberOfFloors ?? 0).ToString(),
                Size = x.Size,
                Year = x.Year,
                BuildingType = x.BuildingType.Name,
                PropertyType = x.PropertyType.Name,
            };
        }

       
    }
}
