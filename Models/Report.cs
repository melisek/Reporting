using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace szakdoga.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Query Query { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreationDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifyDate { get; set; }

        public string Style { get; set; }
    }
}