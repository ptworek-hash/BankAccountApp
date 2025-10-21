using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BT.Model.Customer;

namespace Testing.BTModel
{
	[TestClass]
	public class SampleItemUnitTest
	{
		[TestMethod]
		public void SampleDataCreate()
		{
			var repo = CustomerFactory.CreateRepository();
			Assert.IsNotNull(repo.GetNewCustomer());

		}


		[TestMethod]
        public void AddressBlockFormatting()
        {
            var repo = CustomerFactory.CreateRepository();
            var customer = repo.GetNewCustomer();
            Assert.AreEqual(string.Empty, customer.Address.AddressBlock());
            customer.Address.Street = "123 Main St";
            customer.Address.City = "Chicago";
            customer.Address.State = "IL";
            customer.Address.Zip = "60601";
            Assert.AreEqual("123 Main St\nChicago, IL 60601", customer.Address.AddressBlock());
        }

		[TestMethod]
		public void CustomerRepository_SaveGetDelete()
		{
			var repo = CustomerFactory.CreateRepository();
			var customer = repo.GetNewCustomer();
			customer.FirstName = "Jane";
			customer.LastName = "Doe";
			customer.CompanyName = "Acme Inc.";
			customer.Address.Street = "1 Loop";
			customer.Address.City = "Chicago";
			customer.Address.State = "IL";
			customer.Address.Zip = "60601";

			Assert.IsTrue(customer.Save());
			Assert.IsTrue(customer.Id > 0);

			var fetched = repo.GetCustomerById(customer.Id);
			Assert.IsNotNull(fetched);
			Assert.AreEqual("Doe", fetched.LastName);
			Assert.AreEqual("Acme Inc.", fetched.CompanyName);

			Assert.IsTrue(customer.Delete());
			var missing = repo.GetCustomerById(customer.Id);
			Assert.IsNull(missing);
		}

	}
}
