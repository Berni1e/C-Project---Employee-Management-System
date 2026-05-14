using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataModel
{
    using System;

    public class Tasks
    {
        public Guid TasksId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsDone { get; set; }

        public Tasks()
        {
            TasksId = Guid.NewGuid();
            Title = string.Empty;
            Desc = string.Empty;
            DueDate = DateTime.MinValue;
            IsDone = false;
        }

        public Tasks(string title, string desc, DateTime dueDate)
        {
            TasksId = Guid.NewGuid();
            Title = title;
            Desc = desc;
            DueDate = dueDate;
            IsDone = false; // domyślnie zadanie nie jest wykonane
        }

        public void MarkAsDone()
        {
            IsDone = true;
        }

        public void ShowTask()
        {
            Console.WriteLine($"--- Task Info ---");
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Description: {Desc}");
            Console.WriteLine($"Due Date: {DueDate:yyyy-MM-dd}");
            Console.WriteLine($"Status: {(IsDone ? "Done ✅" : "Pending ⏳")}");
            Console.WriteLine();
        }
    }

}
