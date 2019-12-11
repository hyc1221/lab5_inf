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
        double[,] s_c; // хранит индексы для s, которые надо суммировать
        double[,] binary; // массив, содержащий двоичные числа
        int k = 9, countb, countmis, ind_mis = -1;
        Random rand = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            Calc_code();
            Output_code();
            Calc_code_h();
            Calc_code_h2();
            Calc_mistake();
            Calc_s();
            Output_s_c();
            Output_b();
            Output_code_h();
            Output_mis();
            Output_s();
        }

        void Calc_code()
        {
            code = new double[k];
            for (int i = 0; i < k; i++) code[i] = rand.Next(2);
        }

        void Calc_code_h() //код хэмминга
        {
            countb = 0; // количество проверочных бит b
            while (Math.Pow(2, countb) < k)
            {
                countb++;
            }
            int x = k + countb; // прост чтоб много раз не писать
            int y = x / 2 + 1; 
            code_h = new double[x];
            binary = new double[x, countb];
            int pp = 0;
            for (int i = 0; i < x; i++) //генерация двоичных чисел от 0 до k + countb
            {
                pp = i + 1;
                for (int j = countb - 1; j >= 0; j--)
                {
                    binary[i, j] = pp % 2;
                    pp /= 2;
                }  
            }

            s_c = new double[countb + 1, x + 1]; //индексы для s, которые суммируются
            for (int i = 0; i <= countb; i++)
                for (int j = 0; j <= x; j++) s_c[i, j] = -1; // -1 значит этот индекс не используется

            for (int i = 0; i < countb; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    bool ok = true;
                    for (int h = 0; h < countb; h++)
                        if (binary[Convert.ToInt32(Math.Pow(2,i)) - 1, h] == 1 && binary[j, h] == 0) //если нужные биты из binary совпадают, то записываем индекс в s_c
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
                for (int j = 0; j < countb; j++) sum += binary[i, j]; // если сумма единиц равна 1, то число - степень двойки
                if (sum != 1)
                {
                    code_h[i] = code[g]; // заполнение кодовой комбинации в код хэмминга на нужные позиции
                    g++;
                }
            }
            b = new double[countb + 1]; // массив проверочных битов с учетом последнего бита, для проверки на две ошибки

            for (int i = 0; i < countb; i++)
            {
                for (int j = 0; j < x; j++) if (s_c[i, j] == 1) b[i] += code_h[j]; //вычисление проверочных бит по индексам, которые посчитали ранее
                if (b[i] % 2 != 0) b[i] = 1;
                else b[i] = 0;
            }
            g = 0;
            for (int i = 0; i < x; i++)
                if (Math.Pow(2, g) == i + 1)
                {
                    code_h[i] = b[g]; //заполнение проверочных битов в код хэмминга
                    g++;
                }
        }

        void Calc_code_h2()
        {
            code_h2 = new double[k + countb + 1]; //код хэмминга для двух ошибок
            for (int i = 0; i <= k + countb; i++)
            {
                if (i < k + countb) code_h2[i] = code_h[i]; //копирование всех бит обычного кода хэмминга в новый массив
                s_c[countb, i] = 1; // у последнего проверочного бита суммируются все биты кода хэмминга
                if (i < k + countb) b[countb] += code_h[i];
            }
            if (b[countb] % 2 != 0) b[countb] = 1;
            else b[countb] = 0;
            code_h2[k + countb] = b[countb]; //запись последнего проверочного бита в код хэмминга
        }

        void Calc_mistake()
        {
            mis = new double[k + countb + 1];// код с ошибкой
            for (int i = 0; i < k + countb + 1; i++) mis[i] = code_h2[i]; //копирование всех битов из кода хэмминга
             int mis_count = rand.Next(3); // генерация количества ошибок от 0 до 2
           // int mis_count = 2;
            for (int i = 0; i < mis_count; i++)
            {
                int mis_ind = rand.Next(k + countb + 1); //генерация позиции ошибки
                if (mis[mis_ind] == 1) mis[mis_ind] = 0;
                else mis[mis_ind] = 1;
            }
        }

        void Calc_s() //расчет контрольной суммы
        {
            ind_mis = 0; //предполагаемая?(хз как правильно пишется) позиция ошибки
            countmis = 0; //предп... количество ошибок
            s = new double[countb + 1]; //контрольная сумма
            for (int i = 0; i < countb; i++)
            {
                for (int j = 0; j < k + countb; j++)
                    if (s_c[i, j] == 1) s[i] += mis[j];
                if (s[i] % 2 != 0) s[i] = 1;
                else s[i] = 0;
            }
            for (int j = 0; j <= k + countb; j++) s[countb] += mis[j]; //расчет последнего бита симптома
            if (s[countb] % 2 != 0) s[countb] = 1;
            else s[countb] = 0;

            if (s[countb] == 0) //если последний бит 0, то ошибки две, либо 0
            {
                double g = 0;
                for (int i = 0; i < countb; i++) g += s[i];
                if (g > 0) countmis = 2; //если есть хоть одна единица, то две ошибки
                else countmis = 0;
             }
            if (s[countb] == 1) //если последний бит 1, то ошибка одна 
            {
                countmis = 1;
                for (int i = 0; i < countb; i++)
                {
                    ind_mis += Convert.ToInt32(s[i] * Math.Pow(2, i)); //перевод из двоичной в 10
                }
                if (ind_mis == 0) //если позиция = 0, значит ошибок нет
                {
                    countmis = 0;
                    ind_mis = -1;
                }
                else ind_mis--;
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

        void Output_mis()
        {
            for (int i = 0; i < k + countb + 1; i++) richTextBox3.AppendText(code_h2[i].ToString());
            richTextBox3.AppendText(" - код Хэмминга\n");
            for (int i = 0; i < k + countb + 1; i++) richTextBox3.AppendText(mis[i].ToString());
            richTextBox3.AppendText(" - код с ошибками (возможно)\n");
        }

        void Output_s()
        {
            for (int i = 0; i <= countb; i++)
                richTextBox3.AppendText("\nS[" + (i + 1) + "] = " + s[i].ToString());
            richTextBox3.AppendText("\n\n");
            for (int i = 0; i <= countb; i++)
                richTextBox3.AppendText(s[i].ToString());
            richTextBox3.AppendText(" - контрольная сумма\n\n");
            richTextBox3.AppendText("Количество ошибок - " + countmis.ToString() + "\n");
            if (countmis == 1) richTextBox3.AppendText("Позиция ошибки - " + (ind_mis + 1).ToString());
        }

        public Form1()
        {
            InitializeComponent();
        }

    }
}
