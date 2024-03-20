using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P_2_4_en_raya
{
	public partial class Pausa : Form
	{
		private Sonidos sonidos;
		private Reanudar reanudar = new Reanudar();
		private byte[,] tablero = new byte[6, 7]; //Matriz en bytes espacio en memoria de 8 bits para gestionar memoria
		private Stack<string> movimientos = new Stack<string>(); // pila 
		private Queue<byte> turnos = new Queue<byte>();// cola

		public Pausa(Queue<byte> turnos, byte[,] tablero, Stack<string> movimientos, Sonidos sonidos )
		{
			InitializeComponent();
			this.sonidos = sonidos;
			this.turnos = turnos;
			this.tablero = tablero;
			this.movimientos = movimientos;
		}

		private void BTN_REANUDAR_Click(object sender, EventArgs e)
		{
			sonidos.EfectoClickPausaBtn();
			this.Close();
		}

		private void BTN_SALIR_SAVE_Click(object sender, EventArgs e)
		{
			sonidos.EfectoClickPausaBtn();
			sonidos.DetenerMusicaFondos();
			reanudar.GuardarDatosPartida(tablero, movimientos, turnos);
			
			Tag = "SS";

            this.Close();
		}
	}
}
