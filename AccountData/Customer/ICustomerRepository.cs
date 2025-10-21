using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Interface for Customer Repository operations
	/// This defines the contract for accessing and creating customers
	/// Exposed publicly so it can be mocked for testing
	/// </summary>
	public interface ICustomerRepository
	{
		/// <summary>
		/// Returns a new, empty customer object ready to be populated
		/// </summary>
		ICustomer GetNewCustomer();

		/// <summary>
		/// Returns a customer by ID
		/// Returns null if not found
		/// </summary>
		ICustomer GetCustomerById(long customerId);

		/// <summary>
		/// Returns a list of all customers in the system
		/// Returns empty list if no customers found
		/// </summary>
		List<ICustomer> GetList();
	}
}
