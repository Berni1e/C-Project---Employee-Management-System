using System;
using System.Collections.Generic;
using MG_Project.DataModel;

namespace MG_Project.ServiceAbstractions
{
    public interface ITasksService
    {
        Guid AddTask(Guid taskId, string title, string desc, DateTime dueDate, bool isDone);
        Tasks? get(Guid id);
        IReadOnlyList<Tasks> MarkAsDone();
        IReadOnlyList<Tasks> ShowTask();
    }
}
