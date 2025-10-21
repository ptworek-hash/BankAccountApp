using System;

namespace BT.Model.CustomerData
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string AddressBlock()
        {
            if (string.IsNullOrWhiteSpace(Street) &&
                string.IsNullOrWhiteSpace(City) &&
                string.IsNullOrWhiteSpace(State) &&
                string.IsNullOrWhiteSpace(Zip))
            {
                return string.Empty;
            }

            return $"{Street}\n{City}, {State} {Zip}";
        }
    }
}