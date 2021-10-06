using System;
using System.Collections.Generic;

namespace LINQ_Sorgu_Uygulamalari.Data.EfCore
{
    public partial class OrdersTaxStatus
    {
        public OrdersTaxStatus()
        {
            Orders = new HashSet<Orders>();
        }

        public sbyte Id { get; set; }
        public string TaxStatusName { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
