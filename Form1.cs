using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;

namespace P_2_4_en_raya
{
	public partial class Form1 : Form
	{
		private byte[,] tablero = new byte[6,7]; //Matriz en bytes espacio en memoria de 8 bits para gestionar memoria
		private Stack<int> movimientos = new Stack<int>();
		
		private Queue<byte> turnos = new Queue<byte>();
		private List<Guna2Button> fichas = new List<Guna2Button>();
		private Image jugador2 = P_2_4_en_raya.Properties.Resources.TIGRE;
		private Image jugador1 = P_2_4_en_raya.Properties.Resources.DRAGON;
		public Form1()
		{
			InitializeComponent();
			Agregar_botones_ficha(); //metodo para agregar todas referencias a los botones en lista ficha
			AgregarTurnos();
			Mostrar_Texto_Turno();
		}

		private void Guna2ControlBox3_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void BTN_COLUM_A_Click(object sender, EventArgs e)
		{
			Mostrar_En_Tablero(0);
            
		}

		private void BTN_COLUM_B_Click(object sender, EventArgs e)
		{
			Mostrar_En_Tablero(1);
		}

		private void BTN_COLUM_C_Click(object sender, EventArgs e)
		{
			Mostrar_En_Tablero(2);
		}

		private void BTN_COLUM_D_Click(object sender, EventArgs e)
		{
			Mostrar_En_Tablero(3);
		}

		private void BTN_COLUM_E_Click(object sender, EventArgs e)
		{
			Mostrar_En_Tablero(4);
		}

		private void BTN_COLUM_F_Click(object sender, EventArgs e)
		{
			Mostrar_En_Tablero(5);
		}

		private void BTN_COLUM_G_Click(object sender, EventArgs e)
		{
			Mostrar_En_Tablero(6);
		}



		#region Metodos


		private sbyte FilaDesocupada(byte columna) // devuelve la fila vacia del tablero, hay que pasar la columna
		{
			sbyte fila_null = -1; // devuelve -1 si esta llena la columna

			for( sbyte i = 0; i < 6; i++ ) // se recore las filas
			{
				if (tablero[5-i, columna] == 0) //se valida de la uitlma a la primera
				{
					fila_null = (sbyte)(5 - i); // conversion explicita a sbyte
					break;
				}

			}
			return fila_null;

		}

		private void Mostrar_En_Tablero(byte columna)
		{
			string posicion;

			switch(columna)
			{
				case 0:
					if(FilaDesocupada(columna) != -1)
					{
						posicion = FilaDesocupada(columna) + "," + columna;
						tablero[FilaDesocupada(columna), columna] = turnos.Peek();
						AnimacionCaidaFichas(columna,posicion);
						
					}
					else
					{
						AlertaLleno(" ");
					}
					
					break;

				case 1:

					if (FilaDesocupada(columna) != -1)
					{
						posicion = FilaDesocupada(columna) + "," + columna;
						tablero[FilaDesocupada(columna), columna] = turnos.Peek();
						AnimacionCaidaFichas(columna, posicion);
					}
					else
					{
						AlertaLleno(" ");
					}

					break;

				case 2:

					if (FilaDesocupada(columna) != -1)
					{
						posicion = FilaDesocupada(columna) + "," + columna;
						tablero[FilaDesocupada(columna), columna] = turnos.Peek();
						AnimacionCaidaFichas(columna, posicion);
					}
					else
					{
						AlertaLleno(" ");
					}

					break;

				case 3:

					if (FilaDesocupada(columna) != -1)
					{
						posicion = FilaDesocupada(columna) + "," + columna;
						tablero[FilaDesocupada(columna), columna] = turnos.Peek();
						AnimacionCaidaFichas(columna, posicion);
					}
					else
					{
						AlertaLleno(" ");
					}

					break;

				case 4:

					if (FilaDesocupada(columna) != -1)
					{
						posicion = FilaDesocupada(columna) + "," + columna;
						tablero[FilaDesocupada(columna), columna] = turnos.Peek();
						AnimacionCaidaFichas(columna, posicion);
					}
					else
					{
						AlertaLleno(" ");
					}

					break;

				case 5:

					if (FilaDesocupada(columna) != -1)
					{
						posicion = FilaDesocupada(columna) + "," + columna;
						tablero[FilaDesocupada(columna), columna] = turnos.Peek();
						AnimacionCaidaFichas(columna, posicion);
					}
					else
					{
						AlertaLleno(" ");
					}

					break;

				case 6:

					if (FilaDesocupada(columna) != -1)
					{
						posicion = FilaDesocupada(columna) + "," + columna;
						tablero[FilaDesocupada(columna), columna] = turnos.Peek();
						AnimacionCaidaFichas(columna, posicion);
					}
					else
					{
						AlertaLleno(" ");
					}

					break;

			}
		}


		private void AlertaLleno(string columna)
		{
			MessageBox.Show("Columna " + columna + " Llena", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}


		#endregion

		private void Mostrar_Ficha_Ultima(string posicion, byte jugadorturno)
		{
			// expresión lambda que define la condición para la búsqueda verifica si el valor de la propiedad Tag
			// del botón convertido a cadena es igual a posicion y almacena el boton .

			Guna2Button ficha_now = fichas.Find(x => x.Tag.ToString().Trim() == posicion); 
			if(jugadorturno == 1)
			{
				ficha_now.BackgroundImage = jugador1;
			}else
			{
				ficha_now.BackgroundImage = jugador2;
			}

			Mostrar_Texto_Turno();

		}


		private async void AnimacionCaidaFichas(byte columna, string posicion_ultima_ficha)
		{
			Image jugador;
			byte filas = 0;

			for (byte i = 0; i < 6; i++) // se recorren las filas
			{
				if (tablero[5 - i, columna] == 0) //se valida de la uitlma a la primera
				{
					filas++;
				}

			}

			if (turnos.Peek() == 1) //se valida cual es el jugador actual y asigna una varible con la imagen del jugador actual
			{
				jugador = jugador1;
			}
			else
			{
				jugador = jugador2;
			}
				string nombreBoton = "BTN_C_" + columna.ToString();  //se obtiene el nombre de la ficha donde empieza la caida  
			Guna2Button boton = (Guna2Button)this.Controls.Find(nombreBoton, true)[0]; // se guarda la referencia

			boton.BackgroundImage = jugador; // se muestra

			await Task.Delay(100); // se espera 0.1 segundos 

			boton.BackgroundImage = null; // se oculta la imagen 

			string posicion_caida; // variable con la posicion de la ficha en la caida 
            for (byte i = 0; i < filas; i++) // ciclo para mostrar y ocultar la ficha en los distintos puntos de la caida
            {

				posicion_caida = i + "," + columna; //obtiene la posicion
				Guna2Button ficha_caida = fichas.Find(x => x.Tag.ToString().Trim() == posicion_caida); // se busca la referencia con un lamda que valida que el tag y la posicion coincidan;

					ficha_caida.BackgroundImage = jugador; //se muestra la ficha

				await Task.Delay(100); // se espera 0.1 segundos 

				ficha_caida.BackgroundImage = null; // se oculta la ficha
				
			}

			Mostrar_Ficha_Ultima(posicion_ultima_ficha, turnos.Dequeue());

		}











		private void AgregarTurnos() //agregar todos los turnos de la partida
		{
			byte jugador_inicial = 1;
			if (turnos.Count == 0) //valida si la cola tiene elementos
			{
				for (int i = 0; i < 42; i++)  //son 42 truenos 
				{
					turnos.Enqueue(jugador_inicial);

					if (jugador_inicial == 1) // se cambia de jugador al terminar de guardar el turno 
					{
						jugador_inicial = 2;
					}
					else
					{
						jugador_inicial = 1;
					}
				}

			}
			else
			{
				// se dejo para implementar 
			}

		}


		private void Agregar_botones_ficha()
		{
			for (int i = 1; i <= 42; i++) //total de fichas de tablero 42
			{
				string nombreBoton = "BTN_F_" + i.ToString(); //se concatena el nombre con numnero para obtener el nombre conmpleto del boton
				Guna2Button boton = (Guna2Button)this.Controls.Find(nombreBoton, true)[0];  //se busca el boton y se guarda en una variable guna
				fichas.Add(boton); //se agrega a al lista
			}
		}

		private void Mostrar_Texto_Turno()
		{
			if (turnos.Peek() == 1)
			{
				TXT_TURNO.Text = "Turno: Dragon";
			}
			else
			{
				TXT_TURNO.Text = "Turno: Tigre";
			}

				
		}
	}
}
