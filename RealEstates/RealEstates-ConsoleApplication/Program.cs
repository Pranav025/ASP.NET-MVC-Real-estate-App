using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Text;

namespace RealEstates_ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var db = new RealEstateDbContext();
            db.Database.Migrate();

            // Top districts by AvgPrice
            IDistrictsService districtsService = new DistrictService(db);
            var districts = districtsService.GetTopDistrictsByAveragePrice();
            foreach (var district in districts)
            {
                Console.WriteLine($"{district.Name} => Average price: {district.AveragePrice:0.00} ({district.MinPrice} - {district.MaxPrice}) => {district.PropertiesCount} properties.");
            }



            // Search by price
            IPropertiesService propertiesService = new PropertyService(db);
            Console.Write("Enter min price: ");
            int minPrice = int.Parse(Console.ReadLine());
            Console.Write("Enter max price: ");
            int maxPrice = int.Parse(Console.ReadLine());

            var properties = propertiesService.SearchByPrice(minPrice, maxPrice);

            foreach (var property in properties)
            {
                Console.WriteLine($"{property.District}, Тип имот: {property.PropertyType}, Строеж: {property.BuildingType}, Етаж: {property.Floor},  Размер: {property.Size}m2, Година: {property.Year}г., Цена: {property.Price}лв.");
            }
        }

        public static void ResetDatabase(DbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            Console.WriteLine("Database deleted");
            dbContext.Database.EnsureCreated();
            Console.WriteLine("Database created");
        }
    }
}
