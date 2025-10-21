using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BT.Model.AccountData;

namespace BankAccountApp.Controllers
{
	/// <summary>
	/// RESTful API Controller for Customer operations
	/// Provides JSON endpoints for customer data
	/// </summary>
	public class CustomerController : Controller
	{
		/// <summary>
		/// GET: /Customer/List
		/// RESTful endpoint that returns all customers as JSON
		/// Used by the customer list page to display data
		/// </summary>
		/// <returns>JSON array of all customers with their addresses</returns>
		[HttpGet]
		public JsonResult List()
		{
			try
			{
				// Use the factory to create the repository (proper encapsulation)
				var repo = CustomerFactory.Create();
				
				// Get all customers
				var customers = repo.GetList();

				// Transform to a simplified DTO for the API response
				// This prevents circular references and controls what data is exposed
				var customerData = customers.Select(c => new
				{
					Id = c.Id,
					FirstName = c.FirstName ?? string.Empty,
					LastName = c.LastName ?? string.Empty,
					CompanyName = c.CompanyName ?? string.Empty,
					Address = new
					{
						Street = c.Address?.Street ?? string.Empty,
						City = c.Address?.City ?? string.Empty,
						State = c.Address?.State ?? string.Empty,
						Zip = c.Address?.Zip ?? string.Empty,
						AddressBlock = c.Address?.AddressBlock() ?? string.Empty
					}
				}).ToList();

				return Json(new
				{
					success = true,
					data = customerData,
					count = customerData.Count
				}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				// Return error response
				return Json(new
				{
					success = false,
					error = ex.Message
				}, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// GET: /Customer/GetById?id=123
		/// RESTful endpoint that returns a specific customer by ID
		/// </summary>
		/// <param name="id">Customer ID</param>
		/// <returns>JSON object of the customer</returns>
		[HttpGet]
		public JsonResult GetById(long id)
		{
			try
			{
				var repo = CustomerFactory.Create();
				var customer = repo.GetCustomerById(id);

				if (customer == null)
				{
					return Json(new
					{
						success = false,
						error = "Customer not found"
					}, JsonRequestBehavior.AllowGet);
				}

				var customerData = new
				{
					Id = customer.Id,
					FirstName = customer.FirstName ?? string.Empty,
					LastName = customer.LastName ?? string.Empty,
					CompanyName = customer.CompanyName ?? string.Empty,
					Address = new
					{
						Street = customer.Address?.Street ?? string.Empty,
						City = customer.Address?.City ?? string.Empty,
						State = customer.Address?.State ?? string.Empty,
						Zip = customer.Address?.Zip ?? string.Empty,
						AddressBlock = customer.Address?.AddressBlock() ?? string.Empty
					}
				};

				return Json(new
				{
					success = true,
					data = customerData
				}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					error = ex.Message
				}, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// POST: /Customer/Create
		/// RESTful endpoint to create a new customer
		/// </summary>
		/// <param name="model">Customer data from JSON post</param>
		/// <returns>JSON result with new customer ID</returns>
		[HttpPost]
		public JsonResult Create(CustomerModel model)
		{
			try
			{
				var repo = CustomerFactory.Create();
				var customer = repo.GetNewCustomer();

				customer.FirstName = model.FirstName;
				customer.LastName = model.LastName;
				customer.CompanyName = model.CompanyName;
				
				if (model.Address != null)
				{
					customer.Address.Street = model.Address.Street;
					customer.Address.City = model.Address.City;
					customer.Address.State = model.Address.State;
					customer.Address.Zip = model.Address.Zip;
				}

				if (!customer.Save())
				{
					return Json(new
					{
						success = false,
						error = "Failed to save customer. Customer must have LastName and CompanyName."
					});
				}

				return Json(new
				{
					success = true,
					data = new { Id = customer.Id },
					message = "Customer created successfully"
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					error = ex.Message
				});
			}
		}

		/// <summary>
		/// POST: /Customer/Update
		/// RESTful endpoint to update an existing customer
		/// </summary>
		[HttpPost]
		public JsonResult Update(CustomerModel model)
		{
			try
			{
				if (model.Id <= 0)
				{
					return Json(new
					{
						success = false,
						error = "Invalid customer ID"
					});
				}

				var repo = CustomerFactory.Create();
				var customer = repo.GetCustomerById(model.Id);

				if (customer == null)
				{
					return Json(new
					{
						success = false,
						error = "Customer not found"
					});
				}

				customer.FirstName = model.FirstName;
				customer.LastName = model.LastName;
				customer.CompanyName = model.CompanyName;
				
				if (model.Address != null)
				{
					customer.Address.Street = model.Address.Street;
					customer.Address.City = model.Address.City;
					customer.Address.State = model.Address.State;
					customer.Address.Zip = model.Address.Zip;
				}

				if (!customer.Save())
				{
					return Json(new
					{
						success = false,
						error = "Failed to update customer"
					});
				}

				return Json(new
				{
					success = true,
					message = "Customer updated successfully"
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					error = ex.Message
				});
			}
		}

		/// <summary>
		/// POST: /Customer/Delete
		/// RESTful endpoint to delete a customer
		/// </summary>
		[HttpPost]
		public JsonResult Delete(long id)
		{
			try
			{
				var repo = CustomerFactory.Create();
				var customer = repo.GetCustomerById(id);

				if (customer == null)
				{
					return Json(new
					{
						success = false,
						error = "Customer not found"
					});
				}

				if (!customer.Delete())
				{
					return Json(new
					{
						success = false,
						error = "Failed to delete customer"
					});
				}

				return Json(new
				{
					success = true,
					message = "Customer deleted successfully"
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					error = ex.Message
				});
			}
		}
	}

	#region View Models

	/// <summary>
	/// Model for customer API requests
	/// </summary>
	public class CustomerModel
	{
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CompanyName { get; set; }
		public AddressModel Address { get; set; }
	}

	/// <summary>
	/// Model for address data in API requests
	/// </summary>
	public class AddressModel
	{
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
	}

	#endregion
}
