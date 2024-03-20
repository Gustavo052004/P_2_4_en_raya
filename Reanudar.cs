using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel;

namespace P_2_4_en_raya
{
	public class Reanudar
	{
		public bool nueva_Partida;


		private byte[,] tablero = new byte[6, 7];
		private Stack<string> movimientos = new Stack<string>();
		private Queue<byte> turnos = new Queue<byte>();


		public void GuardarDatosPartida(byte[,] tablerodato, Stack<string> movimientosdato, Queue<byte> turnosdato)
		{
			Borrar_Archivos();

			tablero = tablerodato;
			movimientos = movimientosdato;
			turnos = turnosdato;

			GuardarEnJSON(tablero, "tablero.json");
			GuardarEnJSON(movimientos, "movimientos.json");
			GuardarEnJSON(turnos, "turnos.json");

		}


		 public bool ValidarHayDatos()
		{
			if (File.Exists("tablero.json") && File.Exists("movimientos.json") && File.Exists("turnos.json"))
			{
				tablero = RecuperarDesdeJSON<byte[,]>("tablero.json");
				movimientos = RecuperarDesdeJSON<Stack<string>>("movimientos.json");
				turnos = RecuperarDesdeJSON<Queue<byte>>("turnos.json");

				if (tablero != null && movimientos != null && turnos != null)
				{
					return true;
				}
				else
				{
					return false;
				}

			}
			return false;
			
		}

		public void Borrar_Archivos()
		{
			BorrarArchivo("tablero.json");
			BorrarArchivo("movimientos.json");
			BorrarArchivo("turnos.json");
		}

		public byte[,] DatosTablero()
		{
			tablero = RecuperarDesdeJSON<byte[,]>("tablero.json");
			return tablero;
		}

		public Stack<string> DatosMovimientos()
		{
			movimientos = RecuperarDesdeJSON<Stack<string>>("movimientos.json");
			return movimientos;
		}

		public Queue<byte> DatosTurnos()
		{
			turnos = RecuperarDesdeJSON<Queue<byte>>("turnos.json");
			return turnos;
		}







		static void GuardarEnJSON(object objeto, string archivo)
		{
			string json = JsonConvert.SerializeObject(objeto, Newtonsoft.Json.Formatting.Indented);
			File.WriteAllText(archivo, json);
			Console.WriteLine($"Datos guardados en '{archivo}'");
		}

		static T RecuperarDesdeJSON<T>(string archivo)
		{
			if (File.Exists(archivo))
			{
				string json = File.ReadAllText(archivo);
				return JsonConvert.DeserializeObject<T>(json);
			}
			else
			{
				Console.WriteLine($"El archivo '{archivo}' no existe.");
				return default(T);
			}
		}

		static void BorrarArchivo(string archivo)
		{
			if (File.Exists(archivo))
			{
				File.Delete(archivo);
				Console.WriteLine($"El archivo '{archivo}' ha sido eliminado.");
			}
			else
			{
				Console.WriteLine($"El archivo '{archivo}' no existe.");
			}
		}

	}
}
