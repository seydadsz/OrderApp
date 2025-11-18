# 📘 OrderApp – Project Analysis Document  
**Version:** 1.0  
**Author:** Şeyda Adsız  
**Technology:** ASP.NET Core MVC, ADO.NET, SQL Server

---

# 1. 🎯 Project Overview

OrderApp is a lightweight Order Management System built with **ASP.NET Core MVC** and **ADO.NET** without Entity Framework.  
The project demonstrates:

- Backend development skills  
- Layered architecture  
- Clean code practices  
- SQL database design  
- Real business workflows (pricing, ordering, email confirmation)  
- Basic validation and token-based confirmation  

The system includes:

- **Product Management**  
- **Customer Management**  
- **Product Pricing (history)**  
- **Order Creation**  
- **Email Confirmation Flow**

---

# 2. 📦 Project Scope

## ✔ Product Management
- Create / list products  
- `StockCode` is unique  
- Simple and clean product structure  

## ✔ Customer Management
- Create / list customers  
- `CustomerCode` is unique  
- Customers with orders cannot be deleted  

## ✔ Product Pricing Module
- Add unlimited price entries  
- Full price history  
- Only latest price is editable  
- Older prices remain read-only  

## ✔ Order Management
- Select customer & product  
- Auto-generate OrderNumber  
- Fetch latest product price  
- Send email with ConfirmationToken  
- Customer confirms via link  
- Order becomes **IsConfirmed = 1**

---

# 3. 🏛 System Architecture

## 3.1 Folder / Layer Structure

OrderApp/
│
├── Controllers/
├── Repositories/
├── Services/
├── Models/
├── Views/
├── wwwroot/
└── appsettings.json


## 3.2 Responsibilities by Layer

| Layer | Responsibility |
|-------|---------------|
| **Controllers** | Handles requests and controls flow |
| **Repositories (ADO.NET)** | All SQL queries & CRUD operations |
| **Services** | Email sending & external processes |
| **Models** | Entities, DTOs, ViewModels |
| **Views** | Razor-based UI |

**Note:**  
No ORM (EF Core) is used. All DB operations rely on **SqlConnection**, **SqlCommand**, **SqlDataReader**.

---

# 4. 🗄 Database Design

## 4.1 Tables

| Table | Description |
|--------|-------------|
| **Products** | Product information |
| **Customers** | Customer details |
| **ProductPrices** | Price history per product |
| **Orders** | Customer orders |

## 4.2 Relationships

Customer (1) ─── (N) Orders
Product (1) ─── (N) Orders
Product (1) ─── (N) ProductPrices


### Key Notes
- A product may have multiple price revisions  
- A customer may place multiple orders  
- Every order references exactly one product and one customer  

---

# 5. 🔄 Business Workflows

## 5.1 Order Creation Flow
1. User selects customer & product  
2. System generates unique **OrderNumber**  
3. Latest product price is selected  
4. ConfirmationToken is generated  
5. Email is sent to customer  
6. Customer clicks confirmation URL  
7. Order becomes **IsConfirmed = 1**  

## 5.2 Email Confirmation Workflow
- Token is embedded into a secure confirmation link  
- SMTP sends a simple HTML email  
- Unauthorized confirmation attempts are blocked  

## 5.3 Product Pricing Flow
- User adds new price  
- `ValidFrom = GETDATE()` automatically  
- Older prices stay unchanged  
- Latest price is editable  

---

# 6. ⚙ Technical Requirements

- ASP.NET Core MVC  
- C# / ADO.NET  
- SQL Server  
- Bootstrap 5  
- SMTP (Gmail or custom SMTP server)  
- No Entity Framework (per requirement)  

---

# 7. 🚧 Project Limitations

Current limitations:

- No authentication / authorization  
- No inventory tracking  
- No PDF/Excel export  
- No REST API  
- No multicurrency support  
- No user roles  
- No central logging system  

These can be added easily in future development.

---

# ✍ Author  
**Şeyda Adsız**  
OrderApp – Software Developer Candidate



