# JO Train (Jordanian Railway Management System)

A modern web application built with **C# ASP.NET Core MVC** designed to model and simulate railway routes, station management, and ticketing across major Jordanian cities (Amman, Zarqa, Irbid, and Aqaba). 

**Live Demo:** [jotrain.runasp.net](https://jotrain.runasp.net)

## Key Features

* **Interactive Route Modeling:** Simulates railway networks and connectivity between key hubs (Amman, Zarqa, Irbid, Aqaba).
* **Distance-Based Pricing Engine:** Automatically calculates ticket pricing based on travel distance and route parameters.
* **Booking & Passenger Management:** Seamless workflows for reserving trips, managing schedules, and tracking bookings.
* **Responsive UI:** Clean, modern user interface optimized for smooth user experience across devices.

## Tech Stack

* **Backend:** C#, ASP.NET Core MVC (.NET)
* **Database & ORM:** Microsoft SQL Server, Entity Framework Core
* **Frontend:** HTML5, CSS3, JavaScript, Bootstrap / Tailwind
* **Hosting & Deployment:** MonsterASP.NET

## Getting Started Locally

To run a local copy of this project for development or testing purposes, follow these steps:

### Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/) (latest compatible version)
* [Microsoft SQL Server](https://www.microsoft.com/sql-server) or SQL Server Express
* An IDE like [Visual Studio](https://visualstudio.microsoft.com/), [JetBrains Rider](https://www.jetbrains.com/rider/), or [Visual Studio Code](https://code.visualstudio.com/)

### Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/Hamzeh-Alafaghni/JOTrain.git](https://github.com/Hamzeh-Alafaghni/JOTrain.git)
   cd JOTrain
   ```

2. **Configure the Database:**
   Update your connection string in `appsettings.json` to point to your local SQL Server instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JOTrainDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. **Apply Database Migrations:**
   ```bash
   dotnet ef database update
   ```

4. **Run the Application:**
   ```bash
   dotnet run
   ```

5. Open your browser and navigate to `https://localhost:5001` (or the HTTP port specified in your console).

## Author

* **Hamzeh Alafaghni** 
* GitHub: [@Hamzeh-Alafaghni](https://github.com/Hamzeh-Alafaghni)

## License

This project is open-source and available under the [MIT License](LICENSE).
