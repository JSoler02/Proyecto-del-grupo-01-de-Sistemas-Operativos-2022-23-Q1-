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
    public partial class Volcan : Form
    {
        int nForm;
        Socket server;

        int idPartida;
        public Volcan(int nForm, Socket server)
        {
            InitializeComponent();
            this.nForm = nForm;
            this.server = server;
        }
        // Chat
        private void EnviarChatBut_Click(object sender, EventArgs e)
        {
            if (chatbox.Text != null)
            {
                string mensaje_chat = "20/" + nForm + "/5/" + idPartida + "/" + chatbox.Text;
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
    }
}
