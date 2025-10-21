using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.CustomerData
{
    public interface ICustomerRepository
    {
        List<Customer> GetList();
        Customer GetNewCustomer();
        Customer GetCustomerById(int id);
    }
}