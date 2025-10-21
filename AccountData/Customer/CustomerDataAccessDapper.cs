using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Configuration;
using Dapper;
using System.Linq;

namespace BT.Model.AccountData
{
	/// <summary>
	/// Dapper-based implementation of customer data access
	/// Uses SQLite for demo purposes, but can easily be swapped to SQL Server
	/// by changing the connection string and database provider
	/// </summary>
	internal class CustomerDataAccessDapper : ICustomerDataAccess
	{
		private readonly string _connectionString;

		public CustomerDataAccessDapper()
		{
			// Get connection string from web.config
			// Falls back to a default SQLite database if not configured
			_connectionString = ConfigurationManager.ConnectionStrings["CustomerDB"]?.ConnectionString 
				?? "Data Source=|DataDirectory|\\CustomerDB.db;Version=3;";
			
			// Ensure database and tables exist
			InitializeDatabase();
		}

		/// <summary>
		/// Creates database and tables if they don't exist
		/// </summary>
		private void InitializeDatabase()
		{
			using (var connection = GetConnection())
			{
				connection.Open();

				// Create Customer table
				var createCustomerTable = @"
					CREATE TABLE IF NOT EXISTS Customer (
						Id INTEGER PRIMARY KEY AUTOINCREMENT,
						FirstName TEXT,
						LastName TEXT NOT NULL,
						CompanyName TEXT NOT NULL
					)";
				connection.Execute(createCustomerTable);

				// Create Address table
				var createAddressTable = @"
					CREATE TABLE IF NOT EXISTS Address (
						Id INTEGER PRIMARY KEY AUTOINCREMENT,
						CustomerId INTEGER NOT NULL,
						Street TEXT,
						City TEXT,
						State TEXT,
						Zip TEXT,
						FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
					)";
				connection.Execute(createAddressTable);

				// Create index on CustomerId for performance
				var createIndex = @"
					CREATE INDEX IF NOT EXISTS IX_Address_CustomerId 
					ON Address(CustomerId)";
				connection.Execute(createIndex);
			}
		}

		/// <summary>
		/// Creates a database connection
		/// This method can be modified to use SQL Server instead of SQLite
		/// </summary>
		private IDbConnection GetConnection()
		{
			// For SQLite
			return new SQLiteConnection(_connectionString);

			// For SQL Server, you would use:
			// return new SqlConnection(_connectionString);
		}

		#region Customer Operations

		/// <summary>
		/// Inserts a new customer into the database
		/// Returns the new customer ID
		/// </summary>
		public long InsertCustomer(CustomerDto customer)
		{
			using (var connection = GetConnection())
			{
				var sql = @"
					INSERT INTO Customer (FirstName, LastName, CompanyName)
					VALUES (@FirstName, @LastName, @CompanyName);
					SELECT last_insert_rowid();";

				return connection.ExecuteScalar<long>(sql, customer);
			}
		}

		/// <summary>
		/// Updates an existing customer in the database
		/// </summary>
		public void UpdateCustomer(CustomerDto customer)
		{
			using (var connection = GetConnection())
			{
				var sql = @"
					UPDATE Customer 
					SET FirstName = @FirstName,
						LastName = @LastName,
						CompanyName = @CompanyName
					WHERE Id = @Id";

				connection.Execute(sql, customer);
			}
		}

		/// <summary>
		/// Deletes a customer from the database
		/// </summary>
		public void DeleteCustomer(long customerId)
		{
			using (var connection = GetConnection())
			{
				var sql = "DELETE FROM Customer WHERE Id = @Id";
				connection.Execute(sql, new { Id = customerId });
			}
		}

		/// <summary>
		/// Gets a customer by ID
		/// Returns null if not found
		/// </summary>
		public CustomerDto GetCustomerById(long customerId)
		{
			using (var connection = GetConnection())
			{
				var sql = "SELECT * FROM Customer WHERE Id = @Id";
				return connection.QueryFirstOrDefault<CustomerDto>(sql, new { Id = customerId });
			}
		}

		/// <summary>
		/// Gets all customers from the database
		/// </summary>
		public List<CustomerDto> GetAllCustomers()
		{
			using (var connection = GetConnection())
			{
				var sql = "SELECT * FROM Customer ORDER BY LastName, FirstName";
				return connection.Query<CustomerDto>(sql).ToList();
			}
		}

		#endregion

		#region Address Operations

		/// <summary>
		/// Inserts a new address into the database
		/// Returns the new address ID
		/// </summary>
		public long InsertAddress(AddressDto address)
		{
			using (var connection = GetConnection())
			{
				var sql = @"
					INSERT INTO Address (CustomerId, Street, City, State, Zip)
					VALUES (@CustomerId, @Street, @City, @State, @Zip);
					SELECT last_insert_rowid();";

				return connection.ExecuteScalar<long>(sql, address);
			}
		}

		/// <summary>
		/// Updates an existing address in the database
		/// </summary>
		public void UpdateAddress(AddressDto address)
		{
			using (var connection = GetConnection())
			{
				var sql = @"
					UPDATE Address 
					SET CustomerId = @CustomerId,
						Street = @Street,
						City = @City,
						State = @State,
						Zip = @Zip
					WHERE Id = @Id";

				connection.Execute(sql, address);
			}
		}

		/// <summary>
		/// Deletes an address from the database
		/// </summary>
		public void DeleteAddress(long addressId)
		{
			using (var connection = GetConnection())
			{
				var sql = "DELETE FROM Address WHERE Id = @Id";
				connection.Execute(sql, new { Id = addressId });
			}
		}

		/// <summary>
		/// Gets an address by customer ID
		/// Returns null if not found
		/// </summary>
		public AddressDto GetAddressByCustomerId(long customerId)
		{
			using (var connection = GetConnection())
			{
				var sql = "SELECT * FROM Address WHERE CustomerId = @CustomerId";
				return connection.QueryFirstOrDefault<AddressDto>(sql, new { CustomerId = customerId });
			}
		}

		#endregion
	}
}
