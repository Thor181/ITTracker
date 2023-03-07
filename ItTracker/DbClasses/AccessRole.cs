using System;
using System.Collections.Generic;

namespace ITTracker
{
    public partial class AccessRole
    {
        public AccessRole()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Role { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
