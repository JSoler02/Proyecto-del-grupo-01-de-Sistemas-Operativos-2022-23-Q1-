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
using System.Net.Sockets;

namespace ProyectoSO
{
    public partial class Mapa1 : Form
    {
        Socket server;
        public Mapa1()
        {
            InitializeComponent();
        }
  
        public void PassarSocket(Socket s)
        {
            // passamos el socket del menu principal a la lista de conectados
            this.server = s;
        }


    }
}
