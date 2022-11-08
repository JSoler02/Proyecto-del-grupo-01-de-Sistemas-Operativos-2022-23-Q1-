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
using System.Net.Sockets;

namespace ProyectoSO
{
    public partial class Main : Form
    {
        Socket server;

        public Main()
        {
            InitializeComponent();
            label3.Visible = false;
            PuntMax_But.Visible = false;
            Juan120_But.Visible = false;
            Templo_But.Visible = false;
            desconnectButton.Visible = false;
            listaCon_but.Visible = false;
            passwordBox.PasswordChar = ('*');
        }

        private void desconnectButton_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            label3.Visible = false;
            PuntMax_But.Visible = false;
            Juan120_But.Visible = false;
            Templo_But.Visible = false;
            listaCon_but.Visible = false;
            panel1.Visible = true;
            desconnectButton.Visible = false;
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
        private void PuntMax_But_Click(object sender, EventArgs e)
        {
            string mensaje = "3/";// + palabra_box.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            MessageBox.Show("La puntuación máxima es: " + mensaje);
        }

        private void Juan120_But_Click(object sender, EventArgs e)
        {
            string mensaje = "4/";// + palabra_box.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            MessageBox.Show("Juan ha jugado más de 120 segundos en las partidas: " + mensaje + ".");
        }

        private void Templo_But_Click(object sender, EventArgs e)
        {
            string mensaje = "5/";// + palabra_box.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

            MessageBox.Show(mensaje + " han jugado partidas en el mapa 'templo' como jugador 1.");
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            /*Llenamos textbox para que no dé error si están vacías
            if (usernameBox.Text == "")
            { usernameBox.Text = " "; }
            if(passwordBox.Text == "")
             { passwordBox.Text = " "; }
            */

            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9085);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                string mensaje = "1/" + usernameBox.Text + "/" + passwordBox.Text; // + palabra_box.Text ;
                server.Connect(ipep);//Intentamos conectar el socket

                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


                MessageBox.Show(mensaje);
                if (mensaje == "Conectado")
                {
                    label3.Visible = true;
                    PuntMax_But.Visible = true;
                    Juan120_But.Visible = true;
                    Templo_But.Visible = true;
                    desconnectButton.Visible = true;
                    listaCon_but.Visible = true;
                    panel1.Visible = false;
                    this.BackColor = Color.Green;
                    //TableRefresh();


                    
                }


            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }
        
        }

        private void NewAccountButton_Click(object sender, EventArgs e)
        {
            string mensaje = "2/"+usernameBox.Text+"/"+passwordBox.Text;
            //-->> cositas para crear username etc en la base de datos de C
            // pasar los datos de username, y contraseña en una cadena de texto. 
            // Hacer protocolo de applicación de Crear usuario en la base de datos (mirar numero más de ID, y poner +1)

            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

            // hacer messagebox diciendo que usuario existente y/o usuario creado correctamente
            MessageBox.Show(mensaje);
        }

        private void listaCon_but_Click(object sender, EventArgs e)
        {
            
            ListaCon f = new ListaCon();
            f.PassarSocket(server);
            f.ShowDialog();
        }
    }
}
