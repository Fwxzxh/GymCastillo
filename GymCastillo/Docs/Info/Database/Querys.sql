-- Querys
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

-- Consultas de información
	-- Full cliente
SELECT ci.IdCliente,ci.Nombre,ci.ApellidoPaterno,ci.ApellidoMaterno,ci.FechaNacimiento,
       ci.Telefono,ci.CondicionEspecial,ci.NombreContacto,ci.TelefonoContacto,
       ci.FechaUltimoAcceso,ci.MontoUltimoPago,ci.Activo,ci.Asistencias,
       ci.FechaVencimientoPago,ci.DeudaCliente,ci.medioconocio,ci.Locker,
       ci.IdTipoCliente,tc.NombreTipoCliente,
       group_concat(ca.NombreClase) as NombreClase
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
UPDATE cliente SET
        Telefono=@telefono, CondicionEspecial=@CondicionEspecial,
        NombreContacto=@NombreContacto,
        TelefonoContacto=@telefonoContacto,
        IdTipoCliente=@IdTipoCliente,
        Activo=@Activo
WHERE IdCliente=@IdCliente;

	-- 2.- Todos los editables de clase
UPDATE clase
SET costohora=@costohora, horario=@horario 
WHERE idclase=@idclase;
	-- 3.- Todos los editables de instructor
UPDATE instructor
SET pagohora=@pagohora, fechaultimopago=@fechaultimopago, montoultimopago=@montoultimopago
WHERE idinstructor=@idinstructor;	
	-- 4.- Inactividad de cliente
UPDATE cliente
SET Activo=FALSE
WHERE idcliente=@idcliente;

-- Drops
	-- 1.- Eliminación CLIENTE
DROP  cliente
WHERE idcliente=@idcliente;
	-- 2.- Baja de CLASES A CLIENTE
DROP clienteclase
WHERE idcliente=@idcliente AND idclase=@idclase;
	-- 3.- Baja de CLASES A INSTRUCTORES
DROP instructorclase
WHERE idinstructor=@idinstructor AND idclase=@idclase;

-- Inserts
	-- 
-- INSERT INTO cliente VALUES (default,,,,);
