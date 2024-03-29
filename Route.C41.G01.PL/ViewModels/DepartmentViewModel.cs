using Route.C41.G01.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Route.C41.G01.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }

        //[InverseProperty(nameof(Models.Employee.Department))]
        // Navigational Property => [Many]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
