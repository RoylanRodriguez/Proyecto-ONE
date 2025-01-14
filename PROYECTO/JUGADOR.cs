using System.Data;
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;

public class Jugador
{
   public string Nombre { get; set; }
   public int Puntos {get;set;}
   public int Velocidad{ get;set;} = 4;
   


   public Jugador(string nombre, int puntos)
   {
       Nombre = nombre;
       Puntos = puntos;
        
   }
    
    public override string ToString()
    {
      return $"Jugador: {Nombre} Puntos {Puntos}";
    }
    public  void Reducirvelocidad(int reducion)
       {
         Velocidad -=reducion;
         if(Velocidad < 4) Velocidad = 4;
         {
            Console.WriteLine("disminuyo su velocidad");
         }
       }   
   
   public void ReduccirPuntos()
   {

   }
      
      
}
    
     public class Laberinto
     {
        public int JugadorX{ get; set; }
        public int JugadorY{ get; set; }
        private char[,] laberinto;
        public Laberinto(int jugadorX,int jugadorY)
        {
          JugadorX = jugadorX;
          JugadorY = jugadorY;
          laberinto = new char[21,21];
          for (int i = 0; i < 21; i++)
          {
            for (int j = 0; j < 21; j++)

            {
               laberinto[i, j] = ' ';
            }
          }
        }
        
       
     }

        
     
      
      
        


      

     
     





