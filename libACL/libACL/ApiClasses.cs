
namespace libACL
{
    
    
    public class acl_t 
        :System.IDisposable
    {
        internal System.IntPtr Native;

        public acl_t()
        {}

        protected internal acl_t(System.IntPtr ptr)
        {
            this.Native = ptr;
        }

        public void Dispose()
        {
            if(this.Native != System.IntPtr.Zero)
                API.AclFree(this.Native);
        }
    }
    
    
    public class acl_entry_t
    {
        internal System.IntPtr Native;

        public acl_entry_t()
        {}

        protected internal acl_entry_t(System.IntPtr ptr)
        {
            this.Native = ptr;
        }
    }
    
    
    public class acl_permset_t
    {
        internal System.IntPtr Native;
        
        public acl_permset_t()
        {}

        protected internal acl_permset_t(System.IntPtr ptr)
        {
            this.Native = ptr;
        }
    }
    
    
    public class AclQualifier
        : System.IDisposable
    {
        internal System.IntPtr Native;
        
        public AclQualifier()
        { }

        protected internal AclQualifier(System.IntPtr ptr)
        {
            this.Native = ptr;
        } // End Constructor 
        
        
        public uint Id
        {
            get
            {
                // Is this correct ? 
                // There is no ReadUInt32...
                uint entityId = (uint)System.Runtime.InteropServices.Marshal.ReadInt32(this.Native);
                return (uint) entityId;
            }
        } // End Property Id 
        
        
        public string IdString
        {
            get
            {
                return this.Id.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        } // End Property IdString 
        
        
        public void Dispose()
        {
            if(this.Native != System.IntPtr.Zero)
                API.AclFree(this.Native);
        } // End Sub Dispose 
        
        
    } // End Class AclQualifier
    
    
}