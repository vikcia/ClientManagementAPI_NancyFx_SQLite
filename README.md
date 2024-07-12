# Customer Management Web API Application using .NET Framework

## Overview
This task involves creating a Customer Management Web API using .NET Framework. 
The API will handle CRUD operations for customers, including adding, retrieving, updating, and deleting customer records, along with logging actions performed on these records. 
Data will be stored in an SQLite database.

## Running
- Run the application using the Visual Studio.
- Or run **ClientManagementAPI_NancyFx_SQLite.sln** solution inside ClientManagementAPI_NancyFx_SQLite folder.
- Application will launch Swagger in web browser.

![image](https://github.com/user-attachments/assets/98d82392-82af-4d83-b045-f527d4edb70c)

## API Functionality
1. **The API should be capable of:**
   - Adding a client (name, age, comment)
   - Retrieving all client
   - Retrieving a specific client
   - Updating a client
   - Deleting a client
   - Viewing the action history for client

2. **Endpoints**
*Base URL: https://localhost:7029/*
  *- Create a New Client*
   Endpoint: POST /client
   *- Get All Clients*
   Endpoint: GET /client
   *- Get a Specific Client*
   Endpoint: GET /client/{id}
   *- Update a Specific Client*
   Endpoint: PUT /client/{id}
   *-Delete a Client*
   Endpoint: DELETE /client/{id}
   *-Get Action History*
   Endpoint: GET /client/history
