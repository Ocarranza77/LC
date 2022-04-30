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
<Column ss:Width='150.25' />
<Column ss:Width='70.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='75.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
<Column ss:Width='150.25' />
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
            <Data ss:Type='String'>SUPERVISOR</Data>
        </Cell>
          <Cell ss:StyleID='head'>
            <Data ss:Type='String'>FECHA DAILY</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>NUMERO ZS</Data>
        </Cell>
       
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>TOTAL INGRESOS</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>TARJETA CREDITO</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>TARJETA DEBITO</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>O.F DE PAGO</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>TOTAL EFECTIVO DAILY</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>TOTAL EFECTIVO ZS</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>VARIACION DAILY</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>TIPO DE CAMBIO</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>PESOS A DEPOSITAR</Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>DOLARES A DEPOSITAR</Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>CONVERSION PESOS</Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>SERVICIO BLINDADO PESOS</Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>SERVICIO BLINDADO DOLARES</Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>CONVERSION PESOS</Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>FOLIO </Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>GASTOS/DEUDORES</Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>SOBRANTE</Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>CAJERO CORTO</Data>
        </Cell>
        <Cell ss:StyleID='head'>
            <Data ss:Type='String'>FALTANTES</Data>
        </Cell>
         <Cell ss:StyleID='head'>
            <Data ss:Type='String'>COMENTARIOS</Data>
        </Cell>
       



        
     </Row>  

     <xsl:for-each select='//CorteSucursal'>
     <Row>
         

      
        
        <Cell>
            <Data ss:Type='String'>
            <xsl:value-of select='Sucursal/Nombre'/>
        </Data>
        </Cell>
         <Cell>
            <Data ss:Type='String'>
           
            <xsl:value-of select='Supervisor'/>
        </Data>
        </Cell>
         
         <Cell ss:StyleID='sdate'>
            <Data ss:Type='DateTime'>
              
            <xsl:value-of select='FechaVta'/>
        </Data>
        </Cell>

         <Cell>
            <Data ss:Type='Number'>
            <xsl:value-of select='NoZ'/>
        </Data>
      </Cell>
        <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='Total'/>
        </Data>
        </Cell>
        <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='TotalTCredito'/>
        </Data>
        </Cell>
        <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='TotalTDebito'/>
        </Data>
        </Cell>

        <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='TotalOtraFormaPago'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='0'/>
        </Data>
        </Cell>
        <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='EfectivoZ'/>
        </Data>
        </Cell>

        <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='0'/>
        </Data>
        </Cell>

         <Cell ss:StyleID='stime'>
            <Data ss:Type='String'>
            <xsl:value-of select='TC'/>
        </Data>
        </Cell>

         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='PesosADeposito'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='DolarADeposito'/>
        </Data>
        </Cell>

         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='0'/>
        </Data>
        </Cell>

         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='PesosSB'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='DolarSB'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='0'/>
        </Data>
        </Cell>

         <Cell>
            <Data ss:Type='String'>
            <xsl:value-of select='FolioFactura'/>
        </Data>
        </Cell>

         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='Gastos'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='Sobrante'/>
        </Data>
        </Cell>
         <Cell>
            <Data ss:Type='String'>
            <xsl:value-of select='CajeroCorto'/>
        </Data>
        </Cell>
         <Cell ss:StyleID='money'>
            <Data ss:Type='Number'>
            <xsl:value-of select='Faltante'/>
        </Data>
        </Cell>

         <Cell >
            <Data ss:Type='String'>
            <xsl:value-of select='Comentarios'/>
        </Data>
        </Cell>
       

   </Row>
  </xsl:for-each>
  </Table>
  </Worksheet>

 






  </xsl:template>

  </xsl:stylesheet>

