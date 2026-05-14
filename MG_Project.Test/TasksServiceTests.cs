using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MG_Project.Test
{
    public class TasksServiceTests : IClassFixture<InMemoryServicesFixture>
    {
        private readonly InMemoryServicesFixture _fixture;

        public TasksServiceTests(InMemoryServicesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddTask_ShouldCreateTask()
        {
            var taskId = _fixture.TasksService.AddTask(
                Guid.NewGuid(),
                "Write unit test",
                "Write a test for EmployeeService",
                DateTime.Now.AddDays(5),
                false
            );

            var task = _fixture.TasksService.get(taskId);
            Assert.NotNull(task);
            Assert.Equal("Write unit test", task.Title);
        }

        [Fact]
        public void MarkAsDone_ShouldUpdateTask()
        {
            var taskId = _fixture.TasksService.AddTask(
                Guid.NewGuid(),
                "Temporary task",
                "Complete quickly",
                DateTime.Now.AddDays(1),
                false
            );

            var tasks = _fixture.TasksService.MarkAsDone();
            var task = _fixture.TasksService.get(taskId);
            Assert.True(task?.IsDone);
        }
    }
}
