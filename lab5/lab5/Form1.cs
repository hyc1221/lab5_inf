using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab5
{
    public partial class Form1 : Form
    {

        double[] code, code_h;
        double[] b, s;
        double[,] s_c;
        double[,] binary;
        int k =9, countb;
        Random rand = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            Calc_code();
            Output_code();
            Calc_code_h();
            Output_s_c();
            Output_code_h();
        }

        void Calc_code()
        {
            code = new double[k];
            for (int i = 0; i < k; i++) code[i] = rand.Next(2);
        }

        void Calc_code_h()
        {
            countb = 0;
            int p = 0;
            
            while (Math.Pow(2, p) < k)
            {
                p++;
                countb++;
            }
            int x = k + countb;
            int y = x / 2 + 1;
            code_h = new double[x];
            binary = new double[x, countb];
            int pp = 0;
            for (int i = 0; i < x; i++)
            {
                pp = i + 1;
                for (int j = countb - 1; j >= 0; j--)
                {
                    binary[i, j] = pp % 2;
                    pp /= 2;
                }  
            }

            s_c = new double[countb, x];
            for (int i = 0; i < countb; i++)
                for (int j = 0; j < x; j++) s_c[i, j] = -1;

            for (int i = 0; i < countb; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    bool ok = true;
                    for (int h = 0; h < countb; h++)
                        if (binary[Convert.ToInt32(Math.Pow(2,i)) - 1, h] == 1 && binary[j, h] == 0)
                        {
                            ok = false;
                            break;
                        }
                    if (ok) s_c[i, j] = 1;
                }
            }
            int g = 0;
            for (int i = 0; i < x; i++)
            {
                double sum = 0;
                for (int j = 0; j < countb; j++) sum += binary[i, j];
                if (sum != 1)
                {
                    code_h[i] = code[g];
                    g++;
                }
            }
            b = new double[countb];

            for (int i = 0; i < countb; i++)
            {
                for (int j = 0; j < x; j++) if (s_c[i, j] == 1) b[i] += code_h[j];
                if (b[i] % 2 != 0) b[i] = 1;
                else b[i] = 0;
            }
            g = 0;
            for (int i = 0; i < x; i++)
                if (Math.Pow(2, g) == i + 1)
                {
                    code_h[i] = b[g];
                    g++;
                }
        }

        void Output_code()
        {
            for (int i = 0; i < k; i++) richTextBox1.AppendText(code[i].ToString());
            richTextBox1.AppendText("\n");
        }

        void Output_s_c()
        {
            for (int i = 0; i < countb; i++)
            {
                richTextBox1.AppendText("\n");
                for (int j = 0; j < countb + k; j++)
                    if (s_c[i,j] != -1) richTextBox1.AppendText((j + 1).ToString() + " ");
            }
            richTextBox1.AppendText("\n");
        }

        void Output_code_h()
        {
            for (int i = 0; i < k + countb; i++) richTextBox1.AppendText(code_h[i].ToString());
            richTextBox1.AppendText("\n");
        }

        public Form1()
        {
            InitializeComponent();
        }

    }
}
