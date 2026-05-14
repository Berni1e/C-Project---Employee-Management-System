# Employee Management System

## 📌 About the Project
An application designed for managing employee HR data. 
This project was developed for my object-oriented programming (OOP) class. The main goal was to build a scalable solution that clearly separates the user interface from the business logic and the data layer.
As of now, this is the final version of the program. It includes full functionalities for the console version and the most important functionalities for the MVVM version.

## 🚀 Key Features
* Full support for **CRUD** operations on employee records (Create, Read, Update, Delete).
* User-friendly interface for browsing and searching database entries.
* Secure and efficient connection management with a relational database.

## 🛠 Technologies & Design Patterns
* **Language:** C# (.NET)
* **Architecture:** MVVM (Model-View-ViewModel) – *ensures encapsulation and a strict separation of the view logic from the data model.*
* **Data Access:** Repository Pattern – *centralizes query logic and protects the database from direct, unmanaged modifications from the view layer.*
* **Database:** [Insert e.g., Microsoft SQL Server / SQLite]
* **UI:** [Insert visual technology, e.g., WPF (Windows Presentation Foundation) / Windows Forms]
* **ORM (optional):** [Insert e.g., Entity Framework Core / pure ADO.NET]

## 📸 Screenshots

<img width="985" height="588" alt="Screen1" src="https://github.com/user-attachments/assets/2a4b5da4-f507-44e8-a5b1-9bbdd54cf212" />
<img width="981" height="587" alt="Screen2" src="https://github.com/user-attachments/assets/e56bc829-2be9-4523-9fb5-0f426d0aa1ec" />

## ⚙️ How to run locally?
1. Clone the repository:
   ```bash
   git clone https://github.com/Berni1e/C-Project---Employee-Management-System.git
2. Open the solution file (.sln) in Visual Studio.
3. Build the project (Build Solution) to restore necessary dependencies and NuGet packages.
4. (If database requires it) Configure the connection string in the configuration file or apply migrations using the Update-Database command in the Package Manager Console.
5. Run the application (press F5).
