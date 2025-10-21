using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.CustomerData
{
    public interface ICustomerRepository
    {
        List<ICustomer> GetList();
        ICustomer GetNewCustomer();
        ICustomer GetCustomerById(int id);
    }
}