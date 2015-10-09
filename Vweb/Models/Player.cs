using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vweb.Models
{
    [Table("vweb_player")]
    public class Player
    {
        [Key]
        public int ID { get; set; }
        public int TeamID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        
    }


    public class DisplayPlayer
    {
        [Key]
        public int ID { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }

    }
}