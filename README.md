# 💻 TT E-Commerce — Internship Project

> A multi-portal laptop e-commerce platform built during a software engineering internship at GNN Technology Solutions. Features separate customer-facing and admin portals with role-based access control.

---

## 🏢 Context

This project was developed during my **Software Engineering Internship** at **Công ty TNHH Giải Pháp Công Nghệ Phần Mềm GNN** (Sep 2024 – Nov 2024), as part of a team of 5 developers. I was responsible for the backend authentication system, RBAC implementation, and admin dashboard modules.

---

## ✨ Features

- 🔐 **User Authentication** — Secure login/registration with session management
- 👥 **Role-Based Access Control (RBAC)** — Separate permissions for customers, staff, and admins
- 🛍️ **Product Management** — Admin dashboard for inventory, pricing, and product listings
- 🛒 **Shopping Flow** — Product catalog, cart, and order processing for customers
- 📦 **Order Management** — Track, update, and manage orders from the admin panel
- 🔄 **Version Control Workflow** — Developed with Git branching and code review practices

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend Framework | ASP.NET Core (C#) |
| Frontend | HTML5, CSS3, JavaScript, SCSS |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Auth | ASP.NET Identity |
| Containerization | Docker |
| Version Control | Git / GitHub |

---

## 📁 Project Structure

```
TT_ECommerce/
├── Areas/
│   └── Admin/          # Admin portal (controllers, views)
├── Controllers/         # Customer-facing controllers
├── Models/              # Entity models
├── Data/                # DbContext and seed data
├── Services/            # Business logic layer
├── Views/               # Razor views
├── Components/          # Reusable view components
├── Utils/               # Helper utilities
├── Migrations/          # EF Core database migrations
├── wwwroot/             # Static assets (CSS, JS, images)
├── Dockerfile
└── Program.cs
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Docker (optional)

### Installation

```bash
# 1. Clone the repository
git clone https://github.com/TTTai12/Thuc_Tap.git
cd Thuc_Tap

# 2. Configure the connection string in appsettings.json
# Update "DefaultConnection" to point to your SQL Server instance

# 3. Apply database migrations
dotnet ef database update

# 4. Run the application
dotnet run
```

### Using Docker

```bash
docker build -t tt-ecommerce .
docker run -p 8080:80 tt-ecommerce
```

---

## 🔑 Default Roles

| Role | Access |
|------|--------|
| Admin | Full access to admin dashboard, user management, orders |
| Staff | Product and inventory management |
| Customer | Browse products, place orders, view order history |

---

## 📚 What I Learned

- Implementing RBAC from scratch using ASP.NET Identity
- MVC architecture patterns in a production-like environment
- Collaborating in a team using Git branching strategies
- Agile development workflow and code review practices
- Debugging real-world issues in order processing flows

---

## 👤 Author

**Nguyễn Tất Thành** — [github.com/TTTai12](https://github.com/TTTai12)

> *Internship project — Sep 2024 to Nov 2024*
