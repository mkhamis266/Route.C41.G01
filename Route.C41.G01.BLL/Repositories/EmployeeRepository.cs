using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.BLL.Repositories
{
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(ApplicationDbContext dbContext):base(dbContext)
		{
			
		}

		public IQueryable<Employee> GetEmployeesByAddress(string address)
		{
			//return _dbContext.Employees.Where(E=> E.Address.ToLower() == address.ToLower());\

			// for better performance
			return _dbContext.Employees.Where(E=> string.Equals(E.Address, address, StringComparison.OrdinalIgnoreCase));
		}
	}
}
