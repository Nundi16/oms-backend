namespace OMS.Common
{
    public static class Constants
    {
        public static class Infrastructure
        {
            public const string LOGICAL_DELETION_FILTER_KEY = "LogicalDeletionQueryFilter";
            public const string DEFAULT_CONNECTION = "DefaultConnections";

            public static class ShadowProperties
            {
                public const string ROW_VERSION = "RowVersion";
                public const string CREATOR = "Creator";
                public const string MODIFIER = "Modifier";
                public const string CREATED_AT = "CreatedAt";
                public const string MODIFIED_AT = "ModifiedAt";
                public const string DELETED_AT = "DeletedAt";
            }
        }

        public static class Errors
        {
            public const string AUTHORIZATION_FAILED = nameof(AUTHORIZATION_FAILED);
        }
    }
}
