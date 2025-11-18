# 📌 Gmail Integration – Detailed Project Explanation

In OrderApp, email delivery is **mandatory** because order confirmation requires sending a token-based confirmation link to the customer.  
To enable this, the user must provide SMTP credentials for their own Gmail or corporate email account.

---

# ✔ Required Email Settings (Mandatory for System Operation)

The following configuration must be manually added to `appsettings.json`:

```json
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "EnableSSL": true,
  "UserName": "USER_GMAIL_ADDRESS",
  "Password": "GMAIL_APP_PASSWORD"
}

# 🌐 Localhost Browser Compatibility Notice

During development and testing, the application runs best on **Microsoft Edge**.

## ✅ Advantages with Microsoft Edge

- ✔ Email confirmation links open correctly
- ✔ HTTPS / localhost cookie handling is more stable  
- ✔ MVC routing behaves more consistently
- ✔ Debugging works faster and smoother

## ⚠️ Notes on Other Browsers

Other browsers (Chrome, Firefox) work as well but may cause:

- Token link not opening properly
- Mixed-content warnings  
- Localhost security restrictions

## 💡 Recommendation

**Recommended browser for running OrderApp locally: Microsoft Edge**
