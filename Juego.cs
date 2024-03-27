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
using System.Xml;

namespace P_2_4_en_raya
{
	public partial class Juego : Form
	{
		private Sonidos musica;
		private Reanudar reanudar;
		private Menu_Inicio menu_inicio;

		private string[] letrasColumnas = { "A", "B", "C", "D", "E", "F", "G" };

		private byte[,] tablero = new byte[6,7]; //Matriz en bytes espacio en memoria de 8 bits para gestionar memoria
		private Stack<string> movimientos = new Stack<string>(); // pila 
		private Queue<string> posiones_gane_fichas = new Queue<string>();
		private Queue<byte> turnos = new Queue<byte>();// cola
		private List<Guna2Button> fichas = new List<Guna2Button>();
		private Image jugador2 = Properties.Resources.TIGRE;
		private Image jugador1 = Properties.Resources.DRAGON;


		public bool presionado = false; // bandera para que solo se presione el boton una vez
		

		public Juego(Reanudar reanudar , Menu_Inicio menu, Sonidos sonidos)
		{
			InitializeComponent();
			this.musica = sonidos;
			 this.reanudar = reanudar;
			this.menu_inicio = menu;
			musica.ContinuarMusicaFondos(2);
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
			musica.EfectoClick();
			Mostrar_En_Tablero(0);
			
			//reanudar.GuardarDatosPartida(tablero, movimientos, turnos);
		}

		private void BTN_COLUM_B_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			Mostrar_En_Tablero(1);
			
		}

		private void BTN_COLUM_C_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			Mostrar_En_Tablero(2);
		}

		private void BTN_COLUM_D_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			Mostrar_En_Tablero(3);
		}

		private void BTN_COLUM_E_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			Mostrar_En_Tablero(4);
		}

		private void BTN_COLUM_F_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
			Mostrar_En_Tablero(5);
		}

		private void BTN_COLUM_G_Click(object sender, EventArgs e)
		{
			musica.EfectoClick();
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

			if (FilaDesocupada(columna) == -1) //valida si la columna esta llena 
			{
				AlertaColumLlena(letrasColumnas[columna]); // bloquea el boton de la columna llena
			}
			else
			{
				Desactivarbotones(); // descativa todos los botones
				sbyte fila = FilaDesocupada(columna); // obtiene la fila vacia de la columna actual
				string posicion = fila + "," + columna; //guarda la posion actual

				tablero[fila, columna] = turnos.Peek(); // guarda la ficha del jugador actual en la matriz tablero
				movimientos.Push(posicion + "," + turnos.Peek()); // en movimientos se guarda la secuencia (fila,columna,jugador) ejemplo = 5,0,1  // usar esto para validar cuando se gana o no
				AnimacionCaidaFichas(columna, posicion);

				if(Validar_Gane((byte)fila, columna))
				{
					Mostrar_Fin_Juego(1, tablero[fila, columna]);
					
				}
				//else
				//{

				//}

				AlertaTableroLleno();
			}

		}

		private async Task Mostrar_Fin_Juego(byte win_empaty,byte player)
		{
			
			presionado = true; //No permitir que el boton de pausa funcione mas.

			if (win_empaty == 1) // si es 1 es gane 2  empate
			{
				await Task.Delay(1000);
			}
			Fin_Juego fin_Juego = new Fin_Juego(win_empaty,player,musica,this);

			fin_Juego.TopLevel = false; // eliminamos el nuvel de superior de la ventana

			Panel_p_y_f.Controls.Add(fin_Juego); // agregamos el formulario a un panel 

			musica.DetenerMusicaFondos();

			fin_Juego.Show();
			

			Panel_p_y_f.Location = new Point(120,80);

			Panel_p_y_f.BorderRadius = 15;
			Panel_p_y_f.Size = new Size(fin_Juego.Size.Width, fin_Juego.Size.Height);

			fin_Juego.FormClosed += Fin_Juego_FormClosed; // Heredamos el evento FORMCLOSED de cierre del formularo y utilizamos el evento
		}

		private void Fin_Juego_FormClosed(object sender, FormClosedEventArgs e)
		{
			menu_inicio.Show();

			this.Close();
		}

		private void AlertaColumLlena(string columna)
		{

			string nombreBoton = "BTN_COLUM_" + columna; //se concatena el nombre con numnero para obtener el nombre conmpleto del boton
			Guna2Button boton = (Guna2Button)this.Controls.Find(nombreBoton, true)[0];  //se busca el boton y se guarda en una variable guna
			
			boton.Enabled = false; // se desactiva el boton de la coulmna
		}

		private void AlertaTableroLleno() //Verificar que el tablero esta lleno
		{
			byte camposvacios_cantidad = 0;

			for (byte i = 0; i < tablero.GetLength(0); i++) //filas devuelve GetLength(0)
			{
				for(byte j = 0; j < tablero.GetLength(1); j++) // columnas devuelve GetLength(1)
				{
					if (tablero[i,j] == 0) //Vailda si hay campos vacios en el tablero (matriz)
					{
						camposvacios_cantidad++;  // si hay campos vacios agrega
                       
                    }
				}
			}

			if (camposvacios_cantidad == 0)
			{
				Mostrar_Fin_Juego(2, 2);
			}



		}


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
			Activarbotones();
		}


		private async void AnimacionCaidaFichas(byte columna, string posicion_ultima_ficha)
		{
			Image jugador;
			byte filas = 0;

			for (byte i = 0; i < tablero.GetLength(0); i++) // se recorren las filas
			{
				if (tablero[(tablero.GetLength(0) -1) -i, columna] == 0) //se valida de la ultima a la primera
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


		private bool Validar_Gane(byte fila, byte columna)
		{
			byte ficha_valor = tablero[fila, columna]; // obtiene el valor de la ficha 1 o 2 dependiendo del jugador

			//vectores para buscar en las 8 direcciones
			sbyte[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
			sbyte[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

			/*
				direcciones 

				indice 0 diagonal superior izquierda 		indice 4 derecha
				indice 1 arriba ,							indice 5 diagonal inferior izquierda 
				indice 2 diagonal superior derecha	 		indice 6 abajo
				indice 3 iquierda					 		indice 7 diagonal inferior derecha
			 */

			for (byte direccion = 0; direccion < 8; direccion++)
			{
				byte contador = 1; // Inicia en 1 por el valor en la posición dada

				posiones_gane_fichas.Enqueue(fila + "," + columna);

				// variables x y para no modificar los valores originales de fila y columna
				int x = fila;
				int y = columna;

				

				for (byte pasos = 0; pasos < 3; pasos++) // Buscar 3 posiciones en la dirección actual
				{
					x += dx[direccion];
					y += dy[direccion];

					// se valida que no esta fuera del rango de la matriz y si la posicion tiene la ficha del jugador actual
					if (x >= 0 && x < tablero.GetLength(0) && y >= 0 && y < tablero.GetLength(1) && tablero[x, y] == ficha_valor) 
					{
						contador++; // incrementa si es valido
						posiones_gane_fichas.Enqueue(x + "," + y);
					}
					else { break; } // Sale del bucle si la secuencia se interrumpe en la direccion actual
				}

				x = fila;
				y = columna;


				for (byte pasos = 0; pasos < 3; pasos++)
				{
					x -= dx[direccion]; // Usa -dx[direccion] para ir en dirección opuesta
					y -= dy[direccion]; // Usa -dy[direccion] para ir en dirección opuesta

					if (x >= 0 && x < tablero.GetLength(0) && y >= 0 && y < tablero.GetLength(1) && tablero[x, y] == ficha_valor)
					{
						contador++; // Incrementa si es válido
						posiones_gane_fichas.Enqueue(x + "," + y);
					}
					else
					{
						break; // Sale del bucle si la secuencia se interrumpe
					}
				}


				if (contador >= 4) // Ahora necesitas al menos 5 porque contaste la ficha inicial dos veces
				{
					//MessageBox.Show("Secuencia encontrada", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
					Mostrar_Fichas_Ganadoras(ficha_valor);
					return true;
				}

				posiones_gane_fichas.Clear(); // Limpia las posiciones guardadas si no se encuentra una secuencia válida en esta dirección
			}

			return false; // No se encontraron secuencias válidas
		}


		private void Mostrar_Texto_Turno()
		{
			if (turnos.Peek() == 1)
			{
				TXT_TURNO.Text = "Turno: Dragon";
				IMG_TURNO.Image = jugador1;
				IMG_TURNO.HoverState.Image = jugador1;
				IMG_TURNO.PressedState.Image = jugador1;
			}
			else
			{
				TXT_TURNO.Text = "Turno: Tigre";
				IMG_TURNO.Image = jugador2;
				IMG_TURNO.HoverState.Image = jugador2;
				IMG_TURNO.PressedState.Image = jugador2;
			}

		}


		private async void Mostrar_Fichas_Ganadoras( byte jugador)
		{
			Image imgplayerwin;
		
			if(jugador == 1)
			{
				imgplayerwin = Properties.Resources.D_WIN;
			}
			else
			{
				imgplayerwin = Properties.Resources.T_WIN;
			}
			string elemento_cola;

			await Task.Delay(700);
			for (byte i = 0; i < 4; i++)
            {
				elemento_cola = posiones_gane_fichas.Peek();
				posiones_gane_fichas.Enqueue(posiones_gane_fichas.Dequeue());
				Guna2Button ficha_win = fichas.Find(x => x.Tag.ToString().Trim() == elemento_cola);
				ficha_win.ShadowDecoration.Enabled = true;
				ficha_win.BackgroundImage = imgplayerwin;
				
				

			}

        }


		private void Mostrar_Tablero_Reanuado()
		{
			string posicion; 
			 // se busca la referencia con un lamda que valida que el tag y la posicion coincidan;

			for (byte i = 0; i < tablero.GetLength(0); i++) //filas devuelve GetLength(0)
			{
				for (byte j = 0; j < tablero.GetLength(1); j++) // columnas devuelve GetLength(1)
				{
					if (tablero[i, j] == 0) 
					{
						  // si hay campos vacios lo deja asi

					}else if(tablero[i, j] == 1)
					{
						posicion = i + "," + j;
						Guna2Button ficha = fichas.Find(x => x.Tag.ToString().Trim() == posicion);
						ficha.BackgroundImage = jugador1;
					}
					else
					{
						posicion = i + "," + j;
						Guna2Button ficha = fichas.Find(x => x.Tag.ToString().Trim() == posicion);
						ficha.BackgroundImage = jugador2;
					}


				}
			}
			Desactivarbotones();
			Activarbotones();


			// HACER LA VALIDACION SI QUIERE UN JUEGO NUEVO O REANUDAR LA PARTIDA
		}


		private void AgregarTurnos() //agregar todos los turnos de la partida y reanuda la partida
		{
			if (reanudar.ValidarHayDatos() && reanudar.nueva_Partida == false)
			    {
				turnos = reanudar.DatosTurnos();
				tablero = reanudar.DatosTablero();
				movimientos = reanudar.DatosMovimientos();
			}

			if (turnos.Count == 0) //valida si la cola tiene elementos
			{
				byte jugador_inicial = 1;

				for (byte i = 0; i < tablero.Length + 1; i++)  //son 42 turnos + 1 para evitar error por falta de errores
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

				// metodo para mostrar las posiones en el tablero y validar si la columnas estan llenas para bloquear los botones
				Mostrar_Tablero_Reanuado();
			}

		}

		private void Agregar_botones_ficha()
		{
			for (byte i = 1; i <= tablero.Length; i++) //total de fichas de tablero 42
			{
				string nombreBoton = "BTN_F_" + i.ToString(); //se concatena el nombre con numnero para obtener el nombre conmpleto del boton
				Guna2Button boton = (Guna2Button)this.Controls.Find(nombreBoton, true)[0];  //se busca el boton y se guarda en una variable guna
				fichas.Add(boton); //se agrega a al lista
			}
		}
		public void Desactivarbotones()
		{
			BTN_COLUM_A.Enabled = false;
			BTN_COLUM_B.Enabled = false;
			BTN_COLUM_C.Enabled = false;
			BTN_COLUM_D.Enabled = false;
			BTN_COLUM_E.Enabled = false;
			BTN_COLUM_F.Enabled = false;
			BTN_COLUM_G.Enabled = false;
		}

		private void Activarbotones()
		{

            for (byte i = 0; i < 7; i++)
            {
				if (tablero[0,i] != 0)
				{

				}else
				{
				string nombreBoton = "BTN_COLUM_" + letrasColumnas[i]; //se concatena el nombre con numnero para obtener el nombre conmpleto del boton
				Guna2Button boton = (Guna2Button)this.Controls.Find(nombreBoton, true)[0];  //se busca el boton y se guarda en una variable guna
					boton.Enabled = true;
				}

			}
        }


		#endregion

		private void BTN_PAUSA_Click(object sender, EventArgs e)
		{
			Desactivarbotones();
			musica.EfectoClickPausaBtn();
			if(!presionado)
			{
				Pausa pausa = new Pausa(turnos,tablero,movimientos,musica);

				pausa.TopLevel = false;

				Panel_p_y_f.Controls.Add(pausa);

				pausa.Show();

				Panel_p_y_f.Location = new Point(164,92);

				Panel_p_y_f.BorderRadius = 15;
				Panel_p_y_f.Size = new Size(pausa.Size.Width, pausa.Size.Height);

				presionado = true;

				

				pausa.FormClosed += (s, args) => Pausa_FormClosed(s, args, pausa.Tag.ToString());

				//(s, args) son los parámetros de entrada de la expresión lambda. s representa
				//el objeto que está generando el evento
				//(en este caso, el formulario pausa),
				//y args representa cualquier argumento adicional asociado con el evento FormClosed.



			}

		}

		private void Pausa_FormClosed(object sender, FormClosedEventArgs e,String e_s)
		{
			presionado = false;
			Panel_p_y_f.Size = new Size(0,0);
			
			if(e_s == "SS")
			{
				menu_inicio.Show();

               this.Close();
            }
			Activarbotones();

		}
	}
}
