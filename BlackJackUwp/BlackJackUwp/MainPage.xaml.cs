using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BlackJackUwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Card> playerHand;
        List<Card> dealerHand;
        Deck deck;

        public MainPage()
        {
            this.InitializeComponent();
            setupTable();
            


        }

        public void setupTable()
        {
            Grid table = FindName("grdContainer") as Grid;

            playerHand = new List<Card>();
            dealerHand = new List<Card>();

            deck = new Deck();

            deck.shuffle();

            playerHand.Add(deck.DealCard());
            dealerHand.Add(deck.DealCard());
            playerHand.Add(deck.DealCard());
            dealerHand.Add(deck.DealCard());

            Rectangle pc1 = CreateCard(playerHand[0]);
            Rectangle pc2 = CreateCard(playerHand[1]);
            Rectangle dc1 = CreateCard(dealerHand[0]);
            Rectangle dc2 = CreateCard(dealerHand[1]);

            table.Children.Add(pc1);
            pc1.SetValue(Grid.RowProperty, 3);
            pc1.SetValue(Grid.ColumnProperty, 2);
            table.Children.Add(pc2);
            pc2.SetValue(Grid.RowProperty, 3);
            pc2.SetValue(Grid.ColumnProperty, 3);
            table.Children.Add(dc1);
            dc1.SetValue(Grid.RowProperty, 1);
            dc1.SetValue(Grid.ColumnProperty, 2);
            table.Children.Add(dc2);
            dc2.SetValue(Grid.RowProperty, 1);
            dc2.SetValue(Grid.ColumnProperty, 3);

            playerScore.Text= "Player Score : " + GetScore(playerHand);
            dealerScore.Text = "Dealer Score : " + GetScore(dealerHand);
        }

        public Rectangle CreateCard(Card card)
        {
                Rectangle c = new Rectangle();
                ImageBrush bpBrush = new ImageBrush();
                bpBrush.ImageSource = new BitmapImage(new Uri(card.pictureLocation));
                c.Fill = bpBrush;
                return c;
        }
        public void gameOver()
        {

        }
        public void Hitfunc(string player)
        {
            if (player.Equals("player"))
            { 
                Card c = deck.DealCard();
                playerHand.Add(c);
                Rectangle r = CreateCard(c);
                Grid table = FindName("grdContainer") as Grid;
                table.Children.Add(r);
                r.SetValue(Grid.RowProperty, 3);
                r.SetValue(Grid.ColumnProperty, playerHand.Count+1);
                playerScore.Text = "Player Score : " + GetScore(playerHand);
                if (GetScore(playerHand)>21)
                {
                    gameOver();
                }
            }
        }

        public int GetScore(List<Card> hand)
        {
            int score =0;
            int containsAce = 0;
            foreach(Card card in hand)
            {
                if (card.rank.Equals("J") || card.rank.Equals("Q") || card.rank.Equals("K"))
                {
                    score += 10;
                }
                else if (card.rank.Equals("A"))
                {
                    containsAce++;
                    score += 11;
                }
                else
                {
                    score += Int32.Parse(card.rank);
                }
            }
            
            if(score > 21)
            {
                while(containsAce>0 && score>21)
                {
                    score -= 10;
                    containsAce--;
                }
            }
            return score;
        }

        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            Hitfunc("player");
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
