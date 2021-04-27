using System;

namespace TODO.DTO
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Day { get; set; }
        public bool? Status { get; set; }
        public DateTime? DateStarted { get; set; }
        public string CreatedBy { get; set; }
    }

    public class TaskEditDTO
    {
        public string Name { get; set; }
        public DateTime? Day { get; set; }
        public bool? Status { get; set; }
        public DateTime? DateStarted { get; set; }
        public string CreatedBy { get; set; }
    }
}
