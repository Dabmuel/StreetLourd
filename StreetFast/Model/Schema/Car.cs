using System;
using System.Collections.Generic;
using System.Text;

namespace StreetLourd.Model.Schema
{
    class Car
    {
        public string Company { get; set; }
        public string Name { get; set; }
        public string Classe { get; set; }
        public int Score { get; set; }
        public List<Run> Runs { get; set; } = new List<Run>();
        public bool Enabled { get; set; } = true;
    }
}
