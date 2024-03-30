using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Data;

namespace Route.C41.G01.BLL
{
	public class UnintOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;

		public IEmployeeRepository EmployeeRepository { get; set ; }
		public IDepartmentRepository DepartmentRepository { get; set; }

		public UnintOfWork(ApplicationDbContext dbContext) // Ask CLR for object from ApplicationDbContext
        {
			_dbContext = dbContext;

			EmployeeRepository = new EmployeeRepository(dbContext);
			DepartmentRepository = new DepartmentRepository(dbContext);
		}

        public int Complete()
		{
			return _dbContext.SaveChanges();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}
	}
}
