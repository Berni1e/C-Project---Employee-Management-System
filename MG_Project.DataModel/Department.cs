using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Department
    {
        public Guid DepartmentId { get; set; }
        public string DeptName { get; set; }

        public Guid FirmId { get; set; }
        public virtual Firm Firm { get; set; }

        public virtual ICollection<Employee> EmpList { get; set; }

        public Department()
        {
            DepartmentId = Guid.NewGuid();
            DeptName = string.Empty;
            EmpList = new List<Employee>();
        }

        public Department(string deptName)
        {
            DepartmentId = Guid.NewGuid();
            DeptName = deptName;
            EmpList = new List<Employee>();
        }

        public void AddEmp(Employee emp)
        {
            if (emp != null && !EmpList.Contains(emp))
            {
                EmpList.Add(emp);
                emp.Department = this;
            }
        }

        public void DelEmp(Employee emp)
        {
            if (emp != null && EmpList.Contains(emp))
            {
                EmpList.Remove(emp);
                if (emp.Department == this)
                    emp.Department = null;
            }
        }
    }

}
