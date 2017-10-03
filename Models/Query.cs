using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace szakdoga.Models
{
    public class Query
    {
        [Key]
        public int Id { get; set; }
        public string SQL { get; set; }

        public int ResultTableName { get; set; }

        public string TranslatedColumnNames { get; set; }

        public DateTime NextUpdating { get; set; }

        [NotMapped]
        public TimeSpan UpdatePeriod { get; set; }

        /// <summary>
        /// TimeSpan SQL-ben Time 0-24 órában korlátozott
        /// SQL-ben így tudjuk lekérdezni: SELECT CONVERT(VARCHAR, DATEPART(DAY,DATEADD(ms, TimeToCompleteFormTicks/10000, 0))) + '.' + CONVERT(VARCHAR, DATEADD(ms, TimeToCompleteFormTicks/10000, 0), 114)
        /// </summary>
        public long UpdatePeriodTicks
        {
            get
            {
                return UpdatePeriod.Ticks;
            }
            set
            {
                UpdatePeriod = TimeSpan.FromTicks(value);
            }
        }
    }
}