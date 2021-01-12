using ShopModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Varga_Bogdan_Proiect.Models.ShopViewModels
{
    public class MagazinIndexData
    {
        public IEnumerable<Magazin> Magazine { get; set; }
        public IEnumerable<Costum> Costume { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
