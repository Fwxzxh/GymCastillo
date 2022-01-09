-- Queries -- Consultas generales
SELECT * FROM cliente;
SELECT * FROM clase;
SELECT * FROM tipoinstructor;
SELECT * FROM tipocliente;
SELECT * FROM instructor;
SELECT * FROM rentas;
SELECT * FROM usuario;
SELECT * FROM clienterenta;
SELECT * FROM locker;
SELECT * FROM egresos;
SELECT * FROM ingresos;

-- Cliente
	-- Consulta de lo de cliente
SELECT
    c.IdCliente, c.Nombre, c.ApellidoMaterno,
    c.ApellidoPaterno, c.FechaNacimiento, c.Telefono, 
    c.NombreContacto, c.TelefonoContacto, c.Foto, 
    c.CondicionEspecial, c.DescripcionCondicionEspecial,
    c.FechaUltimoAcceso, c.MontoUltimoPago, c.Activo, 
    c.FechaUltimoPago, c.FechaVencimientoPago, 
    c.DeudaCliente, c.MedioConocio,
    c.ClasesTotalesDisponibles, c.ClasesSemanaDisponibles,
    c.DuracionPaquete, c.Nino,
    p.IdPaquete, p.NombrePaquete,
    tc.IdTipoCliente, tc.NombreTipoCliente,
    l.IdLocker, l.Nombre as NombreLocker,
    c.ChatID, c.FechaIngreso
FROM cliente c
LEFT JOIN paquete p ON c.IdPaquete = p.IdPaquete
LEFT JOIN tipocliente tc ON c.IdTipoCliente = tc.IdTipoCliente
LEFT JOIN locker l ON c.IdLocker = l.IdLocker;

	-- Dar de alta
INSERT INTO cliente
    (IdCliente, Nombre, ApellidoPaterno, ApellidoMaterno,
     FechaNacimiento, Telefono, CondicionEspecial,
     DescripcionCondicionEspecial, NombreContacto,
     TelefonoContacto, Foto, Activo, MedioConocio,
     Nino, IdTipoCliente, FechaRenta)
VALUES
    (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
     @FechaNacimiento, @Telefono, @CondicionEspecial,
     @DescripcionCondicionEspecial, @NombreContacto,
     @TelefonoContacto, @Foto, @Activo, @MedioConocio,
     @Nino, @IdTipoCliente, @FechaRenta);
	-- Editar valores (usuario)

UPDATE cliente
SET Telefono=@Telefono, CondicionEspecial=@CondicionEspecial, DescripcionCondicionEspecial=@DescripcionCondicionEspecial,
    NombreContacto=@NombreContacto, TelefonoContacto=@TelefonoContacto, Foto=@Foto,
    Activo=@Activo, MedioConocio=@MedioConocio, DuracionPaquete=@DuracionPaquete, Nino=@Nino,
    IdTipoCliente=@IdTipoCliente, ChatID=@ChatID
WHERE IdCliente=@IdCliente;

-- Instructor
	-- Consulta de lo de Instructor
SELECT
    i.IdInstructor, i.Nombre, i.ApellidoPaterno,
    i.ApellidoMaterno, i.Domicilio, i.FechaNacimiento,
    i.Telefono, i.NombreContacto, i.TelefonoContacto,
    i.Foto, i.FechaUltimoAcceso, i.FechaUltimoPago,
    i.MontoUltimoPago, i.HoraEntrada, i.HoraSalida,
    i.DiasATrabajar, i.DiasTrabajados, i.Sueldo,
    i.SueldoADescontar, i.MetodoFechaPago,
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
	@DiasTrabajados, @Sueldo, @SueldoADescontar, @MetodoFechaPago,
	@IdTipoInstructor);
	-- Editar valores (usuario)
UPDATE instructor
SET domicilio=@Domicilio, telefono=@Telefono, NombreContacto=@NombreContacto,
telefonocontacto=@TelefonoContacto, foto=@Foto, horaentrada=@HoraEntrada,
horasalida=@HoraSalida, sueldo=@Sueldo, sueldoadescontar=@SueldoADescontar,
metodofechapago=@MetodoFechaPago, idtipoinstructor=@IdTipoInstructor
WHERE idinstructor=@IdInstructor;
	-- Editar valores (automatico)

-- ClienteRenta
	-- Consulta de lo de ClienteRenta
SELECT
    cr.IdClienteRenta, cr.Nombre, cr.ApellidoPaterno,
    cr.ApellidoPaterno, cr.Domicilio, cr.FechaNacimiento,
    cr.Telefono, cr.NombreContacto, cr.TelefonoContacto,
    cr.Foto, cr.FechaUltimoPago, cr.MontoUltimoPago,
    cr.DeudaCliente,
    group_concat(r.IdRenta) as IDRenta,
    group_concat(r.FechaRenta) as FechaRenta,
    group_concat(r.Costo) as CostoRenta
FROM clienterenta cr, rentas r
WHERE cr.IdClienteRenta = r.IdClienteRenta
GROUP BY IdClienteRenta;
Select * from clienterenta;
SELECT * from rentas where IdClienteRenta=@IdClienteRenta;
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
	-- Editar valores (automático)

-- Usuarios
	-- Consulta de lo de Usuarios
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
	-- Editar valores (automático)


-- Clases
    -- Obtener todas las clases.
select
    c.IdClase, c.NombreClase, c.Descripcion,
    c.CupoMaximo, c.Activo,
    i.IdInstructor, CONCAT(i.Nombre, ' ', i.ApellidoPaterno, ' ', i.ApellidoMaterno) as NombreInstructor,
    e.IdEspacio, e.NombreEspacio,
    group_concat(p.IdPaquete) as IdPaquete,
    group_concat(p.NombrePaquete) as NombrePaquete
FROM ClaseInstructores ci
left join clases c on c.IdClase = ci.IdClase
left join instructores i on i.IdInstructor = ci.IdInstructor
left join espacio e on e.IdEspacio = c.IdEspacio
left join paquetesclases pc on c.IdClase = pc.IdClase
left join paquete p on pc.IdPaquete = p.IdPaquete
group by c.IdClase;

-- Clases
-- Consulta clase con horario:
SELECT
    c.IdClase, c.NombreClase, c.Descripcion,
    c.CupoMaximo, c.Activo,
    i.IdInstructor, CONCAT(i.Nombre, ' ', i.ApellidoPaterno, ' ', i.ApellidoMaterno) as NombreInstructor,
    e.IdEspacio, e.NombreEspacio,
    group_concat(h.Dia) as Dia,
    group_concat(h.HoraInicio) HoraDeInicio,
    group_concat(h.HoraFin) HoraDeTermino,
    group_concat(p.IdPaquete) as IdPaquete,
    group_concat(p.NombrePaquete) as NombrePaquete
FROM ClaseInstructores ci
left join clases c on c.IdClase = ci.IdClase
left join instructores i on i.IdInstructor = ci.IdInstructor
left join espacio e on e.IdEspacio = c.IdEspacio
left join paquetesclases pc on c.IdClase = pc.IdClase
left join paquete p on pc.IdPaquete = p.IdPaquete
left join horario h on c.IdClase = h.IdClase
group by c.IdClase;

-- Consulta clase sin horario:
SELECT
    c.IdClase, c.NombreClase, c.Descripcion,
    c.CupoMaximo, c.Activo,
    i.IdInstructor, CONCAT(i.Nombre, ' ', i.ApellidoPaterno, ' ', i.ApellidoMaterno) as NombreInstructor,
    e.IdEspacio, e.NombreEspacio,
    group_concat(p.IdPaquete) as IdPaquete,
    group_concat(p.NombrePaquete) as NombrePaquete
FROM ClaseInstructores ci
left join clases c on c.IdClase = ci.IdClase
left join instructores i on i.IdInstructor = ci.IdInstructor
left join espacio e on e.IdEspacio = c.IdEspacio
left join paquetesclases pc on c.IdClase = pc.IdClase
left join paquete p on pc.IdPaquete = p.IdPaquete
group by c.IdClase;

-- Alta de clases
INSERT INTO clase
VALUES	(default, @NombreClase, @Descripcion,
		@CupoMaximo, @Activo, @IdEspacio);

-- Actualización de clases
UPDATE	clase
SET	cupomaximo=@CupoMaximo, activo=@Activo, idespacio=@IdEspacio
WHERE	idclase=@IdClase;

-- Paquetes
-- Consulta paquete
SELECT
    p.IdPaquete, p.Gym, p.NombrePaquete,
    p.Descripcion, p.NumClasesTotales,
    p.NumClasesSemanales, p.Costo,
    group_concat(c.IdClase) as IdClase,
    group_concat(c.NombreClase) as NombreClase
FROM paquete p
left join paquetesclases pc on pc.IdPaquete = p.IdPaquete
left join clase c on c.IdClase = pc.IdClase
group by p.IdPaquete;

-- Alta Paquetes
INSERT INTO paquete
VALUES
    (default, @Gym, @NombrePaquete,
    @Descripcion, @NumClasesTotales, 
    @NumClasesSemanales, @Costo);

-- Actualización paquetes
UPDATE paquete
SET gym=@Gym, nombrepaquete=@NombrePaquete,
    descripcion=@Descripcion,
    numclasestotales=@NumClasesTotales,
    numclasessemanales=@NumClasesSemanales,
    costo=@Costo
WHERE idpaquete=@IdPaquete;


-- PaquetesClases
INSERT INTO paquetesclases
VALUES (@IdPaquete, @IdClase);

UPDATE paquetesclases
SET (@IdPaquete, @IdClase);

DELETE 
FROM paquetesclases
WHERE idpaquete=@IdPaquete
AND idclase=@IdClase;

-- Horarios
-- Consulta horario
SELECT h.idhorario, h.dia, h.horainicio,
h.horafin, h.cupoactual,
c.idclase, c.nombreclase
FROM horario h
LEFT JOIN clase c ON c.idclase=h.idclase;

-- Alta horarios
INSERT INTO horario
VALUES
    (default, @Dia, @HoraInicio,
    @HoraFin, @CupoActual, @IdClase);

-- Actualización horarios
UPDATE horario
SET dia=@Dia, horainicio=@HoraInicio,
    horafin=@HoraFin, idclase=@IdClase
WHERE idhorario=@IdHorario;

-- Egresos
	-- Consulta Egresos
SELECT
    p.IdPagosGeneral, p.FechaRegistro,
    p.IdUsuario, CONCAT(u.Nombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) as NombreUsuario,
    p.Servicios, p.Nomina, p.IdUsuarioPagar,
    CONCAT(up.Nombre, ' ', up.ApellidoPaterno, ' ', up.ApellidoMaterno) as NombreUsuarioPagar,
    p.IdInstructor, CONCAT(i.Nombre, ' ', i.ApellidoPaterno, ' ', i.ApellidoMaterno) as NombreInstructor,
    p.IdPersonal, CONCAT(ps.Nombre, ' ', ps.ApellidoPaterno, ' ', ps.ApellidoMaterno) as NombrePersonal,
    p.Otros, p.Concepto, p.NumeroRecibo, p.Monto
FROM egresos p
INNER JOIN usuario u ON p.IdUsuario = u.IdUsuario
LEFT JOIN usuario up ON p.IdUsuarioPagar = up.IdUsuario
LEFT JOIN instructor i ON p.IdInstructor = i.IdInstructor
LEFT JOIN personal ps ON p.IdPersonal = ps.IdPersonal;
	-- Alta pagos
INSERT INTO egresos
VALUES
    (@IdPagosGeneral, @FechaRegistro, @IdUsuario,
    @Servicios, @Nomina, @Otros, @IdUsuarioPagar,
    @IdInstructor, @IdPersonal, @Concepto, 
    @NumeroRecibo, @Monto);


-- Ingresos
	-- Consulta Ingresos
SELECT
    i.IdIngresos, i.FechaRegistro,
    i.IdUsuario, CONCAT(u.Nombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) as NombreUsuario,
    i.IdRenta, r.FechaRenta, i.IdCliente,
    CONCAT(c.Nombre, ' ', c.ApellidoPaterno, ' ', c.ApellidoMaterno) as NombreCliente,
    i.IdClienteRenta,
    CONCAT(cr.Nombre, ' ', cr.ApellidoPaterno, ' ', cr.ApellidoMaterno) as NombreClienteRenta, i.IdVenta,
    i.Otros, i.Concepto,
    i.IdPaquete, p.NombrePaquete,
    i.IdLocker, l.Nombre,
    i.NumeroRecibo, i.Monto, i.MontoRecibido
FROM ingresos i
INNER JOIN usuario u ON i.IdUsuario = u.IdUsuario
LEFT JOIN rentas r ON i.IdRenta = r.IdRenta
LEFT JOIN cliente c ON i.IdCliente = c.IdCliente
LEFT JOIN clienterenta cr ON i.IdClienteRenta = cr.IdClienteRenta
LEFT JOIN ventas v ON i.IdVenta = v.IdVenta
LEFT JOIN paquete p ON i.IdPaquete = p.IdPaquete
LEFT JOIN locker l ON i.IdLocker = l.IdLocker;
	-- Alta ingresos
INSERT INTO ingresos
VALUES
    (@IdIngresos, @FechaRegistro, @IdUsuario,
    @IdRenta, @IdCliente, @IdVenta, @IdClienteRenta, @Otros, @Concepto,
    @IdPaquete, @IdLocker, @NumeroRecibo, @Monto,
    @MontoRecibido);
    

-- Personal
SELECT
    p.IdPersonal, p.Nombre, p.ApellidoPaterno,
    p.ApellidoMaterno, p.Domicilio, p.Puesto,
    p.FechaNacimiento, p.Telefono, p.NombreContacto,
    p.TelefonoContacto, p.Foto,
    p.FechaUltimoPago, p.MontoUltimoPago
FROM personal p;
	-- Dar de alta
INSERT INTO personal
VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
	    @Domicilio, @Puesto, @FechaNacimiento, @Telefono, 
	    @NombreContacto, @TelefonoContacto, @Foto,
	    @FechaUltimoPago, @MontoUltimoPago);

	-- Editar valores (usuario)
UPDATE usuario
SET domicilio=@Domicilio, telefono=@Telefono, 
    NombreContacto=@NombreContacto,
    telefonocontacto=@TelefonoContacto, foto=@Foto
WHERE IdUsuario=@IdUsuario;
	-- Editar valores (automático)


-- Inventario
	-- Consulta Productos
SELECT	i.IdProducto, i.NombreProducto,
	i.Descripcion, i.Costo, i.Existencias
FROM	inventario i

	-- Dar de alta productos
INSERT INTO inventario
VALUES
    (default, @NombreProducto,
    @Descripcion, @Costo, @Existencias);

	-- Actualizar productos
UPDATE inventario
SET Descripcion=@Descripcion, Costo=@Costo,
    Existencias=@Existencias
WHERE IdProducto=@IdProducto;


-- Rentas
	-- Consulta Rentas
SELECT
    r.IdRenta, r.FechaRenta, r.IdClienteRenta,
    CONCAT(cr.Nombre, ' ', cr.ApellidoPaterno, ' ', cr.ApellidoMaterno) as NombreCliente,
    r.IdEspacio, e.NombreEspacio, r.HoraInicio, 
    r.HoraFin, r.Costo, r.MontoRecibido
FROM rentas r
LEFT JOIN clienterenta cr ON r.IdClienteRenta = cr.IdClienteRenta
LEFT JOIN espacio e ON e.IdEspacio = r.IdEspacio;

	-- Dar de alta Rentas
INSERT INTO rentas
VALUES (default, @FechaRenta, @IdClienteRenta,
       @IdEspacio, @NombreEspacio,
       @HoraInicio, @HoraFin, @Costo, @MontoRecibido);
       
       -- Actualizar Rentas Creo que no debería poder actualizar nada, pero idk pongo todoxd
UPDATE	rentas
SET	FechaRenta=@FechaRenta, IdClienteRenta=@IdClienteRenta,
	IdEspacio=@IdEspacio, NombreEspacio=@NombreEspacio,
	HoraInicio=@HoraInicio, HoraFin=@HoraFin, Costo=@Costo,
	MontoRecibido=@MontoRecibido
WHERE	IdRenta=@IdRenta;


-- Ventas
SELECT	v.IdVenta, v.FechaVenta, v.IdsProductos,
	v.VisitaGym, v.Concepto, v.Costo
FROM	ventas v;

	-- Alta ventas
INSERT INTO ventas
VALUES (default, @FechaVenta, @IdsProductos,
       @VisitaGym, @Concepto, @Costo);
       
       -- Actualizar ventas
UPDATE	ventas
SET	FechaVenta=@FechaVenta, IdsProductos=@IdsProductos,
       	VisitaGym=@VisitaGym, Concepto=@Concepto, Costo=@Costo
WHERE	IdVenta=@IdVenta;


-- ClaseInstructores
-- Hacer que una clase tenga un instructor
INSERT INTO ClaseInstructores
VALUES (@IdClase, @IdInstructor);

-- Para actualizar: borrar y luego a dar de alta, para borrar:
DELETE FROM ClaseInstructores 
WHERE IdClase = @IdClase
AND IdInstructor = @IdInstructor;


-- ClienteHorario
INSERT INTO clientehorario
VALUES (@IdCliente, @IdHoraio);

DELETE FROM clientehorario
WHERE IdCliente = @IdCliente
AND IdHorario = @IdHorario;


-- Consulta cupo maximo, cupo actual y cuantos clientes escritos en una hora
SELECT
	COUNT(ch.IdCliente) AS NumeroClientes, h.CupoActual, c.CupoMaximo
FROM
	clientehorario ch
LEFT JOIN horario h ON h.IdHorario = ch.IdHorario
LEFT JOIN clase c ON h.IdClase = c.IdClase
WHERE ch.IdHorario = @IdHorario;
