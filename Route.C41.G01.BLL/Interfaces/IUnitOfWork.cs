using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.BLL.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IEmployeeRepository EmployeeRepository { get; set; }
		IDepartmentRepository DepartmentRepository { get; set; }

		int Complete();
	}
}
