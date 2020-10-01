namespace RealEstates.Importer
{
    // "Size": 20,
    // "Floor": 5,
    // "TotalFloors": 5,
    // "District": "град София, Център",
    // "Year": 0,
    // "Type": "1-СТАЕН",
    // "BuildingType": "Тухла",
    // "Price": 38000

    class JsonProperty
    {
        public int Size { get; set; }

        public int Floor { get; set; }

        public int TotalFloors { get; set; }

        public string District { get; set; }

        public int Year { get; set; }

        public string Type { get; set; }

        public string BuildingType { get; set; }

        public int Price { get; set; }

    }
}
