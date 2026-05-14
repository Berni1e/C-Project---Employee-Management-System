using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using MG_Project.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace MG_Project.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepositories _repo;
        private readonly IUnitOfWork _uow;

        public TasksService(ITasksRepositories repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public Guid AddTask(Guid taskId, string title, string desc, DateTime dueDate, bool isDone)
        {
            var task = new Tasks
            {
                TasksId = taskId,
                Title = title,
                Desc = desc,
                DueDate = dueDate,
                IsDone = isDone
            };

            _repo.Add(task);
            _uow.SaveChanges();
            return task.TasksId;
        }

        public Tasks? get(Guid id)
        {
            return _repo.Get(id);
        }

        public IReadOnlyList<Tasks> MarkAsDone()
        {
            var tasks = _repo.Query()
                .Where(t => !t.IsDone)
                .ToList();

            foreach (var t in tasks)
            {
                t.IsDone = true;
            }

            _uow.SaveChanges();
            return new ReadOnlyCollection<Tasks>(tasks);
        }

        public IReadOnlyList<Tasks> ShowTask()
        {
            return new ReadOnlyCollection<Tasks>(_repo.Query().ToList());
        }
    }
}
