using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_TaskManager
{
    public class TaskEdit:TaskAdd
    {
        public string TaskId { get; set; }
    }
}