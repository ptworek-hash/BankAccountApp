using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace BT.Model.CustomerData
{
    /// <summary>
    /// Dapper implementation of customer data access
    /// This can be swapped out for a different ORM or data technology
    /// </summary>
    internal class CustomerDataAccess : ICustomerDataAccess
    {
        private IDbConnection GetConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return new SqlConnection(connectionString);
        }

        public int Insert(Customer customer)
        {
            using (var conn = GetConnection())
            {
                string sql = @"
                    INSERT INTO Customers (FirstName, LastName, CompanyName, Street, City, State, Zip)
                    VALUES (@FirstName, @LastName, @CompanyName, @Street, @City, @State, @Zip);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                
                return conn.QuerySingle<int>(sql, new
                {
                    customer.FirstName,
                    customer.LastName,
                    customer.CompanyName,
                    customer.Address.Street,
                    customer.Address.City,
                    customer.Address.State,
                    customer.Address.Zip
                });
            }
        }

        public void Update(Customer customer)
        {
            using (var conn = GetConnection())
            {
                string sql = @"
                    UPDATE Customers
                    SET FirstName = @FirstName,
                        LastName = @LastName,
                        CompanyName = @CompanyName,
                        Street = @Street,
                        City = @City,
                        State = @State,
                        Zip = @Zip
                    WHERE Id = @Id";
                
                conn.Execute(sql, new
                {
                    customer.FirstName,
                    customer.LastName,
                    customer.CompanyName,
                    customer.Address.Street,
                    customer.Address.City,
                    customer.Address.State,
                    customer.Address.Zip,
                    customer.Id
                });
            }
        }

        public void Delete(int id)
        {
            using (var conn = GetConnection())
            {
                string sql = "DELETE FROM Customers WHERE Id = @Id";
                conn.Execute(sql, new { Id = id });
            }
        }

        public Customer GetById(int id)
        {
            using (var conn = GetConnection())
            {
                string sql = "SELECT * FROM Customers WHERE Id = @Id";
                var row = conn.QuerySingleOrDefault(sql, new { Id = id });
                if (row == null) return null;

                return MapRowToCustomer(row);
            }
        }

        public List<Customer> GetAll()
        {
            using (var conn = GetConnection())
            {
                string sql = "SELECT * FROM Customers";
                var results = conn.Query(sql);
                var customers = new List<Customer>();

                foreach (var row in results)
                {
                    customers.Add(MapRowToCustomer(row));
                }
                return customers;
            }
        }

        private Customer MapRowToCustomer(dynamic row)
        {
            var customer = new Customer
            {
                FirstName = row.FirstName,
                LastName = row.LastName,
                CompanyName = row.CompanyName,
                Address = new Address
                {
                    Street = row.Street,
                    City = row.City,
                    State = row.State,
                    Zip = row.Zip
                }
            };
            customer.SetId(row.Id);
            return customer;
        }
    }
}
