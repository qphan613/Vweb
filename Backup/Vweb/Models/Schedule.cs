using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vweb.Models
{
    [Table("vweb_schedule")]
    public class Schedule
    {
        [Key]
        public int ID { get; set; }
        public int HomeTeam { get; set; }
        public int RoadTeam { get; set; }
        public int RoadScore { get; set; }
        public int HomeScore { get; set; }
        public int Location { get; set; }
        [DataType(DataType.Date)]
        public DateTime GameDateTime { get; set; }
    }

    public class TeamSchedule
    {
        
        public int RoadScore { get; set; }
        public int HomeScore { get; set; }
        public int Location { get; set; }
        public DateTime GameDateTime { get; set; }
        public bool ForScoreSubmit { get; set; }
        public virtual Team Road_Team { get; set; }
        public virtual Team Home_Team { get; set; }
    }

    public class SubmitScore
    {
        public int RoadScore { get; set; }
        public int HomeScore { get; set; }
    }



}