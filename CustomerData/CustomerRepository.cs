using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace BT.Model.CustomerData
{
    internal class CustomerRepository : ICustomerRepository
    {
        private IDbConnection GetConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return new SqlConnection(connectionString);
        }

        public List<Customer> GetList()
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

        public Customer GetNewCustomer()
        {
            return new Customer();
        }

        public Customer GetCustomerById(int id)
        {
            using (var conn = GetConnection())
            {
                string sql = "SELECT * FROM Customers WHERE Id = @Id";
                var row = conn.QuerySingleOrDefault(sql, new { Id = id });
                if (row == null) return null;

                return MapRowToCustomer(row);
            }
        }

        private Customer MapRowToCustomer(dynamic row)
        {
            return new Customer
            {
                Id = row.Id,
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
        }
    }
}