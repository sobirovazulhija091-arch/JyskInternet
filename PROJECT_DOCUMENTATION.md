# Internet JYSK – Project Documentation

A full-stack e-commerce application for an internet shop, inspired by JYSK. Built with .NET 9, Blazor WebAssembly, and PostgreSQL.

---

## Table of Contents

1. [Project Overview](#1-project-overview)
2. [Architecture](#2-architecture)
3. [Solution Structure](#3-solution-structure)
4. [Backend (WebJysk API)](#4-backend-webjysk-api)
5. [Domain Layer](#5-domain-layer)
6. [Infrastructure Layer](#6-infrastructure-layer)
7. [WebJysk.Admin](#7-webjyskadmin)
8. [WebJysk.Marketplace](#8-webjyskmarketplace)
9. [Configuration](#9-configuration)
10. [Authentication & Authorization](#10-authentication--authorization)
11. [Key Features](#11-key-features)
12. [Quick Start](#12-quick-start)
13. [Known Issues](#13-known-issues)

---

## 1. Project Overview

| Aspect | Details |
|--------|---------|
| **Type** | E-commerce / Internet shop |
| **Target** | .NET 9 |
| **Database** | PostgreSQL |
| **Auth** | JWT Bearer + ASP.NET Core Identity |
| **Frontends** | Blazor WebAssembly (Admin + Marketplace) |

### Components

| Component | Technology | Purpose |
|-----------|------------|---------|
| **WebJysk** | ASP.NET Core Web API | REST API backend |
| **WebJysk.Admin** | Blazor WASM | Admin panel for management |
| **WebJysk.Marketplace** | Blazor WASM | Customer-facing store |
| **Domain** | Class Library | Entities, DTOs, enums, filters |
| **Infrastructure** | Class Library | DbContext, services, migrations |

---

## 2. Architecture

```
┌─────────────────────┐     ┌─────────────────────┐
│  WebJysk.Admin      │     │  WebJysk.Marketplace │
│  (Blazor WASM)      │     │  (Blazor WASM)       │
│  localhost:5271     │     │  localhost:5211      │
└──────────┬──────────┘     └──────────┬──────────┘
           │                           │
           │    HTTP + JWT             │
           └─────────────┬─────────────┘
                         │
                         ▼
           ┌─────────────────────────┐
           │  WebJysk API            │
           │  localhost:5244         │
           │  (ASP.NET Core)         │
           └────────────┬────────────┘
                        │
           ┌────────────┼────────────┐
           │            │            │
           ▼            ▼            ▼
     ┌──────────┐ ┌──────────┐ ┌──────────┐
     │ Domain   │ │ Infra    │ │PostgreSQL│
     └──────────┘ └──────────┘ └──────────┘
```

---

## 3. Solution Structure

### Solutions

- **Internet_jysk.sln** – Full solution (Domain, Infrastructure, WebJysk.Admin, WebJysk.Marketplace, WebJysk.Client*)
- **Web.sln** – Backend only (Domain, Infrastructure, WebJysk)

> \* WebJysk.Client is referenced in Internet_jysk.sln but the project folder is missing.

### Folder Layout

```
Internet_jysk/
├── Domain/                 # Entities, DTOs, Enums, Filters
├── Infrastructure/         # DbContext, Services, Migrations
├── WebJysk/               # REST API
├── WebJysk.Admin/         # Admin Blazor app
├── WebJysk.Marketplace/   # Store Blazor app
├── Internet_jysk.sln
├── Web.sln
└── README.md
```

---

## 4. Backend (WebJysk API)

**Base URL:** `http://localhost:5244`

### Controllers & Endpoints

| Controller | Route | Auth | Endpoints |
|------------|-------|------|-----------|
| **JwtUserController** | `api/auth` | Mixed | POST register, login, logout; GET me |
| **ProductController** | `api/product` | Admin/Public | CRUD, image upload, filter, search, sort, top-selling, reviews |
| **OrderController** | `api/order` | User/Admin | POST create, GET, GET my-orders, GET {id}, PUT {id}/status, DELETE |
| **CartController** | `api/cart` | User | Add item, update quantity, remove, delete cart, get cart |
| **CategoryController** | `api/category` | Admin/Public | CRUD, image upload, subcategories |
| **BrandController** | `api/brand` | Admin/Public | CRUD |
| **UserController** | `api/user` | Admin | List, get by id, delete |
| **StoreController** | `api/store` | Admin/Public | CRUD |
| **WarehouseController** | `api/warehouse` | Admin | CRUD, add/remove stock |
| **ReviewController** | `api/review` | User/Public | Add review, get reviews, average rating |
| **PaymentController** | `api/payment` | Admin | Create payment, update status |
| **DeliverController** | `api/deliver` | Admin | Create delivery, update status |
| **DescountController** | `api/descount` | Admin/User | Create, validate, apply discount |
| **EmailController** | `api/email` | Admin/Public | Send test, forgot/reset password |

### API Documentation

- Swagger UI: `https://localhost:7197/swagger` (or HTTP equivalent)

---

## 5. Domain Layer

### Entities

| Entity | Description |
|--------|-------------|
| **Product** | Name, Description, Price, DiscountPrice, StockQuantity, ImageUrl, CategoryId, BrandId, IsActive |
| **Category** | Hierarchical (Parent/Children) |
| **Brand** | Brand name and details |
| **User** | Extends IdentityUser; FullName, Phone, CreatedAtUtc |
| **Cart** / **CartItem** | User cart |
| **Order** / **OrderItem** | Orders and line items |
| **Store** | Store locations |
| **Warehouse** / **WarehouseProduct** | Inventory |
| **Payment** | Payment records |
| **Delivery** | Delivery tracking |
| **Review** | Product reviews |
| **Discount** | Discount codes |

### DTOs (Data Transfer Objects)

| Area | DTOs |
|------|------|
| Product | ProductDto, UpdateProductDto |
| Order | OrderDto, OrderItemDto, UpdateOrderDto, UpdateOrderStatusDto |
| Cart | CartDto, CartItemDto, UpdateCartDto, UpdateCartItemDto |
| User | UserDto, UpdateUserDto, RegisterDto, LoginDto |
| Category | CategoryDto, UpdateCategoryDto |
| Brand | BrandDto |
| Store | StoreDto, UpdateStoreDto |
| Warehouse | WarehouseDto, AddStockDto, DecrStockDto |
| Misc | ReviewDto, DiscountDto, ForgetPasswordDto, ResetPasswordDto |

### Enums

| Enum | Values |
|------|--------|
| **EnumStatus** | Paid, Processing, Delivered, Cancelled |
| **EnumPaymentMethod** | Cash, Card, Online |
| **EnumPaymentStatus** | Pending, Paid, Failed, Refunded |
| **EnumDeliveryType** | Courier, Pickup |
| **EnumDeliveryStatus** | Preparing, Shipped, Delivered |

### Filters

- FilterProduct, FilterUser, FilterOrder, FilterReview (for paged queries)

---

## 6. Infrastructure Layer

### ApplicationDbContext

- Extends `IdentityDbContext<User>`
- DbSets for all entities
- Relationships and cascade rules configured
- **Database:** PostgreSQL via Npgsql

### Services (Interfaces → Implementations)

| Service | Purpose |
|---------|---------|
| UserService | User CRUD |
| OrderService | Order CRUD, create with items, status updates |
| ProductService | Product CRUD, filter, search, sort |
| CategoryServce | Category CRUD, hierarchy |
| BrandService | Brand CRUD |
| CartService | Cart operations (server-side cart) |
| StoreService | Store CRUD |
| WarehouseService | Warehouse and stock management |
| DeliveryService | Delivery CRUD |
| PaymentSevice | Payment CRUD |
| DiscountService | Discount codes |
| ReviewService | Reviews and ratings |
| JwtService | JWT token generation |
| EmailService | Email sending |
| EmailWorker | Background email worker |

### Response Models

- **Response&lt;T&gt;** – StatusCode, Description, Data
- **PagedResult&lt;T&gt;** – Items, Page, PageSize, TotalCount, TotalPages

### Seed Data

- Roles: Admin, User
- Default admin: `admin@jysk.local` / `Admin1234`
- CatalogSeed: Brands, categories, sample products

---

## 7. WebJysk.Admin

**URL:** `http://localhost:5271`

### Pages & Routes

| Route | Page | Purpose |
|-------|------|---------|
| `/` | Dashboard | KPIs (products, orders, users, revenue) |
| `/login` | Login | Admin sign-in (empty layout) |
| `/products` | Products | Product CRUD |
| `/categories` | Categories | Category management |
| `/brands` | Brands | Brand management |
| `/orders` | Orders | Order list and status updates |
| `/users` | Users | User management |

### Layouts

- **AdminLayout** – Sidebar, top bar, logout
- **EmptyLayout** – Login page
- **MainLayout** – Default

### Services

- **AuthService** – Login, logout, token in localStorage
- **ApiClient** – Attaches Bearer token to API requests
- **ProductApiService**, **CategoryApiService**, **BrandApiService**, **OrderApiService**, **UserApiService**

### Auth Flow

1. Login at `/login` → `POST api/auth/login`
2. Token stored in `localStorage` (key: `authToken`)
3. `AuthRedirect` sends unauthenticated users to `/login`
4. ApiClient sends Bearer token for protected calls

---

## 8. WebJysk.Marketplace

**URL:** `http://localhost:5211` (typical)

### Pages & Routes

| Route | Page | Purpose |
|-------|------|---------|
| `/` | Welcome | Landing / entry |
| `/home` | Index | Home page |
| `/products` | Products | Product catalog |
| `/product/{id}` | ProductDetail | Product details, add to cart |
| `/categories` | Categories | Browse by category |
| `/cart` | Cart | Cart view (requires login) |
| `/checkout` | Checkout | Place order (requires login) |
| `/checkout/success` | CheckoutSuccess | Order confirmation |
| `/login` | Login | Customer login |
| `/about` | About | About page |
| `/delivery` | Delivery | Delivery information |

### Components

- **ProductCard** – Product display
- **HeroCarousel** – Home hero
- **CatalogDropdown**, **MoreDropdown** – Navigation

### Services

- **AuthService** – Login, logout, guest mode, IsAdminAsync, OnAuthChanged
- **ApiClient** – Bearer token for authenticated requests
- **CartService** – Local cart in localStorage (key: `jysk_cart`)
- **ProductApiService**, **CategoryApiService**, **BrandApiService**, **OrderApiService**

### Cart & Checkout Flow

1. Cart stored in **localStorage** (client-only, no API cart sync)
2. Login required for Cart and Checkout
3. Checkout: delivery address, payment method
4. `POST api/order/create` with JWT
5. Cart cleared, redirect to `/checkout/success`

### Guest Mode

- "Continue without login" → guest flag in localStorage
- Guests cannot use cart or checkout
- Admin Panel link shown only for admin users

---

## 9. Configuration

### WebJysk (API) – appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=InternetShop;Username=postgres;Password=1234"
  },
  "Jwt": {
    "Key": "THIS_IS_A_32_CHAR_SECRET_KEY_123456",
    "Issuer": "ShopInternet",
    "Audience": "ShopInternet"
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Email": "...",
    "Password": "..."
  }
}
```

### Frontend API Base

- **Admin:** `ApiBaseUrl` from config, default `http://localhost:5244`
- **Marketplace:** Same pattern

### CORS

- Policy: `AllowBlazor` – AllowAnyOrigin, AllowAnyHeader, AllowAnyMethod

---

## 10. Authentication & Authorization

### JWT

- Scheme: JwtBearer
- Claims: Sub (UserId), Email, Name, Role
- Expiry: 3 hours
- ClockSkew: 30 seconds

### Roles

| Role | Usage |
|------|-------|
| **Admin** | Admin panel, product/category/brand CRUD, orders, users |
| **User** | Cart, checkout, orders, reviews |

### Password Policy

- Min length: 8
- Digit required
- Lockout: 5 failed attempts → 10 minutes

---

## 11. Key Features

| Feature | API | Admin | Marketplace |
|---------|-----|-------|-------------|
| JWT Auth | Yes | Yes | Yes |
| Products | CRUD, filter, search | Manage | Browse, detail |
| Categories | CRUD, hierarchy | Manage | Browse |
| Brands | CRUD | Manage | Browse |
| Orders | Create, list, status | Manage | Create, checkout |
| Cart | API (User) | — | Local (localStorage) |
| Users | List, delete | Manage | Register, login |
| Reviews | Add, list, avg | — | — |
| Stores | CRUD | — | — |
| Warehouses | CRUD, stock | — | — |
| Payments | Create, status | — | — |
| Delivery | Create, status | — | — |
| Discounts | Create, validate | — | — |
| Email | Forgot/reset | — | — |

---

## 12. Quick Start

### Requirements

- .NET 9 SDK
- PostgreSQL

### Run API

```bash
cd WebJysk
dotnet run
```

API: `http://localhost:5244`

### Run Admin

```bash
cd WebJysk.Admin
dotnet run
```

Admin: `http://localhost:5271`  
Login: `admin@jysk.local` / `Admin1234`

### Run Marketplace

```bash
cd WebJysk.Marketplace
dotnet run
```

Marketplace: `http://localhost:5211`

### Database

1. Create PostgreSQL database `InternetShop`
2. Update connection string in `WebJysk/appsettings.json`
3. Apply migrations: `dotnet ef database update` (from WebJysk)

---

## 13. Known Issues

1. **Internet_jysk.sln** references `WebJysk.Client`, which does not exist → build may fail
2. **Cart** in Marketplace is local (localStorage); server Cart API is for a different flow
3. **Admin** and **Marketplace** run on different origins; separate login for each

---

*Document generated for Internet JYSK project.*
