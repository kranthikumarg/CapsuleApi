using System;

namespace WebAPI_TaskManager
{
    public class TaskSearch
    {
        public string Tasks { get; set; }
        public string ParentTasks { get; set; }
        public string PriorityFrom { get; set; }
        public string PriorityTo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}