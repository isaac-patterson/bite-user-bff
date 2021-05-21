using System;

namespace veni_bff.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Status { get; set; }
        public string Task { get; set; }
    }
}