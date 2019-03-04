using System;
using System.Collections.Generic;
using NBench;
using WebAPI_TaskManager.Controllers;
using System.Web.Http;
using System.Web.Http.Results;


namespace WebAPI_TaskManager.Tests.Controllers
{
    public class PerfTestTaskManagerController
    {
        private const int AcceptableMinAddThroughput = 50;

        private static TaskManagerController controllerAPI = new TaskManagerController()
        {
            Request = new System.Net.Http.HttpRequestMessage()
        };
        List<TaskAdd> tasks = new List<TaskAdd>();

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            for (var cnt = 0; cnt < 100; cnt++)
            {
                tasks.Add(new TaskAdd()
                {
                    Tasks = "Performance Test",
                    ParentTasks = "Test1",
                    Priority = "10",
                    StartDate = DateTime.Today.ToString("dd-MM-yyyy")
                });
            }
        }

        [PerfBenchmark(NumberOfIterations = 5, RunMode = RunMode.Throughput, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 10000, MinTimeMilliseconds = 1000)]

        public void AddTask_Throughput_IterationMode(BenchmarkContext context)
        {
            for (var i = 0; i < tasks.Count; i++)
            {
                IHttpActionResult result = controllerAPI.AddTask(tasks[i]);
            }
        }

        [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Throughput, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 10000, MinTimeMilliseconds = 1000)]

        public void GetTask_Throughput_IterationMode(BenchmarkContext context)
        {
            for (var i = 0; i < AcceptableMinAddThroughput; i++)
            {
                IEnumerable<TaskAndParent> result = controllerAPI.Get();
            }
        }

        [PerfCleanup]
        public void Cleanup(BenchmarkContext context)
        {

        }
    }
}
