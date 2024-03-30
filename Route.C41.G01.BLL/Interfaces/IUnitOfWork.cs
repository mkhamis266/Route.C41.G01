using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.BLL.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		//IEmployeeRepository EmployeeRepository { get; set; }
		//IDepartmentRepository DepartmentRepository { get; set; }

		IGenericRepository<T> Repository<T>() where T : ModelBase;

		int Complete();
	}
}
