using System;
using System.Collections.Generic;

namespace ITTracker
{
    public partial class Order
    {
        public int Id { get; set; }
        public int IdOrderStatus { get; set; }
        public int IdService { get; set; }
        public int IdRequest { get; set; }
        public int IdEmployee { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string? ClarifationText { get; set; }
        public int IdCustomer { get; set; }
        public string? CustomerAnswer { get; set; }

        public virtual Customer IdCustomerNavigation { get; set; } = null!;
        public virtual Employee IdEmployeeNavigation { get; set; } = null!;
        public virtual OrderStatus IdOrderStatusNavigation { get; set; } = null!;
        public virtual Request IdRequestNavigation { get; set; } = null!;
        public virtual Service IdServiceNavigation { get; set; } = null!;
    }
}
