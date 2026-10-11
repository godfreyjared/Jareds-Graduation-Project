# Continental Computing Solutions

**Multi-Store Inventory Management System**  
Graduation Project | C# and ASP.NET Core MVC

## Overview

Continental Computing Solutions is a computer-parts inventory and storefront application built as a software development graduation project. It models operations across three stores: Houston, Dallas, and San Antonio.

The project addresses a common retail challenge: keeping online product availability aligned with physical inventory, including customer cart reservations and employee stock operations.

## Features

### Customer Experience
- Browse the product catalog.
- Add items to a cart, temporarily reserving stock.
- Remove items from a cart, returning stock to availability.
- Check out, permanently deducting purchased quantities and clearing the cart.

### Inventory Management
- View inventory by store, including product available, total reserved, and employee-reserved quantities.
- The system refreshes inventory quantitiy every 10 seconds.
- Transfer available stock between the three locations.
- Add or remove available stock while protecting reserved inventory.
- Reserve stock for employee handling and return it to the shelf.
- **SPECIAL NOTE** To succeed in performing employee tasks like create, delete, or move inventory between stores, the username is "employee1" and the password is "password" - simple username/password were chosen for demo purposes.  

### Administration
- Create, edit, and delete products.
- Require employee credentials for protected operations.
- Validate stock quantities and reject invalid employee credentials.

## Technologies

- C# and .NET 9
- ASP.NET Core MVC and Razor
- HTML, CSS, and JavaScript
- Git and GitHub

## Project Structure

The Visual Studio solution includes an ASP.NET Core MVC web application (`JaredStore.Web`) and a C# console project containing shared models and inventory logic.

## How to Run

1. Clone or download the repository.
2. Install Visual Studio with ASP.NET and web development support and the .NET 9 SDK.
3. Open `Jared's Graduation Project.sln`.
4. Set `JaredStore.Web` as the startup project.
5. Build and run the application using the HTTPS launch profile.
6. Explore the Products, Inventory, and Cart pages.

## Verified Functional Tests

| Test | Verified result |
| --- | --- |
| Select Houston | Houston inventory appears |
| Add two items to cart | Available inventory decreases by two |
| Remove one item | Available inventory increases by one |
| Checkout | Cart clears; purchased stock remains deducted |
| Transfer inventory | Source decreases; destination increases |
| Employee reserve/return | Available and reserved quantities update |
| Create, edit, delete product | Changes appear correctly |
| Invalid employee credentials | Operation is rejected |
| Refresh inventory | Updated quantities appear automatically |

## Project Scope

This is a demonstration application. Product and inventory data are held in memory, so changes are not persisted after an application restart. Employee credential validation is for demonstration purposes and is not a production authentication system.

Potential next steps include database persistence, role-based authentication, and automated tests.

## Author

Jared Godfrey
