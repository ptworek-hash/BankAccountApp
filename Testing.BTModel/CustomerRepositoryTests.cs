using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BT.Model.AccountData;

namespace Testing.BTModel
{
	/// <summary>
	/// Unit tests for Customer Repository
	/// Tests all repository methods: GetList, GetNewCustomer, GetCustomerById
	/// Uses mock data access layer for isolated testing
	/// </summary>
	[TestClass]
	public class CustomerRepositoryTests
	{
		private MockCustomerDataAccess _mockDataAccess;
		private ICustomerRepository _repository;

		[TestInitialize]
		public void Setup()
		{
			// Create fresh mock data access for each test
			_mockDataAccess = new MockCustomerDataAccess();
			
			// Create repository with mock data access
			_repository = CustomerFactory.Create(_mockDataAccess);
		}

		#region Repository Tests

		[TestMethod]
		public void GetNewCustomer_ReturnsEmptyCustomer()
		{
			// Act
			var customer = _repository.GetNewCustomer();

			// Assert
			Assert.IsNotNull(customer, "GetNewCustomer should return a customer object");
			Assert.AreEqual(0, customer.Id, "New customer should have Id = 0");
			Assert.IsNull(customer.FirstName, "New customer FirstName should be null");
			Assert.IsNull(customer.LastName, "New customer LastName should be null");
			Assert.IsNull(customer.CompanyName, "New customer CompanyName should be null");
			Assert.IsNotNull(customer.Address, "New customer should have an Address object");
		}

		[TestMethod]
		public void GetCustomerById_ExistingCustomer_ReturnsCustomer()
		{
			// Arrange
			_mockDataAccess.AddTestCustomer(1, "John", "Doe", "Acme Corp", 
				"123 Main St", "New York", "NY", "10001");

			// Act
			var customer = _repository.GetCustomerById(1);

			// Assert
			Assert.IsNotNull(customer, "Should return customer when it exists");
			Assert.AreEqual(1, customer.Id);
			Assert.AreEqual("John", customer.FirstName);
			Assert.AreEqual("Doe", customer.LastName);
			Assert.AreEqual("Acme Corp", customer.CompanyName);
			Assert.IsNotNull(customer.Address);
			Assert.AreEqual("123 Main St", customer.Address.Street);
			Assert.AreEqual("New York", customer.Address.City);
			Assert.AreEqual("NY", customer.Address.State);
			Assert.AreEqual("10001", customer.Address.Zip);
		}

		[TestMethod]
		public void GetCustomerById_NonExistingCustomer_ReturnsNull()
		{
			// Act
			var customer = _repository.GetCustomerById(999);

			// Assert
			Assert.IsNull(customer, "Should return null when customer doesn't exist");
		}

		[TestMethod]
		public void GetList_NoCustomers_ReturnsEmptyList()
		{
			// Act
			var customers = _repository.GetList();

			// Assert
			Assert.IsNotNull(customers, "GetList should never return null");
			Assert.AreEqual(0, customers.Count, "Should return empty list when no customers");
		}

		[TestMethod]
		public void GetList_WithCustomers_ReturnsAllCustomers()
		{
			// Arrange
			_mockDataAccess.AddTestCustomer(1, "John", "Doe", "Acme Corp", 
				"123 Main St", "New York", "NY", "10001");
			_mockDataAccess.AddTestCustomer(2, "Jane", "Smith", "TechStart Inc", 
				"456 Tech Blvd", "San Francisco", "CA", "94102");
			_mockDataAccess.AddTestCustomer(3, "Bob", "Johnson", "Global Solutions", 
				"789 Business Park", "Austin", "TX", "78701");

			// Act
			var customers = _repository.GetList();

			// Assert
			Assert.IsNotNull(customers);
			Assert.AreEqual(3, customers.Count, "Should return all 3 customers");
			
			// Verify first customer
			var customer1 = customers.FirstOrDefault(c => c.Id == 1);
			Assert.IsNotNull(customer1);
			Assert.AreEqual("John", customer1.FirstName);
			Assert.AreEqual("Acme Corp", customer1.CompanyName);
			
			// Verify second customer
			var customer2 = customers.FirstOrDefault(c => c.Id == 2);
			Assert.IsNotNull(customer2);
			Assert.AreEqual("Jane", customer2.FirstName);
			Assert.AreEqual("San Francisco", customer2.Address.City);
		}

		[TestMethod]
		public void GetList_LoadsAddresses_Correctly()
		{
			// Arrange
			_mockDataAccess.AddTestCustomer(1, "John", "Doe", "Acme Corp", 
				"123 Main St", "New York", "NY", "10001");

			// Act
			var customers = _repository.GetList();

			// Assert
			Assert.AreEqual(1, customers.Count);
			var customer = customers[0];
			Assert.IsNotNull(customer.Address, "Customer should have address");
			Assert.AreEqual("123 Main St", customer.Address.Street);
		}

		#endregion

		#region Customer Save/Delete Tests

		[TestMethod]
		public void Customer_Save_ValidCustomer_Succeeds()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.FirstName = "John";
			customer.LastName = "Doe";
			customer.CompanyName = "Acme Corp";

			// Act
			bool result = customer.Save();

			// Assert
			Assert.IsTrue(result, "Save should succeed for valid customer");
			Assert.IsTrue(customer.Id > 0, "Customer should have ID after save");
			Assert.AreEqual(1, _mockDataAccess.Customers.Count, "Customer should be in database");
		}

		[TestMethod]
		public void Customer_Save_WithAddress_SavesAddress()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.FirstName = "John";
			customer.LastName = "Doe";
			customer.CompanyName = "Acme Corp";
			customer.Address.Street = "123 Main St";
			customer.Address.City = "New York";
			customer.Address.State = "NY";
			customer.Address.Zip = "10001";

			// Act
			bool result = customer.Save();

			// Assert
			Assert.IsTrue(result, "Save should succeed");
			Assert.AreEqual(1, _mockDataAccess.Addresses.Count, "Address should be saved");
			var savedAddress = _mockDataAccess.Addresses[0];
			Assert.AreEqual("123 Main St", savedAddress.Street);
			Assert.AreEqual(customer.Id, savedAddress.CustomerId, "Address should be linked to customer");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Customer_Save_MissingLastName_ThrowsException()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.FirstName = "John";
			// LastName is missing
			customer.CompanyName = "Acme Corp";

			// Act
			customer.Save(); // Should throw InvalidOperationException
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Customer_Save_MissingCompanyName_ThrowsException()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.FirstName = "John";
			customer.LastName = "Doe";
			// CompanyName is missing

			// Act
			customer.Save(); // Should throw InvalidOperationException
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Customer_Save_MissingBothRequired_ThrowsException()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.FirstName = "John";
			// Both LastName and CompanyName are missing

			// Act
			customer.Save(); // Should throw InvalidOperationException
		}

		[TestMethod]
		public void Customer_Update_ExistingCustomer_Succeeds()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.FirstName = "John";
			customer.LastName = "Doe";
			customer.CompanyName = "Acme Corp";
			customer.Save();

			// Act
			customer.FirstName = "Jane";
			customer.CompanyName = "New Company";
			bool result = customer.Save();

			// Assert
			Assert.IsTrue(result, "Update should succeed");
			var savedCustomer = _mockDataAccess.Customers[0];
			Assert.AreEqual("Jane", savedCustomer.FirstName);
			Assert.AreEqual("New Company", savedCustomer.CompanyName);
		}

		[TestMethod]
		public void Customer_Delete_ExistingCustomer_Succeeds()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.FirstName = "John";
			customer.LastName = "Doe";
			customer.CompanyName = "Acme Corp";
			customer.Save();
			long customerId = customer.Id;

			// Act
			bool result = customer.Delete();

			// Assert
			Assert.IsTrue(result, "Delete should succeed");
			Assert.AreEqual(0, customer.Id, "Customer ID should be reset to 0");
			Assert.AreEqual(0, _mockDataAccess.Customers.Count, "Customer should be removed from database");
		}

		[TestMethod]
		public void Customer_Delete_NewCustomer_ReturnsFalse()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();

			// Act
			bool result = customer.Delete();

			// Assert
			Assert.IsFalse(result, "Cannot delete a customer that hasn't been saved");
		}

		[TestMethod]
		public void Customer_Delete_WithAddress_DeletesAddress()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.FirstName = "John";
			customer.LastName = "Doe";
			customer.CompanyName = "Acme Corp";
			customer.Address.Street = "123 Main St";
			customer.Address.City = "New York";
			customer.Save();

			// Act
			customer.Delete();

			// Assert
			Assert.AreEqual(0, _mockDataAccess.Addresses.Count, "Address should be deleted with customer");
		}

		#endregion

		#region Validation Tests

		[TestMethod]
		public void Customer_IsValid_WithBothRequired_ReturnsTrue()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.LastName = "Doe";
			customer.CompanyName = "Acme Corp";

			// Act
			bool isValid = customer.IsValid();

			// Assert
			Assert.IsTrue(isValid, "Customer with LastName and CompanyName should be valid");
		}

		[TestMethod]
		public void Customer_IsValid_MissingLastName_ReturnsFalse()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.CompanyName = "Acme Corp";

			// Act
			bool isValid = customer.IsValid();

			// Assert
			Assert.IsFalse(isValid, "Customer without LastName should be invalid");
		}

		[TestMethod]
		public void Customer_IsValid_MissingCompanyName_ReturnsFalse()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.LastName = "Doe";

			// Act
			bool isValid = customer.IsValid();

			// Assert
			Assert.IsFalse(isValid, "Customer without CompanyName should be invalid");
		}

		[TestMethod]
		public void Customer_IsValid_EmptyStrings_ReturnsFalse()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.LastName = "   "; // Whitespace only
			customer.CompanyName = "";  // Empty string

			// Act
			bool isValid = customer.IsValid();

			// Assert
			Assert.IsFalse(isValid, "Customer with empty/whitespace should be invalid");
		}

		#endregion
	}
}
