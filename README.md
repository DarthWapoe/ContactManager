# ContactManager
ContactManager is a REST API for managing contact items. This project uses ASP.NET Core and PostgreSQL, and it can be easily started using Docker Compose.

## Prerequisites
Before you begin, ensure you have the following installed on your machine:
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Build and Run the Services
Use Docker Compose to build and run the services:
```
docker-compose up --build
```

This command will:

1. Build the Docker image for the ASP.NET Core application.
2. Start the PostgreSQL database service.
3. Start the ASP.NET Core application service.

## Access the Application
Once the services are up and running, you can access the ContactManager API at:
```
http://localhost:8080
```

## API Documentation
The API documentation is available via Swagger at:
```
http://localhost:8080/swagger
```
