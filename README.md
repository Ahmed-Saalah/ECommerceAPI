# E-Commerce API

This project is a modern E-Commerce Web API built using **ASP.NET Core**, adhering to **Clean Architecture** principles and **SOLID** design patterns. The API provides a robust backend solution for e-commerce platforms with integrated payment functionality using **Stripe**.

---

## Features

- **Authentication & Authorization:** JWT-based secure authentication.
- **Product Management:** Create, read, update, and delete products.
- **Cart Functionality:** Add, remove, and manage items in the cart.
- **Payment Integration:** Seamless payment processing with Stripe.
- **Clean Architecture:** Separation of concerns with layers for Domain, Application, Infrastructure, and API.
- **SOLID Principles:** High cohesion and low coupling for maintainable and extensible code.

---

## Technologies Used

- **Framework:** ASP.NET Core
- **Database:** SQL Server
- **Authentication:** JSON Web Tokens (JWT)
- **Payment Gateway:** Stripe
- **Mapping:** AutoMapper
- **Logging:** Serilog

---

## Project Structure

The project follows the Clean Architecture pattern with the following layers:

1. **Domain:** Contains core entities, enums, exceptions, and interfaces.
2. **Application:** Includes application-specific logic such as use cases and service interfaces.
3. **Infrastructure:** Handles database access, external services, and third-party integrations like Stripe.
4. **API:** The entry point for clients, handling HTTP requests and responses.