﻿// See https://aka.ms/new-console-template for more information
using registryData;

Console.WriteLine("Hello, World!");
registryDataClass reg = new registryDataClass();
Console.WriteLine("Hello, World!");
Console.WriteLine( reg.getLoginName());
Console.WriteLine( reg.getLoginStatus());
reg.validateLicense();