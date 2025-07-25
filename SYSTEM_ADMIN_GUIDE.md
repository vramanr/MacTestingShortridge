# Calibration Management System - System Administration Guide

## Table of Contents
1. [System Overview](#system-overview)
2. [Installation and Setup](#installation-and-setup)
3. [Database Administration](#database-administration)
4. [User Management](#user-management)
5. [System Configuration](#system-configuration)
6. [Backup and Recovery](#backup-and-recovery)
7. [Monitoring and Maintenance](#monitoring-and-maintenance)
8. [Security Management](#security-management)
9. [Troubleshooting](#troubleshooting)
10. [Performance Optimization](#performance-optimization)

## System Overview

### Architecture
The Calibration Management System is built on a modern three-tier architecture:
- **Frontend**: Angular 18 with Angular Material UI
- **Backend**: .NET 8 ASP.NET Core Web API
- **Database**: PostgreSQL 15

### System Requirements

#### Server Requirements
- **CPU**: 4+ cores, 2.4GHz or higher
- **RAM**: 8GB minimum, 16GB recommended
- **Storage**: 100GB minimum, SSD recommended
- **OS**: Linux (Ubuntu 20.04+), Windows Server 2019+, or Docker-compatible environment

#### Client Requirements
- **Browser**: Chrome 90+, Firefox 88+, Safari 14+, Edge 90+
- **JavaScript**: Must be enabled
- **Network**: Stable internet connection

## Installation and Setup

### Docker Deployment (Recommended)

#### Prerequisites
```bash
# Install Docker and Docker Compose
sudo apt update
sudo apt install docker.io docker-compose
sudo systemctl start docker
sudo systemctl enable docker
```

#### Quick Start
```bash
# Clone the repository
git clone https://github.com/vramanr/MacTestingShortridge.git
cd MacTestingShortridge

# Start all services
docker-compose -f docker-compose.full.yml up -d

# Verify services are running
docker-compose -f docker-compose.full.yml ps
```

#### Service URLs
- **Frontend**: http://localhost:4200
- **API**: http://localhost:5000
- **Database**: localhost:5432

### Manual Installation

#### Database Setup
```bash
# Install PostgreSQL
sudo apt install postgresql postgresql-contrib

# Create database and user
sudo -u postgres psql
CREATE DATABASE CalibrationManagement;
CREATE USER calibration_user WITH PASSWORD 'your_secure_password';
GRANT ALL PRIVILEGES ON DATABASE CalibrationManagement TO calibration_user;
\q
```

#### Backend Setup
```bash
# Install .NET 8 SDK
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install dotnet-sdk-8.0

# Build and run the API
cd CalibrationManagement
dotnet restore
dotnet ef database update
dotnet run --project CalibrationManagement.API --urls "http://localhost:5000"
```

#### Frontend Setup
```bash
# Install Node.js and npm
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt install nodejs

# Build and serve the frontend
cd calibration-management-ui
npm install
npm run build
npm start
```

## Database Administration

### Database Schema
The system uses Entity Framework Core migrations for schema management.

#### Running Migrations
```bash
cd CalibrationManagement
dotnet ef migrations add MigrationName
dotnet ef database update
```

#### Key Tables
- **CalInfo**: Main calibration records
- **CalData**: Measurement point data
- **Company**: Customer information
- **CalTechs**: Technician records
- **Tolerances**: Calibration tolerances
- **CalStandards**: Standards used in calibrations

### Data Migration from VFP

#### DBF File Import
```bash
# Use the built-in data importer
cd CalibrationManagement
dotnet run --project CalibrationManagement.API -- --import-dbf /path/to/dbf/files
```

#### Manual Data Import
```sql
-- Example SQL for manual data import
COPY cal_info(cal_no, company_id, serial_number, model_number)
FROM '/path/to/cal_info.csv'
DELIMITER ','
CSV HEADER;
```

### Database Maintenance

#### Regular Maintenance Tasks
```sql
-- Update table statistics
ANALYZE;

-- Reindex tables
REINDEX DATABASE CalibrationManagement;

-- Vacuum to reclaim space
VACUUM ANALYZE;
```

#### Performance Monitoring
```sql
-- Check slow queries
SELECT query, mean_time, calls
FROM pg_stat_statements
ORDER BY mean_time DESC
LIMIT 10;

-- Monitor table sizes
SELECT schemaname, tablename, pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) as size
FROM pg_tables
WHERE schemaname = 'public'
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC;
```

## User Management

### Authentication System
The system uses JWT-based authentication with role-based authorization.

#### User Roles
- **Technician**: Basic calibration entry and search
- **Supervisor**: Full access including management functions
- **Admin**: System administration and user management
- **ReadOnly**: View-only access to calibrations and reports

### Adding Users

#### Through API
```bash
curl -X POST http://localhost:5000/api/users \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -d '{
    "username": "newuser",
    "email": "user@company.com",
    "password": "SecurePassword123!",
    "role": "Technician"
  }'
```

#### Through Database
```sql
-- Add user directly to database
INSERT INTO Users (Username, Email, PasswordHash, Role, CreatedDate)
VALUES ('newuser', 'user@company.com', 'hashed_password', 'Technician', NOW());
```

### Password Management

#### Password Policy
- Minimum 8 characters
- Must contain uppercase, lowercase, number, and special character
- Cannot reuse last 5 passwords
- Expires every 90 days

#### Reset Password
```bash
# Reset user password
curl -X POST http://localhost:5000/api/users/reset-password \
  -H "Content-Type: application/json" \
  -d '{
    "username": "username",
    "newPassword": "NewSecurePassword123!"
  }'
```

## System Configuration

### Environment Variables

#### Production Configuration
```bash
# API Configuration
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__DefaultConnection="Host=postgres;Database=CalibrationManagement;Username=calibration_user;Password=your_password"
export JWT__SecretKey="your-256-bit-secret-key"
export JWT__Issuer="CalibrationManagement"
export JWT__Audience="CalibrationManagement"

# Frontend Configuration
export API_URL="http://api:80"
export API_BASE_URL="http://api:80/api"
```

#### Development Configuration
```bash
# API Configuration
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection="Host=localhost;Database=CalibrationManagement;Username=calibration_user;Password=your_password"

# Frontend Configuration
export API_URL="http://localhost:5000"
export API_BASE_URL="http://localhost:5000/api"
```

### Application Settings

#### API Configuration (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=CalibrationManagement;Username=calibration_user;Password=your_password"
  },
  "JWT": {
    "SecretKey": "your-256-bit-secret-key",
    "Issuer": "CalibrationManagement",
    "Audience": "CalibrationManagement",
    "ExpiryMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## Backup and Recovery

### Database Backup

#### Automated Backup Script
```bash
#!/bin/bash
# backup-database.sh

DATE=$(date +%Y%m%d_%H%M%S)
BACKUP_DIR="/backups/calibration"
DB_NAME="CalibrationManagement"
DB_USER="calibration_user"

# Create backup directory
mkdir -p $BACKUP_DIR

# Perform backup
pg_dump -h localhost -U $DB_USER -d $DB_NAME > $BACKUP_DIR/calibration_backup_$DATE.sql

# Compress backup
gzip $BACKUP_DIR/calibration_backup_$DATE.sql

# Remove backups older than 30 days
find $BACKUP_DIR -name "*.sql.gz" -mtime +30 -delete

echo "Backup completed: calibration_backup_$DATE.sql.gz"
```

#### Schedule Backups
```bash
# Add to crontab for daily backups at 2 AM
crontab -e
0 2 * * * /path/to/backup-database.sh
```

### Application Backup

#### File System Backup
```bash
# Backup application files
tar -czf calibration_app_backup_$(date +%Y%m%d).tar.gz \
  /path/to/CalibrationManagement \
  /path/to/calibration-management-ui \
  /etc/nginx/sites-available/calibration \
  /etc/systemd/system/calibration-api.service
```

### Recovery Procedures

#### Database Recovery
```bash
# Stop the application
docker-compose -f docker-compose.full.yml down

# Restore database
gunzip -c /backups/calibration/calibration_backup_YYYYMMDD_HHMMSS.sql.gz | \
  psql -h localhost -U calibration_user -d CalibrationManagement

# Restart application
docker-compose -f docker-compose.full.yml up -d
```

#### Application Recovery
```bash
# Extract application backup
tar -xzf calibration_app_backup_YYYYMMDD.tar.gz -C /

# Restart services
systemctl restart calibration-api
systemctl restart nginx
```

## Monitoring and Maintenance

### Health Checks

#### API Health Check
```bash
# Check API status
curl -f http://localhost:5000/health || echo "API is down"

# Detailed health check
curl http://localhost:5000/health/detailed
```

#### Database Health Check
```bash
# Check database connectivity
pg_isready -h localhost -p 5432 -U calibration_user

# Check database size
psql -h localhost -U calibration_user -d CalibrationManagement -c "
SELECT pg_size_pretty(pg_database_size('CalibrationManagement')) as database_size;"
```

### Log Management

#### Application Logs
```bash
# View API logs
docker logs calibration-api

# View frontend logs
docker logs calibration-frontend

# View database logs
docker logs calibration-postgres
```

#### Log Rotation
```bash
# Configure logrotate for application logs
cat > /etc/logrotate.d/calibration << EOF
/var/log/calibration/*.log {
    daily
    rotate 30
    compress
    delaycompress
    missingok
    notifempty
    create 644 calibration calibration
}
EOF
```

### Performance Monitoring

#### System Metrics
```bash
# Monitor system resources
htop
iotop
nethogs

# Monitor disk usage
df -h
du -sh /var/lib/docker/
```

#### Application Metrics
```bash
# Monitor API performance
curl http://localhost:5000/metrics

# Database performance
psql -h localhost -U calibration_user -d CalibrationManagement -c "
SELECT * FROM pg_stat_activity WHERE state = 'active';"
```

## Security Management

### SSL/TLS Configuration

#### Nginx SSL Configuration
```nginx
server {
    listen 443 ssl http2;
    server_name calibration.yourcompany.com;

    ssl_certificate /etc/ssl/certs/calibration.crt;
    ssl_certificate_key /etc/ssl/private/calibration.key;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers ECDHE-RSA-AES256-GCM-SHA512:DHE-RSA-AES256-GCM-SHA512;

    location / {
        proxy_pass http://localhost:4200;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    location /api/ {
        proxy_pass http://localhost:5000/;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

### Firewall Configuration

#### UFW Configuration
```bash
# Enable firewall
sudo ufw enable

# Allow SSH
sudo ufw allow ssh

# Allow HTTP and HTTPS
sudo ufw allow 80
sudo ufw allow 443

# Allow database access from application server only
sudo ufw allow from 10.0.0.0/8 to any port 5432

# Check status
sudo ufw status
```

### Security Auditing

#### Regular Security Tasks
```bash
# Update system packages
sudo apt update && sudo apt upgrade

# Check for security updates
sudo unattended-upgrades --dry-run

# Audit user accounts
sudo lastlog
sudo who
sudo w

# Check failed login attempts
sudo grep "Failed password" /var/log/auth.log
```

## Troubleshooting

### Common Issues

#### Application Won't Start
```bash
# Check service status
systemctl status calibration-api
systemctl status nginx

# Check logs
journalctl -u calibration-api -f
tail -f /var/log/nginx/error.log

# Check port availability
netstat -tlnp | grep :5000
netstat -tlnp | grep :4200
```

#### Database Connection Issues
```bash
# Test database connection
psql -h localhost -U calibration_user -d CalibrationManagement

# Check PostgreSQL status
systemctl status postgresql

# Check database logs
tail -f /var/log/postgresql/postgresql-15-main.log
```

#### Performance Issues
```bash
# Check system resources
top
free -h
df -h

# Check database performance
psql -h localhost -U calibration_user -d CalibrationManagement -c "
SELECT * FROM pg_stat_activity WHERE state = 'active';"

# Check slow queries
psql -h localhost -U calibration_user -d CalibrationManagement -c "
SELECT query, mean_time, calls FROM pg_stat_statements ORDER BY mean_time DESC LIMIT 10;"
```

### Emergency Procedures

#### Service Recovery
```bash
# Restart all services
docker-compose -f docker-compose.full.yml restart

# Or restart individual services
docker restart calibration-api
docker restart calibration-frontend
docker restart calibration-postgres
```

#### Database Recovery
```bash
# If database is corrupted
sudo -u postgres pg_resetwal /var/lib/postgresql/15/main

# Restore from backup
gunzip -c /backups/calibration/latest_backup.sql.gz | \
  psql -h localhost -U calibration_user -d CalibrationManagement
```

## Performance Optimization

### Database Optimization

#### Index Management
```sql
-- Create indexes for frequently queried columns
CREATE INDEX idx_cal_info_serial_number ON cal_info(serial_number);
CREATE INDEX idx_cal_info_company_id ON cal_info(company_id);
CREATE INDEX idx_cal_data_cal_no ON cal_data(cal_no);
CREATE INDEX idx_cal_info_cal_date ON cal_info(cal_date);

-- Monitor index usage
SELECT schemaname, tablename, indexname, idx_scan, idx_tup_read, idx_tup_fetch
FROM pg_stat_user_indexes
ORDER BY idx_scan DESC;
```

#### Query Optimization
```sql
-- Enable query statistics
ALTER SYSTEM SET shared_preload_libraries = 'pg_stat_statements';
ALTER SYSTEM SET pg_stat_statements.track = 'all';

-- Restart PostgreSQL to apply changes
sudo systemctl restart postgresql
```

### Application Optimization

#### API Performance
```csharp
// Enable response caching in Program.cs
builder.Services.AddResponseCaching();
app.UseResponseCaching();

// Configure Entity Framework for performance
builder.Services.AddDbContext<CalibrationDbContext>(options =>
    options.UseNpgsql(connectionString)
           .EnableSensitiveDataLogging(false)
           .EnableServiceProviderCaching()
           .EnableDetailedErrors(false));
```

#### Frontend Optimization
```bash
# Build for production with optimization
cd calibration-management-ui
npm run build -- --configuration=production

# Enable gzip compression in nginx
gzip on;
gzip_vary on;
gzip_min_length 1024;
gzip_types text/plain text/css text/xml text/javascript application/javascript application/xml+rss application/json;
```

This system administration guide provides comprehensive instructions for managing the Calibration Management System. Regular maintenance and monitoring will ensure optimal performance and reliability.
