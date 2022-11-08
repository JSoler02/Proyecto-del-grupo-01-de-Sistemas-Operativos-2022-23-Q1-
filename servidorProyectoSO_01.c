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

typedef struct{
	char nombre[20];
	int socket;
}Conectado;

typedef struct{
	Conectado conectados[100];
	int num;
}ListaConectados;

int PonConectado(ListaConectados *lista, char nombre[20], int socket){
	// A￱ade nuevo conectado.retorna 0 si ha ido bien y -1 si la lista ya 
	// estaba llena y no lo ha podido a￱adir
	if (lista->num == 100)
		return -1;
	else{
		strcpy(lista->conectados[lista->num].nombre,nombre);
		lista->conectados[lista->num].socket= socket;
		lista->num = lista->num +1;
		return 0;
	}
}

int DameSocket (ListaConectados *lista, char nombre[20])
{ //Devuelve el socket o -1 si no lo encuentra
	int i = 0;
	int encontrado = 0;
	//b￯﾿ﾺsqueda
	while ((i<lista->num)&&(encontrado == 0))
	{
		if (strcmp(lista->conectados[i].nombre, nombre) ==0)
			encontrado =1;
		if (encontrado == 0)
			i++;
		
	}
	if (encontrado == 1)
		return lista->conectados[i].socket;
	else
		return -1;
	
}

int DamePosicion (ListaConectados *lista, char nombre[20])
{ //Devuelve la posicion o -1 si no lo encuentra
	int i = 0;
	int encontrado = 0;
	//b￯﾿ﾺsqueda
	while ((i<lista->num)&&(encontrado == 0))
	{
		if (strcmp(lista->conectados[i].nombre, nombre) ==0)
			encontrado =1;
		if (encontrado == 0)
			i++;
		
	}
	if (encontrado == 1)
		return i;
	else
		return -1;
	
}

int EliminaConectado (ListaConectados *lista, char nombre[20])	
{ // Devuelve 0 si elimina y -1 si el usuario no est￯﾿ﾡ en la lista
	int pos = DamePosicion (lista,nombre);
	if (pos == -1)
		return -1;
	else
	{ // Haremos un for, y desplazaremos a todos hacia la izquierda
		// Bucle para eliminar comienza en la posicion a eliminar
		// Y va hasta el num -1
		for (int i = pos; i<lista->num-1;i++)
		{
			strcpy(lista->conectados[i].nombre, lista->conectados[i+1].nombre);
			lista->conectados[i].socket = lista->conectados[i+1].socket;
		}
		lista->num --; //restamos 1
		return 0;
	}
}

void DameNombreConectados (ListaConectados *lista, char respuesta[512])
{ //Devuelve los nombres de los jugadores conectados separados por &quot;/&quot;
	//Primero pone el numero de conectado. &quot;3/Juan/Pedro/Maria&quot;
	sprintf (respuesta, "%d", lista->num);
	for (int i = 0; i<lista->num; i++)
	{
		sprintf (respuesta, "%s/%s", respuesta, lista->conectados[i].nombre);
	}
}

void DameSocketsDeConectados (ListaConectados *lista, char conectados[512], char sockets[200])
{ //Recibe una lista con nombres de jugadores separados por &quot;/&quot;: &quot;3/Juan/Pedro/Maria&quot;
	//Devuelve una lista con los sockets de estos jugadores separados por &quot;/&quot;:&quot;3/5/1/3&quot;
	char *p = strtok (conectados, "/");
	int n = atoi (p);
	sprintf (sockets,"%d", n);
	p=strtok(NULL, "/");
	char nombre [20];
	int socket;
	for (int i = 0; i<n; i++)
	{
		strcpy (nombre, p);
		int pos = DamePosicion(lista, nombre);
		if (pos != -1)
		{
			socket = lista->conectados[i].socket;
			
			sprintf(sockets, "%s/%d", sockets, socket);
		}
	}
}


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
		ListaConectados listaconectados;
		listaconectados.num = 0;
		
		
		
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
			LogIn(username, password, consulta, respuesta, &listaconectados, p, conn, err, resultado, row, s);
		
		else if (codigo ==2) // quieren crear un usuario
			CrearUsuario(username, password, consulta, respuesta, p, conn, err, resultado, row);
		
		else if (codigo == 3)
			Consulta1(consulta, nombre, respuesta, max_jugador, p, conn, err, resultado, row);
	
		else if (codigo == 4)
			Consulta2(consulta, nombre, respuesta, p, conn, err, resultado, row);
		
		else if (codigo == 5)	
			Consulta2(consulta, mapa, respuesta, p, conn, err, resultado, row);
		
		else if (codigo == 6)
		{
			// Dame lista conectados
			printf(listaconectados.num);
			DameNombreConectados(&listaconectados, respuesta);
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

void LogIn(char user[60], char passw[60], char cons[500], char resp[512], ListaConectados *lista, char *p, MYSQL *conn, int err, MYSQL_RES *resultado, MYSQL_ROW row, int *s)
{
	// Obtenemos usuario
	p = strtok(NULL, "/");
	strcpy(user, p);
	
	//Obtenemos contrase￯﾿ﾱa
	p = strtok(NULL, "/");
	strcpy(passw, p);
	
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
	strcpy(cons, "SELECT DISTINCT jugador.username, jugador.password FROM (jugador) WHERE jugador.username = '");
	strcat(cons, user);
	strcat(cons, "';");
	
	err = mysql_query (conn, cons);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("No se han obtenido datos en la consulta\n");
		strcpy(resp, "Usuario no encontrado");
	}
	else
	{
		if (strcmp(passw,row[1]) == 0){
			int res = PonConectado(&lista, user, &s);
			if (res == -1)
				strcpy(resp,"No se ha podido conectar el usuario");
			else {
				strcpy(resp, "Conectado");
			}
		}
		else
			strcpy(resp,"No conectado, warning");
	}
	mysql_close (conn);
	
}

void CrearUsuario(char user[60], char passw[60], char cons[500], char resp[512], char *p, MYSQL *conn, int err, MYSQL_RES *resultado, MYSQL_ROW row)
{
	// Obtenemos usuario
	p = strtok(NULL, "/");
	strcpy(user, p);
	
	//Obtenemos contrase￯﾿ﾱa
	p = strtok(NULL, "/");
	strcpy(passw, p);
	
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
	
	strcpy(cons, "SELECT jugador.username FROM (jugador) WHERE jugador.username = '");
	strcat(cons, user);
	strcat(cons, "';");
	
	err = mysql_query (conn, cons);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	
	
	if (row == NULL) // Afegim usuari
	{
		
		//COnsultamos el ID MAX
		strcpy(cons, "SELECT MAX(jugador.id) FROM (jugador);");
		err = mysql_query (conn, cons);
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
		strcpy(cons, "INSERT INTO jugador VALUES (");
		strcat(cons,id_max_s);
		strcat(cons,",'");
		strcat(cons,user);
		strcat(cons,"','");
		strcat(cons,passw);
		strcat(cons,"');");
		
		err = mysql_query (conn, cons);
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
			exit (1);
		}
		strcpy(resp,"Usuario creado GOOOOD");
	}
	else
	{
		strcpy(resp, "Usuario ya existente");
	}
	mysql_close (conn);
}

void Consulta1(char cons[500], char name[60], char resp[512], int max, char *p, MYSQL *conn, int err, MYSQL_RES *resultado, MYSQL_ROW row)
{
	//quiere saber la puntuacion maxima de Maria
	strcpy (name, "'Maria'");
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
	strcpy(cons, "SELECT MAX(historial.puntos) FROM (jugador, historial) WHERE historial.id_j= (SELECT jugador.id FROM (jugador) WHERE jugador.username = ");
	strcat(cons, name);
	strcat(cons, ");");
	
	err = mysql_query (conn, cons);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	if (row == NULL)
		printf ("No se han obtenido datos en la consulta\n");
	else
		max = atoi (row[0]);
	// cerrar la conexion con el servidor MYSQL 
	mysql_close (conn);
	sprintf (resp,"%d", max);
}

void Consulta2(char cons[500], char name[60], char resp[512], char *p, MYSQL *conn, int err, MYSQL_RES *resultado, MYSQL_ROW row)
{
	// Partidas en las que Juan ha jugado mas de 120 s	
	strcpy (name, "'Juan'");
	
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
	strcpy (cons,"SELECT partida.id FROM (jugador, partida, historial) WHERE jugador.username ="); 
	strcat (cons, name);
	strcat (cons," AND jugador.id = historial.id_j AND historial.id_p = partida.id AND partida.id IN (SELECT partida.id FROM (partida) WHERE partida.duracion > 120);");
	
	err=mysql_query (conn, cons);
	
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
			strcpy(resp, row[0]);
			row = mysql_fetch_row (resultado);
			/*sprintf (respuesta,"%s/%s",respuesta, row[0]);*/
			
		}
	}
	// cerrar la conexion con el servidor MYSQL 
	mysql_close (conn);
}

void Consulta3(char cons[500], char map[60], char resp[512], char *p, MYSQL *conn, int err, MYSQL_RES *resultado, MYSQL_ROW row)
{
	//Jugadores que han jugado en templo como J1
	strcpy(map, "'templo'");
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
	strcpy (cons,"SELECT jugador.username FROM (jugador, partida, historial) WHERE partida.mapa = "); 
	strcat (cons, map);
	strcat (cons," AND historial.id_p = partida.id AND historial.id_j = jugador.id AND historial.posicion = 1;");
	// hacemos la consulta 
	err=mysql_query (conn, cons); 
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
			sprintf (resp, "%s %s", resp, row[0]);
			// obtenemos la siguiente fila
			row = mysql_fetch_row (resultado);
	}		
		// cerrar la conexion con el servidor MYSQL 
		mysql_close (conn);
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
	serv_adr.sin_port = htons(9070);
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
