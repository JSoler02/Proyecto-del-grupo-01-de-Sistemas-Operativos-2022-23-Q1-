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
        int julia = 1;  // 1: si IP de Julia en la Maquina Virtual; 0: si IP del resto en la Maquina virtual

        int idPartida;
        string nombre;

        int x_bicho;
        int y_bicho;

        string invitados;
        int NumInvitados = 0; // maximo puede ser 4
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
            tableroJuego.Visible = false;
            AcabarPartida_But.Visible = false;

            //chat partida
            chatGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            chatGrid.ReadOnly = true;
            chatGrid.RowHeadersVisible = false;
            chatGrid.ColumnHeadersVisible = false;
            chatGrid.ColumnCount = 1;
            chatGrid.Visible = false;
            chatbox.Visible = false;
            EnviarChatBut.Visible = false;

            bicho_pb.Image = Image.FromFile("Fireboy.gif");
            bicho_pb.BackColor = Color.Transparent;
            bicho_pb.Width = 60;
            bicho_pb.Height = 75;



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

                string mensaje = trozos[1];
                switch (codigo)
                {
                    case 1: // Login
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
                                this.BackColor = Color.Green;
                            }));
                        }
                        break;
                    case 2: // New User
                        MessageBox.Show(mensaje);
                        break;
                    case 3: //consulta 1 --> Puntos maximos de Maria
                        MessageBox.Show("La puntuación máxima es: " + mensaje);
                        break;
                    case 4: //consulta 2 --> Id de las partidas de más de 120 s de Juan
                        MessageBox.Show("Juan ha jugado más de 120 segundos en las partidas: " + mensaje);
                        break;
                    case 5: //consulta 3 --> Nombre de los jugadores que han jugado como J1 en "templo"
                        MessageBox.Show(mensaje + " han jugado partidas en el mapa 'templo' como jugador 1");
                        break;
                    case 6: // Notificación de la Lista de Conectados

                        // string mensaje = "3/Juan/Pedro/Maria"; 
                        // el numero inicial nos indica el número de usuarios conectados
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
                        string anfitrion = mensaje;
                        string resp; // "Si o No"
                        DialogResult r = MessageBox.Show(anfitrion + " quiere jugar contigo.", "¿Aceptar?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (r == DialogResult.OK)
                        {
                            resp = "Si";
                            Invoke(new Action(() =>
                            {
                                tableroJuego.Visible = true;
                                chatGrid.Visible = true;
                                chatbox.Visible = true;
                                EnviarChatBut.Visible = true;
                                AcabarPartida_But.Visible = true;
                            }));                            

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
                    case 8: // Respuesta a  peticion de partida
                            // "8/Juan/Si/2": Quien ha aceptado/su respuesta/idPartida
                        string nombre_acepta = mensaje;
                        string respuesta = Convert.ToString(trozos[2].Split('\0')[0]);
                        if (respuesta == "Si")
                        {
                            idPartida = Convert.ToInt32(trozos[3].Split('\0')[0]);
                            MessageBox.Show(nombre_acepta + " ha aceptado tu invitación a partida");
                            Invoke(new Action(() =>
                            {
                                tableroJuego.Visible = true;    //  ---> inicia la partida para el anfitrion
                                chatGrid.Visible = true;
                                chatbox.Visible = true;
                                EnviarChatBut.Visible = true;
                                AcabarPartida_But.Visible = true;
                            }));
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
                    case 9: // acciones durante el juego: saltar, derecha, izquierda, quieto
                            // 9/movimiento/IDpartida --> mensaje que recibo conforme otro jugador ha realizado un movimiento
                        string ans = Convert.ToString(trozos[2].Split('\0')[0]);
                        // idPartida = Convert.ToInt32(trozos[3].Split('\0')[0]);
                        // 9/x/y/idpartida
                        
                        Invoke(new Action(() =>
                        {
                            MoverBicho(Convert.ToInt32(mensaje), Convert.ToInt32(trozos[2].Split('\0')[0]));
                            
                        }));
                        

                        //if (ans == "saltar")
                        //{
                        //    // se me actualiza el monigote del otro jugador con el movimento que ha realizado
                        //}
                        //else if (ans == "derecha")
                        //{

                        //}
                        //else if (ans == "izquierda")
                        //{

                        //}
                        //else // quieto
                        //{

                        //}

                        break;

                    case 10: // em diuen que un company s'ha desconnectat
                             // 10/idpartida

                        MessageBox.Show("La partida ha terminado.");

                        Invoke(new Action(() =>
                        {
                            tableroJuego.Visible = false;
                            chatGrid.Visible = false;
                            chatbox.Visible = false;
                            EnviarChatBut.Visible = false;
                            AcabarPartida_But.Visible = false;
                        }));
                        NumInvitados = 0;
                        chatGrid.Rows.Clear();
                        break;

                    case 20:
                        // mensaje del chat
                        //--> "20/Juan/Hola compañeros/idpartida"
                        string n = mensaje; // trozos[1];
                        string informacion = trozos[2];
                        Invoke(new Action(() =>
                        {
                            chatGrid.Rows.Add(n + ": " + informacion);
                            chatGrid.ClearSelection();
                        }));
                        break;
                }
            }
        }
        private void desconnectButton_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            label3.Visible = false;
            lbl_lista_con.Visible = false;
            tableroJuego.Visible = false;
            PuntMax_But.Visible = false;
            Juan120_But.Visible = false;
            Templo_But.Visible = false;
            GridConectados.Visible = false;
            //panel1.Visible = true;
            desconnectButton.Visible = false;

            chatGrid.Visible = false;
            chatbox.Visible = false;
            EnviarChatBut.Visible = false;
            AcabarPartida_But.Visible = false;
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
            //int conexion = ConectarConServidor();
            //if (conexion == 0)
            //{
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
            }
            


            //}

        }

        private void NewAccountButton_Click(object sender, EventArgs e)
        {
            //int conexion = ConectarConServidor();
            //if (conexion == 0)
            //{
            string mensaje = "2/" + usernameBox.Text + "/" + passwordBox.Text;
            // pasar los datos de username, y contraseña en una cadena de texto. 
            // Hacer protocolo de applicación de Crear usuario en la base de datos (mirar numero más de ID, y poner +1)


            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //}
        }


        private void GridConectados_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string invitado = Convert.ToString(GridConectados.CurrentCell.Value);
            
            if (invitado != nombre)
            {
                DialogResult r = MessageBox.Show("Quieres invitar a " + invitado + " a una partida?", "¿Aceptar?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                {
                    GridConectados.CurrentCell.Style.BackColor = Color.LightGreen;

                    invitados = invitados + "/" + invitado;
                    if (NumInvitados < 3)
                    {
                        DialogResult m = MessageBox.Show("Quieres invitar a alguien más?\nPuedes a " + (2-NumInvitados) + " más.", "¿Aceptar?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (m == DialogResult.OK)
                        {
                            MessageBox.Show("Indique el jugador.");
                        }
                        else
                        {
                            EnviarJugadoresPartida(invitados);
                            invitados = "";
                        }
                        NumInvitados++;
                    }
                }
            }
        }
        
        private void EnviarJugadoresPartida(string guests)
        {
            MessageBox.Show("Vamos a invitar a los otros jugadores.");
            string mensaje = "7" + guests;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void tableroJuego_MouseClick(object sender, MouseEventArgs e)
        {
            // añadimos ciruclo a la lista de circulos
            //MoverBicho(e.X, e.Y);
            string movimiento = "9/" + e.X + "/" + e.Y + "/" + idPartida;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(movimiento);
            server.Send(msg);

        }
        private void MoverBicho(int x, int y)
        {
            this.x_bicho = x;
            this.y_bicho = y;
            Point posicion = new Point(x_bicho, y_bicho);
            bicho_pb.Location = posicion;
        }

        private void EnviarChatBut_Click(object sender, EventArgs e)
        {
            if (chatbox.Text != null)
            {
                string mensaje_chat = "20/" + chatbox.Text + "/" + idPartida;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);
                chatbox.Text = null;
            }
        }


        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Mensaje de desconexión
            label3.Visible = false;
            lbl_lista_con.Visible = false;
            tableroJuego.Visible = false;
            PuntMax_But.Visible = false;
            Juan120_But.Visible = false;
            Templo_But.Visible = false;
            GridConectados.Visible = false;
            //panel1.Visible = true;
            desconnectButton.Visible = false;
            AcabarPartida_But.Visible = false;

            chatGrid.Visible = false;
            chatbox.Visible = false;
            EnviarChatBut.Visible = false;

            string mensaje = "0/" + Convert.ToString(idPartida);

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            atender.Abort(); // cerramos thread

            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

            
        }

        private void AcabarPartida_But_Click(object sender, EventArgs e)
        {
            string mensaje = "10/" + Convert.ToString(idPartida);
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        /*
        private void tableroJuego_VisibleChanged(object sender, EventArgs e)
        {
            // cuando el tablero se haga visible (hay cambio en su visibilidad) --> apareceria todo
            
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            // le enviamos un movimento que hace el cliente a otro jugador.
            // lo hacemos solo si ha empezado la partida 
            // faltaria saber el id de la partida per tenir el missatge guai
            if (tableroJuego.Visible == true)
            {
                if (e.KeyCode == Keys.Left)
                {
                    string mov1 = "9/Left";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mov1);
                    server.Send(msg);
                }
            }            
        }
        */
    }
}
