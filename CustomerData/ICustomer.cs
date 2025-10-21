using System;

namespace BT.Model.CustomerData
{
    /// <summary>
    /// Public interface for Customer object - this is what external consumers will use
    /// </summary>
    public interface ICustomer
    {
        int Id { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string CompanyName { get; set; }
        Address Address { get; set; }
        
        bool Save();
        bool Delete();
    }
}
