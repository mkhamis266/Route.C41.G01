﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.BLL.Interfaces
{
	public interface IGenericRepository<T> where T : ModelBase 
	{
		IEnumerable<T> GetAll();

		T Get(int id);

		void Delete(T entity);

		void Add(T entity);

		void Update(T entity);
	}
}
