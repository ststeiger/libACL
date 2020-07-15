
#define NOT_USE_LPUTF8Str


namespace libACL
{
    
    using System.Security;
    using System.Runtime.InteropServices;


    // typedef struct __acl_permset_ext *acl_permset_t;
    // typedef struct __acl_entry_ext	*acl_entry_t;
    // typedef struct __acl_ext	*acl_t;
    public class NativeMethods 
    {
        
        private const string ACL_LIBRARY = "acl";
        
        
        // extern const char *acl_error(int code);
        // https://linux.die.net/man/3/acl_error
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_error")]
        internal static extern string acl_error(int code);


        // extern acl_t acl_get_file(const char *path_p, acl_type_t type);
        // https://linux.die.net/man/3/acl_get_file
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_file")]
#if USE_LPUTF8Str
        internal static extern System.IntPtr acl_get_file([MarshalAs(UnmanagedType.LPUTF8Str)] string path, acl_type_t type);
#else
        internal static extern System.IntPtr acl_get_file([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(FileNameMarshaler))] string path, acl_type_t type);
#endif



        // extern int acl_set_file(const char *path_p, acl_type_t type, acl_t acl);
        // https://linux.die.net/man/3/acl_set_file
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_set_file")]
#if USE_LPUTF8Str
        internal static extern int acl_set_file([MarshalAs(UnmanagedType.LPUTF8Str)] string path, acl_type_t type, System.IntPtr acl);
#else
        internal static extern int acl_set_file([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(FileNameMarshaler))] string path, acl_type_t type, System.IntPtr acl);
#endif



        // int acl_extended_file(const char *path_p);
        // https://linux.die.net/man/3/acl_extended_file
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_extended_file")]
#if USE_LPUTF8Str
        internal static extern int acl_extended_file([MarshalAs(UnmanagedType.LPUTF8Str)] string path);
#else
        internal static extern int acl_extended_file([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(FileNameMarshaler))] string path);
#endif


        // extern int acl_get_entry(acl_t acl, int entry_id, acl_entry_t *entry_p);
        // https://linux.die.net/man/3/acl_get_entry
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_entry")]
        internal static extern int acl_get_entry(System.IntPtr acl, AclEntryConstants entry_id, out System.IntPtr entry); // Double pointer, correct ???
        
        
        // extern void * acl_get_qualifier(acl_entry_t entry_d);
        // https://linux.die.net/man/3/acl_get_qualifier
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_qualifier")]
        internal static extern System.IntPtr acl_get_qualifier(System.IntPtr entry);
        
        
        // extern int acl_get_tag_type(acl_entry_t entry_d, acl_tag_t *tag_type_p);
        // https://linux.die.net/man/3/acl_get_tag_type
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_tag_type")]
        internal static extern int acl_get_tag_type(System.IntPtr entry, out acl_tag_t tag_type);
        
        
        // extern int acl_get_permset(acl_entry_t entry_d, acl_permset_t *permset_p);
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_permset")]
        internal static extern int acl_get_permset(System.IntPtr entry, out System.IntPtr permset); // double pointer ?
        
        
        // extern int acl_set_permset(acl_entry_t entry_d, acl_permset_t permset_d);
        // https://man7.org/linux/man-pages//man3/acl_set_permset.3.html
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_set_permset")]
        internal static extern int acl_set_permset(System.IntPtr entry, System.IntPtr permset);
        
        
        // extern int acl_add_perm(acl_permset_t permset_d, acl_perm_t perm);
        // https://linux.die.net/man/3/acl_add_perm
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_add_perm")]
        internal static extern int acl_add_perm(System.IntPtr permset, acl_perm_t perm);
        
        
        // extern int acl_delete_perm(acl_permset_t permset_d, acl_perm_t perm);
        // https://linux.die.net/man/3/acl_delete_perm
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_delete_perm")]
        internal static extern int acl_delete_perm(System.IntPtr permset, acl_perm_t perm);
        
        
        // extern int acl_clear_perms(acl_permset_t permset_d);
        // https://linux.die.net/man/3/acl_clear_perms
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_clear_perms")]
        internal static extern int acl_clear_perms(System.IntPtr permset);
        
        
        // extern int acl_get_perm(acl_permset_t permset_d, acl_perm_t perm);    
        // https://linux.die.net/man/3/acl_get_perm
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_perm")]
        internal static extern int acl_get_perm(System.IntPtr permset, acl_perm_t perm);
        
        
        // extern int acl_get_perm_np(acl_permset_t permset_d, acl_perm_t perm); 
        // https://developer.apple.com/library/archive/documentation/System/Conceptual/ManPages_iPhoneOS/man3/acl_get_perm_np.3.html
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_perm_np")]
        internal static extern int acl_get_perm_np(System.IntPtr permset, acl_perm_t perm);
        

        // int acl_calc_mask(acl_t *acl_p);
        // https://linux.die.net/man/3/acl_calc_mask
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_calc_mask")]
        internal static extern int acl_calc_mask(ref System.IntPtr acl);
        
        
        // extern int acl_valid(acl_t acl);
        // https://linux.die.net/man/3/acl_valid
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_valid")]
        internal static extern int acl_valid(System.IntPtr acl);
        
        
        // extern int acl_check(acl_t acl, int *last);
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_check")]
        internal static extern int acl_check(System.IntPtr acl, out int last);
        
        
        // extern int acl_cmp(acl_t acl1, acl_t acl2); 
        // https://linux.die.net/man/3/acl_cmp
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_cmp")]
        internal static extern int acl_cmp(System.IntPtr acl1, System.IntPtr acl2);
        
        
        // extern acl_t acl_from_text(const char *buf_p);
        // https://linux.die.net/man/3/acl_from_text
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_from_text")]
#if USE_LPUTF8Str
        internal static extern System.IntPtr acl_from_text([MarshalAs(UnmanagedType.LPUTF8Str)] string text);
#else
        internal static extern System.IntPtr acl_from_text([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8CustomMarshaler))] string text);
#endif
        


        // extern char *acl_to_text(acl_t acl, ssize_t *len_p);
        // https://linux.die.net/man/3/acl_to_text
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_to_text")]
        internal static extern System.IntPtr acl_to_text(System.IntPtr acl, out ulong len);
        
        
        // extern char *acl_to_any_text(acl_t acl, const char *prefix, char separator, int options);
        // https://linux.die.net/man/3/acl_to_any_text
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_to_any_text")]
#if USE_LPUTF8Str
        internal static extern System.IntPtr acl_to_any_text(System.IntPtr acl, [MarshalAs(UnmanagedType.LPUTF8Str)] string prefix, sbyte separator, int options);
#else
        internal static extern System.IntPtr acl_to_any_text(System.IntPtr acl, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8CustomMarshaler))] string prefix, sbyte separator, int options);
#endif

        


        // extern int acl_free(void *obj_p);
        // https://linux.die.net/man/3/acl_free
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_free")]
        internal static extern int acl_free(System.IntPtr addr);
        
        
    } // End Class NativeMethods 
    
    
} // End Namespace libACL 