using Microsoft.Win32;

namespace registryData;
public class registryDataClass
{
    public registryDataClass()
    {
        RegistryKey testKey = Registry.LocalMachine.CreateSubKey(@"Software\CompanionPOS\");
        testKey.
        testKey.SetValue("CPOStest2","TestData2");
        Console.WriteLine("registryWritten");
    }
}
