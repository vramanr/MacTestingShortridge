# Calibration Management System

A modern web application for managing calibration processes, migrated from a legacy Visual FoxPro application to Angular, .NET Core, and PostgreSQL.

## Technology Stack

- **Frontend**: Angular 18 with Angular Material
- **Backend**: .NET 8 ASP.NET Core Web API
- **Database**: PostgreSQL 15
- **Containerization**: Docker & Docker Compose

## Features

### Core Functionality
- **Order-based Calibration Entry**: Complete workflow for entering calibration data for existing orders
- **No-order Calibration Workflows**: Type selection and calibration entry without pre-existing orders
- **Search & Edit**: Comprehensive search functionality with role-based edit permissions
- **Report Generation**: Multiple report types including calibration certificates, standards reports, and cover sheets
- **Supervisor/Admin Functions**: Company, tolerance, and technician management

### Business Logic Preservation
- 100% feature parity with legacy VFP application
- All business rules and validation logic preserved
- Multi-mode calibration support (Standard, Precision, etc.)
- Complex numerical formatting and calculations
- Role-based security and permissions

## Quick Start

### Prerequisites
- Docker and Docker Compose
- .NET 8 SDK (for development)
- Node.js 18+ (for development)

### Running with Docker Compose

1. Clone the repository:
```bash
git clone https://github.com/vramanr/MacTestingShortridge.git
cd MacTestingShortridge
```

2. Start all services:
```bash
docker-compose -f docker-compose.full.yml up -d
```

3. Access the application:
- Frontend: http://localhost:4200
- API: http://localhost:5000
- Database: localhost:5432

### Development Setup

#### Backend (.NET API)
```bash
cd CalibrationManagement
dotnet restore
dotnet ef database update
dotnet run --project CalibrationManagement.API --urls "http://localhost:5000"
```

#### Frontend (Angular)
```bash
cd calibration-management-ui
npm install
npm start
```

## Architecture

### Backend Structure
```
CalibrationManagement/
├── CalibrationManagement.API/          # Web API controllers and configuration
├── CalibrationManagement.Application/  # Business logic and services
├── CalibrationManagement.Core/         # Domain entities and interfaces
├── CalibrationManagement.Infrastructure/ # Data access and external services
└── CalibrationManagement.Tests/        # Unit and integration tests
```

### Frontend Structure
```
calibration-management-ui/src/app/
├── calibration/     # Calibration entry and search components
├── company/         # Company management
├── dashboard/       # Main dashboard
├── order/          # Order management
├── reports/        # Report generation
├── shared/         # Shared services and components
├── supervisor/     # Admin functions
├── technician/     # Technician management
└── tolerance/      # Tolerance management
```

## Key Components

### Calibration Workflows
- **CalibrationEntry**: Order-based calibration data entry
- **CalibrationTypeSelection**: No-order calibration type selection
- **CalibrationSearch**: Search and edit existing calibrations

### Management Modules
- **CompanyManagement**: Customer and company information
- **TechnicianManagement**: Technician profiles and certifications
- **ToleranceManagement**: Calibration tolerance settings

### Reports
- **CalibrationCertificate**: Official calibration certificates
- **StandardsReport**: Standards usage reports
- **CoverSheet**: Report cover sheets

## Data Migration

The system includes comprehensive data migration from legacy DBF files:
- Preserves all historical calibration data
- Maintains referential integrity
- Supports incremental updates

## Testing

### Backend Tests
```bash
cd CalibrationManagement
dotnet test
```

### Frontend Tests
```bash
cd calibration-management-ui
npm test
```

## Deployment

### Production Deployment
1. Build and deploy using Docker Compose:
```bash
docker-compose -f docker-compose.full.yml up -d --build
```

2. The application will be available at:
- Frontend: http://localhost:4200
- API: http://localhost:5000

### Environment Configuration
- Development: Uses local database and API endpoints
- Production: Uses containerized services with internal networking

## Security Features
- JWT-based authentication
- Role-based authorization
- Input validation and sanitization
- SQL injection prevention
- XSS protection

## Migration Notes

This application is a complete modernization of a legacy Visual FoxPro calibration management system. Key migration achievements:

- **100% Feature Parity**: Every feature from the VFP application has been preserved
- **Modern UI/UX**: Responsive Angular Material design
- **Scalable Architecture**: Clean architecture with separation of concerns
- **Enhanced Security**: Modern authentication and authorization
- **Cloud-Ready**: Containerized for easy deployment

## Support

For questions or issues, please contact the development team or create an issue in the repository.

## License

This project is proprietary software developed for Shortridge Instruments, Inc.
