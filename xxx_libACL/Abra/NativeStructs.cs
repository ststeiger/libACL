namespace libACL.NativeStructs
{
    
    
    // https://kernel.googlesource.com/pub/scm/fs/ext2/xfstests-bld/+/301faaf37f99fc30105f261f23d44e2a0632ffc0/acl/libacl/libobj.h
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct obj_prefix
    {
        public ulong p_magic;
        public ulong p_flags;
    }
    
    // typedef struct __acl_permset_ext *acl_permset_t;
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct __acl_permset_ext
    {
        // permset_t		s_perm; // typedef unsigned int permset_t;
        public uint s_perm;
    };

    // typedef struct acl_permset_obj_tag acl_permset_obj;
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct acl_permset_obj_tag
    {
        public obj_prefix o_prefix;
        public __acl_permset_ext i;
    };

    // #define __U32_TYPE		unsigned int
    // #define __ID_T_TYPE		__U32_TYPE
    // __STD_TYPE __ID_T_TYPE __id_t;		/* General type for IDs.  */
    // typedef __id_t id_t;

    /* qualifier object */
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct __qualifier_ext
    {
        //id_t                    q_id;
        public uint q_id;
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct qualifier_obj_tag
    {
        public obj_prefix o_prefix;
        public __qualifier_ext i;
    }


    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct acl_entry_obj_tag
    {
        public obj_prefix o_prefix;
        public __acl_entry_ext i;
    }


    // typedef struct __acl_ext	*acl_t;
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct __acl_ext
    {
        // typedef struct acl_entry_obj_tag acl_entry_obj;
        // acl_entry_obj		*a_prev, *a_next;
        // acl_entry_obj		*a_curr;
        // acl_entry_obj		*a_prealloc, *a_prealloc_end;

        public acl_entry_obj_tag* a_prev;
        public acl_entry_obj_tag* a_next;
        public acl_entry_obj_tag* a_curr;
        public acl_entry_obj_tag* a_prealloc;

        public acl_entry_obj_tag* a_prealloc_end;

        // size_t a_used; // typedef __SIZE_TYPE__ size_t;
        public ulong a_used;
    }


    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct acl_obj_tag
    {
        public obj_prefix o_prefix;
        public __acl_ext i;
    }


    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct __acl_entry
    {
        acl_tag_t e_tag;

        // qualifier_obj		e_id; // typedef struct qualifier_obj_tag qualifier_obj;
        qualifier_obj_tag e_id;

        // acl_permset_obj		e_perm;  //typedef struct acl_permset_obj_tag acl_permset_obj;
        acl_permset_obj_tag e_perm;
    }


    // typedef struct __acl_entry_ext	*acl_entry_t;
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct __acl_entry_ext
    {
        // acl_entry_obj		*e_prev, *e_next; // typedef struct acl_entry_obj_tag acl_entry_obj;
        public acl_entry_obj_tag* e_prev;

        public acl_entry_obj_tag* e_next;

        // acl_obj			*e_container; // typedef struct acl_obj_tag acl_obj;
        public acl_obj_tag* e_container;
        public __acl_entry e_entry;
    }
    
    
}