using System.Collections.Generic;

namespace BT.Model.Customer
{
    public interface ICustomerRepository
    {
        List<ICustomer> GetList();
        ICustomer GetNewCustomer();
        ICustomer GetCustomerById(long id);
    }
}
