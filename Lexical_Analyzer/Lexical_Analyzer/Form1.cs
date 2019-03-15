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
                    //introducirlo al arbol con reglas

                    //1ero: numero de nodo
                    //segundo: followpos
                    //tercero: dato del arbol
                    DGVFollow.ColumnCount = 3;

                    DGVFollow.RowCount = followpos.Count + 1;

                    DGVFollow[0, 0].Value = "Nodo No: ";
                    DGVFollow[1, 0].Value = "FollowPos: ";
                    DGVFollow[2, 0].Value = "Valor: ";

                    DGVFollow.ColumnHeadersVisible = false;
                    DGVFollow.RowHeadersVisible = false;

                    for (int i = 1; i <= followpos.Count; i++)
                    {
                        DGVFollow[0, i].Value = i;
                    }

                    for (int i = 1; i <= followpos.Count; i++)
                    {
                        foreach (var item in followpos[i])
                        {
                            DGVFollow[1, i].Value += item + " ";
                        }
                    }

                    NodeData = tree.ObtainLeafs(root, NodeData);

                    for (int i = 1; i <= followpos.Count; i++)
                    {
                        DGVFollow[2, i].Value = NodeData[i];
                    }

                    regEx = "";
                    root = new ExpressionNode();
                    followpos = new Dictionary<int, List<int>>();

                    AFD.CreateAutomata(root, NodeData, followpos);

                    
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
    }
}
