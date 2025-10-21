using System;
using System.Collections.Generic;

namespace BT.Model.CustomerData
{
    /// <summary>
    /// Public interface for Customer Repository
    /// Returns ICustomer interface to prevent external code from directly instantiating Customer objects
    /// </summary>
    public interface ICustomerRepository
    {
        List<ICustomer> GetList();
        ICustomer GetNewCustomer();
        ICustomer GetCustomerById(int id);
    }
});
    }
}