
namespace libACL
{


    internal class Tests
    {


        public static void Test()
        {
            ConstUtf8Marshaler.GetInstance();
            Utf8Marshaler.GetInstance();

            string htmlData = "Test123";
            System.IntPtr data = Utf8Marshaler._staticInstance.MarshalManagedToNative(htmlData);
            Utf8Marshaler._staticInstance.CleanUpNativeData(data);

            // TODO: This is WRONG !
            System.IntPtr ptrVersion = data; // getVersion();
            string ver = ConstUtf8Marshaler._staticInstance.MarshalNativeToManaged(ptrVersion);
            System.Console.WriteLine(ver);
        }


    }


}
