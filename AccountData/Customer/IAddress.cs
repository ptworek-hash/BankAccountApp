using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Public interface for Address business object
	/// </summary>
	public interface IAddress
	{
		string Street { get; set; }
		string City { get; set; }
		string State { get; set; }
		string Zip { get; set; }

		/// <summary>
		/// Returns the street address in a standard text format:
		/// Street
		/// City, State Zip
		/// Returns empty string if there is no address data
		/// </summary>
		string AddressBlock();
	}
}
