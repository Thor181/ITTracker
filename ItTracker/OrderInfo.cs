using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTracker
{
    public class OrderInfo
    {
        public int? Id { get; set; }
        public int IdOrderStatus { get; set; }
        public string? Status { get; set; }
        public int IdService { get; set; }
        public string? Service { get; set; }
        public int IdRequest { get; set; }
        public string? CustomerFullName { get; set; }
        public int IdEmployee { get; set; }
        public int IdCustomer { get; set; }
        public string? ClarifationText { get; set; }
        public string? CustomerAnswer { get; set; }
        public string? OrderDescription { get; set; }
        public DateTime? FinalDate { get; set; }
        public decimal? Price { get; set; }
    }
}
