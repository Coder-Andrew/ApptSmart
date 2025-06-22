# ApptSmart

**ApptSmart** is a full-stack appointment scheduling platform tailored for small businesses to manage availability and client bookings efficiently. Built with a focus on security, modularity, and modern development best practices, ApptSmart provides a responsive and scalable solution for both business owners and customers.

---
## ⚠️ Disclaimer

This project is currently in active development and is intended for demonstration and educational purposes only.

ApptSmart is not production-ready and should not be used to store or manage real user data in its current state.

---

## Tech Stack

### **Frontend**

- [Next.js](https://nextjs.org/) (App Router)
- TypeScript
- API proxy routing via Next.js API handlers

### **Backend**

- .NET 8 Web API
- ASP.NET Identity for authentication
- JWT stored in HTTP-only cookies
- SQL Server (two databases: Identity + Application)
- Entity Framework Core (with migrations and seeding)
- Swagger for API documentation and testing

### **Infrastructure & DevOps**

- Docker (optional production setup)
- .NET User Secrets for secure local development
- CSRF protection using custom anti-forgery token validation

---

## Security Features

- JWT authentication using HTTP-only, secure cookies
- CSRF protection with token in both cookie and request header
- Role-based authorization for endpoints
- Secure password handling with ASP.NET Identity
- HTTPS recommended for deployment (especially with cookies)

---

## 🛠️ Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Node.js](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Docker](https://www.docker.com/) (optional)

### Backend Setup

```bash
cd apptsmart-backend

# Initialize and set user secrets
dotnet user-secrets init

# Add secrets (see .env section below for keys)
dotnet user-secrets set "ConnectionStrings:AuthConnection" "<auth-conn-string>"
dotnet user-secrets set "ConnectionStrings:AppConnection" "<app-conn-string>"
dotnet user-secrets set "Jwt:Secret" "<jwt-secret>"
dotnet user-secrets set "Jwt:ExpiryMinutes" "<jwt-expiry-time-minutes>"
dotnet user-secrets set "Jwt:ExpiryRefreshMinutes" "<refresh-token-expiry-time-minutes>"
dotnet user-secrets set "SeedUserPassword" "<password-for-seeded-users>"
dotnet user-secrets set "Cors:AllowedSites:<number>" "<allowed-site-url>"


# Run EF migrations
dotnet ef database update --context AuthDbContext
dotnet ef database update --context AppDbContext

# Run the API server
dotnet run
```

### Frontend Setup

```bash
cd apptsmart-frontend

# Install dependencies
npm install

# Add environment variables
touch .env.local
# (Fill in env vars below)

# Start the development server
npm run dev
```

---

## 🛋️ Environment Variables

### Frontend `.env.local`

```env
BACKEND_URL=<backend-url>
NEXT_PUBLIC_BASE_PATH=<base-path (defaulted to /app1)>
```


> **Note**: Ensure `JwtSettings:Secret` is long and random for production environments.

---

## 📂 Project Structure

```
apptsmart-backend/
├── Controllers/
├── DAL/
├── Models/
├── Services/
├── Utilities/
└── Program.cs

apptsmart-frontend/
├── app/
├── components/
├── lib/
├── utilities/
└── middleware.ts
```

---

## 🔖 API Documentation

Once the backend is running, you can access the Swagger UI at:

```
http://localhost:5000/swagger
```

Use this for exploring, testing, and debugging your API endpoints.

---

## 📌 Roadmap

- User account management
- Owner/admin management
- Email integration
- Google Calendar integration

---

## 🧑‍💻 Author

**Coder-Andrew**\
[LinkedIn](https://www.linkedin.com/in/andrew-e-84229b27a) • [Portfolio](https://andrewjesch.com) • [GitHub](https://github.com/Coder-Andrew)


