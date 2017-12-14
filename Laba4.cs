using System;
using System.Windows.Forms;

namespace Laba04
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GameButton_Click(object sender, EventArgs e)
        {
            int winCondition, primordialChoice, amountOfWins = 0, amountOfLoses = 0;
            Random randomizer = new Random();
            bool changeDecision = false;
            if (ChangeChoiceCheckBox.Checked) changeDecision = true;
            int amountOfGames = int.Parse(GamesAmountTextBox.Text);
            int amountOfDoors = int.Parse(DoorAmountTextBox.Text);

            for (int i = 0; i < amountOfGames; i++)
            {
                winCondition = randomizer.Next(1, amountOfDoors + 1); //Определение значения победной ситуации
                primordialChoice = randomizer.Next(1, amountOfDoors + 1); //Определение значения выбранной двери

                if (primordialChoice == winCondition && changeDecision == false) amountOfWins++; //Выбрав верную дверь и не меняя выбора мы побеждаем.
                else if (primordialChoice != winCondition && changeDecision == false) amountOfLoses++; //Выбрав неверную дверь и не меняя выбора проигрываем.
                //Дальше остаётся только 2 двери, одна из которых - наш первоначальный выбор.
                else if (primordialChoice == winCondition && changeDecision == true) amountOfLoses++;//Выбрав верную дверь и изменив выбор мы проигрываем.
                else amountOfWins++; //Выбрав неверную дверь и изменив выбор мы побеждаем.
            }
            MessageBox.Show(string.Concat("Количество побед: ", amountOfWins, ", количество поражений: ", amountOfLoses));
        }
    }
}
