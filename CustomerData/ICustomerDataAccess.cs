using System;
using System.Collections.Generic;

namespace BT.Model.CustomerData
{
    /// <summary>
    /// Interface for data access operations - allows swapping out the data layer
    /// </summary>
    internal interface ICustomerDataAccess
    {
        int Insert(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        Customer GetById(int id);
        List<Customer> GetAll();
    }
}
