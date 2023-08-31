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

<xsl:template match="/ReceiptData">
  <html>
    <body>
      <h4>Companion POS</h4>
      <h5>0300-1234567</h5>
      <h5>NTN</h5>
      <!-- <br/> -->
      <xsl:for-each select="Data">      
      <h5>InvoiceNo: 
      <xsl:value-of select="InvoiceNo"/>
      </h5>
      <h5>Customer: 
      <xsl:value-of select="Customer"/>
      </h5>
      </xsl:for-each>
      <table border="1">
        <tr>
          <th>Sr</th>
          <th>Name</th>
          <th>Quantity</th>
          <th>Total</th>        
      </tr>
      <xsl:for-each select="Drug">
        <tr>
          <td>
            <xsl:value-of select="Sr"/>
          </td>
          <td>
            <xsl:value-of select="Name"/>
          </td>
          <td>
            <xsl:value-of select="Quantity"/>
          </td>
          <td>
            <xsl:value-of select="Total"/>
          </td>        
        </tr>      
      </xsl:for-each>
      </table> 
      <xsl:for-each select="Data">      
      <h5>Created:
      <xsl:value-of select="Checkout"/>
      </h5>
      <h5>GrandTotal: 
      <xsl:value-of select="GrandTotal"/>
      </h5>
      <h5>Payment: 
      <xsl:value-of select="Payment"/>
      </h5>
        <h5>Balance: 
      <xsl:value-of select="Balance"/>
      </h5>
      </xsl:for-each>  
      <h3>Thank You for Visiting</h3>
    </body>  
  </html>
</xsl:template>
</xsl:stylesheet>
