
#define NOT_USE_LPUTF8Str


namespace libACL
{
    
    using System.Security;
    using System.Runtime.InteropServices;
    
    
    // typedef struct __acl_permset_ext *acl_permset_t;
    // typedef struct __acl_entry_ext	*acl_entry_t;
    // typedef struct __acl_ext	*acl_t;


    // https://linux.die.net/man/5/acl
    public class NativeMethods 
    {
        
        private const string ACL_LIBRARY = "acl";
        private const string LIBC = "libc";


        // char *strerror(int errnum);
        // https://man7.org/linux/man-pages/man3/strerror.3.html
        [System.Security.SuppressUnmanagedCodeSecurity]
        [System.Runtime.InteropServices.DllImport(LIBC, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "strerror")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstUtf8CustomMarshaler))]
        internal static extern string strerror(Errno errnum);
        
        
        internal static Errno errno
        {
            get
            {
                // How would you guarantee that the runtime has not called some CRT function 
                // during its internal processing that has affected the errno?

                // For the same reason, you should not call GetLastError directly either. 
                // The DllImportAttribute provides a SetLastError property so the runtime knows 
                // to immediately capture the last error and store it in a place that the managed code 
                // can read using Marshal.GetLastWin32Error.

                // this work on Linux !
                // Marshal.GetLastWin32Error can be used to retrieve errno.
                return (Errno)System.Runtime.InteropServices.Marshal.GetLastWin32Error();
            }
        }


        internal static string ErrorMessage
        {
            get
            {
                // throw ACLManagerException(Glib::locale_to_utf8(strerror(errno)));
                int err = (int) errno;
                
                string enu = " (Error " + System.Convert.ToString(err, System.Globalization.CultureInfo.InvariantCulture) + ")";
                return strerror(errno) + enu;
            }
        }




        // extern const char *acl_error(int code);
        // https://linux.die.net/man/3/acl_error
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_error", SetLastError = true)]
        internal static extern string acl_error(int code);


        // ssize_t acl_size(acl_t acl);
        // https://linux.die.net/man/3/acl_size
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_size", SetLastError = true)]
        internal static extern long acl_size(System.IntPtr acl);



        // extern acl_t acl_get_file(const char *path_p, acl_type_t type);
        // https://linux.die.net/man/3/acl_get_file
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_file", SetLastError = true)]
#if USE_LPUTF8Str
        internal static extern System.IntPtr acl_get_file([MarshalAs(UnmanagedType.LPUTF8Str, SetLastError = true)] string path, acl_type_t type);
#else
        internal static extern System.IntPtr acl_get_file([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(FileNameMarshaler))] string path, acl_type_t type);
#endif


        // acl_t acl_get_fd(int fd);
        // https://linux.die.net/man/3/acl_get_fd
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_fd", SetLastError = true)]
        internal static extern System.IntPtr acl_get_fd(int fd);



        // extern int acl_set_file(const char *path_p, acl_type_t type, acl_t acl);
        // https://linux.die.net/man/3/acl_set_file
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_set_file", SetLastError = true)]
#if USE_LPUTF8Str
        internal static extern int acl_set_file([MarshalAs(UnmanagedType.LPUTF8Str, SetLastError = true)] string path, acl_type_t type, System.IntPtr acl);
#else
        internal static extern int acl_set_file([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(FileNameMarshaler))] string path, acl_type_t type, System.IntPtr acl);
#endif


        // https://linux.die.net/man/3/acl_delete_def_file
        // int acl_delete_def_file(const char* path_p);
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_delete_def_file", SetLastError = true)]
#if USE_LPUTF8Str
        internal static extern int acl_delete_def_file([MarshalAs(UnmanagedType.LPUTF8Str, SetLastError = true)] string path);
#else
        internal static extern int acl_delete_def_file([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(FileNameMarshaler))] string path);
#endif



        // int acl_set_fd(int fd, acl_t acl);
        // https://linux.die.net/man/3/acl_set_fd
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_set_fd", SetLastError = true)]
        internal static extern int acl_set_fd(int fd, System.IntPtr acl);


        // int acl_extended_file(const char *path_p);
        // https://linux.die.net/man/3/acl_extended_file
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_extended_file", SetLastError = true)]
#if USE_LPUTF8Str
        internal static extern int acl_extended_file([MarshalAs(UnmanagedType.LPUTF8Str, SetLastError = true)] string path);
#else
        internal static extern int acl_extended_file([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(FileNameMarshaler))] string path);
#endif


        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_extended_file_nofollow ", SetLastError = true)]
#if USE_LPUTF8Str
        internal static extern int acl_extended_file_nofollow ([MarshalAs(UnmanagedType.LPUTF8Str, SetLastError = true)] string path);
#else
        internal static extern int acl_extended_file_nofollow([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(FileNameMarshaler))] string path);
#endif


        // int acl_extended_fd(int fd);
        // https://linux.die.net/man/3/acl_extended_fd
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_extended_fd", SetLastError = true)]
        internal static extern int acl_extended_fd(int fd);



        // extern int acl_get_entry(acl_t acl, int entry_id, acl_entry_t *entry_p);
        // https://linux.die.net/man/3/acl_get_entry
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_entry", SetLastError = true)]
        internal static extern int acl_get_entry(System.IntPtr acl, AclEntryConstants entry_id, out System.IntPtr entry); // Double pointer, correct ???


        // acl_t acl_from_mode(mode_t mode);
        // https://linux.die.net/man/3/acl_from_mode
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_from_mode", SetLastError = true)]
        internal static extern System.IntPtr acl_from_mode(mode_t mode);


        // int acl_equiv_mode(acl_t acl, mode_t* mode_p);
        // https://linux.die.net/man/3/acl_equiv_mode
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_equiv_mode", SetLastError = true)]
        internal static extern int acl_equiv_mode(System.IntPtr acl, ref mode_t mode);


        // acl_t acl_init(int count);
        // https://linux.die.net/man/3/acl_init
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_init", SetLastError = true)]
        internal static extern System.IntPtr acl_init(int count);


        // int acl_create_entry(acl_t *acl_p, acl_entry_t *entry_p);
        // https://linux.die.net/man/3/acl_create_entry
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_create_entry", SetLastError = true)]
        internal static extern int acl_create_entry(ref System.IntPtr acl, ref System.IntPtr entry);



        // https://linux.die.net/man/3/acl_delete_entry
        // int acl_delete_entry(acl_t acl, acl_entry_t entry_d);
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_delete_entry", SetLastError = true)]
        internal static extern int acl_delete_entry(System.IntPtr acl, System.IntPtr entry);


        // int acl_copy_entry(acl_entry_t dest_d, acl_entry_t src_d);
        // https://linux.die.net/man/3/acl_copy_entry
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_copy_entry", SetLastError = true)]
        internal static extern int acl_copy_entry(System.IntPtr dest, System.IntPtr src_d);


        // acl_t acl_copy_int(const void* buf_p);
        // https://linux.die.net/man/3/acl_copy_int
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_copy_int", SetLastError = true)]
        internal static extern System.IntPtr acl_copy_int(System.IntPtr buf);


        // https://linux.die.net/man/3/acl_copy_ext
        // ssize_t acl_copy_ext(void* buf_p, acl_t acl, ssize_t size);
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_copy_ext", SetLastError = true)]
        internal static extern long acl_copy_ext(System.IntPtr buf, System.IntPtr acl, long size);



        // acl_t acl_dup(acl_t acl);
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_dup", SetLastError = true)]
        internal static extern System.IntPtr acl_dup( System.IntPtr acl);



        // extern void * acl_get_qualifier(acl_entry_t entry_d);
        // https://linux.die.net/man/3/acl_get_qualifier
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_qualifier", SetLastError = true)]
        internal static extern System.IntPtr acl_get_qualifier(System.IntPtr entry);

        
        // int acl_set_qualifier(acl_entry_t entry_d, const void *qualifier_p);
        // https://linux.die.net/man/3/acl_set_qualifier
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_set_qualifier", SetLastError = true)]
        internal static extern int acl_set_qualifier(System.IntPtr entry, ref uint qualifier);
        
        
        // extern int acl_get_tag_type(acl_entry_t entry_d, acl_tag_t *tag_type_p);
        // https://linux.die.net/man/3/acl_get_tag_type
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_tag_type", SetLastError = true)]
        internal static extern int acl_get_tag_type(System.IntPtr entry, out acl_tag_t tag_type);

        // int acl_set_tag_type(acl_entry_t entry_d, acl_tag_t tag_type);
        // https://linux.die.net/man/3/acl_set_tag_type
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_set_tag_type", SetLastError = true)]
        internal static extern int acl_set_tag_type(System.IntPtr entry, acl_tag_t tag_type);
        
        
        // extern int acl_get_permset(acl_entry_t entry_d, acl_permset_t *permset_p);
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_permset", SetLastError = true)]
        internal static extern int acl_get_permset(System.IntPtr entry, out System.IntPtr permset); // double pointer ?
        
        
        // extern int acl_set_permset(acl_entry_t entry_d, acl_permset_t permset_d);
        // https://man7.org/linux/man-pages//man3/acl_set_permset.3.html
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_set_permset", SetLastError = true)]
        internal static extern int acl_set_permset(System.IntPtr entry, System.IntPtr permset);
        
        
        // extern int acl_add_perm(acl_permset_t permset_d, acl_perm_t perm);
        // https://linux.die.net/man/3/acl_add_perm
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_add_perm", SetLastError = true)]
        internal static extern int acl_add_perm(System.IntPtr permset, acl_perm_t perm);
        
        
        // extern int acl_delete_perm(acl_permset_t permset_d, acl_perm_t perm);
        // https://linux.die.net/man/3/acl_delete_perm
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_delete_perm", SetLastError = true)]
        internal static extern int acl_delete_perm(System.IntPtr permset, acl_perm_t perm);
        
        
        // extern int acl_clear_perms(acl_permset_t permset_d);
        // https://linux.die.net/man/3/acl_clear_perms
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_clear_perms", SetLastError = true)]
        internal static extern int acl_clear_perms(System.IntPtr permset);
        
        
        // extern int acl_get_perm(acl_permset_t permset_d, acl_perm_t perm);    
        // https://linux.die.net/man/3/acl_get_perm
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_perm", SetLastError = true)]
        internal static extern int acl_get_perm(System.IntPtr permset, acl_perm_t perm);
        
        
        // extern int acl_get_perm_np(acl_permset_t permset_d, acl_perm_t perm); 
        // https://developer.apple.com/library/archive/documentation/System/Conceptual/ManPages_iPhoneOS/man3/acl_get_perm_np.3.html
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_get_perm_np", SetLastError = true)]
        internal static extern int acl_get_perm_np(System.IntPtr permset, acl_perm_t perm);
        

        // int acl_calc_mask(acl_t *acl_p);
        // https://linux.die.net/man/3/acl_calc_mask
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_calc_mask", SetLastError = true)]
        internal static extern int acl_calc_mask(ref System.IntPtr acl);
        
        
        // extern int acl_valid(acl_t acl);
        // https://linux.die.net/man/3/acl_valid
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_valid", SetLastError = true)]
        internal static extern int acl_valid(System.IntPtr acl);
        
        
        // extern int acl_check(acl_t acl, int *last);
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_check", SetLastError = true)]
        internal static extern int acl_check(System.IntPtr acl, out int last);
        
        
        // extern int acl_cmp(acl_t acl1, acl_t acl2); 
        // https://linux.die.net/man/3/acl_cmp
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_cmp", SetLastError = true)]
        internal static extern int acl_cmp(System.IntPtr acl1, System.IntPtr acl2);
        
        
        // extern acl_t acl_from_text(const char *buf_p);
        // https://linux.die.net/man/3/acl_from_text
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_from_text", SetLastError = true)]
#if USE_LPUTF8Str
        internal static extern System.IntPtr acl_from_text([MarshalAs(UnmanagedType.LPUTF8Str, SetLastError = true)] string text);
#else
        internal static extern System.IntPtr acl_from_text([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8CustomMarshaler))] string text);
#endif
        


        // extern char *acl_to_text(acl_t acl, ssize_t *len_p);
        // https://linux.die.net/man/3/acl_to_text
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_to_text", SetLastError = true)]
        internal static extern System.IntPtr acl_to_text(System.IntPtr acl, out ulong len);
        
        
        // extern char *acl_to_any_text(acl_t acl, const char *prefix, char separator, int options);
        // https://linux.die.net/man/3/acl_to_any_text
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_to_any_text", SetLastError = true)]
#if USE_LPUTF8Str
        internal static extern System.IntPtr acl_to_any_text(System.IntPtr acl, [MarshalAs(UnmanagedType.LPUTF8Str, SetLastError = true)] string prefix, sbyte separator, int options);
#else
        internal static extern System.IntPtr acl_to_any_text(System.IntPtr acl, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8CustomMarshaler))] string prefix, sbyte separator, int options);
#endif

        


        // extern int acl_free(void *obj_p);
        // https://linux.die.net/man/3/acl_free
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ACL_LIBRARY, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "acl_free", SetLastError = true)]
        internal static extern int acl_free(System.IntPtr addr);
        
        
    } // End Class NativeMethods 
    
    
} // End Namespace libACL 
