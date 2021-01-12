using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopModel.Data;
using ShopModel.Models;

namespace Varga_Bogdan_Proiect.Data
{
    public class DbInitializer
    {
        public static void Initialize(Shop context)
        {
            context.Database.EnsureCreated();
            if (context.Costume.Any())
            {
                return; // BD a fost creata anterior
            }
            var Costume = new Costum[]
            {
 new Costum{Denumire="SpiderMan",Categorie="Barbati",Pret=Decimal.Parse("150")},
 new Costum{Denumire="Batman",Categorie="Copii",Pret=Decimal.Parse("75")},
 new Costum{Denumire="Harley Quen",Categorie="Femei",Pret=Decimal.Parse("110")}
            };
            foreach (Costum b in Costume)
            {
                context.Costume.Add(b);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

 new Customer{CustomerID=1050,Name="PopescuMarcela",BirthDate=DateTime.Parse("1979-09-01")},
            new Customer{CustomerID=1045,Name="MihailescuCornel",BirthDate=DateTime.Parse("1969-07-08")},

 };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
 new Order{CostumID=1,CustomerID=1050,OrderDate=DateTime.Parse("2020-12-20")},
 new Order{CostumID=3,CustomerID=1045,OrderDate=DateTime.Parse("2020-12-21")},
 new Order{CostumID=1,CustomerID=1045,OrderDate=DateTime.Parse("2020-12-20")},
 new Order{CostumID=2,CustomerID=1050,OrderDate=DateTime.Parse("2020-12-25")},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();

            var magazine = new Magazin[]
 {

 new Magazin{MagazinName="MagazinBlaj",Adress="Str. Al.Golescu, nr.11"},
 new Magazin{MagazinName="MgazinCluj",Adress="Str.Hasdeu,nr.15"},
 };
            foreach (Magazin p in magazine )
            {
                context.Magazine.Add(p);
            }
            context.SaveChanges();
            var magazincostume = new MagazinCostum[]
            {
 new MagazinCostum { CostumID = Costume.Single(c => c.Denumire == "Spiderman" ).ID, MagazinID = magazine.Single(i => i.MagazinName =="MagazinBlaj").ID },
 new MagazinCostum { CostumID = Costume.Single(c => c.Denumire == "Batman" ).ID,MagazinID = magazine.Single(i => i.MagazinName =="MagazinCluj").ID },
 new MagazinCostum { CostumID = Costume.Single(c => c.Denumire == "Harley Quen" ).ID, MagazinID =magazine.Single(i => i.MagazinName =="MagazinCluj").ID },
            };
            foreach (MagazinCostum pb in magazincostume)
            {
                context.MagazinCostume.Add(pb);
            }
            context.SaveChanges();
        }
    }
}
    

