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
using System.Threading; // libreria de threads

namespace ProyectoSO
{
    public partial class Templo : Form
    {
        int idPartida;
        Socket server; // declaramos socket
        int nForm;

        // Mi personaje --> 1,2,3,4
        int miPersonajeQueControlo;

        // Variables globales que se crean en False
        bool goLeft_J1, goRight_J1, jumping_J1, isGameOver_J1, onPlatform_J1;
        bool goLeft_J2, goRight_J2, jumping_J2, isGameOver_J2, onPlatform_J2;
        bool goLeft_J3, goRight_J3, jumping_J3, isGameOver_J3, onPlatform_J3;
        bool goLeft_J4, goRight_J4, jumping_J4, isGameOver_J4, onPlatform_J4;

        bool permitirAnimaciones_J1 = true;
        bool permitirAnimaciones_J2 = true;
        bool permitirAnimaciones_J3 = true;
        bool permitirAnimaciones_J4 = true;


        // Variables para el Jugador
        int jumpSpeed = 8; // velocidad vertical
        int jumpSpeed1; int jumpSpeed2; int jumpSpeed3; int jumpSpeed4; // --> una para cada uno para controlar el salto
        int force1; int force2; int force3; int force4; // contador para el maximo del salto
        int puntos1; int puntos2; int puntos3; int puntos4;
        int hspeed = 7; // velocidad horizontal --> general


        // Velocidades para las PLATAFORMAS verticales y horizontales
        int horizontalSpeed = 5;
        int verticalSpeed = 3;

        private void Templo_Load(object sender, EventArgs e)
        {
            RestartGame();
        }

        bool plataformaVertical1_activa1; bool plataformaVertical1_activa2; bool plataformaVertical1_activa3; bool plataformaVertical1_activa4;
        bool plataformaVertical1_final;

        // Velocidad para el ENEMIGO
        int enemy1Speed = 5;

        // Personajes/PictureBoxes
        //EquipoJugadores miequipo = new EquipoJugadores();
        List<PictureBox> misPicsPersonajes = new List<PictureBox>();
        int numPics = 1;

        Jugador Jug1 = new Jugador(1, "Juan", 30, 200);
        Jugador Jug2 = new Jugador(2, "Maria", 100, 200);
        Jugador Jug3 = new Jugador(3, "Bernat", 225, 200);
        Jugador Jug4 = new Jugador(4, "Pedro", 300, 200);
        // variables del tiempo
        int segundos = 0;

        private void tiempoJuego_Tick(object sender, EventArgs e)
        {
            segundos++;
            label_tiempo.Text = "Tiempo: " + segundos;
        }

        public Templo(int idPartida, Socket server)
        {
            InitializeComponent();
            this.idPartida = idPartida; // num de form que em donen --> l'afegeixo en els missatges de peticio de servei
            this.server = server;
        }

        private void pintarPersonajesEnSusPosiciones()
        {
            Point p = new Point(Jug1.GetX(), Jug1.GetY());
            misPicsPersonajes[0].Location = p;
            p = new Point(Jug2.GetX(), Jug2.GetY());
            misPicsPersonajes[1].Location = p;
            p = new Point(Jug3.GetX(), Jug3.GetY());
            misPicsPersonajes[2].Location = p;
            p = new Point(Jug4.GetX(), Jug4.GetY());
            misPicsPersonajes[3].Location = p;
        }
        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            lblPuntos.Text = "Puntos J1: " + puntos1;

            pintarPersonajesEnSusPosiciones();
            movimientosPersonajesConCadaTick();

            // Control de los Picturebox
            foreach (Control x in this.Controls)
            {
                // Plataformas
                if ((string)x.Tag == "plataforma")
                {
                    ColisionesPersonajesPlataformas(x);
                }
                //placas
                if ((string)x.Name == "placa1")
                {
                    ColisionesPersonajesPlaca1(x);

                }
                // techo
                if ((string)x.Tag == "techo")
                {
                    ColisionesPersonajesTecho(x);
                }
                // pared izquierda
                if ((string)x.Tag == "pared_izquierda")
                {
                    ColisionesPersonajesParedIzquierda(x);
                }
                // pared izquierda
                if ((string)x.Tag == "pared_derecha")
                {
                    ColisionesPersonajesParedDerecha(x);
                }
                // Puntos
                if ((string)x.Tag == "diamante1")
                {
                    if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false; // se eliminan los diamantes
                        puntos1++;
                    }
                }
                if ((string)x.Tag == "diamante2")
                {
                    if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false; // se eliminan los diamantes
                        puntos2++;
                    }
                }
                if ((string)x.Tag == "diamante3")
                {
                    if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false; // se eliminan los diamantes
                        puntos3++;
                    }
                }
                if ((string)x.Tag == "diamante4")
                {
                    if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false; // se eliminan los diamantes
                        puntos4++;
                    }
                }
            }

            // Plataforma horizontal
            horizontal1.Left -= horizontalSpeed;
            if ((horizontal1.Left < 0) || (horizontal1.Left + horizontal1.Width > this.ClientSize.Width)) // Calculo dinámico
            {
                horizontalSpeed = -horizontalSpeed;
            }
            // Plataforma vertical
            if ((plataformaVertical1_activa1 == true) || (plataformaVertical1_activa2 == true) || (plataformaVertical1_activa3 == true) || (plataformaVertical1_activa4 == true))
            {
                //if (plataformaVertical1_final == false)
                //{
                vertical1.Top += verticalSpeed;
                if ((vertical1.Bounds.IntersectsWith(plataformaVertical1_Bot_limit.Bounds) || vertical1.Bounds.IntersectsWith(plataformaVertical1_Top_limit.Bounds)))
                {
                    //plataformaVertical1_final = true;
                    verticalSpeed = -verticalSpeed;
                }
                //}
            }

            // Traemos todas las placas delante de las plataformas
            placa1.BringToFront();
        }


        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                TeclaIzquierdaClicada();
            }
            if (e.KeyCode == Keys.Right)
            {
                TeclaDerechaClicada();
            }
            if (e.KeyCode == Keys.Up)
            {
                if ((jumping_J1 == false) || (jumping_J2 == false) || (jumping_J3 == false) || (jumping_J4 == false))
                {
                    if ((goLeft_J1 == true) || (goLeft_J2 == true) || (goLeft_J3 == true) || (goLeft_J4 == true))
                    {
                        TeclaArribaConIzquierdaClicada();
                    }
                    else if ((goRight_J1 == true) || (goRight_J2 == true) || (goRight_J3 == true) || (goRight_J4 == true))
                    {
                        TeclaArribaConDerechaClicada();
                    }
                    else
                    {
                        TeclaArribaSolaClicada();
                    }
                }
            }

        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            // Cuando levantemos las teclas, las booleanas se vuelven False
            if (e.KeyCode == Keys.Left)
            {
                TeclaIzquierdaDejadaDeClicar();
            }
            if (e.KeyCode == Keys.Right)
            {
                TeclaDerechaDejadaDeClicar();
            }
            if ((jumping_J1 == true) || (jumping_J2 == true) || (jumping_J3 == true) || (jumping_J4 == true))
            {
                TeclaArribaDejadaDeClicar();
            }
            //// Cuando le demos al enter y tengamos GameOver reempezamos
            //if (e.KeyCode == Keys.Enter && isGameOver_J1 == true)
            //{
            //    RestartGame();
            //}

        }
        private void RestartGame()
        {
            //miequipo.AddJugadorAEquipo(J1);
            //miequipo.AddJugadorAEquipo(J2);
            //miequipo.AddJugadorAEquipo(J3);

            //FlightPlan f = form.GetFlightFromForm(); // Metodo getter to get the information of the flightplan introduced
            //if (f != null)
            //{
            //    miEquipo.AddJugador(j)
            PictureBox J1 = CrearPictureBoxJugador(Jug1);
            PictureBox J2 = CrearPictureBoxJugador(Jug2);
            PictureBox J3 = CrearPictureBoxJugador(Jug3);
            PictureBox J4 = CrearPictureBoxJugador(Jug4);


            // Reiniciamos variables de los personajes
            goLeft_J1 = false; goRight_J1 = false; jumping_J1 = false; isGameOver_J1 = false; onPlatform_J1 = false;
            goLeft_J2 = false; goRight_J2 = false; jumping_J2 = false; isGameOver_J2 = false; onPlatform_J2 = false;
            goLeft_J3 = false; goRight_J3 = false; jumping_J3 = false; isGameOver_J3 = false; onPlatform_J3 = false;
            goLeft_J4 = false; goRight_J4 = false; jumping_J4 = false; isGameOver_J4 = false; onPlatform_J4 = false;

            jumpSpeed1 = jumpSpeed; jumpSpeed2 = jumpSpeed; jumpSpeed3 = jumpSpeed; jumpSpeed4 = jumpSpeed;
            puntos1 = 0; puntos2 = 0; puntos3 = 0; puntos4 = 0;


            permitirAnimaciones_J1 = true;
            permitirAnimaciones_J2 = true;
            permitirAnimaciones_J3 = true;
            permitirAnimaciones_J4 = true;


            // Plataformas
            vertical1.Width = 200 / 2;
            vertical1.Height = 50 / 2;
            vertical1.Image = Image.FromFile("plataforma1.png");
            vertical1.SizeMode = PictureBoxSizeMode.Zoom;
            vertical1.BackColor = Color.Transparent;
            // reseteo de las placas
            plataformaVertical1_activa1 = false; plataformaVertical1_activa2 = false; plataformaVertical1_activa3 = false; plataformaVertical1_activa4 = false;
            plataformaVertical1_final = false;
            placa1.Width = 120 / 2;
            placa1.Height = 36 / 2;
            placa1.Image = Image.FromFile("placa1_desactivada.png");
            placa1.SizeMode = PictureBoxSizeMode.Zoom;
            placa1.BackColor = Color.Transparent;

            foreach (Control x in this.Controls)
            {
                // techos   --> 1 por cada plataforma que no se pueda atravesar desde abajo.
                if ((string)x.Tag == "techo")
                {
                    x.Visible = false;
                }
                // Puntos
                if ((string)x.Tag == "diamante1")
                {   // Image + SizeMode.Zoom se ponen desde el diseño
                    x.Visible = true;
                    x.BackColor = Color.Transparent;
                    x.Width = 62 / 3;
                    x.Height = 58 / 3;
                }
                // Puntos
                if ((string)x.Tag == "diamante2")
                {   // Image + SizeMode.Zoom se ponen desde el diseño
                    x.Visible = true;
                    x.BackColor = Color.Transparent;
                    x.Width = 62 / 3;
                    x.Height = 58 / 3;
                }
                // Puntos
                if ((string)x.Tag == "diamante3")
                {   // Image + SizeMode.Zoom se ponen desde el diseño
                    x.Visible = true;
                    x.BackColor = Color.Transparent;
                    x.Width = 62 / 3;
                    x.Height = 58 / 3;
                }
                // Puntos
                if ((string)x.Tag == "diamante4")
                {   // Image + SizeMode.Zoom se ponen desde el diseño
                    x.Visible = true;
                    x.BackColor = Color.Transparent;
                    x.Width = 62 / 3;
                    x.Height = 58 / 3;
                }
                // Plataformas
                if ((string)x.Tag == "plataforma")
                {
                    x.BackgroundImage = Image.FromFile("plataforma_Mapa1.png");
                }

            }

        }


        // Animaciones y mensaje a enviar al mover MI PERSONAJE
        private void TeclaArribaSolaClicada()
        {
            switch (miPersonajeQueControlo)
            {
                case 1:

                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionSalto());
                    }
                    jumping_J1 = true;

                    break;
                case 2:
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionSalto());
                    }
                    jumping_J2 = true;
                    break;
                case 3:
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionSalto());
                    }
                    jumping_J3 = true;
                    break;
                case 4:
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionSalto());
                    }
                    jumping_J4 = true;
                    break;
            }
            string mensaje = "19/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        private void TeclaArribaConIzquierdaClicada()
        {
            switch (miPersonajeQueControlo)
            {
                case 1:
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionSalto_H_Izq());
                    }
                    jumping_J1 = true;
                    break;
                case 2:
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionSalto_H_Izq());
                    }
                    jumping_J2 = true;
                    break;
                case 3:
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionSalto_H_Izq());

                    }
                    jumping_J3 = true;
                    break;
                case 4:
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionSalto_H_Izq());

                    }
                    jumping_J4 = true;
                    break;
            }
            string mensaje = "21/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        private void TeclaArribaConDerechaClicada()
        {
            switch (miPersonajeQueControlo)
            {
                case 1:
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionSalto_H_Der());
                    }
                    jumping_J1 = true;
                    break;
                case 2:
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionSalto_H_Der());
                    }
                    jumping_J2 = true;
                    break;
                case 3:
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionSalto_H_Der());
                    }
                    jumping_J3 = true;
                    break;
                case 4:
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionSalto_H_Der());
                    }
                    jumping_J4 = true;
                    break;
            }
            string mensaje = "22/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        private void TeclaArribaDejadaDeClicar()
        {
            switch (miPersonajeQueControlo)
            {
                case 1:
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionCae());
                        if (onPlatform_J1 == true)
                        { misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionNormal()); }
                    }
                    jumping_J1 = false;
                    break;
                case 2:
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionCae());
                        if (onPlatform_J2 == true)
                        { misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionNormal()); }
                    }
                    jumping_J2 = false;
                    break;
                case 3:
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionCae());
                        if (onPlatform_J3 == true)
                        { misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionNormal()); }
                    }
                    jumping_J3 = false;
                    break;
                case 4:
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionCae());
                        if (onPlatform_J4 == true)
                        { misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionNormal()); }
                    }
                    jumping_J4 = false;
                    break;
            }
            string mensaje = "23/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        private void TeclaIzquierdaClicada()
        {
            switch (miPersonajeQueControlo)
            {
                case 1:
                    goLeft_J1 = true;
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionCorriendo_Izq());
                    }
                    break;
                case 2:
                    goLeft_J2 = true;
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionCorriendo_Izq());
                    }
                    break;
                case 3:
                    goLeft_J3 = true;
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionCorriendo_Izq());
                    }
                    break;
                case 4:
                    goLeft_J4 = true;
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionCorriendo_Izq());
                    }
                    break;
            }
            string mensaje = "15/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        private void TeclaIzquierdaDejadaDeClicar()
        {
            switch (miPersonajeQueControlo)
            {
                case 1:
                    goLeft_J1 = false;
                    if (permitirAnimaciones_J1 == true)
                    {

                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionNormal());
                    }
                    break;
                case 2:
                    goLeft_J2 = false;
                    if (permitirAnimaciones_J2 == true)
                    {

                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionNormal());
                    }
                    break;
                case 3:
                    goLeft_J3 = false;
                    if (permitirAnimaciones_J3 == true)
                    {

                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionNormal());
                    }
                    break;
                case 4:
                    goLeft_J4 = false;
                    if (permitirAnimaciones_J4 == true)
                    {

                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionNormal());
                    }
                    break;
            }
            string mensaje = "16/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        private void TeclaDerechaClicada()
        {
            switch (miPersonajeQueControlo)
            {
                case 1:
                    goRight_J1 = true;
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionCorriendo_Der());
                    }
                    break;
                case 2:
                    goRight_J2 = true;
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionCorriendo_Der());
                    }
                    break;
                case 3:
                    goRight_J3 = true;
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionCorriendo_Der());
                    }
                    break;
                case 4:
                    goRight_J4 = true;
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionCorriendo_Der());
                    }
                    break;
            }
            string mensaje = "17/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        private void TeclaDerechaDejadaDeClicar()
        {
            switch (miPersonajeQueControlo)
            {
                case 1:
                    goRight_J1 = false;
                    if (permitirAnimaciones_J1 == true)
                    {

                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionNormal());
                    }
                    break;
                case 2:
                    goRight_J2 = false;
                    if (permitirAnimaciones_J2 == true)
                    {

                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionNormal());
                    }
                    break;
                case 3:
                    goRight_J3 = false;
                    if (permitirAnimaciones_J3 == true)
                    {

                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionNormal());
                    }
                    break;
                case 4:
                    goRight_J4 = false;
                    if (permitirAnimaciones_J4 == true)
                    {

                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionNormal());
                    }
                    break;
            }
            string mensaje = "18/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            label_mensaje.Text = mensaje;
        }

        // Simulacion de control de los otros personajes
        // necesitan el número de Personaje
        public void TeclaArribaSolaClicada_Otro(int otropersonaje)
        {
            switch (otropersonaje)
            {
                case 1:
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionSalto());
                    }
                    jumping_J1 = true;
                    break;
                case 2:
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionSalto());
                    }
                    jumping_J2 = true;
                    break;
                case 3:
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionSalto());
                    }
                    jumping_J3 = true;
                    break;
                case 4:
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionSalto());
                    }
                    jumping_J4 = true;
                    break;
            }
            string mensaje = "19/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        public void TeclaArribaConIzquierdaClicada_Otro(int otropersonaje)
        {
            switch (otropersonaje)
            {
                case 1:
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionSalto_H_Izq());
                    }
                    jumping_J1 = true;
                    break;
                case 2:
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionSalto_H_Izq());
                    }
                    jumping_J2 = true;
                    break;
                case 3:
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionSalto_H_Izq());

                    }
                    jumping_J3 = true;
                    break;
                case 4:
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionSalto_H_Izq());

                    }
                    jumping_J4 = true;
                    break;
            }
            string mensaje = "21/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        public void TeclaArribaConDerechaClicada_Otro(int otropersonaje)
        {
            switch (otropersonaje)
            {
                case 1:
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionSalto_H_Der());
                    }
                    jumping_J1 = true;
                    break;
                case 2:
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionSalto_H_Der());
                    }
                    jumping_J2 = true;
                    break;
                case 3:
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionSalto_H_Der());
                    }
                    jumping_J3 = true;
                    break;
                case 4:
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionSalto_H_Der());
                    }
                    jumping_J4 = true;
                    break;
            }
            string mensaje = "22/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        public void TeclaArribaDejadaDeClicar_Otro(int otropersonaje)
        {
            switch (otropersonaje)
            {
                case 1:
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionCae());
                        if (onPlatform_J1 == true)
                        { misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionNormal()); }
                    }
                    jumping_J1 = false;
                    break;
                case 2:
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionCae());
                        if (onPlatform_J2 == true)
                        { misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionNormal()); }
                    }
                    jumping_J2 = false;
                    break;
                case 3:
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionCae());
                        if (onPlatform_J3 == true)
                        { misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionNormal()); }
                    }
                    jumping_J3 = false;
                    break;
                case 4:
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionCae());
                        if (onPlatform_J4 == true)
                        { misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionNormal()); }
                    }
                    jumping_J4 = false;
                    break;
            }
            string mensaje = "23/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        public void TeclaIzquierdaClicada_Otro(int otropersonaje)
        {
            switch (otropersonaje)
            {
                case 1:
                    goLeft_J1 = true;
                    if (permitirAnimaciones_J1 == true)
                    {
                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionCorriendo_Izq());
                    }
                    break;
                case 2:
                    goLeft_J2 = true;
                    if (permitirAnimaciones_J2 == true)
                    {
                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionCorriendo_Izq());
                    }
                    break;
                case 3:
                    goLeft_J3 = true;
                    if (permitirAnimaciones_J3 == true)
                    {
                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionCorriendo_Izq());
                    }
                    break;
                case 4:
                    goLeft_J4 = true;
                    if (permitirAnimaciones_J4 == true)
                    {
                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionCorriendo_Izq());
                    }
                    break;
            }
            string mensaje = "15/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        public void TeclaIzquierdaDejadaDeClicar_Otro(int otropersonaje)
        {
            switch (otropersonaje)
            {
                case 1:
                    goLeft_J1 = false;
                    if (permitirAnimaciones_J1 == true)
                    {

                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionNormal());
                    }
                    break;
                case 2:
                    goLeft_J2 = false;
                    if (permitirAnimaciones_J2 == true)
                    {

                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionNormal());
                    }
                    break;
                case 3:
                    goLeft_J3 = false;
                    if (permitirAnimaciones_J3 == true)
                    {

                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionNormal());
                    }
                    break;
                case 4:
                    goLeft_J4 = false;
                    if (permitirAnimaciones_J4 == true)
                    {

                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionNormal());
                    }
                    break;
            }
            string mensaje = "16/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        public void TeclaDerechaClicada_Otro(int otropersonaje)
        {
            switch (otropersonaje)
            {
                case 1:
                    goRight_J1 = true;
                    if (permitirAnimaciones_J1 == true)
                    {

                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionCorriendo_Der());
                    }
                    break;
                case 2:
                    goRight_J2 = true;
                    if (permitirAnimaciones_J2 == true)
                    {

                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionCorriendo_Der());
                    }
                    break;
                case 3:
                    goRight_J3 = true;
                    if (permitirAnimaciones_J3 == true)
                    {

                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionCorriendo_Der());
                    }
                    break;
                case 4:
                    goRight_J4 = true;
                    if (permitirAnimaciones_J4 == true)
                    {

                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionCorriendo_Der());
                    }
                    break;
            }
            string mensaje = "17/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server.Send(msg);
            label_mensaje.Text = mensaje;
        }
        public void TeclaDerechaDejadaDeClicar_Otro(int otropersonaje)
        {
            switch (otropersonaje)
            {
                case 1:
                    goRight_J1 = false;
                    if (permitirAnimaciones_J1 == true)
                    {

                        misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionNormal());
                    }
                    break;
                case 2:
                    goRight_J2 = false;
                    if (permitirAnimaciones_J2 == true)
                    {

                        misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionNormal());
                    }
                    break;
                case 3:
                    goRight_J3 = false;
                    if (permitirAnimaciones_J3 == true)
                    {

                        misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionNormal());
                    }
                    break;
                case 4:
                    goRight_J4 = false;
                    if (permitirAnimaciones_J4 == true)
                    {

                        misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionNormal());
                    }
                    break;
            }
            string mensaje = "18/" + idPartida + "/" + miPersonajeQueControlo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server.Send(msg);
            label_mensaje.Text = mensaje;
        }

        // Inserir los picturebox de los jugadores
        private PictureBox CrearPictureBoxJugador(Jugador j)
        {

            PictureBox p = new PictureBox();
            p.Width = j.GetAnchoNormal();
            p.Height = j.GetAlturaNormal();
            p.Location = new Point(Convert.ToInt32(j.GetX() - p.Width / 2), Convert.ToInt32(j.GetY() - p.Height / 2));
            p.SizeMode = PictureBoxSizeMode.Zoom;
            p.BackColor = Color.Transparent;
            p.Image = Image.FromFile(j.GetAnimacionNormal());
            p.Tag = "Jugador" + numPics;
            //p.Click += new System.EventHandler(this.ShowFlightInfo);
            this.Controls.Add(p); // se podrá cambiar por un panel1.Controls.Add(p);
            misPicsPersonajes.Add(p);
            numPics++;

            return p;

        }

        // Función para dentro del MainTimer, donde explica los movimientos de los personajes
        private void movimientosPersonajesConCadaTick()
        {
            // gravedad -->  solo si el personaje está en plataforma
            if (onPlatform_J1 == false)
            {
                Jug1.SetY(Jug1.GetY() + jumpSpeed1);
            }
            if (onPlatform_J2 == false)
            {
                Jug2.SetY(Jug2.GetY() + jumpSpeed2);
            }
            if (onPlatform_J3 == false)
            {
                Jug3.SetY(Jug3.GetY() + jumpSpeed3);
            }
            if (onPlatform_J4 == false)
            {
                Jug4.SetY(Jug4.GetY() + jumpSpeed4);
            }

            // Saltos del personaje 1
            if (jumping_J1 == true && force1 < 0) // contador del máximo del salto
            {
                jumping_J1 = false;
            }
            if (jumping_J1 == true)
            {
                onPlatform_J1 = false;
                jumpSpeed1 = -jumpSpeed;
                force1 -= 1;
            }
            else
            { jumpSpeed1 = jumpSpeed; };
            // Saltos del personaje 2
            if (jumping_J2 == true && force2 < 0) // contador del máximo del salto
            {
                jumping_J2 = false;
            }
            if (jumping_J2 == true)
            {
                onPlatform_J2 = false;
                jumpSpeed2 = -jumpSpeed;
                force2 -= 1;
            }
            else
            { jumpSpeed2 = jumpSpeed; };
            // Saltos del personaje 3
            if (jumping_J3 == true && force3 < 0) // contador del máximo del salto
            {
                jumping_J3 = false;
            }
            if (jumping_J3 == true)
            {
                onPlatform_J3 = false;
                jumpSpeed3 = -jumpSpeed;
                force3 -= 1;
            }
            else
            { jumpSpeed3 = jumpSpeed; };
            // Saltos del personaje 4
            if (jumping_J4 == true && force4 < 0) // contador del máximo del salto
            {
                jumping_J4 = false;
            }
            if (jumping_J4 == true)
            {
                onPlatform_J4 = false;
                jumpSpeed4 = -jumpSpeed;
                force4 -= 1;
            }
            else
            { jumpSpeed4 = jumpSpeed; };


            // Movimiento horizontal de los Jugadores
            // Personaje 1
            if (goLeft_J1 == true)
            { Jug1.SetX(Jug1.GetX() - hspeed); }
            if (goRight_J1 == true)
            { Jug1.SetX(Jug1.GetX() + hspeed); }
            // Personaje 2
            if (goLeft_J2 == true)
            { Jug2.SetX(Jug2.GetX() - hspeed); }
            if (goRight_J2 == true)
            { Jug2.SetX(Jug2.GetX() + hspeed); }
            // Personaje 3
            if (goLeft_J3 == true)
            { Jug3.SetX(Jug3.GetX() - hspeed); }
            if (goRight_J3 == true)
            { Jug3.SetX(Jug3.GetX() + hspeed); }
            // Personaje 4
            if (goLeft_J4 == true)
            { Jug4.SetX(Jug4.GetX() - hspeed); }
            if (goRight_J4 == true)
            { Jug4.SetX(Jug4.GetX() + hspeed); }
        }
        // Función para dentro del MainTimer, donde explica las colisiones con las plataformas
        private void ColisionesPersonajesPlataformas(Control x)
        {
            // Colisiones Jugador 1 - plataforma
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J1 == false)
                {
                    force1 = 4;
                    if (misPicsPersonajes[0].Top < x.Top)
                    {
                        onPlatform_J1 = true;
                        Jug1.SetY(x.Top - Jug1.GetAlturaNormal()); // mueve directamente arriba de la plataforma al saltar
                    }
                }
                else
                {  // Para asegurar que Jugador esté pegado a plataforma horizontal cuando no se mueva
                    if ((string)x.Name == "horizontal1" && (goLeft_J1 == false || goRight_J1 == false))
                    {
                        Jug1.SetX(Jug1.GetX() - horizontalSpeed); // se moverá con la velocidad de la plataforma
                    }
                }
            }

            // Colisiones Jugador 2 - plataforma
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J2 == false)
                {
                    force2 = 4;
                    if (misPicsPersonajes[1].Top < x.Top)
                    {
                        onPlatform_J2 = true;
                        Jug2.SetY(x.Top - Jug2.GetAlturaNormal()); // mueve directamente arriba de la plataforma al saltar
                    }
                }
                else
                {// Para asegurar que Jugador esté pegado a plataforma horizontal cuando no se mueva
                    if ((string)x.Name == "horizontal1" && (goLeft_J2 == false || goRight_J2 == false))
                    {
                        Jug2.SetX(Jug2.GetX() - horizontalSpeed); // se moverá con la velocidad de la plataforma
                    }
                }
            }
            // Colisiones Jugador 3 - plataforma
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J3 == false)
                {
                    force3 = 4;
                    if (misPicsPersonajes[2].Top < x.Top)
                    {
                        onPlatform_J3 = true;
                        Jug3.SetY(x.Top - Jug3.GetAlturaNormal()); // mueve directamente arriba de la plataforma al saltar
                    }
                }
                else
                {// Para asegurar que Jugador esté pegado a plataforma horizontal cuando no se mueva
                    if ((string)x.Name == "horizontal1" && (goLeft_J3 == false || goRight_J3 == false))
                    {
                        Jug3.SetX(Jug3.GetX() - horizontalSpeed); // se moverá con la velocidad de la plataforma
                    }
                }
            }
            // Colisiones Jugador 4 - plataforma
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J4 == false)
                {
                    force4 = 4;
                    if (misPicsPersonajes[3].Top < x.Top)
                    {
                        onPlatform_J4 = true;
                        Jug4.SetY(x.Top - Jug4.GetAlturaNormal()); // mueve directamente arriba de la plataforma al saltar
                    }
                }
                else
                {// Para asegurar que Jugador esté pegado a plataforma horizontal cuando no se mueva
                    if ((string)x.Name == "horizontal1" && (goLeft_J4 == false || goRight_J4 == false))
                    {
                        Jug4.SetX(Jug4.GetX() - horizontalSpeed); // se moverá con la velocidad de la plataforma
                    }
                }
            }
            x.BringToFront(); // plataformas delante de todo

        }
        private void ColisionesPersonajesPlaca1(Control x)
        {
            // Colisiones Jugador 1 - placa 1
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa1 == false)
                {
                    plataformaVertical1_activa1 = true;
                    placa1.Image = Image.FromFile("placa1_activada.png");
                }
            }
            else
            {
                if (plataformaVertical1_activa1 == true)
                {
                    plataformaVertical1_activa1 = false;
                    placa1.Image = Image.FromFile("placa1_desactivada.png");
                }
            }
            // Colisiones Jugador 2 - placa 1
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa2 == false)
                {
                    plataformaVertical1_activa2 = true;
                    placa1.Image = Image.FromFile("placa1_activada.png");
                }
            }
            else
            {
                if (plataformaVertical1_activa2 == true)
                {
                    plataformaVertical1_activa2 = false;
                    placa1.Image = Image.FromFile("placa1_desactivada.png");
                }
            }
            // Colisiones Jugador 3 - placa 1
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa3 == false)
                {
                    plataformaVertical1_activa3 = true;
                    placa1.Image = Image.FromFile("placa1_activada.png");
                }
            }
            else
            {
                if (plataformaVertical1_activa3 == true)
                {
                    plataformaVertical1_activa3 = false;
                    placa1.Image = Image.FromFile("placa1_desactivada.png");
                }
            }
            // Colisiones Jugador 4 - placa 1
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa4 == false)
                {
                    plataformaVertical1_activa4 = true;
                    placa1.Image = Image.FromFile("placa1_activada.png");
                }
            }
            else
            {
                if (plataformaVertical1_activa4 == true)
                {
                    plataformaVertical1_activa4 = false;
                    placa1.Image = Image.FromFile("placa1_desactivada.png");
                }

            }

        }
        private void ColisionesPersonajesTecho(Control x)
        {
            // Colisiones Jugador 1 - plataforma
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J1 == false)
                {
                    Jug1.SetY(x.Bottom); // mueve directamente abajo del techo al colisionar
                }
            }
            // Colisiones Jugador 2 - plataforma
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J2 == false)
                {
                    Jug2.SetY(x.Bottom); // mueve directamente abajo del techo al colisionar
                }
            }
            // Colisiones Jugador 3 - plataforma
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J3 == false)
                {
                    Jug3.SetY(x.Bottom); // mueve directamente abajo del techo al colisionar
                }
            }
            // Colisiones Jugador 4 - plataforma
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J4 == false)
                {
                    Jug4.SetY(x.Bottom); // mueve directamente abajo del techo al colisionar
                }
            }
        }
        private void ColisionesPersonajesParedIzquierda(Control x)
        {
            // Colisiones Jugador 1 - plataforma
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                Jug1.SetX(x.Left - Jug1.GetAnchoNormal()); // mueve directamente
            }
            // Colisiones Jugador 2 - plataforma
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                Jug2.SetX(x.Left - Jug2.GetAnchoNormal()); // mueve directamente
            }
            // Colisiones Jugador 3 - plataforma
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                Jug3.SetX(x.Left - Jug3.GetAnchoNormal()); // mueve directamente
            }
            // Colisiones Jugador 4 - plataforma
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                Jug4.SetX(x.Left - Jug4.GetAnchoNormal()); // mueve directamente
            }
        }
        private void ColisionesPersonajesParedDerecha(Control x)
        {
            // Colisiones Jugador 1 - plataforma
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                Jug1.SetX(x.Right); // mueve directamente
            }
            // Colisiones Jugador 2 - plataforma
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                Jug2.SetX(x.Right); // mueve directamente
            }
            // Colisiones Jugador 3 - plataforma
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                Jug3.SetX(x.Right); // mueve directamente
            }
            // Colisiones Jugador 4 - plataforma
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                Jug4.SetX(x.Right); // mueve directamente
            }
        }



        // funciones para inicializar este cliente
        public void MiPersonaje(int personaje)
        {
            this.miPersonajeQueControlo = personaje;
        }
    }
}
