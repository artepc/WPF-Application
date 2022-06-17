using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Solution
{
    public class Phone
    {
        [Key]
        public int NumberId { get; set; }
        public long PhoneNumber { get; set; }

        public int PersonId { get; set; }


        public virtual Person Person { get; set; }


    }
}
