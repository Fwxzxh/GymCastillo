
-- Tipo Cliente: Definidos en el documento
INSERT INTO tipocliente VALUES (default, 'De Clase', 'Solo Gym');
INSERT INTO tipocliente VALUES (default, 'Con Membresia', 'Con Clases');
INSERT INTO tipocliente VALUES (default, 'Con Membresia en promocion', 'Con Clases y descuento');

-- Clases
INSERT INTO clase VALUES (default, 'Zumba', 'No se que es zumba', 50.0, '1;700:800;1,3;700:800;1,5;700:800;1', TRUE);
INSERT INTO clase VALUES (default, 'Salsa', 'No se que es bailar', 70.0, '2;700:800;2,4;700:800;2,6;700:800;2', TRUE);
INSERT INTO clase VALUES (default, 'Kung Fu', 'No se que hace esto aqui', 65.0, '1;900:1000;1,3;900:1000;1,5;900:1000;1', TRUE);
INSERT INTO clase VALUES (default, 'Zumba 2', 'No se que es zumba 2', 45.0, '2;900:1000;2,4;900:1000;2,6;900:1000;2', TRUE);
INSERT INTO clase VALUES (default, 'Jaripeo', 'No se que si asi se escribe', 80.0, '1;1100:1200;1,3;1100:1200;1,5;1100:1200;1', TRUE);

-- Clientes
INSERT INTO cliente VALUES (default, 'Persona1', 'ApellidoP1', 'ApellidoM1', 'Calle 1 Colonia A', CURDATE(), '1111111111', FALSE, 'Amigo1' ,'2222222221', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), 1, null, 'Amigo', 'A1');
INSERT INTO cliente VALUES (default, 'Persona2', 'ApellidoP2', 'ApellidoM2', 'Calle 2 Colonia A', CURDATE(), '1111111112', FALSE, 'Amigo2' ,'2222222222', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), 1, null, 'Amigo', 'A2');
INSERT INTO cliente VALUES (default, 'Persona3', 'ApellidoP3', 'ApellidoM3', 'Calle 3 Colonia A', CURDATE(), '1111111113', FALSE, 'Amigo3' ,'2222222223', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), 2, null, 'Redes sociales', 'A3');
INSERT INTO cliente VALUES (default, 'Persona4', 'ApellidoP4', 'ApellidoM4', 'Calle 4 Colonia A', CURDATE(), '1111111114', FALSE, 'Amigo4' ,'2222222224', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), 2, null, 'Redes sociales', 'A4');
INSERT INTO cliente VALUES (default, 'Persona5', 'ApellidoP5', 'ApellidoM5', 'Calle 5 Colonia A', CURDATE(), '1111111115', FALSE, 'Amigo5' ,'2222222225', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), 3, null, 'Publicidad', 'A5');
INSERT INTO cliente VALUES (default, 'Persona6', 'ApellidoP6', 'ApellidoM6', 'Calle 6 Colonia A', CURDATE(), '1111111116', FALSE, 'Amigo6' ,'2222222226', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 1, null, 'Publicidad', 'B1');
INSERT INTO cliente VALUES (default, 'Persona7', 'ApellidoP7', 'ApellidoM7', 'Calle 7 Colonia A', CURDATE(), '1111111117', FALSE, 'Amigo7' ,'2222222227', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 2, null, 'Publicidad', 'B2');
INSERT INTO cliente VALUES (default, 'Persona8', 'ApellidoP8', 'ApellidoM8', 'Calle 8 Colonia A', CURDATE(), '1111111118', TRUE, 'Amigo8' ,'2222222238', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 3, null, 'Redes sociales', 'B3');
INSERT INTO cliente VALUES (default, 'Persona9', 'ApellidoP9', 'ApellidoM9', 'Calle 9 Colonia A', CURDATE(), '1111111119', FALSE, 'Amigo9' ,'2222222229', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 2, null, 'Redes sociales', 'B4');
INSERT INTO cliente VALUES (default, 'Persona10', 'ApellidoP10', 'ApellidoM10', 'Calle 11 Colonia A', CURDATE(), '1111111110', FALSE, 'Amigo10' ,'2222222210', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 1, null, 'Amigo', 'B5');

-- ClienteClase tabla relacion (aclaracion pendiente)
INSERT INTO clienteclase VALUES (7,3);
INSERT INTO clienteclase VALUES (1,2);
INSERT INTO clienteclase VALUES (4,5);
INSERT INTO clienteclase VALUES (9,1);
INSERT INTO clienteclase VALUES (2,4);
INSERT INTO clienteclase VALUES (1,1);
INSERT INTO clienteclase VALUES (1,3);
INSERT INTO clienteclase VALUES (1,4);
INSERT INTO clienteclase VALUES (3,1);
INSERT INTO clienteclase VALUES (3,5);
INSERT INTO clienteclase VALUES (3,2);
INSERT INTO clienteclase VALUES (5,4);
INSERT INTO clienteclase VALUES (5,1);

-- Instructores
INSERT INTO instructor VALUES (default, 'Instructor1', 'ApellidoPI1', 'ApellidoMI1', 'Calle 1 Colonia B', CURDATE(), '2111111111', FALSE, 'AmigoI1' ,'1222222221', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);
INSERT INTO instructor VALUES (default, 'Instructor2', 'ApellidoPI2', 'ApellidoMI2', 'Calle 2 Colonia B', CURDATE(), '2111111112', FALSE, 'AmigoI2' ,'1222222222', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);
INSERT INTO instructor VALUES (default, 'Instructor3', 'ApellidoPI3', 'ApellidoMI3', 'Calle 3 Colonia B', CURDATE(), '2111111113', FALSE, 'AmigoI3' ,'1222222223', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);
INSERT INTO instructor VALUES (default, 'Instructor4', 'ApellidoPI4', 'ApellidoMI4', 'Calle 4 Colonia B', CURDATE(), '2111111114', FALSE, 'AmigoI4' ,'1222222224', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);
INSERT INTO instructor VALUES (default, 'Instructor5', 'ApellidoPI5', 'ApellidoMI5', 'Calle 5 Colonia B', CURDATE(), '2111111115', TRUE, 'AmigoI5' ,'1222222225', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);
INSERT INTO instructor VALUES (default, 'Instructor6', 'ApellidoPI6', 'ApellidoMI6', 'Calle 6 Colonia B', CURDATE(), '2111111116', FALSE, 'AmigoI6' ,'1222222226', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);
INSERT INTO instructor VALUES (default, 'Instructor7', 'ApellidoPI7', 'ApellidoMI7', 'Calle 7 Colonia B', CURDATE(), '2111111117', FALSE, 'AmigoI7' ,'1222222227', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);
INSERT INTO instructor VALUES (default, 'Instructor8', 'ApellidoPI8', 'ApellidoMI8', 'Calle 8 Colonia B', CURDATE(), '2111111118', FALSE, 'AmigoI8' ,'1222222238', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);
INSERT INTO instructor VALUES (default, 'Instructor9', 'ApellidoPI9', 'ApellidoMI9', 'Calle 9 Colonia B', CURDATE(), '2111111119', FALSE, 'AmigoI9' ,'1222222229', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);
INSERT INTO instructor VALUES (default, 'Instructor10', 'ApellidoPI10', 'ApellidoMI10', 'Calle 10 Colonia B', CURDATE(), '2111111110', FALSE, 'AmigoI10' ,'1222222210', null, CURDATE(), CURDATE(), 350.0, '***', 100.0);

-- InstructorClase
INSERT INTO instructorclase VALUES (1,1);
INSERT INTO instructorclase VALUES (1,2);
INSERT INTO instructorclase VALUES (4,5);
INSERT INTO instructorclase VALUES (7,3);
INSERT INTO instructorclase VALUES (9,4);

-- Usuarios
INSERT INTO usuario VALUES (default, 'Usuario1', 'ApellidoPU1', 'ApellidoMU1', 'Calle 1 Colonia C', 'Itsuki', 'root', CURDATE(), '3111111111', FALSE, 'AmigoU1' ,'3222222221', null, CURDATE(), CURDATE(), 350.0);
INSERT INTO usuario VALUES (default, 'Usuario2', 'ApellidoPU2', 'ApellidoMU2', 'Calle 2 Colonia C', 'Yotsuba', 'root', CURDATE(), '3111111112', FALSE, 'AmigoU2' ,'3222222222', null, CURDATE(), CURDATE(), 350.0);
INSERT INTO usuario VALUES (default, 'Usuario3', 'ApellidoPU3', 'ApellidoMU3', 'Calle 3 Colonia C', 'Nino', 'root', CURDATE(), '3111111113', FALSE, 'AmigoU3' ,'3222222223', null, CURDATE(), CURDATE(), 350.0);
INSERT INTO usuario VALUES (default, 'Usuario4', 'ApellidoPU4', 'ApellidoMU4', 'Calle 4 Colonia C', 'Ichika', 'root', CURDATE(), '3111111114', FALSE, 'AmigoU4' ,'3222222224', null, CURDATE(), CURDATE(), 350.0);
INSERT INTO usuario VALUES (default, 'Usuario5', 'ApellidoPU5', 'ApellidoMU5', 'Calle 5 Colonia C', 'Miku', 'root', CURDATE(), '3111111115', FALSE, 'AmigoU5' ,'3222222225', null, CURDATE(), CURDATE(), 350.0);

-- ClienteRenta
INSERT INTO clienterenta VALUES (default, 'ClienteRenta1', 'ApellidoPCR1', 'ApellidoMCR1', 'Calle 1 Colonia A', CURDATE(), '4111111111', FALSE, 'AmigoCR1' ,'4222222221', null, CURDATE(), 350.0, null);
INSERT INTO clienterenta VALUES (default, 'ClienteRenta2', 'ApellidoPCR2', 'ApellidoMCR2', 'Calle 2 Colonia B', CURDATE(), '4111111112', FALSE, 'AmigoCR2' ,'4222222222', null, CURDATE(), 350.0, 100.0);
INSERT INTO clienterenta VALUES (default, 'ClienteRenta3', 'ApellidoPCR3', 'ApellidoMCR3', 'Calle 3 Colonia B', CURDATE(), '4111111113', FALSE, 'AmigoCR3' ,'4222222223', null, CURDATE(), 350.0, null);

-- Renta
INSERT INTO renta VALUES (default, 1, CURDATE(), '1;700:900');
INSERT INTO renta VALUES (default, 2, CURDATE(), '3;900:1000');
INSERT INTO renta VALUES (default, 3, CURDATE(), '2;1100:1300');
INSERT INTO renta VALUES (default, 3, CURDATE(), '3;1500:1600');

-- Pagos
INSERT INTO pagos VALUES (default, CURDATE(), 1, 'Instructores', '3, Instructor3, Trabajo', 'SA12121212', 350.0);
INSERT INTO pagos VALUES (default, CURDATE(), 1, 'Instructores', '1, Instructor1, Trabajo', 'SA12121213', 350.0);
INSERT INTO pagos VALUES (default, CURDATE(), 1, 'Servicios', 'Pago Luz', 'SA12121214', 1500.0);
INSERT INTO pagos VALUES (default, CURDATE(), 2, 'Usuarios', '4, Usuario4, Trabajo', 'SA12121215', 400.0);
INSERT INTO pagos VALUES (default, CURDATE(), 2, 'Otros', 'Comida', 'SA12121216', 100.0);

-- Ingresos
INSERT INTO ingresos VALUES (default, CURDATE(), 1, 'Cliente', 'Mensualidad', 'BE32323231', 350.0);
INSERT INTO ingresos VALUES (default, CURDATE(), 1, 'Cliente', 'Mensualidad', 'BE32323232', 350.0);
INSERT INTO ingresos VALUES (default, CURDATE(), 2, 'Cliente', 'Mensualidad', 'BE32323233', 350.0);
INSERT INTO ingresos VALUES (default, CURDATE(), 2, 'ClienteRenta', 'Adeudo', 'BE32323234', 500.0);
INSERT INTO ingresos VALUES (default, CURDATE(), 3, 'Otros', 'Pago daño a material', 'BE32323235', 850.0);
