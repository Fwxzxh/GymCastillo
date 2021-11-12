-- Querys
-- Consultas generales
SELECT * FROM clientes;
SELECT * FROM clases;
SELECT * FROM clienteclases;
SELECT * FROM tipocliente;
SELECT * FROM instructor;
SELECT * FROM instructorclase;
SELECT * FROM usuario;
SELECT * FROM clienterenta;
SELECT * FROM renta;
SELECT * FROM pagos;
SELECT * FROM ingresos;

-- Consultas de información
	-- Full cliente
SELECT  ci.idcliente, ci.nombre, ci.ApellidoPaterno, ci.ApellidoMaterno, ci.FechaNacimiento, ci.Telefono, ci.CondicionEspecial, ci.NombreContacto, ci.TelefonoContacto,
ci.FechaUltimoAcceso, ci.MontoUltimoPago, ci.Activo, ci.Asistencias, ci.DeudaCliente,
ci.IdTipoCliente, tc.NombreTipoCliente, 
group_concat(ca.IdClase) IDClase, group_concat(ca.NombreClase) NomClase
FROM cliente ci, clase ca, clienteclase cc, tipocliente tc
WHERE cc.IdCliente = ci.IdCliente
AND cc.IdClase = ca.IdClase
AND tc.IdTipoCliente = ci.IdTipoCliente
group by ci.IdCliente;

-- Consultas de modificación
SELECT 
FROM ;

-- Updates
	-- 1.- Todos los editables de cliente (modificar si/no locker preguntar)
UPDATE cliente
SET telefono=@telefono, domicilio=@domicilio, condicionespecial=@condicionespecial,
nombrecontacto=@nombrecontacto, telefonocontacto=@telefonocontacto, idtipocliente=@idtipocliente
WHERE idcliente=@idcliente;
	-- 2.- Todos los editables de clase
UPDATE clase
SET costohora=@costohora, horario=@horario 
WHERE idclase=@idclase;
	-- 3.- Todos los editables de instructor
UPDATE instructor
SET telefono=@telefono, domicilio=@domicilio, condicionespecial=@condicionespecial, nombrecontacto=@nombrecontacto, 
telefonocontacto=@telefonocontacto, pagohora=@pagohora, fechaultimopago=@fechaultimopago, montoultimopago=@montoultimopago
WHERE idinstructor=@idinstructor;	
	-- 4.- Inactividad de cliente
UPDATE cliente
SET status=FALSE 
WHERE idcliente=@idcliente;
	-- 5.- Todos los editables de cliente (modificar si/no locker preguntar)
UPDATE usuario
SET telefono=@telefono, domicilio=@domicilio, username=@username, password=@password, condicionespecial=@condicionespecial,
nombrecontacto=@nombrecontacto, telefonocontacto=@telefonocontacto
WHERE idusuario=@idusuario;
	-- 5.- Todos los editables de cliente (modificar si/no locker preguntar)
UPDATE clienterenta
SET telefono=@telefono, domicilio=@domicilio, condicionespecial=@condicionespecial, nombrecontacto=@nombrecontacto,
telefonocontacto=@telefonocontacto
WHERE clienterenta=@clienterenta;


-- Drops
	-- 1.- Eliminación CLIENTE
DROP cliente
WHERE idcliente=@idcliente;
	-- 2.- Baja de CLASES A CLIENTE
DROP clienteclase
WHERE idcliente=@idcliente AND idclase=@idclase;
	-- 3.- Baja de CLASES A INSTRUCTORES
DROP instructorclase
WHERE idinstructor=@idinstructor AND idclase=@idclase;
	-- 4.- Eliminación CLASES
DROP clase
WHERE idclase=@idclase;
	-- 5.- Eliminación TIPOCLIENTE
DROP tipocliente
WHERE idtipocliente=@idtipocliente;
	-- 6.- Eliminación INSTRUCTOR
DROP instructor
WHERE idinstructor=@idinstructor;
	-- 7.- Eliminación USUARIO
DROP usuario
WHERE idusuario=@idusuario;
	-- 8.- Eliminación CLIENTERENTA
DROP clienterenta
WHERE idclienterenta=@idclienterenta;
	-- 9.- Eliminación RENTA
DROP renta
WHERE idrenta=@idrenta;
	-- 10.- Eliminación Pagos
DROP pagos
WHERE idpagosgeneral=@idpagosgeneral;
	-- 11.- Eliminación INGRESOS
DROP ingresos
WHERE idingresos=@idingresos;

-- Inserts
	-- Cliente
INSERT INTO cliente VALUES (default, @nombre, @apellidopaterno, @apellidomaterno, @fechanacimiento, @telefono, @condicionespecial, @nombrecontacto, @telefonocontacto, @foto, @fechaultimoacceso, @montoultimopago, @activo, @asistencias, @fechavencimientopago, @idtipocliente, @deudacliente, @medioconocio, @locker);
	-- Clase
INSERT INTO clase VALUES (default, @nombreclase, @descripcion, @costohora, @horario, @estatus);
	-- ClienteClase
INSERT INTO clienteclase VALUES (@idcliente, @idclase);
	-- Instructor
INSERT INTO instructor VALUES (default, @nombre, @apellidopaterno, @apellidomaterno, @fechanacimiento, @telefono, @condicionespecial, @nombrecontacto, @telefonocontacto, @foto, @fechaultimoacceso, @fechaultimopago, @montoultimopago, @asistencias, @pagohora);
	-- InstructorClase
INSERT INTO instructorclase VALUES (@idinstructor, @idclase);
	-- Usuario
INSERT INTO usuario VALUES (default, @nombre, @apellidopaterno, @apellidomaterno, @username, @password, @fechanacimiento, @telefono, @condicionespecial, @nombrecontacto, @telefonocontacto, @foto, @fechaultimoacceso, @fechaultimopago, @montoultimopago);
	-- ClienteRenta
INSERT INTO clienterenta VALUES (default, @nombre, @apellidopaterno, @apellidomaterno, @fechanacimiento, @telefono, @condicionespecial, @nombrecontacto, @telefonocontacto, @foto, @fechaultimopago, @montoultimopago, @deudacliente);
	-- Renta
INSERT INTO renta VALUES (default, @idclienterenta, @fecharenta, @horario);
	-- Pagos
INSERT INTO pagos VALUES (default, @fecharegistro, @idusuario, @tipopago, @concepto, @numerorecibo, @monto);
	-- Ingresos
INSERT INTO ingresos VALUES (default, @fecharegistro, @idusuario, @tipopago, @concepto, @numerorecibo, @monto);
