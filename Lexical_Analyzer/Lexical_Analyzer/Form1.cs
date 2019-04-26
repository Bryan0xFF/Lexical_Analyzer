using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace Lexical_Analyzer
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            ExpressionTree expression = new ExpressionTree();
            ExpressionNode node = expression.PrefixToBinaryTree(expression.toPrefix(textBox1.Text));
            */
            FileLecture fileLecture = new FileLecture();
            To_AFD AFD = new To_AFD();
            ExpressionTree tree = new ExpressionTree();
            OpenFileDialog ofd = new OpenFileDialog();
            Dictionary<int, List<int>> followpos;
            Dictionary<int, string> NodeData = new Dictionary<int, string>();
            

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                try
                {
                    string regEx = fileLecture.ReadFile(path);
                    ExpressionNode root = AFD.CreateTree(regEx);
                    root = tree.assignRules(root);
                    root = tree.AssignMidRules(root);
                    followpos = tree.AssignFollowPos(root);
                    followpos = tree.ComputeFPosConcat(root, followpos);
                    NodeData = tree.ObtainLeafs(root, NodeData);

                    Dictionary<string, string> automata = AFD.CreateAutomata(root, NodeData, followpos);
                    Export export = new Export(automata);
                    tbxCompiler.Text = export.ExportCode(automata);
                    //introducirlo al arbol con reglas

                    //1ero: numero de nodo
                    //segundo: followpos
                    //tercero: dato del arbol
                    DGVFollow.ColumnCount = 2;

                    DGVFollow.RowCount = followpos.Count + 1;

                    DGVFollow[0, 0].Value = "Transicion";
                    DGVFollow[1, 0].Value = "Estado: ";

                    DGVFollow.ColumnHeadersVisible = false;
                    DGVFollow.RowHeadersVisible = false;

                    for (int i = 0; i < automata.Count; i++)
                    {
                        DGVFollow[0, i].Value = automata.ElementAt(i).Key;
                        DGVFollow[1, i].Value = automata.ElementAt(i).Value;
                    }

                    regEx = "";

                    root = new ExpressionNode();
                    followpos = new Dictionary<int, List<int>>();

                }
                catch (Exception x )
                {
                    MessageBox.Show((x.Message), "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


             

            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void btnCompilar_Click(object sender, EventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                saveFileDialog.DefaultExt = ".cs";
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
                streamWriter.Write(tbxCompiler.Text);
                streamWriter.Flush();
                streamWriter.Close();

            }

            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCompilar_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;


                StreamReader streamReader = new StreamReader(path);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(streamReader.ReadToEnd());

                //target framework v 4.6.1
                CSharpCodeProvider csc = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });
                CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, "validacion.exe", true);
                parameters.GenerateExecutable = true;
                CompilerResults compilerResults = csc.CompileAssemblyFromSource(parameters, stringBuilder.ToString());
                
                if (compilerResults.Errors.Cast<CompilerError>().ToList().Count == 0)
                {
                    Process.Start(Application.StartupPath + "/" + "validacion.exe");
                }
                
            }

            
        }
    }
}
