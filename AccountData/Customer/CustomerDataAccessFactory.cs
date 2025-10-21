using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Factory for creating CustomerDataAccess instances
	/// This allows us to centralize the creation logic and easily swap implementations
	/// </summary>
	internal static class CustomerDataAccessFactory
	{
		/// <summary>
		/// Creates an instance of the customer data access layer
		/// Returns the Dapper-based implementation
		/// </summary>
		public static ICustomerDataAccess Create()
		{
			// This will return our Dapper implementation (to be created in Part 1b)
			return new CustomerDataAccessDapper();
		}
	}
}
