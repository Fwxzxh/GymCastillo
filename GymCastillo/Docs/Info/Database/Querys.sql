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
	-- Consulta de todo lo de cliente
SELECT c.IdCliente, c.Nombre, c.ApellidoMaterno, 
c.ApellidoPaterno, c.Domicilio, c.FechaNacimiento, 
c.Telefono, c.NombreContacto, c.TelefonoContacto, 
c.Foto, c.CondicionEspecial, c.FechaUltimoAcceso,
c.MontoUltimoPago, c.Activo, c.FechaVencimientoPago, 
c.DeudaCliente, c.MedioConocio, c.MedioConocio, 
c.ClasesTotalesDisponibles, c.ClasesSemanaDisponibles, 
c.Descuento, c.Nino, 
p.IdPaquete, p.NombrePaquete,
tc.IdTipoCliente, tc.NombreTipoCliente,
l.IdLocker, l.Nombre
FROM cliente c
INNER JOIN paquete p ON c.IdPaquete = p.IdPaquete
INNER JOIN tipocliente tc ON c.IdTipoCliente = tc.IdTipoCliente
LEFT JOIN locker l ON c.IdCliente = l.IdCliente;
	-- Dar de alta
INSERT INTO cliente 
VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno, 
	    @FechaNacimiento, @Domicilio, @Telefono, @CondicionEspecial,
	    @NombreContacto, @TelefonoContacto, @Foto, @FechaUltimoAcceso,
	    @MontoUltimoPago, @Activo, @FechaVencimientoPago, @DeudaCliente,
	    @MedioConocio, @ClasesTotalesDisponibles, @ClasesSemanaDisponible,
	    @Descuento, @Nino, @IdTipoCliente, @IdPaquete);

-- Editar valores (usuario)
UPDATE cliente
SET domicilio=@Domicilio, telefono=@Telefono, condicionespecial=@CondicionEspecial,
nombrecontacto=@NombreContacto, telefonocontacto=@TelefonoContacto, foto=@Foto,
activo=@Activo, medioconocio=@MedioConocio, descuento=@Descuento, nino=@Nino,
idtipocliente=@IdTipoCliente, idpaquete=@IdPaquete;
	-- Editar valores (automatico)
