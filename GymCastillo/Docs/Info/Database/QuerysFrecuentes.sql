-- Pasos para realizar un pago a instructor o usuario
	-- Instructores
	-- idk Quisiera saber si cuando fue la última vez que le pague a un vato y cuanto:
    	SELECT CONCAT(i.Nombre, ' ', i.ApellidoPaterno, ' ', i.ApellidoMaterno) AS Nombre,
    	i.FechaUltimoPago, i.MontoUltimoPago
    	FROM instructores i
	WHERE idinstructor = @IdInstructor;
    	-- Luego procedería a hacer el pago:
    	INSERT INTO egresos
    	VALUES (default, @FechaRegistro, @IdUsuario, @Servicios, @Nomina, @Otros, @IdUsuarioPagar, @IdInstructor, @Concepto, @NumeroRecibo, @Monto);
    	-- Luego actulizaría los datos del instructor:
    	UPDATE instructor
    	SET fechaultimopago=@FechaUltimoPago, montoultimopago=@MontoUltimoPago, sueldoadescontar=@SueldoADescontar, faltas=@Faltas;
    	-- Y idk si quisiera saber cuando fue la última vez que le pagué a un pibardo:
    	SELECT e.fecharegistro, e.idinstructor, e.monto, e.concepto
    	FROM e.egresos
    	WHERE IdInstructor=@IdInstructor;
    
    	-- Usuarios
   	-- idk Quisiera saber si cuando fue la última vez que le pague a un vato y cuanto:
    	SELECT CONCAT(i.Nombre, ' ', i.ApellidoPaterno, ' ', i.ApellidoMaterno) AS Nombre,
    	i.FechaUltimoPago, i.MontoUltimoPago
    	FROM usuarios i
	WHERE idusuario = @IdUsuario;
    	-- Luego procedería a hacer el pago:
    	INSERT INTO egresos
    	VALUES (default, @FechaRegistro, @IdUsuario, @Servicios, @Nomina, @Otros, @IdUsuarioPagar, @IdInstructor, @Concepto, @NumeroRecibo, @Monto);
    	-- Luego actulizaría los datos del instructor:
    	UPDATE usuarios
    	SET fechaultimopago=@FechaUltimoPago, montoultimopago=@MontoUltimoPago;
    	-- Y idk si quisiera saber cuando fue la última vez que le pagué a un pibardo:
    	SELECT e.fecharegistro, e.idusuariopagar, e.monto, e.concepto
    	FROM e.egresos
    	WHERE IdUsuario=@IdUsuario;
 
 
-- Pasos para realizar un ingreso de un cliente o una renta (Aviso, primero tendría que crearse la renta y luego pagarse)
	-- Cliente
  	-- idk otra vez, quisiera saber hasta cuando tenía este men para pagar
    	SELECT CONCAT(c.Nombre, ' ', c.ApellidoPaterno, ' ', c.ApellidoMaterno) AS Nombre,
    	c.FechaVencimientoPago, c.MontoUltimoPago, c.DeudaCliente
    	FROM cliente c
    	WHERE idcliente=@IdCliente;
    	-- Ingresar el ingreso xd
    	INSERT INTO ingresos
    	VALUES (default, @FechaRegistro, @IdUsuario, @IdRenta, @IdCliente, @IdVenta, @Otros, @Concepto, @IdPaquete, @IdLocker, @NumeroRecibo, @Monto);
    	-- Actualizar los campos pertinentes en Cliente o Cliente Renta cuando pagan
    	UPDATE cliente
    	SET montoultimopago=@MontoUltimoPago, activo=@Activo, 
    	fechaultimopago=@FechaUltimoPago, fechavencimientopago=@fechavencimientopago,
    	deudacliente=@DeudaCliente, clasestotalesdisponibles=@ClasesTotalesDisponibles, 
    	clasessemanadisponibles=@ClasesSemanaDisponibles, duracionpaquete=@DuracionPaquete,
    	idpaquete=@IdPaquete, idlocker=@IdLocker;
    	-- idk de nuevo si quisiera saber cuando fue la última vez que ingrese dinero de un pibardo
    	SELECT i.fecharegistro, i.idcliente, i.monto, i.concepto
    	FROM ingresos i
    	WHERE Idcliente=@Idcliente;
    
    	-- Cliente Renta
    	-- idk otra vez, quisiera saber hasta cuando tenía este men para pagar
    	SELECT CONCAT(c.Nombre, ' ', c.ApellidoPaterno, ' ', c.ApellidoMaterno) AS Nombre,
    	c.FechaVencimientoPago, c.MontoUltimoPago, c.DeudaCliente
    	FROM clienterenta c
    	WHERE idclienterenta=@IdClienteRenta;
    	-- Ingresar el ingreso xd
    	INSERT INTO ingresos
    	VALUES (default, @FechaRegistro, @IdUsuario, @IdRenta, @IdCliente, @IdVenta, @Otros, @Concepto, @IdPaquete, @IdLocker, @NumeroRecibo, @Monto);
    	-- Actualizar los campos pertinentes en Cliente o Cliente Renta cuando pagan
	UPDATE clienterenta
	SET montoultimopago=MontoUltimoPago, fechaultimopago=@FechaUltimoPago,
	deudacliente=@DeudaCliente
	WHERE idclienterenta=@IdClienteRenta;
    	-- idk de nuevo si quisiera saber cuando fue la última vez que ingrese dinero de un pibardo
    	SELECT i.fecharegistro, i.idrenta, i.monto, i.concepto
    	FROM ingresos i
    	WHERE Idrenta=@IdRenta;
 
 
-- Pantalla de home
	-- Mostrando la info de los usuarios
    	SELECT  ci.idcliente, ci.Nombre, ci.ApellidoPaterno, 
    	ci.ApellidoMaterno, ci.Foto, ci.FechaVencimientoPago,
    	ci.ClasesTotalesDisponibles, ci.ClasesSemanaDisponibles,
    	ci.IdTipoCliente, t.NombreTipoCliente, ci.idpaquete, 
    	p.NombrePaquete, p.Costo, ca.IdClase, ca.NombreClase,
    	h.Dia, h.HoraInicio, h.HoraFin
    	FROM cliente ci
    	LEFT JOIN paquete p ON ci.IdPaquete = p.IdPaquete
    	LEFT JOIN clase ca ON p.IdClase = ca.IdClase
    	LEFT JOIN horario h ON ca.IdClase = h.IdClase
    	LEFT JOIN tipocliente t ON ci.IdTipoCliente = t.IdTipoCliente
	AND idcliente=@IdCliente;
    	-- Mostrando la info de los instructores
    	SELECT t.IdInstructor, t.Nombre, t.ApellidoPaterno, 
    	t.ApellidoMaterno, t.FechaUltimoPago, t.MontoUltimoPago,
    	t.Foto, t.IdTipoInstructor, t.MetodoFechaPago,
    	ti.NombreTipoInstructor, t.Sueldo, c.IdInstructor,
    	c.NombreClase, h.Dia, h.HoraInicio, h.HoraFin,
    	t.HoraEntrada, t.HoraSalida
    	FROM instructor t
    	LEFT JOIN clase c ON t.IdInstructor = c.IdInstructor
    	LEFT JOIN horario h ON c.IdClase = h.IdClase
    	LEFT JOIN tipoinstructor ti ON t.IdTipoInstructor = ti.IdTipoInstructor
    	WHERE t.IdInstructor = @IdInstructor;

    
-- Actualización de campos cuando entren al gym
	-- Cliente
    	UPDATE cliente
    	SET fechaultimoacceso=@FechaUltimoAcceso, 
    	clasestotalesdisponibles=@ClasesTotalesDisponibles, 
    	clasessemanadisponibles=@ClasesSemanaDisponibles
    	WHERE idcliente=@IdCliente;
    	-- Horarios
    	UPDATE horario 
    	SET cupoactual=@CupoActual
    	WHERE idclase=@IdClase;
    	-- Instructores
    	UPDATE instructor
    	SET fechaultimoacceso=@FechaUltimoAcceso,
    	sueldoadescontar=@SueldoADescontar,
    	diastrabajados=@DiasTrabajados
    	WHERE idinstructor=@IdInstructor;
