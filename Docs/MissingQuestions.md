# ❓ Missing Information – Critical Questions  
This document includes only the required clarifications for **Email (SMTP/Gmail) integration** and **Browser compatibility** for running the OrderApp system.

---

## 📨 1. Email & SMTP (Gmail) Configuration Requirements

OrderApp uses **email confirmation** to validate orders.  
For this reason, correct SMTP information must be provided by the user.

### ✔ Mandatory Values for appsettings.json

The following information must be supplied by the project owner:

- SMTP Host  
- Port  
- EnableSSL (true/false)  
- Gmail Address (UserName)  
- Gmail App Password (Password)

Example structure:

```json
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "EnableSSL": true,
  "UserName": "USER_GMAIL_ADDRESS",
  "Password": "GMAIL_APP_PASSWORD"
}

### ✔ Why This Information Is Required

- Gmail requires a **personal App Password** for sending emails.  
- Without correct SMTP credentials:
  - Email sending will fail  
  - The confirmation link will not reach the user  
  - Orders will **never get confirmed**

### ❓ Clarification Required From Project Owner

**Will the system use Gmail or a corporate SMTP server?**  
- If Gmail → User must generate an App Password and enter it manually.  
- If corporate SMTP → Details must be shared (host, port, username, password).

---

## 🌐 2. Browser Compatibility (Localhost Behavior)

During development, the project has been tested on several browsers.

### ✔ Most Stable Browser: **Microsoft Edge**

Edge provides:

- More stable `https://localhost` handling  
- Better compatibility with MVC routing  
- Fewer cookie/security warnings  
- More consistent behavior for token-based email confirmation links  

### ❓ Clarification Required From Project Owner

**Is there an official browser requirement for the system?**

Options may include:

- Microsoft Edge (recommended for development)
- Google Chrome
- Mozilla Firefox
- Company-specific browser policy

If cross-browser support is required, this must be confirmed.

---

### ✍ Document Author  
**Şeyda Adsız**  


