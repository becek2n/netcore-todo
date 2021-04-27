using System;
using System.Collections.Generic;

#nullable disable

namespace TODO.Repository.Models
{
    public partial class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Day { get; set; }
        public bool? Status { get; set; }
        public DateTime? DateStarted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}
