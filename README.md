## 🛍️ MarketPlace - Backend API

**MarketPlace** is a scalable and maintainable e-commerce backend solution built using **ASP.NET Core Web API**, designed to support small vendors in listing and selling products online. It includes robust authentication, role-based authorization, and follows a clean **N-Tier Architecture** to promote modularity and maintainability.

---

### 🔧 Tech Stack

- **Framework:** ASP.NET Core Web API (.NET 8)
- **Authentication:** JWT-based authentication 
- **Authorization:** Role-based access control (Admin, Vendor, Customer)   
- **Architecture:** N-Tier (API, BLL, DAL) 
- **Database:** SQL Server 
- **ORM:** Entity Framework Core

---

### 👥 User Roles

- **Admin:** Manages vendors and products
- **Vendor:** Manages own product listings
- **Customer:** Browses and purchases products

---

### 📦 Core Features

#### 🔐 Authentication & Authorization

- Secure login/logout functionality for all roles
- Role-based access to APIs (Admin/Vendor/Customer) 

#### 🧑‍💼 Admin
- Approve/reject vendor accounts 
- Enable/disable vendor features or permissions
- Approve/reject vendor product posts
- Allow auto-approval for specific vendors
    
#### 🛒 Customer
- View products without logging in  
- Search by category or price  
- Add items to cart and purchase (1 or more units)  
- Cannot purchase out-of-stock items
    
#### 🛍️ Vendor
- Full product management (CRUD) 
- Update available units
- View purchase history for each product

---
