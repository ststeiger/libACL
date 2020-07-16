
namespace libACL
{

    // TODO: type ?
    public enum mode_t 
    : int
    {
        foobar
    }


    // acl_check error codes
    public enum acl_check_errors
        : int
    {
        ACL_MULTI_ERROR = 0x1000, // multiple unique objects 
        ACL_DUPLICATE_ERROR = 0x2000, // duplicate Id's in entries 
        ACL_MISS_ERROR = 0x3000, // missing required entry 
        ACL_ENTRY_ERROR = 0x4000 // wrong entry type  
    }

    
    // 23.2.2 acl_perm_t values
    public enum acl_perm_t
        : uint
    {
        ACL_READ = 0x04,
        ACL_WRITE = 0x02,
        ACL_EXECUTE = 0x01,
        // ACL_ADD = 0x08,
        // ACL_DELETE = 0x10,
    }
    
    
    // 23.2.5 acl_tag_t values
    public enum acl_tag_t 
        : int
    {
        ACL_UNDEFINED_TAG = 0x00,
        ACL_USER_OBJ = 0x01,
        ACL_USER = 0x02,
        ACL_GROUP_OBJ = 0x04,
        ACL_GROUP = 0x08,
        ACL_MASK = 0x10,
        ACL_OTHER = 0x20
    }
    
    public enum acl_type_t
        : uint
    {
        ACL_TYPE_ACCESS = 0x8000,
        ACL_TYPE_DEFAULT = 0x4000
    }    
    // 23.2.8 ACL Entry Constants
    public enum AclEntryConstants
        : int
    {
        ACL_FIRST_ENTRY = 0,
        ACL_NEXT_ENTRY = 1,
    }
    
    
}