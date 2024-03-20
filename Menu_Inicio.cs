using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace P_2_4_en_raya
{
	public partial class Menu_Inicio : Form
	{
	//	private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		private byte imagenefectos = 1;// 1 es encendido
		private byte imagenemusica = 1;// 1 es encendido
		private Sonidos musica = new Sonidos();
		private Reanudar reanudar = new Reanudar();
		private byte J = 1;

		private bool v = true;
		public Menu_Inicio()
		{
			InitializeComponent();
			AnimacionLateralAsync();
			
			

		}



		private async Task AnimacionLateralAsync()
		{
			
				if (J == 1)
				{
					J = 2;
				}
				else
				{
					J = 1;
				}

				for (byte i = 1; i < 12; i++)
				{
					string nombreBoton = "A" + i; //se concatena el nombre con numnero para obtener el nombre conmpleto del boton
					Guna2Button boton = (Guna2Button)this.Controls.Find(nombreBoton, true)[0];
					if (J == 1)
					{
						boton.DisabledState.FillColor = Color.Transparent;
						boton.BackgroundImage = Properties.Resources.D_WIN;
						boton.ShadowDecoration.Enabled = true;
					}
					else
					{
						boton.DisabledState.FillColor = Color.Transparent;
						boton.BackgroundImage = Properties.Resources.T_WIN;
						boton.ShadowDecoration.Enabled = true;
				}

					await Task.Delay(200);

				boton.ShadowDecoration.Enabled = false;
				boton.BackgroundImage = null;
				boton.DisabledState.FillColor = Color.DimGray;

				}

				AnimacionLateralAsync();


		}
		

		private void BTN_EXIT_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			Application.Exit();
		}





		private void BTN_SALIR_SONIDO_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			BTN_JUGAR.Enabled = true;
			PANEL_SONIDOS.Size = new Size(0, 0);
		}

		

		private void BTN_SONIDOS_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			BTN_JUGAR.Enabled = false;
			PANEL_SONIDOS.Size = new Size(204, 225);
			

		}
		private void BTN_EFECTOS_Click(object sender, EventArgs e)
		{

			// inicia en 1 para efecto practicos
			if (imagenefectos == 1) // efectos esta encendido
			{
				// apagar
				BTN_EFECTOS.Image = Properties.Resources.sin_sonido;
				musica.DetenerEfectos();
				imagenefectos = 2;
			}
			else
			{
				//enceder
				BTN_EFECTOS.Image = Properties.Resources.volumen_medio;
				musica.ContinuarEfectos();
				imagenefectos = 1;
			}

		}
		private void BTN_MUSICA_Click(object sender, EventArgs e)
		{

			// inicia en 1 para efecto practicos
			if (imagenemusica == 1) // efectos esta encendido
			{
				// apagar
				BTN_MUSICA.Image = Properties.Resources.musica_apagada;
			//	musica.DetenerMusicaFondos();
				musica.DesactivarMusicaFondo();
				imagenemusica = 2;
			}
			else
			{
				//enceder
				BTN_MUSICA.Image = Properties.Resources.musica;
				musica.ActivarMusicaFondo();
				musica.ContinuarMusicaFondos(1);
				imagenemusica = 1;
			}
		}

		private void BTN_REANUDAR_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			if (reanudar.ValidarHayDatos()){
				reanudar.nueva_Partida = false;
				musica.DetenerMusicaFondos();
				Ir_Juego();
			}
			else
			{
				MessageBox.Show("No hay partida guardada.","Alerta",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}

		}

		private void BTN_JUGAR_Click(object sender, EventArgs e)
		{
			musica.DetenerMusicaFondos();
			musica.EfectoClick();
			reanudar.nueva_Partida = true;
			Ir_Juego();
		}

		private void Ir_Juego()
		{
			Juego juego = new Juego(reanudar,this,musica);
			Hide(); // Oculta el formulario actual
			juego.Show(); // Muestra el nuevo formulario
		}

		private void Menu_Inicio_VisibleChanged(object sender, EventArgs e)
		{
			if (v)
			{
			musica.ContinuarMusicaFondos(1);
				v = false;
			}
			else
			{
				v = true;
			}
			
		}
	}
}
