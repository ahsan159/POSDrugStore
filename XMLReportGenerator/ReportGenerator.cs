using System;
using System.Windows;
using System.Collections;
using System.Data.SqlClient;
using DataWrapper;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace XMLReportGenerator
{
    public class ReportGenerator
    {        
        public ReportGenerator(string invoiceNo)
        {
            sqlWrapper wrapper = sqlWrapper.getInstance();
            DataTable InvoiceData = wrapper.getInvoiceData(invoiceNo);
            DataTable Invoice = wrapper.getTable(invoiceNo);
            XElement ele = new XElement("ReceiptData");
            DataRow iData = InvoiceData.Rows[0];
            XElement dataEle = new XElement("Data");
            dataEle.Add(new XElement("user", iData["UserName"]));
            dataEle.Add(new XElement("Customer",iData["Customer"]));
            dataEle.Add(new XElement("Checkout",iData["CheckoutDate"] + " " + iData["CheckoutTime"]));
            dataEle.Add(new XElement("GrandTotal",iData["Total"]));
            dataEle.Add(new XElement("Payment",iData["Payment"]));
            dataEle.Add(new XElement("Balance",iData["Balance"]));
            ele.Add(dataEle);
            
            foreach(DataRow dr in Invoice.Rows)
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
    }
}
