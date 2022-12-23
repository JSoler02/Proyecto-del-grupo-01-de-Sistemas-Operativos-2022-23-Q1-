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
    public partial class prueba_Teclas : Form
    {
        int idPartida;
        Socket server; // declaramos socket
        int nForm;

        public prueba_Teclas(int nForm, Socket server)
        {
            InitializeComponent();
            this.nForm = nForm; // num de form que em donen --> l'afegeixo en els missatges de peticio de servei
            this.server = server;
        }

        private void prueba_Teclas_KeyDown(object sender, KeyEventArgs e)
        {
            string mensaje = "";
            if (e.KeyCode == Keys.Left)
            {
                label_Envio.Text = "toco la izquierda";
                mensaje = "35/" + idPartida;
                
            }
            if (e.KeyCode == Keys.Right)
            {
                label_Envio.Text = "toco la derecha";
                mensaje = "37/" + idPartida;
            }
            if (e.KeyCode == Keys.Up)
            {
                label_Envio.Text = "toco la arriba";
                mensaje = "39/" + idPartida;

            }
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void prueba_Teclas_KeyUp(object sender, KeyEventArgs e)
        {
            string mensaje = "";

            // Cuando levantemos las teclas, las booleanas se vuelven False
            if (e.KeyCode == Keys.Left)
            {
                label_Envio.Text = "suelto la izquierda";
                mensaje = "36/" + idPartida;

            }
            if (e.KeyCode == Keys.Right)
            {
                label_Envio.Text = "suelto la derecha";
                mensaje = "38/" + idPartida;

            }
            if (e.KeyCode == Keys.Up)
            {
                label_Envio.Text = "suelto la arriba";
                mensaje = "40/" + idPartida;

            }
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }
        public void TocaDerecha_otro()
        {
            Invoke(new Action(() =>
            {
                label_Recibo.Text = "tocan la derecha";
            }));
        }
        public void TocaIzquierda_otro()
        {
            Invoke(new Action(() =>
            {
                label_Recibo.Text = "tocan la Izquierda";
            }));

        }
        public void TocaArriba_otro()
        {
            Invoke(new Action(() =>
            {
                label_Recibo.Text = "tocan la arriba";
            }));

        }
        public void SueltaDerecha_otro()
        {
            MessageBox.Show("Prueba teclas: Otro suelta la derecha: 118;");
            Invoke(new Action(() =>
            {
                label_Recibo.Text = "Sueltan la derecha";
            }));

        }
        public void SueltaIzquierda_otro()
        {
            Invoke(new Action(() =>
            {
                label_Recibo.Text = "Sueltan la Izquierda"; ;
            }));

        }
        public void SueltaArriba_otro()
        {
            Invoke(new Action(() =>
            {
                label_Recibo.Text = "Sueltan la arriba";
            }));
        }
    }
}
