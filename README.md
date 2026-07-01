# Student Management System (ASP.NET Core MVC)

A web app for managing student records. Built with ASP.NET Core MVC, includes user login/registration, and lets you upload student photos.

---

## What I used

- C# (.NET 6)
- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- ASP.NET Core Identity (for login and roles)

---

## What it does

- Add, view, update, and delete student records
- Upload and display student photos
- User registration and login
- Admin and User roles (seeded when the app starts)
- Stores data in a local SQLite database

---

## How to run it

1. Open the solution in Visual Studio 2022
2. Build the project to restore packages
3. Open Package Manager Console and run:
   - `Add-Migration InitialIdentitySchema -Context LoginDBContext`
   - `Update-Database -Context LoginDBContext`
4. Press F5 to run the app
5. Use the seeded admin account to log in (check the code for credentials)

---

## Project structure

- Controllers
- Views
- Models
- Data (DbContexts and migrations)
- Interfaces and Repositories
- wwwroot (CSS, images, and static files)

---

## What this project shows

- Building a web app with ASP.NET Core MVC
- Using Entity Framework with SQLite
- Setting up login and roles with ASP.NET Core Identity
- Handling file uploads in a web app

---

## Author

Nhlanhlayethu Mazibuko  
IT Student at CUT

- GitHub: https://github.com/NhlanhlayethuMazibuko  
- Portfolio: https://nhlanhlayethu-mazibuko.netlify.app

---

## License

Just a school project—not for commercial use.