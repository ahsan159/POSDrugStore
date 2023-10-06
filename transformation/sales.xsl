<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
>
  <!--xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"-->
  <!--<xsl:output method="xml" indent="yes"/>-->

  <!--<xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>-->

  <xsl:template match="/SalesData">
    <html>
      <body>
        <h4>Companion POS</h4>
        <h5>0300-1234567</h5>
        <h5>NTN</h5>
        <!-- <br/> -->
        <xsl:for-each select="Data">
          <table border="1">
            <tr>
              <th>
                From Date
              </th>
              <td>
                <xsl:value-of select="FromDate"/>
              </td>
            </tr>
            <tr>
              <th>
                To Date
              </th>
              <td>
                <xsl:value-of select="ToDate"/>
              </td>
            </tr>
            <tr>
              <th>
                Total Sales
              </th>
              <td>
                <xsl:value-of select="TotalSales"/>
              </td>
            </tr>
          </table>          
        </xsl:for-each>
        <h2>Sales</h2>
        <table border="1">
          <tr>
            <th>Sr</th>
            <th>Invoice No.</th>
            <th>Customer Name</th>
            <th>Contact No.</th>
            <th>Total</th>
            <th>Discount</th>
            <th>Items Count</th>
            <th>Checout Date</th>
            <th>Checkout Time</th>            
          </tr>
          <xsl:for-each select="Sale">
            <tr>
              <td>
                <xsl:value-of select="Sr"/>
              </td>
              <td>
                <xsl:value-of select="InvoiceNo"/>
              </td>
              <td>
                <xsl:value-of select="CustomerName"/>
              </td>
              <td>
                <xsl:value-of select="CustomerNo"/>
              </td>
              <td>
                <xsl:value-of select="Total"/>
              </td>
                <td>
                <xsl:value-of select="Discount"/>
              </td>
                <td>
                <xsl:value-of select="ItemsCount"/>
              </td>
                <td>
                <xsl:value-of select="Date"/>
              </td>
                <td>
                <xsl:value-of select="Time"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
        <h3>Thank You for Visiting</h3>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
