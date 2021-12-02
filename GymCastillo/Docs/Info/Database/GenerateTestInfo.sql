
use gymcastillo;

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
VALUES (default, 'Daniel', 'Gonzalez', 'Martinez', 'Calle Las Campanas #23 Colonia Centro', CURDATE()
       , '4426861255', FALSE, 'Miriam Robledo Gonzalez' ,'4420142743', null, CURDATE(), 350.0, TRUE, CURDATE()
       , null, 'Redes sociales', null, null, null, FALSE, 1, 1, null);
INSERT INTO cliente
VALUES (default, 'Enrique', 'Padilla', 'Martinez', 'Calle Juan Escutia #230 Colonia San Pablo', CURDATE()
       , '4426483144', FALSE, 'Lucero Guevara Hernandez' ,'4428052331', null, CURDATE(), 350.0, TRUE, CURDATE()
       , null, 'Otro', null, null, null, FALSE, 1, 1, null);
INSERT INTO cliente
VALUES (default, 'Julieta', 'Izquierdo', 'Perez', 'Calle Molino del Rey #123 Colonia Menchaca', CURDATE()
       , '4423209770', FALSE, 'Adriana Guevara García' ,'4421621366', null, CURDATE(), 350.0, TRUE, CURDATE()
       , null, 'Amig@', null, null, null, FALSE, 1, 1, 1);
INSERT INTO cliente
VALUES (default, 'Daniela', 'Paredes', 'Castro', 'Calle Francisco Marquez #43B Colonia Menchaca ll', CURDATE()
       , '4421256969', FALSE, 'Andres Lima Rangel' ,'4428248718', null, CURDATE(), 350.0, TRUE, CURDATE()
       , null, 'Amig@', 4, 1, null, FALSE, 3, 2, 2);
INSERT INTO cliente
VALUES (default, 'Paola', 'Salinas', 'Hernandez', 'Calle Salvador Uribe #1 Colonia Menchaca lll', CURDATE()
       , '4428510144', FALSE, 'Ruben García Ordaz' ,'4425261623', null, CURDATE(), 700.0, TRUE, CURDATE()
       , null, 'Redes sociales', 8, 2, null, FALSE, 3, 3, 6);
INSERT INTO cliente
VALUES (default, 'Miriam', 'Robledo', 'Gonzalez', 'Calle Laurel #12 Colonia Arboledas', CURDATE()
       , '4428169031', FALSE, 'Daniel Gonzalez Martinez' ,'4422934869', null, CURDATE(), 1050.0, TRUE, CURDATE()
       , null, 'Otro', 12, 3, null, FALSE, 3, 4, null);
INSERT INTO cliente
VALUES (default, 'Lucero', 'Guevara', 'Hernandez', 'Calle Encino #65 Colonia Arboledas', CURDATE()
       , '4420812321', FALSE, 'Enrique Padilla Martinez' ,'4428077153', null, CURDATE(), 700.0, TRUE, CURDATE()
       , null, 'Otro', 4, 1, null, FALSE, 3, 5, null);
INSERT INTO cliente
VALUES (default, 'Adriana', 'Guevara', 'García', 'Calle Fresno #78 Colonia Arboledas', CURDATE()
       , '4427136487', TRUE, 'Julieta Izquierdo Perez' ,'4428733623', null, CURDATE(), 1050.0, TRUE, CURDATE()
       , null, 'Redes sociales', 8, 2, null, FALSE, 3, 6, null);
INSERT INTO cliente
VALUES (default, 'Andres', 'Lima', 'Rangel', 'Calle Acacia #99 Colonia Arboledas', CURDATE()
       , '4425191796', FALSE, 'Daniela Paredes Castro' ,'4426784592', null, CURDATE(), 1400.0, TRUE, CURDATE()
       , null, 'Amig@', 12, 3, null, FALSE, 3, 7, null);
INSERT INTO cliente
VALUES (default, 'Ruben', 'García', 'Ordaz', 'Calle Eucalipto #54 Colonia Arboledas', CURDATE()
, '4423623553', FALSE, 'Paola Salinas Hernandez' ,'4425122504', null, CURDATE(), 1050.0, TRUE,  CURDATE()
, null, 'Redes sociales', 12, 3, null, FALSE, 3, 4, null);

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
VALUES (default, CURDATE(), 'Venta 1', 100.0);
INSERT INTO ventas
VALUES (default, CURDATE(), 'Venta 2', 200.0);
INSERT INTO ventas
VALUES (default, CURDATE(), 'Venta 3', 300.0);
INSERT INTO ventas
VALUES (default, CURDATE(), 'Venta 4', 250.0);
INSERT INTO ventas
VALUES (default, CURDATE(), 'Venta 5', 150.0);

-- pagos
INSERT INTO pagos
VALUES (default, CURDATE(), 1, FALSE, FALSE, TRUE, NULL, NULL, 'Comida pal staff', 'ABCDEFGHIJ1234567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, TRUE, FALSE, FALSE, NULL, NULL, 'Pago agua', 'ABCDEFGHIJ2234567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, FALSE, TRUE, FALSE, NULL, 1, 'Pago a Instructor', 'ABCDEFGHIJ2224567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, FALSE, FALSE, TRUE, NULL, NULL, 'Jueguete pal staff', 'ABCDEFGHIJ3234567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, TRUE, FALSE, FALSE, NULL, NULL, 'Pago luz', 'ABCDEFGHIJ3324567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, FALSE, TRUE, FALSE, 1, NULL, 'Pago a Usuario', 'ABCDEFGHIJ3334567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, FALSE, FALSE, TRUE, NULL, NULL, 'Saldo pal staff', 'ABCDEFGHIJ4234567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, TRUE, FALSE, FALSE, NULL, NULL, 'Pago gas', 'ABCDEFGHIJ4424567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, FALSE, TRUE, FALSE, NULL, 2, 'Pago a Instructor', 'ABCDEFGHIJ4444567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, FALSE, FALSE, TRUE, NULL, NULL, 'Cervezas pal staff', 'ABCDEFGHIJ5234567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, TRUE, FALSE, FALSE, NULL, NULL, 'Pago uranio', 'ABCDEFGHIJ5524567890', 100.0);
INSERT INTO pagos
VALUES (default, CURDATE(), 1, FALSE, TRUE, FALSE, 1, NULL, 'Pago a Usuario', 'ABCDEFGHIJ5554567890', 100.0);

-- ingresos
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, 1, NULL, NULL, FALSE, 'Renta', 'ABCDEFGHIJ1234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, 1, NULL, FALSE, 'Cliente', 'ABCDEFGHIJ2234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, 1, FALSE, 'Venta', 'ABCDEFGHIJ2224567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, NULL, TRUE, 'Reposición juguete staff', 'ABCDEFGHIJ3234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, 2, NULL, NULL, FALSE, 'Renta', 'ABCDEFGHIJ3324567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, 2, NULL, FALSE, 'Cliente', 'ABCDEFGHIJ3334567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, 2, FALSE, 'Venta', 'ABCDEFGHIJ4234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, NULL, TRUE, 'Reposición Maquina Gym ', 'ABCDEFGHIJ4424567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, 3, NULL, NULL, FALSE, 'Renta', 'ABCDEFGHIJ4444567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, 3, NULL, FALSE, 'Cliente', 'ABCDEFGHIJ5234567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, 3, FALSE, 'Venta', 'ABCDEFGHIJ5524567890', 100.0);
INSERT INTO ingresos
VALUES (default, CURDATE(), 1, NULL, NULL, NULL, TRUE, 'Reposición comida staff', 'ABCDEFGHIJ5554567890', 100.0);
