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
idtipocliente=@IdTipoCliente, idpaquete=@IdPaquete
WHERE idcliente=@IdCliente;
	-- Editar valores (automatico)

-- Instructor
	-- Consulta de todo lo de Instructor
SELECT i.IdInstructor, i.Nombre, i.ApellidoPaterno,
i.ApellidoMaterno, i.Domicilio, i.FechaNacimiento,
i.Telefono, i.NombreContacto, i.TelefonoContacto,
i.Foto, i.FechaUltimoAcceso, i.FechaUltimoPago,
i.MontoUltimoPago, i.HoraEntrada, i.HoraSalida,
i.DiasATrabajar, i.DiasTrabajados, i.Sueldo,
i.SueldoADescontar,
ti.IdTipoInstructor, ti.NombreTipoInstructor,
group_concat(c.IdClase) IDClase, group_concat(c.NombreClase) NombreClase
FROM instructor i
INNER JOIN tipoinstructor ti ON i.IdTipoInstructor = ti.IdTipoInstructor
LEFT JOIN clase c ON c.IdInstructor = i.IdInstructor
GROUP BY i.IdInstructor;
	-- Dar de alta
INSERT INTO instructor
VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
	@Domicilio, @FechaNacimiento, @Telefono, @NombreContacto,
	@TelefonoContacto, @Foto, @FechaUltimoAcceso, @FechaUltimoPago,
	@MontoUltimoPago, @HoraEntrada, @HoraSalida, @DiasATrabajar,
	@DiasTrabajados, @Sueldo, @SueldoADescontar, @IdTipoInstructor);
	-- Editar valores (usuario)
UPDATE instructor
SET domicilio=@Domicilio, telefono=@Telefono, NombreContacto=@NombreContacto,
telefonocontacto=@TelefonoContacto, foto=@Foto, horaentrada=@HoraEntrada,
horasalida=@HoraSalida, sueldo=@Sueldo, sueldoadescontar=@SueldoADescontar,
idtipoinstructor=@IdTipoInstructor
WHERE idinstructor=@IdInstructor;
	-- Editar valores (automatico)

-- ClienteRenta
	-- Consulta de todo lo de ClienteRenta
SELECT cr.IdClienteRenta, cr.Nombre, cr.ApellidoPaterno,
cr.ApellidoPaterno, cr.Domicilio, cr.FechaNacimiento,
cr.Telefono, cr.NombreContacto, cr.TelefonoContacto,
cr.Foto, cr.FechaUltimoPago, cr.MontoUltimoPago,
cr.DeudaCliente,
group_concat(r.IdRenta) IDRenta, group_concat(r.FechaRenta) FechaRenta, group_concat(r.Costo) Costo
FROM clienterenta cr, rentas r
WHERE cr.IdClienteRenta = r.IdClienteRenta
GROUP BY IdClienteRenta;
	-- Dar de alta
INSERT INTO clienterenta
VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
	@Domicilio, @FechaNacimiento, @Telefono, @NombreContacto,
	@TelefonoContacto, @Foto, @FechaUltimoPago, @MontoUltimoPago, 
	@DeudaCliente;
	-- Editar valores (usuario)
UPDATE clienterenta
SET domicilio=@Domicilio, telefono=@Telefono, 
NombreContacto=@NombreContacto, telefonocontacto=@TelefonoContacto, 
foto=@Foto,
WHERE clienterenta=@IdClienteRenta;
	-- Editar valores (automatico)

-- Usuarios
	-- Consulta de todo lo de Usuarios
	-- Dar de alta
	-- Editar valores (usuario)
	-- Editar valores (automatico)
