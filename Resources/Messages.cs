namespace geoproject.Resources
{
    public static class Messages
    {
        //* Success Messages
        public static class Success
        {
            public const string PointCreated = "Point added successfully with ID #{0}";
            public const string PointUpdated = "Point with ID #{0} has been successfully updated.";
            public const string PointDeleted = "Point with ID #{0} has been successfully deleted.";
            public const string PointRetrieved = "Point with ID #{0} returned successfully.";
            public const string AllPointsRetrieved = "All points successfully returned";
            public const string DatabaseConnected = "Database connection successful!";
            public const string DatabaseMigrated = "Database migrated successfully!";
        }

        //*  Error Messages
        public static class Errors
        {
            public const string PointNotFound = "No point with ID #{0} found in the database.";
            public const string PointNameEmpty = "Point name cannot be empty";
            public const string GeometryEmpty = "Geometry cannot be empty";
            public const string InvalidId = "ID must be greater than 0";
            public const string InvalidGeometry = "Invalid geometry format for coordinate type {0}";
            public const string PointAlreadyExists = "Point with this name already exists";
            public const string DatabaseConnectionFailed = "Database connection failed: {0}";
            public const string MigrationFailed = "Migration failed: {0}";
            public const string GeneralError = "An error occurred while {0}";
            public const string PointNull = "Point cannot be null";
            public const string PointRetrievalError = "An error occurred while retrieving the point :";
            public const string DatabaseSetupGuide = "Make sure PostgreSQL is running and connection string is correct.";
        }

        //* Validation Messages
        public static class Validation
        {
            public const string PointRequired = "Point cannot be null";
            public const string NameRequired = "Point name cannot be empty";
            public const string GeometryRequired = "Geometry cannot be empty";
            public const string CoordinateTypeRequired = "Coordinate type is required";
            public const string NameMaxLength = "Point name cannot exceed 100 characters";
            public const string GeometryMaxLength = "Geometry cannot exceed 500 characters";
        }

        //* Geometry Format Help
        public static class GeometryHelp
        {
            public const string PointFormat = "Point format: 'longitude latitude' (e.g., '28.9784 41.0082')";
            public const string LineFormat = "Line format: 'lon1 lat1, lon2 lat2' (e.g., '28.9784 41.0082, 32.8597 39.9334')";
            public const string PolygonFormat = "Polygon format: 'lon1 lat1, lon2 lat2, lon3 lat3, lon1 lat1' (closed polygon)";
            public const string CoordinateLimits = "Longitude: -180 to 180, Latitude: -90 to 90";
            public const string InvalidCoordinateType = "Invalid coordinate type";
        }

        //* Operation Types (for generic error messages)
        public static class Operations
        {
            public const string Retrieving = "retrieving points";
            public const string Adding = "adding the point";
            public const string Updating = "updating the point";
            public const string Deleting = "deleting the point";
            public const string RetrievingPoint = "retrieving the point";
        }
    }
}
