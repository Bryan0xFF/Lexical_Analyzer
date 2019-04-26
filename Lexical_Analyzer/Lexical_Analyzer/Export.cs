using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class Export
    {
        StringBuilder sb;
        int estado = 0;
        Dictionary<string, bool> esFinal = new Dictionary<string, bool>();


        public Export(Dictionary<string, string> AFD)
        {
            sb = new StringBuilder();
        }

        private Dictionary<int, List<string>> ObtenerTransiciones(Dictionary<string, string> AFD, List<int> cantidadTransiciones)
        {
            Dictionary<int, List<string>> transiciones = new Dictionary<int, List<string>>();
            //List<string> insert = new List<string>();

            //TODO: agregar nodos con su distintivo si es final o no esFinal = Dictionary<string, bool>
            esFinal = new Dictionary<string, bool>();

            for (int i = 0; i < cantidadTransiciones.Count ; i++)
            {
                esFinal.Add(i.ToString(), false);
            }

            for (int i = 0; i < cantidadTransiciones.Count; i++)
            {   //se selecciona q0 como primera transicion
                List<string> insert = new List<string>();

                for (int j = 0; j < AFD.Count; j++)
                {
                    
                    string[] split = AFD.ElementAt(j).Key.Split('/');
                    string compare = "";

                    for (int k = 1; k < split[0].Trim().Length; k++)
                    {
                        compare += split[0].Substring(k, 1);
                    }

                    if (Convert.ToInt32(compare) == i)
                    {
                        string add_string = "";

                        //se obtiene el dato de movimiento hacia otra transicion
                        for (int k = 1; k < AFD.ElementAt(j).Value.Length; k++)
                        {
                            if (int.TryParse(AFD.ElementAt(j).Value.Substring(k, 1), out int n) == true)
                            {
                                add_string += AFD.ElementAt(j).Value.Substring(k, 1);

                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (AFD.ElementAt(j).Value.Substring(0, 1) == "#")
                        {   //quiere decir que el nodo al cual se ira es terminal
                            string numero = add_string;
                            //numero = numero.Remove(0, 1);

                            esFinal[numero] = true;
                        }

                        add_string += "/";
                        add_string += split[1];
                        insert.Add(add_string);
 
                    }
                    else
                    {
                        continue;
                    }
                }

                transiciones.Add(i, new List<string>(insert));
                

            }

            

            return transiciones;
        }

        private List<int> obtenerCantidadTransiciones(Dictionary<string, string> AFD)
        {
            List<int> TotalTransiciones = new List<int>();
            string numero = "";

            for (int i = 0; i < AFD.Count; i++)
            {
                numero = "";
                string[] temp = AFD.ElementAt(i).Key.Split('/');
                
                for (int j = 1; j < temp[0].Trim().Length; j++)
                {
                    numero += temp[0].Substring(j, 1);    
                }

                if (!TotalTransiciones.Contains(Convert.ToInt32(numero)))
                {
                    TotalTransiciones.Add(Convert.ToInt32(numero));
                }

                numero = "";

                for (int j = 1; j < AFD.ElementAt(i).Value.Length; j++)
                {
                    if (int.TryParse(AFD.ElementAt(i).Value.Substring(j, 1), out int n) == true)
                    {
                        numero += AFD.ElementAt(i).Value.Substring(j, 1);
                    }
                    
                }

                if (!TotalTransiciones.Contains(Convert.ToInt32(numero)))
                {
                    TotalTransiciones.Add(Convert.ToInt32(numero));
                }
            }

            TotalTransiciones.Sort();

            return TotalTransiciones;
        }

        public string GenerateCode(Dictionary<string, string> AFD, int state)
        {
            List<int> totalTransiciones = new List<int>();
            totalTransiciones = obtenerCantidadTransiciones(AFD);
            Dictionary<int, List<string>> transiciones = new Dictionary<int, List<string>>();
            transiciones = ObtenerTransiciones(AFD, totalTransiciones);
            //char Lexema = readline[pos++];

            sb.AppendLine("case " + estado.ToString() + ":");

            List<string> datos_transicion = transiciones[estado];

            for (int i = 0; i < totalTransiciones.Count; i++)
            {   //se agregan los datos a la transicion al StringBuilder

                string data = "";
                data += "if(";

                for (int j = 0; j < datos_transicion.Count; j++)
                {// vamos insertando en orden para cambio de estados

                    string[] split = datos_transicion.ElementAt(j).Split('/');

                    //[0] = numero transicion, [1] con que se va a la transicion
                    if (split[1] == " \'")
                    {
                        split[1] = "\\" + split[1].Trim();
                    }
                    if (split[1] == " \"")
                    {
                        split[1] = "\\" + split[1].Trim(); 
                    }

                    if (Convert.ToInt32(split[0]) == i + 1)
                    {
                        if (split[1].Trim().Length > 1 && split[1] != "\\\'" && split[1] != "\\\"")
                        {
                            for (int k = 0; k < FileLecture.alfabetos.Count; k++)
                            {
                                if (split[1].Trim() == FileLecture.alfabetos.ElementAt(k).Key.Trim())
                                {
                                    int count = 1;
                                    int dato_inicial = Convert.ToChar(FileLecture.alfabetos.ElementAt(k).Value.ElementAt(0));
                                    int dato_final = Convert.ToChar(FileLecture.alfabetos.ElementAt(k).Value.ElementAt(
                                        FileLecture.alfabetos.ElementAt(k).Value.Count - 1));

                                    if (dato_final > dato_inicial)
                                    {
                                        char dato = (char)dato_inicial;
                                        data += "Lexema >= \'" + dato + "\' &&";
                                        dato = (char)dato_final;
                                        data += " Lexema <= \'" + dato + "\' ||";
                                    }
                                    else
                                    {
                                        while (dato_final < dato_inicial)
                                        {
                                            count = count + 1;
                                            dato_final = Convert.ToChar(FileLecture.alfabetos.ElementAt(k).Value.ElementAt(
                                            FileLecture.alfabetos.ElementAt(k).Value.Count - 1));
                                        }
                                        
                                    }
                                }
                            }
                        }
                        else
                        {
                            data += "Lexema == \'" + split[1].Trim() + "\'";
                            data += " ||";
                        }

                    }
                }

                data = data.Remove(data.Length - 2, 2);
                data += ")" + "\r\n";

                if (data[0] == 'i' && data[1] == ')' /*&& data[2] == ')'*/)
                {
                    data = "";
                    continue;
                }
                else
                {
                    
                    sb.Append(data.Trim());
                    sb.AppendLine("{");

                }
                sb.AppendLine("word_chunk += readline[pos].ToString();");
                sb.AppendLine("estado = " + (i + 1).ToString() + ";");

                //en retroceso, hacer un lexema-- y validar que no se vuelva a ingresar al mismo if
                sb.AppendLine("pos = pos + 1;");
                sb.AppendLine("ValuateStr(readline, pos, estado);");
                sb.AppendLine("return;");
                sb.AppendLine("}");
                
            }
           
            if (estado < totalTransiciones.Count )
            {
                if (estado > 0)
                {
                    if (esFinal[estado.ToString()] == true && transiciones[estado].Count == 0)
                    {
                        sb.AppendLine("error = Retroceso(estado);");
                        sb.AppendLine("if(!error)");
                        sb.AppendLine("{");
                        sb.AppendLine("Console.WriteLine(word_chunk + \":\" + VerificarActions(\"ERROR\").ToString());");
                        sb.AppendLine("Console.ReadLine();");
                        sb.AppendLine("int originalLength = readline.Length;");
                        sb.AppendLine("readline = readline.Substring(word_chunk.Length, (readline.Length - word_chunk.Length));");
                        sb.AppendLine("if (pos < originalLength)");
                        sb.AppendLine("{");
                        sb.AppendLine("word_chunk = \"\";");
                        sb.AppendLine("ValuateStr(readline, 0, 0);");
                        sb.AppendLine("return;");

                        sb.AppendLine("}");
                        sb.AppendLine("}");
                        sb.AppendLine("else");
                        sb.AppendLine("{");
                        sb.AppendLine("Console.WriteLine(word_chunk + \":\" + VerificarActions(word_chunk).ToString()); ");
                        sb.AppendLine("Console.ReadLine();");
                        sb.AppendLine("int originalLength = readline.Length;");
                        sb.AppendLine("readline = readline.Substring(word_chunk.Length, (readline.Length - word_chunk.Length));");
                        sb.AppendLine("if (pos < originalLength)");
                        sb.AppendLine("{");
                        sb.AppendLine("word_chunk = \"\";");
                        sb.AppendLine("ValuateStr(readline, 0, 0);");
                        sb.AppendLine("return;");

                        sb.AppendLine("}");
                        sb.AppendLine("}");
                    }
                    else
                    {
                        sb.AppendLine("else");
                        sb.AppendLine("{");
                        sb.AppendLine("error = Retroceso(estado);");
                        sb.AppendLine("if(!error)");
                        sb.AppendLine("{");
                        sb.AppendLine("Console.WriteLine(word_chunk + \":\" + VerificarActions(\"ERROR\").ToString());");
                        sb.AppendLine("Console.ReadLine();");
                        sb.AppendLine("int originalLength = readline.Length;");
                        sb.AppendLine("readline = readline.Substring(word_chunk.Length, (readline.Length - word_chunk.Length));");
                        sb.AppendLine("if (pos < originalLength)");
                        sb.AppendLine("{");
                        sb.AppendLine("word_chunk = \"\";");
                        sb.AppendLine("ValuateStr(readline, 0, 0);");
                        sb.AppendLine("return;");

                        sb.AppendLine("}");

                        sb.AppendLine("}");
                        sb.AppendLine("else");
                        sb.AppendLine("{");
                        sb.AppendLine("Console.WriteLine(word_chunk + \":\" + VerificarActions(word_chunk).ToString());");
                        sb.AppendLine("Console.ReadLine();");
                        sb.AppendLine("int originalLength = readline.Length;");
                        sb.AppendLine("readline = readline.Substring(word_chunk.Length, (readline.Length - word_chunk.Length));");
                        sb.AppendLine("if (pos < originalLength)");
                        sb.AppendLine("{");
                        sb.AppendLine("word_chunk = \"\";");
                        sb.AppendLine("ValuateStr(readline, 0, 0);");
                        sb.AppendLine("return;");
                        sb.AppendLine("}");

                        sb.AppendLine("}");
                        sb.AppendLine("}");
                    }
                   
                }

                
            }

            if (estado + 1 < totalTransiciones.Count)
            {
                sb.AppendLine();

                sb.AppendLine("break;");
                estado = estado + 1;
                GenerateCode(AFD, estado);
            }
            /*
            if (estado + 1 == totalTransiciones.Count)
            {
                sb.AppendLine("break;");
            }
            */
            else
            {
                //sb.AppendLine("break;");
                return sb.ToString();
            }

            return sb.ToString();
        }

        public string ExportCode(Dictionary<string, string> automata)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using System.IO;");

            sb.AppendLine();

            sb.AppendLine("namespace Lexical_Analyzer");
            sb.AppendLine("{");
            sb.AppendLine("class Program");
            sb.AppendLine("{");

            sb.AppendLine("public static char lexema_anterior = default(char);");
            sb.AppendLine("public static char Lexema = default(char);");
            sb.AppendLine("public static int pos = 0;");
            sb.AppendLine("public static string word_chunk = \"\";");
            sb.AppendLine("public static bool error = false;");
            sb.AppendLine();
            sb.AppendLine("static void Main(string[] args)");
            sb.AppendLine("{");
            sb.AppendLine("StreamReader sr = new StreamReader(@\"C:\\Users\\Bryan\\Desktop\\Scanner.txt\");");
            sb.AppendLine("while(!sr.EndOfStream)");
            sb.AppendLine("{");
            sb.AppendLine("string readline = sr.ReadLine().Trim();");
            sb.AppendLine("ValuateStr(readline, 0, 0);");
            sb.AppendLine("word_chunk = \"\";");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("public static void ValuateStr(string readline, int pos, int estado)");
            sb.AppendLine("{");
            sb.AppendLine("if( pos == readline.Length)");
            sb.AppendLine("{");
            sb.AppendLine("error = Retroceso(estado);");
            sb.AppendLine("if(!error)");
            sb.AppendLine("{");
            sb.AppendLine("Console.WriteLine(word_chunk + \":\" + VerificarActions(\"ERROR\").ToString());");
            sb.AppendLine("Console.ReadLine();");
            sb.AppendLine("int originalLength = readline.Length;");
            sb.AppendLine("readline = readline.Substring(word_chunk.Length, (readline.Length - word_chunk.Length));");
            sb.AppendLine("if (pos < originalLength)");
            sb.AppendLine("{");
            sb.AppendLine("word_chunk = \"\";");
            sb.AppendLine("ValuateStr(readline, 0, 0);");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("else");
            sb.AppendLine("{");
            sb.AppendLine("Console.WriteLine(word_chunk + \":\" + VerificarActions(word_chunk).ToString()); ");
            sb.AppendLine("Console.ReadLine();");
            sb.AppendLine("int originalLength = readline.Length;");
            sb.AppendLine("readline = readline.Substring(word_chunk.Length, (readline.Length - word_chunk.Length));");
            sb.AppendLine("if (pos < originalLength)");
            sb.AppendLine("{");
            sb.AppendLine("word_chunk = \"\";");
            sb.AppendLine("ValuateStr(readline, 0, 0);");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("return;");
            sb.AppendLine("}");
            sb.AppendLine("Lexema = readline[pos];");
            sb.AppendLine();
            sb.AppendLine(" switch( " + "estado" + " )");
            sb.AppendLine(" {");

            GenerateCode(automata, 0);

            sb.AppendLine("break;");
            sb.AppendLine(" }");
            sb.AppendLine("}");
            //sb.AppendLine("}");
            //sb.AppendLine("}");
            
            sb.AppendLine("private static bool Retroceso(int estado)");
            sb.AppendLine("{");
            sb.AppendLine(EscribirRetroceso());
            sb.AppendLine("}");
            sb.AppendLine("private static int VerificarActions(string estado)");
            sb.AppendLine("{");
            VerificarActions();
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void RetreiveTokens()
        {
            
        }

        private void VerificarActions()
        {
            sb.AppendLine("switch(estado)");
            sb.AppendLine("{");

            for (int i = 0; i < FileLecture.ACTIONS.Count; i++)
            {
                sb.AppendLine("case" + "\"" + FileLecture.ACTIONS.ElementAt(i).Value +"\"" + ":");
                sb.AppendLine("return " + FileLecture.ACTIONS.ElementAt(i).Key.ToString() + ";");
            }

            sb.AppendLine("default: ");
            sb.AppendLine("return -1;");
            sb.AppendLine("}");
        }

        private string EscribirRetroceso()
        {
            string data = "";
            data += "switch(estado)\r\n";
            data += "{\r\n";

            for (int i = 0; i < esFinal.Count; i++)
            {
                data += "case " + i.ToString() + ":\r\n";
                data += "return " + esFinal.ElementAt(i).Value.ToString().ToLower() +";" + "\r\n";
            }
            data += "default:\r\n";
            data += "return false; \r\n";
            data += "}";
            return data;
        }
    }
}
