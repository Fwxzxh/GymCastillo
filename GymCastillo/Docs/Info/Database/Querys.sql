-- Querys
-- Consultas generales
SELECT * FROM cliente;
SELECT * FROM clase;
SELECT * FROM tipoinstructor;
SELECT * FROM tipocliente;
SELECT * FROM instructor;
SELECT * FROM rentas;
SELECT * FROM usuario;
SELECT * FROM clienterenta;
SELECT * FROM locker;
SELECT * FROM pagos;
SELECT * FROM ingresos;

-- Cliente
	-- Consulta de todo lo de cliente
SELECT
    c.IdCliente, c.Nombre, c.ApellidoMaterno,
    c.ApellidoPaterno, c.Domicilio, c.FechaNacimiento,
    c.Telefono, c.NombreContacto, c.TelefonoContacto,
    c.Foto, c.CondicionEspecial, c.FechaUltimoAcceso,
    c.MontoUltimoPago, c.Activo, c.FechaVencimientoPago,
    c.DeudaCliente, c.MedioConocio, c.MedioConocio,
    c.ClasesTotalesDisponibles, c.ClasesSemanaDisponibles,
    c.Descuento, c.Nino,
    p.IdPaquete, p.NombrePaquete,
    tc.IdTipoCliente, tc.NombreTipoCliente,
    l.IdLocker, l.Nombre as NombreLocker
FROM cliente c
INNER JOIN paquete p ON c.IdPaquete = p.IdPaquete
INNER JOIN tipocliente tc ON c.IdTipoCliente = tc.IdTipoCliente
LEFT JOIN locker l ON c.IdLocker = l.IdLocker;
	-- Dar de alta
INSERT INTO cliente 
VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno, 
	@FechaNacimiento, @Domicilio, @Telefono, @CondicionEspecial, 
	@NombreContacto, @TelefonoContacto, @Foto, @FechaUltimoAcceso, 
	@MontoUltimoPago, @Activo, @FechaVencimientoPago, @DeudaCliente, 
	@MedioConocio, @ClasesTotalesDisponibles, @ClasesSemanaDisponible, 
	@Descuento, @Nino, @IdTipoCliente, @IdPaquete, @IdLocker);
	-- Editar valores (usuario)
UPDATE cliente
SET Domicilio=@Domicilio, Telefono=@Telefono, CondicionEspecial=@CondicionEspecial,
    NombreContacto=@NombreContacto, TelefonoContacto=@TelefonoContacto, Foto=@Foto,
    Activo=@Activo, MedioConocio=@MedioConocio, Descuento=@Descuento, Nino=@Nino,
    IdTipoCliente=@IdTipoCliente, IdPaquete=@IdPaquete, IdLocker=@IdLocker
WHERE IdCliente=@IdCliente;

UPDATE cliente
SET domicilio='domT', telefono='0123456789', condicionespecial=true,
    nombrecontacto='contactot', telefonocontacto='9876543210', foto=null,
    activo=true, medioconocio='lol', descuento=1, nino=false,
    idtipocliente=2, idpaquete=1
WHERE idcliente=13;
	-- Editar valores (automatico)

-- Instructor
	-- Consulta de todo lo de Instructor
SELECT
    i.IdInstructor, i.Nombre, i.ApellidoPaterno,
    i.ApellidoMaterno, i.Domicilio, i.FechaNacimiento,
    i.Telefono, i.NombreContacto, i.TelefonoContacto,
    i.Foto, i.FechaUltimoAcceso, i.FechaUltimoPago,
    i.MontoUltimoPago, i.HoraEntrada, i.HoraSalida,
    i.DiasATrabajar, i.DiasTrabajados, i.Sueldo,
    i.SueldoADescontar,
    ti.IdTipoInstructor, ti.NombreTipoInstructor,
    group_concat(c.IdClase) as IdClase, group_concat(c.NombreClase) as NombreClase
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
SELECT
    cr.IdClienteRenta, cr.Nombre, cr.ApellidoPaterno,
    cr.ApellidoPaterno, cr.Domicilio, cr.FechaNacimiento,
    cr.Telefono, cr.NombreContacto, cr.TelefonoContacto,
    cr.Foto, cr.FechaUltimoPago, cr.MontoUltimoPago,
    cr.DeudaCliente,
    group_concat(r.IdRenta) as IDRenta, group_concat(r.FechaRenta) as FechaRenta, group_concat(r.Costo) as CostoRenta
FROM clienterenta cr, rentas r
WHERE cr.IdClienteRenta = r.IdClienteRenta
GROUP BY IdClienteRenta;
	-- Dar de alta
INSERT INTO clienterenta
VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
        @Domicilio, @FechaNacimiento, @Telefono, @NombreContacto,
        @TelefonoContacto, @Foto, @FechaUltimoPago, @MontoUltimoPago,
        @DeudaCliente);
	-- Editar valores (usuario)
UPDATE clienterenta
SET Domicilio=@Domicilio, Telefono=@Telefono,
    NombreContacto=@NombreContacto, TelefonoContacto=@TelefonoContacto,
    Foto=@Foto
WHERE IdClienteRenta=@IdClienteRenta;
	-- Editar valores (automatico)

-- Usuarios
	-- Consulta de todo lo de Usuarios
SELECT
    u.IdUsuario, u.Nombre, u.ApellidoPaterno,
    u.ApellidoMaterno, u.Domicilio, u.Username,
    u.Password, u.FechaNacimiento, u.Telefono,
    u.NombreContacto, u.TelefonoContacto,
    u.Foto, u.FechaUltimoAcceso,
    u.FechaUltimoPago, u.MontoUltimoPago
FROM usuario u;
	-- Dar de alta
INSERT INTO usuario
VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
	    @Domicilio, @Username, @Password, @FechaNacimiento,
	    @Telefono, @NombreContacto, @TelefonoContacto, @Foto,
	    @FechaUltimoAcceso, @FechaUltimoPago, @MontoUltimoPago);
	-- Editar valores (usuario)
UPDATE usuario
SET domicilio=@Domicilio, username=@Username, password=@Password,
    telefono=@Telefono, NombreContacto=@NombreContacto,
    telefonocontacto=@TelefonoContacto, foto=@Foto
WHERE IdUsuario=@IdUsuario;
	-- Editar valores (automatico)


-- Clases
    -- Obtener todas las clases.
select
    c.IdClase, c.NombreClase, c.Descripcion,
    c.CupoMaximo, c.Activo,
    i.IdInstructor, i.Nombre, i.ApellidoPaterno,
    e.IdEspacio, e.NombreEspacio
from clase c
left join instructor i on c.IdInstructor = i.IdInstructor
left join espacio e on e.IdEspacio = c.IdEspacio;


-- Clases
-- Consulta todo clase con horario:
SELECT c.IdClase, c.NombreClase, c.Descripcion,
c.CupoMaximo, c.Activo,
i.IdInstructor, i.Nombre, i.ApellidoPaterno,
e.IdEspacio, e.NombreEspacio,
group_concat(h.Día) Dia, group_concat(h.HoraInicio) HoraDeInicio, group_concat(h.HoraFin) HoraDeTermino
FROM clase c
LEFT JOIN instructor i ON c.IdInstructor = i.IdInstructor
LEFT JOIN espacio e ON e.IdEspacio = c.IdEspacio
LEFT JOIN horario h ON h.IdClase = c.IdClase
group by c.IdClase;

-- Consulta todo clase sin horario:
SELECT c.IdClase, c.NombreClase, c.Descripcion,
c.CupoMaximo, c.Activo,
i.IdInstructor, i.Nombre, i.ApellidoPaterno,
e.IdEspacio, e.NombreEspacio
FROM clase c
LEFT JOIN instructor i ON c.IdInstructor = i.IdInstructor
LEFT JOIN espacio e ON e.IdEspacio = c.IdEspacio;

-- Alta de clases
INSERT INTO clase
VALUES (default, @NombreClase, @Descripcion, 
@CupoMaximo, @Activo, @IdInstructor, @IdEspacio);

-- Actulización de clases
UPDATE clase
SET cupomaximo=@CupoMaximo, activo=@Activo,
idinstructor=@IdInstructor, idespacio=@IdEspacio
WHERE idclase=@IdClase;


-- Paquetes
-- Consulta todo paquete
SELECT p.IdPaquete, p.Gym, p.NombrePaquete,
p.NumClasesTotales, p.NumClasesSemanales, p.Costo,
c.IdClase, c.NombreClase
FROM paquete p
LEFT JOIN clase c ON c.IdClase = p.IdClase;

-- Alta Paquetes
INSERT INTO paquete
VALUES (default, @Gym, @NombrePaquete,
@NumClasesTotales, @NumClasesSemanales,
@Costo, @IdClase);

-- Actulización paquetes
UPDATE paquete
SET gym=@Gym, nombrepaquete=@NombrePaquete,
numclasestotales=@NumClasesTotales, 
numclasessemanales=@NumClasesSemanales,
costo=@Costo, idclase=@IdClase
WHERE idpaquete=@IdPaquete;
