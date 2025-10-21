using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Internal implementation of Address business object
	/// External code cannot directly instantiate this class
	/// </summary>
	internal class Address : IAddress
	{
		private AddressDto _record;

		/// <summary>
		/// Constructor - accepts an AddressDto (for loading from database)
		/// or creates a new empty one
		/// </summary>
		public Address(AddressDto record = null)
		{
			if (record == null) { record = new AddressDto(); }
			_record = record;
		}

		public long Id
		{
			get { return _record.Id; }
			set { _record.Id = value; }
		}

		public long CustomerId
		{
			get { return _record.CustomerId; }
			set { _record.CustomerId = value; }
		}

		public string Street
		{
			get { return _record.Street; }
			set { _record.Street = value; }
		}

		public string City
		{
			get { return _record.City; }
			set { _record.City = value; }
		}

		public string State
		{
			get { return _record.State; }
			set { _record.State = value; }
		}

		public string Zip
		{
			get { return _record.Zip; }
			set { _record.Zip = value; }
		}

		/// <summary>
		/// Returns the street address in a standard text format:
		/// Street
		/// City, State Zip
		/// Returns empty string if there is no address data
		/// </summary>
		public string AddressBlock()
		{
			// Check if we have any address data
			bool hasData = !string.IsNullOrWhiteSpace(Street) ||
						   !string.IsNullOrWhiteSpace(City) ||
						   !string.IsNullOrWhiteSpace(State) ||
						   !string.IsNullOrWhiteSpace(Zip);

			if (!hasData)
			{
				return string.Empty;
			}

			// Build the address block
			StringBuilder sb = new StringBuilder();

			// First line: Street
			if (!string.IsNullOrWhiteSpace(Street))
			{
				sb.AppendLine(Street.Trim());
			}

			// Second line: City, State Zip
			List<string> cityStateZip = new List<string>();

			if (!string.IsNullOrWhiteSpace(City))
			{
				cityStateZip.Add(City.Trim());
			}

			if (!string.IsNullOrWhiteSpace(State))
			{
				// If we have a city, add comma separator
				if (cityStateZip.Count > 0)
				{
					cityStateZip[cityStateZip.Count - 1] += ",";
				}
				cityStateZip.Add(State.Trim());
			}

			if (!string.IsNullOrWhiteSpace(Zip))
			{
				cityStateZip.Add(Zip.Trim());
			}

			if (cityStateZip.Count > 0)
			{
				sb.Append(string.Join(" ", cityStateZip));
			}

			return sb.ToString().TrimEnd();
		}

		/// <summary>
		/// Internal method to get the underlying DTO
		/// Used by the Customer class for database operations
		/// </summary>
		internal AddressDto GetDto()
		{
			return _record;
		}
	}
}
