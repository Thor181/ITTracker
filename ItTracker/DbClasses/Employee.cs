using System;
using System.Collections.Generic;

namespace ITTracker
{
    public partial class Employee
    {
        public Employee()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int IdRole { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual AccessRole IdRoleNavigation { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
