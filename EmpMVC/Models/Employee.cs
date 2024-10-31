using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpMVC.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        
        public Department? Department { get; set; }
        public int DepartmentId { get; set; }
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        public DateTime? CreatedOn { get; set; }
        
    }
}
