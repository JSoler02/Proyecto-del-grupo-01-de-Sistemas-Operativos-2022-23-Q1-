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
    public partial class Templo4 : Form
    {
        int idPartida;
        Socket server; // declaramos socket

        string mapa = "Templo";
        // Mi personaje --> 1,2,3,4
        int miPersonajeQueControlo;

        // Variables bool para el envío de mensajes: flechas
        // para enviar solo 1 vez el mensaje
        bool tecla_arriba = true;
        bool tecla_derecha = true;
        bool tecla_izquierda = true;

        // Variables globales que se crean en False
        bool goLeft_J1, goRight_J1, jumping_J1, isGameOver_J1, onPlatform_J1;
        bool goLeft_J2, goRight_J2, jumping_J2, isGameOver_J2, onPlatform_J2;
        bool goLeft_J3, goRight_J3, jumping_J3, isGameOver_J3, onPlatform_J3;
        bool goLeft_J4, goRight_J4, jumping_J4, isGameOver_J4, onPlatform_J4;

        bool permitirAnimaciones_J1 = true;
        bool permitirAnimaciones_J2 = true;
        bool permitirAnimaciones_J3 = true;
        bool permitirAnimaciones_J4 = true;

        bool estaEnPuerta_J1; bool estaEnPuerta_J2; bool estaEnPuerta_J3; bool estaEnPuerta_J4;

        // Variables para el Jugador
        int jumpSpeed = 8; // velocidad vertical
        int jumpSpeed1; int jumpSpeed2; int jumpSpeed3; int jumpSpeed4; // --> una para cada uno para controlar el salto
        int force1; int force2; int force3; int force4; // contador para el maximo del salto
        int puntos1; int puntos2; int puntos3; int puntos4;
        int hspeed = 7; // velocidad horizontal --> general

        int vidas1; int vidas2; int vidas3; int vidas4;

        // Velocidades para las PLATAFORMAS verticales y horizontales
        int horizontalSpeed = 5;
        int verticalSpeed_v1 = 3; int verticalSpeed_v2 = 3;

        // Placa del tipo 1
        bool plataformaVertical1_activa1_1; bool plataformaVertical1_activa1_2; bool plataformaVertical1_activa2_1; bool plataformaVertical1_activa2_2;
        bool plataformaVertical1_activa3_1; bool plataformaVertical1_activa3_2; bool plataformaVertical1_activa4_1; bool plataformaVertical1_activa4_2;
        bool plataformaVertical1_ACT;
        bool plataformaVertical1_final;

        bool plataformaVertical2_activa;
        bool paredPalanca3_activa;

        // Velocidad para el ENEMIGO
        int enemy1Speed = 5;

        // Personajes/PictureBoxes
        //EquipoJugadores miequipo = new EquipoJugadores();
        List<PictureBox> misPicsPersonajes = new List<PictureBox>();
        int numPics = 1;

        Jugador Jug1 = new Jugador(1, 30, 200);
        Jugador Jug2 = new Jugador(2, 100, 200);
        Jugador Jug3 = new Jugador(3, 225, 200);
        Jugador Jug4 = new Jugador(4, 300, 200);
        // variables del tiempo
        int segundos;

        private void tiempoJuego_Tick(object sender, EventArgs e)
        {
            segundos++;
            label_tiempo.Text = "Tiempo: " + segundos;
        }

        // funciones para inicializar este cliente
        public void MiPersonaje(int personaje)
        {
            this.miPersonajeQueControlo = personaje;
        }

        public void SetJug1Nombre(string name)
        { this.Jug1.SetNombre(name); }
        public void SetJug2Nombre(string name)
        { this.Jug2.SetNombre(name); }
        public void SetJug3Nombre(string name)
        { this.Jug3.SetNombre(name); }
        public void SetJug4Nombre(string name)
        { this.Jug4.SetNombre(name); }
        public Templo4(int idPartida, Socket server)
        {
            InitializeComponent();
            this.idPartida = idPartida; // num de form que em donen --> l'afegeixo en els missatges de peticio de servei
            this.server = server;
        }

        private void Controles1Personaje_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            RestartGame();
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
            lblPuntos.Text = "Puntos J1: " + puntos1 + "\nPuntos J2: " + puntos2 + "\nPuntos J3: " + puntos3 + "\nPuntos J4: " + puntos4;
            label_estaEnPuerta.Text = "J1 en puerta: " + estaEnPuerta_J1 + "\nJ2 en puerta: " + estaEnPuerta_J2 + "\nJ3 en puerta: " + estaEnPuerta_J3 + "\nJ4 en puerta: " + estaEnPuerta_J4;

            pintarPersonajesEnSusPosiciones();
            movimientosPersonajesConCadaTick();

            // Control de los Picturebox
            foreach (Control x in this.Controls)
            {
                // Plataformas
                if ((string)x.Tag == "plataforma")
                {
                    ColisionesPersonajesPlataformas(x);
                    //NoColisionesPersonajesPlataformas();
                }
                // aire = no plataformas
                if ((string)x.Tag == "aire")
                {
                    ColisionesPersonajesAire(x);
                }
                // placas
                if ((string)x.Tag == "placa1_1")
                {
                    ColisionesPersonajesPlaca1_1(x);
                }
                if ((string)x.Tag == "placa1_2")
                {
                    ColisionesPersonajesPlaca1_2(x);
                }
                // palanca
                if ((string)x.Tag == "palanca2")
                {
                    ColisionesPersonajesPalanca2(x);
                }
                if ((string)x.Tag == "palanca3")
                {
                    ColisionesPersonajesPalanca3(x);
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
                // portal 1
                if ((string)x.Tag == "portal1")
                {
                    ColisionesPersonajesPortal1(x);
                }
                // portal 2
                if ((string)x.Tag == "portal2")
                {
                    ColisionesPersonajesPortal2(x);
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
                // enemigos
                if ((string)x.Tag == "acido")
                {
                    ColisionesPersonajesAcido(x);
                }
                // puerta final
                if ((string)x.Tag == "puerta")
                {
                    ColisionesPersonajesPuertas(x);
                }

            }

            // Plataforma horizontal
            horizontal1.Left -= horizontalSpeed;
            if ((horizontal1.Left < 0) || (horizontal1.Left + horizontal1.Width > this.ClientSize.Width)) // Calculo dinámico
            {
                horizontalSpeed = -horizontalSpeed;
            }
            // Plataforma vertical 1
            if ((plataformaVertical1_activa1_1 || plataformaVertical1_activa1_2) || (plataformaVertical1_activa2_1 || plataformaVertical1_activa2_2) || (plataformaVertical1_activa3_1 || plataformaVertical1_activa3_2) || (plataformaVertical1_activa4_1 || plataformaVertical1_activa4_2))
            { plataformaVertical1_ACT = true; }
            else
            { plataformaVertical1_ACT = false; }
            if (plataformaVertical1_ACT)
            {
                placa1_1.Image = Image.FromFile("placa1_activada.png");
                placa1_2.Image = Image.FromFile("placa1_activada.png");
                //if (plataformaVertical1_final == false)
                //{
                vertical1.Top += verticalSpeed_v1;
                if ((vertical1.Bounds.IntersectsWith(plataformaVertical1_Bot_limit.Bounds) || vertical1.Bounds.IntersectsWith(plataformaVertical1_Top_limit.Bounds)))
                {
                    //plataformaVertical1_final = true;
                    verticalSpeed_v1 = -verticalSpeed_v1;
                }
                //}
            }
            else
            {
                placa1_1.Image = Image.FromFile("placa1_desactivada.png");
                placa1_2.Image = Image.FromFile("placa1_desactivada.png");
            }
            // Plataforma vertical 2
            if (plataformaVertical2_activa == true)
            {
                vertical2.Top += verticalSpeed_v2;
                if ((vertical2.Bounds.IntersectsWith(plataformaVertical2_Bot_limit.Bounds) || vertical2.Bounds.IntersectsWith(plataformaVertical2_Top_limit.Bounds)))
                {
                    //plataformaVertical1_final = true;
                    verticalSpeed_v2 = -verticalSpeed_v2;
                }
                //}
            }
            // Pared Palanca 3
            if (paredPalanca3_activa == true)
            {
                while (paredPalanca3.Height > 1)    // se parará el timer para hacer esta acción
                {
                    paredPalanca3.Height = paredPalanca3.Height - 1;
                }
                if (paredPalanca3.Height <= 1)
                { this.Controls.Remove(paredPalanca3); }
            }

            // Todos los jugadores en las puertas
            // Solo el jugador 1 envía el mensaje al servidor para que este le
            // devuelva el mensaje de fin de partida con el panel de las estadísticas y el resultado de la partida
            if (estaEnPuerta_J1 == true && estaEnPuerta_J2 == true && estaEnPuerta_J3 == true && estaEnPuerta_J4 == true)
            {
                string letra_miResultado = CalcularPuntosTotales();
                EnvíoMensajeFinDePartida("SUPERADO", letra_miResultado);
                // Quitar esta función cuando esté ya trabajada. De momento está aquí para trabajarla.
                // parámetros sacados de la seleccion de partida y del MENSAJE RECIBIDO
                // FinDePartida("SUPERADO", letra_miResultado);
            }

            if (isGameOver_J1 == true || isGameOver_J2 == true || isGameOver_J3 == true || isGameOver_J4 == true)
            {
                if (isGameOver_J1 == true && miPersonajeQueControlo == 1)
                { EnvíoMensajeFinDePartida("No Superado", "F"); }
                else if (isGameOver_J2 == true && miPersonajeQueControlo == 2)
                { EnvíoMensajeFinDePartida("No Superado", "F"); }
                else if (isGameOver_J3 == true && miPersonajeQueControlo == 3)
                { EnvíoMensajeFinDePartida("No Superado", "F"); }
                else if (isGameOver_J4 == true && miPersonajeQueControlo == 4)
                { EnvíoMensajeFinDePartida("No Superado", "F"); }
            }

        }


        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                //if ((jumping_J1 == true) || (jumping_J2 == true) || (jumping_J3 == true) || (jumping_J4 == true))
                //{
                if (e.KeyCode == Keys.Up)
                {
                    TeclaArribaConIzquierdaClicada();
                }
                //}
                else
                {
                    TeclaIzquierdaClicada();
                }
            }
            if (e.KeyCode == Keys.Right)
            {
                //if ((jumping_J1 == true) || (jumping_J2 == true) || (jumping_J3 == true) || (jumping_J4 == true))
                //{
                if (e.KeyCode == Keys.Up)
                {
                    TeclaArribaConDerechaClicada();
                }
                //}
                else
                {
                    TeclaDerechaClicada();
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                //if ((jumping_J1 == false) || (jumping_J2 == false) || (jumping_J3 == false) || (jumping_J4 == false))
                //{
                if (e.KeyCode == Keys.Left)//((goLeft_J1 == true) || (goLeft_J2 == true) || (goLeft_J3 == true) || (goLeft_J4 == true))
                {
                    TeclaArribaConIzquierdaClicada();
                }
                else if (e.KeyCode == Keys.Right)//((goRight_J1 == true) || (goRight_J2 == true) || (goRight_J3 == true) || (goRight_J4 == true))
                {
                    TeclaArribaConDerechaClicada();
                }
                else
                {
                    TeclaArribaSolaClicada();
                }
                //}
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
            if (e.KeyCode == Keys.Up)
            {
                TeclaArribaDejadaDeClicar();
            }
        }
        private void RestartGame()
        {
           
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
            vidas1 = 3; vidas2 = 3; vidas3 = 3; vidas4 = 3;

            permitirAnimaciones_J1 = true;
            permitirAnimaciones_J2 = true;
            permitirAnimaciones_J3 = true;
            permitirAnimaciones_J4 = true;

            segundos = 0;

            // Plataformas
            // plataforma vertical 1
            vertical1.Width = 200 / 2;
            vertical1.Height = 50 / 2;
            vertical1.Image = Image.FromFile("plataforma1.png");
            vertical1.SizeMode = PictureBoxSizeMode.Zoom;
            vertical1.BackColor = Color.Transparent;
            // plataforma vertical 2
            vertical2.Width = 200 / 2;
            vertical2.Height = 50 / 2;
            vertical2.Image = Image.FromFile("plataforma2.png");
            vertical2.SizeMode = PictureBoxSizeMode.Zoom;
            vertical2.BackColor = Color.Transparent;
            // pared palanca 3
            paredPalanca3.Height = 200 / 2;
            paredPalanca3.Width = 50 / 2;
            paredPalanca3.Image = Image.FromFile("plataforma3.png");
            paredPalanca3.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            paredPalanca3.SizeMode = PictureBoxSizeMode.Zoom;
            paredPalanca3.BackColor = Color.Transparent;

            // reseteo de las placas
            placa1_1.Width = 120 / 2;
            placa1_1.Height = 36 / 2;
            placa1_1.Image = Image.FromFile("placa1_desactivada.png");
            placa1_1.SizeMode = PictureBoxSizeMode.Zoom;
            placa1_1.BackColor = Color.Transparent;
            placa1_2.Width = 120 / 2;
            placa1_2.Height = 36 / 2;
            placa1_2.Image = Image.FromFile("placa1_desactivada.png");
            placa1_2.SizeMode = PictureBoxSizeMode.Zoom;
            placa1_2.BackColor = Color.Transparent;
            // reseteo de las palancas
            // palanca 2
            plataformaVertical2_activa = false;
            palanca2.Width = 30;
            palanca2.Height = 35;
            palanca2.Image = Image.FromFile("palanca2_desactivada.png");
            palanca2.SizeMode = PictureBoxSizeMode.Zoom;
            palanca2.BackColor = Color.Transparent;
            // palanca 2
            paredPalanca3_activa = false;
            palanca3.Width = 35;
            palanca3.Height = 30;
            palanca3.Image = Image.FromFile("palanca3_desactivada.png");
            palanca3.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            palanca3.SizeMode = PictureBoxSizeMode.Zoom;
            palanca3.BackColor = Color.Transparent;
            // Portales
            // Portal 1
            PictureBox portal1_imagen = new PictureBox();
            portal1_imagen.Width = 45;
            portal1_imagen.Height = 80;
            portal1_imagen.Location = new Point(portal1_inicio_hitbox.Location.X - 45 / 4, portal1_inicio_hitbox.Location.Y - 80 / 4);
            portal1_imagen.Image = Image.FromFile("portal.gif");
            portal1_imagen.SizeMode = PictureBoxSizeMode.Zoom;
            portal1_imagen.BackColor = Color.Transparent;
            this.Controls.Add(portal1_imagen);

            portal1_final.Width = 50;
            portal1_final.Height = 50;
            portal1_final.Image = Image.FromFile("portal_salida.gif");
            portal1_final.SizeMode = PictureBoxSizeMode.Zoom;
            portal1_final.BackColor = Color.Transparent;

            // Portal 2
            PictureBox portal2_imagen = new PictureBox();
            portal2_imagen.Width = 45;
            portal2_imagen.Height = 80;
            portal2_imagen.Location = new Point(portal2_inicio_hitbox.Location.X - 45 / 4, portal2_inicio_hitbox.Location.Y - 80 / 4);
            portal2_imagen.Image = Image.FromFile("portal.gif");
            portal2_imagen.SizeMode = PictureBoxSizeMode.Zoom;
            portal2_imagen.BackColor = Color.Transparent;
            this.Controls.Add(portal2_imagen);

            portal2_final.Width = 50;
            portal2_final.Height = 50;
            portal2_final.Image = Image.FromFile("portal_salida.gif");
            portal2_final.SizeMode = PictureBoxSizeMode.Zoom;
            portal2_final.BackColor = Color.Transparent;

            foreach (Control x in this.Controls)
            {
                // techos   --> 1 por cada plataforma que no se pueda atravesar desde abajo.
                if ((string)x.Tag == "techo")
                {
                    x.Visible = false;
                }
                if ((string)x.Tag == "pared_izquierda")
                {
                    x.Visible = false;
                }
                if ((string)x.Tag == "pared_derecha")
                {
                    x.Visible = false;
                }
                if ((string)x.Tag == "aire")
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
                    x.BringToFront(); // plataformas delante de todo
                }
                if ((string)x.Tag == "vidas")
                {
                    x.Width = 99;
                    x.Height = 30;
                }
                if ((string)x.Tag == "puerta")
                {
                    x.Width = 45;
                    x.Height = 80;
                }

            }
            // Vidas --> imagen inicial
            vidasJ1_pb.BackgroundImage = Image.FromFile("vidas_fire_3.png");
            vidasJ1_pb.BackColor = Color.Transparent;
            vidasJ2_pb.BackgroundImage = Image.FromFile("vidas_water_3.png");
            vidasJ2_pb.BackColor = Color.Transparent;
            vidasJ3_pb.BackgroundImage = Image.FromFile("vidas_rock_3.png");
            vidasJ3_pb.BackColor = Color.Transparent;
            vidasJ4_pb.BackgroundImage = Image.FromFile("vidas_cloud_3.png");
            vidasJ4_pb.BackColor = Color.Transparent;
            // Posiciones de los picturebox de las vidas
            vidasJ1_pb.Top = 5;
            vidasJ1_pb.Left = 5;
            vidasJ2_pb.Top = vidasJ1_pb.Bottom + 2;
            vidasJ2_pb.Left = vidasJ1_pb.Left;
            vidasJ3_pb.Top = vidasJ2_pb.Bottom;
            vidasJ3_pb.Left = vidasJ1_pb.Left;
            vidasJ4_pb.Top = vidasJ3_pb.Bottom;
            vidasJ4_pb.Left = vidasJ1_pb.Left;

            // puertas
            puertaJ1.Image = Image.FromFile("puerta_fire.png");
            puertaJ2.Image = Image.FromFile("puerta_water.png");
            puertaJ3.Image = Image.FromFile("puerta_rock.png");
            puertaJ4.Image = Image.FromFile("puerta_cloud.png");
            puertaJ1.BackColor = Color.Transparent;
            puertaJ1.SendToBack();
            puertaJ2.BackColor = Color.Transparent;
            puertaJ2.SendToBack();
            puertaJ3.BackColor = Color.Transparent;
            puertaJ3.SendToBack();
            puertaJ4.BackColor = Color.Transparent;
            puertaJ4.SendToBack();

            // Traemos todas las placas y palancas delante de las plataformas
            placa1_1.BringToFront();
            placa1_2.BringToFront();
            palanca2.BringToFront();
            palanca3.BringToFront();
            portal1_final.SendToBack();
            portal1_imagen.SendToBack();
            portal2_final.SendToBack();
            portal2_imagen.SendToBack();
            // Eliminamos visibilidades extras
            portal1_inicio_hitbox.Visible = false;
            portal2_inicio_hitbox.Visible = false;

            label_mensaje.Visible = false;
            label_estaEnPuerta.Visible = false;
            lbl_conquePlat_contacto.Visible = false;
            lbl_noColisionesPlataforma.Visible = false;
        }


        // Animaciones y mensaje a enviar al mover MI PERSONAJE
        private void TeclaArribaSolaClicada()
        {
            if (tecla_arriba == true)
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

                string mensaje = "19/" + idPartida + "/" + mapa + "/" + miPersonajeQueControlo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                label_mensaje.Text = mensaje;
                tecla_arriba = false;
            }
        }
        private void TeclaArribaConIzquierdaClicada()
        {
            if (tecla_arriba == true && tecla_izquierda == true)
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

                string mensaje = "21/" + idPartida + "/" + mapa + "/" + miPersonajeQueControlo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                label_mensaje.Text = mensaje;
                tecla_arriba = false;
                tecla_izquierda = false;
            }
        }
        private void TeclaArribaConDerechaClicada()
        {
            if (tecla_arriba == true && tecla_derecha == true)
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

                string mensaje = "22/" + idPartida + "/" + mapa + "/" + miPersonajeQueControlo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                label_mensaje.Text = mensaje;
                tecla_arriba = false;
                tecla_derecha = false;
            }
        }
        private void TeclaArribaDejadaDeClicar()
        {
            if (tecla_arriba == false)
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
                string mensaje = "23/" + idPartida + "/" + mapa + "/" + miPersonajeQueControlo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                label_mensaje.Text = mensaje;
                tecla_arriba = true;
            }
        }
        private void TeclaIzquierdaClicada()
        {
            if (tecla_izquierda == true)
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

                string mensaje = "15/" + idPartida + "/" + mapa + "/" + miPersonajeQueControlo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                label_mensaje.Text = mensaje;
                tecla_izquierda = false;
            }
        }
        private void TeclaIzquierdaDejadaDeClicar()
        {
            if (tecla_izquierda == false)
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
                string mensaje = "16/" + idPartida + "/" + mapa + "/" + miPersonajeQueControlo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                label_mensaje.Text = mensaje;
                tecla_izquierda = true;
            }
        }
        private void TeclaDerechaClicada()
        {
            if (tecla_derecha == true)
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

                string mensaje = "17/" + idPartida + "/" + mapa + "/" + miPersonajeQueControlo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                label_mensaje.Text = mensaje;
                tecla_derecha = false;
            }
        }
        private void TeclaDerechaDejadaDeClicar()
        {
            if (tecla_derecha == false)
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
                string mensaje = "18/" + idPartida + "/" + mapa + "/" + miPersonajeQueControlo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                label_mensaje.Text = mensaje;
                tecla_derecha = true;
            }
        }

        // Simulacion de control de los otros personajes
        // necesitan el número de Personaje
        public void TeclaArribaSolaClicada_Otro(int otropersonaje)
        {
            Invoke(new Action(() =>
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
            }));
        }
        public void TeclaArribaConIzquierdaClicada_Otro(int otropersonaje)
        {
            Invoke(new Action(() =>
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
            }));
        }
        public void TeclaArribaConDerechaClicada_Otro(int otropersonaje)
        {
            Invoke(new Action(() =>
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
            }));
        }
        public void TeclaArribaDejadaDeClicar_Otro(int otropersonaje)
        {
            Invoke(new Action(() =>
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
            }));
        }
        public void TeclaIzquierdaClicada_Otro(int otropersonaje)
        {
            Invoke(new Action(() =>
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
            }));
        }
        public void TeclaIzquierdaDejadaDeClicar_Otro(int otropersonaje)
        {
            Invoke(new Action(() =>
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
            }));
        }
        public void TeclaDerechaClicada_Otro(int otropersonaje)
        {
            Invoke(new Action(() =>
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
            }));
        }
        public void TeclaDerechaDejadaDeClicar_Otro(int otropersonaje)
        {
            Invoke(new Action(() =>
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
            }));
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
                    // Para asegurar que Jugador esté pegado a la plataforma vertical
                    if ((string)x.Name == "vertical2")
                    {
                        Jug1.SetY(vertical2.Top - Jug1.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical1")
                    {
                        Jug1.SetY(vertical1.Top - Jug1.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical4")
                    {
                        Jug1.SetY(vertical1.Top - Jug1.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical5")
                    {
                        Jug1.SetY(vertical1.Top - Jug1.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                }
            }
            //else
            //{
            //    if ((onPlatform_J1 == true) && ((string)x.Name == "horizontal1" || (string)x.Name == "vertical1" || (string)x.Name == "vertical2" || (string)x.Name == "pictureBox1" || (string)x.Name == "pictureBox2" || (string)x.Name == "pictureBox6" || (string)x.Name == "pictureBox15" || (string)x.Name == "pictureBox17" || (string)x.Name == "pictureBox18"))
            //    {
            //        onPlatform_J1 = false;
            //    }
            //}

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
                    // Para asegurar que Jugador esté pegado a la plataforma vertical
                    if ((string)x.Name == "vertical2")
                    {
                        Jug2.SetY(vertical2.Top - Jug2.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical1")
                    {
                        Jug2.SetY(vertical1.Top - Jug2.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical4")
                    {
                        Jug2.SetY(vertical1.Top - Jug2.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical5")
                    {
                        Jug2.SetY(vertical1.Top - Jug2.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
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
                    // Para asegurar que Jugador esté pegado a la plataforma vertical
                    if ((string)x.Name == "vertical2")
                    {
                        Jug3.SetY(vertical2.Top - Jug3.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical1")
                    {
                        Jug3.SetY(vertical1.Top - Jug3.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical4")
                    {
                        Jug3.SetY(vertical1.Top - Jug2.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical5")
                    {
                        Jug3.SetY(vertical1.Top - Jug2.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                }
            }
            // Colisiones Jugador 4 - plataforma
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
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
                    // Para asegurar que Jugador esté pegado a la plataforma vertical
                    if ((string)x.Name == "vertical2")
                    {
                        Jug4.SetY(vertical2.Top - Jug4.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical1")
                    {
                        Jug4.SetY(vertical1.Top - Jug4.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical4")
                    {
                        Jug4.SetY(vertical1.Top - Jug4.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                    if ((string)x.Name == "vertical5")
                    {
                        Jug4.SetY(vertical1.Top - Jug4.GetAlturaNormal()); // se moverá con la velocidad de la plataforma
                    }
                }
            }
        }
        private void ColisionesPersonajesPlaca1_1(Control x)
        {
            // Colisiones Jugador 1 - placa 1
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa1_1 == false)
                {
                    plataformaVertical1_activa1_1 = true;
                }
            }
            else
            {
                if (plataformaVertical1_activa1_1 == true)
                {
                    plataformaVertical1_activa1_1 = false;
                }
            }
            // Colisiones Jugador 2 - placa 1
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa2_1 == false)
                {
                    plataformaVertical1_activa2_1 = true;

                }
            }
            else
            {
                if (plataformaVertical1_activa2_1 == true)
                {
                    plataformaVertical1_activa2_1 = false;
                }
            }
            // Colisiones Jugador 3 - placa 1
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa3_1 == false)
                {
                    plataformaVertical1_activa3_1 = true;
                }
            }
            else
            {
                if (plataformaVertical1_activa3_1 == true)
                {
                    plataformaVertical1_activa3_1 = false;
                }
            }
            // Colisiones Jugador 4 - placa 1
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa4_1 == false)
                {
                    plataformaVertical1_activa4_1 = true;
                }
            }
            else
            {
                if (plataformaVertical1_activa4_1 == true)
                {
                    plataformaVertical1_activa4_1 = false;
                }

            }

        }
        private void ColisionesPersonajesPlaca1_2(Control x)
        {
            // Colisiones Jugador 1 - placa 1
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa1_2 == false)
                {
                    plataformaVertical1_activa1_2 = true;
                }
            }
            else
            {
                if (plataformaVertical1_activa1_2 == true)
                {
                    plataformaVertical1_activa1_2 = false;
                }
            }
            // Colisiones Jugador 2 - placa 1
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa2_2 == false)
                {
                    plataformaVertical1_activa2_2 = true;

                }
            }
            else
            {
                if (plataformaVertical1_activa2_2 == true)
                {
                    plataformaVertical1_activa2_2 = false;
                }
            }
            // Colisiones Jugador 3 - placa 1
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa3_2 == false)
                {
                    plataformaVertical1_activa3_2 = true;
                }
            }
            else
            {
                if (plataformaVertical1_activa3_2 == true)
                {
                    plataformaVertical1_activa3_2 = false;
                }
            }
            // Colisiones Jugador 4 - placa 1
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical1_activa4_2 == false)
                {
                    plataformaVertical1_activa4_2 = true;
                }
            }
            else
            {
                if (plataformaVertical1_activa4_2 == true)
                {
                    plataformaVertical1_activa4_2 = false;
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
                jumping_J1 = false;
            }
            // Colisiones Jugador 2 - plataforma
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J2 == false)
                {
                    Jug2.SetY(x.Bottom); // mueve directamente abajo del techo al colisionar
                }
                jumping_J2 = false;

            }
            // Colisiones Jugador 3 - plataforma
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J3 == false)
                {
                    Jug3.SetY(x.Bottom); // mueve directamente abajo del techo al colisionar
                }
                jumping_J3 = false;

            }
            // Colisiones Jugador 4 - plataforma
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J4 == false)
                {
                    Jug4.SetY(x.Bottom); // mueve directamente abajo del techo al colisionar
                }
                jumping_J4 = false;

            }
        }
        private void ColisionesPersonajesParedIzquierda(Control x)
        {
            // Colisiones Jugador 1 - plataforma
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                Jug1.SetX(x.Left - Jug1.GetAnchoNormal()); // mueve directamente
                goLeft_J1 = false;
            }
            // Colisiones Jugador 2 - plataforma
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                Jug2.SetX(x.Left - Jug2.GetAnchoNormal()); // mueve directamente
                goLeft_J2 = false;
            }
            // Colisiones Jugador 3 - plataforma
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                Jug3.SetX(x.Left - Jug3.GetAnchoNormal()); // mueve directamente
                goLeft_J3 = false;
            }
            // Colisiones Jugador 4 - plataforma
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                Jug4.SetX(x.Left - Jug4.GetAnchoNormal()); // mueve directamente
                goLeft_J4 = false;
            }
        }
        private void ColisionesPersonajesParedDerecha(Control x)
        {
            // Colisiones Jugador 1 - plataforma
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                Jug1.SetX(x.Right); // mueve directamente
                goRight_J1 = false;
            }
            // Colisiones Jugador 2 - plataforma
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                Jug2.SetX(x.Right); // mueve directamente
                goRight_J2 = false;
            }
            // Colisiones Jugador 3 - plataforma
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                Jug3.SetX(x.Right); // mueve directamente
                goRight_J3 = false;
            }
            // Colisiones Jugador 4 - plataforma
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                Jug4.SetX(x.Right); // mueve directamente
                goRight_J4 = false;
            }
        }

        private void abandonar_but_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                EnvíoMensajeFinDePartida("No Superado", "F");
            }));
        }

        private void abandonar_label_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                EnvíoMensajeFinDePartida("No Superado", "F");
            }));
        }

        private void ColisionesPersonajesPalanca2(Control x)
        {
            // Colisiones Jugador 1 - palanca 2
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical2_activa == false)
                {
                    plataformaVertical2_activa = true;
                    palanca2.Image = Image.FromFile("palanca2_activada.png");
                }
            }

            // Colisiones Jugador 2 - palanca 2
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical2_activa == false)
                {
                    plataformaVertical2_activa = true;
                    palanca2.Image = Image.FromFile("palanca2_activada.png");
                }
            }

            // Colisiones Jugador 3 - palanca 2
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical2_activa == false)
                {
                    plataformaVertical2_activa = true;
                    palanca2.Image = Image.FromFile("palanca2_activada.png");
                }
            }
            // Colisiones Jugador 4 - palanca 2
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if (plataformaVertical2_activa == false)
                {
                    plataformaVertical2_activa = true;
                    palanca2.Image = Image.FromFile("palanca2_activada.png");
                }
            }

        }
        private void ColisionesPersonajesPalanca3(Control x)
        {
            // Colisiones Jugador 1 - palanca 3
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if (paredPalanca3_activa == false)
                {
                    paredPalanca3_activa = true;
                    palanca3.Image = Image.FromFile("palanca3_activada.png");
                    palanca3.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
            }

            // Colisiones Jugador 2 - palanca 3
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (paredPalanca3_activa == false)
                {
                    paredPalanca3_activa = true;
                    palanca3.Image = Image.FromFile("palanca3_activada.png");
                    palanca3.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
            }

            // Colisiones Jugador 3 - palanca 3
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if (paredPalanca3_activa == false)
                {
                    paredPalanca3_activa = true;
                    palanca3.Image = Image.FromFile("palanca3_activada.png");
                    palanca3.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
            }
            // Colisiones Jugador 4 - palanca 3
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if (paredPalanca3_activa == false)
                {
                    paredPalanca3_activa = true;
                    palanca3.Image = Image.FromFile("palanca3_activada.png");
                    palanca3.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
            }

        }
        private void ColisionesPersonajesPortal1(Control x)
        {
            // Colisiones Jugador 1 - portal 1
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if ((string)x.Name == "portal1_inicio_hitbox")
                {
                    Jug1.SetX(portal1_final.Left);
                    Jug1.SetY(portal1_final.Top);
                    onPlatform_J1 = false;
                }
            }
            // Colisiones Jugador 2 - portal 1
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if ((string)x.Name == "portal1_inicio_hitbox")
                {
                    Jug2.SetX(portal1_final.Left);
                    Jug2.SetY(portal1_final.Top);
                    onPlatform_J2 = false;
                }
            }
            // Colisiones Jugador 3 - portal 1
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if ((string)x.Name == "portal1_inicio_hitbox")
                {
                    Jug3.SetX(portal1_final.Left);
                    Jug3.SetY(portal1_final.Top);
                    onPlatform_J3 = false;
                }
            }
            // Colisiones Jugador 4 - portal 1
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if ((string)x.Name == "portal1_inicio_hitbox")
                {
                    Jug4.SetX(portal1_final.Left);
                    Jug4.SetY(portal1_final.Top);
                    onPlatform_J4 = false;
                }
            }
        }
        private void ColisionesPersonajesPortal2(Control x)
        {
            // Colisiones Jugador 1 - portal 2
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if ((string)x.Name == "portal2_inicio_hitbox")
                {
                    Jug1.SetX(portal2_final.Left);
                    Jug1.SetY(portal2_final.Top);
                    onPlatform_J1 = false;
                }
            }
            // Colisiones Jugador 2 - portal 2
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if ((string)x.Name == "portal2_inicio_hitbox")
                {
                    Jug2.SetX(portal2_final.Left);
                    Jug2.SetY(portal2_final.Top);
                    onPlatform_J2 = false;
                }
            }
            // Colisiones Jugador 3 - portal 2
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if ((string)x.Name == "portal2_inicio_hitbox")
                {
                    Jug3.SetX(portal2_final.Left);
                    Jug3.SetY(portal2_final.Top);
                    onPlatform_J3 = false;
                }
            }
            // Colisiones Jugador 4 - portal 2
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if ((string)x.Name == "portal2_inicio_hitbox")
                {
                    Jug4.SetX(portal2_final.Left);
                    Jug4.SetY(portal2_final.Top);
                    onPlatform_J4 = false;
                }
            }
        }
        private void ColisionesPersonajesAcido(Control x)
        {
            // Jugador 1
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
            {
                if (permitirAnimaciones_J1 == true)
                {
                    misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionDerrotado());
                }
                permitirAnimaciones_J1 = false;

                J1derrotado.Start();
            }
            // Jugador 2
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (permitirAnimaciones_J2 == true)
                {
                    misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionDerrotado());
                }
                permitirAnimaciones_J2 = false;

                J2derrotado.Start();
            }
            // Jugador 3
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
            {
                if (permitirAnimaciones_J3 == true)
                {
                    misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionDerrotado());
                }
                permitirAnimaciones_J3 = false;

                J3derrotado.Start();
            }
            // Jugador 4
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
            {
                if (permitirAnimaciones_J4 == true)
                {
                    misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionDerrotado());
                }
                permitirAnimaciones_J4 = false;

                J4derrotado.Start();
            }
        }
        private void ColisionesPersonajesPuertas(Control x)
        {
            // Colisiones Jugador 1 - puerta 1
            if ((string)x.Name == "puertaJ1")
            {
                if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds))
                {

                    if (estaEnPuerta_J1 == false)
                    {
                        estaEnPuerta_J1 = true;
                    }
                }
                else
                {
                    if (estaEnPuerta_J1 == true)
                    {
                        estaEnPuerta_J1 = false;
                    }
                }
            }
            // Colisiones Jugador 2 - puerta 2
            if ((string)x.Name == "puertaJ2")
            {
                if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
                {

                    if (estaEnPuerta_J2 == false)
                    {
                        estaEnPuerta_J2 = true;
                    }
                }
                else
                {
                    if (estaEnPuerta_J2 == true)
                    {
                        estaEnPuerta_J2 = false;
                    }
                }
            }
            // Colisiones Jugador 3 - puerta 3
            if ((string)x.Name == "puertaJ3")
            {
                if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds))
                {

                    if (estaEnPuerta_J3 == false)
                    {
                        estaEnPuerta_J3 = true;
                    }
                }
                else
                {
                    if (estaEnPuerta_J3 == true)
                    {
                        estaEnPuerta_J3 = false;
                    }
                }
            }
            // Colisiones Jugador 4 - puerta 4
            if ((string)x.Name == "puertaJ4")
            {
                if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds))
                {

                    if (estaEnPuerta_J4 == false)
                    {
                        estaEnPuerta_J4 = true;
                    }
                }
                else
                {
                    if (estaEnPuerta_J4 == true)
                    {
                        estaEnPuerta_J4 = false;
                    }
                }
            }
        }
        private void ColisionesPersonajesAire(Control x)
        {
            // Colisiones Jugador 1 - plataforma
            if (misPicsPersonajes[0].Bounds.IntersectsWith(x.Bounds) && EstaColisionandoConAlgunaPlataforma(misPicsPersonajes[0]) == false)
            {
                if (onPlatform_J1 == true)
                {
                    onPlatform_J1 = false;
                }
            }
            // Colisiones Jugador 2 - plataforma
            if (misPicsPersonajes[1].Bounds.IntersectsWith(x.Bounds))
            {
                if (onPlatform_J2 == true && EstaColisionandoConAlgunaPlataforma(misPicsPersonajes[1]) == false)
                {
                    onPlatform_J2 = false;
                }

            }
            // Colisiones Jugador 3 - plataforma
            if (misPicsPersonajes[2].Bounds.IntersectsWith(x.Bounds) && EstaColisionandoConAlgunaPlataforma(misPicsPersonajes[2]) == false)
            {
                if (onPlatform_J3 == true)
                {
                    onPlatform_J3 = false;
                }

            }
            // Colisiones Jugador 4 - plataforma
            if (misPicsPersonajes[3].Bounds.IntersectsWith(x.Bounds) && EstaColisionandoConAlgunaPlataforma(misPicsPersonajes[3]) == false)
            {
                if (onPlatform_J4 == true)
                {
                    onPlatform_J4 = false;
                }
            }
        }
        // Funcion para saber si el picturebox colisiona con alguna de las posibles plataformas
        private bool EstaColisionandoConAlgunaPlataforma(PictureBox pb)
        {
            bool colisiona = false;
            foreach (Control x in this.Controls)
            {
                // Plataformas
                if ((string)x.Tag == "plataforma")
                {
                    if (pb.Bounds.IntersectsWith(x.Bounds))
                    {
                        colisiona = true;
                    }
                }
            }
            return colisiona;
        }


        // Timers para las animaciones de las muertes
        private void J1derrotado_Tick(object sender, EventArgs e)
        {
            if (onPlatform_J1 == true)
            { onPlatform_J1 = false; }
            Jug1.SetX(30);
            Jug1.SetY(200);
            //misPicsPersonajes[0].Image = Image.FromFile("vacio.png");
            if (vidas1 > 1)
            {
                vidas1--;
                if (vidas1 == 2)
                { vidasJ1_pb.BackgroundImage = Image.FromFile("vidas_fire_2.png"); }
                else if (vidas1 == 1)
                { vidasJ1_pb.BackgroundImage = Image.FromFile("vidas_fire_1.png"); }

                permitirAnimaciones_J1 = true;
                misPicsPersonajes[0].Image = Image.FromFile(Jug1.GetAnimacionNormal());
            }
            else
            {
                vidasJ1_pb.BackgroundImage = Image.FromFile("vidas_0.png");
                isGameOver_J1 = true;
                //MessageBox.Show("J1 ha muerto...");
                misPicsPersonajes[0].Image = Image.FromFile("vacio.png");

            }
            J1derrotado.Stop();

        }
        private void J2derrotado_Tick(object sender, EventArgs e)
        {
            if (onPlatform_J2 == true)
            { onPlatform_J2 = false; }
            Jug2.SetX(100);
            Jug2.SetY(200);
            //misPicsPersonajes[1].Image = Image.FromFile("vacio.png");
            if (vidas2 > 1)
            {
                vidas2--;
                if (vidas2 == 2)
                { vidasJ2_pb.BackgroundImage = Image.FromFile("vidas_water_2.png"); }
                else if (vidas2 == 1)
                { vidasJ2_pb.BackgroundImage = Image.FromFile("vidas_water_1.png"); }

                permitirAnimaciones_J2 = true;
                misPicsPersonajes[1].Image = Image.FromFile(Jug2.GetAnimacionNormal());

            }
            else
            {
                vidasJ2_pb.BackgroundImage = Image.FromFile("vidas_0.png");
                isGameOver_J2 = true;
                //MessageBox.Show("J2 ha muerto...");
                misPicsPersonajes[1].Image = Image.FromFile("vacio.png");

            }
            J2derrotado.Stop();

        }
        private void J3derrotado_Tick(object sender, EventArgs e)
        {
            if (onPlatform_J3 == true)
            { onPlatform_J3 = false; }
            Jug3.SetX(225);
            Jug3.SetY(200);
            //misPicsPersonajes[1].Image = Image.FromFile("vacio.png");
            if (vidas3 > 1)
            {
                vidas3--;
                if (vidas3 == 2)
                { vidasJ3_pb.BackgroundImage = Image.FromFile("vidas_rock_2.png"); }
                else if (vidas3 == 1)
                { vidasJ3_pb.BackgroundImage = Image.FromFile("vidas_rock_1.png"); }

                permitirAnimaciones_J3 = true;
                misPicsPersonajes[2].Image = Image.FromFile(Jug3.GetAnimacionNormal());

            }
            else
            {
                vidasJ3_pb.BackgroundImage = Image.FromFile("vidas_0.png");
                isGameOver_J3 = true;
                //MessageBox.Show("J2 ha muerto...");
                misPicsPersonajes[2].Image = Image.FromFile("vacio.png");

            }
            J3derrotado.Stop();
        }
        private void J4derrotado_Tick(object sender, EventArgs e)
        {
            if (onPlatform_J4 == true)
            { onPlatform_J4 = false; }
            Jug4.SetX(300);
            Jug4.SetY(200);
            //misPicsPersonajes[1].Image = Image.FromFile("vacio.png");
            if (vidas4 > 1)
            {
                vidas4--;
                if (vidas4 == 2)
                { vidasJ4_pb.BackgroundImage = Image.FromFile("vidas_cloud_2.png"); }
                else if (vidas4 == 1)
                { vidasJ4_pb.BackgroundImage = Image.FromFile("vidas_cloud_1.png"); }

                permitirAnimaciones_J4 = true;
                misPicsPersonajes[3].Image = Image.FromFile(Jug4.GetAnimacionNormal());
            }
            else
            {
                vidasJ4_pb.BackgroundImage = Image.FromFile("vidas_0.png");
                isGameOver_J4 = true;
                //MessageBox.Show("J2 ha muerto...");
                misPicsPersonajes[3].Image = Image.FromFile("vacio.png");


            }
            J4derrotado.Stop();
        }
        // Funcion para calcular los puntos totales. 
        // "S", "A", "B", "C", "F"
        private string CalcularPuntosTotales()
        {
            string letra;

            int total_puntos = puntos1 + puntos2 + puntos3 + puntos4;
            int total_restar_vidas = (3 - vidas1) + (3 - vidas2) + (3 - vidas3) + (3 - vidas4);
            int total_tiempo = segundos;

            // 50% los puntos de los diamantes; poner dividiento los diamantes totales presentes
            // 50% puntos por tiempo: 60s como referencia
            // restaremos 20% dependiendo de las vidas: 12 vidas se pueden perder en total
            double porcion_puntos = (Convert.ToDouble(total_puntos) / 4) * 0.5;
            double porcion_tiempo = 0;             if (total_tiempo <= 60)             { porcion_tiempo = 0.5; }             else             { porcion_tiempo = 0.25; }
            double porcion_restar_vidas = (Convert.ToDouble(total_restar_vidas) / 12) * 0.2;
            double puntos_partida = porcion_puntos + porcion_tiempo - porcion_restar_vidas;

            lbl_noColisionesPlataforma.Text = "Porcion puntos: " + porcion_puntos + "; Porcion timempo: " + porcion_tiempo + "; Porcion vidas: " + porcion_restar_vidas + "\nTotal: " + puntos_partida;
            if (puntos_partida >= 0.85 && puntos_partida <= 1)
            { letra = "S"; }
            else if (puntos_partida >= 0.6 && puntos_partida < 0.85)
            { letra = "A"; }
            else if (puntos_partida >= 0.3 && puntos_partida < 0.6)
            { letra = "B"; }
            else
            { letra = "C"; }
            return (letra);
        }
        // función para enviar el mensaje de fin de partida + parar el timer principal
        private void EnvíoMensajeFinDePartida(string result_partida, string letra_resultado)
        {
            tiempoJuego.Stop();
            MainTimerJuego.Stop();
            if (result_partida != "No Superado")
            {
                // Solo envía el mensaje al servidor el jugador 1
                if (miPersonajeQueControlo == 1)
                {
                    string mensaje = "50/" + idPartida + "/" + mapa + "/" + result_partida + "/" + letra_resultado + "/" + segundos;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    label_mensaje.Text = mensaje;
                }
            }
            else
            {
                // Envía el mensaje el personaje que controla al jugador sin vidas
                string mensaje = "50/" + idPartida + "/" + mapa + "/" + result_partida + "/" + letra_resultado + "/" + segundos;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                label_mensaje.Text = mensaje;
            }
        }
        // función para representar el resultado de la partida y que se abra el panel con las estadísticas
        // Esta función se llama desde el main que la llama desde la selección de partida
        public void FinDePartida(string resultado, string letra_resultado)
        {
            // Creamos controles.
            Panel panelResultado = new Panel();

            Label labelResultado = new Label();
            Label label_letraResultado = new Label();

            Label labelNombreJ1 = new Label();
            Label labelNombreJ2 = new Label();
            Label labelNombreJ3 = new Label();
            Label labelNombreJ4 = new Label();
            Label labelPuntosJ1 = new Label();
            Label labelPuntosJ2 = new Label();
            Label labelPuntosJ3 = new Label();
            Label labelPuntosJ4 = new Label();
            Label labelTiempo = new Label();

            Button botonCerrarPartida = new Button();

            PictureBox p1 = new PictureBox();
            PictureBox p2 = new PictureBox();
            PictureBox p3 = new PictureBox();
            PictureBox p4 = new PictureBox();

            PictureBox vidas_fin1 = new PictureBox();
            PictureBox vidas_fin2 = new PictureBox();
            PictureBox vidas_fin3 = new PictureBox();
            PictureBox vidas_fin4 = new PictureBox();


            // Initialize the Panel control.
            panelResultado.Size = new Size(500, 600);
            panelResultado.Location = new Point(this.Width / 2 - panelResultado.Width / 2, this.Height / 2 - panelResultado.Height / 2);
            // Set the Borderstyle for the Panel to three-dimensional.
            panelResultado.BackColor = Color.Goldenrod;
            panelResultado.BorderStyle = BorderStyle.FixedSingle;
            // Initialize the Label controls.
            labelResultado.Text = resultado;
            labelResultado.Font = new Font("Arial Black", 20, FontStyle.Bold);
            labelResultado.AutoSize = true;
            labelResultado.TextAlign = ContentAlignment.MiddleCenter;
            labelResultado.Location = new Point(panelResultado.Width / 2 - labelResultado.Width, 30);
            label_letraResultado.Text = letra_resultado; 
            label_letraResultado.Font = new Font("Arial Black", 35, FontStyle.Bold);
            label_letraResultado.BorderStyle = BorderStyle.FixedSingle;
            label_letraResultado.AutoSize = true;
            label_letraResultado.TextAlign = ContentAlignment.MiddleCenter;
            label_letraResultado.Location = new Point(panelResultado.Width - 100, 60);

            labelNombreJ1.Text = Jug1.GetUsername();
            labelNombreJ1.Location = new Point(125 - labelNombreJ1.Width / 2 - 50, 300);
            labelNombreJ2.Text = Jug2.GetUsername();
            labelNombreJ2.Location = new Point(250 - labelNombreJ2.Width / 2 - 50, 300);
            labelNombreJ3.Text = Jug3.GetUsername();
            labelNombreJ3.Location = new Point(375 - labelNombreJ3.Width / 2 - 50, 300);
            labelNombreJ4.Text = Jug4.GetUsername();
            labelNombreJ4.Location = new Point(500 - labelNombreJ4.Width / 2 - 50, 300);

            labelPuntosJ1.Text = "Puntos: " + puntos1;
            labelPuntosJ1.Location = new Point(125 - labelPuntosJ1.Width / 2 - 50, 200);
            labelPuntosJ2.Text = "Puntos: " + puntos2;
            labelPuntosJ2.Location = new Point(250 - labelPuntosJ2.Width / 2 - 50, 200);
            labelPuntosJ3.Text = "Puntos: " + puntos3;
            labelPuntosJ3.Location = new Point(375 - labelPuntosJ3.Width / 2 - 50, 200);
            labelPuntosJ4.Text = "Puntos: " + puntos4;
            labelPuntosJ4.Location = new Point(500 - labelPuntosJ4.Width / 2 - 50, 200);

            labelTiempo.Text = "Tiempo: " + segundos;
            labelTiempo.Location = new Point(panelResultado.Width / 2 - labelTiempo.Width / 2, 150);

            botonCerrarPartida.Text = "Finalizar";
            botonCerrarPartida.Location = new Point(panelResultado.Width / 2 - botonCerrarPartida.Width / 2, panelResultado.Height - 75);
            botonCerrarPartida.BackColor = Color.LightGoldenrodYellow;
            botonCerrarPartida.Click += botonCerrarPartida_Click;   //ponemos el evento al boton que hemos creado

            p1.Width = Jug1.GetAnchoNormal();
            p1.Height = Jug1.GetAlturaNormal();
            p1.Location = new Point(labelNombreJ1.Location.X, labelNombreJ1.Location.Y + p1.Height);
            p1.SizeMode = PictureBoxSizeMode.Zoom;
            p1.BackColor = Color.Transparent;
            p1.Image = Image.FromFile(Jug1.GetAnimacionSaludo());
            p2.Width = Jug2.GetAnchoNormal();
            p2.Height = Jug2.GetAlturaNormal();
            p2.Location = new Point(labelNombreJ2.Location.X, labelNombreJ2.Location.Y + p2.Height - 20);
            p2.SizeMode = PictureBoxSizeMode.Zoom;
            p2.BackColor = Color.Transparent;
            p2.Image = Image.FromFile(Jug2.GetAnimacionSaludo());
            p3.Width = Jug3.GetAnchoNormal();
            p3.Height = Jug3.GetAlturaNormal();
            p3.Location = new Point(labelNombreJ3.Location.X, labelNombreJ3.Location.Y + p3.Height);
            p3.SizeMode = PictureBoxSizeMode.Zoom;
            p3.BackColor = Color.Transparent;
            p3.Image = Image.FromFile(Jug3.GetAnimacionSaludo());
            p4.Width = Jug4.GetAnchoNormal();
            p4.Height = Jug4.GetAlturaNormal();
            p4.Location = new Point(labelNombreJ4.Location.X, labelNombreJ4.Location.Y + p4.Height + 10);
            p4.SizeMode = PictureBoxSizeMode.Zoom;
            p4.BackColor = Color.Transparent;
            p4.Image = Image.FromFile(Jug4.GetAnimacionSaludo());

            vidas_fin1.Width = vidasJ1_pb.Width;
            vidas_fin1.Height = vidasJ1_pb.Height;
            vidas_fin1.Location = new Point(labelPuntosJ1.Location.X - 20, labelPuntosJ1.Location.Y + 30);
            vidas_fin1.SizeMode = PictureBoxSizeMode.Zoom;
            vidas_fin1.BackColor = Color.Transparent;
            if (vidas1 == 3)
            { vidas_fin1.Image = Image.FromFile("vidas_fire_3.png"); }
            else if (vidas1 == 2)
            { vidas_fin1.Image = Image.FromFile("vidas_fire_2.png"); }
            else if (vidas1 == 1)
            { vidas_fin1.Image = Image.FromFile("vidas_fire_1.png"); }
            else
            { vidas_fin1.Image = Image.FromFile("vidas_0.png"); }
            if (isGameOver_J1 == true)
            {
                vidas_fin1.Image = Image.FromFile("vidas_0.png");
                p1.Image = Image.FromFile(Jug1.GetAnimacionSaludo());
            }

            vidas_fin2.Width = vidasJ2_pb.Width;
            vidas_fin2.Height = vidasJ2_pb.Height;
            vidas_fin2.Location = new Point(labelPuntosJ2.Location.X - 20, labelPuntosJ2.Location.Y + 30);
            vidas_fin2.SizeMode = PictureBoxSizeMode.Zoom;
            vidas_fin2.BackColor = Color.Transparent;
            if (vidas2 == 3)
            { vidas_fin2.Image = Image.FromFile("vidas_water_3.png"); }
            else if (vidas2 == 2)
            { vidas_fin2.Image = Image.FromFile("vidas_water_2.png"); }
            else if (vidas2 == 1)
            { vidas_fin2.Image = Image.FromFile("vidas_water_1.png"); }
            else
            { vidas_fin2.Image = Image.FromFile("vidas_0.png"); }
            if (isGameOver_J2 == true)
            {
                vidas_fin2.Image = Image.FromFile("vidas_0.png");
                p2.Image = Image.FromFile(Jug2.GetAnimacionSaludo());
            }

            vidas_fin3.Width = vidasJ3_pb.Width;
            vidas_fin3.Height = vidasJ3_pb.Height;
            vidas_fin3.Location = new Point(labelPuntosJ3.Location.X - 20, labelPuntosJ3.Location.Y + 30);
            vidas_fin3.SizeMode = PictureBoxSizeMode.Zoom;
            vidas_fin3.BackColor = Color.Transparent;
            if (vidas3 == 3)
            { vidas_fin3.Image = Image.FromFile("vidas_rock_3.png"); }
            else if (vidas3 == 2)
            { vidas_fin3.Image = Image.FromFile("vidas_rock_2.png"); }
            else if (vidas3 == 1)
            { vidas_fin3.Image = Image.FromFile("vidas_rock_1.png"); }
            else
            { vidas_fin3.Image = Image.FromFile("vidas_0.png"); }
            if (isGameOver_J3 == true)
            {
                vidas_fin3.Image = Image.FromFile("vidas_0.png");
                p3.Image = Image.FromFile(Jug3.GetAnimacionSaludo());
            }

            vidas_fin4.Width = vidasJ4_pb.Width;
            vidas_fin4.Height = vidasJ4_pb.Height;
            vidas_fin4.Location = new Point(labelPuntosJ4.Location.X - 20, labelPuntosJ4.Location.Y + 30);
            vidas_fin4.SizeMode = PictureBoxSizeMode.Zoom;
            vidas_fin4.BackColor = Color.Transparent;
            if (vidas4 == 3)
            { vidas_fin4.Image = Image.FromFile("vidas_cloud_3.png"); }
            else if (vidas4 == 2)
            { vidas_fin4.Image = Image.FromFile("vidas_cloud_2.png"); }
            else if (vidas4 == 1)
            { vidas_fin4.Image = Image.FromFile("vidas_cloud_1.png"); }
            else
            { vidas_fin4.Image = Image.FromFile("vidas_0.png"); }
            if (isGameOver_J4 == true)
            {
                vidas_fin4.Image = Image.FromFile("vidas_0.png");
                p4.Image = Image.FromFile(Jug4.GetAnimacionDerrotado());
            }
            Invoke(new Action(() =>
            {
                // Add the Panel control to the form.
                this.tiempoJuego.Stop(); this.Controls.Add(panelResultado);
                // Add the Label controls to the Panel.
                panelResultado.Controls.Add(labelResultado);
                panelResultado.Controls.Add(label_letraResultado);

                panelResultado.Controls.Add(labelNombreJ1);
                panelResultado.Controls.Add(labelNombreJ2);
                panelResultado.Controls.Add(labelNombreJ3);
                panelResultado.Controls.Add(labelNombreJ4);
                panelResultado.Controls.Add(labelPuntosJ1);
                panelResultado.Controls.Add(labelPuntosJ2);
                panelResultado.Controls.Add(labelPuntosJ3);
                panelResultado.Controls.Add(labelPuntosJ4);
                panelResultado.Controls.Add(labelTiempo);

                panelResultado.Controls.Add(botonCerrarPartida);

                panelResultado.Controls.Add(p1);
                panelResultado.Controls.Add(p2);
                panelResultado.Controls.Add(p3);
                panelResultado.Controls.Add(p4);
                panelResultado.Controls.Add(vidas_fin1);
                panelResultado.Controls.Add(vidas_fin2);
                panelResultado.Controls.Add(vidas_fin3);
                panelResultado.Controls.Add(vidas_fin4);

                panelResultado.BringToFront();
            }));
        }
        // función del click del botón de finalizar partida 
        void botonCerrarPartida_Click(object sender, EventArgs e)
        {
            // Este messagebox se quitará y en vez de esto, se cerrara el mapa
            MessageBox.Show("La partida ha terminado.");             string mensaje = "51/" + idPartida + "/" + mapa;             byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);            server.Send(msg);
            this.Close();
        }

    }

}
