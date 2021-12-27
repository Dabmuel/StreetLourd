using System;
using System.Collections.Generic;
using System.Text;

namespace StreetLourd.Model.Schema
{
    class CarStat
    {
        public string Company { get; set; }
        public string Name { get; set; }
        public string Classe { get; set; }
        public int Score { get; set; }
        public int Stat { get; set; }
        public int RunNb { get; set; }
        public int MapNb { get; set; } = 1;
    }
}
