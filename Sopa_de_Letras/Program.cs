using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* ------------------------------------------------------------------------------------------------------------------------------------------*
 
Reto: 16 ( Sopa de letras ).
 
 *Ejemplo de entrada propuesto originalmente en la especificación del Reto.
ENTRADA
2 3
aal
ala
2
al
ala
 
SALIDA
6
2
 
 ________________________________________________________________________________________
 
 Ejemplo para un fichero de entrada.
 Pegar todas las líneas en un archivo de texto plano con nombre "MatrizLetras.txt" para poder ejecutar la solución.
 
15 15
foktaicergniica
grtcuyagtreuniv
rtaathurdtcmcbf
einntcflouheior
cliacivomprsaba
iytdyiitageuron
ffpasiatdbfwcnc
rawtkilvhsbcaai
axwsfdaudqdxnhn
nzxibdaiutvaajd
cekmaeyiefdgdei
irondssrlakbaaa
tcayrurnoruegai
sgrecianoruegaa
agailatitaliatr
7
italia
grecia
canada
india
peru
francia
noruega
 */

namespace Sopa_de_Letras
{
    class Program
    {
        static string[] sopa_filas;
        static ListaBuscar[] palabras_objetivo;

        static void Main(string[] args)
        {
            bool tmpbool;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Clear();

            // Proceso para cargar datos. El enunciado pide que sea via teclado, pero de esta manera es más rápido testar el programa.
            if (File.Exists("C:\\PROYECTO\\PROYECTO_UNIDAD_III\\SopaDeLetras.txt"))
            {
                tmpbool = Cargar_Datos_Fichero("C:\\PROYECTO\\PROYECTO_UNIDAD_III\\SopaDeLetras.txt");
            }
            else
            {
                tmpbool = Cargar_Datos_Teclado();
            }


            if (tmpbool)
            {
                // Proceso de búsqueda.
                foreach (ListaBuscar objetivos in palabras_objetivo)
                {
                    // Buscar para cada uno de los 8 posibles sentidos.
                    for (int fila_actual = 0; fila_actual < sopa_filas.Length; fila_actual++)
                    {
                        for (int col_actual = 0; col_actual < sopa_filas[fila_actual].Length; col_actual++)
                        {
                            for (int incx = -1; incx < 2; incx++)
                            {
                                for (int incy = -1; incy < 2; incy++)
                                {
                                    if (!(incx == 0 && incy == 0))
                                    {
                                        if (buscar_palabra(objetivos.palabra(), fila_actual, col_actual, incx, incy))
                                        {
                                            objetivos.encontrada();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Proceso de muestra de resultados.
                foreach (ListaBuscar objetivos in palabras_objetivo)
                {
                    Console.WriteLine("{0}  --> {1} Veces.", objetivos.palabra(), objetivos.veces());
                }
            }
            else
            {
                Console.WriteLine("El proceso de carga de datos ha fallado.");
            }

            Console.ReadKey(); // Pausa para que la pantalla no se cierre al terminar el proceso y podamos ver los resultados.
        }

        static bool Cargar_Datos_Teclado()
        {
            // Primera línea de datos: Nº de filas y columnas de nuestra Sopa de letras.
            string linealeida;
            int filas, columnas;

            Console.WriteLine("Introduce el número de FILAS y COLUMNAS de la Sopa de letras separados por un Espacio.");
            linealeida = Console.ReadLine();

            filas = Convert.ToInt32(linealeida.Substring(0, linealeida.IndexOf(" ")));
            columnas = Convert.ToInt32(linealeida.Substring(linealeida.IndexOf(" ")));
            sopa_filas = new string[filas];

            // Cargar los caracteres de la matriz.
            for (int indice = 0; indice < filas; indice++)
            {
                Console.WriteLine("Introduce la fila número {0} de la Sopa de Letras.", indice + 1);
                linealeida = Console.ReadLine();
                if (linealeida.Length < columnas) // Por si nos falta algún caracter en alguna de las líneas.
                {
                    Console.WriteLine("La línea " + indice + " no tiene el número de caracteres necesarios.");
                }
                sopa_filas[indice] = linealeida.Substring(0, columnas); // Para que no tome más columnas de las debidas.
            }
            // Capturar el número de palabras a buscar y las palabras.
            Console.Clear();
            Console.WriteLine("Introduce el número de palabras a buscar.");
            linealeida = Console.ReadLine();
            palabras_objetivo = new ListaBuscar[Convert.ToInt32(linealeida)];
            for (int indice = 0; indice < palabras_objetivo.Length; indice++)
            {
                Console.WriteLine("Escribe la palabra a buscar Num. {0}.", indice);
                linealeida = Console.ReadLine();
                if (linealeida.Length < 2)
                {
                    Console.WriteLine("No se admiten palabras de menos de dos caracteres.\n Se cancela el proceso.");
                    Console.ReadKey(); // Para hacer una pausa y dar tiempo a leer el mensaje.
                    return false;
                }
                palabras_objetivo[indice] = new ListaBuscar(linealeida);
            }
            return true;
        }

        static bool Cargar_Datos_Fichero(string nombrefichero)
        // Retorna "true" si la carga se ha producido sin problemas.Solo devuelve False si el fichero no existe o alguna de las palabras objetivo
        // tiene longitud inferior a 2 caracteres , pero realmentese pueden dar bastantes más casos que no están contemplados en esta solución.
        {
            StreamReader fich_datos;
            string linealeida;
            int filas, columnas;

            fich_datos = new StreamReader(nombrefichero);
            // Comprobar que el fichero exista.
            if (!System.IO.File.Exists(nombrefichero))
            {
                Console.WriteLine("No existe el fichero " + nombrefichero);
                return false;
            }
            fich_datos = File.OpenText(nombrefichero);
            // Empezamos a leer el fichero y a cargar los valores necesarios.
            linealeida = fich_datos.ReadLine();
            // Cargamos el número de filas y columnas que vamos a necesitar.
            filas = Convert.ToInt32(linealeida.Substring(0, linealeida.IndexOf(" ")));
            columnas = Convert.ToInt32(linealeida.Substring(linealeida.IndexOf(" ")));
            sopa_filas = new string[filas];
            // Cargamos los caracteres de la matriz de la Sopa de Letras.
            for (int indice = 0; indice < filas; indice++)
            {
                linealeida = fich_datos.ReadLine();
                if (linealeida.Length < columnas) // Por si nos falta algún caracter en alguna de las líneas.
                {
                    Console.WriteLine("La línea " + indice + " no tiene el número de caracteres necesarios.");
                    fich_datos.Close();
                    return false;
                }
                sopa_filas[indice] = linealeida.Substring(0, columnas); // Para que no tome más columnas de las debidas.
            }
            // Cargamos el número de palabras a buscar.
            linealeida = fich_datos.ReadLine();
            palabras_objetivo = new ListaBuscar[Convert.ToInt32(linealeida)];
            for (int indice = 0; indice < palabras_objetivo.Length; indice++)
            {
                linealeida = fich_datos.ReadLine();
                if (linealeida.Length < 2)
                {
                    Console.WriteLine("No se admiten palabras de menos de dos caracteres.\n Se cancela el proceso.");
                    Console.ReadKey(); // Para hacer una pausa y dar tiempo a leer el mensaje.
                    return false;
                }
                palabras_objetivo[indice] = new ListaBuscar(linealeida);
            }

            fich_datos.Close();
            return true;
        }

        static bool buscar_palabra(string palabra, int xinicial, int yinicial, int inc_x, int inc_y)
        {
            bool retorno = false;
            bool seguir = true;
            int iteraciones = 0;

            while (seguir)
            {
                if (xinicial < 0 || yinicial < 0 || xinicial + 1 > sopa_filas.Length || yinicial + 1 > sopa_filas[0].Length)
                {
                    seguir = false;
                }
                else
                {
                    if (palabra.Substring(iteraciones, 1) == sopa_filas[xinicial].Substring(yinicial, 1))
                    {
                        iteraciones++;
                        xinicial += inc_x;
                        yinicial += inc_y;
                        if (iteraciones == palabra.Length) // Hemos encontrado una coincidencia.
                        {
                            retorno = true;
                            seguir = false;
                        }
                    }
                    else
                    {
                        seguir = false;
                    }
                }
            }

            return retorno;

        }
    }

    class ListaBuscar
    // Guarda las palabras y las veces que la hemos encontrado.
    {
        string _palabra;
        int _veces;

        public ListaBuscar(string palabra)
        {
            _palabra = palabra;
            _veces = 0;
        }
        public string veces()
        {
            return _veces.ToString().Trim();
        }
        public string palabra()
        {
            return _palabra;
        }
        public int Longitud()
        {
            return _palabra.Length;
        }
        public void encontrada()
        {
            _veces++;
        }

    }
}