using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.BLL.Interfaces
{
    internal interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();

        Department Get(int id);

        int Delete(Department entity);

        int Add(Department entity);

        int Update(Department entity);
    }
}
