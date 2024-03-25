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


        public int Add(T entity)
		{
			_dbContext.Set<T>().Add(entity);
			return _dbContext.SaveChanges();
		}

		public int Delete(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			return _dbContext.SaveChanges();
		}

		

		public T Get(int id)
		{
			return _dbContext.Find<T>(id);
		}

		public IEnumerable<T> GetAll()
			=> _dbContext.Set<T>().AsNoTracking().ToList();


		public int Update(T entity)
		{
			_dbContext.Set<T>().Update(entity);
			return _dbContext.SaveChanges();
		}
	}
}
