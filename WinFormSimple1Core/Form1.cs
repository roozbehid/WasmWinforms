using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormSimple1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
         
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Console.Write("pressed");
        }
        static int i = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {

            button1.Text = (i++).ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
