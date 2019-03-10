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
            Dictionary<string, string> tokens = new Dictionary<string, string>();
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

                        if (readline == "" && !streamReader.EndOfStream)
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
                                            columna++;
                                            continue;
                                        }

                                        if (dato_actual == "\t" && ingreso_valido == false)
                                        {
                                            columna++;
                                            continue;
                                        }
                                        columna++;

                                        if (ingreso_valido == false && dato_actual == "C")
                                        {//puede que posiblemente sea CHR
                                            
                                            while ( i < datos[1].Length && datos[1].Substring(i + 1, 1) != "(")
                                            {
                                                string set = "CHR";
                                                string integer_init = "";
                                                string integer_final = "";

                                                dato_actual += datos[1].Substring(i + 1, 1);

                                                if (set.Contains(dato_actual))
                                                {
                                                    if (dato_actual == "CHR")
                                                    {
                                                        if ( i + 2 < datos[1].Length)
                                                        {
                                                            i++;
                                                            //se busca el numero contenido dentro del CHR
                                                            integer_init += datos[1].Substring(i + 2, 1);
                                                            i = i + 2;

                                                            while (i + 1 < datos[1].Length && datos[1].Substring(i + 1,1) != ")")
                                                            {
                                                                integer_init += datos[1].Substring(i + 1, 1);
                                                                i++;
                                                            }

                                                            //se trata de parsear el dato, si da error entonces 
                                                            //hay inconsistencia de datos

                                                            try
                                                            {
                                                                //integer_init += datos[1].Substring(i + 2, 1);

                                                                if ( i + 2 < datos[1].Length)
                                                                {
                                                                    if (datos[1].Substring(i + 2, 1) == ".")
                                                                    {//se verifica si el siguiente es un punto y si el i + 2 es diferente de .
                                                                        if (datos[1].Substring( i + 3, 1) == ".")
                                                                        {//posiblemente es un intervalo
                                                                            i = i + 3;
                                                                            dato_actual = "";

                                                                            while (i + 1 < datos[1].Length)
                                                                            {
                                                                                dato_actual += datos[1].Substring(i + 1, 1);

                                                                                if (set.Contains(dato_actual))
                                                                                {
                                                                                    if (dato_actual == "CHR")
                                                                                    {
                                                                                        i = i + 3;

                                                                                        while (datos[1].Substring(i, 1) != ")" && i < datos[1].Length)
                                                                                        {
                                                                                            integer_final += datos[1].Substring(i, 1);
                                                                                            i++;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        i++;
                                                                                    }
                                                                                }

                                                                                else
                                                                                {
                                                                                    throw new Exception();
                                                                                }


                                                                            }
                                                                        }
                                                                        
                                                                    }
                                                                    else if (datos[1].Substring(i + 2, 1) == "C")
                                                                    {//posible concatenacion de CHR's

                                                                        //se incrementa en uno para librarse del ")"
                                                                        i++;
                                                                        dato_actual = "";
                                                                        string dato_convertir = "";

                                                                        while (i + 1 < datos[1].Length)
                                                                        {
                                                                            dato_actual += datos[1].Substring(i + 1, 1);
                                                                            i++;

                                                                            if ("CHR".Contains(dato_actual))
                                                                            {
                                                                                if (dato_actual == "CHR")
                                                                                {
                                                                                    if (i + 2 < datos[1].Length)
                                                                                    {
                                                                                        dato_convertir += datos[1].Substring(i + 2, 1);
                                                                                        i = i + 2;

                                                                                        //se procede a tomar la parte numerica del CHR,
                                                                                        //caso termine por un ), entonces convertira el numero a 
                                                                                        //char y lo agregara al diccionario
                                                                                        while (i + 1 < datos[1].Length && datos[1].Substring
                                                                                            (i + 1, 1) != ")")
                                                                                        {
                                                                                            dato_convertir += datos[1].Substring(i + 1, 1);
                                                                                            i++;
                                                                                        }

                                                                                        //se agrega al alfabeto
                                                                                        int num = int.Parse(dato_convertir);
                                                                                        alfabeto.Add(Convert.ToChar(num).ToString());
                                                                                        dato_convertir = "";

                                                                                        dato_actual = "";
                                                                                        if (i + 1 < datos[1].Length)
                                                                                        {
                                                                                            i = i + 1;

                                                                                            if (integer_init != "")
                                                                                            {
                                                                                                num = int.Parse(integer_init);
                                                                                                alfabeto.Add(Convert.ToChar(num).ToString());
                                                                                                integer_init = "";
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //verificar si el ultimo char es un ")"
                                                                                            if (i + 1 < datos[1].Length)
                                                                                            {
                                                                                                dato_actual = datos[1].Substring(i + 1, 1);

                                                                                                if (dato_actual != ")")
                                                                                                {
                                                                                                    throw new Exception("Error al parsear CHR");
                                                                                                }
                                                                                            }
                                                                                            break;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        throw new Exception();
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                throw new Exception("Error en lectura CHR");
                                                                            }
                                                                        }
                                                                        break;
                                                                    }


                                                                }

                                                                int a = Convert.ToInt32(integer_init);
                                                                int b = Convert.ToInt32(integer_final);

                                                                for (int j = a; j <= b; j++)
                                                                {
                                                                    alfabeto.Add(Convert.ToChar(j).ToString());
                                                                }

                                                                break;
                                                            }
                                                            catch (Exception)
                                                            {
                                                                //hay error en datos almacenados
                                                                throw new Exception("Error en el parse de CHR");
                                                            }

                                                        }
                                                    }
                                                    else
                                                    {
                                                        i++;
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            continue;
                                        }
                                        
                                        if (ingreso_valido == false && dato_actual != "'" && dato_actual != "+" && dato_actual != "\t")
                                        {
                                            //hay un error en el archivo
                                            string pos_err = "Error en lectura archivo. linea: " + linea.ToString() +
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
                                            string datoIngresar = dato_actual;

                                            while (i + 1 < datos[1].Length && datos[1].Substring(i + 1, 1) != "'")
                                            {
                                                dato_actual = datos[1].Substring(i + 1, 1);
                                                datoIngresar += dato_actual;
                                                i++;
                                            }

                                           

                                            alfabeto.Add(datoIngresar);
                                        
                                        }

                                        
                                        if (dato_actual == "'" && count_separadores == 0)
                                        {
                                            //se lleva un contador de separadores de char y se cambia el
                                            //estatus del bool que me permite ingresar si es dato es valido
                                            count_separadores++;
                                            ingreso_valido = true;
                                        }

                                        
                                    }

                                    //se asigna al alfabeto
                                    alfabetos.Add(datos[0].Trim(), alfabeto);

                                }

                                if (default_value == "TOKENS")
                                {
                                    //verificar si los tokens son validos y agregarlo a una RegEx con un separador |
                                    //luego concatenar el .# y operar en el arbol
                                    string[] datos = readline.Split('=');
                                    string dato_actual = "";
                                    ingreso_valido = false;
                                    string RegEx = "";
                                    linea++;

                                    for (int i = 0; i < datos[1].Length; i++)
                                    {
                                        dato_actual = datos[1].Substring(i, 1);

                                        if (dato_actual == "'" && ingreso_valido == false)
                                        {
                                            ingreso_valido = true;
                                            continue;
                                        }

                                        if (dato_actual == "'" && ingreso_valido == true)
                                        {
                                            if (datos[1].Substring(i + 1, 1) == "'")
                                            {
                                                //se toma el dato actual como parte de algun alfabeto y se agrega al alfabeto
                                                RegEx += dato_actual + ".";
                                                i++;
                                                ingreso_valido = false;
                                                continue;
                                            }
                                        }

                                        if (dato_actual != "'" && ingreso_valido == false)
                                        {//posible ingreso de alfabeto completo

                                            //aqui los espacios son los que denotan fin de chunk de texto
                                            while (datos[1].Substring(i + 1, 1) != " " && datos[1].Substring(i + 1, 1) != "\t" 
                                                && i + 1 < datos[1].Length) 
                                            {
                                                dato_actual += datos[1].Substring(i + 1, 1);
                                                i++;
                                            }

                                            if (alfabetos.ContainsKey(dato_actual))
                                            {
                                                if (i + 1 < datos[1].Length && datos[1].Substring(i + 1, 1) == "|")
                                                {
                                                    RegEx += dato_actual + "|";
                                                    i++;
                                                }
                                                else
                                                {
                                                    RegEx += dato_actual + ".";
                                                }
                                                
                                            }

                                        }

                                        while (datos[1].Substring(i + 1, 1) != " " || datos[1].Substring(i + 1, 1) != "\t")
                                        {
                                            dato_actual += datos[1].Substring(i + 1, 1);
                                            i++;
                                        }

                                        //validamos si es palabra clave de algun alfabeto o bien, si es un palabra particular

                                    }
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
