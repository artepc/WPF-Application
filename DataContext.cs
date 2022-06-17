using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            // Create database if not already existing
            Database.EnsureCreated();
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Phone> Phones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(GetPerson());
            modelBuilder.Entity<Phone>().HasData(GetPhone());
            base.OnModelCreating(modelBuilder);
        }

        private Person[] GetPerson()
        {
            return new Person[]
            {
            new Person { PersonId = 1, LastName = "Blue", FirstName = "Lars", Street = "Julianstr. 1", City = "Prag", ZipCode = 10000, Photo = ""},
            new Person { PersonId = 2, LastName = "Mustermann", FirstName = "Bob", Street = "Bachstr. 2", City = "Siegen", ZipCode = 12345, Photo = "" }
             };
        }

        private Phone[] GetPhone()
        {
            return new Phone[]
            {
                   new Phone { NumberId = 1, PhoneNumber = 12345689, PersonId = 1},
                   new Phone { NumberId = 2, PhoneNumber = 56789000, PersonId = 1},
                   new Phone { NumberId = 3, PhoneNumber = 3456711, PersonId = 2}
             };
        }
    }
}
