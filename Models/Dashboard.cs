using System;

namespace szakdoga.Models
{
    public class Dashboard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Style { get; set; }
    }
}