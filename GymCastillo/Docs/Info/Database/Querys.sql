-- Querys
-- Consultas generales
SELECT * FROM cliente;
SELECT * FROM clase;
SELECT * FROM tipoinstructor;
SELECT * FROM tipocliente;
SELECT * FROM instructor;
SELECT * FROM renta;
SELECT * FROM usuario;
SELECT * FROM clienterenta;
SELECT * FROM locker;
SELECT * FROM pagos;
SELECT * FROM ingresos;

-- Cliente
	-- Dar de alta
INSERT INTO cliente 
VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno, 
	@FechaNacimiento, @Domicilio, @Telefono, @CondicionEspecial, 
	@NombreContacto, @TelefonoContacto, @Foto, @FechaUltimoAcceso, 
	@MontoUltimoPago, @Activo, @FechaVencimientoPago, @DeudaCliente, 
	@MedioConocio, @ClasesTotalesDisponibles, @ClasesSemanaDisponible, 
	@Descuento, @Nino, @IdTipoCliente, @IdPaquete);
UPDATE cliente 
	-- Editar valores (usuario)
SET domicilio=@Domicilio, telefono=@Telefono, condicionespecial=@CondicionEspecial,
nombrecontacto=@NombreContacto, telefonocontacto=@TelefonoContacto, foto=@Foto,
activo=@Activo, medioconocio=@MedioConocio, descuento=@Descuento, nino=@Nino,
idtipocliente=@IdTipoCliente, idpaquete=@IdPaquete;
	-- Editar valores (automatico)
