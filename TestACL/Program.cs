
namespace TestACL
{

    using libACL;
    using Mono.Unix;




    public enum FileSystemRights
    {
        Read,
        Write,
        ExecuteFile
    }


    // Eiciel Access Control List Editor
    // https://en.wikipedia.org/wiki/VDSO
    class Program
    {



        public static void TestUserFileSystemInfo()
        {
            // Mono.Unix.UnixUserInfo ui = new Mono.Unix.UnixUserInfo(123);
            // Mono.Unix.UnixGroupInfo gi = new Mono.Unix.UnixGroupInfo(123);
            // Mono.Unix.UnixDriveInfo drv = new Mono.Unix.UnixDriveInfo("/");
            // Mono.Unix.UnixSymbolicLinkInfo si = new Mono.Unix.UnixSymbolicLinkInfo("");

            // Mono.Unix.UnixDirectoryInfo di = new Mono.Unix.UnixDirectoryInfo("");
            // Mono.Unix.UnixFileInfo fi = new Mono.Unix.UnixFileInfo("test.txt");

            Mono.Unix.UnixFileSystemInfo fsi = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry("path");
        }


        public static bool TestAccessACL(System.IO.DirectoryInfo di, FileSystemRights AccessRight)
        {
            Mono.Unix.UnixFileInfo unixFileInfo = new Mono.Unix.UnixFileInfo("test.txt");
            // set file permission to 644
            unixFileInfo.FileAccessPermissions =
                Mono.Unix.FileAccessPermissions.UserRead | Mono.Unix.FileAccessPermissions.UserWrite
                | Mono.Unix.FileAccessPermissions.GroupRead
                | Mono.Unix.FileAccessPermissions.OtherRead;


            // https://www.geeksforgeeks.org/access-control-listsacl-linux/
            // https://www.tecmint.com/secure-files-using-acls-in-linux/

            // grep -i acl /boot/config*
            // nm -D /lib/x86_64-linux-gnu/libacl.so.1 | grep "acl"

            // [on RedHat based systems]
            // yum install nfs4-acl-tools acl libacl
            // [on Debian based systems]
            // sudo apt-get install nfs4-acl-tools acl

            // cat /proc/mounts
            // df -h | grep " /$"
            // mount | grep -i root
            // ==>
            // mount | grep `df -h | grep " /$" | awk '{print $1}'`
            // tune2fs -l /dev/nvme0n1p2 | grep acl
            // Mono.Unix.Native.Passwd ent = Mono.Unix.Native.Syscall.getpwent();
            // Mono.Unix.Native.Syscall.getgrouplist()


            if (AccessRight == FileSystemRights.Read)
            {
                Mono.Unix.UnixDirectoryInfo unixDirectoryInfo = new Mono.Unix.UnixDirectoryInfo(di.FullName);
                return (unixDirectoryInfo.FileAccessPermissions.HasFlag(FileAccessPermissions.UserRead)
                        || unixDirectoryInfo.FileAccessPermissions.HasFlag(FileAccessPermissions.GroupRead)
                        || unixDirectoryInfo.FileAccessPermissions.HasFlag(FileAccessPermissions.OtherRead)
                    );
            }
            else if (AccessRight == FileSystemRights.Write)
            {
                Mono.Unix.UnixDirectoryInfo unixDirectoryInfo = new Mono.Unix.UnixDirectoryInfo(di.FullName);
                return (unixDirectoryInfo.FileAccessPermissions.HasFlag(FileAccessPermissions.UserWrite)
                        || unixDirectoryInfo.FileAccessPermissions.HasFlag(FileAccessPermissions.GroupWrite)
                        || unixDirectoryInfo.FileAccessPermissions.HasFlag(FileAccessPermissions.OtherWrite)
                    );
            }
            else if (AccessRight == FileSystemRights.ExecuteFile)
            {
                Mono.Unix.UnixDirectoryInfo unixDirectoryInfo = new Mono.Unix.UnixDirectoryInfo(di.FullName);
                return (unixDirectoryInfo.FileAccessPermissions.HasFlag(FileAccessPermissions.UserExecute)
                        || unixDirectoryInfo.FileAccessPermissions.HasFlag(FileAccessPermissions.GroupExecute)
                        || unixDirectoryInfo.FileAccessPermissions.HasFlag(FileAccessPermissions.OtherExecute)
                    );
            }

            return false;
        }


        public static void TestExtendedAttributes()
        {
            // https://man7.org/linux/man-pages/man7/xattr.7.html
            string path = "/root/Desktop/CppSharp.txt";

            // https:// linux.die.net/man/2/access
            // https:// linux.die.net/man/1/kpseaccess
            // https:// linux.die.net/man/1/explain

            // https:// linux.die.net/man/5/acl
            // https:// linux.die.net/man/1/setfacl


            // https:// www.chromium.org/developers/how-tos/api-keys



            // Mono.Unix.Native.Syscall.getxattr()
            // Mono.Unix.Native.Syscall.fgetxattr()
            // Mono.Unix.Native.Syscall.lgetxattr()

            // Mono.Unix.Native.Syscall.setxattr()
            // Mono.Unix.Native.Syscall.fsetxattr()
            // Mono.Unix.Native.Syscall.lsetxattr

            // Mono.Unix.Native.Syscall.llistxattr()
            // Mono.Unix.Native.Syscall.flistxattr()
            // Mono.Unix.Native.Syscall.llistxattr()

            // Mono.Unix.Native.Syscall.removexattr()
            // Mono.Unix.Native.Syscall.fremovexattr()
            // Mono.Unix.Native.Syscall.lremovexattr()

            if (System.IO.File.Exists(path))
                System.Console.WriteLine("path exists");
            else
                System.Console.WriteLine("path doesn't exists");

            System.Text.Encoding enc = new System.Text.UTF8Encoding(false);
            string[] values = null;

            int setXattrSucceeded = Mono.Unix.Native.Syscall.setxattr(path, "user.foobar",
                enc.GetBytes("Hello World äöüÄÖÜ"), Mono.Unix.Native.XattrFlags.XATTR_CREATE);

            if (setXattrSucceeded == -1)
            {
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                // https://stackoverflow.com/questions/12662765/how-can-i-get-error-message-for-errno-value-c-language
                System.Console.WriteLine(message);
            } // End if (setXattrSucceeded == -1)

            byte[] data = null;
            long szLen = Mono.Unix.Native.Syscall.getxattr(path, "user.foobar", out data);

            string value = enc.GetString(data);
            System.Console.WriteLine(value);

            Mono.Unix.Native.Syscall.listxattr(path, System.Text.Encoding.UTF8, out values);
            System.Console.WriteLine(values);

            // https://man7.org/linux/man-pages/man2/getxattr.2.html
        } // End Sub TestExtendedAttributes 


        public static void ListUsersAndGroup()
        {
            string name = null;
            Mono.Unix.Native.Passwd pwbuf = null;
            Mono.Unix.Native.Passwd pwbufp;
            int ret = Mono.Unix.Native.Syscall.getpwnam_r(name, pwbuf, out pwbufp);


            bool _show_system = true;


            // Create the list of users
            Mono.Unix.Native.Passwd u = null;
            Mono.Unix.Native.Syscall.setpwent();
            while ((u = Mono.Unix.Native.Syscall.getpwent()) != null)
            {
                if (_show_system || (u.pw_uid >= 1000))
                {
                    System.Console.WriteLine(u.pw_name);
                } // End if (_show_system || (u.pw_uid >= 1000))

            } // Whend 
            Mono.Unix.Native.Syscall.endpwent();

            // Create the list of groups
            Mono.Unix.Native.Group g = null;
            Mono.Unix.Native.Syscall.setgrent();
            while ((g = Mono.Unix.Native.Syscall.getgrent()) != null)
            {
                if (_show_system || (g.gr_gid >= 1000))
                {
                    System.Console.WriteLine(g.gr_name);
                } // End if (_show_system || (g.gr_gid >= 1000)) 

            } // Whend 

            Mono.Unix.Native.Syscall.endgrent();
        } // End Sub ListUsersAndGroup 


        // typedef struct __acl_permset_ext* acl_permset_t;
        // typedef struct __acl_entry_ext* acl_entry_t;
        // typedef struct __acl_ext* acl_t;
        public static void ReadACL()
        {
            string fileName = "/root/Desktop/CppSharp.txt";
            acl_t acl_file = API.AclGetFile(fileName, acl_type_t.ACL_TYPE_ACCESS);

            if (acl_file == null)
            {
                // throw ACLManagerException(Glib::locale_to_utf8(strerror(errno)));
                Mono.Unix.Native.Errno er = Mono.Unix.Native.Stdlib.GetLastError();
                string message = Mono.Unix.Native.Stdlib.strerror(er);
                throw new System.InvalidOperationException(message);
            } // End if (acl_file == null) 


            acl_entry_t acl_entry_ = new acl_entry_t();
            int found = API.AclGetEntry(acl_file, AclEntryConstants.ACL_FIRST_ENTRY, acl_entry_);
            System.Console.WriteLine(found);

            while (found == 1)
            {
                acl_permset_t permission_set = API.AclGetPermset(acl_entry_);
                acl_tag_t acl_kind_tag = API.AclGetTagType(acl_entry_);

                if (acl_kind_tag == acl_tag_t.ACL_USER || acl_kind_tag == acl_tag_t.ACL_GROUP)
                {
                    // A user|group entry
                    // Gather the permissions
                    int reading = API.AclGetPerm(permission_set, acl_perm_t.ACL_READ);
                    int writing = API.AclGetPerm(permission_set, acl_perm_t.ACL_WRITE);
                    int execution = API.AclGetPerm(permission_set, acl_perm_t.ACL_EXECUTE);
                    System.Console.WriteLine(reading);
                    System.Console.WriteLine(writing);
                    System.Console.WriteLine(execution);


                    if (acl_kind_tag == acl_tag_t.ACL_USER)
                    {
                        AclQualifier qualifier = API.AclGetQualifier(acl_entry_);
                        bool validName = false;
                        Mono.Unix.Native.Passwd p = Mono.Unix.Native.Syscall.getpwuid(qualifier.Id);

                        string newName = null;
                        if (p != null)
                        {
                            validName = true;
                            newName = p.pw_name;
                        }
                        else
                        {
                            newName = "(" + qualifier.IdString + ")";
                        }

                        API.AclFree(qualifier);

                        // TODO: Add user to collection
                    }
                    else
                    {
                        AclQualifier qualifier = API.AclGetQualifier(acl_entry_);
                        Mono.Unix.Native.Group group = Mono.Unix.Native.Syscall.getgrgid(qualifier.Id);
                        bool validName = (group != null);
                        string newName = null;

                        if (group == null)
                        {
                            newName = "(" + qualifier.IdString + ")";
                        }
                        else
                        {
                            newName = group.gr_name;
                        }

                        API.AclFree(qualifier);

                        // TODO: Add group to collection
                        // _group_acl.push_back(new_acl);
                    } // End Else

                } // End if (acl_kind_tag == acl_tag_t.ACL_USER || acl_kind_tag == acl_tag_t.ACL_GROUP)
                else if (acl_kind_tag == acl_tag_t.ACL_USER_OBJ)
                {
                    // Owner
                    int reading = API.AclGetPerm(permission_set, acl_perm_t.ACL_READ);
                    int writing = API.AclGetPerm(permission_set, acl_perm_t.ACL_WRITE);
                    int execution = API.AclGetPerm(permission_set, acl_perm_t.ACL_EXECUTE);
                    System.Console.WriteLine(reading);
                    System.Console.WriteLine(writing);
                    System.Console.WriteLine(execution);
                }
                else if (acl_kind_tag == acl_tag_t.ACL_MASK)
                {
                    // The ACL mask
                    bool _there_is_mask = true;
                    int reading = API.AclGetPerm(permission_set, acl_perm_t.ACL_READ);
                    int writing = API.AclGetPerm(permission_set, acl_perm_t.ACL_WRITE);
                    int execution = API.AclGetPerm(permission_set, acl_perm_t.ACL_EXECUTE);
                }
                else if (acl_kind_tag == acl_tag_t.ACL_USER_OBJ)
                {
                    // Owner
                    int reading = API.AclGetPerm(permission_set, acl_perm_t.ACL_READ);
                    int writing = API.AclGetPerm(permission_set, acl_perm_t.ACL_WRITE);
                    int execution = API.AclGetPerm(permission_set, acl_perm_t.ACL_EXECUTE);
                }
                else if (acl_kind_tag == acl_tag_t.ACL_GROUP_OBJ)
                {
                    // Group
                    int reading = API.AclGetPerm(permission_set, acl_perm_t.ACL_READ);
                    int writing = API.AclGetPerm(permission_set, acl_perm_t.ACL_WRITE);
                    int execution = API.AclGetPerm(permission_set, acl_perm_t.ACL_EXECUTE);
                }
                else if (acl_kind_tag == acl_tag_t.ACL_OTHER)
                {
                    // Other
                    int reading = API.AclGetPerm(permission_set, acl_perm_t.ACL_READ);
                    int writing = API.AclGetPerm(permission_set, acl_perm_t.ACL_WRITE);
                    int execution = API.AclGetPerm(permission_set, acl_perm_t.ACL_EXECUTE);
                }

                found = API.AclGetEntry(acl_file, AclEntryConstants.ACL_NEXT_ENTRY, acl_entry_);
            } // Whend

            API.AclFree(acl_file);
        } // End Sub ReadACL 


        public static string StringifyAcl(acl_t acl)
        {
            // NativeMethods.AclAddPerm();
            string prefix = "";
            sbyte separator = 0x0b;
            int options = 134;
            return API.AclToAnyText(acl, prefix, separator, options);
        } // End Function StringifyAcl 


        public static void ModifyAcl()
        {
            string path = "";
            string aclText = "";

            acl_t acl_default = API.AclFromText(aclText);
            if (acl_default == null)
            {
                System.Console.Error.WriteLine($"ACL \"{aclText}\" is wrong!!!");
                throw new System.IO.InvalidDataException("Textual representation of the ACL is wrong");
            } // End if (acl_default == null)

            API.AclSetFile(path, acl_type_t.ACL_TYPE_DEFAULT, acl_default);

            // https://www.oreilly.com/library/view/c-cookbook/0596003390/ch16s10.html
            string str = API.AclToText(acl_default);
            System.Console.WriteLine(str);
        } // End Sub ModifyAcl 


        static void Main(string[] args)
        {
            Test2.Test();

            // https://linux.die.net/man/2/access
            Mono.Unix.Native.Syscall.access("path", Mono.Unix.Native.AccessModes.F_OK | Mono.Unix.Native.AccessModes.R_OK);
            // https://linux.die.net/man/2/faccessat
            Mono.Unix.Native.Syscall.faccessat(666, "ABSOLUTEpath", Mono.Unix.Native.AccessModes.R_OK, Mono.Unix.Native.AtFlags.AT_SYMLINK_NOFOLLOW);


            // ListUsersAndGroup();
            // TestExtendedAttributes();
            ReadACL();
            // ModifyAcl();

            //  <!-- apt-get install castxml -->
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 


    } // End Class API 


} // End Namespace 
