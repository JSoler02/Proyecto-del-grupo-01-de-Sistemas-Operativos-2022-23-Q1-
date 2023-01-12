DROP DATABASE IF EXISTS T1bdFireWater;
CREATE DATABASE T1bdFireWater;

USE T1bdFireWater;

CREATE TABLE jugador (
	id INT NOT NULL,
	username VARCHAR(60) NOT NULL,
	password VARCHAR (60) NOT NULL,
	PRIMARY KEY (id)
)ENGINE=InnoDB;

CREATE TABLE partida (
	id INT NOT NULL,
	mapa	VARCHAR(30) NOT NULL,
	resultado VARCHAR(30),
	nota VARCHAR(5),
	duracion INT, 
	PRIMARY KEY (id)
	
)ENGINE=InnoDB;

CREATE TABLE historial (
	id_j INT NOT NULL,
	id_p INT NOT NULL,
	posicion INT,
	FOREIGN KEY (id_j)REFERENCES jugador(id),
	FOREIGN KEY (id_p)REFERENCES partida(id)
)ENGINE=InnoDB;

INSERT INTO jugador VALUES(1,"Juan","123");
INSERT INTO jugador VALUES(2,"Maria","123");
INSERT INTO jugador VALUES(3,"Bernat","contrasenya");

INSERT INTO partida VALUES(1,"Templo","Superado", "A", 75);
INSERT INTO partida VALUES(2,"Templo","Superado", "S", 45);
INSERT INTO partida VALUES(3,"Volcan","No Superado", "F", 180);

INSERT INTO historial VALUES(1,1,1);
INSERT INTO historial VALUES(2,1,2);
INSERT INTO historial VALUES(3,2,3);
INSERT INTO historial VALUES(1,2,1);
INSERT INTO historial VALUES(2,2,2);
INSERT INTO historial VALUES(3,3,1);
INSERT INTO historial VALUES(1,3,4);
INSERT INTO historial VALUES(2,3,2);