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
    public partial class SeleccionPartida : Form
    {
        string username;
        int idPartida;

        int numJugadoresPartida;

        Socket server; // declaramos socket

        // 0: libre; 1: ocupado por mi; -1: ocupado por otro jugador; 2: no lo puedo ocupar porque ya ocupo uno
        int J1seleccionado = 0;
        int J2seleccionado = 0;
        int J3seleccionado = 0;
        int J4seleccionado = 0;

        // Si anfitrion = true --> este cliente es el creador de la partida: escoge mapa y dice cuando empezar
        bool anfitrion = false;
        string mapa;

        public SeleccionPartida(int idpartida, Socket server)
        {
            InitializeComponent();
            this.idPartida = idpartida; // num de form que em donen --> l'afegeixo en els missatges de peticio de servei
            this.server = server;
        }

        // Seters
        public void SetUsername(string username)
        { this.username = username; }

        public void SetAnfitrion()
        { this.anfitrion = true; }
        public void SetMapa(string mapa)
        { this.mapa = mapa; }
        public void SetNumJugadores(int num)
        { this.numJugadoresPartida = num; }
        private void PantallaEleccionPersonaje_Load(object sender, EventArgs e)
        {
            // - - - - - - - - - - - - - - -- - - - - - Establecemos las diferentes opciones de mapas y personajes dependiendo del numero de Jugadores -- - - - - - - -- - - - -- - - -- - - - - - -- - 

            comboBox_Mapa.Items.Clear();
            // Para el anfitrion ponemos las opciones de MAPAS  -   -   -   -   -   -   -   -   -   -   -   -   -   -   
            if (numJugadoresPartida == 4)
            {
                comboBox_Mapa.Items.Add("Templo");
                comboBox_Mapa.Items.Add("Volcan");
                comboBox_Mapa.Items.Add("Cueva");
            }
            else if (numJugadoresPartida == 3)
            {
                comboBox_Mapa.Items.Add("Volcan (3Jug)");
            }
            else // if (numJugadoresPartida == 2)
            {
                comboBox_Mapa.Items.Add("Volcan (2Jug)");
                comboBox_Mapa.Items.Add("Cueva");
            }

            //picurebox1 -- Fireboy
            pictureBox1.Width = 200 / 2;
            pictureBox1.Height = 300 / 2;
            pictureBox1.Image = Image.FromFile("Fireboy_grande.gif");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BackColor = Color.Transparent;
            //picurebox2 -- Watergirl
            pictureBox2.Width = 200 / 2;
            pictureBox2.Height = 300 / 2;
            pictureBox2.Image = Image.FromFile("Watergirl_grande.gif");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.BackColor = Color.Transparent;
            //picurebox3 -- Rockboy
            pictureBox3.Width = 200 / 2;
            pictureBox3.Height = 300 / 2;
            pictureBox3.Image = Image.FromFile("Rockboy_grande.gif");
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.BackColor = Color.Transparent;
            //picurebox4 -- Cloudgirl
            pictureBox4.Width = 250 / 2;
            pictureBox4.Height = 300 / 2;
            pictureBox4.Image = Image.FromFile("Cloudgirl_grande.gif");
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.BackColor = Color.Transparent;

            //Posiciones:
            pictureBox1.Top = label1.Bottom;
            pictureBox1.Left = label1.Left - 15;
            pictureBox2.Top = label2.Bottom;
            pictureBox2.Left = label2.Left - 15;
            pictureBox3.Top = label3.Bottom;
            pictureBox3.Left = label3.Left - 15;
            pictureBox4.Top = label4.Bottom;
            pictureBox4.Left = label4.Left - 30;

            //Label usuario
            usuario1.Width = pictureBox1.Width;
            usuario1.Left = pictureBox1.Left;
            usuario1.Top = label1.Bottom + pictureBox1.Height + 15;
            usuario2.Width = pictureBox2.Width;
            usuario2.Left = pictureBox2.Left;
            usuario2.Top = label2.Bottom + pictureBox2.Height + 15;
            usuario3.Width = pictureBox3.Width;
            usuario3.Left = pictureBox3.Left;
            usuario3.Top = label3.Bottom + pictureBox3.Height + 15;
            usuario4.Width = pictureBox4.Width;
            usuario4.Left = pictureBox4.Left;
            usuario4.Top = label4.Bottom + pictureBox4.Height + 15;

            //Botones de selcción
            SelJ1_but.Top = usuario1.Bottom + 15;
            SelJ1_but.Left = usuario1.Left + 5;
            SelJ2_but.Top = usuario2.Bottom + 15;
            SelJ2_but.Left = usuario2.Left + 5;
            SelJ3_but.Top = usuario3.Bottom + 15;
            SelJ3_but.Left = usuario3.Left + 5;
            SelJ4_but.Top = usuario4.Bottom + 15;
            SelJ4_but.Left = usuario4.Left + 20;

            //chat partida
            chatGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            chatGrid.ReadOnly = true;
            chatGrid.RowHeadersVisible = false;
            chatGrid.ColumnHeadersVisible = false;
            chatGrid.ColumnCount = 1;

            // Propiedades del anfitrion: mapa y empezar partida
            if (anfitrion == false)
            {
                comboBox_Mapa.Visible = false;
                empezarPartida_but.Visible = false;
            }
            numForm.Text = "ID partida: " + idPartida;
        }

        // Funciones de atender mensajes:
        public void RecibirSeleccionDePersonaje(int nPersonaje, string usuario_nombre)
        {
            Invoke(new Action(() =>
            {
                OtroSeleccionaJugador(nPersonaje, usuario_nombre);
            }));
        }
        public void RecibirDeseleccionDePersonaje(int nPersonaje, string usuario_nombre)
        {
            Invoke(new Action(() =>
            {
                OtroDesSeleccionaJugador(nPersonaje, usuario_nombre);
            }));
        }
        public void AtenderMensajeEleccionMapa(string mapa_escogido)
        {
            Invoke(new Action(() =>
            {
                this.mapa = mapa_escogido;
                map_escogido_lbl.Text = mapa_escogido;
            }));
        }

        // Métodos para manejar que los otros jugadores escojan personajes
        // Necesitan el número de jugador escogido y el nombre del usuario
        private void OtroSeleccionaJugador(int Jugador, string nombre)
        {
            switch (Jugador)
            {
                case 1:
                    this.J1seleccionado = -1;
                    SelJ1_but.Text = "Ocupado";
                    SelJ1_but.BackColor = Color.SteelBlue;
                    usuario1.Text = nombre;
                    break;
                case 2:
                    this.J2seleccionado = -1;
                    SelJ2_but.Text = "Ocupado";
                    SelJ2_but.BackColor = Color.SteelBlue;
                    usuario2.Text = nombre;
                    break;
                case 3:
                    this.J3seleccionado = -1;
                    SelJ3_but.Text = "Ocupado";
                    SelJ3_but.BackColor = Color.SteelBlue;
                    usuario3.Text = nombre;
                    break;
                case 4:
                    this.J4seleccionado = -1;
                    SelJ4_but.Text = "Ocupado";
                    SelJ4_but.BackColor = Color.SteelBlue;
                    usuario4.Text = nombre;
                    break;
            }

        }
        private void OtroDesSeleccionaJugador(int Jugador, string nombre)
        {
            switch (Jugador)
            {
                case 1:
                    this.J1seleccionado = 0;
                    SelJ1_but.Text = "Seleccionar";
                    SelJ1_but.BackColor = Color.Silver;
                    usuario1.Text = "Usuario 1";
                    break;
                case 2:
                    this.J2seleccionado = 0;
                    SelJ2_but.Text = "Seleccionar";
                    SelJ2_but.BackColor = Color.Silver;
                    usuario2.Text = "Usuario 2";
                    break;
                case 3:
                    this.J3seleccionado = 0;
                    SelJ3_but.Text = "Seleccionar";
                    SelJ3_but.BackColor = Color.Silver;
                    usuario3.Text = "Usuario 3";
                    break;
                case 4:
                    this.J4seleccionado = 0;
                    SelJ4_but.Text = "Seleccionar";
                    SelJ4_but.BackColor = Color.Silver;
                    usuario4.Text = "Usuario 4";
                    break;
            }

        }

        // Métodos de nuestros botones
        private void SelJ1_but_Click(object sender, EventArgs e)
        {
            if (J1seleccionado == 0)
            {
                J1seleccionado = 1;
                SelJ1_but.Text = "Deseleccionar";
                usuario1.Text = username;

                if (J2seleccionado != -1)
                { J2seleccionado = 2; }
                if (J3seleccionado != -1)
                { J3seleccionado = 2; }
                if (J4seleccionado != -1)
                { J4seleccionado = 2; }

                string mensaje_chat = "10/" + idPartida + "/1";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);

            }
            else if (J1seleccionado == 1)
            {
                J1seleccionado = 0;
                usuario1.Text = null;
                SelJ1_but.Text = "Seleccionar";
                usuario1.Text = "Usuario 1";

                if (J2seleccionado != -1)
                { J2seleccionado = 0; }
                if (J3seleccionado != -1)
                { J3seleccionado = 0; }
                if (J4seleccionado != -1)
                { J4seleccionado = 0; }

                string mensaje_chat = "11/" + idPartida + "/1/" + username;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);
            }
        }

        private void SelJ2_but_Click(object sender, EventArgs e)
        {
            if (J2seleccionado == 0)
            {
                J2seleccionado = 1;
                SelJ2_but.Text = "Deseleccionar";
                usuario2.Text = username;

                if (J1seleccionado != -1)
                { J1seleccionado = 2; }
                if (J3seleccionado != -1)
                { J3seleccionado = 2; }
                if (J4seleccionado != -1)
                { J4seleccionado = 2; }

                string mensaje_chat = "10/" + idPartida + "/2/" + username;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);
            }
            else if (J2seleccionado == 1)
            {
                J2seleccionado = 0;
                usuario2.Text = null;
                SelJ2_but.Text = "Seleccionar";
                usuario2.Text = "Usuario 2";

                if (J1seleccionado != -1)
                { J1seleccionado = 0; }
                if (J3seleccionado != -1)
                { J3seleccionado = 0; }
                if (J4seleccionado != -1)
                { J4seleccionado = 0; }

                string mensaje_chat = "11/" + idPartida + "/2/" + username;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);
            }
        }

        private void SelJ3_but_Click(object sender, EventArgs e)
        {
            if (J3seleccionado == 0)
            {
                J3seleccionado = 1;
                SelJ3_but.Text = "Deseleccionar";
                usuario3.Text = username;

                if (J1seleccionado != -1)
                { J1seleccionado = 2; }
                if (J2seleccionado != -1)
                { J2seleccionado = 2; }
                if (J4seleccionado != -1)
                { J4seleccionado = 2; }

                string mensaje_chat = "10/" + idPartida + "/3/" + username;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);
            }
            else if (J3seleccionado == 1)
            {
                J3seleccionado = 0;
                usuario3.Text = null;
                SelJ3_but.Text = "Seleccionar";
                usuario3.Text = "Usuario 3";

                if (J1seleccionado != -1)
                { J1seleccionado = 0; }
                if (J2seleccionado != -1)
                { J2seleccionado = 0; }
                if (J4seleccionado != -1)
                { J4seleccionado = 0; }

                string mensaje_chat = "11/" + idPartida + "/3/" + username;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);
            }
        }

        private void SelJ4_but_Click(object sender, EventArgs e)
        {
            if (J4seleccionado == 0)
            {
                J4seleccionado = 1;
                SelJ4_but.Text = "Deseleccionar";
                usuario4.Text = username;

                if (J1seleccionado != -1)
                { J1seleccionado = 2; }
                if (J2seleccionado != -1)
                { J2seleccionado = 2; }
                if (J3seleccionado != -1)
                { J3seleccionado = 2; }

                string mensaje_chat = "10/" + idPartida + "/4/" + username;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);
            }
            else if (J4seleccionado == 1)
            {
                J4seleccionado = 0;
                usuario4.Text = null;
                SelJ4_but.Text = "Seleccionar";
                usuario4.Text = "Usuario 4";

                if (J1seleccionado != -1)
                { J1seleccionado = 0; }
                if (J2seleccionado != -1)
                { J2seleccionado = 0; }
                if (J3seleccionado != -1)
                { J3seleccionado = 0; }

                string mensaje_chat = "11/" + idPartida + "/4/" + username;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);
            }
        }

        private void comboBox_Mapa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Mapa.Text != null)
            {
                this.mapa = comboBox_Mapa.Text;
                string mensaje = "12/" + idPartida + "/" + mapa;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                map_escogido_lbl.Text = mapa;

            }
        }


        private void empezarPartida_but_Click(object sender, EventArgs e)
        {
            //Primero miramos que se haya escogido la partida

            if (map_escogido_lbl.Text != "")
            {
                // miramos si alguno de los personajes está libre: si lo está no podemos empezar la partida
                // 0: libre; 1: ocupado por mi; -1: ocupado por otro jugador; 2: no lo puedo ocupar porque ya ocupo uno
                // hablar de partidas de menos de 4 jugadores
                if (numJugadoresPartida == 4)
                {
                    if (Estan4JugadoresListos() == true)
                    {
                        EnviarMensajeEmpiezaPartida();
                    }
                    else
                    {
                        MessageBox.Show("Alguien no está listo.");
                    }
                }
                else if (numJugadoresPartida == 3)  // mirar qué personajes hay disponibles
                {
                    if (Estan3JugadoresListos() == true)
                    {
                        EnviarMensajeEmpiezaPartida();
                    }
                    else
                    {
                        MessageBox.Show("Alguien no está listo.");
                    }
                }
                else if (numJugadoresPartida == 2)  // mirar qué personajes hay disponibles
                {
                    if (Estan2JugadoresListos() == true)
                    {
                        EnviarMensajeEmpiezaPartida();
                    }
                    else
                    {
                        MessageBox.Show("Alguien no está listo.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Escoge un mapa para la partida.");
            }
        }
        
        private void EnviarMensajeEmpiezaPartida()
        {
            string mensaje_chat = "14/" + idPartida + "/" + DameNombreJug1() + "/" + DameNombreJug2() + "/" + DameNombreJug3() + "/" + DameNombreJug4();
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
            server.Send(msg);
        }
        // funciones para ver si están los jugadores listos para cuando hay 4, 3 o 2 jugadores (mira todas las opciones)
        private bool Estan4JugadoresListos()
        { 
            if ((J1seleccionado == 1 || J1seleccionado == -1) && (J2seleccionado == 1 || J2seleccionado == -1) && (J3seleccionado == 1 || J3seleccionado == -1) && (J4seleccionado == 1 || J4seleccionado == -1))
            { return true; }
            else
            { return false;}
        }
        private bool Estan3JugadoresListos() // partida de 3jugadores --> solo 1 no puede jugar
        {
            // miramos primero si jug 1 juega
            if (((J1seleccionado == 1 || J1seleccionado == -1) && (J2seleccionado == 1 || J2seleccionado == -1) && (J3seleccionado == 1 || J3seleccionado == -1))   //jug 4 no juega
                || ((J1seleccionado == 1 || J1seleccionado == -1) && (J2seleccionado == 1 || J2seleccionado == -1) && (J4seleccionado == 1 || J4seleccionado == -1))   //jug 3 no juega
                || ((J1seleccionado == 1 || J1seleccionado == -1) && (J3seleccionado == 1 || J3seleccionado == -1) && (J4seleccionado == 1 || J4seleccionado == -1)))   //jug 2 no juega
            { 
                return true;
            }
            else // jug 1 no juega --> juegan los otros 3
            {
                if ((J4seleccionado == 1 || J4seleccionado == -1) && (J2seleccionado == 1 || J2seleccionado == -1) && (J3seleccionado == 1 || J3seleccionado == -1))
                { return true; }
                else
                { return false; }
            }
        }
        private bool Estan2JugadoresListos() // partida de 2jugadores --> solo 2 no puede jugar
        {
            // miramos primero si jug 1 juega
            if (((J1seleccionado == 1 || J1seleccionado == -1) && (J2seleccionado == 1 || J2seleccionado == -1))   //jug 3 y jug 4 no juegan
                || ((J1seleccionado == 1 || J1seleccionado == -1) && (J4seleccionado == 1 || J4seleccionado == -1))   //jug 2 y jug 3 no juegan
                || ((J1seleccionado == 1 || J1seleccionado == -1) && (J3seleccionado == 1 || J3seleccionado == -1)))   //jug 2 y jug 4 no juegan
            {
                return true;
            }
            else // jug 1 no juega
            {
                // miramos si jug 2 juega
                if (((J2seleccionado == 1 || J2seleccionado == -1) && (J4seleccionado == 1 || J4seleccionado == -1))   //jug 1 y jug 3 no juegan
                   || ((J2seleccionado == 1 || J2seleccionado == -1) && (J3seleccionado == 1 || J3seleccionado == -1)))   //jug 1 y jug 4 no juegan
                {
                    return true;
                }
                else // no juega ni jug 1 ni 2 --> juega jug 3 y jug 4
                {
                    if ((J3seleccionado == 1 || J3seleccionado == -1) && (J4seleccionado == 1 || J4seleccionado == -1))
                    { return true; }
                    else
                    { return false; }
                }
            }
        }
        // Chat
        private void EnviarChatBut_Click(object sender, EventArgs e)
        {
            if (chatbox.Text != null)
            {
                string mensaje_chat = "20/" + idPartida + "/" + chatbox.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_chat);
                server.Send(msg);
                chatbox.Text = null;
            }
        }

        public void AtenderMensajeChat(string nombre, string informacion)
        {
            Invoke(new Action(() =>
            {
                chatGrid.Rows.Add(nombre + ": " + informacion);
                chatGrid.ClearSelection();
            }));
        }

        // - - - - - - - - - - - - - - - - Funciones para pasar información a los mapas - - -- - - - - - - - - - - - - 
        public int DameMiPersonajeQueHeEscogido()
        {
            int found = 0;
            if (J1seleccionado == 1)
            { found = 1; }
            else if (J2seleccionado == 1)
            { found = 2; }
            else if (J3seleccionado == 1)
            { found = 3; }
            else if (J4seleccionado == 1)
            { found = 4; }
            return found;
        }
        public string DameNombreJug1()
        { return this.usuario1.Text; }
        public string DameNombreJug2()
        { return this.usuario2.Text; }
        public string DameNombreJug3()
        { return this.usuario3.Text; }
        public string DameNombreJug4()
        { return this.usuario4.Text; }
        public bool DameJug1Juega()
        { 
            if (J1seleccionado == 1 || J1seleccionado == -1)
            { return true; }
            else
            { return false; }
        }
        public bool DameJug2Juega()
        {
            if (J2seleccionado == 1 || J2seleccionado == -1)
            { return true; }
            else
            { return false; }
        }
        public bool DameJug3Juega()
        {
            if (J3seleccionado == 1 || J3seleccionado == -1)
            { return true; }
            else
            { return false; }
        }
        public bool DameJug4Juega()
        {
            if (J4seleccionado == 1 || J4seleccionado == -1)
            { return true; }
            else
            { return false; }
        }

        public string GetMapa()
        { return this.mapa; }
        public void CerrarSeleccionPartida()
        {
            Invoke(new Action(() =>
            {
                this.Close();
            }));
        }
    }
}
