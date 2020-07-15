
using System;
using System.Runtime.InteropServices;
using System.Security;


namespace acl
{
    
    public unsafe class NativeMethods
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_get_file")]
        internal static extern System.IntPtr acl_get_file([MarshalAs(UnmanagedType.LPUTF8Str)] string path_p, uint type);
        
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_set_file")]
        internal static extern int acl_set_file([MarshalAs(UnmanagedType.LPUTF8Str)] string path_p, uint type, System.IntPtr acl);
        
        
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_get_entry")]
        internal static extern int acl_get_entry(System.IntPtr acl, int entry_id, System.IntPtr entry_p);
        
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_get_qualifier")]
        internal static extern System.IntPtr acl_get_qualifier(System.IntPtr entry_d);
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_get_tag_type")]
        internal static extern int acl_get_tag_type(System.IntPtr entry_d, acl_tag_t* tag_type_p);
        
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint="acl_get_permset")]
        internal static extern int acl_get_permset(System.IntPtr entry_d, global::System.IntPtr permset_p);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_set_permset")]
        internal static extern int acl_set_permset(global::System.IntPtr entry_d, global::System.IntPtr permset_d);
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_from_text")]
        internal static extern System.IntPtr acl_from_text([MarshalAs(UnmanagedType.LPUTF8Str)] string buf_p);
        
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_to_text")]
        internal static extern sbyte* acl_to_text(System.IntPtr acl, long* len_p);
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_to_any_text")]
        internal static extern sbyte* acl_to_any_text(IntPtr acl, [MarshalAs(UnmanagedType.LPUTF8Str)] string prefix, sbyte separator, int options);
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_free")]
        internal static extern int acl_free(global::System.IntPtr obj_p);
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_add_perm")]
        internal static extern int acl_add_perm(global::System.IntPtr permset_d, uint perm);

        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_get_perm")]
        internal static extern int acl_get_perm(global::System.IntPtr permset_d, uint perm);
            
        [SuppressUnmanagedCodeSecurity]
        [DllImport("acl", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint="acl_get_perm_np")]
        internal static extern int acl_get_perm_np(global::System.IntPtr permset_d, uint perm);

        
    }
    
}
