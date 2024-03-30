using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.BLL.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
	{
		private protected readonly ApplicationDbContext _dbContext;

		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}


		public void Add(T entity)
			=> _dbContext.Set<T>().Add(entity);

		public void Delete(T entity)
			=> _dbContext.Set<T>().Remove(entity);



		public T Get(int id)
		{
			return _dbContext.Find<T>(id);
		}

		public IEnumerable<T> GetAll()
		{
			if (typeof(T) == typeof(Employee))
				return (IEnumerable<T>)_dbContext.Set<Employee>().Include(E => E.Department).AsNoTracking().ToList();
			else
				return _dbContext.Set<T>().AsNoTracking().ToList();
		}


		public void Update(T entity)
			=> _dbContext.Set<T>().Update(entity);

	}
}
