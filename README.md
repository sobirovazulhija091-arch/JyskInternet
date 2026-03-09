# Internet_jysk

---- Small ASP.NET Core Web API project for internet shop practice.

It has 3 projects:
- `Domain` (entities, dtos, response models,filter,enum,seed)
- `Infrastructure` (services, interfaces, db context,response,migration)
- `WebJysk` (API controllers, Program.cs,Middlware,)
- `gitignore` 

## What is inside
- JWT auth (register/login)
- User, Product, Order, Category, Cart ,Brand ,Delivery ,Discount,Payment,Review,WarehouseProduct controllers
- Email service (test email, forgot/reset password)
- Swagger UI

## Requirements

- .NET SDK 9.0
- PostgreSQL

## Quick start

* Go to API project:
* cd WebJysk

* Run app:
 dotnet run

## Notes
- Some endpoints use `[Authorize]`, so get JWT token first from auth endpoints.
- Roles are seeded on app startup.

## Not Done YeT
--
