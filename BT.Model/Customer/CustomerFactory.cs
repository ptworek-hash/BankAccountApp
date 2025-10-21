using System;

namespace BT.Model.Customer
{
    public static class CustomerFactory
    {
        public static ICustomerRepository CreateRepository()
        {
            return CustomerRepository.Instance;
        }

        public static ICustomer CreateNew()
        {
            return CustomerRepository.Instance.GetNewCustomer();
        }
    }
}
