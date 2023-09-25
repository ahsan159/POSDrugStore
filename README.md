# POSDrugStore
Point of Sale for drug/medical store
# INSTRUCTIONS
software requires Microsoft SQL Server 2012 and .net 6.0 Runtime
1) SQL SERVER: https://download.microsoft.com/download/F/6/7/F673709C-D371-4A64-8BF9-C1DD73F60990/ENU/x64/SQLEXPR_x64_ENU.exe
2) .net 6.0 (runtime): https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.22-windows-x64-installer
2) .net 6.0 (sdk): https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.414-windows-x64-installer

1. start microsoft powershell in the main folder apply command
dotnet build
2. run the exe 
F:\cswork\drugStore\POSDrugStore\CPOS\login\loginWindow.exe

# WORK LOG
## 22-jun-2023
Created the basic drug class and reused persons class from previous project
## 27-Jun-2023
SQL interfacing for delete retrieve and create completed
## 3-Jul-2023
Created view drug form and succesfully integrated with main and sql
developing main sale screen
## 5-Jul-2023
Found a way of working with DataGridComboBoxColumn but some of the basics of Binding xaml with cs are clearly missing and have to work on it.
## 7-Jul-2023
Datagrid binding and calculations almost complete. Require integration with SQL and total and grand total calculations.
simplified row addition and deletion.
## 9-Jul-2023
Unable to trigger change while editing datagridtextboxcolumn cell. Calculations both general and discount are working fine. 
## 10-Jul-2023
Overall grand total calculations algorithm implemented. SQL database now updating correctly. Updated front end. to display current item and checkout process. 
## 12-Jul-2023
Checkout and New sale generation and invoice database creation performed.
## 13-Jul-2023
Added new sql wrapper class to clean code in main POS window. Added Sale List window to view previous sales.
## 14-Jul-2023
Minor improvements in SQL wrapper functions
## 15-Jul-2023
Updated the sale window by adding date range selector and total sales calculation for till.
SQL Wrapper requires more work to deal with special queries and creating and deleting table
## 17-Jul-2023
Adding Stock update window. Updates in backend of POS main Window.
## 18-Jul-2023
Stock table added and now being updated for data entry. Minor changes also expected in stock entry window in future.
## 19-Jul-2023
Added dash board window created a commercial makeup of it. Now combining functionalities in it.
## 20-Jul-2023
Completely revamped the dash board sales, products and stocks are updated and internally tested.
## 21-Jul-2023
Updated and optimized new sale algorithms.
## 31-Jul-2023
Started working on registry.
## 01-Aug-2023
Registry update and creation method created. License validation method created. 
## 02-Aug-2023
More updates in registry algorithms.
## 04-Aug-2023
Login process integration complete
## 07-Aug-2023
Implemented stock tab algorithm and new sale execution algorithm
## 08-Aug-2023
App is almost ready. Time for reintegration with license manager software. which will be seperate app and seperatly installed. Also develop installation routine of the softwware. 
## 25-Aug-2023
Updated viewDrug window.
## 28-Aug-2023
Added images to tab items and drug views.
## 31-Aug-2023
Created XML report generator. Prepared initial XSLT. Report generator testing.
## 7-Sep-2023
Windows installer project is almost complete. Installation class libraries for setup are required to be created for registry and sql server scripts running rest is all well. 
## 16-Sep-2023
Made small changes in *.csproj files to update the output and software assembly
## 25-Sep-2023
Using Wixv3 requires a full project process to make the msi installer. However, I am trying to get along. I can not understand that Wix and VS 2022 has not properly integrated. I can either compile for x64 or x86 and that by changing the wix project file. Tutorial I am following is asking to use "Typora" for creating license agreement files for installation UI display.