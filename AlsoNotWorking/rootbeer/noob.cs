
namespace acl
{
    // Flags for acl_to_any_text() 

    // Print NO, SOME or ALL effective permissions comments.
    // SOME prints effective rights comments for entries 
    // which have different permissions than effective permissions.
    public unsafe partial class libacl
    {
        public const int TEXT_SOME_EFFECTIVE = 0x01;
        public const int TEXT_ALL_EFFECTIVE = 0x02;

        // Align effective permission comments to column 32 using tabs or use a single tab.
        public const int TEXT_SMART_INDENT = 0x04;

        // User and group IDs instead of names.
        public const int TEXT_NUMERIC_IDS = 0x08;

        // Only output the first letter of entry types ("u::rwx" instead of "user::rwx").
        public const int TEXT_ABBREVIATE = 0x10;

        // acl_check error codes
        public const int ACL_MULTI_ERROR = (0x1000); // multiple unique objects 
        public const int ACL_DUPLICATE_ERROR = (0x2000); // duplicate Id's in entries 
        public const int ACL_MISS_ERROR = (0x3000); // missing required entry 
        public const int ACL_ENTRY_ERROR = (0x4000); // wrong entry type 


        // 23.2.8 ACL Entry Constants
        public const int ACL_FIRST_ENTRY = 0;
        public const int ACL_NEXT_ENTRY = 1;


        // 23.3.6 acl_type_t values
        public const uint ACL_TYPE_ACCESS = (0x8000);
        public const uint ACL_TYPE_DEFAULT = (0x4000);


        // 23.2.2 acl_perm_t values
        // public enum acl_perm_t : uint
        //{
        public const uint ACL_READ = (0x04);
        public const uint ACL_WRITE = (0x02);

        public const uint ACL_EXECUTE = (0x01);
        // public const uint ACL_ADD = (0x08);
        // public const uint ACL_DELETE = (0x10);
        //}
    }
/*
    // 23.2.5 acl_tag_t values
    public enum acl_tag_t : int
    {
        ACL_UNDEFINED_TAG = (0x00),
        ACL_USER_OBJ = (0x01),
        ACL_USER = (0x02),
        ACL_GROUP_OBJ = (0x04),
        ACL_GROUP = (0x08),
        ACL_MASK = (0x10),
        ACL_OTHER = (0x20)
    }
    */
}

