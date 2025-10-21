using System.Collections.Generic;

namespace BT.Model.CustomerData
{
    /// <summary>
    /// Internal implementation of Customer Repository
    /// Uses the data access layer to retrieve customers
    /// </summary>
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly ICustomerDataAccess _dataAccess;

        public CustomerRepository() : this(new CustomerDataAccess())
        {
        }

        // Constructor for dependency injection (useful for testing)
        internal CustomerRepository(ICustomerDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<ICustomer> GetList()
        {
            var customers = _dataAccess.GetAll();
            // Convert to ICustomer list
            return new List<ICustomer>(customers);
        }

        public ICustomer GetNewCustomer()
        {
            return new Customer();
        }

        public ICustomer GetCustomerById(int id)
        {
            return _dataAccess.GetById(id);
        }
    }
}p = row.Zip
                }
            };
        }
    }
}