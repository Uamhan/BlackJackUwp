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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BlackJackUwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        int cardHeight = 125;
        int cardWidth = 100;
        int maxCards = 6;

        public MainPage()
        {
            this.InitializeComponent();

            

            Deck deck = new Deck();
            deck.loadimages();
            deck.shuffle();
            Card[] playerHand = {deck.DealCard(), deck.DealCard()};
            Card[] dealerHand = {deck.DealCard(), deck.DealCard()};
            int playerscore = GetScore(playerHand);
            setupTable();
            


        }

        public void setupTable()
        {
            Grid grdPlayerHand = new Grid();
            grdPlayerHand.Name = "playerHand";
            grdPlayerHand.Height = cardHeight ;
            grdPlayerHand.Width = cardWidth * maxCards;
            grdPlayerHand.Margin = new Thickness(5);
            grdPlayerHand.Background = new SolidColorBrush(Colors.Gray);
            grdPlayerHand.SetValue(Grid.ColumnProperty, 1);
            grdPlayerHand.SetValue(Grid.RowProperty, 1);
            grdPlayerHand.HorizontalAlignment = HorizontalAlignment.Center;
            grdPlayerHand.VerticalAlignment = VerticalAlignment.Bottom;

            for (int i = 0; i < maxCards;i++)
            {
                grdPlayerHand.ColumnDefinitions.Add(new ColumnDefinition());
                
            }

            grdContainer.Children.Add(grdPlayerHand);

            Grid grdDealerHand = new Grid();
            grdDealerHand.Name = "dealerHand";
            grdDealerHand.Height = cardHeight;
            grdDealerHand.Width = cardWidth * maxCards;
            grdDealerHand.Margin = new Thickness(5);
            grdDealerHand.Background = new SolidColorBrush(Colors.Gray);
            grdDealerHand.SetValue(Grid.ColumnProperty, 1);
            grdDealerHand.SetValue(Grid.RowProperty, 1);
            grdDealerHand.HorizontalAlignment = HorizontalAlignment.Center;
            grdDealerHand.VerticalAlignment = VerticalAlignment.Top;

            for (int i = 0; i < maxCards; i++)
            {
                grdPlayerHand.ColumnDefinitions.Add(new ColumnDefinition());

            }

            grdContainer.Children.Add(grdDealerHand);

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
