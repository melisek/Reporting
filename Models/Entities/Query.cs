using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace szakdoga.Models
{
    public class Query : BaseEntity, IComparable
    {
        public string SQL { get; set; }

        [MaxLength(200)]
        public string ResultTableName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string QueryGUID { get; set; }

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

        public int CompareTo(object obj)
        {
            Query query = obj as Query;
            if (query != null)
            {
                return this.Name.CompareTo(query.Name);
            }
            return 0;
        }
    }
}