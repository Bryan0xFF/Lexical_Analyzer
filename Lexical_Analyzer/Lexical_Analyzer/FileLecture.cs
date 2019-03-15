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
        public static Dictionary<string, List<string>> alfabetos = new Dictionary<string, List<string>>();



        public string ReadFile(string path)
        {
            try
            {
                //TODO: Validar por que no me incluye la R del 'O''R' en el token
                //TODO: ingresar reglas ya al arbol para su construccion
                String final_RE = null;
                StreamReader streamReader = new StreamReader(path);

                Dictionary<string, List<string>> actions = new Dictionary<string, List<string>>();

                Dictionary<string, string> tokens = new Dictionary<string, string>();
                int linea = 0;
                int columna = 1;
                string RegEx = "";

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

                                case "RESERVADAS()":
                                    final_RE = final_RE.Remove(final_RE.Length - 1, 1);
                                    final_RE = final_RE + ".#";
                                    default_value = "RESERVADAS";
                                    break;

                                default:
                                    //no contiene ninguna palabra reservada
                                    if (default_value == "SETS")
                                    {//si la palabra reservada sigue siendo SETS entonces obtiene expresion en datos[1]
                                        string[] datos = new string[2];
                                        string nombre = "";
                                        columna = 0;

                                        for (int i = 0; i < readline.Length; i++)
                                        {
                                            string separador = readline.Substring(i, 1);

                                            if (separador == "=")
                                            {
                                                datos[0] = nombre;
                                                datos[1] = readline.Substring(i + 1, (readline.Length - 1) - (i));
                                                break;
                                            }
                                            columna++;
                                            nombre += readline[i];
                                        }
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

                                                while (i < datos[1].Length && datos[1].Substring(i + 1, 1) != "(")
                                                {
                                                    string set = "CHR";
                                                    string integer_init = "";
                                                    string integer_final = "";

                                                    dato_actual += datos[1].Substring(i + 1, 1);

                                                    if (set.Contains(dato_actual))
                                                    {
                                                        if (dato_actual == "CHR")
                                                        {
                                                            if (i + 2 < datos[1].Length)
                                                            {
                                                                i++;
                                                                columna++;
                                                                //se busca el numero contenido dentro del CHR
                                                                integer_init += datos[1].Substring(i + 2, 1);
                                                                i = i + 2;
                                                                columna = columna + 2;

                                                                while (i + 1 < datos[1].Length && datos[1].Substring(i + 1, 1) != ")")
                                                                {
                                                                    integer_init += datos[1].Substring(i + 1, 1);
                                                                    i++;
                                                                    columna++;
                                                                }

                                                                //se trata de parsear el dato, si da error entonces 
                                                                //hay inconsistencia de datos

                                                                try
                                                                {
                                                                    //integer_init += datos[1].Substring(i + 2, 1);

                                                                    if (i + 2 < datos[1].Length)
                                                                    {
                                                                        if (datos[1].Substring(i + 2, 1) == ".")
                                                                        {//se verifica si el siguiente es un punto y si el i + 2 es diferente de .
                                                                            if (datos[1].Substring(i + 3, 1) == ".")
                                                                            {//posiblemente es un intervalo
                                                                                i = i + 3;
                                                                                columna = columna + 3;
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
                                                                                                columna++;
                                                                                            }
                                                                                            break;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            i++;
                                                                                            columna++;
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
                                                                            columna++;
                                                                            dato_actual = "";
                                                                            string dato_convertir = "";

                                                                            while (i + 1 < datos[1].Length)
                                                                            {
                                                                                dato_actual += datos[1].Substring(i + 1, 1);
                                                                                i++;
                                                                                columna++;
                                                                                if ("CHR".Contains(dato_actual))
                                                                                {
                                                                                    if (dato_actual == "CHR")
                                                                                    {
                                                                                        if (i + 2 < datos[1].Length)
                                                                                        {
                                                                                            dato_convertir += datos[1].Substring(i + 2, 1);
                                                                                            i = i + 2;
                                                                                            columna = columna + 2;
                                                                                            //se procede a tomar la parte numerica del CHR,
                                                                                            //caso termine por un ), entonces convertira el numero a 
                                                                                            //char y lo agregara al diccionario
                                                                                            while (i + 1 < datos[1].Length && datos[1].Substring
                                                                                                (i + 1, 1) != ")")
                                                                                            {
                                                                                                dato_convertir += datos[1].Substring(i + 1, 1);
                                                                                                i++;
                                                                                                columna++;
                                                                                            }

                                                                                            //se agrega al alfabeto
                                                                                            int num = int.Parse(dato_convertir);
                                                                                            alfabeto.Add(Convert.ToChar(num).ToString());
                                                                                            dato_convertir = "";

                                                                                            dato_actual = "";
                                                                                            if (i + 1 < datos[1].Length)
                                                                                            {
                                                                                                i = i + 1;
                                                                                                columna++;
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
                                                            columna++;
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
                                                        else
                                                        {
                                                            ingreso_valido = true;
                                                            alfabeto.Add(dato_actual);
                                                            i = i + 2;
                                                            columna = columna + 2;
                                                            continue;
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
                                                                columna = columna + 4;
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
                                                    columna++;
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
                                        string[] datos = new string[2];
                                        string nombre = "";
                                        columna = 0;

                                        for (int i = 0; i < readline.Length; i++)
                                        {
                                            string separador = readline.Substring(i, 1);

                                            if (separador == "=")
                                            {
                                                datos[0] = nombre;
                                                datos[1] = readline.Substring(i + 1, (readline.Length - 1) - (i));
                                                break;
                                            }

                                            
                                            nombre += readline[i];
                                            columna++;
                                        }
                                        /*
                                        string[] data = datos[0].Split();
                                        bool validation = false;
                                        string actualToken = "";
                                        string digits = "0123456789";

                                        for (int j = 0; j < data.Length; j++)
                                        {

                                            if (data[j] == " " || data[j] == "\t" || data[j] == "")
                                            {
                                                continue;
                                            }

                                            if (data[j].ToUpper() == "TOKEN")
                                            {
                                                validation = true;
                                            }

                                            if (digits.Contains(data[j]))
                                            {
                                                actualToken = data[j];
                                            }

                                            
                                        }

                                        if (validation != true)
                                        {
                                            throw new Exception("Error en token no: " + actualToken);
                                        }
                                        */

                                        string dato_actual = "";
                                        RegEx = "";
                                        ingreso_valido = false;
                                        bool exists = false;
                                        linea++;

                                        if (datos[1] == null)
                                        {
                                            continue;
                                        }

                                        for (int i = 0; i < datos[1].Length; i++)
                                        {
                                            exists = false;
                                            dato_actual = datos[1].Substring(i, 1);

                                            if (dato_actual == "'" && ingreso_valido == false)
                                            {
                                                ingreso_valido = true;
                                                columna++;
                                                continue;
                                            }

                                            if (dato_actual == "(" || dato_actual == ")")
                                            {

                                                if (dato_actual == ")")
                                                {
                                                    RegEx = RegEx.Remove(RegEx.Length - 1, 1);
                                                    //se concatena para ver si hay un caracter especial fuera
                                                    RegEx += dato_actual;
                                                }
                                                else
                                                {
                                                    if (datos[1].Substring(i - 1, 1) == ".")
                                                    {
                                                        RegEx += ".";
                                                        continue;
                                                    }
                                                    RegEx += dato_actual;
                                                }

                                                continue;
                                            }

                                            if (dato_actual == "'" && ingreso_valido == true)
                                            {
                                                if (i + 1 < datos[1].Length && datos[1].Substring(i - 1, 1) == "'")
                                                {
                                                    if (/*datos[1].Substring(i + 2, 1) == "'" &&*/ datos[1].Substring(i + 1, 1) == "'")
                                                    {
                                                        for (int j = 0; j < alfabetos.Count; j++)
                                                        {
                                                            List<string> alfabeto_temp = alfabetos.ElementAt(j).Value;

                                                            if (alfabeto_temp.Contains(dato_actual))
                                                            {
                                                                exists = true;
                                                            }
                                                        }

                                                        if (exists)
                                                        {
                                                            //se toma el dato actual como parte de algun alfabeto y se agrega a la RegEx
                                                            RegEx += dato_actual + ".";
                                                            i++;
                                                            columna = columna ++ ;
                                                            ingreso_valido = false;
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            throw new Exception("Error en linea No: " + linea + "Columna: " + columna);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ingreso_valido = false;
                                                    }
                                                }
                                                else
                                                {
                                                    ingreso_valido = false;
                                                    columna++;
                                                    continue;
                                                }

                                            }

                                            if (dato_actual == "\"" && ingreso_valido == true)
                                            {
                                                if (datos[1].Substring(i + 1, 1) == "'")
                                                {//el " es un dato a validar dentro del alfabeto

                                                    for (int j = 0; j < alfabetos.Count; j++)
                                                    {
                                                        List<string> alfabeto_temp = alfabetos.ElementAt(j).Value;

                                                        if (alfabeto_temp.Contains(dato_actual))
                                                        {
                                                            exists = true;
                                                        }
                                                    }

                                                    if (exists)
                                                    {
                                                        if (datos[1].Substring(i + 1, 1) == "|")
                                                        {
                                                            RegEx += dato_actual + "|";
                                                            i++;
                                                            columna++;
                                                            ingreso_valido = false;
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            RegEx += dato_actual + ".";
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("Error en linea No: " + linea + "Columna: " + columna);
                                                    }
                                                }
                                            }

                                            if (dato_actual == " " || dato_actual == "\t")
                                            {
                                                columna++;
                                                continue;
                                            }

                                            if ((dato_actual == "|" || dato_actual == "*" || dato_actual == "?"
                                                || dato_actual == "+") && ingreso_valido == false)
                                            {//ingreso de nullables especiales a la RegEx

                                                if (i + 1 < datos[1].Length)
                                                {
                                                    if (dato_actual == "|")
                                                    {//verificamos si el dato anterior de la RegEx es una concat

                                                        if (RegEx.Substring(RegEx.Length - 1, 1) == ".")
                                                        {
                                                            RegEx = RegEx.Remove(RegEx.Length - 1, 1);
                                                        }
                                                        RegEx += dato_actual;
                                                        continue;
                                                    }
                                                    if (datos[1].Substring(i + 1, 1) == "|" && datos[1].Substring(i + 1, 1) != " ")
                                                    {
                                                        RegEx += dato_actual + "|";
                                                        i++;
                                                        columna++;
                                                        continue;
                                                    }
                                                    if (datos[1].Substring(i + 1, 1) != " ")
                                                    {
                                                        while (datos[1].Substring(i + 1, 1) == " ")
                                                        {
                                                            dato_actual = datos[1].Substring(i + 1, 1);
                                                            i++;
                                                            columna++;
                                                        }

                                                        if (datos[1].Substring(i + 1, 1) == "|")
                                                        {
                                                            RegEx += dato_actual + "|";
                                                            i++;
                                                            columna++;
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (RegEx[RegEx.Length - 1] == '.')
                                                        {
                                                            RegEx = RegEx.Remove(RegEx.Length - 1, 1);
                                                        }
                                                        RegEx += dato_actual;
                                                    }
                                                }
                                                else
                                                {//falta agregarle el numeral del final
                                                 //tiene una concat que hay que quitar
                                                    if (RegEx[RegEx.Length - 1] == '.')
                                                    {
                                                        RegEx = RegEx.Remove(RegEx.Length - 1, 1);
                                                    }
                                                    RegEx += dato_actual;
                                                }
                                                continue;

                                            }

                                            if ((dato_actual == "|" || dato_actual == "*" || dato_actual == "?"
                                                || dato_actual == "+") && ingreso_valido == true)
                                            {// es parte de algun alfabeto
                                                for (int k = 0; k < alfabetos.Count; k++)
                                                {
                                                    if (alfabetos.ElementAt(k).Value.Contains(dato_actual))
                                                    {
                                                        RegEx += dato_actual + "Æ.";
                                                        exists = true;
                                                        break;
                                                    }
                                                }

                                                if (exists)
                                                {
                                                    continue;
                                                }
                                            }

                                            if (dato_actual != "'" && dato_actual != "\"" && ingreso_valido == false)
                                            {//posible ingreso de alfabeto completo

                                                if (i + 1 < datos[1].Length)
                                                {
                                                    while (datos[1].Substring(i + 1, 1) != " " && datos[1].Substring(i + 1, 1) != "\t"
                                                                                                       && i + 1 < datos[1].Length)
                                                    {
                                                        dato_actual += datos[1].Substring(i + 1, 1);
                                                        i++;
                                                        columna++;
                                                    }
                                                }
                                                //aqui los espacios son los que denotan fin de chunk de texto
                                               

                                                if (alfabetos.ContainsKey(dato_actual))
                                                {
                                                    i++;
                                                    columna++;
                                                    if (i + 1 < datos[1].Length && datos[1].Substring(i + 1, 1) == "|")
                                                    {
                                                        RegEx += dato_actual + "|";
                                                        i++;
                                                        columna++;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        RegEx += dato_actual + ".";
                                                        continue;
                                                    }

                                                }
                                                else
                                                {
                                                    throw new Exception("Error en linea No: " + linea + "Columna: " + columna);
                                                }
                                            }

                                            if (ingreso_valido == true && dato_actual != "'")
                                            {
                                                string palabra = dato_actual;

                                                while (datos[1].Substring(i + 1, 1) != "'" && i + 1 < datos[1].Length)
                                                {
                                                    dato_actual = datos[1].Substring(i + 1, 1);
                                                    palabra += dato_actual;
                                                    i++;
                                                    columna++;
                                                }

                                                if (i + 2 < datos[1].Length)
                                                {
                                                    if (datos[1].Substring(i + 2, 1) == "'")
                                                    {
                                                        for (int j = 0; j < alfabetos.Count; j++)
                                                        {
                                                            if (alfabetos.ElementAt(j).Value.Contains(dato_actual))
                                                            {
                                                                exists = true;
                                                                break;
                                                            }

                                                        }

                                                        if (exists)
                                                        {
                                                            RegEx += dato_actual + ".";
                                                            columna = columna + 2;
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            throw new Exception("Error en linea No: " + linea + "Columna: " + columna);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        RegEx += dato_actual + ".";
                                                    }

                                                }
                                                else
                                                {//se busca el dato en los alfabetos
                                                    for (int j = 0; j < alfabetos.Count; j++)
                                                    {
                                                        if (alfabetos.ElementAt(j).Value.Contains(dato_actual))
                                                        {
                                                            exists = true;
                                                            break;
                                                        }
                                                    }

                                                    if (exists)
                                                    {
                                                        RegEx += dato_actual + ".";
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("Error en linea No: " + linea + "Columna: " + columna);
                                                    }

                                                }
                                            }
                                        }

                                        if (RegEx.Substring(RegEx.Length - 1, 1) == ".")
                                        {
                                            RegEx = RegEx.Remove(RegEx.Length - 1, 1);
                                        }

                                        final_RE += "(" + RegEx + ")" + "|";
                                    }

                                    if (default_value == "ACTIONS")
                                    {

                                    }
                                    break;


                            }
                        }
                    }
                }

                return final_RE;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        private void Eliminate_Comments(string path)
        {
            //meter al inicio del metodo de arriba para eliminar comentarios
            //crear un nuevo archivo foo_no_comment.txt para salida de texto sin comentarios
        }
    }
}
