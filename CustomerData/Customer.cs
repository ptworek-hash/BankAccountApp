using System;

namespace BT.Model.CustomerData
{
    /// <summary>
    /// Internal Customer implementation - not exposed directly to consumers
    /// Consumers must use ICustomer interface and the repository factory
    /// </summary>
    internal class Customer : ICustomer
    {
        private static ICustomerDataAccess _dataAccess = new CustomerDataAccess();
        private int _id;

        public int Id 
        { 
            get { return _id; }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public Address Address { get; set; } = new Address();

        /// <summary>
        /// Internal method to set the ID (used by data access layer after insert)
        /// </summary>
        internal void SetId(int id)
        {
            _id = id;
        }

        /// <summary>
        /// Saves the customer to the database
        /// Returns false if validation fails (requires LastName and CompanyName)
        /// </summary>
        public bool Save()
        {
            // Validation: customers must have LastName and CompanyName
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(CompanyName))
            {
                return false;
            }

            if (_id == 0)
            {
                // Insert new customer
                _id = _dataAccess.Insert(this);
            }
            else
            {
                // Update existing customer
                _dataAccess.Update(this);
            }

            return true;
        }

        /// <summary>
        /// Deletes the customer from the database
        /// Returns false if customer has not been saved (Id is 0)
        /// </summary>
        public bool Delete()
        {
            if (_id == 0)
                return false;

            _dataAccess.Delete(_id);
            return true;
        }

        /// <summary>
        /// Allows swapping out the data access implementation
        /// (for testing or changing database technology)
        /// </summary>
        internal static void SetDataAccess(ICustomerDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
    }
}te(sql, new { Id });
                return true;
            }
        }
    }
}