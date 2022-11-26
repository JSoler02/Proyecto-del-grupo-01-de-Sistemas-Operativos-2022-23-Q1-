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

// Estructura de partida. Tiene como maximo 4 jugadores
// campo ocupado = 0: si libre; ocupado=1: si ocupado
typedef struct {
	Conectado jugadores[4];
	int numjugadores;
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
//Esta funcion devuelve el puerto y el Host 
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

int PonConectado(ListaConectados *lista, char nombre[20], int socket){
	// Pone nuevo conectado.retorna 0 si ha ido bien y -1 si la lista ya 
	// estaba llena y no lo ha podido poner
	if (lista->num == 100)
		return -1;
	else{
		strcpy(lista->conectados[lista->num].nombre,nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num = lista->num +1;
		return 0;
	}
}

int DameSocket (ListaConectados *lista, char nombre[20])
{ //Devuelve el socket o -1 si no lo encuentra
	int i = 0;
	int encontrado = 0;
	//busqueda
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
	//bÃ¯Â¿Â¯Ã¯Â¾Â¿Ã¯Â¾Âºsqueda
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
{ // Devuelve 0 si elimina y -1 si el usuario no estÃ¯Â¿Â¯Ã¯Â¾Â¿Ã¯Â¾Â¡ en la lista
	// Elimina el socket de la lista global de sockets
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

void DameNombreConectados (ListaConectados *lista, char respuesta[512])
{ //Devuelve los nombres de los jugadores conectados separados por "/"
	//Primero pone el numero de conectado. "3/Juan/Pedro/Maria"
	sprintf (respuesta, "%d", lista->num);
	for (int i = 0; i<lista->num; i++)
	{
		sprintf (respuesta, "%s/%s", respuesta, lista->conectados[i].nombre);
	}
}

void DameSocketsDeConectados (ListaConectados *lista, char conectados[512], char sockets[200])
{ //Recibe una lista con nombres de jugadores separados por "/";: "3/Juan/Pedro/Maria";
	//Devuelve una lista con los sockets de estos jugadores separados por "/":"3/5/1/3";
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

// Devuelve la id de la partida (busca el primero libre)
// Devuelve -1 si no encuentra ninguna libre
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


// Elimina la partida de la lista: pone a 0 el tÃ¯Â¿Â©rmino ocupado 
// de la partida que le venga como parametro. Eliminamos (ponemos a -1)
// tambiÃ¯Â¿Â©n los sockets de la lista de partidas
void AcabaPartida (Partida lista[20], int idpartida)	
{
	lista[idpartida].numjugadores = 0;
	
	for (int i=0;i < 4;i++)
	{
		lista[idpartida].jugadores[i].socket = -1;
		strcpy(lista[idpartida].jugadores[i].nombre, "");
	}
}

//Pon jugador en partida en la posicion que se especifica
//en el mensaje.
void PonJugadorPartida(Partida lista[20], int idpartida, char jugador[20])
{
	printf ("Partida n%d tiene a los jugadores en este orden: %s, %s, %s, %s --> %d\n", idpartida, listaPartidas[idpartida].jugadores[0].nombre, listaPartidas[idpartida].jugadores[1].nombre, listaPartidas[idpartida].jugadores[2].nombre, listaPartidas[idpartida].jugadores[3].nombre, listaPartidas[idpartida].numjugadores);
	if (lista[idpartida].ocupado == 0)
	{
		AcabaPartida(lista, idpartida);
		lista[idpartida].ocupado = 1;
	}
	
	int s1 = DameSocket(&listaconectados,jugador);
	lista[idpartida].jugadores[lista[idpartida].numjugadores].socket = s1;
	strcpy(lista[idpartida].jugadores[lista[idpartida].numjugadores].nombre, jugador);
	lista[idpartida].numjugadores = lista[idpartida].numjugadores +1;
	
}
int BuscarIDPartida (Partida lista[20], char jugador[20])
{
	int idP = 0;
	for (int i = 0; i < 20; i++)
	{
		for (int j = 0; j < 4; j++)
			if (strcmp(lista[i].jugadores[j].nombre, jugador)==0)
				idP = i;
	}
	return idP;
}

// Jugada en la partida
void MovimientoPartida(Partida lista[20], int idpartida, char move[20])
{
	for (int i=0; i < lista[idpartida].numjugadores; i++)
	{
		
	}	
}


// Esta funcion hace el LogIn. Necesita un nombre y una contrasenya
// Devuelve 0 si todo OK. Devuelve -1 si no es correcto. Devuelve 1 si contrasenya incorrecta
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

// Esta funcion crea un Usuario. Necesita un nombre y una contrasenya
// Devuelve 0 si todo OK. Devuelve -1 si no se hace correctamente. Devuelve 1 si usuario existente
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

// Nos dice la puntuacion maxima que ha conseguido Maria en sus partidas
int Consulta1()
{
	char cons[500];
	char name [20];
	int max;
	//quiere saber la puntuacion maxima de Maria
	strcpy (name, "'Maria'");
	
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
	
	return max;
}

// ID de las Partidas en las que Juan ha jugado mas de 120 s
void Consulta2(char resp[500])
{
	char cons[500];
	char name[20];
	strcpy (name, "'Juan'");
	
	// Ahora vamos a realizar la consulta
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
			
			sprintf(resp, "%d " ,atoi(row[0]));
			row = mysql_fetch_row (resultado);
			/*sprintf (respuesta,"%s/%s",respuesta, row[0]);*/
			
		}
	}
	
}

// Rellena el vector resp con los nombre de los jugadores que han jugado 
// como J1 en el mapa "templo"
// Devuelve 0 si OK. Devuelve -1 si va mal
int Consulta3(char resp[500])
{
	char map [60];
	char cons[512];
	//Jugadores que han jugado en templo como J1
	strcpy(map, "'templo'");
	// Ahora vamos a realizar la consulta
	
	strcpy (cons,"SELECT jugador.username FROM (jugador, partida, historial) WHERE partida.mapa = "); 
	strcat (cons, map);
	strcat (cons," AND historial.id_p = partida.id AND historial.id_j = jugador.id AND historial.posicion = 1;");
	// hacemos la consulta 
	err=mysql_query (conn, cons); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	if (row == NULL)
		printf ("No se han obtenido datos en la consulta\n");
	else
	{
		while (row != NULL)
		{
			printf("Username: %s\n", row[0]);
			sprintf (resp, "%s %s", resp, row[0]);
			// obtenemos la siguiente fila
			row = mysql_fetch_row (resultado);
		}
		return 0;
	}
}

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
		
		write (sockets[j], respuesta, strlen(respuesta));
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
		char *p = strtok(peticion, "/");
		int codigo =  atoi (p);
		// Ya tenemos el c?digo de la peticion
		// otras variables
		int max_jugador;
		char consulta [500];
		char nombre[60]; // Para consultas 1 i 2
		char mapa[60]; // Para consulta 3
		char peticion[500];
		char username[60];
		char password[60];
		char invitado[60];
		
		
		
		strcpy(respuesta, ""); // vaciamos respuesta
		
		if (codigo ==0) //peticion de desconexion
		{
			pthread_mutex_lock (&mutex);
			EliminaConectado(&listaconectados, username);
			pthread_mutex_unlock (&mutex);
			
			//pthread_mutex_lock (&mutex);			
			EnviarListaConectadosNotificacion(notificacion);
			//pthread_mutex_unlock (&mutex);
			
			terminar = 1;
		}
		
		else if (codigo ==1)
		{//piden logearse
			// Obtenemos usuario
			p = strtok(NULL, "/");
			strcpy(username, p);
			//Obtenemos contrasenya
			p = strtok(NULL, "/");
			strcpy(password, p);
			int resultado = LogIn(username, password);
			if (resultado==-1)
				strcpy(respuesta, "1/Usuario no encontrado");
			else if (resultado  ==1)
				strcpy(respuesta, "1/Password incorrecto");
			else
			{
				strcpy(respuesta, "1/Conectado");
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
				strcpy(respuesta, "2/Usuario no se ha podido crear");
			else if (resultado ==1)
				strcpy(respuesta, "2/Usuario ya existente");
			else
				strcpy(respuesta, "2/Usuario creado correctamente");
		}
		
		else if (codigo == 3)
		{
			int max = Consulta1();
			sprintf (respuesta, "3/%d", max);
		}
		
		else if (codigo == 4)
		{
			strcpy(res, "");
			Consulta2(res);
			sprintf(respuesta, "4/%s", res);
		}
		else if (codigo == 5)	
		{
			strcpy(res, "");
			Consulta3(res);
			sprintf(respuesta, "5/%s", res);
		}
		/*else if (codgo == 6)
		{
		
		}*/
		else if (codigo == 7)
		{ 	// --> 7/guest1/guest2/guest3			
			int partidalibre = BuscarPartidaLibre(listaPartidas);
			
			
			pthread_mutex_lock (&mutex);
			PonJugadorPartida(listaPartidas, partidalibre, username);
			pthread_mutex_unlock (&mutex);
			
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
					sprintf(notificacion, "7/%s/%d", invitado, partidalibre);
					write (sinvitado, notificacion, strlen(notificacion));
				}
				p = strtok(NULL,"/");
			}
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
				PonJugadorPartida(listaPartidas, idpartida, invitado);
				pthread_mutex_unlock (&mutex);
				sprintf(notificacion, "8/%s/%s/%d", invitado, decision, idpartida);
			}
			else
			{
				sprintf(notificacion, "8/%s/%s/%d", invitado, decision, idpartida);
				
			}
			write (listaPartidas[idpartida].jugadores[0].socket, notificacion, strlen(notificacion));
		}
		else // if (codigo == 9)
		{
			/*char movement[20];
			int idpartida;
			
			p = strtok(NULL, "/");
			strcpy(movement, p);
			p = strtok(NULL,"/");
			idpartida = atoi(p);
			
			MovimientoPartida(listaPartidas, idpartida, movement);
			*/			
			p = strtok(NULL, "/");
			int x = atoi(p);
			p = strtok(NULL, "/");
			int y = atoi(p);			
			p = strtok(NULL, "/");
			int idpartida = atoi(p);		
			
			sprintf(notificacion, "9/%d/%d/%d", x, y, idpartida);
			
			for (int j = 0; j<listaPartidas[idpartida].numjugadores; j++)
			{
				if (listaPartidas[idpartida].jugadores[j].socket != s)	// no soy yo
					write (listaPartidas[idpartida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
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
	// NO SHIVA --> conn = mysql_real_connect (conn, "localhost","root", "mysql", "T1bdFireWater",0, NULL, 0);
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
		i=i+1;
	}
}
