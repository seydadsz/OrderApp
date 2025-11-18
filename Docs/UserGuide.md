# 📘 OrderApp – User Guide

This guide explains how to use the main features of the OrderApp system in a clear and simple way.

---

## 🏠 1. Home Page
When the application opens, you will see the main navigation menu:

- **Products**
- **Customers**
- **Product Prices**
- **Orders**

Use the menu to access the section you want.

---

## 📦 2. Products
### ➤ Add a New Product
1. Go to **Products → Create**  
2. Enter:
   - Stock Code (must be unique)
   - Stock Name  
3. Click **Save**

### ➤ View Products
- Go to **Products → List**
- All products will appear in a table

---

## 👥 3. Customers
### ➤ Add a New Customer
1. Go to **Customers → Create**
2. Enter:
   - Customer Code (must be unique)
   - Customer Name
   - Email Address (for order confirmation)
3. Click **Save**

### ➤ View Customers
- Go to **Customers → List**

---

## 💰 4. Product Prices
### ➤ Add Price to a Product
1. Go to **Product Prices → Create**
2. Select a product
3. Enter a new price
4. Click **Save**

Notes:
- The system keeps **all price history**
- Only the **latest price** can be edited

---

## 🛒 5. Creating an Order
1. Go to **Orders → Create**
2. Select a customer
3. Select a product
4. Quantity and unit price are shown automatically
5. Click **Create Order**

The system will:
- Generate a unique **Order Number**
- Send a **confirmation email** to the customer

---

## ✉ 6. Email Confirmation
After creating an order:

- The customer receives an email containing a confirmation link  
- When the customer clicks the link:
  - The order becomes **Confirmed**
  - The status updates in the system

If email settings are not configured correctly, the confirmation process will not work.

---

## 🌐 7. Recommended Browser
For best performance during local development, use **Microsoft Edge**.

Edge ensures:
- Stable `localhost` behavior  
- Proper handling of confirmation links  
- Fewer cookie and security warnings  

---

## ✔ 8. Summary
OrderApp allows you to:

- Manage products  
- Manage customers  
- Add product pricing  
- Create and confirm orders via email  

The system is simple and designed for fast and reliable order processing.

---

### ✍ Document Author  
**Şeyda Adsız**

