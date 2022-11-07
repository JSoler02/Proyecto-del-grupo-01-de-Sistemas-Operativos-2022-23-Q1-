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
    public partial class ListaCon : Form
    {
        Socket server;
        public ListaCon()
        {
            InitializeComponent();
        }
        /*
         * Hacer un DATATABLE que se llene al dar al botón de Lista de Conectados
         * 
         * Poner usuario a conectado (necesitamos nombre y socket)
         mensaje a enviar del tipo: 6/nombreconectado/4
         mensaje recibido: "Conectado!" o "No se ha podido conectar al usuario" 
         ↑ devolverlo directamente desde el C en la respuesta
         
         * Eliminar usuario conectado (necesitamos nombre)
         mensaje a enviar del tipo: 7/nombreaeliminar
         mensaje recibido: "Desconectado." o "No se ha podido desconectar" 
         ↑ devolverlo directamente desde el C en la respuesta
        
         * Dame socket de usuario (necesitamos nombre) ??
         * (Creo que esto se usará para enviar mensaje de jugar entre jugadores)
         mensaje a enviar del tipo: 8/nombreconectado
         
         * Dame listado de nombre de los usuarios conectados
         mensaje a enviar del tipo: 9/
         mensaje recibido: 3/Juan/Pedro/Maria (el numero inicial nos indica el número de usuarios conectados)
         
         * Dame listado de sockets de usuario conectados 
         * (necesitamos lista de conectados - la que nos devuelve la función anterior)
         mensaje a enviar del tipo: 10/3/Juan/Pedro/Maria
         mensaje recibido: 3/5/1/3
         */

        // Funciones Lista Conectados

        public void PassarSocket(Socket s)
        {
            // passamos el socket del menu principal a la lista de conectados
            this.server = s;
        }

        private void ListaCon_Load(object sender, EventArgs e)
        {
            GridConectados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            GridConectados.RowHeadersVisible = false;
            GridConectados.ColumnCount = 2; 
            GridConectados.Columns[1].HeaderText = "Username";
            GridConectados.Columns[0].HeaderText = "Socket ID";
            
            string mensaje = "6/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            MessageBox.Show(mensaje);

            // string mensaje = "3/Juan/Pedro/Maria"; 
            // el numero inicial nos indica el número de usuarios conectados
            int num = Convert.ToInt32(mensaje.Split('/')[0]);
            GridConectados.RowCount = num; 

            for (int i = 0; i < num; i++)
            {                
                string nombre = Convert.ToString(mensaje.Split('/')[i+1]);
                GridConectados.Rows[i].Cells[1].Value = nombre;

            }

            GridConectados.Refresh();
        }
    }
}
