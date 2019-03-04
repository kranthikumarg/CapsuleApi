using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI_TaskManager.Models;

namespace WebAPI_TaskManager.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TaskManagerController : ApiController
    {
        public IEnumerable<TaskAndParent> Get()
        {
            using (TaskManagerEntities taskEntities = new TaskManagerEntities())
            {
               return (from t in taskEntities.Tasks
                             join pt in taskEntities.Parent_Task
                             on t.Parent_ID equals pt.Parent_ID
                             select new TaskAndParent()
                             {
                                 Task_ID = t.Task_ID,
                                 Parent_ID = t.Parent_ID,
                                 Tasks = t.Tasks,
                                 Parent_Tasks = pt.Parent_Tasks,
                                 Start_Date = t.Start_Date,
                                 End_Date = t.End_Date,
                                 Priority = t.Priority
                             }).ToList();
            }
        }

        public IEnumerable<TaskAndParent> Get(string taskID)
        {
            int task_id = int.Parse(taskID);
            using (TaskManagerEntities taskEntities = new TaskManagerEntities())
            {
                return (from t in taskEntities.Tasks
                       join pt in taskEntities.Parent_Task
                       on t.Parent_ID equals pt.Parent_ID
                       where t.Task_ID == task_id
                        select new TaskAndParent()
                       {
                           Task_ID = t.Task_ID,
                           Parent_ID = t.Parent_ID,
                           Tasks = t.Tasks,
                           Parent_Tasks = pt.Parent_Tasks,
                           Start_Date = t.Start_Date,
                           End_Date = t.End_Date,
                           Priority = t.Priority
                       }).ToList();
            }
        }

        [HttpPost]
        public IEnumerable<TaskAndParent> GetSearch([FromBody] TaskSearch taskSearch)
        {
            DateTime StartDate, EndDate;

            using (TaskManagerEntities taskEntities = new TaskManagerEntities())
            {
                var taskList = (from t in taskEntities.Tasks
                                join pt in taskEntities.Parent_Task
                                on t.Parent_ID equals pt.Parent_ID
                                select new TaskAndParent()
                                {
                                    Task_ID = t.Task_ID,
                                    Parent_ID = t.Parent_ID,
                                    Tasks = t.Tasks,
                                    Parent_Tasks = pt.Parent_Tasks,
                                    Start_Date = t.Start_Date,
                                    End_Date = t.End_Date,
                                    Priority = t.Priority
                                });

                if (!string.IsNullOrEmpty(taskSearch.Tasks))
                {
                    taskList = taskList.Where(r => r.Tasks == taskSearch.Tasks);
                }

                if (!string.IsNullOrEmpty(taskSearch.ParentTasks))
                {
                    taskList = taskList.Where(r => r.Parent_Tasks == taskSearch.ParentTasks);
                }

                if (!string.IsNullOrEmpty(taskSearch.PriorityFrom) && !string.IsNullOrEmpty(taskSearch.PriorityTo))
                {
                    var priorFrom = int.Parse(taskSearch.PriorityFrom);
                    var priorTo = int.Parse(taskSearch.PriorityTo);

                    taskList = taskList.Where(r => r.Priority >= priorFrom && r.Priority <= priorTo);
                }

                if (DateTime.TryParse(taskSearch.StartDate, out StartDate))
                {
                    taskList = taskList.Where(r => r.Start_Date == StartDate);
                }

                if (DateTime.TryParse(taskSearch.EndDate, out EndDate))
                {
                    taskList = taskList.Where(r => r.End_Date == EndDate);
                }

                return taskList.ToList();
            }
        }

        [HttpPost]
        public IHttpActionResult AddTask([FromBody] TaskAdd taskAdd)
        {
            using (TaskManagerEntities taskEntities = new TaskManagerEntities())
            {
                var parentId = string.Empty;
                var taskId = taskEntities.Tasks.Max(t => t.Task_ID) +1;                

                var parId = from pt in taskEntities.Parent_Task
                             where pt.Parent_Tasks == taskAdd.ParentTasks
                             select pt.Parent_ID;

                if (parId.ToList().Count() == 0)
                {
                    parentId = (from t in taskEntities.Tasks
                               where t.Tasks == taskAdd.ParentTasks
                                select t.Parent_ID).ToList()[0];
                }
                else{
                    parentId = parId.ToList()[0];
                }

                Task task = new Task();
                task.Task_ID = taskId;
                task.Tasks = taskAdd.Tasks;
                task.Parent_ID = parentId;
                task.Priority = int.Parse(taskAdd.Priority);
                task.Start_Date = Convert.ToDateTime(taskAdd.StartDate);
                if (!string.IsNullOrEmpty(taskAdd.EndDate))
                {
                    task.End_Date = Convert.ToDateTime(taskAdd.EndDate);
                }

                taskEntities.Tasks.Add(task);
                try
                {
                    taskEntities.SaveChanges();
                }
                catch
                {
                    throw;
                }
                return CreatedAtRoute("DefaultApi", new { taskID = task.Task_ID }, task);
            }
        }

        [HttpPut]
        public IHttpActionResult EditTask(TaskEdit taskEdit)
        {
            using (TaskManagerEntities taskEntities = new TaskManagerEntities())
            {
                var parentId = string.Empty;
                var parId = from pt in taskEntities.Parent_Task
                            where pt.Parent_Tasks == taskEdit.ParentTasks
                            select pt.Parent_ID;

                if (parId.ToList().Count() == 0)
                {
                    parentId = (from t in taskEntities.Tasks
                                where t.Tasks == taskEdit.ParentTasks
                                select t.Parent_ID).ToList()[0];
                }
                else
                {
                    parentId = parId.ToList()[0];
                }

                Task task = new Task();
                task.Task_ID = int.Parse(taskEdit.TaskId);
                task.Tasks = taskEdit.Tasks;
                task.Parent_ID = parentId;
                task.Priority = int.Parse(taskEdit.Priority);
                task.Start_Date = Convert.ToDateTime(taskEdit.StartDate);
                if (!string.IsNullOrEmpty(taskEdit.EndDate))
                {
                    task.End_Date = Convert.ToDateTime(taskEdit.EndDate);
                }

                taskEntities.Entry(task).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    taskEntities.SaveChanges();
                }
                catch
                {
                    throw;
                }
                return Ok(task);
            }
        }

        [HttpPost]
        public IHttpActionResult EndTask(String taskID)
        {
            using (TaskManagerEntities taskEntities = new TaskManagerEntities())
            {
                Task task = taskEntities.Tasks.Find(int.Parse(taskID));
                task.End_Date = Convert.ToDateTime(DateTime.Today);

                taskEntities.Entry(task).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    taskEntities.SaveChanges();
                }
                catch
                {
                    throw;
                }
                return Ok(task);
            }
        }
    }
}
