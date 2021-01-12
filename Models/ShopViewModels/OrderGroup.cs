using System;
using System.ComponentModel.DataAnnotations;
namespace Varga_Bogdan_Proiect.Models.ShopViewModels
{
    public class OrderGroup
    {
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }
        public int CostumCount { get; set; }

    }
}
