using System;
using System.Collections.Generic;
using System.Text;

namespace StreetLourd.Model.Schema
{
    class Run
    {
        public double Time { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
