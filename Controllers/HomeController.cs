using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BT.Model.Customer;

namespace BankAccountApp.Controllers
{
	public class HomeController : Controller
	{
		// GET: Home
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Page(String id, string subView = "")
		{
			return PartialView($"Page.{id}");
		}

		[HttpGet]
        public JsonResult CustomerList()
        {
			var repo = CustomerFactory.CreateRepository();
			var customers = repo.GetList();
			var dto = customers.ConvertAll(c => new {
				Id = c.Id,
				FirstName = c.FirstName,
				LastName = c.LastName,
				CompanyName = c.CompanyName,
				Address = new {
					Street = c.Address?.Street,
					City = c.Address?.City,
					State = c.Address?.State,
					Zip = c.Address?.Zip,
					AddressBlock = c.Address?.AddressBlock()
				}
			});
			return Json(dto, JsonRequestBehavior.AllowGet);
        }
	}
}