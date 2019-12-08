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

        double[] code, code_h, code_h2, mis;
        double[] b, s;
        double[,] s_c;
        double[,] binary;
        int k = 9, countb;
        Random rand = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            Calc_code();
            Output_code();
            Calc_code_h();
            Calc_code_h2();
            Output_s_c();
            Output_b();
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
            while (Math.Pow(2, countb) < k)
            {
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

            s_c = new double[countb + 1, x];
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
            b = new double[countb + 1];

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

        void Calc_code_h2()
        {
            code_h2 = new double[k + countb + 1];
            for (int i = 0; i < k + countb; i++)
            {
                code_h2[i] = code_h[i];
                s_c[countb, i] = 1;
                b[countb] += code_h[i];
            }
            if (b[countb] % 2 != 0) b[countb] = 1;
            else b[countb] = 0;

        }

        void Calc_mistake()
        {
            mis = new double[k + countb + 1];
            for (int i = 0; i < k + countb + 1; i++) mis[i] = code_h2[i];
            int mis_count = rand.Next(3);
            for (int i = 0; i < mis_count; i++)
            {
                int mis_ind = rand.Next(k + countb + 2);
                if (mis[mis_ind] == 1) mis[mis_ind] = 0;
                else mis[mis_ind] = 1;
            }
        }

        void Output_code()
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            for (int i = 0; i < k; i++) richTextBox3.AppendText(code[i].ToString());
            richTextBox3.AppendText(" - кодовая комбинация\n");
        }

        void Output_s_c()
        {
            for (int i = 0; i <= countb; i++)
            {
                if (i < countb) richTextBox1.AppendText("\nS[" + (i + 1) + "] = ");
                richTextBox2.AppendText("\nS[" + (i + 1) + "] = ");
                for (int j = 0; j < countb + k; j++)
                {
                    if (i < countb)
                        if (s_c[i, j] != -1) richTextBox1.AppendText((j + 1).ToString() + " ");
                    if (s_c[i, j] != -1) richTextBox2.AppendText((j + 1).ToString() + " ");
                }
            }
            richTextBox1.AppendText("\n\n");
            richTextBox2.AppendText("\n\n");
        }

        void Output_b()
        {
            for (int i = 0; i <= countb; i++)
            {
                if (i < countb) richTextBox1.AppendText("\nb[" + (i + 1) + "] = " + b[i].ToString());
                richTextBox2.AppendText("\nb[" + (i + 1) + "] = " + b[i].ToString());
            }
            richTextBox1.AppendText("\n\n");
            richTextBox2.AppendText("\n\n");
        }

        void Output_code_h()
        {
            for (int i = 0; i <= k + countb; i++)
            {
                if (i < k + countb) richTextBox1.AppendText(code_h[i].ToString());
                richTextBox2.AppendText(code_h2[i].ToString());
            }
            richTextBox1.AppendText(" - код Хэмминга для одной ошибки\n");
            richTextBox2.AppendText(" - код Хэмминга для двух ошибок\n");
        }

        public Form1()
        {
            InitializeComponent();
        }

    }
}
