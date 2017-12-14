using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random rng = new Random();
        List<int> xTableList = new List<int>(4) { 10, 20, 30, 40 };//значения хардкодал для лучшей наглядности результатов 
        List<int> yTableList = new List<int>(4) { 1, 2, 3, 4 };
        double[,] Matrix =
        {//значения матрицы - тем более
            { 0.00, 0.11, 0.12, 0.03 },
            { 0.00, 0.13, 0.09, 0.02 },
            { 0.02, 0.11, 0.08, 0.01 },
            { 0.03, 0.11, 0.05, 0.09 }
        };

        private void Button1_Click(object sender, EventArgs e)
        {
            List<double> XDistributionList = new List<double>();//Ряд распределения X
            List<double> YDistributionList = new List<double>();//Ряд распределения Y
            int amount = int.Parse(textBox1.Text);

            List<double> XAndYList = new List<double>();//Список куда будут нагенериваться значения СВ
            List<string> XYChartNames = new List<string>(16);
            for (int i = 0; i < 16; i++) XYChartNames.Add("");

            for (var i = 0; i < 4; i++)//Находим ряды распределения
            {
                double x = 0, y = 0;
                for (var j = 0; j < 4; j++)
                {
                    x += Matrix[i, j];
                    y += Matrix[j, i];
                }
                XDistributionList.Add(x);
                YDistributionList.Add(y);
            }

            FindSomeValues(XDistributionList, YDistributionList);
            RandomNumbersGeneration(amount,XAndYList,XDistributionList, YDistributionList);//нагенеривания случайных значений СВ

            int counter = 0;//Цикл для корректного отображения имён в графике плотности
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    XYChartNames[counter] = (string.Concat(xTableList[i], "; ", yTableList[j]));
                    counter++;
                }
            }

            Plotting(DistributeXOrYValues(XAndYList,"x"), xTableList, "X", XChart); //Построение графиков
            Plotting(DistributeXOrYValues(XAndYList,"y"), yTableList, "Y", YChart); //Построение графиков
            PlottingXY(DistributeXYValues(XAndYList, xTableList, yTableList), XYChartNames, "Плотность", XYChart); //Построение графиков
        }

        private void FindSomeValues(List<double> XDistList, List<double> YDistList)
        { //Расчёт числовых значений (М(x,y),D(x,y),cov(xy),K(xy))

            double MX = 0; //MO X
            for (var i = 0; i < 4; i++) MX += xTableList[i] * XDistList[i];
            MXTextBox.Text = MX.ToString();

            double MY = 0; //МО Y
            for (var i = 0; i < 4; i++) MY += yTableList[i] * YDistList[i];
            MYTextBox.Text = MY.ToString();

            double SQRX = 0; //Среднеквадратичное отклонение и дисперсия X
            for (var i = 0; i < 4; i++) SQRX += (xTableList[i] ^ 2) * XDistList[i];
            DXTextBox.Text = (SQRX - (MX * MX)).ToString();

            double SQRY = 0; //Среднеквадратичное отклонение и дисперсия Y
            for (var i = 0; i < 4; i++) SQRY += (yTableList[i] ^ 2) * YDistList[i];
            DYTextBox.Text = (SQRY - (MY * MY)).ToString();

            double covXY = 0; //Ковариация XY
            for (var i = 0; i < 4; i++) for (var j = 0; j < 4; j++) covXY += xTableList[i] * yTableList[j] * Matrix[i, j];
            covXYTextBox.Text = covXY.ToString();

            KXYTextBox.Text = (covXY - (MX * MY)).ToString();
        }

        private void RandomNumbersGeneration(int amount, List<double> XAndYList, List<double> XDistList, List<double> YDistList)
        { //Нагенериваем случайные величины
            for (int i = 0; i < amount; i++)
            {
                double X,Y;
                double randomedX = rng.NextDouble();
                if (randomedX <= XDistList[0]) X = xTableList[0];
                else if ((randomedX > XDistList[0]) && (randomedX < XDistList[0] + XDistList[1])) X = xTableList[1];
                else if ((randomedX > XDistList[1]) && (randomedX < XDistList[0] + XDistList[1] + XDistList[2])) X = xTableList[2];
                else X = xTableList[3];
                XAndYList.Add(X);

                double randomedY = rng.NextDouble();
                if (randomedY <= YDistList[0]) Y = yTableList[0];
                else if ((randomedY > YDistList[0]) && (randomedY < YDistList[0] + YDistList[1])) Y = yTableList[1];
                else if ((randomedY > YDistList[1]) && (randomedY < YDistList[0] + YDistList[1] + YDistList[2])) Y = yTableList[2];
                else Y = yTableList[3];
                XAndYList.Add(Y);
            }
        }

        private void Plotting(List<double> XAndYList, List<int> SomeList, string chartName, Chart ChartName)
        { //Логика для отрисовки графиков по X и Y
            ChartName.Series.Clear();
            ChartName.Titles.Clear();
            ChartName.Titles.Add(chartName);
            for (var i = 0; i < XAndYList.Count; i++)
            {
                var series = ChartName.Series.Add(String.Concat(i + 1,") ", SomeList[i].ToString()));
                series.Points.Add(XAndYList[i]);
            }
        }

        private List<double> DistributeXOrYValues(List<double> XandYList, string parameter)
        { //Логика получения значений для графиков X и Y
            int multiplier = 1;
            if (parameter == "x") multiplier = 10;
            else if (parameter == "y") multiplier = 1;

            var newList = new List<double>(4) { 0, 0, 0, 0 };
            foreach (var el in XandYList)
            {
                for (int i = 0; i < 4; i++) if (el == (i + 1) * multiplier) newList[i]++;
            }
            return newList;
        }

        private void PlottingXY(List<double> XAndYList, List<string> XYChartNames, string chartName, Chart ChartName)
        { //Логика для отрисовки графика плотности
            ChartName.Series.Clear();
            ChartName.Titles.Clear();
            ChartName.Titles.Add(chartName);
            for (var i = 0; i < XAndYList.Count; i++)
            {
                var series = ChartName.Series.Add(String.Concat(i + 1,")  ", XYChartNames[i].ToString()));
                series.Points.Add(XAndYList[i]);
            }
        }

        private List<double> DistributeXYValues(List<double> XAndYList, List<int> xList, List<int> yList)
        { //Логика получения значений для графика плотности
            List<double> newXYList = new List<double>();
            for (int i = 0; i < 16; i++) newXYList.Add(0);
            for (int i = 0; i < XAndYList.Count; i ++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (XAndYList[i] == xList[j])
                    {
                        if (XAndYList[i + 1] == yList[0]) newXYList[j * 4]++;
                        else if (XAndYList[i + 1] == yList[1]) newXYList[j * 4 + 1]++;
                        else if (XAndYList[i + 1] == yList[2]) newXYList[j * 4 + 2]++;
                        else newXYList[j * 4 + 3]++;
                    }
                }
            }
            return newXYList;
        }
    }
}