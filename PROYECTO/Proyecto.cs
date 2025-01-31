using System;
using System.Formats.Asn1;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using Spectre.Console;

namespace JugadorNS
{
    class Program
    {
        static int FIla = 47;
        static int Columna = 27;
        static string[,] maze = new string[FIla, Columna];
        static bool turnoJugador1 = true;
        static bool turnoJugador2 = true;
        static bool Juego = true;
        static Jugador jugador1;
        static Jugador jugador2;


        public static int posicionX { get; set; }
        public static int posicionY { get; set; }
        public static int posicionX2 { get; set; }
        public static int posicionY2 { get; set; }
        public static int enfriamiento1 = 0;
        public static int enfriamiento2 = 0;
        public static bool ActivarEscudo1 = false;
        public static bool ActivarEscudo2 = false;
        public static bool habilidadSeikanAActiva = false;
        public static int duracionHabilidadSeikanA = 2;

        public static string seleccionJugador1;
        public static string seleccionJugador2;

        static void Main(string[] args)
        {
            HEllo();
            posicionX = 1;
            posicionY = 1;
            posicionX2 = 45;
            posicionY2 = 25;

            // Menu
            var menu = new SelectionPrompt<string>()
            .Title("Selecciona una opcion:")
            .PageSize(3)
            .AddChoices(new[] { " Iniciar juego", "Salir" });

            string seleccion = AnsiConsole.Prompt(menu);

            switch (seleccion)
            {
                case " Iniciar juego":
                    TipoPersona();
                    AnsiConsole.MarkupLine("[green]Iniciando el juego...[/]");
                    break;
                case "Salir":
                    AnsiConsole.MarkupLine("[red]Saliendo del programa...[/]");
                    break;
            }

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

            Random random = new Random();
            GenerateMaze(maze, 1, 1, random);

            ColocarTrampaTeletransportacion(maze, 20);
            ColocarTrampaperderturno(maze, 20);
            ColocarTrampaEnfriamiento(maze, 20);
            Final(maze, 1);
            MoverJUGdor(maze);
        }

        static void TipoPersona()
        {
            var tipoPersona = new SelectionPrompt<string>()
            .Title("Selecciona tu personaje:")
            .PageSize(3)

            .AddChoices(new[] { "Bebesito", "Kimico", "Chocolate", "SeikanA", "Mawe" });

            seleccionJugador1 = AnsiConsole.Prompt(tipoPersona);
            seleccionJugador2 = AnsiConsole.Prompt(tipoPersona);

            jugador1 = new Jugador(seleccionJugador1, 0);
            jugador2 = new Jugador(seleccionJugador2, 0);
        }

        static void Dibujar(string[,] maze)
        {
            Console.Clear();

            var canvas = new Canvas(maze.GetLength(0), maze.GetLength(1));
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == "1") // Paredes
                    {
                        canvas.SetPixel(i, j, Color.White);
                    }
                    else if (maze[i, j] == "0")
                    {
                        canvas.SetPixel(i, j, Color.Black);
                    }
                    else if (maze[i, j] == "P")
                    {
                        canvas.SetPixel(i, j, Color.Green4);
                    }
                    else if (maze[i, j] == "J")
                    {
                        canvas.SetPixel(i, j, Color.Red3);
                    }
                    else if (maze[i, j] == "M")
                    {
                        canvas.SetPixel(i, j, Color.Yellow);
                    }
                    else if (maze[i, j] == "K" || maze[i, j] == "R" || maze[i, j] == "T") // Trampas
                    {
                        if (habilidadSeikanAActiva)
                        {
                            canvas.SetPixel(i, j, Color.Red); // Trampas en rojo
                        }
                        else
                        {
                            canvas.SetPixel(i, j, Color.Black); // Trampas normales
                        }
                    }
                }
            }
            AnsiConsole.Write(canvas);
        }


        static int objetoX;
        static int objetoY;
        static void MoverJUGdor(string[,] matriz)
        {
            bool JUEGO = true;
            int c1 = 0;
            int c2 = 0;
            int limites1 = 3;
            int limites2 = 3;
            matriz[posicionX, posicionY] = "P";
            matriz[posicionX2, posicionY2] = "J";
            Dibujar(matriz);

            while (JUEGO)
            {


                PrintMap(maze);

                if (c1 < limites1)
                {
                    matriz[posicionX, posicionY] = "0";
                    ConsoleKeyInfo tecla = Console.ReadKey(true);

                    switch (tecla.Key)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                if (turnoJugador1 == true)
                                {
                                    if (matriz[posicionX, posicionY - 1] != "1")
                                    {
                                        posicionY--;
                                        c1++;
                                    }
                                    if (matriz[posicionX, posicionY] == "K" && !ActivarEscudo1)
                                    {
                                        Console.WriteLine("jugador1 ha caido en una trampa de teletranspotacion");
                                        System.Threading.Thread.Sleep(2000);
                                        Trampateletransoportacion(matriz, posicionX, posicionY - 1, 1);
                                    }
                                    else if (matriz[posicionX, posicionY] == "R" && !ActivarEscudo1)
                                    {
                                        Console.WriteLine("¡Jugador 1 ha caído en una trampa de enfriamiento!");
                                        System.Threading.Thread.Sleep(2000);
                                        TrampaEnfriamiento(matriz, posicionX, posicionY, 1);
                                    }
                                    else if (matriz[posicionX, posicionY] == "T"&& !ActivarEscudo1 )
                                    {
                                        Console.WriteLine("Jugador 1 ha caído en una trampa de perder turno!");
                                        System.Threading.Thread.Sleep(2000);
                                        Trampaperderturno(matriz, posicionX, posicionY, 1);
                                    }
                                    else if (matriz[posicionX, posicionY] == "M")
                                    {
                                        Victoria(maze, 1);
                                        JUEGO = false;
                                    }

                                }
                            }
                            break;
                        case ConsoleKey.K:
                            if (enfriamiento1 == 0 && turnoJugador1)
                            {
                                if (seleccionJugador1 == "Bebesito")
                                {
                                    Console.WriteLine("Bebesito canta y camina 5 pasos");
                                    System.Threading.Thread.Sleep(2000);
                                    limites1 = 5;
                                    enfriamiento1 = 2;
                                }

                                else if (seleccionJugador1 == "Kimico")
                                {
                                    Console.WriteLine(" Kimico  se fumo un cigarro y se teletransporta");
                                    System.Threading.Thread.Sleep(2000);
                                    habilidadKimico(matriz, posicionX, posicionY, 1);
                                }

                                else if (seleccionJugador1 == "Chocolate")
                                {
                                    Console.WriteLine("Chocolate  no cree en nadie y es inmune a las trampas");
                                    System.Threading.Thread.Sleep(2000);
                                    habilidadChocolate(matriz, 1);
                                }
                                else if (seleccionJugador1 == "SeikanA")
                                {
                                    Console.WriteLine("SeikanA ha tomado y ve las trampas");
                                    System.Threading.Thread.Sleep(2000);
                                    habilidadSeikanA(matriz, 1);
                                }
                                else if (seleccionJugador1 == "Mawe")
                                {
                                    Console.WriteLine("Selecciona la dirección para romper la pared (↑, ↓, ←, →):");
                                    ConsoleKeyInfo teclaDireccion = Console.ReadKey(true);
                                    int direccion = -1;

                                    switch (teclaDireccion.Key)
                                    {
                                        case ConsoleKey.UpArrow:
                                            direccion = 0; // Arriba
                                            break;
                                        case ConsoleKey.DownArrow:
                                            direccion = 1; // Abajo
                                            break;
                                        case ConsoleKey.LeftArrow:
                                            direccion = 2; // Izquierda
                                            break;
                                        case ConsoleKey.RightArrow:
                                            direccion = 3; // Derecha
                                            break;
                                        default:
                                            Console.WriteLine("Dirección no válida.");
                                            break;
                                    }

                                    if (direccion != -1)
                                    {
                                        habilidadMawe(matriz, 1, direccion); 
                                    }
                                }

                            }

                            break;



                        case ConsoleKey.DownArrow:
                            {
                                if (turnoJugador1 == true)
                                    if (matriz[posicionX, posicionY + 1] != "1")
                                    {
                                        posicionY++;
                                        c1++;
                                    }
                                if (matriz[posicionX, posicionY] == "K" && !ActivarEscudo1)
                                {
                                    Console.WriteLine("jugador1 ha caido en una trampa de teletranspotacion");
                                    System.Threading.Thread.Sleep(2000);
                                    Trampateletransoportacion(matriz, posicionX, posicionY + 1, 1);
                                }
                                else if (matriz[posicionX, posicionY] == "R" && !ActivarEscudo1)
                                {
                                    Console.WriteLine("¡Jugador 1 ha caído en una trampa de enfriamiento!");
                                    System.Threading.Thread.Sleep(2000);
                                    TrampaEnfriamiento(matriz, posicionX, posicionY, 1);
                                }
                                else if (matriz[posicionX, posicionY] == "T" && !ActivarEscudo1)
                                {
                                    Console.WriteLine("Jugador 1 ha caído en una trampa de perder turno!");
                                    System.Threading.Thread.Sleep(2000);
                                    Trampaperderturno(matriz, posicionX, posicionY, 1);
                                }
                                else if (matriz[posicionX, posicionY] == "M")
                                {
                                    Victoria(maze, 1);
                                    JUEGO = false;
                                }

                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            {
                                if (turnoJugador1 == true)
                                {
                                    if (matriz[posicionX - 1, posicionY] != "1")
                                    {
                                        posicionX--;
                                        c1++;
                                    }
                                    if (matriz[posicionX, posicionY] == "K" && !ActivarEscudo1)
                                    {
                                        Console.WriteLine("jugador1 ha caido en una trampa de teletranspotacion");
                                        System.Threading.Thread.Sleep(1000);
                                        Trampateletransoportacion(matriz, posicionX - 1, posicionY, 1);
                                    }
                                    else if (matriz[posicionX, posicionY] == "R" && !ActivarEscudo1)
                                    {
                                        Console.WriteLine("¡Jugador 1 ha caído en una trampa de enfriamiento!");
                                        System.Threading.Thread.Sleep(2000);
                                        TrampaEnfriamiento(matriz, posicionX, posicionY, 1);
                                    }
                                    else if (matriz[posicionX, posicionY] == "T" && !ActivarEscudo1)
                                    {
                                        Console.WriteLine("Jugador 1 ha caído en una trampa de perder turno!");
                                        System.Threading.Thread.Sleep(2000);
                                        Trampaperderturno(matriz, posicionX, posicionY, 1);
                                    }
                                    else if (matriz[posicionX, posicionY] == "M")
                                    {
                                        Victoria(maze, 1);
                                        JUEGO = false;
                                    }
                                }
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            {
                                if (turnoJugador1 == true)
                                {
                                    if (matriz[posicionX + 1, posicionY] != "1")
                                    {
                                        posicionX++;
                                        c1++;
                                    }
                                    if (matriz[posicionX, posicionY] == "K" && !ActivarEscudo1)
                                    {
                                        Console.WriteLine("jugador1 ha caido en una trampa de teletranspotacion");
                                        System.Threading.Thread.Sleep(2000);
                                        Trampateletransoportacion(matriz, posicionX + 1, posicionY, 1);
                                    }
                                    else if (matriz[posicionX, posicionY] == "R" && !ActivarEscudo1)
                                    {
                                        Console.WriteLine("¡Jugador 1 ha caído en una trampa de enfriamiento!");
                                        System.Threading.Thread.Sleep(2000);
                                        TrampaEnfriamiento(matriz, posicionX, posicionY, 1);
                                    }
                                    else if (matriz[posicionX, posicionY] == "T" && !ActivarEscudo1)
                                    {
                                        Console.WriteLine("Jugador 1 ha caído en una trampa de perder turno!");
                                        System.Threading.Thread.Sleep(2000);
                                        Trampaperderturno(matriz, posicionX, posicionY, 1);
                                    }
                                    else if (matriz[posicionX, posicionY] == "M")
                                    {
                                        Victoria(maze, 1);
                                        JUEGO = false;
                                    }
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("TECLA NO VALIDA");
                            break;
                    }
                    matriz[posicionX, posicionY] = "P";
                    Console.Clear();
                    MostrarTabla(c1, c2, turnoJugador1, turnoJugador2);
                    System.Threading.Thread.Sleep(500);
                    if (c1 == limites1)
                    {
                        c1 = 0;
                        limites1 = 3;
                        habilidadSeikanAActiva = false;
                        ActivarEscudo1 = false;
                        turnoJugador1 = false;
                        turnoJugador2 = true;
                    }
                    if (!JUEGO) break;

                    else
                    {
                        if (c2 < limites2)
                        {
                            matriz[posicionX2, posicionY2] = "0";
                            switch (tecla.Key)
                            {
                                case ConsoleKey.W:
                                    {
                                        if (turnoJugador1 == false)
                                        {
                                            if (matriz[posicionX2, posicionY2 - 1] != "1")
                                            {
                                                posicionY2--;
                                                c2++;
                                            }
                                            if (matriz[posicionX2, posicionY2] == "K" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("jugador2 ha caido en una trampa de teletranspotacion");
                                                System.Threading.Thread.Sleep(2000);
                                                Trampateletransoportacion(matriz, posicionX2, posicionY2 - 1, 2);
                                            }
                                            else if (matriz[posicionX, posicionY] == "R" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("¡Jugador 2 ha caído en una trampa de enfriamiento!");
                                                System.Threading.Thread.Sleep(2000);
                                                TrampaEnfriamiento(matriz, posicionX, posicionY, 2);
                                            }
                                            else if (matriz[posicionX2, posicionY2] == "T" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("Jugador 2 ha caído en una trampa de perder turno!");
                                                System.Threading.Thread.Sleep(2000);
                                                Trampaperderturno(matriz, posicionX2, posicionY2, 2);
                                            }
                                            else if (matriz[posicionX2, posicionY2] == "M")
                                            {
                                                Victoria(maze, 1);
                                                JUEGO = false;
                                            }
                                        }
                                    }
                                    break;
                                case ConsoleKey.H:
                                    if (enfriamiento2 == 0)
                                    {
                                        if (seleccionJugador2 == "Bebesito")
                                        {
                                            Console.WriteLine(" Bebesito canta y camina 5 pasos ");
                                            System.Threading.Thread.Sleep(2000);
                                            limites2 = 5;
                                            enfriamiento2 = 2;
                                        }
                                        else if (seleccionJugador2 == "Kimico")
                                        {
                                            Console.WriteLine(" Kimico  se fumo un cigarro y se teletransporta");
                                            System.Threading.Thread.Sleep(2000);
                                            habilidadKimico(matriz, posicionX2, posicionY2, 2);
                                        }
                                        else if (seleccionJugador2 == "Chocolate")
                                        {
                                            Console.WriteLine(" Chocolate  no cree en nadie y es inmune a las trampas");
                                            System.Threading.Thread.Sleep(2000);
                                            habilidadChocolate(matriz, 2);
                                        }
                                        else if (seleccionJugador2 == "SeikanA")
                                        {
                                            Console.WriteLine("SeikanA ha tomado y ve las trampas ");
                                            System.Threading.Thread.Sleep(2000);
                                            habilidadSeikanA(matriz, 2);
                                        }
                                        else if (seleccionJugador2 == "Mawe")
                                        {
                                            Console.WriteLine("Selecciona la dirección para romper la pared (↑, ↓, ←, →):");
                                            ConsoleKeyInfo teclaDireccion = Console.ReadKey(true);
                                            int direccion = -1;

                                            switch (teclaDireccion.Key)
                                            {
                                                case ConsoleKey.UpArrow:
                                                    direccion = 0; // Arriba
                                                    break;
                                                case ConsoleKey.DownArrow:
                                                    direccion = 1; // Abajo
                                                    break;
                                                case ConsoleKey.LeftArrow:
                                                    direccion = 2; // Izquierda
                                                    break;
                                                case ConsoleKey.RightArrow:
                                                    direccion = 3; // Derecha
                                                    break;
                                                default:
                                                    Console.WriteLine("Dirección no válida.");
                                                    break;
                                            }

                                            if (direccion != -1)
                                            {
                                                habilidadMawe(matriz, 2, direccion); // Activar habilidad de Mawe
                                            }
                                        }
                                    }
                                    break;
                                case ConsoleKey.S:
                                    {
                                        if (turnoJugador1 == false)
                                        {
                                            if (matriz[posicionX2, posicionY2 + 1] != "1")
                                            {
                                                posicionY2++;
                                                c2++;
                                            }
                                            if (matriz[posicionX2, posicionY2] == "K" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("jugador2 ha caido en una trampa de teletranspotacion");
                                                System.Threading.Thread.Sleep(2000);
                                                Trampateletransoportacion(matriz, posicionX2, posicionY2 + 1, 2);
                                            }
                                            else if (matriz[posicionX, posicionY] == "R" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("¡Jugador 2 ha caído en una trampa de enfriamiento!");
                                                System.Threading.Thread.Sleep(2000);
                                                TrampaEnfriamiento(matriz, posicionX, posicionY, 2);
                                            }
                                            else if (matriz[posicionX2, posicionY2] == "T" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("Jugador 2 ha caído en una trampa de perder turno!");
                                                System.Threading.Thread.Sleep(2000);
                                                Trampaperderturno(matriz, posicionX2, posicionY2, 2);
                                            }
                                            else if (matriz[posicionX2, posicionY2] == "M")
                                            {
                                                Victoria(maze, 1);
                                                JUEGO = false;
                                            }
                                        }
                                    }
                                    break;
                                case ConsoleKey.A:
                                    {
                                        if (turnoJugador1 == false)
                                        {
                                            if (matriz[posicionX2 - 1, posicionY2] != "1")
                                            {
                                                posicionX2--;
                                                c2++;
                                            }
                                            if (matriz[posicionX2, posicionY2] == "K" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("jugador2 ha caido en una trampa de teletranspotacion");
                                                System.Threading.Thread.Sleep(2000);
                                                Trampateletransoportacion(matriz, posicionX2 - 1, posicionY2, 2);
                                            }
                                            else if (matriz[posicionX, posicionY] == "R" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("¡Jugador 2 ha caído en una trampa de enfriamiento!");
                                                System.Threading.Thread.Sleep(2000);
                                                TrampaEnfriamiento(matriz, posicionX, posicionY, 2);
                                            }
                                            else if (matriz[posicionX2, posicionY2] == "T" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("Jugador 2 ha caído en una trampa de perder turno!");
                                                System.Threading.Thread.Sleep(2000);
                                                Trampaperderturno(matriz, posicionX2, posicionY2, 2);
                                            }
                                            else if (matriz[posicionX2, posicionY2] == "M")
                                            {
                                                Victoria(maze, 1);
                                                JUEGO = false;
                                            }
                                        }
                                    }
                                    Dibujar(maze);
                                    break;

                                case ConsoleKey.D:
                                    {
                                        if (turnoJugador1 == false)
                                        {
                                            if (matriz[posicionX2 + 1, posicionY2] != "1")
                                            {
                                                posicionX2++;
                                                c2++;
                                            }
                                            if (matriz[posicionX2, posicionY2] == "K" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("jugador2 ha caido en una trampa de teletranspotacion");
                                                System.Threading.Thread.Sleep(2000);
                                                Trampateletransoportacion(matriz, posicionX2 + 1, posicionY2, 2);
                                            }
                                            else if (matriz[posicionX, posicionY] == "R" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("¡Jugador 2 ha caído en una trampa de enfriamiento!");
                                                System.Threading.Thread.Sleep(2000);
                                                TrampaEnfriamiento(matriz, posicionX, posicionY, 2);
                                            }
                                            else if (matriz[posicionX2, posicionY2] == "T" && !ActivarEscudo2)
                                            {
                                                Console.WriteLine("Jugador 2 ha caído en una trampa de perder turno!");
                                                System.Threading.Thread.Sleep(2000);
                                                Trampaperderturno(matriz, posicionX2, posicionY2, 2);
                                            }
                                            else if (matriz[posicionX2, posicionY2] == "M")
                                            {
                                                Victoria(maze, 1);
                                                JUEGO = false;
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Tecla no valida");
                                    break;
                            }
                            matriz[posicionX2, posicionY2] = "J";
                            if (c2 == limites2)
                            {
                                c2 = 0;
                                limites2 = 3;
                                habilidadSeikanAActiva = false;


                                ActivarEscudo2 = false;
                                turnoJugador1 = true;
                                turnoJugador2 = false;
                            }
                        }
                        if (!JUEGO) break;
                    }
                }
                if (turnoJugador1 && enfriamiento1 > 0)
                {
                    enfriamiento1--;
                }
                if (turnoJugador2 && enfriamiento2 > 0)
                {
                    enfriamiento2--;
                }
            }

        }


        public static void GenerateMaze(string[,] maze, int x, int y, Random random)
        {
            // Direcciones posibles: arriba, abajo, izquierda, derecha
            int[] dy = { 0, 0, -1, 1 };
            int[] dx = { -1, 1, 0, 0 };

            // Mezclar las direcciones para elegir una al azar
            int[] indices = { 0, 1, 2, 3 };
            Shuffle(indices, random);

            // Recorrer las direcciones
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[indices[i]] * 2;
                int ny = y + dy[indices[i]] * 2;

                // Verifica si esta en los limitess
                if (nx >= 0 && nx < maze.GetLength(0) && ny >= 0 && ny < maze.GetLength(1) && maze[nx, ny] == "1")
                {

                    maze[nx, ny] = "0";
                    maze[x + dx[indices[i]], y + dy[indices[i]]] = "0";


                    GenerateMaze(maze, nx, ny, random);
                }
            }
        }


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


        public static void PrintMap(string[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == "T")
                    {
                        Console.WriteLine(" Trampa colocada");
                        Console.Write("T ");
                    }
                    else
                    {
                        Console.Write(map[i, j] + " ");
                    }
                }
                Console.WriteLine();
                Dibujar(map);
            }
        }

        public static int TrampaColocadas = 0;

        public static void ColocarTrampaTeletransportacion(string[,] matriz, int cantidadTrampas)
        {
            Random random = new Random();
            for (int i = 0; i < cantidadTrampas; i++)
            {
                int x = random.Next(1, matriz.GetLength(0) - 1);
                int y = random.Next(1, matriz.GetLength(1) - 1);
                if (matriz[x, y] == "0")
                {
                    matriz[x, y] = "K";
                }
                else
                {
                    i--;
                }
            }
        }

        public static void Trampateletransoportacion(string[,] matriz, int oldX, int oldY, int jugador)
        {
            int x = 0;
            int y = 0;
            Random aleatorio = new Random();
            do
            {
                x = aleatorio.Next(1, matriz.GetLength(0) - 1);
                y = aleatorio.Next(1, matriz.GetLength(1) - 1);
            } while (matriz[x, y] != "0");

            if (jugador == 1)
            {
                matriz[x, y] = "P";
                matriz[oldX, oldY] = "0";

                posicionX = x;
                posicionY = y;
            }
            else if (jugador == 2)
            {
                matriz[x, y] = "J";
                matriz[oldX, oldY] = "0";
                posicionX2 = x;
                posicionY2 = y;

            }
        }

        public static void ColocarTrampaperderturno(string[,] matriz, int cantidadTrampas)
        {
            Random random = new Random();
            for (int i = 0; i < cantidadTrampas; i++)
            {
                int x = random.Next(1, matriz.GetLength(0) - 1);
                int y = random.Next(1, matriz.GetLength(1) - 1);
                if (matriz[x, y] == "0")
                {
                    matriz[x, y] = "T";
                }
                else
                {
                    i--;
                }
            }
        }

        public static bool Trampaperderturno(string[,] matriz, int x, int y, int jugador)
        {
            if (jugador == 1)
            {
                turnoJugador1 = false;
                turnoJugador2 = true;
                Console.WriteLine("Jugador1 pierde su truno");
                return true;
            }
            else if (jugador == 2)
            {
                turnoJugador1 = true;
                turnoJugador2 = false;
                Console.WriteLine("Jugador2 pierde su turno");
                return true;
            }
            return false;
        }

        static void HEllo()
        {
            Console.Clear();
            AnsiConsole.Write(
            new FigletText("Repa NAMA ")
                .LeftJustified()
                .Color(Color.Yellow3));
            AnsiConsole.Write(
            new FigletText("ABACUA NAMA")
                .LeftJustified()
                .Color(Color.Blue));
        }

        public static void Final(string[,] maze, int Meta)
        {
            Random random = new Random();
            for (int i = 0; i < Meta; i++)
            {
                int x = random.Next(21, 24);
                int y = maze.GetLength(1) / 2;
                if (maze[x, y] == "0")
                {
                    maze[x, y] = "M";
                }
                else
                {
                    i--;
                }
            }
        }

        public static void ColocarTrampaEnfriamiento(string[,] matriz, int cantidadTrampas)
        {
            Random random = new Random();
            for (int i = 0; i < cantidadTrampas; i++)
            {
                int x = random.Next(1, matriz.GetLength(0) - 1);
                int y = random.Next(1, matriz.GetLength(1) - 1);
                if (matriz[x, y] == "0")
                {
                    matriz[x, y] = "R";
                }
                else
                {
                    i--;
                }
            }
        }


        public static bool TrampaEnfriamiento(string[,] matriz, int x, int y, int jugador)
        {
            if (jugador == 1)
            {
                enfriamiento1 += 2;
                Console.WriteLine(" El jugador 1 ha sido afectado por la trampa de enfriamiento");
                return true;
            }
            else if (jugador == 2)
            {
                enfriamiento2 += 2;
                Console.WriteLine(" El jugador 2 ha sido afectado por la trampa de enfriamiento");
                return true;
            }
            return false;
        }
        public static void Victoria(string[,] maze, int jugador)
        {
            AnsiConsole.Write(
            new FigletText(" Ganaste ")
                .LeftJustified()
                .Color(Color.Yellow3));
            AnsiConsole.Write(
            new FigletText("Felicidades")
                .LeftJustified()
                .Color(Color.Blue));
            Juego = false;
        }

        public static void habilidadBebesito(string[,] matriz, int jugador, int pasos)
        {
            pasos = pasos + 5;
            if (jugador == 1)
            {
                Console.WriteLine("el bebesito a cantado se le suma +5 pasos");
                enfriamiento1 = enfriamiento1 + 2;
            }
            if (jugador == 2)
            {
                enfriamiento2 = enfriamiento2 + 2;
            }

        }
        public static void habilidadKimico(string[,] matriz, int oldX, int oldY, int jugador)
        {

            int x = 0;
            int y = 0;
            Random aleatorio = new Random();
            do
            {
                x = aleatorio.Next(1, matriz.GetLength(0) - 1);
                y = aleatorio.Next(1, matriz.GetLength(1) - 1);
            } while (matriz[x, y] != "0");


            if (jugador == 1)
            {
                Console.WriteLine("Kimico ha activado la habilidad de teletransportar a otro jugador");
                matriz[posicionX, posicionY] = "0";
                matriz[x, y] = "P";
                posicionX = x;
                posicionY = y;
                Console.WriteLine("Se a teletransportado");
                enfriamiento1 = 2;

            }
            else if (jugador == 2)
            {
                Console.WriteLine("Kimico ha activado la habilidad de teletransportar a otro jugador");
                matriz[posicionX2, posicionY2] = "0";
                matriz[x, y] = "J";
                posicionX2 = x;
                posicionY2 = y;
                Console.WriteLine("Se a teletransportado");
                enfriamiento1 = 2;
            }
        }
        public static void habilidadChocolate(string[,] matriz, int jugador)
        {
            if (jugador == 1)
            {
                Console.WriteLine("El chocolate activo sus helicopeto es inune a las trampas");
                System.Threading.Thread.Sleep(2000);
                ActivarEscudo1 = true;
                enfriamiento1 = 2;
            }
            else if (jugador == 2)
            {
                Console.WriteLine("El chocolate activo sus helicopeto es inune a las trampas");
                System.Threading.Thread.Sleep(2000);
                ActivarEscudo2 = true;
                enfriamiento2 = 2;
            }
        }
        public static void habilidadSeikanA(string[,] matriz, int jugador)
        {
            if (jugador == 1)
            {
                Console.WriteLine("Seikan A activó la habilidad: ¡Trampas visibles durante 2 turnos!");
                habilidadSeikanAActiva = true;
                duracionHabilidadSeikanA = 2; // Duración de la habilidad
                enfriamiento1 = 5;
            }
            else if (jugador == 2)
            {
                Console.WriteLine("Seikan A activó la habilidad: ¡Trampas visibles durante 2 turnos!");
                habilidadSeikanAActiva = true;
                duracionHabilidadSeikanA = 2;
                enfriamiento2 = 5;
            }
        }

        public static void habilidadMawe(string[,] matriz, int jugador, int direccion)
        {
            int x = (jugador == 1) ? posicionX : posicionX2;
            int y = (jugador == 1) ? posicionY : posicionY2;

            switch (direccion)
            {
                case 0: // Arriba
                    if (y > 1 && matriz[x, y - 1] == "1")
                    {
                        matriz[x, y - 1] = "0"; // Eliminar la pared
                        Console.WriteLine("Mawe ha roto una pared hacia arriba.");
                        System.Threading.Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("NO SE PUEDE ROMPER TE FALTA CALLE");
                        System.Threading.Thread.Sleep(2000);
                    }
                    break;
                case 1: // Abajo
                    if (y < matriz.GetLength(1) - 2 && matriz[x, y + 1] == "1")
                    {
                        matriz[x, y + 1] = "0"; // Eliminar la pared
                        Console.WriteLine("Mawe ha roto una pared hacia abajo.");
                        System.Threading.Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("NO SE PUEDE ROMPER TE FALTA CALLE");
                        System.Threading.Thread.Sleep(2000);
                    }

                    break;
                case 2: // Izquierda
                    if (x > 1 && matriz[x - 1, y] == "1")
                    {
                        matriz[x - 1, y] = "0";
                        Console.WriteLine("Mawe ha roto una pared hacia la izquierda.");
                        System.Threading.Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("NO SE PUEDE ROMPER TE FALTA CALLE");
                        System.Threading.Thread.Sleep(2000);
                    }
                    break;
                case 3:
                    if (x < matriz.GetLength(0) - 2 && matriz[x + 1, y] == "1")
                    {
                        matriz[x + 1, y] = "0";
                        Console.WriteLine("Mawe ha roto una pared hacia la derecha.");
                        System.Threading.Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("NO SE PUEDE ROMPER TE FALTA CALLE");
                        System.Threading.Thread.Sleep(2000);
                    }
                    break;
                default:
                    Console.WriteLine("Dirección no válida.");
                    break;
            }

            if (jugador == 1)
            {
                enfriamiento1 = 3;
            }
            else if (jugador == 2)
            {
                enfriamiento2 = 3;
            }
        }
        static void MostrarTabla(int pasosjugador1, int pasosjugador2, bool turnoJugador1, bool turnoJugador2)
        {
            var table = new Table();

            table.AddColumn("Jugadores");
            table.AddColumn("Pasos");
            table.AddColumn("Turno");
            table.AddColumn(" Tecla Habilidad");
            string teclaHabilidad1 = "K"; 
            string teclaHabilidad2 = "H";

            table.AddRow("Jugador 1", pasosjugador1.ToString(), turnoJugador1 ? "✅" : "❌",teclaHabilidad1);
            table.AddRow("Jugador 2", pasosjugador2.ToString(), turnoJugador2 ? "✅" : "❌",teclaHabilidad2);
            AnsiConsole.Write(table);
        }
    }
}