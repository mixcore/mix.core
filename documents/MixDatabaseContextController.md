# MixDatabaseContextController API Documentation

## Overview
`MixDatabaseContextController` is a RESTful API controller that manages database context operations in the Mix CMS system. It provides endpoints for creating, updating, and managing database contexts.

## Base URL
```
/api/v2/rest/mix-portal/mixdb-context
```

## Endpoints

### 1. Create Database Context
```http
POST /api/v2/rest/mix-portal/mixdb-context
```

#### Description
Creates a new database context.

#### Request Body
```json
{
    "systemName": "example_context",
    "displayName": "Example Context",
    "description": "Example database context",
    "connectionString": "encrypted_connection_string",
    "databaseProvider": "SQLSERVER"
}
```

#### Returns
- 200 OK: Returns the created database context
- 400 Bad Request: Invalid input data

### 2. Update Database Context
```http
PUT /api/v2/rest/mix-portal/mixdb-context/{id}
```

#### Description
Updates an existing database context.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| id | int | ID of the database context to update |

#### Request Body
```json
{
    "systemName": "updated_context",
    "displayName": "Updated Context",
    "description": "Updated database context",
    "connectionString": "encrypted_connection_string",
    "databaseProvider": "POSTGRESQL"
}
```

#### Returns
- 200 OK: Update successful
- 404 Not Found: Database context not found

### 3. Migrate Init Databases
```http
POST /api/v2/rest/mix-portal/mixdb-context/migrate
```

#### Description
Initializes and migrates databases for a new database context.

#### Request Body
```json
{
    "systemName": "example_context",
    "displayName": "Example Context",
    "connectionString": "encrypted_connection_string",
    "databaseProvider": "SQLSERVER"
}
```

#### Returns
- 200 OK: Migration successful
- 400 Bad Request: Invalid database context

## Error Responses

### 404 Not Found
```json
{
    "error": "Database context not found",
    "status": 404
}
```

### 400 Bad Request
```json
{
    "error": "Invalid database context",
    "status": 400
}
```

## Usage Examples

### C# Example
```csharp
// Create database context
var context = new MixDatabaseContextViewModel
{
    SystemName = "example_context",
    DisplayName = "Example Context",
    ConnectionString = "encrypted_connection_string",
    DatabaseProvider = MixDatabaseProvider.SQLSERVER
};
var response = await client.PostAsJsonAsync("/api/v2/rest/mix-portal/mixdb-context", context);

// Migrate databases
var migrateResponse = await client.PostAsJsonAsync(
    "/api/v2/rest/mix-portal/mixdb-context/migrate", 
    context
);
```

### JavaScript Example
```javascript
// Create database context
const context = {
    systemName: "example_context",
    displayName: "Example Context",
    connectionString: "encrypted_connection_string",
    databaseProvider: "SQLSERVER"
};
const response = await fetch('/api/v2/rest/mix-portal/mixdb-context', {
    method: 'POST',
    body: JSON.stringify(context)
});

// Migrate databases
const migrateResponse = await fetch('/api/v2/rest/mix-portal/mixdb-context/migrate', {
    method: 'POST',
    body: JSON.stringify(context)
});
```

## Security
- All endpoints require `MixRoles.Owner` permission
- Uses `MixAuthorize` attribute for authentication and authorization
- Connection strings are encrypted using AES encryption

## Notes
- Database contexts support multiple database providers (SQL Server, PostgreSQL, MySQL, SQLite)
- Automatic migration of database structure when creating new contexts
- PostgreSQL-specific extensions are automatically created when needed 