//Incluir esta libreria para poder hacer las llamadas en shiva2.upc.es
//#include <my_global.h>
#include <mysql.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <ctype.h>
#include <pthread.h>




void *AtenderCliente (void *socket)
{
	int sock_conn, ret;
	int *s;
	s= (int *) socket;
	sock_conn = *s;
	
	char peticion[512];
	char login[512];
	char respuesta[512];

	int terminar =0;
		// Entramos en un bucle para atender todas las peticiones de este cliente
		//hasta que se desconecte
		while (terminar ==0)
		{
			// Ahora recibimos la petici?n
			ret = read(sock_conn,peticion, sizeof(peticion));
			printf ("Recibido\n");
			
			// Tenemos que a?adirle la marca de fin de string 
			// para que no escriba lo que hay despues en el buffer
			peticion[ret]='\0';
			printf ("Peticion: %s\n",peticion);
			
			// vamos a ver que quieren
			char *p = strtok( peticion, "/");
			int codigo =  atoi (p);
			
			// Ya tenemos el c?digo de la peticion
			// Ahora creamos las variables que podremos sacar de las consultas
			// Variables sql:
			MYSQL *conn;
			int err;
			// Estructura especial para almacenar resultados de consultas 
			MYSQL_RES *resultado;
			MYSQL_ROW row;
			// otras variables
			int max_jugador;			
			char consulta [500];
			char nombre[60];
			char mapa[60];
			char peticion[500];
			char username[60];
			char password[60];
			
			
			/* Por si queremos hacer que se imprima en consola el username y el n mero de petici?n que hace
			if (codigo !=0)
			{
			p = strtok( NULL, "/");
			
			strcpy (nombre, p);
			// Ya tenemos el nombre
			printf ("Codigo: %d, Nombre/Palabra: %s\n", codigo, nombre);
			}*/
			
			strcpy(respuesta, ""); // vaciamos respuesta
			
			if (codigo ==0) //petici?n de desconexi?n
				terminar=1;
			
			else if (codigo ==1) //piden logearse
			{
				// Obtenemos usuario
				p = strtok(NULL, "/");
				strcpy(username, p);
				
				//Obtenemos contrase￱a
				p = strtok(NULL, "/");
				strcpy(password, p);
				
				//Creamos conexion con MYSQL
				conn= mysql_init(NULL);
				if (conn==NULL)  {
					printf ("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				//inicializar la conexion
				conn = mysql_real_connect (conn, "localhost","root", "mysql", "bdFireWater",0, NULL, 0);
				if (conn==NULL)  {
					printf ("Error al inicializar la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				
				//Hacemos la consulta
				strcpy(consulta, "SELECT DISTINCT jugador.username, jugador.password FROM (jugador) WHERE jugador.username = '");
				strcat(consulta, username);
				strcat(consulta, "';");
				
				err = mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				resultado = mysql_store_result (conn);
				
				row = mysql_fetch_row (resultado);
				if (row == NULL)
				{
					printf ("No se han obtenido datos en la consulta\n");
					strcpy(respuesta, "Usuario no encontrado");
				}
				else
				{
					if (strcmp(password,row[1]) == 0)
						strcpy(respuesta, "Login EXITOSO");
					else
						strcpy(respuesta,"Password MAL");
				}
				mysql_close (conn);

				
			}
			else if (codigo ==2) // quieren crear un usuario
			{
				// Obtenemos usuario
				p = strtok(NULL, "/");
				strcpy(username, p);
				
				//Obtenemos contrase￱a
				p = strtok(NULL, "/");
				strcpy(password, p);
				
				//Creamos conexion con MYSQL
				conn= mysql_init(NULL);
				if (conn==NULL)  {
					printf ("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				//inicializar la conexion
				conn = mysql_real_connect (conn, "localhost","root", "mysql", "bdFireWater",0, NULL, 0);
				if (conn==NULL)  {
					printf ("Error al inicializar la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				
				//Hacemos la consulta
				
				strcpy(consulta, "SELECT jugador.username FROM (jugador) WHERE jugador.username = '");
				strcat(consulta, username);
				strcat(consulta, "';");
				
				err = mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				
				resultado = mysql_store_result (conn);
				
				row = mysql_fetch_row (resultado);
				
				
				if (row == NULL) // Afegim usuari
				{
					
					//COnsultamos el ID MAX
					strcpy(consulta, "SELECT MAX(jugador.id) FROM (jugador);");
					err = mysql_query (conn, consulta);
					if (err!=0) {
						printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
						exit (1);
					}
					resultado = mysql_store_result (conn);
					
					
					row = mysql_fetch_row (resultado);
					
					int id_max;
					char id_max_s[20];
					id_max = atoi(row[0])+1;
					sprintf(id_max_s,"%d",id_max);
					
					// Hacemos insert
					strcpy(consulta, "INSERT INTO jugador VALUES (");
					strcat(consulta,id_max_s);
					strcat(consulta,",'");
					strcat(consulta,username);
					strcat(consulta,"','");
					strcat(consulta,password);
					strcat(consulta,"');");

					
					err = mysql_query (conn, consulta);
					if (err!=0) {
						printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
						exit (1);
					}
					
					strcpy(respuesta,"Usuario creado GOOOOD");
					
					
				}
				else
				{
					strcpy(respuesta, "Usuario ya existente");
				}
				mysql_close (conn);
				
				
			}
			
			else if (codigo == 3)
			{	//quiere saber la puntuacion maxima de Maria
				strcpy (nombre, "'Maria'");
				// reamos una conexion al servidor MYSQL 
				conn= mysql_init(NULL);
				if (conn==NULL)  {
					printf ("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				//inicializar la conexion
				conn = mysql_real_connect (conn, "localhost","root", "mysql", "bdFireWater",0, NULL, 0);
				if (conn==NULL)  {
					printf ("Error al inicializar la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				// consulta SQL para obtener una tabla con todos los datos
				strcpy(consulta, "SELECT MAX(historial.puntos) FROM (jugador, historial) WHERE historial.id_j= (SELECT jugador.id FROM (jugador) WHERE jugador.username = ");
				strcat(consulta, nombre);
				strcat(consulta, ");");
				
				err = mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				resultado = mysql_store_result (conn);
				
				row = mysql_fetch_row (resultado);
				if (row == NULL)
					printf ("No se han obtenido datos en la consulta\n");
				else
					max_jugador = atoi (row[0]);
				// cerrar la conexion con el servidor MYSQL 
				mysql_close (conn);
				sprintf (respuesta,"%d", max_jugador);
			}
			else if (codigo == 4)
			{	// Partidas en las que Juan ha jugado mas de 120 s
				strcpy (nombre, "'Juan'");
				
				//Creamos conexion
				conn = mysql_init(NULL);
				if (conn==NULL) {
					printf ("Error al crear la conexion: %u %s\n", 
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				//inicializar la conexion
				conn = mysql_real_connect (conn, "localhost","root", "mysql", "bdFireWater",0, NULL, 0);
				if (conn==NULL) {
					printf ("Error al inicializar la conexion: %u %s\n", 
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				// Ahora vamos a realizar la consulta
				printf ("Ahora encontraremos los id de las partidas cuya duracion sea mayor a 120 y donde Juan haya participado: \n");
				strcpy (consulta,"SELECT partida.id FROM (jugador, partida, historial) WHERE jugador.username ="); 
				strcat (consulta, nombre);
				strcat (consulta," AND jugador.id = historial.id_j AND historial.id_p = partida.id AND partida.id IN (SELECT partida.id FROM (partida) WHERE partida.duracion > 120);");
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				// Recogemos el resultado
				resultado = mysql_store_result (conn); 
				row = mysql_fetch_row (resultado);
				//int id_max;
				
				
				if (row == NULL)
					printf ("No se han obtenido datos en la consulta\n");
				else
				{
					while (row != NULL)
					{
						// obtenemos la siguiente fila
						//id_max = row[0];
						strcpy(respuesta, row[0]);
						row = mysql_fetch_row (resultado);
						/*sprintf (respuesta,"%s/%s",respuesta, row[0]);*/
						
					}
				}
				
				// cerrar la conexion con el servidor MYSQL 
				mysql_close (conn);
					
			}
			else //codigo == 5)	
			{	//Jugadores que han jugado en templo como J1
				strcpy(mapa, "'templo'");
				//Creamos una conexion al servidor MYSQL 
				conn = mysql_init(NULL);
				if (conn==NULL) {
					printf ("Error al crear la conexion: %u %s\n", 
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				//inicializar la conexion
				conn = mysql_real_connect (conn, "localhost","root", "mysql", "bdFireWater",0, NULL, 0);
				if (conn==NULL) {
					printf ("Error al inicializar la conexion: %u %s\n", 
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				// Ahora vamos a realizar la consulta
				printf ("Ahora encontraremos el username de los usuarios que han jugado en el mapa 'templo' como jugador 1: \n");
				// construimos la consulta SQL
				strcpy (consulta,"SELECT jugador.username FROM (jugador, partida, historial) WHERE partida.mapa = "); 
				strcat (consulta, mapa);
				strcat (consulta," AND historial.id_p = partida.id AND historial.id_j = jugador.id AND historial.posicion = 1;");
				// hacemos la consulta 
				err=mysql_query (conn, consulta); 
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				//recogemos el resultado de la consulta 
				resultado = mysql_store_result (conn); 
				row = mysql_fetch_row (resultado);
				if (row == NULL)
					printf ("No se han obtenido datos en la consulta\n");
				else
					while (row != NULL){
						printf("Username: %s\n", row[0]);
						sprintf (respuesta, "%s %s", respuesta, row[0]);
						// obtenemos la siguiente fila
						row = mysql_fetch_row (resultado);
				}
					
					
					// cerrar la conexion con el servidor MYSQL 
					mysql_close (conn);
			}
			if (codigo !=0)
			{
				
				printf ("Respuesta: %s\n", respuesta);
				// Enviamos respuesta
				write (sock_conn,respuesta, strlen(respuesta));
			}
		}
		// Se acabo el servicio para este cliente
		close(sock_conn); 
	}
}


int main(int argc, char *argv[])
{
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;

	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket\n");
	// Fem el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// establecemos el puerto de escucha
	serv_adr.sin_port = htons(9050);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind\n");
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen\n");
	
	int i;
	int sockets[100];
	pthread_t thread; //donde se guardan los ID de los sockets
	// Bucle infinito
	for (;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		sockets[i] = sock_conn;
		// Creamos thread y le decimos lo que tiene que hacer
		pthread_create (&thread, NULL, AtenderCliente, &sockets[i]);
		i=i+1;
		}		
	// Bucle para hacer que el servidor se espere a que acabe de todos los clientes.
	/*for (i=0; i<5, i++)
		pthread_join (thread[i], NULL);
	*/
	
}