using LINQ_Sorgu_Uygulamalari.Data.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LINQ_Sorgu_Uygulamalari
{

    class CustomerDemo
    {
         public CustomerDemo()
        {
              Orders = new List<OrderDemo>();
        }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public int OrderCount { get; set; }
        public List<OrderDemo> Orders { get; set; }
    }

    class OrderDemo
    {
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public List<ProductDemo> Products { get; set; }
    }

    class ProductDemo
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {

            using (var db= new NorthwindContextLinq())
            {
                // Tüm müşteri kayıtlarını getiriniz.

                //var t = db.Customers.ToList();
                //foreach (var item in t)
                //{
                //    Console.WriteLine(item.FirstName+" "+ item.LastName);
                //}


                // Tüm müşteri kayıtlarından sadece first_name ve last_name bilgileri gelsin

                //var customers = db.Customers.Select(t => new { t.FirstName, t.LastName });

                //foreach (var item in customers)
                //{
                //    Console.WriteLine(item.FirstName + " " + item.LastName);
                //}


                // New York ta yaşayan müşterileri isim sırasına göre getiriniz.

                //var customers = db.Customers
                //    .Where(w => w.City == "New York")
                //    .Select(s => new { s.FirstName, s.LastName }).ToList();

                //foreach (var item in customers)
                //{
                //    Console.WriteLine(item.FirstName +" "+ item.LastName);
                //}


                // "Beverages" kategorisine ait ürünlerin isimlerini getiriniz

                //var categories = db.Products
                //    .Where(w => w.Category == "Beverages")
                //    .Select(s => s.ProductName).ToList();


                //foreach (var item in categories)
                //{
                //    Console.WriteLine(item);
                //}



                // En son eklenen 5 ürün bilgisini alalım

                //var productend = db.Products.OrderByDescending(o=>o.Id).Take(5);

                //foreach (var name in productend)
                //{
                //    Console.WriteLine(name.ProductName);
                //}


                // Fiyatı 10 ile 30 arasında olan ürünlerin isim,fiyat bilgilerini azalan şeklinde yazdıralım

                //var price = db.Products.Where(w => w.ListPrice>=10 && w.ListPrice<=30).OrderByDescending(o=>o.ListPrice).ToList();

                //foreach (var name in price)
                //{
                //    Console.WriteLine(name.ProductName + " *** "+ name.ListPrice);
                //}


                // "Beverages" kategorisindeki ürünlerin ortalama fiyatı nedir?

                //var categorysum = db.Products.Where(w => w.Category == "Beverages").Average(w=>w.ListPrice);

                //Console.WriteLine("Ortalama: {0} ", categorysum);


                // "Beverages" kategorisinde kaç ürün vardır?

                //var productcount = db.Products.Where(w => w.Category == "Beverages").Count();

                //Console.WriteLine("Ürün sayısı:{0} ", productcount);

                //ikinci yöntem;
                //var adet = db.Products.Count(c=>c.Category== "Beverages");

                //Console.WriteLine("Ürün sayısı:{0} ", adet);

                // "Beverages"  veya "Condiments" kategorilerindeki ürünlerin toplam fiyatı nedir?

                //var totalPrice = db.Products.Where(w => w.Category == "Beverages" || w.Category == "Condiments").Sum(w => w.ListPrice);

                //Console.WriteLine("Toplam Fiyat: {0} ", totalPrice);


                // 'Tea' kelimesini içeren ürünleri getiriniz.

                //var word = db.Products.Where(w =>w.ProductName.ToLower().Contains("Tea".ToLower()) || w.Description.ToLower().Contains("Tea")).ToList();

                //foreach (var item in word)
                //{
                //    Console.WriteLine(item.ProductName);
                //}

                // En pahalı ürün ve en ucuz ürün hangisidir?

                //var productExpensive = db.Products.OrderByDescending(w => w.ListPrice).Take(1).ToList();

                //foreach (var item in productExpensive)
                //{
                //    Console.WriteLine(item.ProductName +" ---->" + item.ListPrice);
                //}

                //var minProduct = db.Products.Min(p => p.ListPrice);
                //var maxProduct = db.Products.Max(p => p.ListPrice);


                //Console.WriteLine("Min : {0} , Max : {1} ", minProduct, maxProduct);

                //var Productmin = db.Products.Where(t => t.ListPrice == (db.Products.Min(p => p.ListPrice))).FirstOrDefault();

                //Console.WriteLine($"name: {Productmin.ProductName} , price: {Productmin.ListPrice}");


                //var Productmax = db.Products.Where(t => t.ListPrice == (db.Products.Max(p => p.ListPrice))).FirstOrDefault();

                //Console.WriteLine($"name: {Productmax.ProductName} , price: {Productmax.ListPrice}");

                // Siperiş sayısı 1 ve 1 den büyük olan müşterilere ulaşmak için

                //var customers = db.Customers.Where(w => w.Orders.Count > 0).Select(s=> new { s.FirstName,s.LastName}).ToList();

                //foreach (var item in customers)
                //{
                //    Console.WriteLine(item.FirstName + " " + item.LastName);
                //}


                var customers2 = db.Customers
                    .Where(w => w.Orders.Any()) //  Any() metodu en az 1 kaydı olan verileri getirir. Yandaki gösterimle aynıdır. Where(w => w.Orders.Count > 0) . Eğer hiç sipariş kaydı olmayan müşterileri getir deseydim .Where(w => !w.Orders.Any())
                    .Select(s =>
                      new CustomerDemo
                      {
                          Name=s.FirstName +" " + s.LastName,
                          CustomerId=s.Id,
                          OrderCount=s.Orders.Count(),
                          Orders=s.Orders.Select(a=> new OrderDemo {
                             OrderId=a.Id,
                             Total=(decimal)a.OrderDetails.Sum(od => od.Quantity * od.UnitPrice),
                             Products=a.OrderDetails.Select(t=>new ProductDemo
                             {
                                 Name=t.Product.ProductName,
                                 ProductId=(int)t.ProductId
                               
                             }).ToList()
                          }).ToList()
                      })
                    .OrderBy(o=>o.OrderCount)
                    .ToList();

                foreach (var customer in customers2)
                {
                    Console.WriteLine($"Id: {customer.CustomerId} Name: {customer.Name} Count: {customer.OrderCount} ");

                    foreach (var order in customer.Orders)
                    {
                        Console.WriteLine($"Order Id: {order.OrderId} Total: {order.Total}");

                        foreach (var product in order.Products)
                        {
                            Console.WriteLine($"ProductId: {product.ProductId} ProductName: {product.Name}");
                        }
                    }
                }

            }

        }

    }

}
