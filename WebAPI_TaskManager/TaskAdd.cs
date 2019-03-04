using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_TaskManager
{
    public class TaskAdd
    {
        public string Tasks { get; set; }
        public string ParentTasks { get; set; }
        public string Priority { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}