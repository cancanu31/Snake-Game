using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNAKE
{
    public partial class Crearesnake : Form
    {
        OleDbConnection con = new OleDbConnection();
        public static string numedat = string.Empty;
        public static string prenumedat = string.Empty;
        public static string emaildat = string.Empty;

        public Crearesnake()
        {
            InitializeComponent();
            con.ConnectionString =
          @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = D:\AN 3\SEM 1\IS\SNAKE\JUCATORI.accdb";
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            numedat = textBox4.Text;
            prenumedat = textBox5.Text;
            emaildat = textBox6.Text;
            int ct = 0, ct1 = 0;
            con.Open();
            string sql1 = "select nume from Participanti_snake where Nume='" + textBox4.Text + "'";
            OleDbCommand cmd = new OleDbCommand(sql1, con);
            OleDbDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ct++;
                numedat = rdr[0].ToString();
            }
            if (ct > 0)
            {
                MessageBox.Show("Numele exista deja!");
                textBox1.Text = "";
                rdr.Close();
                con.Close();
            }
            else
            {
                con.Close();
                con.Open();
                string sql2 = "select nume from Participanti_snake where nume='" + textBox5.Text + "'";
                OleDbCommand cmd1 = new OleDbCommand(sql2, con);
                OleDbDataReader rdr1 = cmd1.ExecuteReader();
                while (rdr1.Read())
                    ct1++;
                if (ct1 > 0)
                {
                    MessageBox.Show("Prenumele exista deja!");
                    textBox2.Text = "";
                    rdr1.Close();
                    con.Close();
                }
                else
                {
                    con.Close();
                    con.Open();
                    string sql3 = "insert into Participanti_snake(Nume,Prenume,Email,Scor)" +
                    " values('" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "'," + 0 + ")";
                    OleDbCommand cmd2 = new OleDbCommand(sql3, con);
                    cmd2.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("V-ați inregistrat cu succes!");
                    con.Close();
                    Snakegame f = new Snakegame(numedat);
                    f.Show();
                    this.Hide();
                }

            }

        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

   
    }
}
