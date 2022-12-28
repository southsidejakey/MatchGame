using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public int tenthsOfSecondsElapsed;
        public int matchesFound;
        public int penalty;
        List<string> hsL = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }
        private string Penalty()
        {
            if (penalty != 0)
            {
                return  $" (penalty: {penalty} secs)";
            }
            else
            {
                return"";
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F+penalty).ToString("0.0s")+Penalty();
            if(matchesFound == 8)
            {
                timer.Stop();
                hsL.Add(timeTextBlock.Text);
                timeTextBlock.Text = timeTextBlock.Text + "\n - Play again?" ;
                highScores.Visibility = Visibility.Visible;
            }
        }

        private void SetUpGame()
        {
            highScores.Visibility = Visibility.Hidden;
            highScoreList.Visibility = Visibility.Hidden;
            highScoreReset.Visibility = Visibility.Hidden;
            List<string> animalEmoji = new List<string>()
            {
                "😂","😂",
                "💩","💩",
                "🐄","🐄",
                "🐸","🐸",
                "🐉","🐉",
                "🐕‍","🐕‍",
                "🤑","🤑",
                "👌","👌",

            };

            Random random = new Random();

            foreach (TextBox textBlock in mainGrid.Children.OfType<TextBox>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
            penalty = 0;



        }
        public TextBox lastTextBlockClicked;
        public bool findingMatch = false;
        

        private void TextBox_TouchDown(object sender, TouchEventArgs e)
        {
            TextBox textBlock = sender as TextBox;

            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
                MixMatch();
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
                penalty++;
            }
        }

        private void timeTextBlock_TouchDown(object sender, TouchEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
        private void MixMatch()
        {
            List<string> boxes = new List<string>();
            Random random = new Random();
            foreach (TextBox textBlock in mainGrid.Children.OfType<TextBox>())
            {
                
                if (textBlock.Visibility != Visibility.Hidden && textBlock.Name != "timeTextBlock")
                {
                    boxes.Add(textBlock.Text);
                }
            }
            foreach (TextBox textBlock in mainGrid.Children.OfType<TextBox>())
            {

                if (textBlock.Visibility != Visibility.Hidden && textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(boxes.Count);
                    string newEmoji = boxes[index];
                    textBlock.Text = newEmoji;
                    boxes.RemoveAt(index);

                }
            }
        }
        
        private void highScores_TouchDown(object sender, TouchEventArgs e)
        {
            hsL.Sort();
            highScoreList.Text = "";
            int index = 0;
            foreach(string score in hsL)
            {
                highScoreList.Text = highScoreList.Text + $"{index+1}. {hsL[index]} \n";
                index++;
            }
            highScoreList.Visibility = Visibility.Visible;
            highScoreReset.Visibility = Visibility.Visible;
        }

        private void highScoreReset_TouchDown(object sender, TouchEventArgs e)
        {
            hsL.Clear();
            highScoreList.Text = "No Highscores";
        }
    }
}
