using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models
{
    public class PurchaseTicketResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
