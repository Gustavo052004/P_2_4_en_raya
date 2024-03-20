using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P_2_4_en_raya
{
	public class Sonidos
	{
		//directorio padre dos veces para obtener el directorio del proyecto
		private string directorioProyecto = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + @"\Resources\Sonidos\";

		private bool musicaFondoGeneral = true;

		private bool continuarReproduccionFondo = true;
		private bool continuarReproduccionEfectos = true;

		private CancellationTokenSource cancellationTokenSource;


		public async Task MusicaFondoInicio(CancellationToken cancellationToken)
		{
			if (musicaFondoGeneral)
			{
				while (continuarReproduccionFondo)  // bucle para repeticion continua el metodo es asyn para que no interrumpa flujo de la app
				{
					//se utiliza para garantizar que los recursos sean liberados correctamente después de su uso
					//crea una instancia del recurso que debe ser liberado después de su uso.
					// el codigo dentro usa el recurso

					using (var stream = File.OpenRead(directorioProyecto+"P_inicio.mp3")) // abre el archivo de audio MP3
					{
						using (var waveOut = new WaveOutEvent())  // instancia del reproductor
						{
							waveOut.Init(new Mp3FileReader(stream)); //inicia el reproductor en mp3 
							waveOut.Play(); //comienza a reproducir
							while (waveOut.PlaybackState == PlaybackState.Playing && continuarReproduccionFondo) // valida que se sigue reproduciendo el audio y continuarReproduccion es true
							{
								await Task.Delay(10); // 10 milisegundos para no afectar audio ya que en 0 no se ejecuta la app
								cancellationToken.ThrowIfCancellationRequested(); // Lanza una excepción si se solicita la cancelación
							}
						}
					}
				}

			}
			
		}

		public async Task MusicaFondoJuego(CancellationToken cancellationToken)
		{
			if (musicaFondoGeneral)
			{
				while (continuarReproduccionFondo)  // bucle para repeticion continua el metodo es asyn para que no interrumpa flujo de la app
				{
					//se utiliza para garantizar que los recursos sean liberados correctamente después de su uso
					//crea una instancia del recurso que debe ser liberado después de su uso.
					// el codigo dentro usa el recurso

					using (var stream = File.OpenRead(directorioProyecto + "P_juego.mp3")) // abre el archivo de audio MP3
					{
						using (var waveOut = new WaveOutEvent())  // instancia del reproductor
						{
							waveOut.Init(new Mp3FileReader(stream)); //inicia el reproductor en mp3 
							waveOut.Play(); //comienza a reproducir
							while (waveOut.PlaybackState == PlaybackState.Playing && continuarReproduccionFondo) // valida que se sigue reproduciendo el audio y continuarReproduccion es true
							{
								await Task.Delay(10); // 10 milisegundos para no afectar audio ya que en 0 no se ejecuta la app
								cancellationToken.ThrowIfCancellationRequested(); // Lanza una excepción si se solicita la cancelación
							}
						}
					}
				}
			}
			
		}


		public async Task MusicaFondoFinal(CancellationToken cancellationToken)
		{
			if (musicaFondoGeneral)
			{
				while (continuarReproduccionFondo)  // bucle para repeticion continua el metodo es asyn para que no interrumpa flujo de la app
				{
					//se utiliza para garantizar que los recursos sean liberados correctamente después de su uso
					//crea una instancia del recurso que debe ser liberado después de su uso.
					// el codigo dentro usa el recurso

					using (var stream = File.OpenRead(directorioProyecto + "AudioFInal.mp3")) // abre el archivo de audio MP3
					{
						using (var waveOut = new WaveOutEvent())  // instancia del reproductor
						{
							waveOut.Init(new Mp3FileReader(stream)); //inicia el reproductor en mp3 
							waveOut.Play(); //comienza a reproducir
							while (waveOut.PlaybackState == PlaybackState.Playing && continuarReproduccionFondo) // valida que se sigue reproduciendo el audio y continuarReproduccion es true
							{
								await Task.Delay(10); // 10 milisegundos para no afectar audio ya que en 0 no se ejecuta la app
								cancellationToken.ThrowIfCancellationRequested(); // Lanza una excepción si se solicita la cancelación
							}
						}
					}
				}
			}

		}

		public async Task EfectoClick()
		{


			using (var stream = File.OpenRead(directorioProyecto + "button-pressed-38129.mp3")) // abre el archivo de audio MP3
			{
				using (var waveOut = new WaveOutEvent())  // instancia del reproductor
				{
					waveOut.Init(new Mp3FileReader(stream));

					if (continuarReproduccionEfectos)
					{
					
					waveOut.Play();
					}

					await Task.Delay(450);
				

					
				}
			}
		}

		public async Task EfectoClickPausaBtn()
		{


			using (var stream = File.OpenRead(directorioProyecto + "BTNPAUSA.mp3")) // abre el archivo de audio MP3
			{
				using (var waveOut = new WaveOutEvent())  // instancia del reproductor
				{
					waveOut.Init(new Mp3FileReader(stream));

					if (continuarReproduccionEfectos)
					{

						waveOut.Play();
					}

					await Task.Delay(450);



				}
			}
		}

		public async Task EfectoVictoria()
		{


			using (var stream = File.OpenRead(directorioProyecto + "victoria.mp3")) // abre el archivo de audio MP3
			{
				using (var waveOut = new WaveOutEvent())  // instancia del reproductor
				{
					waveOut.Init(new Mp3FileReader(stream));

					if (continuarReproduccionEfectos)
					{

						waveOut.Play();
					}

					await Task.Delay(3000);



				}
			}
		}

		public void DetenerEfectos()
		{
			continuarReproduccionEfectos = false; //cambia a false para que no se reproduzca el efecto
		}

		public void ContinuarEfectos()
		{
			continuarReproduccionEfectos = true; //cambia a true para que se reproduzca el efecto
		}


		public void ActivarMusicaFondo()
		{
			musicaFondoGeneral = true; 
		}

		public void DesactivarMusicaFondo()
		{
			cancellationTokenSource?.Cancel();
			musicaFondoGeneral = false; 
		}



		public void DetenerMusicaFondos()
		{
			cancellationTokenSource?.Cancel(); // Cancela la reproducción actual si existe
			continuarReproduccionFondo = false; //cambia a false para que se dentenga el audio
		}



		public void ContinuarMusicaFondos(byte opcion_musica)
		{
			cancellationTokenSource = new CancellationTokenSource(); // Crea un nuevo CancellationTokenSource para la nueva reproducción
			continuarReproduccionFondo = true;

			switch (opcion_musica)
			{
				case 1:
					Task.Run(() => MusicaFondoInicio(cancellationTokenSource.Token)); // Inicia la reproducción de música en un hilo de fondo
					break;

				case 2:
					Task.Run(() => MusicaFondoJuego(cancellationTokenSource.Token));
					break;

				case 3:
					Task.Run(() => MusicaFondoFinal(cancellationTokenSource.Token));
					break;
			}
		}
	}
}
