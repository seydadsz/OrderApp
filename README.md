# 🚀 OrderApp  
Simple and clean Order Management System built with **ASP.NET Core MVC** and **ADO.NET**.

This project demonstrates product management, customer management, price history tracking, email-based order confirmation, and a fully SQL-driven architecture without Entity Framework.

---

## 📦 Features
- Product Management (Unique StockCode)
- Customer Management (Unique CustomerCode)
- Product Prices with full history
- Order Creation & automatic OrderNumber
- Email Confirmation with security token
- ADO.NET database operations (no ORM)
- SQL Server relational schema

---

## 🛠 Technologies
- ASP.NET Core MVC  
- ADO.NET (SqlConnection, SqlCommand, SqlDataReader)  
- SQL Server  
- Bootstrap  
- SMTP (Gmail)

---

## 📂 Project Structure
OrderApp/
├── Controllers/
├── Models/
├── Repositories/
├── Services/
├── Views/
└── Database/OrderAppDB.sql


---

## 🗄 Database
The project includes a full database script:

📁 `Database/OrderAppDB.sql`

Contains:
- Products  
- Customers  
- ProductPrices  
- Orders  
- Keys, indexes, FK relations  

Run it in SQL Server to create the database.

---

## ✉ Email Configuration (Mandatory)
To enable email confirmation, update the following section inside `appsettings.json`:

json
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "EnableSSL": true,
  "UserName": "YOUR_GMAIL",
  "Password": "YOUR_APP_PASSWORD"
}

## ⚠ Email Requirement
Gmail requires a **Google App Password** in order to send emails from external applications.  
Without correct SMTP settings, the **order confirmation feature will not work**.

---

## 🌐 Recommended Browser
For the best local development experience, use **Microsoft Edge**.

Edge provides:
- More stable `localhost` behavior  
- Better handling of confirmation links  
- Fewer cookie and security warnings  

---

## 📘 Documentation
The project includes the following documentation files:

- **ProjectAnalysis.md** – Detailed project overview  
- **MissingQuestions.md** – Required clarification items  
- **UserGuide.md** – Simple usage guide  

All documentation files are located under the **/Docs** folder.






