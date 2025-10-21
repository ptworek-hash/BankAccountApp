using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Internal implementation of Customer business object
	/// External code cannot directly instantiate this class
	/// Must use CustomerFactory to create instances
	/// </summary>
	internal class Customer : ICustomer
	{
		private CustomerDto _record;
		private Address _address;
		private ICustomerDataAccess _dataAccess;

		/// <summary>
		/// Constructor - accepts a CustomerDto and AddressDto (for loading from database)
		/// or creates new empty ones
		/// </summary>
		public Customer(CustomerDto customerRecord = null, AddressDto addressRecord = null, ICustomerDataAccess dataAccess = null)
		{
			if (customerRecord == null) { customerRecord = new CustomerDto(); }
			if (addressRecord == null) { addressRecord = new AddressDto(); }
			
			_record = customerRecord;
			_address = new Address(addressRecord);
			_dataAccess = dataAccess ?? CustomerDataAccessFactory.Create();
		}

		public long Id
		{
			get { return _record.Id; }
			set { _record.Id = value; }
		}

		public string FirstName
		{
			get { return _record.FirstName; }
			set { _record.FirstName = value; }
		}

		public string LastName
		{
			get { return _record.LastName; }
			set { _record.LastName = value; }
		}

		public string CompanyName
		{
			get { return _record.CompanyName; }
			set { _record.CompanyName = value; }
		}

		public IAddress Address
		{
			get { return _address; }
			set 
			{ 
				// If setting a new Address object, we need to convert it
				if (value != null && value is Address)
				{
					_address = (Address)value;
				}
				else if (value != null)
				{
					// Create a new Address from the interface properties
					_address = new Address(new AddressDto
					{
						Street = value.Street,
						City = value.City,
						State = value.State,
						Zip = value.Zip
					});
				}
			}
		}

		/// <summary>
		/// Validates that the customer meets minimum requirements
		/// Customers must have at least LastName AND CompanyName
		/// </summary>
		public bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(LastName) && !string.IsNullOrWhiteSpace(CompanyName);
		}

		/// <summary>
		/// Saves the customer to the database
		/// Returns true if successful, false otherwise
		/// Validates before saving
		/// </summary>
		public bool Save()
		{
			// Validation: Customers must have at least LastName AND CompanyName
			if (!IsValid())
			{
				throw new InvalidOperationException("Customer must have at least LastName and CompanyName to be saved.");
			}

			try
			{
				// Use data access layer to save
				// This will be implemented in Part 1b
				if (_record.Id == 0)
				{
					// Insert new customer
					_record.Id = _dataAccess.InsertCustomer(_record);
					
					// Set the CustomerId for the address
					_address.CustomerId = _record.Id;
					
					// Insert address if it has data
					if (!string.IsNullOrWhiteSpace(_address.Street) || 
						!string.IsNullOrWhiteSpace(_address.City) ||
						!string.IsNullOrWhiteSpace(_address.State) ||
						!string.IsNullOrWhiteSpace(_address.Zip))
					{
						_address.Id = _dataAccess.InsertAddress(_address.GetDto());
					}
				}
				else
				{
					// Update existing customer
					_dataAccess.UpdateCustomer(_record);
					
					// Update or insert address
					if (_address.Id == 0)
					{
						_address.CustomerId = _record.Id;
						_address.Id = _dataAccess.InsertAddress(_address.GetDto());
					}
					else
					{
						_dataAccess.UpdateAddress(_address.GetDto());
					}
				}

				return true;
			}
			catch (Exception)
			{
				// In a production system, we'd log this error
				return false;
			}
		}

		/// <summary>
		/// Deletes the customer from the database
		/// Returns true if successful, false otherwise
		/// </summary>
		public bool Delete()
		{
			if (_record.Id == 0)
			{
				// Can't delete a customer that hasn't been saved
				return false;
			}

			try
			{
				// Delete address first (foreign key constraint)
				if (_address.Id > 0)
				{
					_dataAccess.DeleteAddress(_address.Id);
				}

				// Delete customer
				_dataAccess.DeleteCustomer(_record.Id);
				
				// Clear the IDs to indicate this object is no longer in the database
				_record.Id = 0;
				_address.Id = 0;

				return true;
			}
			catch (Exception)
			{
				// In a production system, we'd log this error
				return false;
			}
		}

		/// <summary>
		/// Internal method to get the underlying DTO
		/// Used by the repository for database operations
		/// </summary>
		internal CustomerDto GetDto()
		{
			return _record;
		}

		/// <summary>
		/// Internal method to get the underlying Address DTO
		/// Used by the repository for database operations
		/// </summary>
		internal AddressDto GetAddressDto()
		{
			return _address.GetDto();
		}
	}
}
