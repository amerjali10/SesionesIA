// =============================================================
//  AHORCADO EN CONSOLA
//  Curso: Prompts de Copilot para Git/GitHub en .NET - Módulo 1
// =============================================================
//
//  Este archivo empieza casi vacío a propósito.
//  El equipo lo construye escribiendo comentarios que describen
//  lo que necesita, y GitHub Copilot propone el código.
//
//  Reglas del equipo:
//  1. Lean cada sugerencia completa antes de aceptarla.
//  2. Después de cada cambio, ejecuten el juego con: dotnet run
//  3. Al cambiar de piloto: commit y push (sale) / pull (entra).

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ===== DATOS =====
string[] palabras =
{
    "algoritmo",
    "aplicación",
    "compilador",
    "depuración",
    "desarrollo",
    "función",
    "generador",
    "instrucción",
    "interfaz",
    "intérprete",
    "lenguaje",
    "lógica",
    "módulo",
    "navegador",
    "objeto",
    "programación",
    "repositorio",
    "servidor",
    "sintaxis",
    "añadir"
};

Random aleatorio = new Random();
int puntajeTotal = 0;

// ===== PROGRAMA PRINCIPAL =====
while (true)
{
    int? puntosPartida = JugarPartida(palabras, aleatorio);
    if (puntosPartida is null)
    {
        break;
    }

    puntajeTotal += puntosPartida.Value;
    Console.WriteLine();
    Console.WriteLine($"Puntaje total: {puntajeTotal}");

    bool? jugarOtraVez = PreguntarSiJugarOtraVez();
    if (jugarOtraVez is null || !jugarOtraVez.Value)
    {
        Console.WriteLine("¡Hasta luego!");
        break;
    }
}

// ===== FUNCIONES =====
// Qué: Quita tildes y diéresis para comparar letras sin distinguir acentos.
// Recibe: Una cadena de texto.
// Devuelve: La misma cadena sin tildes, conservando la ñ.
// Reglas: Ignora acentos en letras vocales; la ñ no se convierte en n.
static string QuitarTilde(string texto)
{
    if (string.IsNullOrEmpty(texto))
    {
        return string.Empty;
    }

    System.Text.StringBuilder resultado = new System.Text.StringBuilder();

    foreach (char caracter in texto)
    {
        char letra = char.ToLowerInvariant(caracter);

        switch (letra)
        {
            case 'á':
            case 'à':
            case 'ä':
                resultado.Append('a');
                break;
            case 'é':
            case 'è':
            case 'ë':
                resultado.Append('e');
                break;
            case 'í':
            case 'ì':
            case 'ï':
                resultado.Append('i');
                break;
            case 'ó':
            case 'ò':
            case 'ö':
                resultado.Append('o');
                break;
            case 'ú':
            case 'ù':
            case 'ü':
                resultado.Append('u');
                break;
            case 'ñ':
                resultado.Append('ñ');
                break;
            default:
                resultado.Append(letra);
                break;
        }
    }

    return resultado.ToString();
}

// Qué: Genera la palabra oculta mostrando letras ya acertadas y guiones para las restantes.
// Recibe: La palabra original y el conjunto de letras ya adivinadas.
// Devuelve: La palabra con guiones y letras visibles, separadas por espacios.
// Reglas: Las letras acertadas se muestran con su tilde original; las restantes se reemplazan por _. 
static string PalabraOculta(string palabra, HashSet<string> letrasAdivinadas)
{
    List<string> partes = new List<string>();

    foreach (char letra in palabra)
    {
        string letraNormalizada = QuitarTilde(letra.ToString());

        if (letrasAdivinadas.Contains(letraNormalizada))
        {
            partes.Add(letra.ToString());
        }
        else
        {
            partes.Add("_");
        }
    }

    return string.Join(" ", partes);
}

// Qué: Pide una letra válida al usuario y la valida contra las ya usadas.
// Recibe: El conjunto de letras ya utilizadas.
// Devuelve: La letra ingresada si es válida, o null si la entrada del usuario fue nula.
// Reglas: Solo acepta una letra, ignora mayúsculas/minúsculas, rechaza repetidas y vuelve a pedir sin contabilizar error.
static char? PedirLetra(HashSet<string> letrasUsadas)
{
    while (true)
    {
        Console.Write("Ingrese una letra: ");
        string? entrada = Console.ReadLine();

        if (entrada is null)
        {
            return null;
        }

        string texto = entrada.Trim();

        if (texto.Length != 1 || !char.IsLetter(texto[0]))
        {
            Console.WriteLine("Debe ingresar una sola letra válida.");
            continue;
        }

        string letraNormalizada = QuitarTilde(texto);

        if (letrasUsadas.Contains(letraNormalizada))
        {
            Console.WriteLine("Esa letra ya fue usada.");
            continue;
        }

        letrasUsadas.Add(letraNormalizada);
        return texto[0];
    }
}

// Qué: Muestra el ahorcado actualizado según la cantidad de errores cometidos.
// Recibe: El número de errores actuales.
// Devuelve: Nada, solo imprime el dibujo.
// Reglas: Cada error agrega una parte del cuerpo; la figura se dibuja en ASCII y se actualiza cada turno.
static void MostrarDibujo(int errores)
{
    Console.WriteLine("  +---+");
    Console.WriteLine("  |   |");
    Console.WriteLine(errores >= 1 ? "  |   O" : "  |");
    Console.WriteLine(errores >= 2 ? "  |   |" : "  |");
    Console.WriteLine(errores >= 3 ? "  |  /|" : "  |");
    Console.WriteLine(errores >= 4 ? "  |  /|\\" : "  |");
    Console.WriteLine(errores >= 5 ? "  |  / " : "  |");
    Console.WriteLine(errores >= 6 ? "  | / \\" : "  |");
    Console.WriteLine("  +---+");
}

// Qué: Ejecuta una partida completa del juego.
// Recibe: La lista de palabras disponibles y el generador aleatorio.
// Devuelve: Los puntos ganados en la partida, o null si la entrada del usuario termina el programa.
// Reglas: Se elige una palabra al azar; se gana con todas las letras descubiertas; se pierde con 6 errores; se devuelve el puntaje final de la partida.
static int? JugarPartida(string[] palabras, Random aleatorio)
{
    string palabra = palabras[aleatorio.Next(palabras.Length)];
    HashSet<string> letrasUsadas = new HashSet<string>(StringComparer.Ordinal);
    HashSet<string> letrasAdivinadas = new HashSet<string>(StringComparer.Ordinal);
    int errores = 0;

    Console.WriteLine();
    Console.WriteLine("=== NUEVA PARTIDA ===");
    Console.WriteLine("Adivina la palabra:");

    while (true)
    {
        bool palabraCompletada = palabra.All(caracter => letrasAdivinadas.Contains(QuitarTilde(caracter.ToString())));
        if (palabraCompletada)
        {
            int puntos = (6 - errores) * 10 + palabra.Length;
            Console.WriteLine();
            Console.WriteLine($"¡Ganaste! La palabra era: {palabra}");
            Console.WriteLine($"Puntos obtenidos: {puntos}");
            return puntos;
        }

        if (errores >= 6)
        {
            Console.WriteLine();
            Console.WriteLine($"¡Perdiste! La palabra era: {palabra}");
            Console.WriteLine("Puntos obtenidos: 0");
            return 0;
        }

        Console.WriteLine();
        MostrarDibujo(errores);
        Console.WriteLine($"Palabra: {PalabraOculta(palabra, letrasAdivinadas)}");

        string letrasMostradas = letrasUsadas.Count == 0 ? "ninguna" : string.Join(" ", letrasUsadas.Select(letra => char.ToUpperInvariant(letra[0])));
        Console.WriteLine($"Letras usadas: {letrasMostradas}");

        char? letra = PedirLetra(letrasUsadas);
        if (letra is null)
        {
            return null;
        }

        string letraNormalizada = QuitarTilde(letra.Value.ToString());
        bool acerto = false;

        for (int i = 0; i < palabra.Length; i++)
        {
            string letraPalabra = QuitarTilde(palabra[i].ToString());
            if (letraPalabra == letraNormalizada)
            {
                letrasAdivinadas.Add(letraPalabra);
                acerto = true;
            }
        }

        if (!acerto)
        {
            errores++;
            Console.WriteLine($"No está. Has cometido {errores} error(es).");
        }
        else
        {
            Console.WriteLine($"¡Correcto! La letra '{letra.Value}' está en la palabra.");
        }

        palabraCompletada = palabra.All(caracter => letrasAdivinadas.Contains(QuitarTilde(caracter.ToString())));

        if (palabraCompletada)
        {
            int puntos = (6 - errores) * 10 + palabra.Length;
            Console.WriteLine();
            Console.WriteLine($"¡Ganaste! La palabra era: {palabra}");
            Console.WriteLine($"Puntos obtenidos: {puntos}");
            return puntos;
        }

        if (errores >= 6)
        {
            Console.WriteLine();
            Console.WriteLine($"¡Perdiste! La palabra era: {palabra}");
            Console.WriteLine("Puntos obtenidos: 0");
            return 0;
        }
    }
}

// Qué: Consulta al usuario si quiere iniciar otra partida.
// Recibe: Ninguno.
// Devuelve: true si responde 's', false si responde 'n', o null si la entrada es nula.
// Reglas: Solo acepta respuestas s o n, sin importar mayúsculas/minúsculas; cualquier otra respuesta obliga a repetir la pregunta.
static bool? PreguntarSiJugarOtraVez()
{
    while (true)
    {
        Console.Write("¿Jugar otra vez? (s/n): ");
        string? respuesta = Console.ReadLine();

        if (respuesta is null)
        {
            return null;
        }

        string texto = respuesta.Trim();

        if (texto.Equals("s", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (texto.Equals("n", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        Console.WriteLine("Respuesta inválida. Debe responder solo 's' o 'n'.");
    }
}
