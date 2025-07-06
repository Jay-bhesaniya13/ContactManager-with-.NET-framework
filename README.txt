📇 Contact Management System (ASP.NET Web Forms + SQL Server)

A web-based contact management application where users can register, log in, and manage their personal contact list securely. The system supports hashed password storage, contact uniqueness, and CSV import functionality.

---

🔧 Features

- ✅ User Registration & Login with password hashing (SHA-256)
- ✅ Secure Session Management
- ✅ Add Contacts (name + 10-digit phone)
- ✅ Unique Contacts per User
- ✅ CSV Import with success/error message colors
- ✅ Bootstrap UI with basic navigation
- ✅ Logout functionality

---

🧱 Tech Stack

Layer         | Technology
--------------|-------------------------------
Backend       | ASP.NET Web Forms (C#)
Frontend      | HTML + Bootstrap + WebForms
Database      | Microsoft SQL Server
Authentication| Session-based

---

📦 Database Setup

Run the following script in SQL Server to create the database and tables:

CREATE DATABASE ContactDB;
GO

USE ContactDB;
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(30) UNIQUE NOT NULL,
    PasswordHash VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL
);
GO

CREATE TABLE Contacts (
    ContactId INT PRIMARY KEY IDENTITY(1,1),
    ContactName VARCHAR(100) NOT NULL,
    ContactPhone VARCHAR(15) NOT NULL,
    UserId INT NOT NULL,
    CONSTRAINT FK_Contacts_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT UQ_ContactName_User UNIQUE (ContactName, UserId),
    CONSTRAINT UQ_ContactPhone_User UNIQUE (ContactPhone, UserId)
);
GO

---

🚀 How to Run

1. Open the project in Visual Studio.
2. Set the connection string in Web.config:

   <connectionStrings>
       <add name="ContactDB"
            connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ContactDB;Integrated Security=True"
            providerName="System.Data.SqlClient" />
   </connectionStrings>

3. Run the app.
4. Register a user and start adding contacts!

---

📥 CSV Import Format

Use a .csv file structured like:

ContactName,ContactPhone
Alice,9876543210
Bob,9123456789

⚠️ Only .csv files are accepted. Messages are shown in green/yellow/red based on outcome.

---

🛡 Security Notes

* Passwords are never stored in plain text.
* Sessions are used for access protection.
* Duplicate contact name/phone are restricted per user.
