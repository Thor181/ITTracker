using System;
using System.Collections.Generic;

namespace ITTracker
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            Requests = new HashSet<Request>();
        }

        public int Id { get; set; }
        public string FullName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
