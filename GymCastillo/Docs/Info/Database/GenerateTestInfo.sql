
use gymcastillo;

-- Paquetes
INSERT INTO paquete
VALUES (default, TRUE, 'Gym', 'Descripción del paquete 1', null, null, 350.0);
INSERT INTO paquete
VALUES (default, FALSE, 'Paquete 1 sin gym', 'Descripción del paquete 2',  4, 1, 350);
INSERT INTO paquete
VALUES (default, FALSE, 'Paquete 2 sin gym', 'Descripción del paquete 3',  8, 2, 700);
INSERT INTO paquete
VALUES (default, FALSE, 'Paquete 3 sin gym', 'Descripción del paquete 4', 12, 3, 1050);
INSERT INTO paquete
VALUES (default, TRUE, 'Paquete 4 con gym', 'Descripción del paquete 5', 4, 1, 700);
INSERT INTO paquete
VALUES (default, TRUE, 'Paquete 5 con gym', 'Descripción del paquete 6', 8, 2, 1050);
INSERT INTO paquete
VALUES (default, TRUE, 'Paquete 6 con gym', 'Descripción del paquete 7', 12, 3, 1400);

-- Tipos de cliente
INSERT INTO tipocliente
VALUES (default, 'Membresía', 'Clientes con solo Gym');
INSERT INTO tipocliente
VALUES (default, 'Membresía con promoción', 'Clientes con descuento');
INSERT INTO tipocliente
VALUES (default, 'De clase', 'Clientes con solo Clases');

-- Espacios
INSERT INTO espacio
VALUES (default, 'Alberca', 'Para las clases de natación');
INSERT INTO espacio
VALUES (default, 'Cuadrilátero', 'Para las clases de box');
INSERT INTO espacio
VALUES (default, 'Vacío 1', 'Libre para nueva clase');
INSERT INTO espacio
VALUES (default, 'Vacío 2', 'Libre para nueva clase');
INSERT INTO espacio
VALUES (default, 'Vacío 3', 'Libre para nueva clase');

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
VALUES (default, 'Juan Pablo', 'Alegría', 'Escobedo', 'Calle Francisco Villa #49B Colonia España', CURDATE()
, '4420424629', 'Cassandra Carrillo Ledezma' ,'4428320490', null, CURDATE(), CURDATE(), 1500.0, '0700', '1400', 5
, null, 1500.0, null, 1, 1);
INSERT INTO instructor 
VALUES (default, 'Miguel', 'Gonzalez', 'Gutierrez', 'Calle Las Palmas #5 Colonia Inglaterra', CURDATE()
, '4424607855', 'Jorge David Cabrera Lopez' ,'4423706021', null, CURDATE(), CURDATE(), 500.0, '0700', '1400', 5
, null, 500.0, null, 1, 2);
INSERT INTO instructor 
VALUES (default, 'Jose Luis', 'Ruiz', 'Espino', 'Calle Centauro #23 Colonia La Condesa', CURDATE()
, '4425724914', 'Brian Gonzalez Gonzalez' ,'4427241624', null, CURDATE(), CURDATE(), 1500.0, '0700', '1400', 5
, null, 1500.0, null, 1, 1);
INSERT INTO instructor 
VALUES (default, 'Javier', 'Ibarra', 'Moroña', 'Calle Domigo #123B Colonia Iglesia', CURDATE()
, '4424602987', 'Hector Rodriguez Rodriguez' ,'4425235314', null, CURDATE(), CURDATE(), 1500.0, '0700', '1400', 5
, null, 1500.0, null, 2, 1);
INSERT INTO instructor 
VALUES (default, 'Manuel', 'Mejía', 'Rodriguez', 'Calle Miguel Barbosa #3 Colonia Lindavista', CURDATE()
, '4422645931', 'Juventino Perez Hernandez' ,'4426501910', null, CURDATE(), CURDATE(), 500.0, '0700', '1400', 5
, null, 500.0, null, 2, 2);
INSERT INTO instructor 
VALUES (default, 'Juventino', 'Perez', 'Hernandez', 'Calle Andres Navy #12 Colonia Los Angeles', CURDATE()
, '4424546787', 'Manuel Mejía Rodriguez' ,'4420521042', null, CURDATE(), CURDATE(), 500.0, '0700', '1400', 5
, null, 500.0, null, 2, 2);
INSERT INTO instructor 
VALUES (default, 'Hector', 'Rodriguez', 'Rodriguez', 'Calle Mirador #98 Colonia Sao Paulo', CURDATE()
, '4421545217', 'Javier Ibarra Moroña' ,'4429200416', null, CURDATE(), CURDATE(), 500.0, '0700', '1400', 5
, null, 500.0, null, 3, 2);
INSERT INTO instructor 
VALUES (default, 'Brian', 'Gonzalez', 'Gonzalez', 'Calle Poniente #1 Colonia Niños heroes', CURDATE()
, '4426871468', 'Jose Luis Ruiz Espino' ,'4424679408', null, CURDATE(), CURDATE(), 500.0, '0700', '1400', 5
, null, 500.0, null, 3, 2);
INSERT INTO instructor 
VALUES (default, 'Jorge David', 'Cabrera', 'Lopez', 'Calle San Javier #35 Colonia Lagunas', CURDATE()
, '4429993970', 'Miguel Gonzalez Gutierrez' ,'4420326526', null, CURDATE(), CURDATE(),1500.0, '0700', '1400', 5
, null, 1500.0, null, 3, 1);
INSERT INTO instructor 
VALUES (default, 'Cassandra', 'Carrillo', 'Ledezma', 'Calle Mi Corazon #25 Colonia Encanto', CURDATE()
, '4420608128', 'Juan Pablo Alegría Escobedo' ,'4426273881', null, CURDATE(), CURDATE(),1500.0, '0700', '1400', 5
, null, 1500.0, null, 3, 1);


-- Clases
INSERT INTO clase
VALUES (default, 'Natación', 'Se enseña a nadar', 24, TRUE, 1, 1);
INSERT INTO clase
VALUES (default, 'Box', 'Se enseña a pegar', 12, TRUE, 1, 2);

-- Horarios
INSERT INTO horario
VALUES (default, 1, '0700', '0800', null, 1);
INSERT INTO horario
VALUES (default, 2, '0700', '0800', null, 1);
INSERT INTO horario
VALUES (default, 3, '0700', '0800', null, 1);
INSERT INTO horario
VALUES (default, 4, '0700', '0800', null, 1);
INSERT INTO horario
VALUES (default, 5, '0700', '0800', null, 1);

-- Lockers
INSERT INTO locker
VALUES (default, 'A1', true);
INSERT INTO locker
VALUES (default, 'A2', true);
INSERT INTO locker
VALUES (default, 'A3', FALSE);
INSERT INTO locker
VALUES (default, 'A4', FALSE);
INSERT INTO locker
VALUES (default, 'A5', FALSE);
INSERT INTO locker
VALUES (default, 'B1', true);
INSERT INTO locker
VALUES (default, 'B2', FALSE);
INSERT INTO locker
VALUES (default, 'B3', FALSE);
INSERT INTO locker
VALUES (default, 'B4', FALSE);
INSERT INTO locker
VALUES (default, 'B5', FALSE);


-- Clientes
INSERT INTO cliente
VALUES (default, 'Daniel', 'Gonzalez', 'Martinez', CURDATE()
       , '4426861255', FALSE, null, 'Miriam Robledo Gonzalez' ,'4420142743', null, CURDATE(), 350.0, TRUE, CURDATE(), CURDATE()
       , null, 'Redes sociales', null, null, 0, FALSE, 1, 1, null);
INSERT INTO cliente
VALUES (default, 'Enrique', 'Padilla', 'Martinez', CURDATE()
       , '4426483144', FALSE, null, 'Lucero Guevara Hernandez' ,'4428052331', null, CURDATE(), 350.0, TRUE, CURDATE(), CURDATE()
       , null, 'Otro', null, null, 0, FALSE, 1, 1, null);
INSERT INTO cliente
VALUES (default, 'Julieta', 'Izquierdo', 'Perez', CURDATE()
       , '4423209770', FALSE, null, 'Adriana Guevara García' ,'4421621366', null, CURDATE(), 350.0, TRUE, CURDATE(), CURDATE()
       , null, 'Amig@', null, null, 0, FALSE, 1, 1, 1);
INSERT INTO cliente
VALUES (default, 'Daniela', 'Paredes', 'Castro', CURDATE()
       , '4421256969', FALSE, null, 'Andres Lima Rangel' ,'4428248718', null, CURDATE(), 350.0, TRUE, CURDATE(), CURDATE()
       , null, 'Amig@', 4, 1, 3, FALSE, 3, 2, 2);
INSERT INTO cliente
VALUES (default, 'Paola', 'Salinas', 'Hernandez', CURDATE()
       , '4428510144', FALSE, null, 'Ruben García Ordaz' ,'4425261623', null, CURDATE(), 700.0, TRUE, CURDATE(), CURDATE()
       , null, 'Redes sociales', 8, 2, 0, FALSE, 3, 3, 6);
INSERT INTO cliente
VALUES (default, 'Miriam', 'Robledo', 'Gonzalez', CURDATE()
       , '4428169031', FALSE, null, 'Daniel Gonzalez Martinez' ,'4422934869', null, CURDATE(), 1050.0, TRUE, CURDATE(), CURDATE()
       , null, 'Otro', 12, 3, 2, FALSE, 3, 4, null);
INSERT INTO cliente
VALUES (default, 'Lucero', 'Guevara', 'Hernandez', CURDATE()
       , '4420812321', FALSE, null, 'Enrique Padilla Martinez' ,'4428077153', null, CURDATE(), 700.0, TRUE, CURDATE(), CURDATE()
       , null, 'Otro', 4, 1, 0, FALSE, 3, 5, null);
INSERT INTO cliente
VALUES (default, 'Adriana', 'Guevara', 'García', CURDATE()
       , '4427136487', TRUE, 'Mala pata de momento', 'Julieta Izquierdo Perez' ,'4428733623', null, CURDATE(), 1050.0, TRUE, CURDATE(), CURDATE()
       , null, 'Redes sociales', 8, 2, 1, FALSE, 3, 6, null);
INSERT INTO cliente
VALUES (default, 'Andres', 'Lima', 'Rangel', CURDATE()
       , '4425191796', FALSE, null, 'Daniela Paredes Castro' ,'4426784592', null, CURDATE(), 1400.0, TRUE, CURDATE(), CURDATE()
       , null, 'Amig@', 12, 3, 0, FALSE, 3, 7, null);
INSERT INTO cliente
VALUES (default, 'Ruben', 'García', 'Ordaz', CURDATE()
       , '4423623553', FALSE, null, 'Paola Salinas Hernandez' ,'4425122504', null, CURDATE(), 1050.0, TRUE, CURDATE(),  CURDATE()
       , null, 'Redes sociales', 12, 3, 2, FALSE, 3, 4, null);

INSERT INTO clienterenta
VALUES (default, 'Jessica', 'Cortes', 'Vazquez', 'Calle Cedros #24 Colonia Arboledas', CURDATE(), '5511258591', 'Antonio Sanchez Salas', '5512928544', null, CURDATE(), 100.0, 200.0);
INSERT INTO clienterenta
VALUES (default, 'Antonio', 'Sanchez', 'Salas', 'Calle Fernando Loyola #122 Colonia Presidentes', CURDATE(), '5511591258', 'Elihihu', '5511917682', null, CURDATE(), 100.0, 200.0);
INSERT INTO clienterenta
VALUES (default, 'Lionel Andres', 'Messi', 'Dominguez', 'Calle Julian Zuniga #7 Colonia Presidentes', CURDATE(), '5517594921', 'Jessica Cortes Vazquez', '5514685493', null, CURDATE(), 100.0, 200.0);
INSERT INTO clienterenta
VALUES (default, 'Raul', 'Hernandez', 'Jimenez', 'Calle Del Meson Cedros #76 Colonia Presidentes', CURDATE(), '5518542660', 'Lionel Andres Messi Dominguez', '5517707234', null, CURDATE(), 100.0, 200.0);
INSERT INTO clienterenta
VALUES (default, 'Camilo', 'Septimo', 'Morona', 'Calle Mariano Perrusquia #88 Colonia Presidentes', CURDATE(), '5518794638', 'Raul Hernandez Jimenez', '5519159992', null, CURDATE(), 100.0, 200.0);

-- rentas
INSERT INTO rentas
VALUES (DEFAULT, CURDATE(), 1, 3, 1, '0700', '0800', 400.0);
INSERT INTO rentas
VALUES (DEFAULT, CURDATE(), 1, 1, 1, '0800', '0900', 700.0);
INSERT INTO rentas
VALUES (DEFAULT, CURDATE(), 2, 1, 1, '1000', '1100', 700.0);

-- ventas
INSERT INTO ventas
VALUES (default, CURDATE(), null, 'Venta 1', 100.0);
INSERT INTO ventas
VALUES (default, CURDATE(), null, 'Venta 2', 200.0);
INSERT INTO ventas
VALUES (default, CURDATE(), null, 'Venta 3', 300.0);
INSERT INTO ventas
VALUES (default, CURDATE(), null, 'Venta 4', 250.0);
INSERT INTO ventas
VALUES (default, CURDATE(), null, 'Venta 5', 150.0);

-- pagos
INSERT INTO egresos
VALUES (default, CURDATE(), 1, FALSE, FALSE, TRUE, NULL, NULL, 'Comida pal staff', 'ABCDEFGHIJ1234567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, TRUE, FALSE, FALSE, NULL, NULL, 'Pago agua', 'ABCDEFGHIJ2234567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, FALSE, TRUE, FALSE, NULL, 1, 'Pago a Instructor', 'ABCDEFGHIJ2224567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, FALSE, FALSE, TRUE, NULL, NULL, 'Jueguete pal staff', 'ABCDEFGHIJ3234567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, TRUE, FALSE, FALSE, NULL, NULL, 'Pago luz', 'ABCDEFGHIJ3324567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, FALSE, TRUE, FALSE, 1, NULL, 'Pago a Usuario', 'ABCDEFGHIJ3334567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, FALSE, FALSE, TRUE, NULL, NULL, 'Saldo pal staff', 'ABCDEFGHIJ4234567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, TRUE, FALSE, FALSE, NULL, NULL, 'Pago gas', 'ABCDEFGHIJ4424567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, FALSE, TRUE, FALSE, NULL, 2, 'Pago a Instructor', 'ABCDEFGHIJ4444567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, FALSE, FALSE, TRUE, NULL, NULL, 'Cervezas pal staff', 'ABCDEFGHIJ5234567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, TRUE, FALSE, FALSE, NULL, NULL, 'Pago uranio', 'ABCDEFGHIJ5524567890', 100.0);
INSERT INTO egresos
VALUES (default, CURDATE(), 1, FALSE, TRUE, FALSE, 1, NULL, 'Pago a Usuario', 'ABCDEFGHIJ5554567890', 100.0);

-- ingresos
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, 1, NULL, NULL, FALSE, 'Renta', 1, null, 'ABCDEFGHIJ1234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, 1, NULL, FALSE, 'Cliente', 1, null, 'ABCDEFGHIJ2234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, 1, FALSE, 'Venta', 1, null, 'ABCDEFGHIJ2224567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, NULL, TRUE, 'Reposición juguete staff', 1, null, 'ABCDEFGHIJ3234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, 2, NULL, NULL, FALSE, 'Renta', 1, 1, 'ABCDEFGHIJ3324567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, 2, NULL, FALSE, 'Cliente', 1, 2, 'ABCDEFGHIJ3334567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, 2, FALSE, 'Venta', 1, null, 'ABCDEFGHIJ4234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, NULL, TRUE, 'Reposición Maquina Gym ', 1, 6, 'ABCDEFGHIJ4424567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, 3, NULL, NULL, FALSE, 'Renta', 1, null, 'ABCDEFGHIJ4444567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, 3, NULL, FALSE, 'Cliente', 1, null, 'ABCDEFGHIJ5234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, 3, FALSE, 'Venta', 1, null, 'ABCDEFGHIJ5524567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, NULL, TRUE, 'Reposición comida staff', 1, 4,'ABCDEFGHIJ5554567890', 100.0);

-- Personal
INSERT INTO personal
VALUES (default, 'Jessica', 'Cortes', 'Vazquez', 'Calle Cedros #24 Colonia Arboledas', 'Puesto 1', CURDATE(), '5511258591', 'Antonio Sanchez Salas', '5512928544', null, CURDATE(), 100.0);
INSERT INTO personal
VALUES (default, 'Antonio', 'Sanchez', 'Salas', 'Calle Fernando Loyola #122 Colonia Presidentes', 'Puesto 2', CURDATE(), '5511591258', 'Elihihu', '5511917682', null, CURDATE(), 100.0);
INSERT INTO personal
VALUES (default, 'Lionel Andres', 'Messi', 'Dominguez', 'Calle Julian Zuniga #7 Colonia Presidentes', 'Puesto 3', CURDATE(), '5517594921', 'Jessica Cortes Vazquez', '5514685493', null, CURDATE(), 100.0);
INSERT INTO personal
VALUES (default, 'Raul', 'Hernandez', 'Jimenez', 'Calle Del Meson Cedros #76 Colonia Presidentes', 'Puesto 4', CURDATE(), '5518542660', 'Lionel Andres Messi Dominguez', '5517707234', null, CURDATE(), 100.0);
INSERT INTO personal
VALUES (default, 'Camilo', 'Septimo', 'Morona', 'Calle Mariano Perrusquia #88 Colonia Presidentes', 'Puesto 5', CURDATE(), '5518794638', 'Raul Hernandez Jimenez', '5519159992', null, CURDATE(), 100.0);

-- PaquetesClases
INSERT INTO paquetesclases
VALUES (1,1);
INSERT INTO paquetesclases
VALUES (1,2);
INSERT INTO paquetesclases
VALUES (2,1);
INSERT INTO paquetesclases
VALUES (2,2);
