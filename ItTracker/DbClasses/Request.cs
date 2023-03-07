using System;
using System.Collections.Generic;

namespace ITTracker
{
    public partial class Request
    {
        public Request()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int IdCustomer { get; set; }
        public int IdStatus { get; set; }
        public string? ClarifationText { get; set; }
        public string? CustomerAnswer { get; set; }

        public virtual Customer IdCustomerNavigation { get; set; } = null!;
        public virtual OrderStatus IdStatusNavigation { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
