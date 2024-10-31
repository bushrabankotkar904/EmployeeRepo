using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmpMVC.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Department Name")]
        public string DeptName { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
