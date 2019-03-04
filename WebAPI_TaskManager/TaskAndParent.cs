using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_TaskManager
{
    public class TaskAndParent
    {
        public int Task_ID { get; set; }
        public string Parent_ID { get; set; }
        public string Tasks { get; set; }
        public string Parent_Tasks { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public int? Priority { get; set; }
    }
}