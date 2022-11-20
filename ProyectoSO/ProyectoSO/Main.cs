﻿using System;
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
        private void Main_Load(object sender, EventArgs e)
        {
            label3.Visible = false;
            PuntMax_But.Visible = false;
            Juan120_But.Visible = false;
            Templo_But.Visible = false;
            desconnectButton.Visible = false;
            GridConectados.Visible = false;
            passwordBox.PasswordChar = ('*');
            panel1.Visible = false;
            GridConectados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            GridConectados.RowHeadersVisible = false;
            GridConectados.ColumnCount = 1;
            GridConectados.Columns[0].HeaderText = "Username";

        }
        public Main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; // que permita modificar la label los threads
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
                            label3.Visible = true;
                            PuntMax_But.Visible = true;
                            Juan120_But.Visible = true;
                            Templo_But.Visible = true;
                            desconnectButton.Visible = true;
                            panel1.Visible = false;
                            GridConectados.Visible = true;
                            this.BackColor = Color.Green;
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
                        GridConectados.Rows.Clear();
                        for (int i = 0; i < num; i++)
                        {
                            string nombre = Convert.ToString(trozos[i+2].Split('\0')[0]);
                            GridConectados.Rows.Add(nombre);
                        }
                        //GridConectados.Refresh();
                        GridConectados.ClearSelection();
                        break;
                    case 7: // Peticion de partida
                            // 7/Maria: A quien te está pidiendo partida
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
                        // --> "7/resp"

                        break;
                    case 8: // Respuesta a  peticion de partida
                            // "8/Juan/SI/2": Quien ha aceptado/su respuesta/idPartida
                        string nombre_acepta = mensaje;
                        string respuesta = Convert.ToString(trozos[2].Split('\0')[0]);
                        if (respuesta == "Si")
                        {
                            idPartida = Convert.ToInt32(trozos[3].Split('\0')[0]);
                            MessageBox.Show(nombre_acepta + " ha aceptado tu invitación a partida");
                            // * * * ** * * Se abre tablero etc etc etc.
                        }
                        else
                        {
                            MessageBox.Show(nombre_acepta + "no ha aceptado tu invitación a partida");
                        }

                        break;
                }
            }
        }
        private void desconnectButton_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            label3.Visible = false;
            PuntMax_But.Visible = false;
            Juan120_But.Visible = false;
            Templo_But.Visible = false;
            GridConectados.Visible = false;
            //panel1.Visible = true;
            desconnectButton.Visible = false;
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            atender.Abort(); // cerramos thread

            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

            conectar_bt.Visible = true;
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
                string mensaje = "1/" + usernameBox.Text + "/" + passwordBox.Text; // + palabra_box.Text ;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                

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
                { ip = "147.83.117.22"; }
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

        private void conectar_bt_Click(object sender, EventArgs e)
        {
            int conexion = ConectarConServidor();
            if (conexion == 0)
            {
                MessageBox.Show("Conectado con el servidor");
                conectar_bt.Visible = false;
                panel1.Visible = true;
            }
        }

    }
}
