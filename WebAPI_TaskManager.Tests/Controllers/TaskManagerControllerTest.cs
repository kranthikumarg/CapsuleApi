using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAPI_TaskManager.Controllers;
using System.Linq;
using System.Text;
using System.Web.Http;
using WebAPI_TaskManager.Models;

namespace WebAPI_TaskManager.Tests.Controllers
{
    [TestClass]
    public class TaskManagerControllerTest
    {
        public List<Models.Task> Get()
        {
            using (TaskManagerEntities taskEntities = new TaskManagerEntities())
            {
                return taskEntities.Tasks.ToList();
            }
        }

        [TestMethod]
        public void GetAllTasks_API()
        {
            var controllerAPI = new TaskManagerController();
            IEnumerable<TaskAndParent> actionResult = controllerAPI.Get();
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetTaskById_API()
        {
            string taskID = "1";

            var controllerAPI = new TaskManagerController();
            IEnumerable<TaskAndParent> actionResult = controllerAPI.Get(taskID);
            
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetTaskSearch_Tasks_API()
        {
            TaskSearch task = new TaskSearch();
            task.Tasks = "Test task0";
            var controllerAPI = new TaskManagerController();
            IEnumerable<TaskAndParent> actionResult = controllerAPI.GetSearch(task);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetTaskSearch_ParentTasks_API()
        {
            TaskSearch task = new TaskSearch();
            task.ParentTasks = "Test1";
            var controllerAPI = new TaskManagerController();
            IEnumerable<TaskAndParent> actionResult = controllerAPI.GetSearch(task);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetTaskSearch_Priority_API()
        {
            TaskSearch task = new TaskSearch();
            task.PriorityFrom = "10";
            task.PriorityTo = "20";
            var controllerAPI = new TaskManagerController();
            IEnumerable<TaskAndParent> actionResult = controllerAPI.GetSearch(task);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetTaskSearch_StartDate_API()
        {
            TaskSearch task = new TaskSearch();
            task.StartDate = "31-01-2019";
            var controllerAPI = new TaskManagerController();
            IEnumerable<TaskAndParent> actionResult = controllerAPI.GetSearch(task);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetTaskSearch_EndDate_API()
        {
            TaskSearch task = new TaskSearch();
            task.EndDate = "05-02-2019";
            var controllerAPI = new TaskManagerController();
            IEnumerable<TaskAndParent> actionResult = controllerAPI.GetSearch(task);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetTaskSearch_All_API()
        {
            TaskSearch task = new TaskSearch();
            task.ParentTasks = "Test2";
            task.PriorityFrom = "2";
            task.PriorityTo = "30";
            task.StartDate = "01-02-2019";
            var controllerAPI = new TaskManagerController();
            IEnumerable<TaskAndParent> actionResult = controllerAPI.GetSearch(task);
            Assert.IsNotNull(actionResult);
        }


        [TestMethod]
        public void AddTask_API()
        {
            TaskAdd task = new TaskAdd();
            task.Tasks = "Test Test123";
            task.ParentTasks = "Test1";
            task.Priority = "12";
            task.StartDate = "02-02-2019";

            TaskManagerEntities taskEnt = new TaskManagerEntities();
            var taskID = taskEnt.Tasks.Max(t => t.Task_ID) + 1;

            var controllerAPI = new TaskManagerController();
            IHttpActionResult actionResult = controllerAPI.AddTask(task);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Models.Task>;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(taskID, createdResult.RouteValues["taskID"]);
        }

        [TestMethod]
        public void AddTask2_API()
        {
            TaskAdd task = new TaskAdd();
            task.Tasks = "Test Test124";
            task.ParentTasks = "Test Test123";
            task.Priority = "12";
            task.StartDate = "04-02-2019";

            TaskManagerEntities taskEnt = new TaskManagerEntities();
            var taskID = taskEnt.Tasks.Max(t => t.Task_ID) + 1;            

            var controllerAPI = new TaskManagerController();
            IHttpActionResult actionResult = controllerAPI.AddTask(task);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Models.Task>;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(taskID, createdResult.RouteValues["taskID"]);
        }

        [TestMethod]
        public void AddTask3_API()
        {
            TaskAdd task = new TaskAdd();
            task.Tasks = "Test Test125";
            task.ParentTasks = "Test2";
            task.Priority = "12";
            task.StartDate = "04-02-2019";

            TaskManagerEntities taskEnt = new TaskManagerEntities();
            var taskID = taskEnt.Tasks.Max(t => t.Task_ID) + 1;

            var controllerAPI = new TaskManagerController();
            IHttpActionResult actionResult = controllerAPI.AddTask(task);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Models.Task>;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(taskID, createdResult.RouteValues["taskID"]);
        }

        [TestMethod]
        public void EditTask_API()
        {
            TaskEdit task = new TaskEdit();
            task.TaskId = "6";
            task.Tasks = "Test Test123";
            task.ParentTasks = "Test2";
            task.Priority = "16";
            task.StartDate = "03-02-2019";
            
            var controllerAPI = new TaskManagerController();
            IHttpActionResult actionResult = controllerAPI.EditTask(task);
            var contentResult = actionResult as OkNegotiatedContentResult<Models.Task>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);            
        }

        [TestMethod]
        public void EditTask2_API()
        {
            TaskEdit task = new TaskEdit();
            task.TaskId = "6";
            task.Tasks = "Test Test123";
            task.ParentTasks = "Test2";
            task.Priority = "16";
            task.StartDate = "03-02-2019";
            task.EndDate = "04-02-2019";

            var controllerAPI = new TaskManagerController();
            IHttpActionResult actionResult = controllerAPI.EditTask(task);
            var contentResult = actionResult as OkNegotiatedContentResult<Models.Task>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }


        [TestMethod]
        public void EndTask_API()
        {
            string taskID = "3";

            var controllerAPI = new TaskManagerController();
            IHttpActionResult actionResult = controllerAPI.EndTask(taskID);
            var contentResult = actionResult as OkNegotiatedContentResult<Models.Task>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(int.Parse(taskID), contentResult.Content.Task_ID);
        }
    }
}
