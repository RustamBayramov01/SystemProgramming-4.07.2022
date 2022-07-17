using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Vendor { get; set; }
        public int Year { get; set; }
        public string ImagePath { get; set; }
        public Uri Image { get { return new Uri(this.ImagePath); } }
    }
}

