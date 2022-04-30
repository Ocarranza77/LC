<xsl:stylesheet version='1.0'
 xmlns='urn:schemas-microsoft-com:office:spreadsheet'
 xmlns:xsl='http://www.w3.org/1999/XSL/Transform'
 xmlns:msxsl='urn:schemas-microsoft-com:xslt'
 xmlns:user='urn:my-scripts'
 xmlns:o='urn:schemas-microsoft-com:office:office'
 xmlns:x='urn:schemas-microsoft-com:office:excel'
  xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet' >
  <xsl:template match='/'>
  <Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'
  xmlns:o='urn:schemas-microsoft-com:office:office'
  xmlns:x='urn:schemas-microsoft-com:office:excel'
  xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet'
  xmlns:html='http://www.w3.org/TR/REC-html40'>
 <Styles>
    <Style ss:ID='head'>
      <Alignment ss:Horizontal='Center' ss:Vertical='Center' ss:WrapText='1'/>
      <Font ss:FontName='Arial' x:Family='Swiss' ss:Color='#FFFFFF'/>
      <Interior ss:Color='#F16527' ss:Pattern='Solid'/>
    </Style>
     <Style ss:ID="money">
   <Alignment ss:Horizontal="Right" ss:Vertical="Center" ss:WrapText="1"/>
   <Borders/>
   <!--<Font ss:FontName="Calibri" x:Family="Swiss" ss:Size="11" ss:Color="#000000"/>-->
   <Interior/>
   <NumberFormat ss:Format="&quot;$&quot;#,##0.00"/>
   <Protection/>
  </Style>
   <Style ss:ID="sdate">
    <NumberFormat ss:Format="dd/mm/yyyy;@"/>
  </Style>
  <Style ss:ID="stime">
    <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
  </Style>
   <Style ss:ID="snombre">
    <Alignment ss:Horizontal="Left" ss:Vertical="Center" ss:WrapText="1"/>
  </Style>
</Styles>  

<xsl:apply-templates/>
  </Workbook>
  </xsl:template>


   <xsl:template match='/*'>
   <Worksheet>
   <xsl:attribute name='ss:Name'>
   <xsl:value-of select='local-name(/*/*)'/>
  </xsl:attribute>



<Table x:FullColumns='1' x:FullRows='1'>

<Column ss:Width='150.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='100.25' />
<Column ss:Width='150.25' />
   
 <Row>
 <Cell><Data ss:Type='String'></Data></Cell>
 </Row>
 
 <Row>
 <Cell><Data ss:Type='String'>Usuario  </Data></Cell>
 <Cell><Data ss:Type='String'>
 <xsl:value-of select='Usuario'/>
 </Data></Cell>
 </Row> 


 
 <Row>
 <Cell><Data ss:Type='String'>Fecha </Data></Cell>
 <Cell><Data ss:Type='String'>
<xsl:value-of select='Fecha'/> 
 </Data></Cell>
 </Row>
 <Row>
 <Cell><Data ss:Type='String'></Data></Cell>
 </Row>
     <Row>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>SUCURSAL</Data>
          </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>FECHA Z</Data>
        </Cell>
          <Cell ss:StyleID='head'>
            <Data ss:Type='String'>HORA CIERRE</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>FOLIO Z</Data>
        </Cell>
       
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>NUM. CAJA</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>TRANSACCIONES</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>CAJEROS</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>EFECTIVO</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>T.CREDITO</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>T.DEBITO</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>O.F.DE PAGO</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>TOTAL</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>USUARIO QUE CAPTURA</Data>
        </Cell>




        
     </Row>  

     <xsl:for-each select='//CorteZ'>
     <Row>
         

      
        
        <Cell>
            <Data ss:Type='String'>
           
            <xsl:value-of select='Sucursal/Nombre'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='sdate'>
            <Data ss:Type='DateTime'>
              
            <xsl:value-of select='FechaVta'/>
        </Data>
        </Cell>

         <Cell ss:StyleID='stime'>
            <Data ss:Type='String'>
            <xsl:value-of select='Hora'/>
        </Data>
        </Cell>

         <Cell>
            <Data ss:Type='Number'>
            <xsl:value-of select='TicketID'/>
        </Data>
        </Cell>
         <Cell>
            <Data ss:Type='Number'>
            <xsl:value-of select='CajaID'/>
        </Data>
           </Cell>



         <Cell>
            <Data ss:Type='Number'>
            <xsl:value-of select='Transacciones'/>
        </Data>
        </Cell>

         <Cell>
            <Data ss:Type='String'>
            <xsl:value-of select='Cajeros'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='Efectivo'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='TCredito'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='TDebito'/>
        </Data>
        </Cell>
        <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='OtraFormaPago'/>
        </Data>
        </Cell>
        <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='Total'/>
        </Data>
        </Cell>
        <Cell>
          <Data ss:Type='String'>
            <xsl:value-of select="CodUsAltaNombre"/>
          </Data>
        </Cell>
       

   </Row>
  </xsl:for-each>
  </Table>
  </Worksheet>


  </xsl:template>
  </xsl:stylesheet>