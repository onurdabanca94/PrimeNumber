using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumber.Core.Models
{
    public class ResultResponseModel
    {
        public Guid UserId { get; set; }
        public int MaxPrime { get; set; }
        public List<int> Inputs { get; set; }
    }
}
