using Microsoft.Win32;

namespace registryData;
public class registryDataClass
{
    public registryDataClass()
    {
        RegistryKey testKey = Registry.CurrentUser.CreateSubKey(@"Software\CompanionPOS\");
        testKey.SetValue("CPOStest","TestData");
        Console.WriteLine("registryWritten");
    }
}
