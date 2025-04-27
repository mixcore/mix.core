# MixDatabaseController API Documentation

## Overview
`MixDatabaseController` is a RESTful API controller that manages database operations in the Mix CMS system. It provides endpoints for database management, including creation, duplication, migration, backup, and restoration.

## Base URL
```
/api/v2/rest/mix-portal/mix-database
```

## Endpoints

### 1. Get Database by Name
```http
GET /api/v2/rest/mix-portal/mix-database/get-by-name/{name}
```

#### Description
Retrieves database information by name.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| name | string | Name of the database to retrieve |

#### Returns
- 200 OK: Returns database information
- 404 Not Found: Database not found

#### Example Response
```json
{
    "id": 1,
    "systemName": "example_database",
    "displayName": "Example Database",
    "description": "Example database description",
    "databaseProvider": "SQLSERVER",
    "type": "Custom",
    "columns": [
        {
            "id": 1,
            "systemName": "id",
            "displayName": "ID",
            "dataType": "Integer"
        }
    ]
}
```

### 2. Duplicate Database
```http
GET /api/v2/rest/mix-portal/mix-database/duplicate/{id}
```

#### Description
Creates a copy of an existing database.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| id | int | ID of the database to duplicate |

#### Returns
- 200 OK: Returns the newly created database
- 404 Not Found: Original database not found

#### Example Response
```json
{
    "id": 2,
    "systemName": "example_database_copy",
    "displayName": "Example Database Copy",
    "description": "Copy of example database",
    "databaseProvider": "SQLSERVER",
    "type": "Custom"
}
```

### 3. Export Entity
```http
GET /api/v2/rest/mix-portal/mix-database/export-entity/{dbContextName}
```

#### Description
Exports entity from a database context.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| dbContextName | string | Name of the database context |

#### Returns
- 200 OK: Returns list of entity source code files
- 400 Bad Request: Database context does not exist

#### Example Response
```json
{
    "files": [
        {
            "name": "ExampleEntity.cs",
            "content": "public class ExampleEntity {...}"
        }
    ]
}
```

### 4. Migrate Database
```http
GET /api/v2/rest/mix-portal/mix-database/migrate/{name}
```

#### Description
Performs migration for a database.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| name | string | Name of the database to migrate |

#### Returns
- 200 OK: Migration successful

### 5. Backup Database
```http
GET /api/v2/rest/mix-portal/mix-database/backup/{name}
```

#### Description
Creates a backup of a database.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| name | string | Name of the database to backup |

#### Returns
- 200 OK: Backup request queued

### 6. Restore Database
```http
GET /api/v2/rest/mix-portal/mix-database/restore/{name}
```

#### Description
Restores a database from backup.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| name | string | Name of the database to restore |

#### Returns
- 200 OK: Restore request queued

### 7. Update Database
```http
GET /api/v2/rest/mix-portal/mix-database/update/{name}
```

#### Description
Updates database structure.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| name | string | Name of the database to update |

#### Returns
- 200 OK: Update request queued

## Error Responses

### 404 Not Found
```json
{
    "error": "Database not found",
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
// Get database by name
var response = await client.GetAsync("/api/v2/rest/mix-portal/mix-database/get-by-name/example_database");
var database = await response.Content.ReadAsAsync<MixDatabaseViewModel>();

// Duplicate database
response = await client.GetAsync("/api/v2/rest/mix-portal/mix-database/duplicate/1");
var newDatabase = await response.Content.ReadAsAsync<MixDatabaseViewModel>();

// Migrate database
await client.GetAsync("/api/v2/rest/mix-portal/mix-database/migrate/example_database");
```

### JavaScript Example
```javascript
// Get database by name
const response = await fetch('/api/v2/rest/mix-portal/mix-database/get-by-name/example_database');
const database = await response.json();

// Duplicate database
const duplicateResponse = await fetch('/api/v2/rest/mix-portal/mix-database/duplicate/1');
const newDatabase = await duplicateResponse.json();

// Migrate database
await fetch('/api/v2/rest/mix-portal/mix-database/migrate/example_database');
```

## Security
- All endpoints (except `get-by-name`) require `MixRoles.Owner` permission
- Uses `MixAuthorize` attribute for authentication and authorization

## Notes
- Backup, restore, and update operations are performed through a queue
- Database structure changes are notified via SignalR
- Cache is used to optimize query performance 