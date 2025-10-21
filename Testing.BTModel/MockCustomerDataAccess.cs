using System;
using System.Collections.Generic;
using System.Linq;
using BT.Model.AccountData;

namespace Testing.BTModel
{
	/// <summary>
	/// Mock implementation of ICustomerDataAccess for unit testing
	/// Stores data in memory instead of a real database
	/// Allows isolated testing without database dependencies
	/// </summary>
	public class MockCustomerDataAccess : ICustomerDataAccess
	{
		// In-memory storage
		public List<CustomerDto> Customers { get; private set; }
		public List<AddressDto> Addresses { get; private set; }

		private long _nextCustomerId;
		private long _nextAddressId;

		public MockCustomerDataAccess()
		{
			Customers = new List<CustomerDto>();
			Addresses = new List<AddressDto>();
			_nextCustomerId = 1;
			_nextAddressId = 1;
		}

		#region Helper Methods for Testing

		/// <summary>
		/// Helper method to add test data
		/// </summary>
		public void AddTestCustomer(long id, string firstName, string lastName, string companyName,
			string street = null, string city = null, string state = null, string zip = null)
		{
			var customer = new CustomerDto
			{
				Id = id,
				FirstName = firstName,
				LastName = lastName,
				CompanyName = companyName
			};
			Customers.Add(customer);

			if (_nextCustomerId <= id)
			{
				_nextCustomerId = id + 1;
			}

			// Add address if provided
			if (!string.IsNullOrEmpty(street) || !string.IsNullOrEmpty(city) || 
				!string.IsNullOrEmpty(state) || !string.IsNullOrEmpty(zip))
			{
				var address = new AddressDto
				{
					Id = _nextAddressId++,
					CustomerId = id,
					Street = street,
					City = city,
					State = state,
					Zip = zip
				};
				Addresses.Add(address);
			}
		}

		#endregion

		#region ICustomerDataAccess Implementation

		public long InsertCustomer(CustomerDto customer)
		{
			customer.Id = _nextCustomerId++;
			Customers.Add(customer);
			return customer.Id;
		}

		public void UpdateCustomer(CustomerDto customer)
		{
			var existing = Customers.FirstOrDefault(c => c.Id == customer.Id);
			if (existing != null)
			{
				existing.FirstName = customer.FirstName;
				existing.LastName = customer.LastName;
				existing.CompanyName = customer.CompanyName;
			}
		}

		public void DeleteCustomer(long customerId)
		{
			var customer = Customers.FirstOrDefault(c => c.Id == customerId);
			if (customer != null)
			{
				Customers.Remove(customer);
			}
		}

		public CustomerDto GetCustomerById(long customerId)
		{
			return Customers.FirstOrDefault(c => c.Id == customerId);
		}

		public List<CustomerDto> GetAllCustomers()
		{
			return new List<CustomerDto>(Customers);
		}

		public long InsertAddress(AddressDto address)
		{
			address.Id = _nextAddressId++;
			Addresses.Add(address);
			return address.Id;
		}

		public void UpdateAddress(AddressDto address)
		{
			var existing = Addresses.FirstOrDefault(a => a.Id == address.Id);
			if (existing != null)
			{
				existing.CustomerId = address.CustomerId;
				existing.Street = address.Street;
				existing.City = address.City;
				existing.State = address.State;
				existing.Zip = address.Zip;
			}
		}

		public void DeleteAddress(long addressId)
		{
			var address = Addresses.FirstOrDefault(a => a.Id == addressId);
			if (address != null)
			{
				Addresses.Remove(address);
			}
		}

		public AddressDto GetAddressByCustomerId(long customerId)
		{
			return Addresses.FirstOrDefault(a => a.CustomerId == customerId);
		}

		#endregion
	}
}
