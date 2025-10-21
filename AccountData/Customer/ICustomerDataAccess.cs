using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Interface for Customer data access operations
	/// This abstraction allows us to swap out the data access implementation
	/// (e.g., from Dapper to Entity Framework or another ORM) without changing business logic
	/// </summary>
	public interface ICustomerDataAccess
	{
		// Customer operations
		long InsertCustomer(CustomerDto customer);
		void UpdateCustomer(CustomerDto customer);
		void DeleteCustomer(long customerId);
		CustomerDto GetCustomerById(long customerId);
		List<CustomerDto> GetAllCustomers();

		// Address operations
		long InsertAddress(AddressDto address);
		void UpdateAddress(AddressDto address);
		void DeleteAddress(long addressId);
		AddressDto GetAddressByCustomerId(long customerId);
	}
}
