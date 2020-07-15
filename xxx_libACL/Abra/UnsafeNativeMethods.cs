using System.Runtime.InteropServices;
using System.Security;

namespace acl
{
    public class UnsafeNativeMethods
    {
        // extern acl_t acl_get_file(const char *path_p, acl_type_t type);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_file")]
        internal static extern __acl_ext* acl_get_file([MarshalAs(UnmanagedType.LPUTF8Str)] string path, acl_type_t type);

        // extern int acl_set_file(const char *path_p, acl_type_t type, acl_t acl);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "acl_set_file")]
        internal static extern int acl_set_file([MarshalAs(UnmanagedType.LPUTF8Str)] string path, uint type, __acl_ext* acl);

        // extern int acl_get_entry(acl_t acl, int entry_id, acl_entry_t *entry_p);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_entry")]
        internal static extern int acl_get_entry(__acl_ext* acl, AclEntryConstants entry_id, out __acl_entry_ext entry_p); // Double pointer, correct ???

        
        // extern void * acl_get_qualifier(acl_entry_t entry_d);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_qualifier")]
        internal static extern void* acl_get_qualifier(__acl_entry_ext* entry);

        
        // extern int acl_get_tag_type(acl_entry_t entry_d, acl_tag_t *tag_type_p);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_tag_type")]
        internal static extern int acl_get_tag_type(__acl_entry_ext* entry_d, acl_tag_t* tag_type_p);

        
        // extern int acl_get_permset(acl_entry_t entry_d, acl_permset_t *permset_p);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "acl_get_permset")]
        internal static extern int acl_get_permset(__acl_entry_ext* entry_d, out __acl_permset_ext permset_p); // double pointer ?

        
        // extern int acl_set_permset(acl_entry_t entry_d, acl_permset_t permset_d);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "acl_set_permset")]
        internal static extern int acl_set_permset(__acl_entry_ext* entry_d, __acl_permset_ext* permset_d);

        
        
        // extern int acl_set_permset(acl_entry_t entry_d, acl_permset_t permset_d);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_set_permset")]
        internal static extern int acl_set_permset2(__acl_entry_ext* entry_d, __acl_permset_ext* permset_d);

        
        // extern int acl_add_perm(acl_permset_t permset_d, acl_perm_t perm);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_add_perm")]
        internal static extern int acl_add_perm(__acl_permset_ext* permset_d, acl_perm_t perm);


        // extern int acl_get_perm(acl_permset_t permset_d, acl_perm_t perm);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_perm")]
        internal static extern int acl_get_perm(__acl_permset_ext* permset_d, acl_perm_t perm);
        

        // extern acl_t acl_from_text(const char *buf_p);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_from_text")]
        internal static extern System.IntPtr acl_from_text([MarshalAs(UnmanagedType.LPUTF8Str)] string text);

        // extern char *acl_to_text(acl_t acl, ssize_t *len_p);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_to_text")]
        internal static extern sbyte* acl_to_text(__acl_ext* acl, ulong* len_p);

        // extern char *acl_to_any_text(acl_t acl, const char *prefix, char separator, int options);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_to_any_text")]
        internal static extern sbyte* acl_to_any_text(__acl_ext* acl,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string prefix, sbyte separator, int options);

        // extern int acl_free(void *obj_p);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_free")]
        internal static extern int acl_free(void* obj_p);
        
        // extern int acl_get_perm(acl_permset_t permset_d, acl_perm_t perm);        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_perm")]
        internal static extern int acl_get_perm2(__acl_permset_ext* permset_d, acl_perm_t perm);

        
        // extern int acl_get_perm_np(acl_permset_t permset_d, acl_perm_t perm);    
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_perm_np")]
        internal static extern int acl_get_perm_np(__acl_permset_ext* permset_d, acl_perm_t perm);



        public static int ACL_GET_PERM(__acl_permset_ext* permset, acl_perm_t perm)
        {
            try
            {
                return acl_get_perm(permset, perm);
            }
            catch (System.Exception)
            {
            }

            return acl_get_perm_np(permset, perm);
        }

        
        
    }
}