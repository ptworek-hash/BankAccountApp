using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace BT.Model.Customer
{
    internal class CustomerRepository : ICustomerRepository
    {
        private static readonly Lazy<CustomerRepository> _instance =
            new Lazy<CustomerRepository>(() => new CustomerRepository());
        public static CustomerRepository Instance => _instance.Value;

        private CustomerRepository() { }

        public List<ICustomer> GetList()
        {
            using (var conn = SqliteConnectionFactory.Create())
            {
                var rows = conn.Query("SELECT Id, FirstName, LastName, CompanyName, Street, City, State, Zip FROM Customers");
                var list = new List<ICustomer>();
                foreach (var r in rows)
                {
                    list.Add(MapRow(r));
                }
                return list;
            }
        }

        public ICustomer GetNewCustomer()
        {
            return new Customer();
        }

        public ICustomer GetCustomerById(long id)
        {
            using (var conn = SqliteConnectionFactory.Create())
            {
                var r = conn.QueryFirstOrDefault("SELECT Id, FirstName, LastName, CompanyName, Street, City, State, Zip FROM Customers WHERE Id=@id", new { id });
                if (r == null) return null;
                return MapRow(r);
            }
        }

        internal bool Save(ICustomer customer)
        {
            using (var conn = SqliteConnectionFactory.Create())
            {
                if (customer.Id <= 0)
                {
                    var sql = @"INSERT INTO Customers (FirstName, LastName, CompanyName, Street, City, State, Zip)
                                VALUES (@FirstName, @LastName, @CompanyName, @Street, @City, @State, @Zip);
                                SELECT last_insert_rowid();";
                    var id = conn.ExecuteScalar<long>(sql, new
                    {
                        customer.FirstName,
                        customer.LastName,
                        customer.CompanyName,
                        Street = customer.Address?.Street,
                        City = customer.Address?.City,
                        State = customer.Address?.State,
                        Zip = customer.Address?.Zip
                    });
                    customer.Id = id;
                    return id > 0;
                }
                else
                {
                    var sql = @"UPDATE Customers SET FirstName=@FirstName, LastName=@LastName, CompanyName=@CompanyName,
                                Street=@Street, City=@City, State=@State, Zip=@Zip WHERE Id=@Id";
                    var rows = conn.Execute(sql, new
                    {
                        customer.FirstName,
                        customer.LastName,
                        customer.CompanyName,
                        Street = customer.Address?.Street,
                        City = customer.Address?.City,
                        State = customer.Address?.State,
                        Zip = customer.Address?.Zip,
                        customer.Id
                    });
                    return rows > 0;
                }
            }
        }

        internal bool Delete(long id)
        {
            using (var conn = SqliteConnectionFactory.Create())
            {
                var rows = conn.Execute("DELETE FROM Customers WHERE Id=@id", new { id });
                return rows > 0;
            }
        }

        private static ICustomer MapRow(dynamic r)
        {
            var c = new Customer
            {
                Id = (long)r.Id,
                FirstName = r.FirstName as string,
                LastName = r.LastName as string,
                CompanyName = r.CompanyName as string,
                Address = new Address
                {
                    Street = r.Street as string,
                    City = r.City as string,
                    State = r.State as string,
                    Zip = r.Zip as string
                }
            };
            return c;
        }
    }
}
