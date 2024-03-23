using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.DAL.Data.Configurations
{
	internal class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			// Fluent APIs for "Employee" Domain
			builder.Property(E => E.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
			builder.Property(E => E.Address).IsRequired();
			builder.Property(E => E.Salary).HasColumnType("decimal(12,2)");

			builder.Property(E => E.Gender)
				.HasConversion
				(
					(Gender) => Gender.ToString(),
					(GenderAsString) => (Gender)Enum.Parse(typeof(Gender), GenderAsString,true)
				);

			builder.Property(E => E.EmployeeType)
				.HasConversion
				(
					(EmployeeType) => EmployeeType.ToString(),
					(EmployeeTypeAsString) => (EmpType)Enum.Parse(typeof(EmpType), EmployeeTypeAsString, true)
				);
		}
	}
}
