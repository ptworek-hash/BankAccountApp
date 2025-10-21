using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Data Transfer Object for Customer
	/// This class is used for database mapping with Dapper
	/// Maps to the Customer table in the database
	/// </summary>
	public class CustomerDto
	{
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CompanyName { get; set; }
		
		// Address is stored in a separate table, linked by CustomerId
		// This DTO only contains the direct customer fields
	}
}
