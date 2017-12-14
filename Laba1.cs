using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Laba01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "Результат: ";
            string data = "";
            int parsing;
            bool FirstIteration = true;

            for (int i = 1; i < int.Parse(textBox2.Text) + 1; i++)
            {
                if (FirstIteration == true) parsing = int.Parse(textBox1.Text);
                else parsing = int.Parse(data);

                data = (parsing ^ 2).ToString();

                while (data.Length < 8) data = "0" + data;

                data = data.Remove(0, 2);
                data = data.Remove(4);

                richTextBox1.Text = richTextBox1.Text + data + ",";
                FirstIteration = false; 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "Результат: " + textBox6.Text;
            bool FirstIteration = true;
            int m = int.Parse(textBox3.Text);
            int k = int.Parse(textBox4.Text);
            string str1 = "";
            string str2 = "";
            float z, a = 0;
            float previous_a = 0;

            for (int i = 1; i < int.Parse(textBox5.Text) + 1; i++)
            {
                if (FirstIteration == true) a = int.Parse(textBox6.Text);
                else a = previous_a;

                previous_a = (k * a) % m;
                z = a / m;

                str1 = str1 + previous_a + ",";
                str2 = str2 + z + ",";
                FirstIteration = false;
            }
            richTextBox2.Text = "An: " + str1 + " ; Z: " + str2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Random rng = new Random();
            int amount = int.Parse(textBox8.Text);
            List<double> ValuesList = new List<double>(amount);
            List<double> FrequencyList = new List<double>(10);
            for(int i = 0; i < amount; i++) FrequencyList.Add(0);
            
            for (int i = 0; i < amount; i++)
            {
                ValuesList.Add(rng.NextDouble());
                for (int j = 0; j < amount; j++) if (ValuesList[i]*10 > j && ValuesList[i]*10 < j + 1) FrequencyList[j] = FrequencyList[j] + 1;
            }

            for(int i = 0; i < 10; i++) FrequencyList[i] = FrequencyList[i] / amount;

            chart1.Series.Clear();
            for (int i = 0; i < 10; i++)
            {
                string SerieName = String.Concat(i + 1, ") " ,FrequencyList[i].ToString());
                Series s = chart1.Series.Add(SerieName); 
                s.Points.Add(FrequencyList[i]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<double> ValuesList = new List<double>();
            int amount = int.Parse(textBox7.Text);
            int s = int.Parse(textBox9.Text);
            bool FirstIteration = true;
            double rate = 0;
            double sum = 0;
            double a = 0;
            double previous_a = 0;
            double z;
            int k = 5;
            int m = 3;

            for (int i = 0; i < amount; i++)
            {
                if (FirstIteration == true) a = 13;
                else a = previous_a;

                previous_a = (k * a) % m;
                z = a / m;

                FirstIteration = false;
                ValuesList.Add(z);
            }

            for (int i = 0; i < amount - s; i++) sum += ValuesList[i] * ValuesList[i + s];

            rate = 1.0/(12 * amount - s) * sum;
            richTextBox3.Text = "R = " + rate.ToString();
        }
    }
}
