using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Data Transfer Object for Address
	/// This class is used for database mapping with Dapper
	/// Maps to the Address table in the database
	/// </summary>
	public class AddressDto
	{
		public long Id { get; set; }
		public long CustomerId { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
	}
}
