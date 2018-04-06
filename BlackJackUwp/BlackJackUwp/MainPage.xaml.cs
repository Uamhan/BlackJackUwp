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

        public MainPage()
        {
            this.InitializeComponent();
            setupTable();
            


        }

        public void setupTable()
        {

            List<Card> playerHand = new List<Card>();
            List<Card> dealerHand = new List<Card>();

            Deck deck = new Deck();

            playerHand.Add(deck.DealCard());
            dealerHand.Add(deck.DealCard());
            playerHand.Add(deck.DealCard());
            dealerHand.Add(deck.DealCard());

            Rectangle pc1 = CreateCard(playerHand[0]);
            Rectangle pc2 = CreateCard(playerHand[1]);
            Rectangle dc1 = CreateCard(dealerHand[0]);
            Rectangle dc2 = CreateCard(dealerHand[1]);

            pc1.SetValue(Grid.RowProperty, 3);
            pc1.SetValue(Grid.ColumnProperty, 3);


        }

        public Rectangle CreateCard(Card card)
        {
                Rectangle c = new Rectangle();
                ImageBrush bpBrush = new ImageBrush();
                bpBrush.ImageSource = new BitmapImage(new Uri(card.pictureLocation));
                c.Fill = bpBrush;
                return c;
        }

        public int GetScore(Card[] hand)
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
    }
}
