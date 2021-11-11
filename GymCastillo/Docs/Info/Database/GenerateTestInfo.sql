
-- Cliente sin tipo
INSERT INTO cliente VALUES (default, 'Persona1', 'ApellidoP1', 'ApellidoM1', CURDATE(), '1111111111', FALSE, 'Amigo1' ,'2222222221', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), null, null, 'Amigo', 'A1');
INSERT INTO cliente VALUES (default, 'Persona2', 'ApellidoP2', 'ApellidoM2', CURDATE(), '1111111112', FALSE, 'Amigo2' ,'2222222222', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), null, null, 'Amigo', 'A2');
INSERT INTO cliente VALUES (default, 'Persona3', 'ApellidoP3', 'ApellidoM3', CURDATE(), '1111111113', FALSE, 'Amigo3' ,'2222222223', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), null, null, 'Redes sociales', 'A3');
INSERT INTO cliente VALUES (default, 'Persona4', 'ApellidoP4', 'ApellidoM4', CURDATE(), '1111111114', FALSE, 'Amigo4' ,'2222222224', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), null, null, 'Redes sociales', 'A4');
INSERT INTO cliente VALUES (default, 'Persona5', 'ApellidoP5', 'ApellidoM5', CURDATE(), '1111111115', FALSE, 'Amigo5' ,'2222222225', null, CURDATE(), 350.0, TRUE, '*', CURDATE(), null, null, 'Publicidad', 'A5');

-- Tipo Cliente: Definidos en el documento
INSERT INTO tipocliente VALUES (default, 'De Clase', 'Solo Gym');
INSERT INTO tipocliente VALUES (default, 'Con Membresia', 'Con Clases');
INSERT INTO tipocliente VALUES (default, 'Con Membresia en promocion', 'Con Clases y descuento');

-- Clases v1
INSERT INTO clase VALUES (default, 'Zumba', 'No se que es zumba', 50.0, '1;700:800;1,3;700:800;1,5;700:800;1', TRUE);
INSERT INTO clase VALUES (default, 'Salsa', 'No se que es bailar', 70.0, '2;700:800;2,4;700:800;2,6;700:800;2', TRUE);
INSERT INTO clase VALUES (default, 'Kung Fu', 'No se que hace esto aqui', 65.0, '1;900:1000;1,3;900:1000;1,5;900:1000;1', TRUE);
INSERT INTO clase VALUES (default, 'Zumba 2', 'No se que es zumba 2', 45.0, '2;900:1000;2,4;900:1000;2,6;900:1000;2', TRUE);
INSERT INTO clase VALUES (default, 'Jaripeo', 'No se que si asi se escribe', 80.0, '1;1100:1200;1,3;1100:1200;1,5;1100:1200;1', TRUE);

-- Cliente con tipo
INSERT INTO cliente VALUES (default, 'Persona6', 'ApellidoP6', 'ApellidoM6', CURDATE(), '1111111116', FALSE, 'Amigo6' ,'2222222226', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 1, null, 'Publicidad', 'B1');
INSERT INTO cliente VALUES (default, 'Persona7', 'ApellidoP7', 'ApellidoM7', CURDATE(), '1111111117', FALSE, 'Amigo7' ,'2222222227', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 2, null, 'Publicidad', 'B2');
INSERT INTO cliente VALUES (default, 'Persona8', 'ApellidoP8', 'ApellidoM8', CURDATE(), '1111111118', TRUE, 'Amigo8' ,'2222222238', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 3, null, 'Redes sociales', 'B3');
INSERT INTO cliente VALUES (default, 'Persona9', 'ApellidoP9', 'ApellidoM9', CURDATE(), '1111111119', FALSE, 'Amigo9' ,'2222222229', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 2, null, 'Redes sociales', 'B4');
INSERT INTO cliente VALUES (default, 'Persona10', 'ApellidoP10', 'ApellidoM10', CURDATE(), '1111111110', FALSE, 'Amigo10' ,'2222222210', null, CURDATE(), 350.0, TRUE, '**', CURDATE(), 1, null, 'Amigo', 'B5');

-- ClienteClase tabla relacion (aclaracion pendiente)
INSERT INTO clienteclase VALUES (7,3);
INSERT INTO clienteclase VALUES (1,2);
INSERT INTO clienteclase VALUES (4,5);
INSERT INTO clienteclase VALUES (9,1);
INSERT INTO clienteclase VALUES (2,4);
