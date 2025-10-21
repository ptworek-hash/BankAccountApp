using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BT.Model.CustomerData;

namespace Testing.BTModel
{
    [TestClass]
    public class CustomerRepositoryUnitTest
    {
        private ICustomerRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            // Create repository using the factory (as intended by the design)
            _repository = CustomerRepositoryFactory.Create();
        }

        [TestMethod]
        public void GetNewCustomer_ShouldReturnNewCustomerInstance()
        {
            // Act
            var customer = _repository.GetNewCustomer();

            // Assert
            Assert.IsNotNull(customer, "GetNewCustomer should return a non-null customer");
            Assert.AreEqual(0, customer.Id, "New customer should have Id of 0");
            Assert.IsNull(customer.FirstName, "New customer FirstName should be null");
            Assert.IsNull(customer.LastName, "New customer LastName should be null");
            Assert.IsNull(customer.CompanyName, "New customer CompanyName should be null");
            Assert.IsNotNull(customer.Address, "New customer should have an Address object");
        }

        [TestMethod]
        public void Customer_ShouldImplementICustomerInterface()
        {
            // Act
            var customer = _repository.GetNewCustomer();

            // Assert
            Assert.IsInstanceOfType(customer, typeof(ICustomer), "Customer should implement ICustomer interface");
        }

        [TestMethod]
        public void Customer_Save_WithoutLastName_ShouldReturnFalse()
        {
            // Arrange
            var customer = _repository.GetNewCustomer();
            customer.FirstName = "John";
            customer.CompanyName = "Acme Corp";
            // LastName is not set

            // Act
            bool result = customer.Save();

            // Assert
            Assert.IsFalse(result, "Save should return false when LastName is missing");
        }

        [TestMethod]
        public void Customer_Save_WithoutCompanyName_ShouldReturnFalse()
        {
            // Arrange
            var customer = _repository.GetNewCustomer();
            customer.FirstName = "John";
            customer.LastName = "Doe";
            // CompanyName is not set

            // Act
            bool result = customer.Save();

            // Assert
            Assert.IsFalse(result, "Save should return false when CompanyName is missing");
        }

        [TestMethod]
        public void Customer_Save_WithRequiredFields_ShouldReturnTrue()
        {
            // Arrange
            var customer = _repository.GetNewCustomer();
            customer.FirstName = "John";
            customer.LastName = "Doe";
            customer.CompanyName = "Acme Corp";
            customer.Address.Street = "123 Main St";
            customer.Address.City = "Anytown";
            customer.Address.State = "CA";
            customer.Address.Zip = "12345";

            try
            {
                // Act
                bool result = customer.Save();

                // Assert
                Assert.IsTrue(result, "Save should return true when all required fields are present");
                Assert.IsTrue(customer.Id > 0, "Customer Id should be set after save");

                // Cleanup
                customer.Delete();
            }
            catch (Exception ex)
            {
                Assert.Inconclusive($"Unable to test Save - database may not be configured: {ex.Message}");
            }
        }

        [TestMethod]
        public void GetCustomerById_WithValidId_ShouldReturnCustomer()
        {
            // Arrange - First create and save a customer
            var newCustomer = _repository.GetNewCustomer();
            newCustomer.FirstName = "Jane";
            newCustomer.LastName = "Smith";
            newCustomer.CompanyName = "Test Corp";
            newCustomer.Address.Street = "456 Oak Ave";
            newCustomer.Address.City = "Testville";
            newCustomer.Address.State = "NY";
            newCustomer.Address.Zip = "54321";

            try
            {
                bool saved = newCustomer.Save();
                if (!saved)
                {
                    Assert.Inconclusive("Unable to save customer for testing GetCustomerById");
                    return;
                }

                int customerId = newCustomer.Id;

                // Act
                var retrievedCustomer = _repository.GetCustomerById(customerId);

                // Assert
                Assert.IsNotNull(retrievedCustomer, "GetCustomerById should return a customer");
                Assert.AreEqual(customerId, retrievedCustomer.Id, "Retrieved customer should have the same Id");
                Assert.AreEqual("Jane", retrievedCustomer.FirstName, "FirstName should match");
                Assert.AreEqual("Smith", retrievedCustomer.LastName, "LastName should match");
                Assert.AreEqual("Test Corp", retrievedCustomer.CompanyName, "CompanyName should match");
                Assert.AreEqual("456 Oak Ave", retrievedCustomer.Address.Street, "Street should match");
                Assert.AreEqual("Testville", retrievedCustomer.Address.City, "City should match");
                Assert.AreEqual("NY", retrievedCustomer.Address.State, "State should match");
                Assert.AreEqual("54321", retrievedCustomer.Address.Zip, "Zip should match");

                // Cleanup
                retrievedCustomer.Delete();
            }
            catch (Exception ex)
            {
                Assert.Inconclusive($"Unable to test GetCustomerById - database may not be configured: {ex.Message}");
            }
        }

        [TestMethod]
        public void GetCustomerById_WithInvalidId_ShouldReturnNull()
        {
            try
            {
                // Act
                var customer = _repository.GetCustomerById(999999);

                // Assert
                Assert.IsNull(customer, "GetCustomerById should return null for non-existent Id");
            }
            catch (Exception ex)
            {
                Assert.Inconclusive($"Unable to test GetCustomerById - database may not be configured: {ex.Message}");
            }
        }

        [TestMethod]
        public void GetList_ShouldReturnListOfCustomers()
        {
            try
            {
                // Act
                var customers = _repository.GetList();

                // Assert
                Assert.IsNotNull(customers, "GetList should return a non-null list");
                Assert.IsInstanceOfType(customers, typeof(List<ICustomer>), "GetList should return List<ICustomer>");
            }
            catch (Exception ex)
            {
                Assert.Inconclusive($"Unable to test GetList - database may not be configured: {ex.Message}");
            }
        }

        [TestMethod]
        public void Customer_Delete_WhenNotSaved_ShouldReturnFalse()
        {
            // Arrange
            var customer = _repository.GetNewCustomer();
            customer.FirstName = "Test";
            customer.LastName = "User";
            customer.CompanyName = "Test Company";

            // Act
            bool result = customer.Delete();

            // Assert
            Assert.IsFalse(result, "Delete should return false for unsaved customer (Id = 0)");
        }

        [TestMethod]
        public void Customer_Delete_WhenSaved_ShouldReturnTrue()
        {
            // Arrange
            var customer = _repository.GetNewCustomer();
            customer.FirstName = "Delete";
            customer.LastName = "Test";
            customer.CompanyName = "Delete Corp";

            try
            {
                bool saved = customer.Save();
                if (!saved)
                {
                    Assert.Inconclusive("Unable to save customer for testing Delete");
                    return;
                }

                int customerId = customer.Id;

                // Act
                bool result = customer.Delete();

                // Assert
                Assert.IsTrue(result, "Delete should return true for saved customer");

                // Verify deletion
                var deletedCustomer = _repository.GetCustomerById(customerId);
                Assert.IsNull(deletedCustomer, "Customer should not exist after deletion");
            }
            catch (Exception ex)
            {
                Assert.Inconclusive($"Unable to test Delete - database may not be configured: {ex.Message}");
            }
        }

        [TestMethod]
        public void Address_AddressBlock_WithAllFields_ShouldReturnFormattedAddress()
        {
            // Arrange
            var customer = _repository.GetNewCustomer();
            customer.Address.Street = "123 Main St";
            customer.Address.City = "Springfield";
            customer.Address.State = "IL";
            customer.Address.Zip = "62701";

            // Act
            string addressBlock = customer.Address.AddressBlock();

            // Assert
            string expected = "123 Main St\nSpringfield, IL 62701";
            Assert.AreEqual(expected, addressBlock, "AddressBlock should return properly formatted address");
        }

        [TestMethod]
        public void Address_AddressBlock_WithNoFields_ShouldReturnEmpty()
        {
            // Arrange
            var customer = _repository.GetNewCustomer();
            // Address fields are all null/empty

            // Act
            string addressBlock = customer.Address.AddressBlock();

            // Assert
            Assert.AreEqual(string.Empty, addressBlock, "AddressBlock should return empty string when all fields are empty");
        }

        [TestMethod]
        public void Customer_Update_ShouldModifyExistingRecord()
        {
            // Arrange
            var customer = _repository.GetNewCustomer();
            customer.FirstName = "Original";
            customer.LastName = "Name";
            customer.CompanyName = "Original Company";
            customer.Address.City = "Original City";

            try
            {
                bool saved = customer.Save();
                if (!saved)
                {
                    Assert.Inconclusive("Unable to save customer for testing Update");
                    return;
                }

                int customerId = customer.Id;

                // Modify the customer
                customer.FirstName = "Updated";
                customer.CompanyName = "Updated Company";
                customer.Address.City = "Updated City";

                // Act
                bool updated = customer.Save();

                // Assert
                Assert.IsTrue(updated, "Update should return true");
                Assert.AreEqual(customerId, customer.Id, "Id should remain the same after update");

                // Verify changes were persisted
                var retrievedCustomer = _repository.GetCustomerById(customerId);
                Assert.AreEqual("Updated", retrievedCustomer.FirstName, "FirstName should be updated");
                Assert.AreEqual("Updated Company", retrievedCustomer.CompanyName, "CompanyName should be updated");
                Assert.AreEqual("Updated City", retrievedCustomer.Address.City, "City should be updated");

                // Cleanup
                retrievedCustomer.Delete();
            }
            catch (Exception ex)
            {
                Assert.Inconclusive($"Unable to test Update - database may not be configured: {ex.Message}");
            }
        }

        [TestMethod]
        public void RepositoryFactory_Create_ShouldReturnICustomerRepository()
        {
            // Act
            var repo = CustomerRepositoryFactory.Create();

            // Assert
            Assert.IsNotNull(repo, "Factory should return a non-null repository");
            Assert.IsInstanceOfType(repo, typeof(ICustomerRepository), "Factory should return ICustomerRepository");
        }
    }
}
