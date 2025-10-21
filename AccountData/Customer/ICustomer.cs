using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Interface for read-only customer record properties
	/// </summary>
	public interface ICustomerRecord
	{
		long Id { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string CompanyName { get; set; }
		IAddress Address { get; set; }
	}

	/// <summary>
	/// Public interface for Customer business object
	/// This is what external consumers will interact with
	/// </summary>
	public interface ICustomer : ICustomerRecord
	{
		/// <summary>
		/// Saves the customer to the database
		/// Returns true if successful, false otherwise
		/// </summary>
		bool Save();

		/// <summary>
		/// Deletes the customer from the database
		/// Returns true if successful, false otherwise
		/// </summary>
		bool Delete();

		/// <summary>
		/// Validates that the customer meets minimum requirements
		/// Customers must have at least LastName AND CompanyName
		/// </summary>
		bool IsValid();
	}
}
