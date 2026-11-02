# ServiceRequestTracker

- [ServiceRequestTracker](#servicerequesttracker)
  - [How to run the solution](#how-to-run-the-solution)
    - [Docker](#docker)
    - [Locally from IDE](#locally-from-ide)
  - [How to test the solution](#how-to-test-the-solution)
    - [Integration tests](#integration-tests)
    - [Development testing](#development-testing)
  - [To be done later](#to-be-done-later)
  - [Unclear or controversial items](#unclear-or-controversial-items)

## How to run the solution

### Docker

Run multi-container Docker applications using a [single YAML configuration file](./docker-compose.yml):

```bat
docker-compose up -d
```

and it will deploy and start all the required containers: PostgreSQL, RabbitMQ with management console, `ServiceRequestTracker.API` ASP.NET 10 Web API application and `ServiceRequestTracker.Service` background service application.

`ServiceRequestTracker.API` Swagger page is available with URLs: http://localhost:8080/swagger or http://localhost:8081/swagger

At the end of the day run

```bat
docker-compose down
```

to remove solution-related containers.

### Locally from IDE

Just clone the code base, configure `ServiceRequestTracker.API` and `ServiceRequestTracker.Service` as multiple startup projects and run them.

## How to test the solution

### Integration tests

Just run the integration tests from a project `ServiceRequestTracker.API.IntegrationTests.csproj`

### Development testing

Just navigate to `ServiceRequestTracker.API` Swagger page with URLs http://localhost:8080/swagger or http://localhost:8081/swagger and try to call some routes.

## To be done later

Features have to be done if there is more time:

1. Introduce **Retry**, **Timeout**, **Circuit Breaker** policies
2. **Authentication** and **Authorization**
3. Logging to external data storage, like ELK stack
4. More integration tests
5. End-to-end tests
6. Performance/Load tests
7. Front-end application
8. Notifications from API to FE with ASP.NET SignalR (with web sockets or SSE)
9. Prepare Kubernetes cluster and deploy solution there

## Unclear or controversial items

1. In fact route `POST /api/service-requests` has to return HTTP 200
2. What is the `ProblemDetails` format?
3. Create and update service request models have to be separated
4. Solution requires a distributed monolith architecture with at least 2 separate applications: API to read data and Service to handle changes
5. In fact applications should not run the DB migrations on their startup or any other way. Migrations should be run either manually or automatically from CI/CD process.
6. Inbox/outbox approach with RDB will slow solution down on scale or high traffic
7. <span style="color: red;font-weight: bold;">Do not forget to specify that engineer must not use MassTransit packages of version 9 or higher! Only version 8! Otherwise, they will have to start their work from scratch!</span>
