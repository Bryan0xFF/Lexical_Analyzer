using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lexical_Analyzer
{
    class FileLecture
    {
        public void ReadFile(string path)
        {
            //TODO: crear automata para validar strings SETS, TOKENS y RESERVADAS
            //TODO: añadir validacion para .. y CHR
            //TODO: añadir validacion para comentarios
            //validar si vienen palabras 'sdasdas'

            StreamReader streamReader = new StreamReader(path);
            Dictionary<string, List<string>> alfabetos = new Dictionary<string, List<string>>();
            int linea = 0;
            int columna = 0;

            while (!streamReader.EndOfStream)
            {
                //leemos la linea de codigo actual
                string readline = streamReader.ReadLine();
                linea++;
                string[] validate = readline.Split();
                string default_value = "";
                bool ingreso_valido = false;
                //validamos si pertenece a algun tipo de dato reservado dentro del archivo

                if (validate[0] == "SETS" || default_value == "SETS")
                {//es un set y se mete a validar alfabetos hasta que encuentra la palabra TOKENS

                    default_value = validate[0];

                    
                    while (!streamReader.EndOfStream)
                    {

                        if (default_value == "SETS" && linea == 1)
                        {
                            linea++;
                            continue;
                        }

                        if (!streamReader.EndOfStream)
                        {
                            readline = streamReader.ReadLine();
                        }

                        if (readline == "\t" && !streamReader.EndOfStream)
                        {
                            continue;
                        }

                        validate = readline.Trim().Split();

                        switch (validate[0])
                        {
                            case "TOKENS":
                                default_value = "TOKENS";
                                break;

                            case "RESERVADAS":
                                default_value = "RESERVADAS";
                                break;

                            default:
                                //no contiene ninguna palabra reservada
                                if (default_value == "SETS")
                                {//si la palabra reservada sigue siendo SETS entonces obtiene expresion en datos[1]
                                    string[] datos = readline.Split('=');
                                    linea++;

                                    //datos[1] contiene la expresion de alfabeto
                                    List<string> alfabeto = new List<string>();
                                    int count_separadores = 0;

                                    for (int i = 0; i < datos[1].Length; i++)
                                    {
                                        string dato_actual = datos[1].Substring(i, 1);

                                        //validar espacios
                                        if (dato_actual == " " && ingreso_valido == false)
                                        {
                                            continue;
                                        }
                                        columna++;

                                        
                                        if (ingreso_valido == false && dato_actual != "'" && dato_actual != "+")
                                        {
                                            //hay un error en el archivo
                                            string pos_err = "Errorlectura archivo. linea: " + linea.ToString() +
                                                ", columna: " + columna.ToString();
                                            throw new Exception(pos_err);
                                        }
                                        

                                        if (dato_actual == "'" && count_separadores == 1)
                                        {
                                            //se verifica la siguiente posicion para ver si no contiene algun caracter
                                            //especial que haya que tomar como parte del alfabeto
                                            if (i + 2 < datos[1].Length)
                                            {
                                                dato_actual = datos[1].Substring(i + 2, 1);
                                            }

                                            if (i + 2 > datos[1].Length)
                                            {// quiere decir que es el ultimo char enclosure
                                                ingreso_valido = false;
                                                count_separadores = 0;
                                                continue;
                                            }

                                            //esto quiere decir que el dato intermedio es un caracter de escape '
                                            if (dato_actual == "'")
                                            {
                                                if (i + 1 < datos[1].Length)
                                                {
                                                    if ((dato_actual = datos[1].Substring(i + 1, 1)) == "+")
                                                    {
                                                        ingreso_valido = false;
                                                        count_separadores = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    //ingresar el dato ' al diccionario
                                                    alfabeto.Add(dato_actual);
                                                }   
                                            }

                                            if (dato_actual == ".")
                                            {//se verifica que el caracter anterior sea un punto tambien
                                                dato_actual = datos[1].Substring(i + 1, 1);

                                                if (dato_actual == ".")
                                                {//es un intervalo
                                                    //se obtiene el ultimo elemento para sacar su char value
                                                    string interval_init = alfabeto.ElementAt(alfabeto.Count - 1);
                                                    string interval_final = "";

                                                    if (i + 4 < datos[1].Length)
                                                    {
                                                        //se obtiene el intervalo final
                                                        dato_actual = datos[1].Substring(i + 4, 1);
                                                        interval_final = dato_actual;
                                                    }
                                                    else
                                                    {
                                                        throw new Exception();
                                                    }

                                                    char init_val = Convert.ToChar(interval_init);
                                                    int init = init_val + 1;

                                                    for (int j = init; j < 256; j++)
                                                    {
                                                        string actual = Convert.ToString((char)j);

                                                        if (actual == interval_final)
                                                        {//se llego al intervalo y termina el ciclo
                                                            i = i + 4;
                                                            break;
                                                        }

                                                        alfabeto.Add(actual); 
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //se cierra el gap de ingreso de datos y se cambia el estatus de ingreso
                                                count_separadores = 0;
                                                ingreso_valido = false;
                                            }

                                        }

                                        if (ingreso_valido == true && dato_actual != "'") 
                                        {//tiene permitido ingresar al diccionario
                                            alfabeto.Add(dato_actual);
                                            continue;
                                        }

                                        
                                        if (dato_actual == "'" && count_separadores == 0)
                                        {
                                            //se lleva un contador de separadores de char y se cambia el
                                            //estatus del bool que me permite ingresar si es dato es valido
                                            count_separadores++;
                                            ingreso_valido = true;
                                        }

                                        
                                    }

                                }

                                if (default_value == "TOKENS")
                                {
                                    //verificar si los tokens son validos y agregarlo a una RegEx con un separador |
                                    //luego concatenar el .# y operar en el arbol
                                }
                                break;
                        }
                    }
                }
            }
        }


        private void Eliminate_Comments(string path)
        {
            //meter al inicio del metodo de arriba para eliminar comentarios
            //crear un nuevo archivo foo_no_comment.txt para salida de texto sin comentarios
        }
    }
}
