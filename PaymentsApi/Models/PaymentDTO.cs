using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsApi.Models
{
    public class PaymentDTO
    {
        public long Id { get; set; }
        public double AmountToPay { get; set; }
        public string Name { get; set; }
        public bool IsPayed { get; set; }
    }
}
