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

  <xsl:template match="/StockData">
    <html>
      <body>
        <h4>Companion POS</h4>
        <h5>0300-1234567</h5>
        <h5>NTN</h5>
        <!-- <br/> -->
        <h1> Product </h1>
        <xsl:for-each select="Product">
          <table border="1">
            <tr>
              <td>
                Product ID
              </td>
              <td>
                <xsl:value-of select="ProductID"/>
              </td>
            </tr>
            <tr>
              <td>
                Name
              </td>
              <td>
                <xsl:value-of select="Name"/>
              </td>
            </tr>
            <tr>
              <td>
                Formula
              </td>
              <td>
                <xsl:value-of select="Formula"/>
              </td>
            </tr>
            <tr>
              <td>
                Manufacturer
              </td>
              <td>
                <xsl:value-of select="Manufacturer"/>
              </td>
            </tr>
            <tr>
              <td>
                Retail Price
              </td>
              <td>
                <xsl:value-of select="RetailPrice"/>
              </td>
            </tr>
            <tr>
              <td>
                Purchase Price
              </td>
              <td>
                <xsl:value-of select="PurchasePrice"/>
              </td>
            </tr>
            <tr>
              <td>
                Available Stock
              </td>
              <td>
                <xsl:value-of select="Quantity"/>
              </td>
            </tr>
          </table>
        </xsl:for-each>
        <table border="1">
          <tr>
            <th>Sr</th>
            <th>Quantity Added</th>
            <th>Purchase Rate</th>
            <th>Retail Price</th>
            <th>Added On</th>
            <th>Added By</th>
            <th>Supplier</th>
            <th>Supplier Contact</th>
          </tr>

          <h1> Stock </h1>
          <xsl:for-each select="Stock">
            <tr>
              <td>
                <xsl:value-of select="Sr"/>
              </td>
              <td>
                <xsl:value-of select="QuantityAdded"/>
              </td>
              <td>
                <xsl:value-of select="PurchasePrice"/>
              </td>
              <td>
                <xsl:value-of select="RetailPrice"/>
              </td>
              <td>
                <xsl:value-of select="Added"/>
              </td>
              <td>
                <xsl:value-of select="AddedBy"/>
              </td>
              <td>
                <xsl:value-of select="Supplier"/>
              </td>
              <td>
                <xsl:value-of select="SupplierContact"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
        <h3>END</h3>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
