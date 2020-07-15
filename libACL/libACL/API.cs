
using System.Net.Http.Headers;

namespace libACL 
{

    using System.Runtime.InteropServices; 
    
    
    public class API
    {
        
        
        /// <summary>
        /// The acl_error() function converts an ACL error code such as returned by the acl_check() function to a text message describing the error condition. In the "POSIX" locale, acl_check() returns the following descriptions for the error codes.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>The acl_error() function returns a text message if the error code is recognized, and a value of (const char *)NULL otherwise.</returns>
        public static string AclError(int code)
        {
            string str = NativeMethods.acl_error(code);
            
            return str;
        } // End Function AclError 
        
        
        /// <summary>
        ///  get an ACL by filename
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static acl_t AclGetFile(string path, acl_type_t type)
        {
            System.IntPtr ptr = NativeMethods.acl_get_file(path, type);
            if (ptr == System.IntPtr.Zero)
            {
                // return null;
                System.Console.Error.WriteLine("Error on AclSetFile");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                
                // throw ACLManagerException(Glib::locale_to_utf8(strerror(errno)));
                throw new System.InvalidOperationException(message);
            } // End if (ptr == System.IntPtr.Zero) 
            
            return new acl_t(ptr);
        } // End Function AclGetFile 
        
        
        /// <summary>
        ///  set an ACL by filename
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="acl"></param>
        /// <returns>The acl_set_file() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AclSetFile(string path, acl_type_t type, acl_t acl)
        {
            int ret = NativeMethods.acl_set_file(path, type, acl.Native);
            
            if (ret != 0)
            {
                System.Console.Error.WriteLine("Error on AclSetFile");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                
                // throw ACLManagerException(Glib::locale_to_utf8(strerror(errno)));
                throw new System.InvalidOperationException(message);
            } // End if (ret != 0) 
            
        } // End Sub AclSetFile 
        
        
        /// <summary>
        /// test for information in ACLs by file name
        /// </summary>
        /// <param name="path"></param>
        /// <returns>If successful, the acl_extended_file() function returns 1 if the file object referred to by path_p has an extended access ACL or a default ACL, and 0 if the file object referred to by path_p has neither an extended access ACL nor a default ACL. Otherwise, the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static bool AclExtendedFile(string path)
        {
            int ret = NativeMethods.acl_extended_file(path);
            
            if(ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclSetFile");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                
                // throw ACLManagerException(Glib::locale_to_utf8(strerror(errno)));
                throw new System.InvalidOperationException(message);
            } // End if (ret != 0) 
            
            return ret == 1;
        } // End Function AclExtendedFile 
        
        
        /// <summary>
        /// get an ACL entry
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="entry_id"></param>
        /// <param name="entry"></param>
        /// <returns>If the function successfully obtains an ACL entry, the function returns a value of 1. If the ACL has no ACL entries, the function returns the value 0. If the value of entry_id is ACL_NEXT_ENTRY and the last ACL entry in the ACL has already been returned by a previous call to acl_get_entry(), the function returns the value 0 until a successful call with an entry_id of ACL_FIRST_ENTRY is made. Otherwise, the value -1 is returned and errno is set to indicate the error.</returns>
        public static int AclGetEntry(acl_t acl, AclEntryConstants entry_id, acl_entry_t entry)
        {
            int ret = NativeMethods.acl_get_entry(acl.Native, entry_id, out entry.Native);
            if (ret == -1) 
            {
                // return null;
                System.Console.Error.WriteLine("Error on AclGetEntry");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret == -1)  
            
            return ret;
        } // End Function AclGetEntry 

        /// <summary>
        /// The acl_create_entry() function creates a new ACL entry in the ACL pointed to by the contents of the pointer argument acl. On success, the function returns a descriptor for the new ACL entry via entry.
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="entry"></param>
        /// <returns>The acl_create_entry() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static int AclCreateEntry(acl_t acl, acl_entry_t entry)
        {
            int ret = NativeMethods.acl_create_entry(ref acl.Native, ref  entry.Native);
            if (ret == -1) 
            {
                // return null;
                System.Console.Error.WriteLine("Error on AclCreateEntry");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret == -1)  
            
            return ret;
        } // End Function AclCreateEntry 
        

        /// <summary>
        /// retrieve the qualifier from an ACL entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>On success, the function returns a pointer to the tag qualifier that was retrieved into ACL working storage. On error, a value of (void *)NULL is returned and errno is set appropriately.</returns>
        public static AclQualifier AclGetQualifier(acl_entry_t entry)
        {
            System.IntPtr ptr = NativeMethods.acl_get_qualifier(entry.Native);
            if (ptr == System.IntPtr.Zero)
            {
                // return null;
                System.Console.Error.WriteLine("Error on AclGetQualifier");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ptr == System.IntPtr.Zero) 
            
            return new AclQualifier(ptr);
        } // End Function AclGetQualifier 
        
        // int acl_set_qualifier(acl_entry_t entry_d, const void *qualifier_p);
        /// <summary>
        /// The acl_set_qualifier() function sets the qualifier of the ACL entry indicated by the argument entry_d to the value referred to by the argument qualifier_p.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="qualifier"></param>
        /// <returns>The acl_set_qualifier() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static void AclSetQualifier(acl_entry_t entry, uint qualifier)
        {
            int ret = NativeMethods.acl_set_qualifier(entry.Native, ref qualifier);
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclSetQualifier");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret == -1)
            
        } // End Function AclSetQualifier 




        /// <summary>
        /// get the tag type of an ACL entry
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="tag_type"></param>
        /// <returns>The acl_get_tag_type() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static acl_tag_t AclGetTagType(acl_entry_t entry)
        {
            acl_tag_t tag_type;
            
            int ret = NativeMethods.acl_get_tag_type(entry.Native, out tag_type);
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclGetTagType");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret == -1) 
            
            return tag_type;
        } // End Function AclGetTagType 

        /// <summary>
        /// The acl_set_tag_type() function sets the tag type of the ACL entry indicated by the argument entry_d to the value of the argument tag_type.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="tag_type"></param>
        /// <returns>The acl_set_tag_type() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AclSetTagType(acl_entry_t entry, acl_tag_t tag_type)
        {
            int ret = NativeMethods.acl_set_tag_type(entry.Native, tag_type);
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclSetTagType");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret == -1) 
            
        } // End Function AclSetTagType 



        /// <summary>
        /// retrieve the permission set from an ACL entry
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="permset"></param>
        /// <returns>The acl_get_permset() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static acl_permset_t AclGetPermset(acl_entry_t entry)
        {
            acl_permset_t permset = new acl_permset_t();
            
            int ret = NativeMethods.acl_get_permset(entry.Native, out permset.Native);
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclGetPermset");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret ==  -1) 
            
            return permset;
        } // End Sub AclGetPermset 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="permset"></param>
        /// <returns>  The acl_set_permset() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static void AclSetPermset(acl_entry_t entry, acl_permset_t permset)
        {
            int ret = NativeMethods.acl_set_permset(entry.Native, permset.Native);
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclSetPermset");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret ==  -1) 
            
        } // End Sub AclSetPermset 
        
        
        /// <summary>
        /// add a permission to an ACL permission set
        /// </summary>
        /// <param name="permset"></param>
        /// <param name="perm"></param>
        /// <returns>The acl_add_perm() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static void AclAddPerm(acl_permset_t permset, acl_perm_t perm)
        {
            int ret = NativeMethods.acl_add_perm(permset.Native, perm);
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclAddPerm");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret ==  -1) 
            
        } // End Function AclAddPerm 
        
        
        /// <summary>
        /// The acl_delete_perm() function deletes the permission contained in the argument perm from the permission set referred to by the argument permset_d. An attempt to delete a permission that is not contained in the permission set is not considered an error.
        /// </summary>
        /// <param name="permset"></param>
        /// <param name="perm"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AclDeletePerm(acl_permset_t permset, acl_perm_t perm)
        {
            int ret = NativeMethods.acl_delete_perm(permset.Native, perm);
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclDeletePerm");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret ==  -1) 
            
        } // End Sub AclDeletePerm 
        
        /// <summary>
        /// The acl_clear_perms() function clears all permissions from the permission set referred to by the argument permset_d.
        /// </summary>
        /// <param name="permset"></param>
        /// <returns>The acl_clear_perms() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AclClearPerms(acl_permset_t permset)
        {
            int ret = NativeMethods.acl_clear_perms(permset.Native);
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclClearPerms");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ret ==  -1)
            
        } // End Sub AclClearPerms 
        
        
        /// <summary>
        /// The acl_calc_mask() function calculates and sets the permissions associated with the ACL_MASK ACL entry of the ACL referred to by acl_p. The value of the new permissions is the union of the permissions granted by all entries of tag type ACL_GROUP, ACL_GROUP_OBJ, or ACL_USER. If the ACL referred to by acl_p already contains an ACL_MASK entry, its permissions are overwritten; if it does not contain an ACL_MASK entry, one is added.
        /// </summary>
        /// <param name="acl"></param>
        /// <returns>The acl_calc_mask() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static void AclCalcMask(acl_t acl)
        {
            int ret = NativeMethods.acl_calc_mask(ref acl.Native);
            
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on AclCalcMask");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new  System.InvalidOperationException( message);
            } // End if (ret == -1) 
            
        } // End Sub AclCalcMask 
        
        
        /// <summary>
        /// The acl_get_perm() function tests if the permission specified by the argument perm is contained in the ACL permission set pointed to by the argument permset.
        /// </summary>
        /// <param name="permset"></param>
        /// <param name="perm"></param>
        /// <returns>If successful, the acl_get_perm() function returns 1 if the permission specified by perm is contained in the ACL permission set permset_d, and 0 if the permission is not contained in the permission set. Otherwise, the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static int AclGetPerm(acl_permset_t permset, acl_perm_t perm)
        {
            int ret = -1;
            
            try
            {
                ret = NativeMethods.acl_get_perm(permset.Native, perm);
            }
            catch (System.Exception)
            {
                try
                {
                    // The acl_get_perm_np() function is a Apple proprietary function that checks if a permission is set in a permission set.
                    ret = NativeMethods.acl_get_perm_np(permset.Native, perm);
                }
                catch (System.Exception)
                {}
            }
            
            if (ret == -1)
            {
                System.Console.Error.WriteLine("Error on ACL_GET_PERM");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.IO.InvalidDataException( message);
            } // End if (ret == -1) 
            
            return ret;
        } // End Function AclGetPerm 
        
        
        /// <summary>
        /// The acl_valid() function checks the ACL referred to by the argument acl for validity.
        /// </summary>
        /// <param name="acl"></param>
        /// <returns>The acl_valid() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        public static bool AclValid(acl_t acl)
        {
            int ret = NativeMethods.acl_valid(acl.Native);
            
            if (ret == -1)
            {
                // Perhaps we need to check if acl is valid...
                // return false;
                
                System.Console.Error.WriteLine("AclValid Error");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.IO.InvalidDataException("Invalid ACL !\r\n" + message);
            } // End if (ret == -1) 
            
            return true;
        } // End Function AclValid 
        
        
        /// <summary>
        /// The acl_check() function checks the ACL referred to by the argument acl for validity.
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        public static int AclCheck(acl_t acl, out int last)
        {
            int ret = NativeMethods.acl_check(acl.Native, out last);
            // In addition, if the pointer last is not NULL,
            // acl_check() assigns the number of the ACL entry
            // at which the error was detected to the value pointed to by last.
            // Entries are numbered starting with zero, in the order
            // in which they would be returned by the acl_get_entry() function.
            
            if (ret == -1)
            {
                // Perhaps we need to check if acl is valid...
                // return false;
                
                System.Console.Error.WriteLine("AclCheck Error");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.IO.InvalidDataException("Invalid ACL !\r\n" + message);
            } // End if (ret == -1)
            
            if (ret > 0)
            {
                string message = AclError(ret);
                throw new System.IO.InvalidDataException(message);
            } // End if (ret > 0) 
            
            return ret;
        } // End Function AclCheck 
        
        
        /// <summary>
        /// The acl_cmp() function compares the ACLs pointed to by the arguments acl1 and acl2 for equality. The two ACLs are considered equal if for each entry in acl1 there is an entry in acl2 with matching tag type, qualifier, and permissions, and vice versa. 
        /// </summary>
        /// <param name="acl1"></param>
        /// <param name="acl2"></param>
        /// <returns>If successful, the acl_cmp() function returns 0 if the two ACLs acl1 and acl2 are equal, and 1 if they differ. Otherwise, the value -1 is returned and the global variable errno is set to indicate the error. </returns>
        public static bool AclCmp(acl_t acl1, acl_t acl2)
        {
            int num = NativeMethods.acl_cmp(acl1.Native, acl2.Native);
            if (num == 0)
                return true;
            
            if (num == -1)
            {
                System.Console.Error.WriteLine("Error on AclCmp !");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (num == -1) 
            
            return false;
        } // End Function AclCmp 
        
        
        /// <summary>
        /// Create an ACL from text
        /// </summary>
        /// <param name="text"></param>
        /// <returns>On success, this function returns a pointer to the working storage. On error, a value of (acl_t)NULL is returned, and errno is set appropriately.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static acl_t AclFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new System.ArgumentNullException(nameof(text));
            
            System.IntPtr ptr = NativeMethods.acl_from_text(text);
            if (ptr == System.IntPtr.Zero)
            {
                // Perhaps we need to check if text is right...
                // return null;
                
                System.Console.Error.WriteLine($"AclFromText: ACL \"{text}\" is wrong!!!");
                
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.IO.InvalidDataException("Textual representation of the ACL is wrong !\r\n" + message);
            } // End if (ptr == System.IntPtr.Zero) 
            
            return new acl_t(ptr);
        } // End Function AclFromText 
        
        
        /// <summary>
        /// convert an ACL to text
        /// </summary>
        /// <param name="acl"></param>
        /// <returns>On success, this function returns a pointer to the long text form of the ACL. On error, a value of (char *)NULL is returned, and errno is set appropriately.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string AclToText(acl_t acl)
        {
            System.IntPtr ptr = NativeMethods.acl_to_text(acl.Native, out ulong len);
            if (ptr == System.IntPtr.Zero)
            {
                System.Console.Error.WriteLine("Error on acl_to_text");
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ptr == System.IntPtr.Zero) 
            
            byte[] ba = new byte[len];
            System.Runtime.InteropServices.Marshal.Copy(ptr, ba, 0, (int) len);
            AclFree(ptr);
            
            return System.Text.Encoding.UTF8.GetString(ba);
        } // End Function AclToText 
        
        
        /// <summary>
        /// The acl_to_any_text() function translates the ACL pointed to by the argument acl into a NULL terminated character string.
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="prefix"></param>
        /// <param name="separator"></param>
        /// <param name="options"></param>
        /// <returns>On success, this function returns a pointer to the text representation of the ACL. On error, a value of (char *)NULL is returned, and errno is set appropriately.</returns>
        public static string AclToAnyText(acl_t acl, string prefix, sbyte separator, int options)
        {
            System.IntPtr ptr = NativeMethods.acl_to_any_text(acl.Native, prefix, separator, options);
            // returnStr = new string(fixedPtr);
            
            if (ptr == System.IntPtr.Zero)
            {
                System.Console.Error.WriteLine("Error on AclToAnyText");
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (ptr == System.IntPtr.Zero) 
            
            System.Collections.Generic.List<byte> lst = new System.Collections.Generic.List<byte>();
            
            byte b;
            while( (b = Marshal.ReadByte(ptr)) != 0)
            {
                lst.Add(b);
            } // Whend 
            
            lst.Add(0);
            
            byte[] ba = lst.ToArray();
            return System.Text.Encoding.UTF8.GetString(ba);
        } // End Function AclToAnyText 
        
        
        /// <summary>
        /// The acl_free() function frees any releasable memory currently allocated by to the ACL data object identified by ptr. 
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns>The acl_free() function returns the value 0 if successful; otherwise the value -1 is returned and the global variable errno is set to indicate the error.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AclFree(System.IntPtr ptr)
        {
            int ret = NativeMethods.acl_free(ptr);
            if (ret != 0) 
            {
                System.Console.Error.WriteLine("Error on acl_free");
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);    
            } // End if (ret != 0) 
            
        } // End Function AclFree 
        
        
        public static void AclFree(acl_t acl_file)
        {
            AclFree(acl_file.Native);
        } // End Function AclFree 
        
        
        public static void AclFree(AclQualifier qualifier)
        {
            AclFree(qualifier.Native);
        } // End Function AclFree 
        
        
    } // End Class API 
    
    
} // End Namespace 
