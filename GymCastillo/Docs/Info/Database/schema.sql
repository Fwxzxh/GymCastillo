drop database if exists GymCastillo;

create database GymCastillo;

use GymCastillo;

create table TipoCliente (
    -- Tabla que guarda los registros de los tipos de Clientes, (de clase, con membresía, y membresía de promoción).
    IdTipoCliente int auto_increment primary key,
    NombreTipoCliente varchar(100),
    Descripcion varchar(2000)
);

create table TipoInstructor (
    -- Tabla que guarda los registros de los tipos de Instructores (de clase, instructor, instructor personal, otros).
    IdTipoInstructor int auto_increment primary key,
    NombreTipoInstructor varchar(100),
    Descripcion varchar(2000)
);

create table Espacio (
    -- Tabla que guarda los registros de los espacios dentro del establecimiento
    IdEspacio int auto_increment primary key,
    NombreEspacio varchar(100),
    Descripcion varchar(2000)
);

create table Instructor (
    -- Tabla que guarda los registros de los instructores del gym.
    IdInstructor int AUTO_INCREMENT PRIMARY KEY,
    Nombre varchar(30) not null not null,
    ApellidoPaterno varchar(30) not null,
    ApellidoMaterno varchar(30) not null,
    Domicilio varchar(150) not null,
    FechaNacimiento datetime not null,
    Telefono varchar(10) unique not null, -- Debe de ser único para poder mandar WhatsApp
    NombreContacto varchar(30),
    TelefonoContacto varchar(10),
    Foto blob,
    FechaUltimoAcceso datetime,
    FechaUltimoPago datetime,
    MontoUltimoPago decimal,
    HoraEntrada varchar(4), -- Con datetime no se puede manejar ya que está hora es fija, y es la hora de entrada de ese instructor
    HoraSalida varchar(4), -- Mismo caso que HoraEntrada pero está es la hora de salida
    DiasATrabajar int, -- Cuantos días debe trabajar esa semana/quincena/mes
    DiasTrabajados int, -- Cuantos días trabajó esa semana/quincena/mes
    Sueldo decimal not null,
    SueldoADescontar decimal, -- Opcional y calculable en backend
    MetodoFechaPago int, -- Serían 3: 1-Semanal, 2-Quincenal, 3-Mes 
    -- IdTipoInstructor
    IdTipoInstructor int,
    foreign key (IdTipoInstructor) references TipoInstructor (IdTipoInstructor)
);

create table Paquete (
    -- Tabla que guarda los registros de los paquetes
    IdPaquete int auto_increment primary key,
    Gym bool not null, -- Si el paquete tendrá o no gym
    NombrePaquete varchar(100),
    Descripcion varchar(300),
    NumClasesTotales int, -- Número de clases por mes
    NumClasesSemanales int, -- Número de clases por semana
    Costo decimal not null
);

create table Clase (
    -- Tabla que guarda los registros de las clases.
    IdClase int auto_increment primary key,
    NombreClase varchar(50) not null,
    Descripcion varchar(500) not null,
    CupoMaximo int not null, -- El cupo maximo de personas por hora
    Activo bool not null,
	-- IdInstructor
    IdInstructor int not null,
    foreign key (IdInstructor) references Instructor (IdInstructor),
	-- IdEspacio
    IdEspacio int,
    foreign key (IdEspacio) references Espacio (IdEspacio)
);

create table PaquetesClases (
	-- Tabla para que los paquetes tengan varias clases y las clases varios paquetes
	IdPaquete int,
	foreign key (IdPaquete) references Paquete (IdPaquete),
	IdClase int,
	foreign key (IdClase) references Clase (IdClase)
);

create table Horario (
    -- Tabla que guarda los registros de los horarios.
    IdHorario int auto_increment primary key,
    Dia int not null,
    HoraInicio varchar(4) not null,
    HoraFin varchar(4) not null,
    CupoActual int, -- Cuantas personas hay en ese momento
	-- IdClase
    IdClase int,
    foreign key (IdClase) references Clase (IdClase)
);

create table Locker (
    -- Tabla que guarda la información de los lockers
                        IdLocker int auto_increment primary key,
                        Nombre varchar(10),
    -- Para saber si dicho locker esta ocupado
                        Ocupado bool
);

create table Cliente (
    -- Tabla Que guarda los registros de los clientes del gym.
    IdCliente int AUTO_INCREMENT PRIMARY KEY,
    Nombre varchar(30) not null not null,
    ApellidoPaterno varchar(30) not null,
    ApellidoMaterno varchar(30) not null,
    FechaNacimiento datetime not null,
    Telefono varchar(10) unique not null, -- Debe de ser único para poder mandar WhatsApp
    CondicionEspecial bool not null,
    DescripcionCondicionEspecial varchar(150),
    NombreContacto varchar(30),
    TelefonoContacto varchar(10),
    Foto blob,
    FechaUltimoAcceso datetime,
    MontoUltimoPago decimal,
    Activo bool,
    FechaUltimoPago datetime,
    FechaVencimientoPago datetime, -- Un mes más 5 días de tolerancia
    DeudaCliente decimal,
    MedioConocio varchar(300), -- Redes sociales, amig@, otros
    ClasesTotalesDisponibles int, -- Acumulables, por mes
    ClasesSemanaDisponibles int, -- Clases disponibles por semana en base a paquete
    DuracionPaquete int, -- Cada que pague se restará (Solo aplica para paquetes con promo)
    Nino bool, -- Por si se da de alta un niño
    IdTipoCliente int,
    foreign key (IdTipoCliente) references TipoCliente (IdTipoCliente),
    IdPaquete int,
    foreign key (IdPaquete) references Paquete (IdPaquete),
	-- Solución ante el problema de crear clientes y al mismo tiempo asignar locker
    IdLocker int,
    foreign key (IdLocker) references Locker (IdLocker)
);


create table ClienteRenta (
    -- Tabla que guarda los registros de los clientes de renta de espacios en el gym.
    IdClienteRenta int AUTO_INCREMENT PRIMARY KEY,
    Nombre varchar(30) not null not null,
    ApellidoPaterno varchar(30) not null,
    ApellidoMaterno varchar(30) not null,
    Domicilio varchar(150) not null,
    FechaNacimiento datetime not null,
    Telefono varchar(10) unique not null, -- Debe de ser único para poder mandar WhatsApp
    NombreContacto varchar(30),
    TelefonoContacto varchar(10),
    Foto blob,
    FechaUltimoPago datetime,
    MontoUltimoPago decimal,
    DeudaCliente decimal
);

create table Rentas (
    -- Tabla que guarda los registros de las rentas de espacios.
    IdRenta int AUTO_INCREMENT PRIMARY KEY,
    FechaRenta datetime not null,
	-- IdClienteRenta
    IdClienteRenta int,
    foreign key (IdClienteRenta) references ClienteRenta (IdClienteRenta),
	-- IdEspacio
    IdEspacio int,
    foreign key (IdEspacio) references Espacio (IdEspacio),
	-- IdHorario
    Dia int not null,
    HoraInicio varchar(4) not null,
    HoraFin varchar(4) not null,
    Costo decimal
);

create table Usuario (
    -- Tabla que guarda los registros de los usuarios del sistema.
    IdUsuario int AUTO_INCREMENT PRIMARY KEY,
    Nombre varchar(30) not null,
    ApellidoPaterno varchar(30) not null,
    ApellidoMaterno varchar(30) not null,
    Domicilio varchar(150) not null,
    Username varchar(20) not null,
    Password varchar(15) not null,
    FechaNacimiento datetime not null,
    Telefono varchar(10) unique not null, -- Debe de ser único para poder mandar WhatsApp
    NombreContacto varchar(30),
    TelefonoContacto varchar(10),
    Foto blob,
    FechaUltimoAcceso datetime,
    FechaUltimoPago datetime,
    MontoUltimoPago decimal
);

create table Personal (
    IdPersonal int AUTO_INCREMENT PRIMARY KEY,
    Nombre varchar(30) not null,
    ApellidoPaterno varchar(30) not null,
    ApellidoMaterno varchar(30) not null,
    Domicilio varchar(150) not null,
    Puesto varchar(50),
    FechaNacimiento datetime not null,
    Telefono varchar(10) unique not null,
    NombreContacto varchar(30),
    TelefonoContacto varchar(30),
    Foto blob,
    FechaUltimoPago datetime,
    MontoUltimoPago decimal
);

create table Inventario (
    IdProducto int AUTO_INCREMENT PRIMARY KEY,
    NombreProducto varchar(30) not null,
    Descripcion varchar(150) not null,
    Costo decimal,
    Existencias int
);

create table Ventas (
    -- Tabla que guarda los registros de las ventas (Visitas al gym o clase) y productos
    IdVenta int auto_increment primary key,
    FechaVenta datetime not null,
    IdProducto int,
    foreign key (IdProducto) references Inventario (IdProducto),
    Concepto varchar(2000) not null,
    Costo decimal not null
);

create table Egresos (
    -- Tabla que guarda los Pagos generales
    IdPagosGeneral int auto_increment primary key,
    FechaRegistro datetime not null,
	-- IdUsuario
    IdUsuario int not null, -- Quien está haciendo el pago
    foreign key (IdUsuario) references Usuario (IdUsuario),
    Servicios bool, -- Si el pago será de servicios
    Nomina bool, -- Si el pago será a nómina (Usuarios, Instructores)
    Otros bool, -- Si el pago será a otros (no específicos)
	-- IdUsuario
    IdUsuarioPagar int, -- Si fue nómina, aquí ingresar el id del usuario a quién se le va a pagar
    foreign key (IdUsuarioPagar) references Usuario (IdUsuario),
	-- IdInstructor
    IdInstructor int, -- Si fue nomina, aquí ingresar el id del instructor a quién se le va a pagar
    foreign key (IdInstructor) references Instructor (IdInstructor),
	-- IdPersonal
    IdPersonal int, -- Si fue nomina, aquí ingresar el id del instructor a quién se le va a pagar
    foreign key (IdPersonal) references Personal (IdPersonal),
    Concepto varchar (2000) not null,
    NumeroRecibo varchar(30) not null,
    Monto decimal not null
);

create table Ingresos (
    -- Tabla que guarda los ingresos (los pagos de los Clientes, Clientes Renta o ventas)
    IdIngresos int auto_increment primary key,
    FechaRegistro datetime not null,
	-- IdUsuario
    IdUsuario int, -- Quien está recibiendo el dinero
    foreign key (IdUsuario) references Usuario (IdUsuario),
	-- IdRenta
    IdRenta int, -- Si el ingreso va a ser por una renta
    foreign key (IdRenta) references Rentas (IdRenta),
	-- IdCliente
    IdCliente int, -- Si el ingreso va a ser por un Cliente
    foreign key (IdCliente) references Cliente (IdCliente),
	-- IdVenta
    IdVenta int, -- Si el ingreso va a ser por una venta
    foreign key (IdVenta) references Ventas (IdVenta),
    Otros bool, -- Si el ingreso proviene de otro
    Concepto varchar(300) not null,
    IdPaquete int, -- Para saber cuanto va a pagar el cliente
    foreign key (IdPaquete) references Paquete (IdPaquete),
    IdLocker int, -- Para saber si le cobramos el adicional a locker (Sigue siendo opcional)
    foreign key (IdLocker) references Locker (IdLocker),
    NumeroRecibo varchar(30) not null,
    Monto decimal,
    Recibido decimal
);

-- Información de inicio:

-- Creamos el usuario admin
insert into usuario
    (IdUsuario, Nombre, ApellidoPaterno, ApellidoMaterno, Domicilio, Username, Password, FechaNacimiento, Telefono, FechaUltimoAcceso)
values
    (1, 'admin', 'admin', 'admin', 'calle', 'admin', 'admin', sysdate(), '0', sysdate());

    -- Tipos de cliente
INSERT INTO tipocliente
VALUES (default, 'Membresía', 'Clientes con solo Gym');
INSERT INTO tipocliente
VALUES (default, 'Membresía con promoción', 'Clientes con descuento');
INSERT INTO tipocliente
VALUES (default, 'De clase', 'Clientes con solo Clases');

-- Tipos de instructores
INSERT INTO tipoinstructor
VALUES (default, 'De clase', 'Instructores solo de clase');
INSERT INTO tipoinstructor
VALUES (default, 'Instructor', 'Instructor de gym');
INSERT INTO tipoinstructor
VALUES (default, 'Instructor personal', 'Instructor para un cliente');
INSERT INTO tipoinstructor
VALUES (default, 'Otros', 'Otro tipo de instructor');
