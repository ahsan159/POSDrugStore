using System;
using System.Windows;
using System.Collections;
using System.Data.SqlClient;
using DataWrapper;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace XMLReportGenerator
{
    public class ReportGenerator
    {        
        public ReportGenerator()
        {
            
        }
        public void InvoiceReportGenerator(string invoiceNo)
        {
            sqlWrapper wrapper = sqlWrapper.getInstance();
            DataTable InvoiceData = wrapper.getInvoiceData(invoiceNo);
            DataTable Invoice = wrapper.getTable(invoiceNo);
            XElement ele = new XElement("ReceiptData");
            DataRow iData = InvoiceData.Rows[0];
            XElement dataEle = new XElement("Data");
            dataEle.Add(new XElement("user", iData["UserName"]));
            dataEle.Add(new XElement("Customer", iData["Customer"]));
            dataEle.Add(new XElement("Checkout", iData["CheckoutDate"] + " " + iData["CheckoutTime"]));
            dataEle.Add(new XElement("GrandTotal", iData["Total"]));
            dataEle.Add(new XElement("Payment", iData["Payment"]));
            dataEle.Add(new XElement("Balance", iData["Balance"]));
            ele.Add(dataEle);

            foreach (DataRow dr in Invoice.Rows)
            {
                XElement dEle = new XElement("Drug");
                XElement sEle = new XElement("Sr", dr["Sr"].ToString());
                XElement nEle = new XElement("Name", dr["Name"].ToString());
                XElement qEle = new XElement("Quantity", dr["Quantity"].ToString());
                XElement cEle = new XElement("Disc", dr["Discount"].ToString());
                XElement tEle = new XElement("Total", dr["Total"].ToString());
                dEle.Add(sEle);
                dEle.Add(nEle);
                dEle.Add(qEle);
                dEle.Add(cEle);
                dEle.Add(tEle);
                ele.Add(dEle);
            }
            XDocument doc = new XDocument();
            doc.Add(ele);
            string pathXML = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\report.xml";
            string pathXSL = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\report.xsl";
            string pathHTM = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\report.html";
            doc.Save(pathXML);
            XslCompiledTransform trans = new XslCompiledTransform();
            trans.Load(pathXSL);
            trans.Transform(pathXML, pathHTM);
        }
        public void PrductReportGenerator(string  ProductID)
        {
            sqlWrapper wrapper = sqlWrapper.getInstance();
            DataTable productData = wrapper.getProductData(ProductID.ToString());
            DataTable stockData = wrapper.getProductStock(ProductID.ToString());

            Dictionary<string, string> ProductDict = new Dictionary<string, string>();
            ProductDict.Add("id", "ProductID");
            ProductDict.Add("name", "Name");
            ProductDict.Add("manufacturer", "Manufacturer");
            ProductDict.Add("Cost", "RetailPrice");
            ProductDict.Add("costIn", "PurchasePrice");
            ProductDict.Add("quantity", "Quantity");
            ProductDict.Add("formula", "Formula");

            Dictionary<string, string> StockDict = new Dictionary<string, string>();
            StockDict.Add("QuantityAdded","QuantityAdded");
            StockDict.Add("Purchase","PurchasePrice");
            StockDict.Add("Retail","RetailPrice");
            StockDict.Add("Added","Added");
            StockDict.Add("Users","AddedBy");
            StockDict.Add("Supplier","Supplier");
            StockDict.Add("SupplierContact","SupplierContact");            

            XElement mainElement = new XElement("StockData");
            XElement ProductXElement;

            if (productData.Rows.Count > 0)
            {
                DataRow dr = productData.Rows[0];
                ProductXElement = new XElement("Product");
                foreach (KeyValuePair<string,string> kvp in ProductDict)
                {
                    ProductXElement.Add(new XElement(kvp.Value, dr[kvp.Key]));
                }
                mainElement.Add(ProductXElement);
            }

            int i = 1;
            foreach(DataRow sdr in stockData.Rows)
            {
                XElement StockXElement = new XElement("Stock");
                StockXElement.Add(new XElement("Sr", i++));
                foreach (KeyValuePair<string,string> kvp in StockDict)
                {
                    StockXElement.Add(new XElement(kvp.Value, sdr[kvp.Key]));
                }
                mainElement.Add(StockXElement);
            }

            XDocument doc = new XDocument();
            doc.Add(mainElement);
            
            string pathXML = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\stockreport.xml";
            string pathXSL = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\stock.xsl";
            string pathHTM = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\report.html";

            doc.Save(pathXML);
            XslCompiledTransform trans = new XslCompiledTransform();
            trans.Load(pathXSL);
            trans.Transform(pathXML, pathHTM);
        }

        public void SalesReportGenerator(DataTable saleTable,string from, string to, string totalSales)
        {
            Dictionary<string, string> SalesDict = new Dictionary<string, string>();
            SalesDict.Add("Invoice", "InvoiceNo");
            SalesDict.Add("Customer", "CustomerName");
            SalesDict.Add("Total", "Total");
            SalesDict.Add("Discount", "Discount");
            SalesDict.Add("DrugCount", "ItemsCount");
            SalesDict.Add("Contact", "CustomerContact");
            SalesDict.Add("CheckoutDate", "Date");
            SalesDict.Add("CheckoutTime", "Time");
            SalesDict.Add("UserName", "Personal");

            XElement SalesXElement = new XElement("SalesData");

            XElement DataXElement = new XElement("Data");
            DataXElement.Add(new XElement("FromDate", from));
            DataXElement.Add(new XElement("ToDate", to));
            DataXElement.Add(new XElement("TotalSales", totalSales));
            SalesXElement.Add(DataXElement);

            int i = 0;
            foreach(DataRow dr in saleTable.Rows)
            {
                XElement SaleElement = new XElement("Sale");
                SaleElement.Add(new XElement("Sr", i++));
                foreach (KeyValuePair<string, string> kvp in SalesDict)
                {
                    SaleElement.Add(new XElement(kvp.Value, dr[kvp.Key]));
                }
                SalesXElement.Add(SaleElement);
            }

            XDocument doc = new XDocument();
            doc.Add(SalesXElement);

            string pathXML = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\salesreport.xml";
            string pathXSL = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\sales.xsl";
            string pathHTM = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\report.html";

            doc.Save(pathXML);
            XslCompiledTransform trans = new XslCompiledTransform();
            trans.Load(pathXSL);
            trans.Transform(pathXML, pathHTM);
        }
    }
}
