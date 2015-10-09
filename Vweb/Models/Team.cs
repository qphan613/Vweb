using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vweb.Models
{
    public class Team
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class DisplayTeam
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Player> Players { get; set; }
        public virtual IEnumerable<TeamSchedule> Schedules { get; set; }
        public string ShowMonth { get; set; }
        public static IEnumerable<SelectMonth> Months
        {
            get
            {
                return DateTimeFormatInfo
                       .CurrentInfo
                       .MonthNames
                       .Select((monthName, index) => new SelectMonth
                       {
                           Value = (index + 1).ToString(),
                           Text = monthName
                       });
            }
        }
    }

    public class SelectMonth
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}