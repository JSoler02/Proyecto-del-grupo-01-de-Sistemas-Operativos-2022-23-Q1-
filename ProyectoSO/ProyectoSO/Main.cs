using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets; // libreria de sockets
using System.Threading; // libreria de threads


namespace ProyectoSO
{
    public partial class Main : Form
    {
        Socket server; // declaramos socket
        Thread atender; // declaramos thread

        // Variables de desarrollo
        int shiva = 0;  // 1: si Shiva; 0: si Maquina Virtual
        int julia = 0;  // 1: si IP de Julia en la Maquina Virtual; 0: si IP del resto en la Maquina virtual

        int idPartida;
        string nombre;

        int x_bicho;
        int y_bicho;

        string invitados;
        int NumInvitados = 0; // maximo puede ser 4
        int InvPartida = 0;
        bool But_empezarPartida_activado = false;
        bool soyanfitrion;

        bool conectado;

        // Lista generica de formularios (memoria) per saber a on enviare el missatge
        List<SeleccionPartida> formularios1 = new List<SeleccionPartida>();

        private void Main_Load(object sender, EventArgs e)
        {
            label3.Visible = false;
            lbl_lista_con.Visible = false;
            PuntMax_But.Visible = false;
            Juan120_But.Visible = false;
            Templo_But.Visible = false;
            desconnectButton.Visible = false;
            GridConectados.Visible = false;
            passwordBox.PasswordChar = ('*');
            panel1.Visible = true;
            GridConectados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            GridConectados.RowHeadersVisible = false;
            GridConectados.ColumnCount = 1;
            GridConectados.Columns[0].HeaderText = "Username";
            GridConectados.ReadOnly = true;
            CrearPartidaBut.Visible = false;


            conectado = false;
        }
        public Main()
        {
            InitializeComponent();
        }

        // Se conecta al servidor. Devuelve 0 si correcto o -1 si no puede.
        // Si se conecta correctamente crea el Thread para atender al servidor
        private int ConectarConServidor()
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            // SHIVA --> 147.83.117.22
            // JÚLIA --> 192.168.195.128 
            // RESTA --> 192.168.56.102
            string ip;
            // SHIVA --> 50050 o 50051 o 50052
            // MAQ VIRT --> 8050...
            int puerto;
            if (this.shiva == 1)
            {
                ip = "147.83.117.22";
                puerto = 50050;
            }
            else
            {
                puerto = 8080;
                if (this.julia == 1)
                { ip = "192.168.195.128"; }
                else
                { ip = "192.168.56.102"; }
            }

            IPAddress direc = IPAddress.Parse(ip);
            IPEndPoint ipep = new IPEndPoint(direc, puerto);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket

                // Inicializamos el thread que atendera los mensajes del servidor
                ThreadStart ts = delegate { AtenderServidor(); };
                atender = new Thread(ts);
                atender.Start();

                return 0;
            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return -1;
            }

        }

        // Creamos funcion del thread
        private void AtenderServidor()
        {
            // Bucle infinito
            while (true)
            {
                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                // Partimos el mensaje por la "/"
                string mensajeSucio = Encoding.ASCII.GetString(msg2);
                string mensajeLimpio = mensajeSucio.Split('\0')[0];
                string[] trozos = mensajeLimpio.Split('/');
                int codigo = Convert.ToInt32(trozos[0]);

                int numForm; // para saber a que formulario debo enviarle la respuesta
                string mensaje;

                switch (codigo)
                {
                    case 1: // Login
                        mensaje = trozos[1];
                        MessageBox.Show(mensaje);
                        if (mensaje == "Conectado")
                        {
                            Invoke(new Action(() =>
                            {
                                label3.Visible = true;
                                lbl_lista_con.Visible = false;
                                PuntMax_But.Visible = true;
                                Juan120_But.Visible = true;
                                Templo_But.Visible = true;
                                desconnectButton.Visible = true;
                                panel1.Visible = false;
                                GridConectados.Visible = true;
                                CrearPartidaBut.Visible = true;
                                this.BackColor = Color.Green;
                            }));
                        }
                        break;
                    case 2: // New User
                        mensaje = trozos[1];
                        MessageBox.Show(mensaje);
                        break;

                    case 3: //consulta 1 --> Puntos maximos de Maria
                        mensaje = trozos[1];
                        MessageBox.Show("La puntuación máxima es: " + mensaje);
                        break;

                    case 4: //consulta 2 --> Id de las partidas de más de 120 s de Juan
                        mensaje = trozos[1];
                        MessageBox.Show("Juan ha jugado más de 120 segundos en las partidas: " + mensaje);
                        break;

                    case 5: //consulta 3 --> Nombre de los jugadores que han jugado como J1 en "templo"
                        mensaje = trozos[1];
                        MessageBox.Show(mensaje + " han jugado partidas en el mapa 'templo' como jugador 1");
                        break;
                    case 6: // Notificación de la Lista de Conectados

                        // string mensaje = "3/Juan/Pedro/Maria"; 
                        // el numero inicial nos indica el número de usuarios conectados
                        mensaje = trozos[1];
                        int num = Convert.ToInt32(mensaje);
                        Invoke(new Action(() =>
                        {
                           GridConectados.Rows.Clear();
                           for (int i = 0; i < num; i++)
                           {
                               string nombre = Convert.ToString(trozos[i + 2].Split('\0')[0]);
                               GridConectados.Rows.Add(nombre);
                           }
                            //GridConectados.Refresh();
                            GridConectados.ClearSelection();
                        }));
                        break;
                    case 7: // Peticion de partida
                            // 7/Maria: A quien te está pidiendo partida
                        mensaje = trozos[1];
                        string anfitrion = mensaje;
                        string resp; // "Si o No"
                        DialogResult r = MessageBox.Show(anfitrion + " quiere jugar contigo.", "¿Aceptar?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (r == DialogResult.OK)
                        {
                            resp = "Si";
                        }
                        else
                        {
                            resp = "No";
                        }
                        // * * * * * Enviamos respuesta como si queremos aceptar o no 
                        // --> "8/Si/idpartida/Juan"
                        idPartida = Convert.ToInt32(trozos[2]);
                        mensaje = "8/" + resp + "/" + idPartida + "/" + nombre; 
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        break;
                    case 8: // Respuesta negativa a peticion de partida
                            // "8/Juan/Si/2": Quien ha aceptado/su respuesta/idPartida
                        mensaje = trozos[1];
                        string nombre_acepta = mensaje;
                        string respuesta = trozos[2];
                        if (respuesta == "Si")
                        {
                            MessageBox.Show(nombre_acepta + " ha aceptado tu invitación a partida");
                        }
                        else
                        {
                            MessageBox.Show(nombre_acepta + " no ha aceptado tu invitación a partida");
                        }

                        Invoke(new Action(() =>
                        {
                            GridConectados.DefaultCellStyle.BackColor = Color.White;
                        }));
                        break;

                    case 9: // empieza la partida para todo el mundo
                        // 9/idpartida/anfitrión
                        idPartida = Convert.ToInt32(trozos[1]);
                        if (trozos[2]==nombre)
                        { soyanfitrion = true; }
                        else
                        { soyanfitrion = false; }
                        MessageBox.Show("Todo el mundo ha aceptado la invitación.");
                        ThreadStart ts = delegate { PonerEnMarchaFormulario1(); }; // creo thread que executa la funcio PonerEnMarchaFormulario
                        Thread t = new Thread(ts);
                        t.Start();

                        break;

                    case 10:
                        idPartida = Convert.ToInt32(trozos[1]);
                        // Recibo seleccion de personaje de alguien:
                        // cridem funcio publica del formulari SeleccionPartida
                        // --> 10/idPartida/jugadorEscogido/Juan
                        formularios1[idPartida].RecibirSeleccionDePersonaje(Convert.ToInt32(trozos[2]), trozos[3]);
                        break;

                    case 11:
                        // Recibo deseleccion de personaje de alguien:
                        // cridem funcio publica del formulari SeleccionPartida
                        // --> 11/idPartida/jugadorDeseleccionado/Juan
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].RecibirDeseleccionDePersonaje(Convert.ToInt32(trozos[2]), trozos[3]);
                        break;

                    case 12:
                        // Recibo notificacion de seleccion de mapa del anfitrion:
                        // 12/idPartida/Mapa
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].AtenderMensajeEleccionMapa(trozos[2]);
                        break;

                    case 13: // LO USAMOS?
                        // Usuario está listo para empezar la partida
                        // --> 13/--------/idPartida
                        numForm = Convert.ToInt32(trozos[1]);
                        break;

                    case 14:
                        // Empieza la partida: El anfitrión le ha dado a empezar: se abrirá otro formulario
                        // --> 14/idPartida
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].AnfitrionEmpiezaPartida();
                        break;

                    case 15:
                        // 15/idPartida/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].TeclaIzquierdaClicada_Otro(Convert.ToInt32(trozos[2]));
                        break;

                    case 16:
                        // 16/idPartida/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].TeclaIzquierdaDejadaDeClicar_Otro(Convert.ToInt32(trozos[2]));
                        break;

                    case 17:
                        // 17/idPartida/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].TeclaDerechaClicada_Otro(Convert.ToInt32(trozos[2]));
                        break;

                    case 18:
                        // 18/idPartida/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[2]));
                        break;

                    case 19:
                        // 19/idPartida/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].TeclaArribaSolaClicada_Otro(Convert.ToInt32(trozos[2]));
                        break;

                    case 20:
                        // mensaje del chat
                        //--> "20/idPartida/Juan/Hola compañeros"
                        // en mapas y seleccion personajes
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].AtenderMensajeChat(trozos[2], trozos[3]);
                        break;

                    case 21:
                        // 21/idPartida/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].TeclaArribaConIzquierdaClicada_Otro(Convert.ToInt32(trozos[2]));
                        break;

                    case 22:
                        // 22/idPartida/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].TeclaArribaConDerechaClicada_Otro(Convert.ToInt32(trozos[2]));
                        break;

                    case 23:
                        // 23/idPartida/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[2]));
                        break;
                   
                }
            }
        }

        private void desconnectButton_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            label3.Visible = false;
            lbl_lista_con.Visible = false;
            PuntMax_But.Visible = false;
            Juan120_But.Visible = false;
            Templo_But.Visible = false;
            GridConectados.Visible = false;
            //panel1.Visible = true;
            desconnectButton.Visible = false;

        
            CrearPartidaBut.Visible = false;
            panel1.Visible = true;

            string mensaje = "0/" + Convert.ToString(idPartida);

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            atender.Abort(); // cerramos thread

            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

            NumInvitados = 0;

            
        }
        private void PuntMax_But_Click(object sender, EventArgs e)
        {
            string mensaje = "3/";// + palabra_box.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void Juan120_But_Click(object sender, EventArgs e)
        {
            string mensaje = "4/";// + palabra_box.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void Templo_But_Click(object sender, EventArgs e)
        {
            string mensaje = "5/";// + palabra_box.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void LogInButton_Click(object sender, EventArgs e)
        {  
            if (conectado == true)
            {
                
                MessageBox.Show("Conectado con el servidor");

                //panel1.Visible = true;
                string mensaje = "1/" + usernameBox.Text + "/" + passwordBox.Text; // + palabra_box.Text ;
                nombre = usernameBox.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                
            }
            else
            {
                int conexion = ConectarConServidor();
                if (conexion == 0)
                {
                    MessageBox.Show("Conectado con el servidor");

                    //panel1.Visible = true;
                    string mensaje = "1/" + usernameBox.Text + "/" + passwordBox.Text; // + palabra_box.Text ;
                    nombre = usernameBox.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    conectado = true;
                }
            }
            
            
        }

        private void NewAccountButton_Click(object sender, EventArgs e)
        {
            int conexion = ConectarConServidor();
            if (conexion == 0)
            {
                MessageBox.Show("Conectado con el servidor");
                string mensaje = "2/" + usernameBox.Text + "/" + passwordBox.Text;
                // pasar los datos de username, y contraseña en una cadena de texto. 
                // Hacer protocolo de applicación de Crear usuario en la base de datos (mirar numero más de ID, y poner +1)


                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                conectado = true;
            }
        }
        private void CrearPartidaBut_Click(object sender, EventArgs e)
        {
            if (But_empezarPartida_activado == false)
            {
                DialogResult r = MessageBox.Show("¿De cuántos jugadores quieres crear la partida?\n{2 jugadores}\t{3 jugadores}\t{4 jugadores}", "Creación de Partida", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    InvPartida = 2;
                }
                else if (r == DialogResult.No)
                {
                    InvPartida = 3;
                }
                else
                {
                    InvPartida = 4;
                }
                MessageBox.Show("Haz doble-click en los nombres de los jugadores que quieras invitar.");
                But_empezarPartida_activado = true;
            }
        }

        private void GridConectados_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (But_empezarPartida_activado == true)
            {
                string invitado = Convert.ToString(GridConectados.CurrentCell.Value);

                if (invitado != nombre)
                {
                    DialogResult r = MessageBox.Show("Quieres invitar a " + invitado + " a una partida?", "¿Aceptar?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (r == DialogResult.OK)
                    {
                        GridConectados.CurrentCell.Style.BackColor = Color.LightGreen;
                        NumInvitados++;
                        invitados = invitados + "/" + invitado;
                        if (NumInvitados == InvPartida-1)
                        {
                            //DialogResult m = MessageBox.Show("Quieres invitar a alguien más?\nPuedes a " + (2-NumInvitados) + " más.", "¿Aceptar?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            //if (m == DialogResult.OK)
                            //{
                            //    MessageBox.Show("Indique el jugador.");

                            //}
                            //else
                            //{
                            EnviarJugadoresPartida(invitados);
                            invitados = "";
                            //}
                        }
                    }
                }
            }
        }
        private void PonerEnMarchaFormulario1()
        { 
            SeleccionPartida f = new SeleccionPartida(idPartida,server);
            formularios1.Add(f);
            f.SetUsername(usernameBox.Text);
            if (soyanfitrion == true)
            {
                f.SetAnfitrion();
            }
            f.ShowDialog();
        }
        private void EnviarJugadoresPartida(string guests)
        {
            MessageBox.Show("Vamos a invitar a los otros jugadores.");
            string mensaje = "7/"+ (InvPartida-1) + guests;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            InvPartida = 0;
            NumInvitados = 0;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Mensaje de desconexión
            label3.Visible = false;
            lbl_lista_con.Visible = false;
            PuntMax_But.Visible = false;
            Juan120_But.Visible = false;
            Templo_But.Visible = false;
            GridConectados.Visible = false;
            //panel1.Visible = true;
            desconnectButton.Visible = false;
            CrearPartidaBut.Visible = false;

            if (conectado == true)
            {
                string mensaje = "0/" + Convert.ToString(idPartida);

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                atender.Abort(); // cerramos thread

                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            conectado = false;
        }


        // SELECCION


        // MAPES

        //private void AcabarPartida_But_Click(object sender, EventArgs e)
        //{
        //    string mensaje = "10/" + Convert.ToString(idPartida);
        //    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
        //    server.Send(msg);
        //}

        //private void tableroJuego_MouseClick(object sender, MouseEventArgs e)
        //{
        //    // añadimos ciruclo a la lista de circulos
        //    //MoverBicho(e.X, e.Y);
        //    string movimiento = "9/" + e.X + "/" + e.Y + "/" + idPartida;
        //    byte[] msg = System.Text.Encoding.ASCII.GetBytes(movimiento);
        //    server.Send(msg);
        //}

        //private void MoverBicho(int x, int y)
        //{
        //    this.x_bicho = x;
        //    this.y_bicho = y;
        //    Point posicion = new Point(x_bicho, y_bicho);
        //    bicho_pb.Location = posicion;
        //}

        //private void EnviarChatBut_Click(object sender, EventArgs e)
        //{
        //    if (chatbox.Text != null)
        //    {
        //        string mensaje_chat = "20/" + chatbox.Text + "/" + idPartida;
        //        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
        //        server.Send(msg);
        //        chatbox.Text = null;
        //    }
        //}
    }
}
