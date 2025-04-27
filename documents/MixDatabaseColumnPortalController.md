# MixDatabaseColumnPortalController API Documentation

## Overview
`MixDatabaseColumnPortalController` is a RESTful API controller that manages database column operations in the Mix CMS system. It provides endpoints for creating, updating, and managing database columns.

## Base URL
```
/api/v2/rest/mix-portal/mix-database-column
```

## Endpoints

### 1. Create Column
```http
POST /api/v2/rest/mix-portal/mix-database-column
```

#### Description
Creates a new database column.

#### Request Body
```json
{
    "systemName": "example_column",
    "displayName": "Example Column",
    "dataType": "String",
    "mixDatabaseName": "example_database",
    "columnConfigurations": {
        "isRequire": true,
        "isUnique": false,
        "maxLength": 255
    }
}
```

#### Returns
- 200 OK: Returns the created column
- 400 Bad Request: Invalid input data

### 2. Update Column
```http
PUT /api/v2/rest/mix-portal/mix-database-column/{id}
```

#### Description
Updates an existing database column.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| id | int | ID of the column to update |

#### Request Body
```json
{
    "systemName": "updated_column",
    "displayName": "Updated Column",
    "dataType": "Integer",
    "columnConfigurations": {
        "isRequire": false,
        "isUnique": true,
        "maxLength": null
    }
}
```

#### Returns
- 200 OK: Update successful
- 404 Not Found: Column not found

### 3. Delete Column
```http
DELETE /api/v2/rest/mix-portal/mix-database-column/{id}
```

#### Description
Deletes a database column.

#### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| id | int | ID of the column to delete |

#### Returns
- 200 OK: Delete successful
- 404 Not Found: Column not found

## Error Responses

### 404 Not Found
```json
{
    "error": "Column not found",
    "status": 404
}
```

### 400 Bad Request
```json
{
    "error": "Invalid column data",
    "status": 400
}
```

## Usage Examples

### C# Example
```csharp
// Create column
var column = new MixdbDatabaseColumnViewModel
{
    SystemName = "example_column",
    DisplayName = "Example Column",
    DataType = MixDataType.String,
    MixDatabaseName = "example_database",
    ColumnConfigurations = new ColumnConfigurations
    {
        IsRequire = true,
        IsUnique = false,
        MaxLength = 255
    }
};
var response = await client.PostAsJsonAsync("/api/v2/rest/mix-portal/mix-database-column", column);

// Update column
column.DisplayName = "Updated Column";
var updateResponse = await client.PutAsJsonAsync(
    $"/api/v2/rest/mix-portal/mix-database-column/{column.Id}", 
    column
);
```

### JavaScript Example
```javascript
// Create column
const column = {
    systemName: "example_column",
    displayName: "Example Column",
    dataType: "String",
    mixDatabaseName: "example_database",
    columnConfigurations: {
        isRequire: true,
        isUnique: false,
        maxLength: 255
    }
};
const response = await fetch('/api/v2/rest/mix-portal/mix-database-column', {
    method: 'POST',
    body: JSON.stringify(column)
});

// Update column
column.displayName = "Updated Column";
const updateResponse = await fetch(`/api/v2/rest/mix-portal/mix-database-column/${column.id}`, {
    method: 'PUT',
    body: JSON.stringify(column)
});
```

## Security
- All endpoints require `MixRoles.Owner` permission
- Uses `MixAuthorize` attribute for authentication and authorization

## Notes
- Supports various data types (String, Integer, DateTime, Boolean, etc.)
- Column configurations include requirements, uniqueness, and length constraints
- Changes to columns trigger database structure updates
- Column operations are queued and processed asynchronously

## Data Types
The following data types are supported:
- String
- Integer
- Long
- Double
- DateTime
- Date
- Time
- Boolean
- Guid
- Text
- Html
- Json
- Array
- ArrayMedia
- ArrayRadio
- TuiEditor 