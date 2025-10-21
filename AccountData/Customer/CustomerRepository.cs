using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Internal implementation of the Customer Repository
	/// This class handles all customer data operations and object creation
	/// External code cannot access this directly - must use CustomerFactory
	/// </summary>
	internal class CustomerRepository : ICustomerRepository
	{
		private readonly ICustomerDataAccess _dataAccess;

		/// <summary>
		/// Constructor - accepts data access layer (for dependency injection/testing)
		/// </summary>
		public CustomerRepository(ICustomerDataAccess dataAccess = null)
		{
			_dataAccess = dataAccess ?? CustomerDataAccessFactory.Create();
		}

		/// <summary>
		/// Returns a new, empty customer object ready to be populated
		/// The customer will have all properties initialized to default values
		/// </summary>
		public ICustomer GetNewCustomer()
		{
			// Create a new customer with empty DTOs
			return new Customer(
				customerRecord: new CustomerDto(),
				addressRecord: new AddressDto(),
				dataAccess: _dataAccess
			);
		}

		/// <summary>
		/// Returns a customer by ID
		/// Returns null if the customer is not found
		/// </summary>
		public ICustomer GetCustomerById(long customerId)
		{
			// Get customer data from database
			var customerDto = _dataAccess.GetCustomerById(customerId);
			
			if (customerDto == null)
			{
				return null;
			}

			// Get associated address data
			var addressDto = _dataAccess.GetAddressByCustomerId(customerId);
			
			// Create and return customer object
			return new Customer(
				customerRecord: customerDto,
				addressRecord: addressDto ?? new AddressDto(),
				dataAccess: _dataAccess
			);
		}

		/// <summary>
		/// Returns a list of all customers in the system
		/// Loads each customer with their associated address
		/// Returns empty list if no customers found
		/// </summary>
		public List<ICustomer> GetList()
		{
			var customers = new List<ICustomer>();

			// Get all customer records
			var customerDtos = _dataAccess.GetAllCustomers();

			// Load each customer with their address
			foreach (var customerDto in customerDtos)
			{
				var addressDto = _dataAccess.GetAddressByCustomerId(customerDto.Id);
				
				var customer = new Customer(
					customerRecord: customerDto,
					addressRecord: addressDto ?? new AddressDto(),
					dataAccess: _dataAccess
				);

				customers.Add(customer);
			}

			return customers;
		}
	}
}
