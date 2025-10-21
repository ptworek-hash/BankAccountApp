using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;


namespace BT.Model.CustomerData
{
    public class Customer : ICustomer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public Address Address { get; set; } = new Address();

        private IDbConnection GetConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return new SqlConnection(connectionString);
        }

        public bool Save()
        {
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(CompanyName))
            {
                return false;
            }

            using (var conn = GetConnection())
            {
                if (Id == 0)
                {
                    // Insert
                    string sql = @"
                        INSERT INTO Customers (FirstName, LastName, CompanyName, Street, City, State, Zip)
                        VALUES (@FirstName, @LastName, @CompanyName, @Street, @City, @State, @Zip);
                        SELECT CAST(SCOPE_IDENTITY() as int)";
                    
                    Id = conn.QuerySingle<int>(sql, new
                    {
                        FirstName,
                        LastName,
                        CompanyName,
                        Address.Street,
                        Address.City,
                        Address.State,
                        Address.Zip
                    });
                }
                else
                {
                    // Update
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
                        FirstName,
                        LastName,
                        CompanyName,
                        Address.Street,
                        Address.City,
                        Address.State,
                        Address.Zip,
                        Id
                    });
                }

                return true;
            }
        }
        public bool Delete()
        {
            if (Id == 0)
                return false;

            using (var conn = GetConnection())
            {
                string sql = "DELETE FROM Customers WHERE Id = @Id";
                conn.Execute(sql, new { Id });
                return true;
            }
        }
    }
}