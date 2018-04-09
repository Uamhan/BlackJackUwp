using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
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
    public sealed partial class MainPage : Page
    {
        //global variables
        List<Card> playerHand;      //list containing player cards
        List<Card> dealerHand;      //list containing dealer cards
        int playerwins = 0;         //integer containing number of player wins
        int dealerwins = 0;         //integer containing number of dealer wins
        Deck deck;                  //deck object containing all usable cards

        MediaPlayer mediaPlayer;    //media player used to play sounds

        public MainPage()
        {
            this.InitializeComponent();
            mediaPlayer = new MediaPlayer();      //initilisation of media player
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/draw.wav")); //source of draw card sound
          
            setupTable();   //calls setup table function which initilises the game board
    
        }

        public async void setupTable()
        {
            Grid table = FindName("grdContainer") as Grid;      //gets the main xaml grid

            playerHand = new List<Card>();      //initilises player hand
            dealerHand = new List<Card>();      //initilises dealer hand

            deck = new Deck();                  //initilises deck of useable cards

            deck.shuffle();                     //shuffles deck of cards to a random order

            //deals two cards to the player and the dealer
            playerHand.Add(deck.DealCard());
            dealerHand.Add(deck.DealCard());
            playerHand.Add(deck.DealCard());
            dealerHand.Add(deck.DealCard());

            //creates rectagle objects based on cards to display cards to the user
            Rectangle pc1 = CreateCard(playerHand[0]);
            Rectangle pc2 = CreateCard(playerHand[1]);
            Rectangle dc1 = CreateCard(dealerHand[0]);
            Rectangle dc2 = CreateCard(dealerHand[1]);
            
            //adds cards to the grid to display to the user
            table.Children.Add(pc1);
            pc1.SetValue(Grid.RowProperty, 3);
            pc1.SetValue(Grid.ColumnProperty, 1);
            table.Children.Add(pc2);
            pc2.SetValue(Grid.RowProperty, 3);
            pc2.SetValue(Grid.ColumnProperty, 2);
            table.Children.Add(dc1);
            dc1.SetValue(Grid.RowProperty, 1);
            dc1.SetValue(Grid.ColumnProperty, 1);
            table.Children.Add(dc2);
            dc2.SetValue(Grid.RowProperty, 1);
            dc2.SetValue(Grid.ColumnProperty, 2);
            
            //sets curret score text for the player and dealer hands to be displayed to the user
            playerScore.Text= "Player Score : " + GetScore(playerHand);
            dealerScore.Text = "Dealer Score : " + GetScore(dealerHand);
            //plays sound of cards being dealt
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/shuffle.wav"));
            mediaPlayer.Play();
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/draw.wav"));
        }
        //creates a rectagle based off of a card object
        public Rectangle CreateCard(Card card)
        {
                Rectangle c = new Rectangle();      //initilises new rectangle
                ImageBrush bpBrush = new ImageBrush();      //initilises new image brush
                bpBrush.ImageSource = new BitmapImage(new Uri(card.pictureLocation));       //sets image brush source to the card image
                c.Fill = bpBrush;       //fills the rectangle with the card image
                return c;
        }
        //function called at the end of each hand
        public void gameOver()
        {
            //sets winner text and play again button to visable
            Winner.Visibility= Visibility.Visible;
            playAgain.Visibility = Visibility.Visible;
            //condition for the player to win
            if (GetScore(playerHand)>GetScore(dealerHand) && GetScore(playerHand) < 22 || GetScore(dealerHand) > 21 && GetScore(playerHand)<22)
            {
                playerwins++;       //increases player win count
                Winner.Text = "Player Wins";        //sets winner text to the player has won
                playerWins.Text = "Player Wins : "+playerwins;      //sets number of wins text
            }
            //dealer wins
            else
            {
                dealerwins++;       //increases dealer win count
                Winner.Text = "Dealer Wins";        //sets winner text to the dealer has won
                dealerWins.Text = "Dealer Wins : "+dealerwins;      //sets number of wins text
            }
            //disables gameplay buttons
            Hit.IsEnabled = false;
            Check.IsEnabled = false;
            
        }
        // function that adds a new card to a hand
        public void Hitfunc(string player)
        {
            //if the player has a requested a new card
            if (player.Equals("player"))
            {
                
                Card c = deck.DealCard();       //initilises a new card from the deck
                playerHand.Add(c);              //adds new card to the layer hand
                Rectangle r = CreateCard(c);    //initilises a new rectangle to display this new card
                Grid table = FindName("grdContainer") as Grid;      //gets the main grid
                table.Children.Add(r);      //adds new card to the grid
                r.SetValue(Grid.RowProperty, 3);        //sets row property fro the new card
                r.SetValue(Grid.ColumnProperty, playerHand.Count);      //sets column propert for the new card
                playerScore.Text = "Player Score : " + GetScore(playerHand);        //sets new player score
                //if the player score has gone over 21 calls gameOver function
                if (GetScore(playerHand)>21)
                {
                    gameOver();
                }

            }
            //dealer has requested a new card
            else
            {
                Card c = deck.DealCard();       //creates a new card from the deck
                dealerHand.Add(c);              //adds the new card to the dealers hand
                Rectangle r = CreateCard(c);    //initilises a new rectangle to display this new card
                Grid table = FindName("grdContainer") as Grid;      //gets the main grid
                table.Children.Add(r);      //adds new card to the grid
                r.SetValue(Grid.RowProperty, 1);        //sets row property for the new card
                r.SetValue(Grid.ColumnProperty, dealerHand.Count);  //sets column property for the new card
                dealerScore.Text = "Dealer Score : " + GetScore(dealerHand);        //sets dealer score
                
            }
        }
        //function that calculates score of a hand
        public int GetScore(List<Card> hand)
        {
            int score =0;       //integer containg the score value
            int containsAce = 0;        //integer that stores the number of aces in a hand
            //foreach loop that iterates through each card in the hand
            foreach(Card card in hand)
            {
                //if the card is a jack queen or king add 10 to the score
                if (card.rank.Equals("J") || card.rank.Equals("Q") || card.rank.Equals("K"))
                {
                    score += 10;
                }
                //if the card is an ace add 11 to the score and increment containsace value
                else if (card.rank.Equals("A"))
                {
                    containsAce++;
                    score += 11;
                }
                //add score bases on the rank of the card
                else
                {
                    score += Int32.Parse(card.rank);
                }
            }
            //if the hand score is greater than 21
            if(score > 21)
            {
                //while loop that checks if there are aces in the hand and that the score is still over 21
                while(containsAce>0 && score>21)
                {
                    //takes 10 from the score changeing the value of the ace form 11 to 1 and decrements the containsace value
                    score -= 10;
                    containsAce--;
                }
            }
            return score;
        }
        //function called when the hit button is clicked
        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            Hitfunc("player");      //calls the hit function for the player
            mediaPlayer.Play();     //plays the draw card sound
        }
        //function called when the player clicks the check button
        private async void Check_Click(object sender, RoutedEventArgs e)
        {
            int finalPlayerScore = GetScore(playerHand); //stores the players final score in integer finalPlayerScore
            //while the player score is greater than the dealer score
            while (GetScore(dealerHand) < finalPlayerScore)
            {
                Hitfunc("dealer");      //deal a card to the dealer
                mediaPlayer.Play();     //play card draw sound
                await Task.Delay(TimeSpan.FromSeconds(1));      //wait 1 second
            }
            //call end of game function
            gameOver();


        }
        //function that is called when the player clicks the play again button
        private void playAgain_Click(object sender, RoutedEventArgs e)
        {

            Grid table = FindName("grdContainer") as Grid;      //gets the main grid

            int index = 0;      //index used in for loop
            int children = table.Children.Count;        //integer used to contain number of children in the grid
            //for loop that iterates through the children in the grid and deletes all rectangle
            for (int i = 0; i < children; i++)
            {
                Rectangle r = table.Children[index] as Rectangle;
                if (r != null)
                {
                    //Remove children
                    table.Children.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
            
            Winner.Visibility = Visibility.Collapsed;       //changes visibility of winner text
            playAgain.Visibility = Visibility.Collapsed;    //changes visibility of play again button
            Hit.IsEnabled = true;                           //reenables gameplay buttons
            Check.IsEnabled = true;                         //renables gameplay buttons
            setupTable();                                   //calls setuptable function to reinitilise the next round
        }
    }
}
