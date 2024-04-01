using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.BLL.Interfaces
{
	public interface IGenericRepository<T> where T : ModelBase 
	{
		Task<IEnumerable<T>> GetAllAsync();

		Task<T> GetAsync(int id);

		void Delete(T entity);

		void Add(T entity);

		void Update(T entity);
	}
}
