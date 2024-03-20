using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P_2_4_en_raya
{
	public partial class Fin_Juego : Form
	{
		private Sonidos musica;
		//	private byte win_emp 1 gano 2 empate
		public Fin_Juego(byte win_emp, byte player, Sonidos sonidos)
		{
			InitializeComponent();

			this.musica = sonidos;
			Mostrar_Panel(win_emp, player);
			AnimacionLogos(win_emp);
			SonidoWinyMusica(win_emp);


		}

		private async void AnimacionLogos(byte Logo)
		{
			Guna2ImageButton imglogo;

			if (Logo == 1) {
				imglogo = Img_Gane;

			}
			else
			{
				imglogo = Img_Empate;

			}

			do
			{
				imglogo.ImageSize = new Size(115, 115);
				imglogo.HoverState.ImageSize = new Size(115, 115);
				imglogo.PressedState.ImageSize = new Size(115, 115);

				await Task.Delay(160);

				imglogo.ImageSize = new Size(110, 110);
				imglogo.HoverState.ImageSize = new Size(110, 110);
				imglogo.PressedState.ImageSize = new Size(110, 110);

				await Task.Delay(160);

				imglogo.ImageSize = new Size(115, 115);
				imglogo.HoverState.ImageSize = new Size(115, 115);
				imglogo.PressedState.ImageSize = new Size(115, 115);

				await Task.Delay(160);

				imglogo.ImageSize = new Size(120, 120);
				imglogo.HoverState.ImageSize = new Size(120, 120);
				imglogo.PressedState.ImageSize = new Size(120, 120);

				await Task.Delay(160);

			} while (true);

		}

		private void Mostrar_Panel(byte panel, byte player)
		{
			if (panel == 1)
			{	
				Panel_Empate.Size = new Size(351, 0);
				Panel_Gane.Size = new Size(351, 150);

				if (player == 1)
				{
					Imagen_Ganador.Image = Properties.Resources.D_WIN;
				}
				else
				{
					Imagen_Ganador.Image = Properties.Resources.T_WIN;
				}
				

				
			}
			else
			{
				Panel_Gane.Size = new Size(351, 0);
				Panel_Empate.Size = new Size(351, 150);
				
			}
		}

		private async void SonidoWinyMusica(byte w_e)
		{

			if(w_e == 1) // 1 victoria 
			{
				musica.EfectoVictoria();

				await Task.Delay(2800);

				musica.ContinuarMusicaFondos(3);
			}

			else
			{
				await Task.Delay(10);
				musica.ContinuarMusicaFondos(3);
			}
		}

		private void BTN_CONTINUAR_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			musica.DetenerMusicaFondos();
			this.Close();
		}
	}
}
