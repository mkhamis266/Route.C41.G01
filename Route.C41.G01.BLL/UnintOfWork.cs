using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.BLL
{
	public class UnintOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;
		private Hashtable _repositories;

		//public IEmployeeRepository EmployeeRepository { get; set ; }
		//public IDepartmentRepository DepartmentRepository { get; set; }

		public UnintOfWork(ApplicationDbContext dbContext) // Ask CLR for object from ApplicationDbContext
        {
			_dbContext = dbContext;
			_repositories = new Hashtable();
			//EmployeeRepository = new EmployeeRepository(dbContext);
			//DepartmentRepository = new DepartmentRepository(dbContext);
		}

        public int Complete()
		{
			return _dbContext.SaveChanges();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}

		public IGenericRepository<T> Repository<T>() where T : ModelBase
		{
			var key = typeof(T).Name;
			if (!_repositories.ContainsKey(key))
			{
				if(key == nameof(Employee))
				{
					var repository = new EmployeeRepository(_dbContext);
					_repositories.Add(key, repository);
				}
				else
				{
					var repository = new GenericRepository<T>(_dbContext);
					_repositories.Add(key, repository);
				}
			}
			return _repositories[key] as IGenericRepository<T>;
		}
	}
}
