
namespace TestACL
{

	using libACL;


	class Test2
	{
		private const int EXIT_FAILURE = 1;
		private const int EXIT_SUCCESS = 0;


		public static int Test()
		{
			acl_t my_acl = null;
			string text_acl;
			ulong len;
			acl_entry_t my_entry = new acl_entry_t();
			uint group_id;
			acl_permset_t permset = null;

			Mono.Unix.Native.Stdlib.system("touch my_file.txt");

			/* Get the file's ACL. */
			my_acl = API.AclGetFile("my_file.txt", acl_type_t.ACL_TYPE_ACCESS);
			if (my_acl == null)
			{
				System.Console.Error.WriteLine("acl_get_file()");
				return EXIT_FAILURE;
			}

			// Convert the ACL into text so we can see what it is.
			text_acl = API.AclToText(my_acl); // &len
			if (text_acl == null)
			{
				System.Console.Error.WriteLine("acl_to_text()");
				return EXIT_FAILURE;
			}

			System.Console.WriteLine($"Initial ACL: {text_acl}");

			// Managed memory !
			// We're done with the text version, so release it.
			// if (API.AclFree(text_acl) == -1)
			// {
			// 	System.Console.Error.WriteLine ("acl_free()");
			// 	return EXIT_FAILURE;
			// }

			// Add an entry for a named group to the ACL.
			API.AclCreateEntry(my_acl, my_entry);
			// Throws !
			// if (acl_create_entry(my_acl, my_entry)== -1)
			// {
			//     System.Console.Error.WriteLine("acl_create_entry()");
			//     return EXIT_FAILURE;
			// }

			API.AclSetTagType(my_entry, acl_tag_t.ACL_USER);
			// if (acl_set_tag_type(my_entry, ACL_USER) == -1)
			// {
			// 	System.Console.Error.WriteLine("acl_set_tag_type");
			// 	return EXIT_FAILURE;
			// }

			group_id = 120;
			API.AclSetQualifier(my_entry, group_id);
			// Throws !
			// if (acl_set_qualifier(my_entry, &group_id) == -1)
			// {
			// 	System.Console.Error.WriteLine("acl_set_qualifier");
			// 	return EXIT_FAILURE;
			// }
			
			
			// Modify the permissions.
			permset = API.AclGetPermset(my_entry);
			
			// Throws !
			// if (acl_get_permset(my_entry) == -1)
			// {
			// 	System.Console.Error.WriteLine ("acl_get_permset");
			// 	return EXIT_FAILURE;
			// }

			API.AclClearPerms(permset);
			// Throws !
			// if (acl_clear_perms (permset ) == -1)
			// {
			// 	System.Console.Error.WriteLine ("acl_clear_perms");
			// 	return EXIT_FAILURE;
			// }

			API.AclAddPerm(permset, acl_perm_t.ACL_READ);
			
			// Throws !
			// if ( acl_add_perm (permset, ACL_READ))
			// {
			// 	System.Console.Error.WriteLine ("acl_add_perm");
			// 	return EXIT_FAILURE;
			// }

			// Recalculate the mask entry.
			API.AclCalcMask(my_acl);
			// Throws !
			// if (acl_calc_mask (my_acl))
			// {
			// 	System.Console.Error.WriteLine ("acl_calc_mask");
			// 	return EXIT_FAILURE;
			// }

			// Make sure the ACL is valid.

			// if (acl_valid (my_acl) ==-1)
			if (!API.AclValid(my_acl))
			{
				System.Console.Error.WriteLine("acl_valid");
				return EXIT_FAILURE;
			}

			// Update the ACL for the file.
			API.AclSetFile("my_file.txt", acl_type_t.ACL_TYPE_ACCESS, my_acl);
			// Throws !
			//if (acl_set_file ("my_file.txt", ACL_TYPE_ACCESS, my_acl) == -1)
			//{
			//	System.Console.Error.WriteLine ("acl_set_file");
			//	return EXIT_FAILURE;
			//}

			// Free the ACL in working storage.
			API.AclFree(my_acl);
			// Throws !
			// if (acl_free (my_acl) == -1)
			// {
			// 	System.Console.Error.WriteLine ("acl_free()");
			// 	return EXIT_FAILURE;
			// }

			// Verify that it all worked, by getting and printing the file's ACL.
			
			my_acl = API.AclGetFile("my_file.txt", acl_type_t.ACL_TYPE_ACCESS);
			if (my_acl == null)
			{
				System.Console.Error.WriteLine("acl_get_file()");
				return EXIT_FAILURE;
			}

			
			text_acl = API.AclToText(my_acl);
			if (text_acl == null)
			{
				System.Console.Error.WriteLine("acl_to_text()");
				return EXIT_FAILURE;
			}
			
			System.Console.WriteLine("Updated ACL: %s\n", text_acl);
			
			// We're done with the text version, so release it.
			// MANAGED MEMORY !
			// API.AclFree(text_acl);
			// if (acl_free (text_acl) == -1)
			// {
			// 	System.Console.Error.WriteLine ("acl_free()");
			// 	return EXIT_FAILURE;
			// }

			return EXIT_SUCCESS;
		} // End Sub Test 


	} // End Class Test2 
	

} // End Namespace TestACL 
