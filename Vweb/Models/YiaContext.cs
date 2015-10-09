using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vweb.Models
{
    public class YiaContext : DbContext
    {
        public YiaContext()
            : base("YiaContext")
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }

    public class Standing
    {
        public string Name { get; set; }
        public virtual Record TeamRecord { get; set; }
    }

    [Table("vweb_record")]
    public class Record
    {
        public int Win { get; set; }
        public int Lost { get; set; }
        [Key]
        public int TeamID { get; set; }
    }

    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
    }
    


}