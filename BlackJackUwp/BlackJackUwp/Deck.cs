using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackUwp
{
    class Deck
    {

        public Card[] deck;
        public int currentCard;
        public const int NUMBER_OF_CARDS = 52;
        private Random rand;

        public Deck()
        {
            string[] rank = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            string[] suits = { "H", "C", "D", "S" };
            deck = new Card[NUMBER_OF_CARDS];
            currentCard = 0;
            rand = new Random();
            for (int i = 0; i < deck.Length;i++)
            {
                deck[i] = new Card(rank[i % 11], suits[i / 13],"notImplemented");
            }
        }

        public void shuffle()
        {
            currentCard = 0;
            for(int first=0;first <deck.Length;first++)
            {
                int second = rand.Next(NUMBER_OF_CARDS);
                Card temp = deck[first];
                deck[first] = deck[second];
                deck[second] = temp;
            }
        }

        public void loadimages()
        {
            foreach(Card card in deck)
            {
                card.pictureLocation = "ms - appx:///CardImages/"+card.rank+card.suit+".png";
            }
        }
    

        public Card DealCard()
        {
            if(currentCard<deck.Length)
            {
                return deck[currentCard++];
            }
            else
            {
                return null;
            }
        }

    }
}
