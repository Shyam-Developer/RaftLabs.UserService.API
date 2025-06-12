# RaftLabs.UserService.API  
# RaftLabs - .NET Developer Assignment Solution

This repository contains a modular and testable implementation of a .NET 8 class library that interacts with the public API [Reqres.in](https://reqres.in/). The solution simulates a service component that fetches and manages user data from an external provider.

---

## 📌 Project Structure

```
RaftLabsUserServiceSolution/
├── UserServiceLibrary/         # Core logic: client, service, models
├── UserServiceTests/          # Unit test project using xUnit
├── UserServiceDemo/           # Console application to demonstrate usage
└── README.md                  # Project documentation
```

---

## 🚀 Features

* Communicates with `https://reqres.in/api/users` endpoints.
* Uses `HttpClient` with async/await.
* Supports pagination and fetch-by-ID.
* Modular design with interfaces.
* Graceful error handling.
* Configurable API base URL.
* Unit tests using xUnit and mocking.
* Optional console application demo.

---

## 🧪 Technologies Used

* .NET 8 / .NET Core
* C#
* HttpClient
* xUnit
* Moq (for mocking in tests)

---

## 🛠 How to Build and Run

### 1. Clone the repository

```bash
git clone https://github.com/Shyam-Developer/RaftLabs.UserService.API.git
cd RaftLabs.UserService.API
```

### 2. Open in Visual Studio 2022

* Open `RaftLabs.UserService.API.sln`
* Build the solution (Ctrl + Shift + B)

### 3. Run Unit Tests

* Open **Test Explorer** > Run All Tests
* Or set `UserServiceTests` as startup and run

### 4. Run Console Demo (optional)

* Set `UserServiceDemo` as startup project
* Press `F5` to run

### 5. Configuration

If using a console/hosted app, add the following in `appsettings.json`:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://reqres.in/api/"
  }
}
```

---

## 📦 Service Methods

### `Task<User> GetUserByIdAsync(int userId)`

Fetches a user by their ID.

### `Task<IEnumerable<User>> GetAllUsersAsync()`

Fetches all users from all pages internally.

---

## ❗ Error Handling

* Handles non-success HTTP codes (e.g., 404)
* Network failures, timeouts, deserialization issues
* Returns meaningful exceptions or fallback

---

## 🏆 Bonus Ideas (Not Implemented Yet)

* In-memory caching via `IMemoryCache`
* Retry policies via `Polly`
* Use of `IOptions<ApiSettings>` for cleaner config binding
* Clean Architecture layers

---

👨‍💻 Author

Koti Shyam📧 [shyamkoti33@gmail.com]🔗 [[LinkedIn Profile](www.linkedin.com/in/shyamkotideveloper)]🔗 [[GitHub Profile](https://github.com/Shyam-Developer)]


> **Note:** This project was created as part of the RaftLabs.
