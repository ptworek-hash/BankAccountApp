using System;

namespace BT.Model.Customer
{
    public interface IAddress
    {
        string Street { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }

        string AddressBlock();
    }
}
