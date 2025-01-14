// See https://aka.ms/new-console-template for more information
using System;
using System.Formats.Asn1;

class Program
{
    static int FIla = 21;
      static   int Columna = 21;
      static   string[,] maze = new string[FIla, Columna];
   
    static void Main(string[] args)
    {
        // Matriz
        
         Jugador jugador1 = new Jugador("Juan",0);
         Jugador jugador2 = new Jugador("Pedro",0);
           Console.WriteLine(jugador1);
           Console.WriteLine(jugador2);
           
           
          
        // Crear una matriz llena de paredes (1)
        for (int i = 0; i < FIla; i++)
        {
            for (int j = 0; j < Columna; j++)
            {
                maze[i, j] = "1";
            }
        }

        // Generar el laberinto desde la celda inicial (1, 1)
        Random random = new Random();
        GenerateMaze(maze, 1, 1, random);

        MoverJUGdor(maze);

        // Imprimir el laberinto
      
    }

 public static void MoverJUGdor(string[,]matriz)
       {
         int posicionX = 1;
         int posicionY = 1;
         int k=0;
         Console.Clear();
         matriz[posicionX,posicionY]="P";
        while(k==0) 
        {
          
         PrintMap(maze);
         matriz[posicionX,posicionY]="0";
         ConsoleKeyInfo tecla = Console.ReadKey(true);
         switch(tecla.Key)
         {
           case ConsoleKey.UpArrow:
           {
            if(matriz[posicionX-1,posicionY]!="1")
            {
              posicionX=posicionX-1;
            }
           }
           break;
           case ConsoleKey.DownArrow:
            {
            if(matriz[posicionX+1,posicionY]!="1")
            {
              posicionX=posicionX+1;
            }
           }
           break;
           case ConsoleKey.LeftArrow:
            {
            if(matriz[posicionX,posicionY-1]!="1")
            {
              posicionY=posicionY-1;
            }
           }
           break;
           case ConsoleKey.RightArrow:
            {
            if(matriz[posicionX,posicionY+1]!="1")
            {
              posicionY=posicionY+1;
            }
           }
           break;

         }
          if(matriz[posicionX,posicionY]=="T")
          {
            Console.WriteLine("¡caiste en una trampa se disminuye tu velocidad!");
            
            k = 1;
          }
          if(matriz[posicionX,posicionY]=="k")
          {
            Console.WriteLine("¡TOCA  la tecla espacio varias veces");
            k = 1;
          }
         
         matriz[posicionX,posicionY]="P";
        
       }
       }

public static void MoverJUGdor2(string[,]matriz)
       {
         int posicionX = 20;
         int posicionY = 20;
         int k=0;
         matriz[posicionX,posicionY]="j";
        while(k==0) 
        {
          
            PrintMap(maze);
         matriz[posicionX,posicionY]="0";
         ConsoleKeyInfo tecla = Console.ReadKey(true);
         switch(tecla.Key)
         {
           case ConsoleKey.UpArrow:
           {
            if(matriz[posicionX-1,posicionY]!="1")
            {
              posicionX=posicionX-1;
            }
           }
           break;
           case ConsoleKey.DownArrow:
            {
            if(matriz[posicionX+1,posicionY]!="1")
            {
              posicionX=posicionX+1;
            }
           }
           break;
           case ConsoleKey.LeftArrow:
            {
            if(matriz[posicionX,posicionY-1]!="1")
            {
              posicionY=posicionY-1;
            }
           }
           break;
           case ConsoleKey.RightArrow:
            {
            if(matriz[posicionX,posicionY+1]!="1")
            {
              posicionY=posicionY+1;
            }
           }
           break;

         }
         matriz[posicionX,posicionY]="j";
        
       }
       }

    // Función recursiva para generar el laberinto
    public static void GenerateMaze(string[,] maze, int x, int y, Random random)
    {
        // Direcciones posibles: arriba, abajo, izquierda, derecha
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };

        // Mezclar las direcciones para elegir una al azar
        int[] indices = { 0, 1, 2, 3 };
        Shuffle(indices, random);

        // Recorrer las direcciones
        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[indices[i]] * 2;
            int ny = y + dy[indices[i]] * 2;

            // Verificar si la celda está dentro de los límites y no ha sido visitada
            if (nx >= 0 && nx < maze.GetLength(0) && ny >= 0 && ny < maze.GetLength(1) && maze[nx, ny] == "1")
            {
                // Eliminar la pared entre la celda actual y la celda vecina
                maze[nx, ny] = "0";
                maze[x + dx[indices[i]], y + dy[indices[i]]] = "0";

                // Llamada recursiva para continuar desde la nueva celda
                GenerateMaze(maze, nx, ny, random);
            }
        }
    }

    // Función para mezclar un array (shuffle)
    public static void Shuffle(int[] array, Random random)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    // Función para imprimir el laberinto
    public static void PrintMap(string[,] map)
    {
       // map[1,1] = "2";
        maze[5,5] = "T";
        maze[9,4]= "k";
        Console.Clear();

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == "0")
                {
                    Console.Write("  "); // Espacio vacío
                }
              /*  else if ( map[i,j]== "2")
                {
                 Console.Write("p "); 
                }
                */
                else if (map[i, j] == "1")
                {
                    Console.Write("# "); // Pared
                }
                else if(map[i,j]=="T")
                {
                   Console.Write("T ");
                }
                else if(map[i,j]=="k")
                {
                   Console.Write("k ");
                }
                else 
                {
                    Console.Write("P "); // Pared
                }
              
            }
            Console.WriteLine();
        }
    }
}