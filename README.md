# E-Commerce-ASP.NET-MVC
A full-featured e-commerce web application built with ASP.NET MVC, implementing a wide range of features including user authentication, product management, shopping cart functionality, payment processing, and admin dashboard.

## Features

### Authentication & User Management
- Email/password registration and login
- Google OAuth integration
- Password reset via email
- User profile management
- Role-based access control

### Product Management
- Product catalog with categories
- Advanced product search and filtering
- Detailed product pages with images, descriptions, and specifications
- Inventory management system
- Product reviews and ratings

### Shopping Experience
- Shopping cart functionality
- Pagination for product listings

### Checkout Process
- Secure payment processing with Stripe integration
- Order summary and confirmation

### Order Management
- Order history for customers
- Order tracking functionality
- Order status updates

### Admin Features
- Comprehensive admin dashboard
- Sales analytics and reporting
- Complete order management
- Inventory control system
- Product CRUD operations
- User management
- Role management

### Technical Implementation
- Asynchronous programming for improved performance
- Responsive design for all devices
- Comprehensive error handling
- Data validation throughout the application
- Repository design pattern for clean separation of concerns
- Data seeding for initial application setup

## Technology Stack

- **Framework**: ASP.NET MVC
- **Backend**: C#
- **Frontend**: HTML, CSS, JavaScript, Bootstrap, JQuery
- **Database**: SQL Server
- **Authentication**: ASP.NET Identity with Google OAuth
- **Payment Processing**: Stripe API
- **Email Service**: SMTP (Gmail)

## Setup Instructions

### Prerequisites
* .NET 9.0 SDK
* Visual Studio 2022 or VS Code
* SQL Server (Local or Express)

### Installation Steps

1. Clone the repository
```bash
git clone https://github.com/ZiadHesham225/E-Commerce-ASP.NET-MVC.git
```
2. Update the connection string in `appsettings.json` to point to your SQL Server instance
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ECommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
3. Configure your Stripe API keys in `appsettings.json`:
```json
"Stripe": {
  "SecretKey": "your_stripe_secret_key",
  "PublishableKey": "your_stripe_publishable_key",
  "SuccessUrl": "https://localhost:7071/Checkout/Success",
  "CancelUrl": "https://localhost:7071/Checkout/Cancel"
}
```
4. Configure your email service settings in `appsettings.json`:
```json
"SmtpSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "EnableSsl": true,
  "UserName": "your_email@gmail.com",
  "Password": "your_app_password",
  "FromEmail": "noreply@store.com",
  "FromName": "E-Commerce"
}
```
5. Configure Google OAuth in `appsettings.json`:
```json
"Authentication": {
  "Google": {
    "ClientId": "your_google_client_id",
    "ClientSecret": "your_google_client_secret"
  }
}
```
6. Navigate to the project directory
```bash
cd ecommerce-app
```
7. Restore dependencies
```bash
dotnet restore
```
8. Apply database migrations
```bash
dotnet ef database update
```
9. Run the application
```bash
dotnet run
```
10. The application will seed initial data including:
- Admin user
- Product categories
- Sample products
- Roles
### Admin Account
After running the application for the first time, you can log in with the seeded admin account:
- Email: admin@example.com
- Password: Admin123!
## Acknowledgements

Feel free to contribute to this project by adding new features, fixing bugs, or providing suggestions for improvement!
