using System.Data;
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;
using Spectre.Console;

namespace JugadorNS
{
    public class Jugador
    {
        public string Nombre { get; set; }
        public int Puntos { get; set; }

        public Jugador(string nombre, int puntos)
        {
            Nombre = nombre;
            Puntos = puntos;
        }

        public override string ToString()
        {
            return $"Jugador: {Nombre} Puntos {Puntos}";
        }

            }

    public class Laberinto
    {
        public int JugadorX { get; set; }
        public int JugadorY { get; set; }
        public string[,] laberinto;

   
   
   
        public Laberinto(int jugadorX, int jugadorY)
        {
            JugadorX = jugadorX;
            JugadorY = jugadorY;
            laberinto = new string[21, 21];
            for (int i = 0; i < 21; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    laberinto[i, j] = " ";
                }
            }
        }

        
      
    }
}
