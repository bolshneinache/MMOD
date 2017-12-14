using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Laba02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            Random rng = new Random();
            const float a = 0;
            const float b = 1;
            int amount= int.Parse(textBox1.Text);
            List<double> ValuesList = new List<double>();
            List<double> FrequencyList = new List<double>();
            for (int i = 0; i < 10; i++) FrequencyList.Add(0);
            List<double> ConsistencyList = new List<double>();//очевидно объявления

            for (int i = 0; i < amount; i++)
            {
                ValuesList.Add(rng.NextDouble());//генерим рандомные значения
                for (int j = 0; j < 10; j++) if ((ValuesList[i]* 10 > j) && (ValuesList[i] * 10 < j + 1)) FrequencyList[j]++;//...и сразу же заполняем список частотами
            }

            ExpectancyTextBox.Text = ((a + b) / 2).ToString();//МО
            DispersionTextBox.Text = ((b - a) ^ 2 / 12).ToString();//Дисперсия

            FrequencyChart.Series.Clear();//Логика графика частот
            for (int i = 0; i < 10; i++) 
            {
                FrequencyList[i] = FrequencyList[i] / amount;
                Series s = FrequencyChart.Series.Add(string.Concat(i + 1, ") ", FrequencyList[i].ToString()););
                s.Points.Add(FrequencyList[i]);
            }

            FunctionChart.Series[0].Points.Clear();
            ValuesList.Sort();
            for (var i = 0; i < ValuesList.Count; i++) FunctionChart.Series[0].Points.AddXY((double)i / 10, ValuesList[i]);//Логика графика функции распределения

            ConsistencyChart.Series[0].Points.Clear();
            for (var i = 0; i < ValuesList.Count; i++)
	    {
	        ConsistencyList.Add((double)1 / (b - a));
                ConsistencyChart.Series[0].Points.AddXY((double)i / 10, ConsistencyList[i]);//Логика графика плотности распределения
	    }
        }
    }
}
