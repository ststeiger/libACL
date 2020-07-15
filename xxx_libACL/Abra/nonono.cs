

using System.Runtime.InteropServices;

namespace libACL
{
    

    // #define ACL_UNDEFINED_ID	((id_t)-1)
    
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

    
/*    
    public struct acl_t
    {
        public System.IntPtr NativePointer;
        
        public acl_t(System.IntPtr ptr)
        {
            this.NativePointer = ptr;
        }
    }


    public struct AclExt
    {
        public System.IntPtr NativePointer;
        
        public AclExt(System.IntPtr ptr)
        {
            this.NativePointer = ptr;
        }


    }




    [StructLayout(LayoutKind.Explicit, Size = 0)]
    public struct AclEntryExt
    {
        [FieldOffset(0)]
        public System.IntPtr NativePointer;
        
        
        public AclEntryExt(System.IntPtr ptr)
        {
            this.NativePointer = ptr;
        }
    }
    
    
    [StructLayout(LayoutKind.Explicit, Size = 0)]
    public struct AclPermsetExt
    {
        
        [FieldOffset(0)]
        public System.IntPtr NativePointer;
        
        public AclPermsetExt(System.IntPtr ptr)
        {
            this.NativePointer = ptr;
        }

    
        
    }


    public unsafe class acl
    {
        public static System.IntPtr AclGetFile(string fileName, acl_type_t acltype)
        {
            return NativeMethods.acl_get_file(fileName, acltype);
        }
        
        
        public static int AclSetFile(string path, acl_type_t type, AclExt acl)
        {
            return NativeMethods.acl_set_file(path, (uint) type, acl.NativePointer);
        }
        
        
        public static int AclGetEntry(AclExt acl_file, AclEntryConstants cs, ref AclEntryExt entry)
        {
            AclEntryExt ex = new AclEntryExt(entry.NativePointer);
            System.IntPtr entryPointer = new System.IntPtr(&ex);
            ex.NativePointer = entryPointer;
            entry = ex;
            
            return NativeMethods.acl_get_entry(acl_file.NativePointer, cs, entryPointer);
        }
        
        
        public static System.IntPtr AclGetQualifier(AclEntryExt entry)
        {
            return NativeMethods.acl_get_qualifier(entry.NativePointer);
        }

        
        public static int AclGetTagType(ref AclEntryExt entry, ref acl_tag_t tag_type)
        {
            int ret = 0; 
            fixed (acl_tag_t* __tag_type_ptr = &tag_type)
            {
                ret = NativeMethods.acl_get_tag_type(entry.NativePointer, __tag_type_ptr);
            }
            
            return ret;
        }
        
        public static int AclGetPermset(ref AclEntryExt entry, ref AclPermsetExt permset)
        {
            // AclPermsetExt ex = new AclPermsetExt(permset.NativePointer); 
            // ex.NativePointer = new System.IntPtr(&ex);
            // permset = ex;
            
            return NativeMethods.acl_get_permset(entry.NativePointer, permset.NativePointer);
        }
        
        public static int AclSetPermset(AclEntryExt entry, ref AclPermsetExt permset)
        {
            AclPermsetExt ex = new AclPermsetExt(permset.NativePointer); 
            ex.NativePointer = new System.IntPtr(&ex);
            permset = ex;   
            
            return NativeMethods.acl_set_permset(entry.NativePointer, permset.NativePointer);
        }
        
        public static int AclGetPerm(AclPermsetExt permset, acl_perm_t perm)
        {   
            return NativeMethods.acl_get_perm(permset.NativePointer, (uint) perm);
        }
        
        public static int AclGetPermNP(AclPermsetExt permset, acl_perm_t perm)
        {            
            return NativeMethods.acl_get_perm_np(permset.NativePointer, (uint) perm);
        }
        
        
        public static int ACL_GET_PERM(AclPermsetExt permset, acl_perm_t perm)
        {
            try
            {
                return AclGetPerm(permset, perm);
            }
            catch (System.Exception)
            { }
            
            return AclGetPermNP(permset, perm);
        }
        
        
        public static AclExt AclFromText(string buffer)
        {
            System.IntPtr ret = NativeMethods.acl_from_text(buffer);
            return new AclExt(ret);
        }
        
        
        public static sbyte* AclToText(AclExt acl, ref long length)
        {
            fixed (long* length_ptr = &length)
            {
                return NativeMethods.acl_to_text(acl.NativePointer, length_ptr);
            }
        }
        
        
        public static sbyte* AclToAnyText(AclExt acl, string prefix, sbyte separator, int options)
        {
            return NativeMethods.acl_to_any_text(acl.NativePointer, prefix, separator, options);
        }
        
        
        public static int AclFree(System.IntPtr ptr)
        {
            return NativeMethods.acl_free(ptr);
        }
        
        public static int AclFree(AclExt ptr)
        {
            return NativeMethods.acl_free(ptr.NativePointer);
        }
        
        
        
        public static int AclAddPerm(AclPermsetExt permset, acl_perm_t perm)
        {
            return NativeMethods.acl_add_perm(permset.NativePointer, (uint) perm);
        }
        
    }
*/    
    
}
