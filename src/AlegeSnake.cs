using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SNAKE
{
    public partial class Alegesnake : Form
    {
        public Alegesnake()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Crearesnake f = new Crearesnake();
            f.Show();
            Hide();
        }
    }
}
