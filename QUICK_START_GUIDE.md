# Quick Start Guide - Customer Management System

## 🚀 Getting Started in 3 Minutes

### Step 1: Open the Project
```
1. Open Visual Studio 2017 or later
2. File → Open → Project/Solution
3. Navigate to /workspace/BankAccountApp.csproj
4. Click Open
```

### Step 2: Restore & Build
```
1. Right-click on the Solution → Restore NuGet Packages
2. Wait for packages to download (Dapper, SQLite, etc.)
3. Press F6 to build the solution
4. Ensure build succeeds with 0 errors
```

### Step 3: Run the Application
```
1. Press F5 to start debugging
2. Browser will open to http://localhost:54330
```

### Step 4: View Customer List
Navigate to:
```
http://localhost:54330/Home/Page?id=CustomerList
```

You should see:
- Beautiful customer list page
- Search/filter box
- Responsive table
- Loading spinner (first time only)

---

## 🧪 Running Unit Tests

### Visual Studio
```
1. Test → Windows → Test Explorer
2. Click "Run All" button
3. All 30+ tests should pass ✅
```

### Expected Results
- ✅ CustomerRepositoryTests: 20 tests passing
- ✅ AddressTests: 10 tests passing
- ✅ Total: ~30 tests, all green

---

## 📝 Testing the System

### 1. Create a Test Customer (Using API)

**Option A: Using Postman or similar tool**
```
POST http://localhost:54330/Customer/Create
Content-Type: application/json

{
  "FirstName": "John",
  "LastName": "Doe",
  "CompanyName": "Acme Corporation",
  "Address": {
    "Street": "123 Main Street",
    "City": "New York",
    "State": "NY",
    "Zip": "10001"
  }
}
```

**Option B: Using code**
```csharp
var repo = CustomerFactory.Create();
var customer = repo.GetNewCustomer();
customer.FirstName = "John";
customer.LastName = "Doe";
customer.CompanyName = "Acme Corp";
customer.Address.Street = "123 Main St";
customer.Address.City = "New York";
customer.Address.State = "NY";
customer.Address.Zip = "10001";
customer.Save();
```

### 2. View Customer List
```
Navigate to: http://localhost:54330/Home/Page?id=CustomerList
You should see your newly created customer!
```

### 3. Test Filtering
```
1. Type "John" in the search box
2. Results filter in real-time
3. Try typing city, state, company name - all work!
```

### 4. Test API Endpoints

**Get All Customers:**
```
GET http://localhost:54330/Customer/List
```

**Get Customer by ID:**
```
GET http://localhost:54330/Customer/GetById?id=1
```

---

## 🐛 Troubleshooting

### Issue: "The type or namespace name 'BT' could not be found"
**Solution:** The AccountData files are in the workspace root, but they use the `BT.Model.AccountData` namespace. You may need to:
1. Check that all `.cs` files are included in the project
2. Or adjust the namespace to match your project structure

### Issue: "Could not load file or assembly 'Dapper'"
**Solution:** 
```
1. Right-click Solution → Restore NuGet Packages
2. If that doesn't work: Delete packages folder and restore again
3. Rebuild solution
```

### Issue: "Cannot find CustomerDB.db"
**Solution:** 
```
The database is auto-created on first use.
Check App_Data folder after first run.
If issues persist, check Web.config connection string.
```

### Issue: "Unit tests fail to compile"
**Solution:**
```
1. Ensure Testing.BTModel project references BT.Model (or BankAccountApp) project
2. Check that all test files are included in Testing.BTModel.csproj
3. Rebuild test project
```

---

## 📖 Understanding the Architecture

### How to Create a Customer
```csharp
// 1. Get repository from factory (ONLY way to access customers)
var repo = CustomerFactory.Create();

// 2. Get a new empty customer
var customer = repo.GetNewCustomer();

// 3. Set properties
customer.FirstName = "Jane";
customer.LastName = "Smith";        // Required
customer.CompanyName = "Tech Inc";  // Required
customer.Address.Street = "456 Oak Ave";
customer.Address.City = "Austin";
customer.Address.State = "TX";
customer.Address.Zip = "78701";

// 4. Save to database
bool success = customer.Save(); // Returns true if successful
```

### How to Get Customers
```csharp
var repo = CustomerFactory.Create();

// Get all customers
var allCustomers = repo.GetList();
foreach (var c in allCustomers)
{
    Console.WriteLine($"{c.FirstName} {c.LastName} - {c.CompanyName}");
    Console.WriteLine(c.Address.AddressBlock()); // Formatted address
}

// Get specific customer
var customer = repo.GetCustomerById(1);
if (customer != null)
{
    Console.WriteLine($"Found: {customer.FirstName} {customer.LastName}");
}
```

### How to Update a Customer
```csharp
var repo = CustomerFactory.Create();
var customer = repo.GetCustomerById(1);

customer.CompanyName = "New Company Name";
customer.Address.City = "Los Angeles";
customer.Save(); // Updates existing record
```

### How to Delete a Customer
```csharp
var repo = CustomerFactory.Create();
var customer = repo.GetCustomerById(1);

customer.Delete(); // Removes from database
```

---

## 🎯 What You Can't Do (By Design)

### ❌ Cannot Create Customer Directly
```csharp
// This will NOT compile:
var customer = new Customer(); // Error: Customer is inaccessible
```
**Why?** Proper encapsulation - must use factory

### ❌ Cannot Save Invalid Customer
```csharp
var repo = CustomerFactory.Create();
var customer = repo.GetNewCustomer();
customer.FirstName = "John";
// Missing LastName and CompanyName
customer.Save(); // Throws InvalidOperationException
```
**Why?** Business rule: Must have LastName AND CompanyName

### ❌ Cannot Access DTOs Directly
```csharp
// This will NOT compile:
var dto = new CustomerDto(); // Error: CustomerDto is internal
```
**Why?** DTOs are implementation details, hidden from consumers

---

## 📊 Sample Data

### To Add Sample Customers
Run this code in your application:
```csharp
var repo = CustomerFactory.Create();

// Customer 1
var c1 = repo.GetNewCustomer();
c1.FirstName = "John";
c1.LastName = "Doe";
c1.CompanyName = "Acme Corporation";
c1.Address.Street = "123 Main Street";
c1.Address.City = "New York";
c1.Address.State = "NY";
c1.Address.Zip = "10001";
c1.Save();

// Customer 2
var c2 = repo.GetNewCustomer();
c2.FirstName = "Jane";
c2.LastName = "Smith";
c2.CompanyName = "TechStart Inc";
c2.Address.Street = "456 Tech Boulevard";
c2.Address.City = "San Francisco";
c2.Address.State = "CA";
c2.Address.Zip = "94102";
c2.Save();

// Customer 3
var c3 = repo.GetNewCustomer();
c3.FirstName = "Bob";
c3.LastName = "Johnson";
c3.CompanyName = "Global Solutions LLC";
c3.Address.Street = "789 Business Park Drive";
c3.Address.City = "Austin";
c3.Address.State = "TX";
c3.Address.Zip = "78701";
c3.Save();
```

Then refresh the customer list page to see them!

---

## 🎓 Key Concepts to Understand

### 1. Factory Pattern
```
CustomerFactory → Creates repository → Creates customers
(Public)           (Interface)         (Internal)
```

### 2. Encapsulation
- Customer/Address classes are **internal** (hidden)
- Only **interfaces** (ICustomer, IAddress) are exposed
- Consumers can't break business rules

### 3. Repository Pattern
- Centralizes all data operations
- Clean API for consumers
- Hides database complexity

### 4. Dependency Injection
- Repository accepts ICustomerDataAccess
- Easy to swap implementations
- Perfect for testing

### 5. DTO Pattern
- CustomerDto/AddressDto map to database
- Separate from business objects
- Clean separation of concerns

---

## ✅ Success Checklist

After following this guide, you should be able to:
- [x] Build the solution without errors
- [x] Run all unit tests (30+ tests passing)
- [x] View the customer list page
- [x] Create customers via code or API
- [x] See customers in the responsive UI
- [x] Filter customers by any field
- [x] Understand the architecture
- [x] Modify and extend the code

---

## 🎉 You're All Set!

The system is fully functional and ready for demonstration. 

**Next Steps:**
- Review `PROJECT_SUMMARY.md` for detailed architecture documentation
- Explore the code - it's heavily commented
- Run the unit tests to see comprehensive test coverage
- Try the responsive design on different screen sizes

**Questions?**
- Check inline code comments (every class/method documented)
- Review unit tests for usage examples
- Read PROJECT_SUMMARY.md for design decisions

---

## 📞 Quick Reference

**Customer List Page:**
```
http://localhost:54330/Home/Page?id=CustomerList
```

**API Endpoints:**
```
GET  /Customer/List          - Get all customers
GET  /Customer/GetById?id=1  - Get customer by ID
POST /Customer/Create        - Create customer
POST /Customer/Update        - Update customer
POST /Customer/Delete        - Delete customer
```

**Connection String Location:**
```
/workspace/Web.config
<connectionStrings> section
```

**Database Location:**
```
App_Data/CustomerDB.db
(Auto-created on first run)
```

**Test Project:**
```
/workspace/Testing.BTModel/
Run via Test Explorer in Visual Studio
```

---

**Happy Coding! 🚀**
