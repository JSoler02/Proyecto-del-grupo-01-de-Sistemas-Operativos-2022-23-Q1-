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

// estructura de jugador = conectado pero con nPersonaje
typedef struct{
	char nombre[20];
	int socket;
	int posicion;
}Jugador;
// Estructura de partida. Tiene como maximo 4 jugadores
// campo ocupado = 0: si libre; ocupado=1: si ocupado
typedef struct {
	Jugador jugadores[4];
	int numjugadores;
	int invitaciones;
	int ocupado;
}Partida;

ListaConectados listaconectados;

Partida listaPartidas[20];
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

// Variables globales del socket
int sock_conn, sock_listen;
struct sockaddr_in serv_adr;
int *s;
// Variables globales SQL
MYSQL *conn;
int err;
MYSQL_RES *resultado;
MYSQL_ROW row;

int i;
int sockets[100];

//Variables de desarrollo
int shiva = 1; //1: si Shiva; 0: si MaqVirtual
//Esta funcion retorna el puerto y el rellena (en el char de entrada) el Host 
// dependiendo de si estamos en el entorno de desarrollo o el de produccion
int DamePuertoYHost (int shiva, char host[50])
{
	int puerto;
	if (shiva == 0)
	{
		strcpy(host, "localhost");
		puerto = 8090;
	}
	else 
	{
		strcpy(host, "shiva2.upc.es");
		puerto = 50050;
	}
	return puerto;
}

// * * * * * * * * * Funciones ListaConectados
	// Pone nuevo conectado en la lista de conectados. Retorna 0 si ha ido bien y -1 si la lista ya 
	// estaba llena y no lo ha podido poner
int PonConectado(ListaConectados *lista, char nombre[20], int socket){

	if (lista->num == 100)
		return -1;
	else{
		strcpy(lista->conectados[lista->num].nombre,nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num = lista->num +1;
		return 0;
	}
}
 //Retorna el socket de un conectado. Si no lo encuentra, retorna un -1.
int DameSocket (ListaConectados *lista, char nombre[20])
{
	int i = 0;
	int encontrado = 0;
	//busqueda
	while ((i<lista->num)&&(encontrado == 0))
	{
		if (strcmp(lista->conectados[i].nombre, nombre) ==0)
			encontrado =1;
		else if (encontrado == 0)
			i++;
		
	}
	if (encontrado == 1)
		return lista->conectados[i].socket;
	else
		return -1;
	
}
//Retorna la posicion de un conectado en la lista de conectados. Si no lo encuentra, retorna un -1. 
int DamePosicion (ListaConectados *lista, char nombre[20])
{ 
	int i = 0;
	int encontrado = 0;
	//b\uffef\uffbf\uffaf\uffef\uffbe\uffbf\uffef\uffbe\uffbasqueda
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

// Elimina un conectado de la lista de conectados. Retorna un 0 si elimina y -1 si el usuario no esta en la lista.
// Elimina el socket de la lista global de sockets
int EliminaConectado (ListaConectados *lista, char nombre[20])	
{ 
	int pos = DamePosicion (lista,nombre);
	if (pos == -1)
		return -1;
	else
	{ // Haremos un for, y desplazaremos a todos hacia la izquierda
		// Bucle para eliminar comienza en la posicion a eliminar
		// Y va hasta el num -1
		
		int socket = DameSocket(lista, nombre);
		
		int found = 0;
		int x = 0;
		
		while (!found && x < 100)
		{
			if (sockets[x] == socket)
				found = 1;
			else
				x++;
		}
		
		
		for (int j = x; j < 99; j++)
		{
			sockets[j]= sockets[j+1];
		}
		i = i -1; // i de los sockets
		
		for (int y = pos; y<lista->num-1;y++)
		{
			strcpy(lista->conectados[y].nombre, lista->conectados[y+1].nombre);
			lista->conectados[y].socket = lista->conectados[y+1].socket;
		}
		lista->num --; //restamos 1
		
		
		return 0;
	}
}
 //Retorna los nombres de los jugadores conectados separados por "/" en la variable vacia de entrada "respuesta". 
//Primero pone el numero de conectado. "3/Juan/Pedro/Maria"
void DameNombreConectados (ListaConectados *lista, char respuesta[512])
{
	sprintf (respuesta, "%d", lista->num);
	for (int i = 0; i<lista->num; i++)
	{
		sprintf (respuesta, "%s/%s", respuesta, lista->conectados[i].nombre);
	}
}
 //Recibe una lista con nombres de jugadores separados por "/";: "3/Juan/Pedro/Maria";
//Retorna una lista con los sockets de estos jugadores separados por "/":"3/5/1/3" en la variable vacia de entrada "sockets";
void DameSocketsDeConectados (ListaConectados *lista, char conectados[512], char sockets[200])
{
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
// * * * * * * * * * Funciones ListaPartidas

// Retorna la id de la partida (busca el primero libre)
// Retorna -1 si no encuentra ninguna libre
int BuscarPartidaLibre(Partida lista[20])
{
	int j = 0;
	int encontrado = 0;
	while ((j< 20)&&(encontrado == 0))
	{
		if (lista[j].ocupado == 0)
			encontrado = 1;
		if (encontrado == 0)
			j=j+1;
	}
	if (encontrado == 1)
		return j;
	else
		return -1;	
}


// Elimina la partida de la lista: pone a 0 el parametro "ocupado" 
// de la partida que le venga como parametro. Eliminamos (ponemos a -1)
// tambien los sockets de la lista de partidas
void AcabaPartida (Partida lista[20], int idpartida)	
{
	lista[idpartida].numjugadores = 0;
	lista[idpartida].invitaciones = 0;
	lista[idpartida].ocupado = 0;
	for (int i=0;i < 4;i++)
	{
		lista[idpartida].jugadores[i].socket = -1;
		lista[idpartida].jugadores[i].posicion = -1;		
		strcpy(lista[idpartida].jugadores[i].nombre, "");
	}
}

// Esta funcion establece el numero de invitaciones que se necesitan para empezar la Partida, poniendo el anfitrion en la partida.
// Pone a 1 el parametro "ocupado" conforme esa partida esta ocupada.
void PonInvitacionesYAnfitrionEnPartida(Partida lista[20], int idpartida, char jugador[20], int n_invitaciones)
{
	if (lista[idpartida].ocupado == 0)
	{
		AcabaPartida(lista, idpartida);
		lista[idpartida].ocupado = 1;
	}
	//printf ("Antes de poner: Partida n%d tiene a los jugadores en este orden:  %s - %d, %s- %d, %s- %d, %s- %d --> %d\n", idpartida, listaPartidas[idpartida].jugadores[0].nombre,listaPartidas[idpartida].jugadores[0].socket,listaPartidas[idpartida].jugadores[1].nombre, listaPartidas[idpartida].jugadores[1].socket,listaPartidas[idpartida].jugadores[2].nombre,listaPartidas[idpartida].jugadores[2].socket, listaPartidas[idpartida].jugadores[3].nombre,listaPartidas[idpartida].jugadores[3].socket, listaPartidas[idpartida].numjugadores);
	
	int s1 = DameSocket(&listaconectados,jugador);
	lista[idpartida].jugadores[lista[idpartida].numjugadores].socket = s1;
	strcpy(lista[idpartida].jugadores[lista[idpartida].numjugadores].nombre, jugador);
	lista[idpartida].numjugadores = lista[idpartida].numjugadores +1;
	lista[idpartida].invitaciones = n_invitaciones;
	printf("En esta partida: [%d] se necesitan %d invitaciones aceptadas\n", idpartida, lista[idpartida].invitaciones);
	
	//printf ("Luego de poner: Partida n%d tiene a los jugadores en este orden: %s - %d, %s- %d, %s- %d, %s- %d --> %d\n", idpartida, listaPartidas[idpartida].jugadores[0].nombre,listaPartidas[idpartida].jugadores[0].socket, listaPartidas[idpartida].jugadores[1].nombre, listaPartidas[idpartida].jugadores[1].socket,listaPartidas[idpartida].jugadores[2].nombre,listaPartidas[idpartida].jugadores[2].socket, listaPartidas[idpartida].jugadores[3].nombre,listaPartidas[idpartida].jugadores[3].socket, listaPartidas[idpartida].numjugadores);
	
}
//Pone al jugador en partida (menos al anfitrion).
void PonJugadorPartida(Partida lista[20], int idpartida, char jugador[20])
{
	//printf ("Antes de poner: Partida n%d tiene a los jugadores en este orden:  %s - %d, %s- %d, %s- %d, %s- %d --> %d\n", idpartida, listaPartidas[idpartida].jugadores[0].nombre,listaPartidas[idpartida].jugadores[0].socket,listaPartidas[idpartida].jugadores[1].nombre, listaPartidas[idpartida].jugadores[1].socket,listaPartidas[idpartida].jugadores[2].nombre,listaPartidas[idpartida].jugadores[2].socket, listaPartidas[idpartida].jugadores[3].nombre,listaPartidas[idpartida].jugadores[3].socket, listaPartidas[idpartida].numjugadores);	
	int s1 = DameSocket(&listaconectados,jugador);
	lista[idpartida].jugadores[lista[idpartida].numjugadores].socket = s1;
	strcpy(lista[idpartida].jugadores[lista[idpartida].numjugadores].nombre, jugador);
	lista[idpartida].numjugadores = lista[idpartida].numjugadores +1;
	
	//printf ("Luego de poner: Partida n%d tiene a los jugadores en este orden: %s - %d, %s- %d, %s- %d, %s- %d --> %d\n", idpartida, listaPartidas[idpartida].jugadores[0].nombre,listaPartidas[idpartida].jugadores[0].socket, listaPartidas[idpartida].jugadores[1].nombre, listaPartidas[idpartida].jugadores[1].socket,listaPartidas[idpartida].jugadores[2].nombre,listaPartidas[idpartida].jugadores[2].socket, listaPartidas[idpartida].jugadores[3].nombre,listaPartidas[idpartida].jugadores[3].socket, listaPartidas[idpartida].numjugadores);
}

// Actualiza la decision sobre la Partida.
// Retorna el numero de invitaciones faltantes para empezar la partida.
int  AceptaInvitacionYDameFaltantes(Partida lista[20], int idpartida)
{
	printf ("Antes de restar faltan %d para empezar la partida [%d]\n", lista[idpartida].invitaciones, idpartida);
	int n_invitaciones_Faltantes = 	lista[idpartida].invitaciones - 1;
	lista[idpartida].invitaciones = n_invitaciones_Faltantes;
	printf ("Ahora faltan %d para empezar la partida [%d]\n", n_invitaciones_Faltantes, idpartida);
	return n_invitaciones_Faltantes;
}

// Funcion que pone la posicion (1,2,3 o 4 --> nJugador) en la posicion del jugador de la Partida (Jugador1, Jugador 2, ...)
void PonPosicionJugadorPartida(Partida lista[20], int idpartida, char jugador[20], int posicion)
{
	// buscamos el nombre y su posicon relativa en la lista de jugadores de la partida
	int encontrado = 0;
	int x = 0;
	while (encontrado == 0 && x<lista[idpartida].numjugadores)
	{
		if (strcmp(lista[idpartida].jugadores[x].nombre, jugador) ==0)
		{
			encontrado = 1;
		}
		if (encontrado == 0)
			x= x+1;
	}
	// ponemos la posicon de personaje
	if (encontrado == 1)
		lista[idpartida].jugadores[x].posicion = posicion;
}
// Esta funcion hace el LogIn. Necesita un nombre y una contrasenya
// Retorna 0 si todo OK, -1 si no es correcto o 1 si contrasenya incorrecta
int LogIn(char user[60], char passw[60])
{
	char cons[500];
	//Hacemos la consulta
	strcpy(cons, "SELECT DISTINCT jugador.username, jugador.password FROM (jugador) WHERE jugador.username = '");
	strcat(cons, user);
	strcat(cons, "';");
	
	err = mysql_query (conn, cons);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("No se han obtenido datos en la consulta\n");
		return -1;
	}
	else
	{
		if (strcmp(passw,row[1]) == 0)
		{
			
			pthread_mutex_lock (&mutex);
			int res = PonConectado(&listaconectados, user, *s);
			printf("[Ponconectado(%s,%d)]\n",user,*s);
			pthread_mutex_unlock (&mutex);
			if (res == -1)
				return -1;
			else 
			{
				//strcpy(resp, "Conectado");
				
				
				return 0;
				
			}
		}
		else
			//strcpy(resp,"No conectado, warning");
			// contrasenya incorrecta
			return 1;
	}
	
}

// Esta funcion crea un Usuario. Necesita un nombre y una contrasenya.
// Retorna 0 si todo OK, -1 si no se hace correctamente o 1 si usuario existente
int CrearUsuario(char user[60], char passw[60])
{
	char cons[500];
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
			return -1;
		}
		return 0;
	}
	else
	{
		//strcpy(resp, "Usuario ya existente");
		return 1;
	}
	
}


// Retorna la puntuacion maxima que ha conseguido el usuario en sus partidas --> lo rellena en el parametro nota
// Tambien necesita como paramentro el nombre del jugador
void Consulta1Buena(char nombre[20], char nota[20])
{
	char cons[500];
	char name [20];

	//quiere saber la puntuacion maxima de usuario
	strcpy (name, nombre);
	
	//sprintf(cons, "SELECT MAX(historial.puntos) FROM (jugador, historial) WHERE historial.id_j= (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s');", name);
	// consulta SQL para obtener una tabla con todos los datos
	// miramos primero si hay una "S"; luego si hay una "A", una "B", una "C", y por ￺ltimo una "F".
	sprintf(cons, "SELECT (partida.nota) FROM (jugador, partida, historial) WHERE historial.id_j= (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s') AND historial.id_p = partida.id AND partida.nota = 'S';", name);
	err = mysql_query (conn, cons);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		//printf ("No se han obtenido datos en la consulta\n");
		// miramos las "A"
		char consA[500];
		sprintf(consA, "SELECT (partida.nota) FROM (jugador, partida, historial) WHERE historial.id_j= (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s') AND historial.id_p = partida.id AND partida.nota = 'A';", name);
		err = mysql_query (conn, consA);
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
			sprintf(nota,"No hay datos");
		}
		resultado = mysql_store_result (conn);
		row = mysql_fetch_row (resultado);
		if (row == NULL)
		{
			//printf ("No se han obtenido datos en la consulta\n");
			// miramos las "B"
			char consB[500];
			sprintf(consB, "SELECT (partida.nota) FROM (jugador, partida, historial) WHERE historial.id_j= (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s') AND historial.id_p = partida.id AND partida.nota = 'B';", name);
			err = mysql_query (conn, consB);
			if (err!=0) {
				printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
				sprintf(nota,"No hay datos");
			}
			resultado = mysql_store_result (conn);
			row = mysql_fetch_row (resultado);
			if (row == NULL)
			{
				//printf ("No se han obtenido datos en la consulta\n");
				// miramos las "C"
				char consC[500];
				sprintf(consC, "SELECT (partida.nota) FROM (jugador, partida, historial) WHERE historial.id_j= (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s') AND historial.id_p = partida.id AND partida.nota = 'C';", name);
				err = mysql_query (conn, consC);
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
					sprintf(nota,"No hay datos");
				}
				resultado = mysql_store_result (conn);
				row = mysql_fetch_row (resultado);
				if (row == NULL)
				{
					//printf ("No se han obtenido datos en la consulta\n");
					// miramos las "F"
					char consF[500];
					sprintf(consF, "SELECT (partida.nota) FROM (jugador, partida, historial) WHERE historial.id_j= (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s') AND historial.id_p = partida.id AND partida.nota = 'F';", name);
					err = mysql_query (conn, consF);
					if (err!=0) {
						printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
						sprintf(nota,"No hay datos");
					}
					resultado = mysql_store_result (conn);
					row = mysql_fetch_row (resultado);
					if (row == NULL)
					{				
						sprintf (nota, "No hay datos");	
					}
					else	
					{
						sprintf (nota, "F");
					}
					
				}
				else	
				{
					sprintf (nota, "C");
				}
				
			}
			else	
			{
				sprintf (nota, "B");
			}			
		}
		else	
		{
			sprintf (nota, "A");
		}
	}
	else	
	{
		sprintf (nota, "S");
	}
}
// Rellena en respuesta la cantidad de partidas que el usuario ha jugado en el mapa que escoja.
// Le entra como parametro el nombre y mapa.
void Consulta2Buena(char resp[500], char map[30], char nombre[20])
{
	char cons[500];
	char name[20];
	strcpy (name, nombre);
	
	// Ahora vamos a realizar la consulta
	sprintf (cons,"SELECT COUNT(historial.id_p) FROM (jugador, partida,historial) WHERE jugador.username ='%s' AND jugador.id = historial.id_j AND historial.id_p = partida.id AND partida.mapa = '%s';", name, map);
	
	err=mysql_query (conn, cons);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	// Recogemos el resultado
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
	{
		printf ("No se han obtenido datos en la consulta\n");
		sprintf(resp,"No hay datos");
	}
	else
	{
		sprintf (resp, "%d", atoi(row[0]));	
	}
	
}


// Rellena en respuesta el tiempo maximo que ha estado el jugador en sus partidas
// Necesita el nombre como parametro.
void Consulta3Buena(char nombre[20], char resp[500])
{
	char cons[500];
	char cons2[500];
	char name [20];
	
	//quiere saber la puntuacion maxima de usuario
	strcpy (name, nombre);
	
	// consulta SQL para obtener una tabla con todos los datos
	
	// primero hacemos una consulta para saber si hay datos de este jugador y sus partidas
	// Ahora vamos a realizar la consulta
	sprintf (cons2,"SELECT COUNT(historial.id_p) FROM (jugador, historial) WHERE jugador.username ='%s' AND jugador.id = historial.id_j;", name);
	
	err=mysql_query (conn, cons2);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	// Recogemos el resultado
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
	{
		sprintf(resp,"No hay datos");
	}
	else
	{
		if (atoi(row[0]) == 0)
		{
			sprintf(resp,"No hay datos");
		}
		else
		{
			//printf("Res Antes de Consulta: %s\n", resp);
			sprintf(cons, "SELECT MAX(partida.duracion) FROM (jugador, partida, historial) WHERE historial.id_j= (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s') AND historial.id_p = partida.id;", name);
			err = mysql_query (conn, cons);
			//printf("Res Despu￩s de Consulta: %s\n", resp);
			//sprintf(resp,"No hay datos");
			/*	printf("Res Despu￩s de Consulta y de poner cosas: %s\n", resp);*/
			if (err!=0) {
				printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
				exit (1);
			}
			// Recogemos el resultado
			resultado = mysql_store_result (conn); 
			row = mysql_fetch_row (resultado);
			
			if (row == NULL)
			{
				//printf ("No se han obtenido datos en la consulta\n");
				sprintf(resp,"No hay datos");
			}
			else
			{
				//printf ("Si se han obtenido datos en la consulta: atoi(row[0])=%d \n", atoi(row[0]));
				sprintf (resp, "%d", atoi(row[0]));	
			}
		}
	}
	
}

// Esta funcion, sirve para eliminar un usuario de la base de datos.
// Necesita el nombre del usuario a eliminar
// Rellena en respuesta "Usuario Eliminado" o "Error al eliminar"
void EliminarUsuarioBD (char resp[500], char nombre[20])
{
	char cons[500];
	char name[20];
	strcpy (name, nombre);
	
	// Ahora vamos a realizar la consulta
	sprintf (cons,"DELETE FROM jugador WHERE jugador.username = '%s';", name);
	
	err=mysql_query (conn, cons);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	sprintf (resp, "Usuario eliminado");	
	
}

// Esta funcion sirve para enviar a todos los conectados la lista de conectados
// Rellena la respuesta con la lista de conectados
void EnviarListaConectadosNotificacion(char respuesta[512])
{
	char res[512];
	// Dame lista conectados
	pthread_mutex_lock (&mutex);
	DameNombreConectados(&listaconectados, res);
	pthread_mutex_unlock (&mutex);
	
	// Notificar a todos los conectados
	sprintf(respuesta, "6/%s", res);		
	// enviar notificacion por todos los sockets de los conectados
	int j;
	for (j=0; j<i; j++)
		write (listaconectados.conectados[j].socket, respuesta, strlen(respuesta));
}

// Esta funcion crea una partida y la añade en la base de datos. Tambien la relaciona con el historial de jugadores
// Retorna 0 si todo OK o -1 si no se hace correctamente.
int PonPartidaYHistorialEnBBDD(int idpartida, char mapa[30], char result[30], char nota[5], int tiempo)
{
	char cons[500];
	//Hacemos la consulta del insert en las PARTIDAS
	
	//COnsultamos el ID MAX
	strcpy(cons, "SELECT MAX(partida.id) FROM (partida);");
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
	sprintf(cons, "INSERT INTO partida VALUES (%s, '%s', '%s', '%s', %d);", id_max_s, mapa, result, nota, tiempo);
	//printf("%s\n",cons);
	
	err = mysql_query (conn, cons);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	
	

	//Hacemos la consulta del insert en el Historial
	for (int i=0; i< listaPartidas[idpartida].numjugadores; i++)
	{
		char cons2[500];
		char nombre_jugador[60];
		strcpy(nombre_jugador,  listaPartidas[idpartida].jugadores[i].nombre);
		
		//COnsultamos el ID del jugador de la partida
		sprintf(cons2, "SELECT (jugador.id) FROM (jugador) WHERE jugador.username = '%s';", nombre_jugador);
		//printf("%s\n",cons2);
		
		err = mysql_query (conn, cons2);
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
			exit (1);
		}
		
		resultado = mysql_store_result (conn); 
		row = mysql_fetch_row (resultado);
		
		int id_esteJug;
		id_esteJug = atoi(row[0]);
		int id_partida_ahora = id_max;
		// Hacemos insert
		sprintf(cons2, "INSERT INTO historial VALUES (%d, %d, %d);", id_esteJug, id_partida_ahora, listaPartidas[idpartida].jugadores[i].posicion);
		//printf("%s\n",cons2);
		
		err = mysql_query (conn, cons2);
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
			return -1;
		}
	}
	return 0;
	
}
// Esta funcion rellena el vector respuesta el numero de partidas con el personaje preferido del jugador
// Necesita como parametros la respuesta y el nombre. Retorna el numero de personaje favorito
int DamePersonajeFavoritoYCantidadPartidas (char resp[500], char nombre[20])
{
	char cons[500];
	char cons1[500];
	char cons2[500];
	char cons3[500];
	char cons4[500];
	
	char name [20];
	
	int posicion_preferida = 0;
	int pos1 = 0;
	int pos2 = 0;
	int pos3 = 0;
	int pos4 = 0;
	
	//quiere saber la puntuacion maxima de usuario
	strcpy (name, nombre);
	
	// consulta SQL para obtener una tabla con todos los datos
	
	// primero hacemos una consulta para saber si hay datos de este jugador y sus partidas
	sprintf (cons,"SELECT COUNT(historial.id_p) FROM (jugador, historial) WHERE jugador.username ='%s' AND jugador.id = historial.id_j;", name);
	
	err=mysql_query (conn, cons);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	// Recogemos el resultado
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
	{
		sprintf(resp,"No hay datos");
	}
	else
	{
		if (atoi(row[0]) == 0)
		{
			sprintf(resp,"No hay datos");
			return -1;
		}
		else
		{
			// Ahora vamos a realizar la consulta
			sprintf(cons1,"SELECT Count(Distinct(historial.id_p)) FROM (jugador, historial) WHERE historial.posicion = 1 AND historial.id_j = (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s');", name); 
			sprintf(cons2,"SELECT Count(Distinct(historial.id_p)) FROM (jugador, historial) WHERE historial.posicion = 2 AND historial.id_j = (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s');", name); 
			sprintf(cons3,"SELECT Count(Distinct(historial.id_p)) FROM (jugador, historial) WHERE historial.posicion = 3 AND historial.id_j = (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s');", name); 
			sprintf(cons4,"SELECT Count(Distinct(historial.id_p)) FROM (jugador, historial) WHERE historial.posicion = 4 AND historial.id_j = (SELECT jugador.id FROM (jugador) WHERE jugador.username = '%s');", name); 
			
			// posicion 1
			err=mysql_query (conn, cons1);
			if (err!=0) {
				printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
				exit (1);
			}
			// Recogemos el resultado
			resultado = mysql_store_result (conn); 
			row = mysql_fetch_row (resultado);
			if (row == NULL)
				sprintf(resp,"No hay datos");
			else
				pos1 = atoi(row[0]);
			
			// posicion 2
			err=mysql_query (conn, cons2);
			if (err!=0) {
				printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
				exit (1);
			}
			// Recogemos el resultado
			resultado = mysql_store_result (conn); 
			row = mysql_fetch_row (resultado);
			if (row == NULL)
				sprintf(resp,"No hay datos");
			else
				pos2 = atoi(row[0]);
			
			// posicion 3
			err=mysql_query (conn, cons3);
			if (err!=0) {
				printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
				exit (1);
			}
			// Recogemos el resultado
			resultado = mysql_store_result (conn); 
			row = mysql_fetch_row (resultado);
			if (row == NULL)
				sprintf(resp,"No hay datos");
			else
				pos3 = atoi(row[0]);
			
			// posicion 4
			err=mysql_query (conn, cons4);
			if (err!=0) {
				printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
				exit (1);
			}
			// Recogemos el resultado
			resultado = mysql_store_result (conn); 
			row = mysql_fetch_row (resultado);
			if (row == NULL)
				sprintf(resp,"No hay datos");
			else
				pos4 = atoi(row[0]);
			
			// miramos cual es el mayor
			if (pos1 >= pos2)
			{
				if (pos1 >= pos3)
				{
					if(pos1>=pos4)
					{ posicion_preferida = 1 ;
						sprintf (resp, "%d",pos1);
					}
					else
					   { posicion_preferida = 4;
					   sprintf (resp, "%d",pos4);
					}
				}
				else
				{
					if (pos2 >= pos3)
					{
						if(pos2>=pos4)
						{ posicion_preferida = 2 ;
						sprintf (resp, "%d",pos2);
						}
						else
						{ posicion_preferida = 4;
						sprintf (resp, "%d",pos4);
						}
					}
					else
					{
						if(pos3>=pos4)
						{ posicion_preferida = 3 ; 
						sprintf (resp, "%d",pos3);}
						else
						{ posicion_preferida = 4;
						sprintf (resp, "%d",pos4);}
					}
				}
			}
			else
			{
				if (pos2 >= pos3)
				{
					if(pos2>=pos4)
					{ posicion_preferida = 2 ; 
					sprintf (resp, "%d",pos2);}
					else
					{ posicion_preferida = 4;
					sprintf (resp, "%d",pos4);}
				}
				else
				{
					if(pos3>=pos4)
					{ posicion_preferida = 3 ;
					sprintf (resp, "%d",pos3);}
					else
					{ posicion_preferida = 4;
					sprintf (resp, "%d",pos4);}
				}
			}
			printf ("Posiciones y partidas con: 1 - %d, 2 - %d, 3 - %d, 4 - %d. --> FAVORITO: %d\n", pos1, pos2, pos3, pos4, posicion_preferida);
			return posicion_preferida;
		}
	}
}


void *AtenderCliente (void *socket)
{
	int sock_conn, ret;
	s= (int *) socket;
	sock_conn = *s;
	char peticion[512];
	char login[512];
	char respuesta[512];
	char notificacion[512];
	char res[512];
	
	int terminar =0;
	// Entramos en un bucle para atender todas las peticiones de este cliente
	//hasta que se desconecte
	
	while (terminar == 0)
	{
		// Ahora recibimos la petici?n
		ret = read(sock_conn,peticion, sizeof(peticion));
		printf ("Recibido\n");
		
		// Tenemos que a?adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		peticion[ret]='\0';
		printf ("Peticion: %s\n",peticion);
		
		// vamos a ver que quieren --> que peticion quieren
		char *p = strtok( peticion, "/");
		int codigo =  atoi (p);
		int numForm;
		// Ya tenemos el c?digo de la peticion
		// otras variables
		int max_jugador;
		char consulta [500];
		char nombre[60]; // Para consultas 1 i 2
		char mapa[60]; // Para consulta 3
		char peticion[500];
		char username[60];
		char usernameFijo[60];
		char password[60];
		
		char invitado[60];
		
		strcpy(respuesta, ""); // vaciamos respuesta
		
		if (codigo == 0) //peticion de desconexion
		{
			pthread_mutex_lock (&mutex);
			EliminaConectado(&listaconectados, username);
			pthread_mutex_unlock (&mutex);
			
			//pthread_mutex_lock (&mutex);			
			EnviarListaConectadosNotificacion(notificacion);
			//pthread_mutex_unlock (&mutex);
			
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			
			for (int i = 0; i < listaPartidas[idpartida].numjugadores; i++)
			{
				sprintf(notificacion, "10/%d", idpartida);
				write (listaPartidas[idpartida].jugadores[i].socket, notificacion, strlen(notificacion));
				printf("%s\n",notificacion);
			}
			
			pthread_mutex_lock (&mutex);
			AcabaPartida(listaPartidas,idpartida);
			pthread_mutex_unlock (&mutex);
			
			terminar = 1;
		}
		
		else if (codigo == 1)
		{//piden logearse
			// Obtenemos usuario
			p = strtok(NULL, "/");
			strcpy(username, p);
			strcpy(usernameFijo, p);
			//Obtenemos contrasenya
			p = strtok(NULL, "/");
			strcpy(password, p);
			int resultado = LogIn(username, password);
			if (resultado==-1)
				sprintf(respuesta, "1/Usuario no encontrado");
			else if (resultado  ==1)
				sprintf(respuesta, "1/Password incorrecto");
			else
			{
				sprintf(respuesta, "1/Conectado");
				//pthread_mutex_lock (&mutex);			
				EnviarListaConectadosNotificacion(notificacion);
				//pthread_mutex_unlock (&mutex);				
			}
		}
		
		else if (codigo ==2) // quieren crear un usuario
		{
			// Obtenemos usuario
			p = strtok(NULL, "/");
			strcpy(username, p);
			//Obtenemos contrasenya
			p = strtok(NULL, "/");
			strcpy(password, p);
			
			int resultado = CrearUsuario(username, password);
			
			if (resultado==-1)
				sprintf(respuesta, "2/Usuario no se ha podido crear");
			else if (resultado ==1)
				sprintf(respuesta, "2/Usuario ya existente");
			else
				sprintf(respuesta, "2/Usuario creado correctamente");
		}
		
		else if (codigo == 3)
		{
			/*int max = Consulta1(username);
			sprintf(respuesta, "3/%d", max);*/
			
			char nota[20];
			Consulta1Buena(username, nota);
			sprintf(respuesta, "3/%s", nota);
		}
		
		else if (codigo == 4) //4/mapa
		{
/*			strcpy(res, "");*/
/*			Consulta2(res);*/
/*			sprintf(respuesta, "4/%s", res);*/
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			
			strcpy(res, "");
			Consulta2Buena(res, map,username);
			sprintf(respuesta, "4/%s", res);
			//printf(respuesta);
		}
		else if (codigo == 5)	
		{
/*			strcpy(res, "");*/
/*			Consulta3(res);*/
/*			sprintf(respuesta, "5/%s", res);*/
			
			strcpy(res, "No hay datos");
			//printf("Res Antes: %s\n", res);
			Consulta3Buena(username,res);
			//printf("Res Despu￩s: %s\n", res);
			if (strcmp(res, "")==0)
				sprintf (res, "No hay datos");
			sprintf(respuesta, "5/%s", res);
		}
		else if (codigo == 7)
		{
			// --> 7/3/guest1/guest2/guest3	
			//Obtenemos contrasenya
			p = strtok(NULL, "/");
			int n_invitaciones =atoi(p);
			
			int partidalibre = BuscarPartidaLibre(listaPartidas);
			
			//metemos al invitador
			pthread_mutex_lock (&mutex);
			PonInvitacionesYAnfitrionEnPartida(listaPartidas, partidalibre, username, n_invitaciones);
			pthread_mutex_unlock (&mutex);
			
			// metemos al invitado
			p = strtok(NULL, "/");
			while (p != NULL)
			{
				strcpy(invitado, p);
				int sinvitado = DameSocket(&listaconectados, invitado);
				if (sinvitado != -1)
				{
					pthread_mutex_lock (&mutex);
					PonJugadorPartida(listaPartidas, partidalibre, invitado);
					pthread_mutex_unlock (&mutex);
					sprintf(notificacion, "7/%s/%d", username, partidalibre);
					write (sinvitado, notificacion, strlen(notificacion));
				}
				p = strtok(NULL,"/");
			}
			printf("Notificacion: %s\n", notificacion);
		}
		
		else if (codigo == 8)
		{
			char decision[10];
			int idpartida;
			
			p = strtok(NULL, "/");
			strcpy(decision, p);
			p = strtok(NULL,"/");
			idpartida = atoi(p);
			p = strtok(NULL,"/");
			strcpy(invitado, p);
			
			if (strcmp(decision,"Si")==0)
			{
				pthread_mutex_lock (&mutex);			
				int n_invitaciones_faltantes =  AceptaInvitacionYDameFaltantes(listaPartidas, idpartida);
				pthread_mutex_unlock (&mutex);
				printf("Para la partida [%d] ahora faltan %d invitaciones para aceptar\n", idpartida, n_invitaciones_faltantes);
				// Enviamos al invitador que el invitado ha aceptado la invitacion
				sprintf(notificacion, "8/%s/%s/%d", invitado, decision, idpartida);				
				write (listaPartidas[idpartida].jugadores[0].socket, notificacion, strlen(notificacion));
				
				
				if (n_invitaciones_faltantes <= 0)	// empieza la partida para todos
				{
					printf ("Ya no faltan invitaciones. Partida n%d tiene a los jugadores en este orden: %s - %d, %s- %d, %s- %d, %s- %d --> %d\n", idpartida, listaPartidas[idpartida].jugadores[0].nombre,listaPartidas[idpartida].jugadores[0].socket, listaPartidas[idpartida].jugadores[1].nombre, listaPartidas[idpartida].jugadores[1].socket,listaPartidas[idpartida].jugadores[2].nombre,listaPartidas[idpartida].jugadores[2].socket, listaPartidas[idpartida].jugadores[3].nombre,listaPartidas[idpartida].jugadores[3].socket, listaPartidas[idpartida].numjugadores);					
					sprintf(notificacion, "9/%d/%s/%d", idpartida, listaPartidas[idpartida].jugadores[0].nombre,  listaPartidas[idpartida].numjugadores) ; 
					printf("Notificacion: %s\n", notificacion);	
					for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
					{
						//if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
						write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
					}
					printf("Notificacion: %s\n", notificacion);
				}
			}
			else
			{	// No ha aceptado la invitacion
				sprintf(notificacion, "8/%s/%s/%d", invitado, decision, idpartida);
				pthread_mutex_lock (&mutex);
				AcabaPartida (listaPartidas, idpartida);	
				pthread_mutex_unlock (&mutex);
				
				// El invitador
				write (listaPartidas[idpartida].jugadores[0].socket, notificacion, strlen(notificacion));
				printf("Notificacion: %s\n", notificacion);
			}
		}
		
	
		else if (codigo == 10) // seleccion de personaje --> 10/idpartida/personaje
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
/*			p = strtok (NULL, "/");*/
/*			strcpy (username, p);*/
			
			sprintf(notificacion, "10/%d/%d/%s", idpartida, jugadorEscogido, username);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		
		else if (codigo == 11) // deseleccion de personaje
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
			sprintf(notificacion, "11/%d/%d/%s", idpartida, jugadorEscogido, username);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		else if (codigo == 12)
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			
			p = strtok(NULL, "/");
			char mapaEsc[20];
			strcpy(mapaEsc, p);
			
			sprintf(notificacion, "12/%d/%s", idpartida, mapaEsc);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		else if (codigo == 14)
		{
			// 14/idpartida/nombreJ1/nombreJ2/nombreJ3/nombreJ4
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char J[20];
			
			// Ponemos las posiciones de los jugadores
			for (int x=0; x<4; x++)
			{
				p = strtok (NULL, "/");
				strcpy(J, p);
				pthread_mutex_lock (&mutex);
				PonPosicionJugadorPartida(listaPartidas, idpartida, J, x+1);
				pthread_mutex_unlock (&mutex);
			}
			
			//printf ("Esta partida tiene a los jugadores: %s, %s, %s, %s\n", listaPartidas[idpartida].jugadores[0].nombre, listaPartidas[idpartida].jugadores[1].nombre,listaPartidas[idpartida].jugadores[2].nombre, listaPartidas[idpartida].jugadores[3].nombre);
			//printf ("en esta posicion: %d,%d,%d,%d\n", listaPartidas[idpartida].jugadores[0].posicion,  listaPartidas[idpartida].jugadores[1].posicion,  listaPartidas[idpartida].jugadores[2].posicion, listaPartidas[idpartida].jugadores[3].posicion);
			
			sprintf(notificacion, "14/%d", idpartida);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		// - - - - - - - - - - INICIO:  movimientos de los personajes de otros - - - - - - -- - - - -- 
		else if (codigo == 15) 
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
			sprintf(notificacion, "15/%d/%s/%d", idpartida, map, jugadorEscogido);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		else if (codigo == 16) // deseleccion de personaje
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
			sprintf(notificacion, "16/%d/%s/%d", idpartida, map, jugadorEscogido);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		else if (codigo == 17) // deseleccion de personaje
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
			sprintf(notificacion, "17/%d/%s/%d", idpartida, map, jugadorEscogido);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		else if (codigo == 18) // deseleccion de personaje
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
			sprintf(notificacion, "18/%d/%s/%d", idpartida, map, jugadorEscogido);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		else if (codigo == 19) // deseleccion de personaje
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
			sprintf(notificacion, "19/%d/%s/%d", idpartida, map, jugadorEscogido);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		else if (codigo == 21) // deseleccion de personaje
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
			sprintf(notificacion, "21/%d/%s/%d", idpartida, map, jugadorEscogido);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		else if (codigo == 22) // deseleccion de personaje
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
			sprintf(notificacion, "22/%d/%s/%d", idpartida, map, jugadorEscogido);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		else if (codigo == 23) // deseleccion de personaje
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			p = strtok(NULL, "/");
			int jugadorEscogido = atoi(p);
			
			sprintf(notificacion, "23/%d/%s/%d", idpartida, map, jugadorEscogido);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != sock_conn)
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		// - - - - - - - - - - FIN:  movimientos de los personajes de otros - - - - - - -- - - - -- 
		else if (codigo == 25) // click boton Eliminar usuario
		{ 					// 9/ --> el username ja el tenim
			
			strcpy(res, "");
			EliminarUsuarioBD(res,username);
			sprintf(respuesta, "25/%s", res);
			
		}
		else if (codigo == 26) // peticion de consulta de personaje favorito
		{
			strcpy(res, "");
			int favorito = DamePersonajeFavoritoYCantidadPartidas(res, username);
			
			sprintf(respuesta, "26/%d/%s", favorito,res);
			
		}
		else if (codigo == 50)	// fin de partida 
		{	// "50/" + idPartida + "/" + mapa + "/" + result_partida + "/" + letra_resultado + "/" + tiempo
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			char resultado_frase[30];
			char letra_result[5];
			p = strtok (NULL, "/");
			strcpy (resultado_frase, p);
			p= strtok (NULL, "/"); 
			strcpy(letra_result, p);
			p= strtok(NULL, "/");
			int tiempo= atoi(p);
			
			// ponemos la partida en la BASE DE DATOS
			int res_bd = PonPartidaYHistorialEnBBDD(idpartida, map, resultado_frase, letra_result, tiempo);
			if (res_bd == 0) // BBDD ok
			{
				sprintf(notificacion, "50/%d/%s/%s/%s", idpartida,map, resultado_frase, letra_result);
				
				for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
				{
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
				}
				printf("Notificacion: %s\n", notificacion);
				
				// acabamos la partida
				pthread_mutex_lock (&mutex);
				AcabaPartida (listaPartidas, idpartida);	
				pthread_mutex_unlock (&mutex);
			}
		}
		else if (codigo == 51) // 51 /idpartida/ mapa
		{
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			char map[30];
			p = strtok(NULL, "/");
			strcpy(map, p);
			
			sprintf(notificacion, "51/%d/%s", idpartida, map);
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
		}
		
		else //if (codigo == 20)
		{	//Mensaje del chat de seleccion partida
			// "20/nForm/form/idpartida/HolaQueTal"
			char mensaje[200];
			
			p = strtok(NULL, "/");
			int idpartida = atoi(p);
			
			p = strtok(NULL, "/");
			strcpy(mensaje, p);
			
			
			//sprintf(notificacion, "20/%s/%s/%d", nombrechat, mensaje, idpartida);
			sprintf(notificacion, "20/%d/%s/%s", idpartida, username, mensaje);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				// se lo enviamos a todos
				write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
			printf("Notificacion: %s\n", notificacion);
			
		}
		
		printf ("- - - - - - - - - [%s] (fijo) tiene este nombre %s - - - - - - - - \n", usernameFijo, username);
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

int main(int argc, char *argv[])
{
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
	// SHIVA --> 50050 o 50051 o 50052
	char host[50];
	int puerto = DamePuertoYHost(shiva, host);
	serv_adr.sin_port = htons(puerto);
	
	setsockopt(sock_listen, SOL_SOCKET, SO_REUSEADDR, &(int){1}, sizeof(int));
	
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind\n");
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen\n");
	
	pthread_t thread; //donde se guardan los ID de los sockets
	
	
	// Creamos conexion 1 sola vez con BaseDatos MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//inicializar la conexion
	conn = mysql_real_connect (conn, host,"root", "mysql", "T1bdFireWater",0, NULL, 0);
	
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	// Bucle infinito
	for (;;){
		printf ("Escuchando\n");
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		sockets[i] = sock_conn;
		// Creamos thread y le decimos lo que tiene que hacer
		// resoldre sockets[i] --> clientes muertos 
		pthread_create (&thread, NULL, AtenderCliente, &sockets[i]);
		printf("Este usuario utiliza %d socket",sock_conn);
		i=i+1;
		printf("[%d]", i);
	}
}
