using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoSO
{
    public partial class CrearPartida : Form
    {
        int r = 0;
        
        public CrearPartida()
        {
            InitializeComponent();
        }

        private void DosJugadores_Click(object sender, EventArgs e)
        {
            r = 2;
            DosJugadores.BackColor = Color.SteelBlue;
            TresJugadores.BackColor = Color.Silver;
            CuatroJugadores.BackColor = Color.Silver;
        }

        private void TresJugadores_Click(object sender, EventArgs e)
        {
            r = 3;
            DosJugadores.BackColor = Color.Silver;
            TresJugadores.BackColor = Color.SteelBlue;
            CuatroJugadores.BackColor = Color.Silver;
        }

        private void CuatroJugadores_Click(object sender, EventArgs e)
        {
            r = 4;
            DosJugadores.BackColor = Color.Silver;
            TresJugadores.BackColor = Color.Silver;
            CuatroJugadores.BackColor = Color.SteelBlue;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }
        
        public int DameNum()
        {
            return r;
        }
    }
}
