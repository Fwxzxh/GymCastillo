﻿-- Querys
-- Consultas generales
SELECT * FROM cliente;
SELECT * FROM clase;
SELECT * FROM clienteclase;
SELECT * FROM tipocliente;
SELECT * FROM instructor;
SELECT * FROM instructorclase;
SELECT * FROM usuario;
SELECT * FROM clienterenta;
SELECT * FROM renta;
SELECT * FROM pagos;
SELECT * FROM ingresos;

-- 4037070112318268

-- Consultas de información
	-- Full Cliente
SELECT  ci.IdCliente, ci.Nombre, ci.ApellidoPaterno, ci.ApellidoMaterno, ci.FechaNacimiento,
        ci.Telefono, ci.CondicionEspecial, ci.NombreContacto, ci.TelefonoContacto,
        ci.FechaUltimoAcceso, ci.MontoUltimoPago, ci.Activo, ci.Asistencias,
        ci.FechaVencimientoPago, ci.DeudaCliente, ci.MedioConocio, ci.Locker,
        ci.IdTipoCliente, tc.NombreTipoCliente,
        group_concat(ca.NombreClase) as NombreClase
FROM tipocliente tc
INNER JOIN cliente ci ON ci.IdTipoCliente = tc.IdTipoCliente
LEFT JOIN clienteclase cc ON cc.IdCliente = ci.IdCliente
LEFT JOIN clase ca ON ca.IdClase = cc.IdClase
group by ci.IdCliente;

	-- Clases Activas
SELECT c.idclase, c.NombreClase, c.Descripcion, c.CostoHora, c.Horario, c.Activo,
		i.idinstructor, i.Nombre, i.ApellidoPaterno, i.ApellidoMaterno
FROM clase c, instructorclase ic, instructor i
WHERE c.IdClase = ic.IdClase
AND ic.IdInstructor = i.IdInstructor
AND Activo = 1;


-- Updates
	-- 1.- Todos los editables de cliente (modificar si/no locker preguntar)
UPDATE cliente
SET telefono=@Telefono, domicilio=@Domicilio, condicionespecial=@CondicionEspecial,
nombrecontacto=@NombreContacto, telefonocontacto=@TelefonoContacto, idtipocliente=@IdTipoCliente
WHERE idcliente=@IdCliente;
	-- 2.- Todos los editables de clase
UPDATE clase
SET costohora=@CostoHora, horario=@Horario 
WHERE idclase=@IdClase;
	-- 3.- Todos los editables de instructor
UPDATE instructor
SET telefono=@Telefono, domicilio=@Domicilio, condicionespecial=@CondicionEspecial, 
nombrecontacto=@NombreContacto, telefonocontacto=@TelefonoContacto, pagohora=@PagoHora, 
fechaultimopago=@FechaUltimoPago, montoultimopago=@MontoUltimoPago
WHERE idinstructor=@IdInstructor;	
	-- 4.- Inactividad de cliente
UPDATE cliente
SET activo=FALSE
WHERE idcliente=@IdCliente;
	-- 5.- Baja temporal a clase
UPDATE clase
SET activo=FALSE
WHERE idclase=@IdClase;
	-- 6.- Todos los editables de usuario
UPDATE usuario
SET telefono=@Telefono, domicilio=@Domicilio, username=@Username, password=@Password, 
condicionespecial=@CondicionEspecial, nombrecontacto=@NombreContacto, telefonocontacto=@TelefonoContacto
WHERE idusuario=@IdUsuario;
	-- 7.- Todos los editables de clienterenta
UPDATE clienterenta
SET telefono=@Telefono, domicilio=@Domicilio, condicionespecial=@CondicionEspecial, 
nombrecontacto=@NombreContacto, telefonocontacto=@TelefonoContacto
WHERE IdClienteRenta=@IdClienteRenta;


-- Drops
	-- 1.- Eliminación CLIENTE
DELETE FROM cliente
WHERE idcliente=@IdCliente;
	-- 2.- Baja de CLASES A CLIENTE
DELETE FROM clienteclase
WHERE idcliente=@IdCliente AND idclase=@IdClase;
	-- 3.- Baja de CLASES A INSTRUCTORES
DELETE FROM instructorclase
WHERE idinstructor=@IdInstructor AND idclase=@IdClase;
	-- 4.- Eliminación CLASES
DELETE FROM clase
WHERE idclase=@IdClase;
	-- 5.- Eliminación TIPOCLIENTE
DELETE FROM tipocliente
WHERE idtipocliente=@IdTipoCliente;
	-- 6.- Eliminación INSTRUCTOR
DELETE FROM instructor
WHERE idinstructor=@IdInstructor;
	-- 7.- Eliminación USUARIO
DELETE FROM usuario
WHERE idusuario=@IdUsuario;
	-- 8.- Eliminación CLIENTERENTA
DELETE FROM clienterenta
WHERE idclienterenta=@IdClienteRenta;
	-- 9.- Eliminación RENTA
DELETE FROM renta
WHERE idrenta=@IdRenta;
	-- 10.- Eliminación Pagos
DELETE FROM pagos
WHERE idpagosgeneral=@IdPagosGeneral;
	-- 11.- Eliminación INGRESOS
DELETE FROM ingresos
WHERE idingresos=@IdIngresos;


-- Inserts
	-- Cliente
INSERT INTO cliente VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Domicilio, 
			    @FechaNacimiento, @Telefono, @CondicionEspecial, @NombreContacto, 
			    @TelefonoContacto, @Foto, @FechaUltimoAcceso, @MontoUltimoPago, 
			    @Activo, @Asistencias, @FechaVencimientoPago, @IdTipoCliente, 
			    @DeudaCliente, @MedioConocio, @Locker);
	-- Clase
INSERT INTO clase VALUES (default, @NombreClase, @Descripcion, @CostoHora, @Horario, @Activo);
	-- ClienteClase (alta)
INSERT INTO clienteclase VALUES (@IdCliente, @IdClase);
	-- Instructor
INSERT INTO instructor VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,  @Domicilio, @FechaNacimiento,
			       @Telefono, @CondicionEspecial, @NombreContacto, @TelefonoContacto, @Foto, 
			       @FechaUltimoAcceso, @FechaUltimoPago, @MontoUltimoPago, @Asistencias, @PagoHora);
	-- InstructorClase
INSERT INTO instructorclase VALUES (@IdInstructor, @IdClase);
	-- Usuario
INSERT INTO usuario VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Domicilio, @Username, @Password, 
			    @FechaNacimiento, @Telefono, @CondicionEspecial, @NombreContacto, @TelefonoContacto, 
			    @Foto, @FechaUltimoAcceso, @FechaUltimoPago, @MontoUltimoPago);
	-- ClienteRenta
INSERT INTO clienterenta VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Domicilio, @FechaNacimiento, 
				 @Telefono, @CondicionEspecial, @NombreContacto, @TelefonoContacto, @Foto, 
				 @FechaUltimoPago, @MontoUltimoPago, @DeudaCliente);
	-- Renta
INSERT INTO renta VALUES (default, @IdClienteRenta, @FechaRenta, @Horario);
	-- Pagos
INSERT INTO pagos VALUES (default, @FechaRegistro, @IdUsuario, @TipoPago, @Concepto, @NumeroRecibo, @Monto);
	-- Ingresos
INSERT INTO ingresos VALUES (default, @FechaRegistro, @IdUsuario, @TipoPago, @Concepto, @NumeroRecibo, @Monto);
