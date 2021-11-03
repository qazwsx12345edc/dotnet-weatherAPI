using System;
using System.Collections.Generic;

#nullable disable

namespace  Domains.Models
{
    public partial class City
    {
        public ulong Id { get; set; }
        public string CityName { get; set; }
        public string FullName { get; set; }
        public byte Monitor { get; set; }
        public DateTime DbCreatedAt { get; set; }
        public DateTime DbUpdatedAt { get; set; }

        private City() { }

        public static City SetCity(ulong Id, string CityName, string FullName)
        {
            City city = new()
            {
                Id = Id,
                CityName = CityName,
                FullName = FullName,
            };

            return city;
        }
    }
}
