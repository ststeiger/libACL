// ----------------------------------------------------------------------------
// <auto-generated>
// This is autogenerated code by CppSharp.
// Do not edit this file or all your changes will be lost after re-generation.
// </auto-generated>
// ----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Security;


namespace acl
{
    public unsafe partial class ErrorContext
    {
        [StructLayout(LayoutKind.Explicit, Size = 0)]
        public partial struct __Internal
        {
        }

        public global::System.IntPtr __Instance { get; protected set; }

        internal static readonly global::System.Collections.Concurrent.ConcurrentDictionary<IntPtr, global::acl.ErrorContext> NativeToManagedMap = new global::System.Collections.Concurrent.ConcurrentDictionary<IntPtr, global::acl.ErrorContext>();

        protected bool __ownsNativeInstance;

        internal static global::acl.ErrorContext __CreateInstance(global::System.IntPtr native, bool skipVTables = false)
        {
            return new global::acl.ErrorContext(native.ToPointer(), skipVTables);
        }

        internal static global::acl.ErrorContext __CreateInstance(global::acl.ErrorContext.__Internal native, bool skipVTables = false)
        {
            return new global::acl.ErrorContext(native, skipVTables);
        }

        private static void* __CopyValue(global::acl.ErrorContext.__Internal native)
        {
            var ret = Marshal.AllocHGlobal(sizeof(global::acl.ErrorContext.__Internal));
            *(global::acl.ErrorContext.__Internal*) ret = native;
            return ret.ToPointer();
        }

        private ErrorContext(global::acl.ErrorContext.__Internal native, bool skipVTables = false)
            : this(__CopyValue(native), skipVTables)
        {
            __ownsNativeInstance = true;
            NativeToManagedMap[__Instance] = this;
        }

        protected ErrorContext(void* native, bool skipVTables = false)
        {
            if (native == null)
                return;
            __Instance = new global::System.IntPtr(native);
        }
    }
    
}