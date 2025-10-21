using System;

namespace BT.Model.Customer
{
    public interface ICustomer
    {
        long Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string CompanyName { get; set; }
        IAddress Address { get; set; }

        bool Save();
        bool Delete();
    }
}
