 e  Informe del Proyecto: Juego Basado en Gustos Musicales 🎶🎮
Descripción General 🌟
Este proyecto es un juego inspirado en mis gustos musicales, donde los jugadores compiten para llegar a la meta mientras enfrentan diversos obstáculos y trampas. El juego comienza con un menú principal que permite iniciar el juego o salir. Luego, los jugadores seleccionan sus personajes, cada uno con habilidades únicas, y comienza la partida. El objetivo es llegar a la meta antes que el oponente, sorteando trampas ocultas en el camino. 🏁

Características del Juego 🎲
Menú de Selección de Personajes 🎭
Los jugadores pueden elegir entre cinco personajes, cada uno con habilidades especiales:
Las habilidades de los jugadores1 se Activan con la tecla K y los del jugador 2 con la tecla H.
Bebesito 👶:

Habilidad: Al cantar, puede caminar 5 pasos en su turno. 🎤🚶‍♂️

Kimico 🚬:

Habilidad: Puede teletransportarse a una ubicación aleatoria del laberinto. ✨🔀

Chocolate 🍫:

Habilidad: Es inmune a las trampas durante su turno. 🛡️🚫

Seikan.A 🍻:

Habilidad: Al estar "borracho", puede ver las trampas en el laberinto durante 2 turnos. 🕵️‍♂️👀

Mawe 💥:

Habilidad: Puede romper paredes para abrir nuevos caminos. 🔨🧱

Mecánicas del Juego 🕹️
Laberinto: El juego se desarrolla en un laberinto generado automáticamente de tamaño 47x27, creado mediante el algoritmo Recursive Backtracking. Este algoritmo garantiza que haya un único camino hacia la meta. 🧩

Trampas ⚠️:

Teletransportación: Teletransporta al jugador a una ubicación aleatoria del laberinto. 🔄

Perder Turno: Hace que el jugador pierda su turno si cae en ella. ⏳

Electrochok: Electrocuta al jugador, impidiéndole moverse durante un turno. ⚡

Meta 🏁: Ambos jugadores comienzan en posiciones equidistantes de la meta. El primero en llegar gana el juego. 🏆

Desarrollo Técnico 💻
Generación del Laberinto 🧱
El laberinto se genera utilizando el algoritmo Recursive Backtracking, que crea un laberinto perfecto (sin ciclos y con un único camino entre dos puntos). Para asegurar la aleatoriedad, se utiliza un método Shuffle que mezcla las direcciones posibles (arriba, abajo, izquierda, derecha) antes de cada movimiento.

Movimiento de los Jugadores 🕹️
El movimiento de los jugadores se controla mediante un sistema de turnos. Cada jugador tiene un número limitado de movimientos por turno, y las trampas se activan cuando un jugador cae en ellas. Las habilidades de los personajes se activan mediante teclas específicas durante el turno del jugador.

Implementación de Trampas ⚠️
Se implementaron tres tipos de trampas:

Trampa de Teletransportación: Coloca al jugador en una ubicación aleatoria del laberinto. 🔄

Trampa de Perder Turno: Hace que el jugador pierda su turno. ⏳

Trampa de Electrochok: Electrocuta al jugador, impidiéndole moverse durante un turno. ⚡

Cada trampa se coloca aleatoriamente en el laberinto, asegurándose de que no esté en una pared.

Habilidades de los Personajes 🦸‍♂️
Cada personaje tiene una habilidad única que se activa durante su turno. Estas habilidades están implementadas en métodos específicos y se llaman dentro del sistema de turnos.

Interfaz Visual 🎨
Para la parte visual del proyecto, se utilizó la librería Spectre.Console, que permite dar color a los elementos del juego:

Paredes: Color blanco. ⬜

Camino: Color negro. ⬛

Trampas: Color rojo (cuando son visibles). 🔴

Jugadores: Color verde y rojo para diferenciarlos. 🟢🔴