using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.CustomerData
{
    public static class CustomerRepositoryFactory
    {
        public static ICustomerRepository Create()
        {
            return new CustomerRepository();
        }
    }
}