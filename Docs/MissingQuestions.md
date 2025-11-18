# ❓ Missing Information – Question List  
This document lists the questions and clarifications required from the project owner to fully define the OrderApp requirements.

---

## 1. 📨 Email & SMTP Configuration (Gmail Integration)

1. Which SMTP provider will be used for sending order confirmation emails?  
   - Gmail  
   - Corporate SMTP  
   - Office365  
   - Other (please specify)

2. If **Gmail** will be used, will each deployment environment use its **own Gmail account and App Password**?

3. The application expects the following configuration in `appsettings.json`:

   - Host  
   - Port  
   - EnableSSL  
   - UserName (sender email address)  
   - Password (App Password or SMTP password)

   Without valid values, the order confirmation process (email + token) will **not work**.  
   Can you provide the final SMTP configuration details for production?

4. Should Gmail **App Password** be used, or will a dedicated corporate SMTP user be provided?

5. Security clarification:  
   - Should we store SMTP credentials using user secrets / environment variables instead of committing them to the repository?


## 2. 🌐 Localhost & Browser Compatibility

1. During development, the application has been tested primarily on **Microsoft Edge** and works most reliably there.  
   - Do you have an official browser support requirement (Edge, Chrome, Firefox, etc.)?

2. Do we need to guarantee full compatibility with all major browsers, or is Edge support sufficient for internal use?

**Note:** For local development, Microsoft Edge has shown:  
- More stable handling of `https://localhost`  
- Fewer issues with token-based confirmation links  
- Better behavior with MVC routing and cookies
