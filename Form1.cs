using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string name1="";
        public static string name2 = "";
        public static int timer ;

        private void button1_Click(object sender, EventArgs e)
        {
            name1 = textBox1.Text;
            name2 = textBox2.Text;
            timer = Convert.ToInt32(textBox3.Text);

            Form2 form2 = new Form2();
            form2.ShowDialog();
            
        }
    }
}
