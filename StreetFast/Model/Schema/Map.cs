using System;
using System.Collections.Generic;
using System.Text;

namespace StreetLourd.Model.Schema
{
    class Map
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public float Km { get; set; }
        public string Type { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
