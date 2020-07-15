//
// Mono.Unix/UnixMarshal.cs
//
// Authors:
//   Jonathan Pryor (jonpryor@vt.edu)
//
// (C) 2004-2006 Jonathan Pryor
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//



// https://github.com/mono/mono/blob/master/mcs/class/Mono.Posix/Mono.Unix/UnixMarshal.cs
namespace libACL
{


	// Scenario:  We want to be able to translate an Error to a string.
	//  Problem:  Thread-safety.  Strerror(3) isn't thread safe (unless
	//            thread-local-variables are used, which is probably only 
	//            true on Windows).
	// Solution:  Use strerror_r().
	//  Problem:  strerror_r() isn't portable. 
	//            (Apparently Solaris doesn't provide it.)
	// Solution:  Cry.  Then introduce an intermediary, ErrorMarshal.
	//            ErrorMarshal exposes a single public delegate, Translator,
	//            which will convert an Error to a string.  It's static
	//            constructor first tries using strerror_r().  If it works,
	//            great; use it in the future.  If it doesn't work, fallback to
	//            using strerror(3).
	//            This should be thread safe, since the check is done within the
	//            class constructor lock.
	//            Strerror(3) will be thread-safe from managed code, but won't
	//            be thread-safe between managed & unmanaged code.
	internal class ErrorMarshal
	{
		internal delegate string ErrorTranslator(Mono.Unix.Native.Errno errno);

		internal static readonly ErrorTranslator Translate;

		static ErrorMarshal()
		{
			try
			{
				Translate = new ErrorTranslator(strerror_r);
				Translate(Mono.Unix.Native.Errno.ERANGE);
			}
			catch (System.EntryPointNotFoundException)
			{
				Translate = new ErrorTranslator(strerror);
			}
		}

		private static string strerror(Mono.Unix.Native.Errno errno)
		{
			return Mono.Unix.Native.Stdlib.strerror(errno);
		}

		private static string strerror_r(Mono.Unix.Native.Errno errno)
		{
			System.Text.StringBuilder buf = new System.Text.StringBuilder(16);
			int r = 0;
			do
			{
				buf.Capacity *= 2;
				r = Mono.Unix.Native.Syscall.strerror_r(errno, buf);
			} while (r == -1 && Mono.Unix.Native.Stdlib.GetLastError() == Mono.Unix.Native.Errno.ERANGE);

			if (r == -1)
				return "** Unknown error code: " + ((int)errno) + "**";
			return buf.ToString();
		}
	}

	public sealed /* static */ class UnixMarshal
	{
		private UnixMarshal() { }

		[System.CLSCompliant(false)]
		public static string GetErrorDescription(Mono.Unix.Native.Errno errno)
		{
			return ErrorMarshal.Translate(errno);
		}

		public static System.IntPtr AllocHeap(long size)
		{
			if (size < 0)
				throw new System.ArgumentOutOfRangeException("size", "< 0");
			return Mono.Unix.Native.Stdlib.malloc((ulong)size);
		}

		public static System.IntPtr ReAllocHeap(System.IntPtr ptr, long size)
		{
			if (size < 0)
				throw new System.ArgumentOutOfRangeException("size", "< 0");
			return Mono.Unix.Native.Stdlib.realloc(ptr, (ulong)size);
		}

		public static void FreeHeap(System.IntPtr ptr)
		{
			Mono.Unix.Native.Stdlib.free(ptr);
		}

		public static unsafe string PtrToStringUnix(System.IntPtr p)
		{
			if (p == System.IntPtr.Zero)
				return null;

			int len = checked((int)Mono.Unix.Native.Stdlib.strlen(p));
			return new string((sbyte*)p, 0, len, UnixEncoding.Instance);
		}

		public static string PtrToString(System.IntPtr p)
		{
			if (p == System.IntPtr.Zero)
				return null;
			return PtrToString(p, UnixEncoding.Instance);
		}

		public static unsafe string PtrToString(System.IntPtr p, System.Text.Encoding encoding)
		{
			if (p == System.IntPtr.Zero)
				return null;

			if (encoding == null)
				throw new System.ArgumentNullException("encoding");

			int len = GetStringByteLength(p, encoding);

			// Due to variable-length encoding schemes, GetStringByteLength() may
			// have returned multiple "null" characters.  (For example, when
			// encoding a string into UTF-8 there will be 4 terminating nulls.)
			// We don't want these null's to be in the returned string, so strip
			// them off.
			string s = new string((sbyte*)p, 0, len, encoding);
			len = s.Length;
			while (len > 0 && s[len - 1] == 0)
				--len;
			if (len == s.Length)
				return s;
			return s.Substring(0, len);
		}

		private static int GetStringByteLength(System.IntPtr p, System.Text.Encoding encoding)
		{
			System.Type encodingType = encoding.GetType();

			int len = -1;

			// Encodings that will always end with a single null byte
			if (typeof(System.Text.UTF8Encoding).IsAssignableFrom(encodingType) ||
					typeof(System.Text.UTF7Encoding).IsAssignableFrom(encodingType) ||
					typeof(UnixEncoding).IsAssignableFrom(encodingType) ||
					typeof(System.Text.ASCIIEncoding).IsAssignableFrom(encodingType))
			{
				len = checked((int)Mono.Unix.Native.Stdlib.strlen(p));
			}
			// Encodings that will always end with a 0x0000 16-bit word
			else if (typeof(System.Text.UnicodeEncoding).IsAssignableFrom(encodingType))
			{
				len = GetInt16BufferLength(p);
			}
			// Encodings that will always end with a 0x00000000 32-bit word
			else if (typeof(System.Text.UTF32Encoding).IsAssignableFrom(encodingType))
			{
				len = GetInt32BufferLength(p);
			}
			// Some non-public encoding, such as Latin1 or a DBCS charset.
			// Look for a sequence of encoding.GetMaxByteCount() bytes that are all
			// 0, which should be the terminating null.
			// This is "iffy", since it may fail for variable-width encodings; for
			// example, UTF8Encoding.GetMaxByteCount(1) = 4, so this would read 3
			// bytes past the end of the string, possibly into garbage memory
			// (which is why we special case UTF above).
			else
			{
				len = GetRandomBufferLength(p, encoding.GetMaxByteCount(1));
			}

			if (len == -1)
				throw new System.NotSupportedException("Unable to determine native string buffer length");
			return len;
		}

		private static int GetInt16BufferLength(System.IntPtr p)
		{
			int len = 0;
			while (System.Runtime.InteropServices.Marshal.ReadInt16(p, len * 2) != 0)
				checked { ++len; }
			return checked(len * 2);
		}

		private static int GetInt32BufferLength(System.IntPtr p)
		{
			int len = 0;
			while (System.Runtime.InteropServices.Marshal.ReadInt32(p, len * 4) != 0)
				checked { ++len; }
			return checked(len * 4);
		}

		private static int GetRandomBufferLength(System.IntPtr p, int nullLength)
		{
			switch (nullLength)
			{
				case 1: return checked((int)Mono.Unix.Native.Stdlib.strlen(p));
				case 2: return GetInt16BufferLength(p);
				case 4: return GetInt32BufferLength(p);
			}

			int len = 0;
			int num_null_seen = 0;

			do
			{
				byte b = System.Runtime.InteropServices.Marshal.ReadByte(p, len++);
				if (b == 0)
					++num_null_seen;
				else
					num_null_seen = 0;
			} while (num_null_seen != nullLength);

			return len;
		}

		/*
		 * Marshal a C `char **'.  ANSI C `main' requirements are assumed:
		 *
		 *   stringArray is an array of pointers to C strings
		 *   stringArray has a terminating NULL string.
		 *
		 * For example:
		 *   stringArray[0] = "string 1";
		 *   stringArray[1] = "string 2";
		 *   stringArray[2] = NULL
		 *
		 * The terminating NULL is required so that we know when to stop looking
		 * for strings.
		 */
		public static string[] PtrToStringArray(System.IntPtr stringArray)
		{
			return PtrToStringArray(stringArray, UnixEncoding.Instance);
		}

		public static string[] PtrToStringArray(System.IntPtr stringArray, System.Text.Encoding encoding)
		{
			if (stringArray == System.IntPtr.Zero)
				return new string[] { };

			int argc = CountStrings(stringArray);
			return PtrToStringArray(argc, stringArray, encoding);
		}

		private static int CountStrings(System.IntPtr stringArray)
		{
			int count = 0;
			while (System.Runtime.InteropServices.Marshal.ReadIntPtr(stringArray, count * System.IntPtr.Size) != System.IntPtr.Zero)
				++count;
			return count;
		}

		/*
		 * Like PtrToStringArray(System.IntPtr), but it allows the user to specify how
		 * many strings to look for in the array.  As such, the requirement for a
		 * terminating NULL element is not required.
		 *
		 * Usage is similar to ANSI C `main': count is argc, stringArray is argv.
		 * stringArray[count] is NOT accessed (though ANSI C requires that 
		 * argv[argc] = NULL, which PtrToStringArray(System.IntPtr) requires).
		 */
		public static string[] PtrToStringArray(int count, System.IntPtr stringArray)
		{
			return PtrToStringArray(count, stringArray, UnixEncoding.Instance);
		}

		public static string[] PtrToStringArray(int count, System.IntPtr stringArray, System.Text.Encoding encoding)
		{
			if (count < 0)
				throw new System.ArgumentOutOfRangeException("count", "< 0");
			if (encoding == null)
				throw new System.ArgumentNullException("encoding");
			if (stringArray == System.IntPtr.Zero)
				return new string[count];

			string[] members = new string[count];
			for (int i = 0; i < count; ++i)
			{
				System.IntPtr s = System.Runtime.InteropServices.Marshal.ReadIntPtr(stringArray, i * System.IntPtr.Size);
				members[i] = PtrToString(s, encoding);
			}

			return members;
		}

		public static System.IntPtr StringToHeap(string s)
		{
			return StringToHeap(s, UnixEncoding.Instance);
		}

		public static System.IntPtr StringToHeap(string s, System.Text.Encoding encoding)
		{
			if (s == null)
				return System.IntPtr.Zero;

			return StringToHeap(s, 0, s.Length, encoding);
		}

		public static System.IntPtr StringToHeap(string s, int index, int count)
		{
			return StringToHeap(s, index, count, UnixEncoding.Instance);
		}

		public static System.IntPtr StringToHeap(string s, int index, int count, System.Text.Encoding encoding)
		{
			if (s == null)
				return System.IntPtr.Zero;

			if (encoding == null)
				throw new System.ArgumentNullException("encoding");

			if (index < 0 || count < 0)
				throw new System.ArgumentOutOfRangeException((index < 0 ? "index" : "count"),
					 "Non - negative number required.");

			if (s.Length - index < count)
				throw new System.ArgumentOutOfRangeException("s", "Index and count must refer to a location within the string.");

			int null_terminator_count = encoding.GetMaxByteCount(1);
			int length_without_null = encoding.GetByteCount(s);
			int marshalLength = checked(length_without_null + null_terminator_count);

			System.IntPtr mem = AllocHeap(marshalLength);
			if (mem == System.IntPtr.Zero)
				throw new UnixIOException(Mono.Unix.Native.Errno.ENOMEM);

			unsafe
			{
				fixed (char* p = s)
				{
					byte* marshal = (byte*)mem;
					int bytes_copied;

					try
					{
						bytes_copied = encoding.GetBytes(p + index, count, marshal, marshalLength);
					}
					catch
					{
						FreeHeap(mem);
						throw;
					}

					if (bytes_copied != length_without_null)
					{
						FreeHeap(mem);
						throw new System.NotSupportedException("encoding.GetBytes() doesn't equal encoding.GetByteCount()!");
					}

					marshal += length_without_null;
					for (int i = 0; i < null_terminator_count; ++i)
						marshal[i] = 0;
				}
			}

			return mem;
		}

		public static bool ShouldRetrySyscall(int r)
		{
			if (r == -1 && Mono.Unix.Native.Stdlib.GetLastError() == Mono.Unix.Native.Errno.EINTR)
				return true;
			return false;
		}

		[System.CLSCompliant(false)]
		public static bool ShouldRetrySyscall(int r, out Mono.Unix.Native.Errno errno)
		{
			errno = (Mono.Unix.Native.Errno)0;
			if (r == -1 && (errno = Mono.Unix.Native.Stdlib.GetLastError()) == Mono.Unix.Native.Errno.EINTR)
				return true;
			return false;
		}

		// we can't permit any printf(3)-style formatting information, since that
		// would kill the stack.  However, replacing %% is silly, and some %* are
		// permitted (such as %m in syslog to print strerror(errno)).
		internal static string EscapeFormatString(string message,
				char[] permitted)
		{
			if (message == null)
				return "";
			System.Text.StringBuilder sb = new System.Text.StringBuilder(message.Length);
			for (int i = 0; i < message.Length; ++i)
			{
				char c = message[i];
				sb.Append(c);
				if (c == '%' && (i + 1) < message.Length)
				{
					char n = message[i + 1];
					if (n == '%' || IsCharPresent(permitted, n))
						sb.Append(n);
					else
						sb.Append('%').Append(n);
					++i;
				}
				// invalid format string: % at EOS.
				else if (c == '%')
					sb.Append('%');
			}
			return sb.ToString();
		}

		private static bool IsCharPresent(char[] array, char c)
		{
			if (array == null)
				return false;
			for (int i = 0; i < array.Length; ++i)
				if (array[i] == c)
					return true;
			return false;
		}

		internal static System.Exception CreateExceptionForError(Mono.Unix.Native.Errno errno)
		{
			string message = GetErrorDescription(errno);
			UnixIOException p = new UnixIOException(errno);

			// Ordering: Order alphabetically by exception first (right column),
			// then order alphabetically by Errno value (left column) for the given
			// exception.
			switch (errno)
			{
				case Mono.Unix.Native.Errno.EBADF:
				case Mono.Unix.Native.Errno.EINVAL: return new System.ArgumentException(message, p);

				case Mono.Unix.Native.Errno.ERANGE: return new System.ArgumentOutOfRangeException(message);
				case Mono.Unix.Native.Errno.ENOTDIR: return new System.IO.DirectoryNotFoundException(message, p);
				case Mono.Unix.Native.Errno.ENOENT: return new System.IO.FileNotFoundException(message, p);

				case Mono.Unix.Native.Errno.EOPNOTSUPP:
				case Mono.Unix.Native.Errno.EPERM: return new System.InvalidOperationException(message, p);

				case Mono.Unix.Native.Errno.ENOEXEC: return new System.InvalidProgramException(message, p);

				case Mono.Unix.Native.Errno.EIO:
				case Mono.Unix.Native.Errno.ENOSPC:
				case Mono.Unix.Native.Errno.ENOTEMPTY:
				case Mono.Unix.Native.Errno.ENXIO:
				case Mono.Unix.Native.Errno.EROFS:
				case Mono.Unix.Native.Errno.ESPIPE: return new System.IO.IOException(message, p);

				case Mono.Unix.Native.Errno.EFAULT: return new System.NullReferenceException(message, p);
				case Mono.Unix.Native.Errno.EOVERFLOW: return new System.OverflowException(message, p);
				case Mono.Unix.Native.Errno.ENAMETOOLONG: return new System.IO.PathTooLongException(message, p);

				case Mono.Unix.Native.Errno.EACCES:
				case Mono.Unix.Native.Errno.EISDIR: return new System.UnauthorizedAccessException(message, p);

				default: /* ignore */     break;
			}
			return p;
		}

		internal static System.Exception CreateExceptionForLastError()
		{
			return CreateExceptionForError(Mono.Unix.Native.Stdlib.GetLastError());
		}

		[System.CLSCompliant(false)]
		public static void ThrowExceptionForError(Mono.Unix.Native.Errno errno)
		{
			throw CreateExceptionForError(errno);
		}

		public static void ThrowExceptionForLastError()
		{
			throw CreateExceptionForLastError();
		}

		[System.CLSCompliant(false)]
		public static void ThrowExceptionForErrorIf(int retval, Mono.Unix.Native.Errno errno)
		{
			if (retval == -1)
				ThrowExceptionForError(errno);
		}

		public static void ThrowExceptionForLastErrorIf(int retval)
		{
			if (retval == -1)
				ThrowExceptionForLastError();
		}
	}
}
