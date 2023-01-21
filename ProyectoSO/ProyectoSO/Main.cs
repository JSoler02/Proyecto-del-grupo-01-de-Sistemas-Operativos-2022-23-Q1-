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
        int shiva = 1;  // 1: si Shiva; 0: si Maquina Virtual
        int julia = 0;  // 1: si IP de Julia en la Maquina Virtual; 0: si IP del resto en la Maquina virtual

        int idPartida;
        string nombre;


        string invitados;
        int NumInvitados = 0; // maximo puede ser 4
        int InvPartida = 0;
        bool But_empezarPartida_activado = false;
        bool soyanfitrion;

        bool conectado;

        // Lista generica de formularios (memoria) per saber donde enviare el mensaje
        List<SeleccionPartida> formularios1 = new List<SeleccionPartida>();
        // listas de los mapas -->  igual que la lista de partidas del servidor[] tiene 10 max
        //Templo[] form_templo_4Jug = new Templo[10];
        //Volcan[] form_volcan_4Jug = new Volcan[10];
        //Cueva_Maritima[] form_cueva_mar_4Jug = new Cueva_Maritima[10];
        //prueba_Teclas[] form_prueba_tecl = new prueba_Teclas[10];
        List<Templo4> form_templo_4Jug = new List<Templo4>();
        List<Templo2> form_templo_2Jug = new List<Templo2>();
        List<Volcan4> form_volcan_4Jug = new List<Volcan4>();
        List<Volcan3> form_volcan_3Jug = new List<Volcan3>();
        List<Volcan2> form_volcan_2Jug = new List<Volcan2>();
        List<TemploHelado> form_templohelado = new List<TemploHelado>();
        List<TemploHelado2> form_templohelado_2Jug = new List<TemploHelado2>();

        List<Cueva_Maritima> form_cueva_mar_4Jug = new List<Cueva_Maritima>();
        List<prueba_Teclas> form_prueba_tecl = new List<prueba_Teclas>();

        // Adornos
        PictureBox Jug1 = new PictureBox();
        PictureBox Jug2 = new PictureBox();
        PictureBox Jug3 = new PictureBox();
        PictureBox Jug4 = new PictureBox();
        Random rnd = new Random();
        int dice;
        Panel panelTituloJuego = new Panel();

        
        private void Main_Load(object sender, EventArgs e)
        {
            // Proceso del título del juego       --> TheELEMENTS
            {    // Creamos controles 
                Label labelResultado = new Label();
                Label labelcomentario = new Label();


                // Initialize the Panel control.  
                panelTituloJuego.Size = this.Size;
                panelTituloJuego.Location = new Point(0, 0);
                panelTituloJuego.BackColor = Color.Turquoise;

                // Initialize the Label controls.
                labelResultado.Text = "The ELEMENTS";
                labelResultado.Font = new Font("Arial Black", 35, FontStyle.Bold);
                labelResultado.AutoSize = false;
                labelResultado.TextAlign = ContentAlignment.MiddleCenter;
                labelResultado.Width = panelTituloJuego.Width;
                labelResultado.Height = 100;
                labelResultado.Location = new Point(0, 30);

                labelcomentario.Text = "Haz click para entrar.";
                labelcomentario.AutoSize = true;
                labelcomentario.Font = new Font("Arial", 13, FontStyle.Bold);
                labelcomentario.TextAlign = ContentAlignment.MiddleCenter;
                labelcomentario.Location = new Point(panelTituloJuego.Width /4 - labelcomentario.Width/2 , this.Width-100);

                this.Controls.Add(panelTituloJuego);
                panelTituloJuego.BringToFront();

                panelTituloJuego.Controls.Add(labelResultado);
                panelTituloJuego.Controls.Add(labelcomentario);
                labelcomentario.BringToFront();

                panelTituloJuego.Click += panelTituloJuego_Click;   //ponemos el evento al boton que hemos creado

            }
            data_mapas_info.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            data_mapas_info.RowHeadersVisible = false;
            data_mapas_info.ColumnHeadersVisible = true;
            data_mapas_info.ColumnCount = 1;
            data_mapas_info.Columns[0].HeaderText = "Mapas";
            data_mapas_info.ReadOnly = true;
            data_mapas_info.Rows.Add("Templo");
            data_mapas_info.Rows.Add("Templo (2Jug)");
            data_mapas_info.Rows.Add("Templo Helado");
            data_mapas_info.Rows.Add("Templo Helado (2Jug)");
            data_mapas_info.Rows.Add("Volcan");
            data_mapas_info.Rows.Add("Volcan (3Jug)");
            data_mapas_info.Rows.Add("Volcan (2Jug)");
            data_mapas_info.Rows.Add("Cueva");
            data_mapas_info.ClearSelection();

            label3.Visible = false;
            lbl_lista_con.Visible = false;
            PuntMax_But.Visible = false;
            eliminar_but.Visible = false;
            PartidasMapa_But.Visible = false;
            TiempoMaxPartida_But.Visible = false;
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
            label_notificacion.Visible = false;
            label_notificacion.Text = "";
            mapaTbx.Visible = false;
            info_mapas_pb.Visible = false;
            data_mapas_info.Visible = false;
           conectado = false;

            InicioPersonajesImagenes();

        }
        void panelTituloJuego_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(panelTituloJuego);
        }
        private void InicioPersonajesImagenes ()
        {
            //string corriendo_der = "_corriendo_der.gif
            Jug1.Image = Image.FromFile("Fireboy.gif"); 
            Jug1.BackColor = Color.Transparent;
            Jug1.SizeMode = PictureBoxSizeMode.Zoom;
            Jug1.Width = 65;
            Jug1.Height = 75;
            Jug1.Location = new Point(250, this.ClientSize.Height - Jug1.Height);
            this.Controls.Add(Jug1);

            Jug2.Image = Image.FromFile("Watergirl.gif");
            Jug2.BackColor = Color.Transparent;
            Jug2.SizeMode = PictureBoxSizeMode.Zoom;
            Jug2.Width = 70;
            Jug2.Height = 85;
            Jug2.Location = new Point(Jug1.Left - Jug2.Width - 10 , this.ClientSize.Height - Jug2.Height);
            this.Controls.Add(Jug2);

            Jug3.Image = Image.FromFile("Rockboy.gif");
            Jug3.BackColor = Color.Transparent;
            Jug3.SizeMode = PictureBoxSizeMode.Zoom;
            Jug3.Width = 60;
            Jug3.Height = 75;
            Jug3.Location = new Point(Jug2.Left - Jug3.Width - 10, this.ClientSize.Height - Jug3.Height);
            this.Controls.Add(Jug3);

            Jug4.Image = Image.FromFile("Cloudgirl.gif");
            Jug4.BackColor = Color.Transparent;
            Jug4.SizeMode = PictureBoxSizeMode.Zoom;
            Jug4.Width = 70;
            Jug4.Height = 70;
            Jug4.Location = new Point(Jug3.Left - Jug4.Width - 10, this.ClientSize.Height - Jug4.Height);
            this.Controls.Add(Jug4);

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
                puerto = 8095;
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
                //MessageBox.Show(mensajeLimpio);
                int numForm; // para saber a que formulario debo enviarle la respuesta
                string mensaje;

                string mapa;
               
                switch (codigo)
                {
                    case 1: // Login
                        mensaje = trozos[1];
                        MessageBox.Show(mensaje);
                        if (mensaje == "Conectado")
                        {
                            Invoke(new Action(() =>
                            {
                                dice = rnd.Next(1, 5); //generates a random in number from 1 to 4
                                timer_saludo.Interval = 2000; //3segundos para el saludo
                                timer_saludo.Start();
                                if (dice == 1)
                                { Jug1.Image = Image.FromFile("Fireboy_saludando.gif");}
                                else if (dice == 2)
                                { Jug2.Image = Image.FromFile("Watergirl_saludando.gif"); }
                                else if (dice == 3)
                                { Jug3.Image = Image.FromFile("Rockboy_saludando.gif"); }
                                else
                                { Jug4.Image = Image.FromFile("Cloudgirl_saludando.gif"); }

                            }));
                        }
                        break;
                    case 2: // New User
                        mensaje = trozos[1];
                        MessageBox.Show(mensaje);
                        break;

                    case 3: //consulta 1 --> Puntos maximos de usuario
                        mensaje = trozos[1];
                        Invoke(new Action(() =>
                        {
                            if (mensaje == "No hay datos")
                            { label_notificacion.Text = mensaje; }
                            else
                            { 
                                label_notificacion.Text = "Tu puntuación máxima es: " + mensaje;
                            }
                        }));
                        break;

                    case 4: //consulta 2 --> Tantas partidas en el mapa X
                        mensaje = trozos[1];
                        Invoke(new Action(() =>
                        {
                            if (mensaje == "No hay datos")
                            { label_notificacion.Text = mensaje; }
                            else
                            {
                                label_notificacion.Text = "En el mapa " + mapaTbx.Text + " has jugado " + mensaje + " partidas.";
                            }
                        }));
                        break;

                    case 5: //consulta 3 --> timepo max que has estado en partida
                        mensaje = trozos[1];
                        Invoke(new Action(() =>
                        {
                            if (mensaje == "No hay datos")
                            { label_notificacion.Text = mensaje; }
                            else
                            {
                                label_notificacion.Text = "Has jugado un máximo de " + mensaje + " segundos en una partida.";
                            }
                        }));
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
                               string nombre_lista_con = Convert.ToString(trozos[i + 2].Split('\0')[0]);
                               GridConectados.Rows.Add(nombre_lista_con);
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
                            Invoke(new Action(() =>
                            {
                                label_notificacion.Text = nombre_acepta + " ha aceptado tu invitación a partida";
                            }));
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                label_notificacion.Text = nombre_acepta + " no ha aceptado tu invitación a partida";
                            }));
                        }
                        Invoke(new Action(() =>
                        {
                            GridConectados.BackgroundColor = Color.White;
                        }));
                        break;

                    case 9: // empieza la seleccion de personaje y partida para todo el mundo
                        // 9/idpartida/anfitrión/numJugadores
                        idPartida = Convert.ToInt32(trozos[1]);
                        if (trozos[2]==nombre)
                        { soyanfitrion = true; }
                        else
                        { soyanfitrion = false; }
                       
                        Invoke(new Action(() =>
                        {
                            label_notificacion.Text = "Todo el mundo ha aceptado la invitación.";
                        }));
                        //MessageBox.Show("Todo el mundo ha aceptado la invitación.");
                        ThreadStart ts = delegate { PonerEnMarchaFormulario1(Convert.ToInt32(trozos[3])); }; // creo thread que executa la funcio PonerEnMarchaFormulario
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

                    case 13: 

                        break;

                    case 14:
                        // Empieza la partida: El anfitrión le ha dado a empezar: se abrirá otro formulario
                        // --> 14/idPartida
                        idPartida = Convert.ToInt32(trozos[1]);
                        //formularios1[idPartida].AnfitrionEmpiezaPartida();
                        string p = formularios1[idPartida].GetMapa();//"PruebaTeclas";// 
                        switch (p)
                        {
                            case "Templo":
                                ThreadStart ts_mapa_templo4 = delegate { PonerEnMarchaForm_Templo4Jug(); };
                                t = new Thread(ts_mapa_templo4);
                                t.Start();
                                break;
                            case "Templo (2Jug)":
                                ThreadStart ts_mapa_templo_2 = delegate { PonerEnMarchaForm_Templo2Jug(); };
                                t = new Thread(ts_mapa_templo_2);
                                t.Start();
                                break;
                            case "Volcan":
                                ThreadStart ts_mapa_volcan4 = delegate { PonerEnMarchaForm_Volcan4Jug(); };
                                t = new Thread(ts_mapa_volcan4);
                                t.Start();
                                break;
                            case "Volcan (3Jug)":
                                ThreadStart ts_mapa_volcan3 = delegate { PonerEnMarchaForm_Volcan3Jug(); };
                                t = new Thread(ts_mapa_volcan3);
                                t.Start();
                                break;
                            case "Volcan (2Jug)":
                                ThreadStart ts_mapa_volcan2 = delegate { PonerEnMarchaForm_Volcan2Jug(); };
                                t = new Thread(ts_mapa_volcan2);
                                t.Start();
                                break;
                            case "Cueva":
                                ThreadStart ts_mapa_cuevaMaritima = delegate { PonerEnMarchaForm_CuevaMaritima(); };
                                t = new Thread(ts_mapa_cuevaMaritima);
                                t.Start();
                                break;
                            case "Templo Helado":
                                ThreadStart ts_mapa_templohelado = delegate { PonerEnMarchaForm_TemploHelado(); };
                                t = new Thread(ts_mapa_templohelado);
                                t.Start();
                                break;
                            case "Templo Helado (2Jug)":
                                ThreadStart ts_mapa_templohelado_2 = delegate { PonerEnMarchaForm_TemploHelado2Jug(); };
                                t = new Thread(ts_mapa_templohelado_2);
                                t.Start();
                                break;
                            case "PruebaTeclas":
                                ThreadStart ts_mapa_pt = delegate { PonerEnMarchaForm_PruebaTecla(); };
                                t = new Thread(ts_mapa_pt);
                                t.Start();
                                break;
                        }
                        Invoke(new Action(() =>
                        { formularios1[idPartida].CerrarSeleccionPartida();}));
 
                        break;
                    //  *   *   *   *   *   *   *   *   *   *   *   *   *  INICIO:  Movimientos de los personajes   *   *   *   *   *   *   *   *   *   
                    case 15:
                        // 15/idPartida/mapa/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];

                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() =>
                                {
                                    form_templo_4Jug[idPartida].TeclaIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug[idPartida].TeclaIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug[idPartida].TeclaIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug[idPartida].TeclaIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug[idPartida].TeclaIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado[idPartida].TeclaIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug[idPartida].TeclaIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug[idPartida].TeclaIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                        }
                        break;

                    case 16:
                        // 16/idPartida/mapa/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];

                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() =>
                                {
                                    form_templo_4Jug[idPartida].TeclaIzquierdaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug[idPartida].TeclaIzquierdaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug[idPartida].TeclaIzquierdaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug[idPartida].TeclaIzquierdaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug[idPartida].TeclaIzquierdaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado[idPartida].TeclaIzquierdaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug[idPartida].TeclaIzquierdaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug[idPartida].TeclaIzquierdaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                        }
                        break;

                    case 17:
                        // 17/idPartida/mapa/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];

                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() =>
                                {
                                    form_templo_4Jug[idPartida].TeclaDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug[idPartida].TeclaDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug[idPartida].TeclaDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug[idPartida].TeclaDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug[idPartida].TeclaDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado[idPartida].TeclaDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug[idPartida].TeclaDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug[idPartida].TeclaDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                        }
                        break;

                    case 18:
                        // 18/idPartida/mapa/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];

                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() =>
                                {
                                    form_templo_4Jug[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug[idPartida].TeclaDerechaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                        }
                        break;

                    case 19:
                        // 19/idPartida/mapa/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];
                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() => 
                                { 
                                    form_templo_4Jug[idPartida].TeclaArribaSolaClicada_Otro(Convert.ToInt32(trozos[3])); 
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug[idPartida].TeclaArribaSolaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug[idPartida].TeclaArribaSolaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug[idPartida].TeclaArribaSolaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug[idPartida].TeclaArribaSolaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado[idPartida].TeclaArribaSolaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug[idPartida].TeclaArribaSolaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug[idPartida].TeclaArribaSolaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                        }
                        break;

                    case 20:
                        // mensaje del chat
                        //--> "20/idPartida/Juan/Hola compañeros"
                        // en mapas y seleccion personajes
                        idPartida = Convert.ToInt32(trozos[1]);
                        formularios1[idPartida].AtenderMensajeChat(trozos[2], trozos[3]);
                        break;

                    case 21:
                        // 21/idPartida/mapa/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];

                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() =>
                                {
                                    form_templo_4Jug[idPartida].TeclaArribaConIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug[idPartida].TeclaArribaConIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug[idPartida].TeclaArribaConIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug[idPartida].TeclaArribaConIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug[idPartida].TeclaArribaConIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado[idPartida].TeclaArribaConIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug[idPartida].TeclaArribaConIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug[idPartida].TeclaArribaConIzquierdaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                        }
                        break;
                    case 22:
                        // 22/idPartida/mapa/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];

                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() =>
                                {
                                    form_templo_4Jug[idPartida].TeclaArribaConDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug[idPartida].TeclaArribaConDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug[idPartida].TeclaArribaConDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug[idPartida].TeclaArribaConDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug[idPartida].TeclaArribaConDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado[idPartida].TeclaArribaConDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug[idPartida].TeclaArribaConDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug[idPartida].TeclaArribaConDerechaClicada_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                        }
                        break;

                    case 23:
                        // 23/idPartida/mapa/PersonajeOtro
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];

                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() =>
                                {
                                    form_templo_4Jug[idPartida].TeclaArribaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug[idPartida].TeclaArribaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug[idPartida].TeclaArribaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug[idPartida].TeclaArribaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug[idPartida].TeclaArribaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado[idPartida].TeclaArribaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug[idPartida].TeclaArribaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug[idPartida].TeclaArribaDejadaDeClicar_Otro(Convert.ToInt32(trozos[3]));
                                }));
                                break;
                        }
                        break;
                    //  *   *   *   *   *   *   *   *   *   *   *   *   *   FIN: Movimientos de los personajes   *   *   *   *   *   *   *   *  *   *
                    case 25:    // elimnar cuenta y desconectar el usuario
                                // 9/Usuario Eliminado
                        mensaje = trozos[1];
                        MessageBox.Show(mensaje);

                        Invoke(new Action(() =>
                        {
                            //Mensaje de desconexión
                            label3.Visible = false;
                            lbl_lista_con.Visible = false;
                            PuntMax_But.Visible = false;
                            eliminar_but.Visible = false;
                            PartidasMapa_But.Visible = false;
                            TiempoMaxPartida_But.Visible = false;
                            GridConectados.Visible = false;
                            //panel1.Visible = true;
                            desconnectButton.Visible = false;
                            mapaTbx.Visible = false;
                            info_mapas_pb.Visible = false;
                            data_mapas_info.Visible = false;

                            CrearPartidaBut.Visible = false;
                            label_notificacion.Visible = false;
                            label_notificacion.Text = "";
                            panel1.Visible = true;

                            InicioPersonajesImagenes();

                            string mensaje2 = "0/" + Convert.ToString(idPartida);

                            byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg3);

                            // Nos desconectamos
                            atender.Abort(); // cerramos thread

                            this.BackColor = Color.LightGray;
                            server.Shutdown(SocketShutdown.Both);
                            server.Close();

                            NumInvitados = 0;
                            conectado = false;

                        }));
                        break;

                    case 50:
                        //"50/%d/%s/%s/%s", idpartida,map, resultado, letra_result);
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];
                        string resultado = trozos[3];
                        string letra_res = trozos[4];
                        Invoke(new Action(() =>
                        {
                            formularios1.Remove(formularios1[idPartida]);

                        }));
                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() =>
                                {
                                    form_templo_4Jug[idPartida].FinDePartida(resultado, letra_res);
                                    form_templo_4Jug.Remove(form_templo_4Jug[idPartida]);
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug[idPartida].FinDePartida(resultado, letra_res);
                                    form_volcan_4Jug.Remove(form_volcan_4Jug[idPartida]);

                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug[idPartida].FinDePartida(resultado, letra_res);
                                    form_volcan_3Jug.Remove(form_volcan_3Jug[idPartida]);

                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug[idPartida].FinDePartida(resultado, letra_res);
                                    form_volcan_2Jug.Remove(form_volcan_2Jug[idPartida]);

                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug[idPartida].FinDePartida(resultado, letra_res);
                                    form_cueva_mar_4Jug.Remove(form_cueva_mar_4Jug[idPartida]);

                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado[idPartida].FinDePartida(resultado, letra_res);
                                    form_templohelado.Remove(form_templohelado[idPartida]);

                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug[idPartida].FinDePartida(resultado, letra_res);
                                    form_templohelado_2Jug.Remove(form_templohelado_2Jug[idPartida]);

                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug[idPartida].FinDePartida(resultado, letra_res);
                                    form_templo_2Jug.Remove(form_templo_2Jug[idPartida]);

                                }));
                                break;
                        }

                        break;
                    case 51: //borrar formularios de la lista de formularios
                             // 51/idpartida/mapa
                        idPartida = Convert.ToInt32(trozos[1]);
                        mapa = trozos[2];
                        switch (mapa)
                        {
                            case "Templo":
                                Invoke(new Action(() =>
                                {
                                    form_templo_4Jug.Remove(form_templo_4Jug[idPartida]);
                                }));
                                break;
                            case "Volcan":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_4Jug.Remove(form_volcan_4Jug[idPartida]);

                                }));
                                break;
                            case "Volcan (3Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_3Jug.Remove(form_volcan_3Jug[idPartida]);

                                }));
                                break;
                            case "Volcan (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_volcan_2Jug.Remove(form_volcan_2Jug[idPartida]);

                                }));
                                break;
                            case "Cueva":
                                Invoke(new Action(() =>
                                {
                                    form_cueva_mar_4Jug.Remove(form_cueva_mar_4Jug[idPartida]);

                                }));
                                break;
                            case "Templo Helado":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado.Remove(form_templohelado[idPartida]);

                                }));
                                break;
                            case "Templo Helado (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templohelado_2Jug.Remove(form_templohelado_2Jug[idPartida]);

                                }));
                                break;
                            case "Templo (2Jug)":
                                Invoke(new Action(() =>
                                {
                                    form_templo_2Jug.Remove(form_templo_2Jug[idPartida]);

                                }));
                                break;
                        }
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
            eliminar_but.Visible = false;
            PartidasMapa_But.Visible = false;
            TiempoMaxPartida_But.Visible = false;
            GridConectados.Visible = false;
            //panel1.Visible = true;
            desconnectButton.Visible = false;
            mapaTbx.Visible = false;
            info_mapas_pb.Visible = false;
            data_mapas_info.Visible = false;
        
            CrearPartidaBut.Visible = false;
            label_notificacion.Visible = false;
            label_notificacion.Text = "";
            panel1.Visible = true;

            InicioPersonajesImagenes();

            string mensaje = "0/" + Convert.ToString(idPartida);

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            atender.Abort(); // cerramos thread

            this.BackColor= Color.LightGray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

            NumInvitados = 0;
            conectado = false;
            
        }
        private void PuntMax_But_Click(object sender, EventArgs e)
        {
            string mensaje = "3/" + usernameBox.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void PartidasMapa_But_Click(object sender, EventArgs e)
        {
            data_mapas_info.Visible = false;
            string mensaje = "4/" + mapaTbx.Text;
            if (mapaTbx.Text == "")
            { mensaje = "4/MapaVacio"; }
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void TiempoMaxPartida_But_Click(object sender, EventArgs e)
        {
            string mensaje = "5/";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void LogInButton_Click(object sender, EventArgs e)
        {  
            if (conectado == true)
            {
                
                //MessageBox.Show("Conectado con el servidor");

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
                    //MessageBox.Show("Conectado con el servidor");

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
                //MessageBox.Show("Conectado con el servidor");
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
                CrearPartida f = new CrearPartida();
                f.ShowDialog();
                InvPartida = f.DameNum();
                MessageBox.Show("Haz doble-click en "+ (InvPartida - 1) + " nombres de los jugadores que quieras invitar.");
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
                            GridConectados.ClearSelection();
                            for (int j = 0; j<GridConectados.Rows.Count; j++)
                            { GridConectados.Rows[j].Cells[0].Style.BackColor = Color.White;}
                            invitados = "";
                            But_empezarPartida_activado = false;
                            //}
                        }
                    }
                }
            }
        }

        // * * * * Inicio: Otros Formularios * * * *
        private void PonerEnMarchaFormulario1(int numJugadores)
        { 
            SeleccionPartida f = new SeleccionPartida(idPartida,server);
            formularios1.Add(f);
            f.SetUsername(usernameBox.Text);
            if (soyanfitrion == true)
            {
                f.SetAnfitrion();
            }
            f.SetNumJugadores(numJugadores);
            f.ShowDialog();
        }
        private void PonerEnMarchaForm_Templo4Jug()
        {
            Templo4 t = new Templo4(idPartida, server);
            t.MiPersonaje(formularios1[idPartida].DameMiPersonajeQueHeEscogido());
            // pasamos nombre
            t.SetJug1Nombre(formularios1[idPartida].DameNombreJug1());
            t.SetJug2Nombre(formularios1[idPartida].DameNombreJug2());
            t.SetJug3Nombre(formularios1[idPartida].DameNombreJug3());
            t.SetJug4Nombre(formularios1[idPartida].DameNombreJug4());

            //switch (formularios1[idPartida].DameMiPersonajeQueHeEscogido())
            //{
            //    case 1:
            //        t.SetJug1Nombre(nombre);
            //        break;
            //    case 2:
            //        t.SetJug2Nombre(nombre);
            //        break;
            //    case 3:
            //        t.SetJug3Nombre(nombre);
            //        break;
            //    case 4:
            //        t.SetJug4Nombre(nombre);
            //        break;
            //}   
            form_templo_4Jug.Add(t);
            t.ShowDialog();
        }
        private void PonerEnMarchaForm_Templo2Jug()
        {
            Templo2 t = new Templo2(idPartida, server);
            t.MiPersonaje(formularios1[idPartida].DameMiPersonajeQueHeEscogido());
            // pasamos nombre
            t.SetJug1Nombre(formularios1[idPartida].DameNombreJug1());
            t.SetJug2Nombre(formularios1[idPartida].DameNombreJug2());
            t.SetJug3Nombre(formularios1[idPartida].DameNombreJug3());
            t.SetJug4Nombre(formularios1[idPartida].DameNombreJug4());
            // pasamos las variables bool que establecen qué jugadores juegan
            t.SetJug1Juega(formularios1[idPartida].DameJug1Juega());
            t.SetJug2Juega(formularios1[idPartida].DameJug2Juega());
            t.SetJug3Juega(formularios1[idPartida].DameJug3Juega());
            t.SetJug4Juega(formularios1[idPartida].DameJug4Juega());

            form_templo_2Jug.Add(t);
            t.ShowDialog();
        }
        private void PonerEnMarchaForm_Volcan4Jug()
        {
            Volcan4 t = new Volcan4(idPartida, server);
            t.MiPersonaje(formularios1[idPartida].DameMiPersonajeQueHeEscogido());
            // pasamos nombre
            t.SetJug1Nombre(formularios1[idPartida].DameNombreJug1());
            t.SetJug2Nombre(formularios1[idPartida].DameNombreJug2());
            t.SetJug3Nombre(formularios1[idPartida].DameNombreJug3());
            t.SetJug4Nombre(formularios1[idPartida].DameNombreJug4());

            form_volcan_4Jug.Add(t);
            t.ShowDialog();
        }
        private void PonerEnMarchaForm_Volcan3Jug()
        {
            Volcan3 t = new Volcan3(idPartida, server);
            t.MiPersonaje(formularios1[idPartida].DameMiPersonajeQueHeEscogido());
            // pasamos nombre
            t.SetJug1Nombre(formularios1[idPartida].DameNombreJug1());
            t.SetJug2Nombre(formularios1[idPartida].DameNombreJug2());
            t.SetJug3Nombre(formularios1[idPartida].DameNombreJug3());
            t.SetJug4Nombre(formularios1[idPartida].DameNombreJug4());
            // pasamos las variables bool que establecen qué jugadores juegan
            t.SetJug1Juega(formularios1[idPartida].DameJug1Juega());
            t.SetJug2Juega(formularios1[idPartida].DameJug2Juega());
            t.SetJug3Juega(formularios1[idPartida].DameJug3Juega());
            t.SetJug4Juega(formularios1[idPartida].DameJug4Juega());

            form_volcan_3Jug.Add(t);
            t.ShowDialog();
        }
        private void PonerEnMarchaForm_Volcan2Jug()
        {
            Volcan2 t = new Volcan2(idPartida, server);
            t.MiPersonaje(formularios1[idPartida].DameMiPersonajeQueHeEscogido());
            // pasamos nombre
            t.SetJug1Nombre(formularios1[idPartida].DameNombreJug1());
            t.SetJug2Nombre(formularios1[idPartida].DameNombreJug2());
            t.SetJug3Nombre(formularios1[idPartida].DameNombreJug3());
            t.SetJug4Nombre(formularios1[idPartida].DameNombreJug4());
            // pasamos las variables bool que establecen qué jugadores juegan
            t.SetJug1Juega(formularios1[idPartida].DameJug1Juega());
            t.SetJug2Juega(formularios1[idPartida].DameJug2Juega());
            t.SetJug3Juega(formularios1[idPartida].DameJug3Juega());
            t.SetJug4Juega(formularios1[idPartida].DameJug4Juega());

            form_volcan_2Jug.Add(t);
            t.ShowDialog();
        }
        private void PonerEnMarchaForm_TemploHelado()
        {
            TemploHelado t = new TemploHelado(idPartida, server);
            t.MiPersonaje(formularios1[idPartida].DameMiPersonajeQueHeEscogido());
            // pasamos nombre
            t.SetJug1Nombre(formularios1[idPartida].DameNombreJug1());
            t.SetJug2Nombre(formularios1[idPartida].DameNombreJug2());
            t.SetJug3Nombre(formularios1[idPartida].DameNombreJug3());
            t.SetJug4Nombre(formularios1[idPartida].DameNombreJug4());
            // pasamos las variables bool que establecen qué jugadores juegan
            t.SetJug1Juega(formularios1[idPartida].DameJug1Juega());
            t.SetJug2Juega(formularios1[idPartida].DameJug2Juega());
            t.SetJug3Juega(formularios1[idPartida].DameJug3Juega());
            t.SetJug4Juega(formularios1[idPartida].DameJug4Juega());

            form_templohelado.Add(t);
            t.ShowDialog();
        }
        private void PonerEnMarchaForm_TemploHelado2Jug()
        {
            TemploHelado2 t = new TemploHelado2(idPartida, server);
            t.MiPersonaje(formularios1[idPartida].DameMiPersonajeQueHeEscogido());
            // pasamos nombre
            t.SetJug1Nombre(formularios1[idPartida].DameNombreJug1());
            t.SetJug2Nombre(formularios1[idPartida].DameNombreJug2());
            t.SetJug3Nombre(formularios1[idPartida].DameNombreJug3());
            t.SetJug4Nombre(formularios1[idPartida].DameNombreJug4());
            // pasamos las variables bool que establecen qué jugadores juegan
            t.SetJug1Juega(formularios1[idPartida].DameJug1Juega());
            t.SetJug2Juega(formularios1[idPartida].DameJug2Juega());
            t.SetJug3Juega(formularios1[idPartida].DameJug3Juega());
            t.SetJug4Juega(formularios1[idPartida].DameJug4Juega());

            form_templohelado_2Jug.Add(t);

            // rellenamos los otros vectores 
            //TemploHelado t_1 = new TemploHelado(idPartida, server);
            //form_templohelado.Add(t_1);
            //Templo2 t_2 = new Templo2(idPartida, server);
            //form_templo_2Jug.Add(t_2);
            //Templo4 t_3 = new Templo4(idPartida, server);
            //form_templo_4Jug.Add(t_3);
            //Cueva_Maritima t_4 = new Cueva_Maritima(idPartida, server);
            //form_cueva_mar_4Jug.Add(t_4);
            //Volcan2 t_5 = new Volcan2(idPartida, server);
            //form_volcan_2Jug.Add(t_5);
            //Volcan3 t_6 = new Volcan3(idPartida, server);
            //form_volcan_3Jug.Add(t_6);
            //Volcan4 t_7 = new Volcan4(idPartida, server);
            //form_volcan_4Jug.Add(t_7);

            t.ShowDialog();
        }
        private void PonerEnMarchaForm_CuevaMaritima()
        {
            Cueva_Maritima t = new Cueva_Maritima(idPartida, server);
            t.MiPersonaje(formularios1[idPartida].DameMiPersonajeQueHeEscogido());
            // pasamos nombre
            t.SetJug1Nombre(formularios1[idPartida].DameNombreJug1());
            t.SetJug2Nombre(formularios1[idPartida].DameNombreJug2());
            t.SetJug3Nombre(formularios1[idPartida].DameNombreJug3());
            t.SetJug4Nombre(formularios1[idPartida].DameNombreJug4());
            // pasamos las variables bool que establecen qué jugadores juegan
            t.SetJug1Juega(formularios1[idPartida].DameJug1Juega());
            t.SetJug2Juega(formularios1[idPartida].DameJug2Juega());
            t.SetJug3Juega(formularios1[idPartida].DameJug3Juega());
            t.SetJug4Juega(formularios1[idPartida].DameJug4Juega());

            form_cueva_mar_4Jug.Add(t);
            t.ShowDialog();
        }
        private void PonerEnMarchaForm_PruebaTecla()
        {

            prueba_Teclas pt = new prueba_Teclas(idPartida, server);
            form_prueba_tecl.Add(pt);
            // para ir a la par con el resto de listas de mapas, añadimos mapas vacíos a esos mapas para que ocupen las columnas    ----------------------------------- ?????????????????????????????????????????
            //Templo t = new Templo(idPartida, server);
            //form_templo_4Jug.Add(t);
            //Volcan v = new Volcan(idPartida, server);
            //form_volcan_4Jug.Add(v);
            //Cueva_Maritima cm = new Cueva_Maritima(idPartida, server);
            //form_cueva_mar_4Jug.Add(cm);

            pt.ShowDialog();
        }
        // * * * * Fin: Otros Formularios * * * *

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
            eliminar_but.Visible = false;
            PartidasMapa_But.Visible = false;
            TiempoMaxPartida_But.Visible = false;
            GridConectados.Visible = false;
            //panel1.Visible = true;
            desconnectButton.Visible = false;
            CrearPartidaBut.Visible = false;
            label_notificacion.Visible = false;
            label_notificacion.Text = "";

            if (conectado == true)
            {
                string mensaje = "0/" + Convert.ToString(idPartida);

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                atender.Abort(); // cerramos thread

                this.BackColor = Color.LightGray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            conectado = false;
        }

        private void timer_personaj_Tick(object sender, EventArgs e)
        {
            Jug1.Left = Jug1.Left + 7;
            Jug2.Left = Jug2.Left + 7;
            Jug3.Left = Jug3.Left + 7;
            Jug4.Left = Jug4.Left + 7;

            if (Jug4.Left > this.ClientSize.Width)
            {
                label3.Visible = true;
                lbl_lista_con.Visible = false;
                PuntMax_But.Visible = true;
                eliminar_but.Visible = true;
                PartidasMapa_But.Visible = true;
                TiempoMaxPartida_But.Visible = true;
                desconnectButton.Visible = true;
                panel1.Visible = false;
                GridConectados.Visible = true;
                CrearPartidaBut.Visible = true;
                label_notificacion.Visible = true;
                label_notificacion.Text = "";
                mapaTbx.Visible = true;
                info_mapas_pb.Visible = true;
               this.BackColor = Color.Green;
                this.Controls.Remove(Jug1);
                this.Controls.Remove(Jug2);
                this.Controls.Remove(Jug3);
                this.Controls.Remove(Jug4);

                timer_personaj.Stop();
            }

        }

        private void timer_saludo_Tick(object sender, EventArgs e)
        {
            timer_personaj.Interval = 30;
            timer_personaj.Start();

            Jug1.Image = Image.FromFile("Fireboy_corriendo_der.gif");
            Jug2.Image = Image.FromFile("Watergirl_corriendo_der.gif");
            Jug3.Image = Image.FromFile("Rockboy_corriendo_der.gif");
            Jug4.Image = Image.FromFile("Cloudgirl_corriendo_der.gif");

            timer_saludo.Stop();
        }

        private void info_mapas_pb_MouseEnter(object sender, EventArgs e)
        {
            data_mapas_info.Visible = true;
            data_mapas_info.ClearSelection();
        }

        private void eliminar_but_Click(object sender, EventArgs e)
        {
            string mensaje = "25/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }
    }
}
