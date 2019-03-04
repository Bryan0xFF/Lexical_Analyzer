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
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;

                fileLecture.ReadFile(path);
            }

        }
    }
}
