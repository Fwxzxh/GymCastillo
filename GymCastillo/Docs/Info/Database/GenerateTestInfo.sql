
-- Tipos de cliente
INSERT INTO tipocliente
VALUES (default, 'Membresia', 'Clientes con solo Gym');
INSERT INTO tipocliente
VALUES (default, 'Membresia con promocion', 'Clientes con descuento');
INSERT INTO tipocliente
VALUES (default, 'De clase', 'Clientes con solo Clases');

-- Espacios
INSERT INTO espacio
VALUES (default, 'Alberca', 'Para las clases de natación');
INSERT INTO espacio
VALUES (default, 'Cuadrilatero', 'Para las clases de box');
INSERT INTO espacio
VALUES (default, 'Vacio 1', 'Libre para nueva clase');
INSERT INTO espacio
VALUES (default, 'Vacio 2', 'Libre para nueva clase');
INSERT INTO espacio
VALUES (default, 'Vacio 3', 'Libre para nueva clase');

-- Tipos de instructores
INSERT INTO tipoinstructor
VALUES (default, 'De clase', 'Instructores solo de clase');
INSERT INTO tipoinstructor
VALUES (default, 'Instructor', 'Instructor de gym');
INSERT INTO tipoinstructor
VALUES (default, 'Instructor personal', 'Instructor para un cliente');
INSERT INTO tipoinstructor
VALUES (default, 'Otros', 'Otro tipo de instructor');

-- Instructores
INSERT INTO instructor 
VALUES (default, 'Instructor1', 'ApellidoPI1', 'ApellidoMI1', 'Calle 1 Colonia B', CURDATE()
, '2111111111', 'AmigoI1' ,'1222222221', null, CURDATE(), CURDATE(), 1500.0, '700', '1400', 5
, null, 1500.0, null, 1);
INSERT INTO instructor 
VALUES (default, 'Instructor2', 'ApellidoPI2', 'ApellidoMI2', 'Calle 2 Colonia B', CURDATE()
, '2111111112', 'AmigoI2' ,'1222222222', null, CURDATE(), CURDATE(), 500.0, '700', '1400', 5
, null, 500.0, null, 2);
INSERT INTO instructor 
VALUES (default, 'Instructor3', 'ApellidoPI3', 'ApellidoMI3', 'Calle 3 Colonia B', CURDATE()
, '2111111113', 'AmigoI3' ,'1222222223', null, CURDATE(), CURDATE(), 1500.0, '700', '1400', 5
, null, 1500.0, null, 1);
INSERT INTO instructor 
VALUES (default, 'Instructor4', 'ApellidoPI4', 'ApellidoMI4', 'Calle 4 Colonia B', CURDATE()
, '2111111114', 'AmigoI4' ,'1222222224', null, CURDATE(), CURDATE(), 1500.0, '700', '1400', 5
, null, 1500.0, null, 1);
INSERT INTO instructor 
VALUES (default, 'Instructor5', 'ApellidoPI5', 'ApellidoMI5', 'Calle 5 Colonia B', CURDATE()
, '2111111115', 'AmigoI5' ,'1222222225', null, CURDATE(), CURDATE(), 500.0, '700', '1400', 5
, null, 500.0, null, 2);
INSERT INTO instructor 
VALUES (default, 'Instructor6', 'ApellidoPI6', 'ApellidoMI6', 'Calle 6 Colonia B', CURDATE()
, '2111111116', 'AmigoI6' ,'1222222226', null, CURDATE(), CURDATE(), 500.0, '700', '1400', 5
, null, 500.0, null, 2);
INSERT INTO instructor 
VALUES (default, 'Instructor7', 'ApellidoPI7', 'ApellidoMI7', 'Calle 7 Colonia B', CURDATE()
, '2111111117', 'AmigoI7' ,'1222222227', null, CURDATE(), CURDATE(), 500.0, '700', '1400', 5
, null, 500.0, null, 2);
INSERT INTO instructor 
VALUES (default, 'Instructor8', 'ApellidoPI8', 'ApellidoMI8', 'Calle 8 Colonia B', CURDATE()
, '2111111118', 'AmigoI8' ,'1222222238', null, CURDATE(), CURDATE(), 500.0, '700', '1400', 5
, null, 500.0, null, 2);
INSERT INTO instructor 
VALUES (default, 'Instructor9', 'ApellidoPI9', 'ApellidoMI9', 'Calle 9 Colonia B', CURDATE()
, '2111111119', 'AmigoI9' ,'1222222229', null, CURDATE(), CURDATE(),1500.0, '700', '1400', 5
, null, 1500.0, null, 1);
INSERT INTO instructor 
VALUES (default, 'Instructor10', 'ApellidoPI10', 'ApellidoMI10', 'Calle 10 Colonia B', CURDATE()
, '2111111110', 'AmigoI10' ,'1222222210', null, CURDATE(), CURDATE(),1500.0, '700', '1400', 5
, null, 1500.0, null, 1);

-- Clases
INSERT INTO clase
VALUES (default, 'Natación', 'Se enseña a nadar', 24, TRUE, 1, 1);
INSERT INTO clase
VALUES (default, 'Box', 'Se enseña a pegar', 12, TRUE, 1, 2);

-- Horarios
INSERT INTO horario
VALUES (default, 'Lunes', '700', '800', null, 1);
INSERT INTO horario
VALUES (default, 'Martes', '700', '800', null, 1);
INSERT INTO horario
VALUES (default, 'Miercoles', '700', '800', null, 1);
INSERT INTO horario
VALUES (default, 'Jueves', '700', '800', null, 1);
INSERT INTO horario
VALUES (default, 'Viernes', '700', '800', null, 1);

-- Paquetes
INSERT INTO paquete
VALUES (default, TRUE, 'Gym', null, null, 350.0, null);
INSERT INTO paquete
VALUES (default, FALSE, 'Paquete 1 sin gym', 4, 1, 350, 1);
INSERT INTO paquete
VALUES (default, FALSE, 'Paquete 2 sin gym', 8, 2, 700, 1);
INSERT INTO paquete
VALUES (default, FALSE, 'Paquete 3 sin gym', 12, 3, 1050, 1);
INSERT INTO paquete
VALUES (default, TRUE, 'Paquete 4 con gym', 4, 1, 700, 1);
INSERT INTO paquete
VALUES (default, TRUE, 'Paquete 5 con gym', 8, 2, 1050, 1);
INSERT INTO paquete
VALUES (default, TRUE, 'Paquete 6 con gym', 12, 3, 1400, 1);

-- Clientes
INSERT INTO cliente 
VALUES (default, 'Persona1', 'ApellidoP1', 'ApellidoM1', 'Calle 1 Colonia A', CURDATE()
, '1111111111', FALSE, 'Amigo1' ,'2222222221', null, CURDATE(), 350.0, TRUE, CURDATE()
, null, 'Redes sociales', null, null, null, FALSE, 1, 1);
INSERT INTO cliente 
VALUES (default, 'Persona2', 'ApellidoP2', 'ApellidoM2', 'Calle 2 Colonia A', CURDATE()
, '1111111112', FALSE, 'Amigo2' ,'2222222222', null, CURDATE(), 350.0, TRUE, CURDATE()
, null, 'Otro', null, null, null, FALSE, 1, 1);
INSERT INTO cliente 
VALUES (default, 'Persona3', 'ApellidoP3', 'ApellidoM3', 'Calle 3 Colonia A', CURDATE()
, '1111111113', FALSE, 'Amigo3' ,'2222222223', null, CURDATE(), 350.0, TRUE, CURDATE()
, null, 'Amig@', null, null, null, FALSE, 1, 1);
INSERT INTO cliente 
VALUES (default, 'Persona4', 'ApellidoP4', 'ApellidoM4', 'Calle 4 Colonia A', CURDATE()
, '1111111114', FALSE, 'Amigo4' ,'2222222224', null, CURDATE(), 350.0, TRUE, CURDATE()
, null, 'Amig@', 4, 1, null, FALSE, 3, 2);
INSERT INTO cliente 
VALUES (default, 'Persona5', 'ApellidoP5', 'ApellidoM5', 'Calle 5 Colonia A', CURDATE()
, '1111111115', FALSE, 'Amigo5' ,'2222222225', null, CURDATE(), 700.0, TRUE, CURDATE()
, null, 'Redes sociales', 8, 2, null, FALSE, 3, 3);
INSERT INTO cliente 
VALUES (default, 'Persona6', 'ApellidoP6', 'ApellidoM6', 'Calle 6 Colonia A', CURDATE()
, '1111111116', FALSE, 'Amigo6' ,'2222222226', null, CURDATE(), 1050.0, TRUE, CURDATE()
, null, 'Otro', 12, 3, null, FALSE, 3, 4);
INSERT INTO cliente 
VALUES (default, 'Persona7', 'ApellidoP7', 'ApellidoM7', 'Calle 7 Colonia A', CURDATE()
, '1111111117', FALSE, 'Amigo7' ,'2222222227', null, CURDATE(), 700.0, TRUE, CURDATE()
, null, 'Otro', 4, 1, null, FALSE, 3, 5);
INSERT INTO cliente 
VALUES (default, 'Persona8', 'ApellidoP8', 'ApellidoM8', 'Calle 8 Colonia A', CURDATE()
, '1111111118', TRUE, 'Amigo8' ,'2222222238', null, CURDATE(), 1050.0, TRUE, CURDATE()
, null, 'Redes sociales', 8, 2, null, FALSE, 3, 6);
INSERT INTO cliente 
VALUES (default, 'Persona9', 'ApellidoP9', 'ApellidoM9', 'Calle 9 Colonia A', CURDATE()
, '1111111119', FALSE, 'Amigo9' ,'2222222229', null, CURDATE(), 1400.0, TRUE, CURDATE()
, null, 'Amig@', 12, 3, null, FALSE, 3, 7);
INSERT INTO cliente 
VALUES (default, 'Persona10', 'ApellidoP10', 'ApellidoM10', 'Calle 11 Colonia A', CURDATE()
, '1111111110', FALSE, 'Amigo10' ,'2222222210', null, CURDATE(), 1050.0, TRUE,  CURDATE()
, null, 'Redes sociales', 12, 3, null, FALSE, 3, 4);

-- Lockers
INSERT INTO locker
VALUES (default, 'A1', null);
INSERT INTO locker
VALUES (default, 'A2', null);
INSERT INTO locker
VALUES (default, 'A3', null);
INSERT INTO locker
VALUES (default, 'A4', null);
INSERT INTO locker
VALUES (default, 'A5', null);
INSERT INTO locker
VALUES (default, 'B1', null);
INSERT INTO locker
VALUES (default, 'B2', null);
INSERT INTO locker
VALUES (default, 'B3', null);
INSERT INTO locker
VALUES (default, 'B4', null);
INSERT INTO locker
VALUES (default, 'B5', null);
