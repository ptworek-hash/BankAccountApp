using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BT.Model.AccountData;

namespace Testing.BTModel
{
	/// <summary>
	/// Unit tests for Address functionality
	/// Tests the AddressBlock() method formatting
	/// </summary>
	[TestClass]
	public class AddressTests
	{
		private ICustomerRepository _repository;

		[TestInitialize]
		public void Setup()
		{
			var mockDataAccess = new MockCustomerDataAccess();
			_repository = CustomerFactory.Create(mockDataAccess);
		}

		[TestMethod]
		public void AddressBlock_FullAddress_FormatsCorrectly()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.Address.Street = "123 Main Street";
			customer.Address.City = "New York";
			customer.Address.State = "NY";
			customer.Address.Zip = "10001";

			// Act
			string addressBlock = customer.Address.AddressBlock();

			// Assert
			Assert.IsNotNull(addressBlock);
			string expected = "123 Main Street\r\nNew York, NY 10001";
			Assert.AreEqual(expected, addressBlock, "Address block should be formatted as: Street\\nCity, State Zip");
		}

		[TestMethod]
		public void AddressBlock_NoAddress_ReturnsEmpty()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			// All address fields are null

			// Act
			string addressBlock = customer.Address.AddressBlock();

			// Assert
			Assert.AreEqual(string.Empty, addressBlock, "Empty address should return empty string");
		}

		[TestMethod]
		public void AddressBlock_OnlyStreet_FormatsCorrectly()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.Address.Street = "123 Main Street";

			// Act
			string addressBlock = customer.Address.AddressBlock();

			// Assert
			Assert.AreEqual("123 Main Street", addressBlock);
		}

		[TestMethod]
		public void AddressBlock_OnlyCityState_FormatsCorrectly()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.Address.City = "New York";
			customer.Address.State = "NY";

			// Act
			string addressBlock = customer.Address.AddressBlock();

			// Assert
			Assert.AreEqual("New York, NY", addressBlock);
		}

		[TestMethod]
		public void AddressBlock_OnlyCityZip_FormatsCorrectly()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.Address.City = "New York";
			customer.Address.Zip = "10001";

			// Act
			string addressBlock = customer.Address.AddressBlock();

			// Assert
			Assert.AreEqual("New York 10001", addressBlock);
		}

		[TestMethod]
		public void AddressBlock_PartialAddress_FormatsCorrectly()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.Address.Street = "123 Main Street";
			customer.Address.City = "New York";
			// No State or Zip

			// Act
			string addressBlock = customer.Address.AddressBlock();

			// Assert
			string expected = "123 Main Street\r\nNew York";
			Assert.AreEqual(expected, addressBlock);
		}

		[TestMethod]
		public void AddressBlock_WithWhitespace_TrimsCorrectly()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.Address.Street = "  123 Main Street  ";
			customer.Address.City = "  New York  ";
			customer.Address.State = "  NY  ";
			customer.Address.Zip = "  10001  ";

			// Act
			string addressBlock = customer.Address.AddressBlock();

			// Assert
			string expected = "123 Main Street\r\nNew York, NY 10001";
			Assert.AreEqual(expected, addressBlock, "Whitespace should be trimmed");
		}

		[TestMethod]
		public void AddressBlock_OnlyWhitespace_ReturnsEmpty()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();
			customer.Address.Street = "   ";
			customer.Address.City = "   ";
			customer.Address.State = "   ";
			customer.Address.Zip = "   ";

			// Act
			string addressBlock = customer.Address.AddressBlock();

			// Assert
			Assert.AreEqual(string.Empty, addressBlock, "Only whitespace should return empty string");
		}

		[TestMethod]
		public void Address_Properties_GetSet_Correctly()
		{
			// Arrange
			var customer = _repository.GetNewCustomer();

			// Act
			customer.Address.Street = "123 Main St";
			customer.Address.City = "New York";
			customer.Address.State = "NY";
			customer.Address.Zip = "10001";

			// Assert
			Assert.AreEqual("123 Main St", customer.Address.Street);
			Assert.AreEqual("New York", customer.Address.City);
			Assert.AreEqual("NY", customer.Address.State);
			Assert.AreEqual("10001", customer.Address.Zip);
		}
	}
}
