using System;

namespace BT.Model.Customer
{
    internal class Customer : ICustomer
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public IAddress Address { get; set; } = new Address();

        public bool Save()
        {
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(CompanyName))
            {
                return false;
            }
            return CustomerRepository.Instance.Save(this);
        }

        public bool Delete()
        {
            if (Id <= 0) return false;
            return CustomerRepository.Instance.Delete(Id);
        }
    }
}
