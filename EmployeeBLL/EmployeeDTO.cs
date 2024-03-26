using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EmployeeDAL;

namespace EmployeeBLL
{
    public class EmployeeDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpCode { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string EmpName { get; set; }
        public string Email { get; set; }
        public Nullable<int> DeptCode { get; set; }

        public virtual DeptMaster DeptMaster { get; set; }
        public DateTime DueDate { get; set; }
    }
}
