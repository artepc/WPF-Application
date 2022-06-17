using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace Solution
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string Street { get; set; }
        public string City { get; set; }

        public int ZipCode { get; set; }

      //  public byte[]? Photo { get; set; }

        public string? Photo { get; set; }
        public virtual ICollection<Phone> Phones { get; set; } = new ObservableCollection<Phone>();


    }
}
