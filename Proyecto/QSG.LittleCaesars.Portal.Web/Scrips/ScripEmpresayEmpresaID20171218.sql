/****** Object:  StoredProcedure [dbo].[getCorteSucursales]    Script Date: 12/18/2017 5:40:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Carlos Felipe Soto Ocio
-- Create date: 14 Mar 2015
-- Modificated: 22 Jul 2015: Carlos Soto; Se agrego la restriccion de SucursalUsuario, 
--					para ello se agrego el parametro @UsRqst.
--					ESTO no aplica en este SP---- Tambien se le dio funcionamiento al parametro @TipoTicket: 0.- Todos 1.- Omite Factura Publico en General
--				03 Sep 2015: Carlos Soto; Se agrego que se ordene por nombre sucursal.
--				18 Ene 2017; Carlos Soto; Se agrego la empresa a la que pertenece la sucursal.
--				18 Dic 2017; Carlos Soto; Se Modifico NomreEmpresa por Empresa y se agrego EmpresaID
-- Description:	Regresa una lista de CortesSucursal, solo se puede hacer consulta por valores directos,
--					No hay busqueda por rango o valores multiples de un mismo tipo.
--					@Entidad puede variar, aunque de forma predeterminada esta para un solo ticket, 
--						pero se puede modificar por ejemplo a Tickets/Ticket para grabar una lista.
-- =============================================
ALTER PROCEDURE [dbo].[getCorteSucursales] (
	@prmXML xml, 
	@Entidad varchar(100) = 'CorteSucursalFilter', 
	@TipoTicket int = 0, 
	@UsRqst int = 1, 
	@Msg varchar(max) out)
AS
BEGIN

 DECLARE @hdoc int

 DECLARE @SucursalID varchar(20)
 DECLARE @FechaVta varchar(25)
 DECLARE @CodUsuario varchar(20)
 DECLARE @Importe varchar(25)

 DECLARE @Sucursales varchar(250)
 DECLARE @FechaVtaHasta varchar(25)
 DECLARE @ImporteHasta varchar(25)

 DECLARE @SQL varchar(max)
 DECLARE @OP varchar(10)

 SET @SucursalID = 0
 SET @FechaVta = null
 SET @CodUsuario = 0
 SET @Importe = 0

 SET @Sucursales = ''
 SET @FechaVtaHasta = null
 SET @ImporteHasta = 0

 SET @SQL = ''
 SET @OP = 'WHERE '

BEGIN TRY 

 EXEC sp_xml_preparedocument @hdoc output, @prmXML

-- OPTENER VALORES DE  B U S Q U E D A

 SELECT @SucursalID = SucursalID, @FechaVta = FechaVta,
	@CodUsuario = CodUsuario, @Sucursales = Sucursales, @FechaVtaHasta = FechaVtaHasta,
	@Importe = Total, @ImporteHasta = TotalHasta
 FROM openxml(@hdoc, @Entidad,2)
	With (
	OperationType varchar(10), 
	SucursalID int 'Sucursal/SucursalID', 
	Total varchar(25),
	FechaVta date, 
	CodUsuario int, 
	Sucursales varchar(250),
	FechaVtaHasta varchar(25),
	TotalHasta varchar(25)
	) 

 --exec sp_xml_removedocument @hdoc

 --SELECT @SucursalID 'Sucursal', @CajaID 'Caja', @FechaVta 'FechaVta', @Cajero 'Cajero', @FechaCaptura 'FechaCaptura', 
 --	@CodUsuario 'CodUsuario', @FechaFactura 'FechaFactura', @RFC 'RFC',
	--@Sucursales 'Sucursales', @Cajas 'Cajas', @FechaVtaHasta 'FechaVtaHasta', @FechaCapturaHasta 'FechaCapturaHasta',
	--@FechaFacturaHasta 'FechaFacturaHasta'

 SET @SQL =  'SELECT C.*, S.Abr, S.Nombre, U.Nombre NomUsuario, EC.Nombre Empresa, EC.EmpresaID   
  FROM CorteSucursal C
	INNER JOIN Sucursales S ON S.SucursalID = C.SucursalID
	INNER JOIN SucursalesUsuarios SU ON SU.SucursalID = S.SucursalID And SU.UsuarioPermisoID = ' + LTrim(RTrim(Cast(@UsRqst as varchar(10)))) + ' 
	LEFT  JOIN EmpresaCliente EC ON EC.EmpresaID = S.EmpresaID
	LEFT  JOIN Usuarios U ON U.CodUsuario = C.CodUsuario 
'

  
 IF @SucursalID <> '0' 
 Begin
	SET @SQL =  @SQL + @OP + 'C.SucursalID = ' + @SucursalID + '
	'
	SET @OP = ' And '
 End 

-- Esto no opera en este SP
 --IF @TipoTicket = 1
 --Begin
	--SET @SQL =  @SQL + @OP + 'C.SucursalID = ' + @SucursalID + '
	--'
	--SET @OP = ' And '
 --End

 IF @Sucursales <> ''
 Begin
	SET @SQL =  @SQL + @OP + 'C.SucursalID in(' + @Sucursales + ')
	'
	SET @OP = ' And '
 End 

 
 IF @Importe <> '0'
 Begin
	SET @SQL =  @SQL + @OP + 'C.Total >= ' + @Importe + '
	'
	SET @OP = ' And '
 End

 IF @ImporteHasta <> '0'
 Begin
	SET @SQL =  @SQL + @OP + 'C.Total <= ' + @ImporteHasta + '
	'
	SET @OP = ' And '
 End

 IF @FechaVta > '2012/01/01' 
 Begin
 	SET @SQL =  @SQL + @OP + 'C.FechaVta >= ''' + @FechaVta  + '''
	'
	SET @OP = ' And '
 End

 IF @FechaVtaHasta > '2012/01/01' 
 Begin
 	SET @SQL =  @SQL + @OP + 'C.FechaVta <= ''' + @FechaVtaHasta  + '''
	'
	SET @OP = ' And '
 End


 IF @CodUsuario <> '0'
 Begin
	SET @SQL =  @SQL + @OP + 'T.CodUsuario = ' + @CodUsuario + '
	'
	SET @OP = ' And '
 End


 SET @SQL =  @SQL + 'ORDER BY S.SucursalID
 '

-- Print @SQL
 EXEC (@SQL)


 --SELECT @SucursalID 'Sucursal', @CajaID 'Caja', @FechaVta 'FechaVta', @Cajero 'Cajero', @FechaCaptura 'FechaCaptura', 
 --	@CodUsuario 'CodUsuario', @FechaFactura 'FechaFactura', @RFC 'RFC',
	--@Sucursales 'Sucursales', @Cajas 'Cajas', @FechaVtaHasta 'FechaVtaHasta', @FechaCapturaHasta 'FechaCapturaHasta',
	--@FechaFacturaHasta 'FechaFacturaHasta'

END TRY
BEGIN CATCH

   SET @Msg = @Msg + '<<DB.getTickets:  Numero de Error: ' + CAST(ERROR_NUMBER() AS VARCHAR(10)) + ',  Mensaje: ' + ERROR_MESSAGE() + ' >>'
END CATCH



END


















