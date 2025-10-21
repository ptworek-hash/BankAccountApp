using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Public factory for creating and accessing customers
	/// This is the ONLY way external code should interact with the customer system
	/// 
	/// Usage Example:
	///   var repo = CustomerFactory.Create();
	///   var customer = repo.GetNewCustomer();
	///   customer.FirstName = "John";
	///   customer.LastName = "Doe";
	///   customer.CompanyName = "Acme Corp";
	///   customer.Save();
	/// </summary>
	public static class CustomerFactory
	{
		/// <summary>
		/// Creates and returns a customer repository instance
		/// The repository provides access to all customer operations
		/// </summary>
		/// <returns>ICustomerRepository instance for accessing customers</returns>
		public static ICustomerRepository Create()
		{
			return new CustomerRepository();
		}

		/// <summary>
		/// Creates a customer repository with a custom data access layer
		/// This overload is primarily used for unit testing with mock data access
		/// </summary>
		/// <param name="dataAccess">Custom data access implementation</param>
		/// <returns>ICustomerRepository instance with custom data access</returns>
		public static ICustomerRepository Create(ICustomerDataAccess dataAccess)
		{
			return new CustomerRepository(dataAccess);
		}
	}
}
