drop database if exists GymCastillo;

create database GymCastillo;

use GymCastillo;


create table TipoCliente (
    -- Tabla que guarda los registros de los tipos de Clientes, (de clase, con membresía, y membresía de promoción).
                             IdTipoCliente int auto_increment primary key,
                             NombreTipoCliente varchar(100),
                             Descripcion varchar(2000)
);

create table Cliente (
    -- Tabla Que guarda los registros de los clientes del gym.
                         IdCliente int AUTO_INCREMENT PRIMARY KEY,
                         Nombre varchar(30) not null not null,
                         ApellidoPaterno varchar(30) not null,
                         ApellidoMaterno varchar(30) not null,
                         Domicilio varchar(150) not null,
                         FechaNacimiento datetime not null,
                         Telefono varchar(10) unique not null, -- Debe de ser único para poder mandar WhatsApp
                         CondicionEspecial bool not null,
                         NombreContacto varchar(30),
                         TelefonoContacto varchar(10),
                         Foto blob,
                         FechaUltimoAcceso datetime,
                         MontoUltimoPago decimal,
                         Activo bool,
                         Asistencias varchar(200), -- TODO: ver como manejar asistencias.
                         FechaVencimientoPago datetime,
    -- IdClase int,
                         IdTipoCliente int,
                         DeudaCliente decimal,
                         MedioConocio varchar(300),
                         Locker varchar(10),
                         foreign key (IdTipoCliente) references TipoCliente (IdTipoCliente)
);

create table Clase (
    -- Tabla que guarda los registros de las clases.
                       IdClase int auto_increment primary key,
    -- IdInstructor int, -- TODO: Cada clase la pueden dar 1 o más instructores. FK
                       NombreClase varchar(30),
                       Descripcion varchar(500),
                       CostoHora decimal,
                       Horario varchar(2000),
                       Activo bool
);

create table ClienteClase (
    -- Tabla de intersección entre Clientes y Clases.
                              IdCliente int,
                              IdClase int,
                              PRIMARY KEY (IdClase, IdCliente),
                              foreign key (IdCliente) references Cliente (IdCliente),
                              foreign key (IdClase) references Clase (IdClase)
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
                            CondicionEspecial bool not null,
                            NombreContacto varchar(30),
                            TelefonoContacto varchar(10),
                            Foto blob,
                            FechaUltimoAcceso datetime,
                            FechaUltimoPago datetime,
                            MontoUltimoPago decimal,
                            Asistencias varchar(200), -- TODO: ver como manejar asistencias.
    -- IdClase int,
                            PagoHora decimal
);

create table InstructorClase (
    -- Tabla de intersección entre Instructor y Clase.
                                 IdInstructor int,
                                 IdClase int,
                                 PRIMARY KEY (IdClase, IdInstructor),
                                 foreign key (IdInstructor) references Instructor (IdInstructor),
                                 foreign key (IdClase) references Clase (IdClase)
);

create table Usuario (
    -- Tabla que guarda los registros de los usuarios del sistema.
                         IdUsuario int AUTO_INCREMENT PRIMARY KEY,
                         Nombre varchar(30) not null not null,
                         ApellidoPaterno varchar(30) not null,
                         ApellidoMaterno varchar(30) not null,
                         Domicilio varchar(150) not null,
                         Username varchar(20) not null,
                         Password varchar(15) not null,
                         FechaNacimiento datetime not null,
                         Telefono varchar(10) unique not null, -- Debe de ser único para poder mandar WhatsApp
                         CondicionEspecial bool not null,
                         NombreContacto varchar(30),
                         TelefonoContacto varchar(10),
                         Foto blob,
                         FechaUltimoAcceso datetime,
                         FechaUltimoPago datetime,
                         MontoUltimoPago decimal
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
                              CondicionEspecial bool not null,
                              NombreContacto varchar(30),
                              TelefonoContacto varchar(10),
                              Foto blob,
                              FechaUltimoPago datetime,
                              MontoUltimoPago decimal,
                              DeudaCliente decimal
);

create table Renta (
    -- Tabla que guarda los registros de las rentas de espacios.
                       IdRenta int AUTO_INCREMENT PRIMARY KEY,
                       IdClienteRenta int,
                       FechaRenta datetime not null,
                       Horario varchar(100) not null, -- (Espacio;HoraInicio:HoraFin)
                       FOREIGN KEY (IdClienteRenta) references ClienteRenta (IdClienteRenta)
);

-- Pagos

create table Pagos (
    -- Tabla que guarda los Pagos generates (luz, agua, electricidad, otros)
                       IdPagosGeneral int auto_increment primary key,
                       FechaRegistro datetime not null,
                       IdUsuario int,
                       TipoPago varchar(30) not null, -- (Servicios, Instructores, Usuarios, Otros)
                       Concepto varchar(300) not null, -- comida
                       NumeroRecibo varchar(30) not null,
                       Monto decimal,
                       foreign key (IdUsuario) references Usuario (IdUsuario)
);

create table Ingresos (
    -- Tabla que guarda los ingresos (los pagos de los Clientes)
                          IdIngresos int auto_increment primary key,
                          FechaRegistro datetime not null,
                          IdUsuario int,
                          TipoIngreso varchar(30) not null, -- (Cliente, ClienteRenta, Otros)
                          Concepto varchar(300) not null,
                          NumeroRecibo varchar(30) not null,
                          Monto decimal,
                          foreign key (IdUsuario) references Usuario (IdUsuario)
);

-- Creamos el usuario admin
insert into usuario
    (IdUsuario, Nombre, ApellidoPaterno, ApellidoMaterno, Domicilio, Username, Password, FechaNacimiento, Telefono, CondicionEspecial, FechaUltimoAcceso)
values
    (1, 'admin', 'admin', 'admin', 'calle', 'admin', 'admin', sysdate(), '0', false, sysdate());
