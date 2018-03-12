using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BlackJackUwp
{
    public class Card
    {

        public string rank;
        public string suit;
        public string pictureLocation;

        public Card(string Rank, string Suit, string PictureLocation)
        {
            rank = Rank;
            suit = Suit;
            pictureLocation = PictureLocation;
        }

    }
}
