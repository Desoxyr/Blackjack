using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack
{
    public enum CardNumber { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
    public enum CardType { Clubs, Hearts, Diamonds, Spades };

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class Card
    {
        public CardNumber Number { get; private set; }
        public CardType Type { get; private set; }
        public int Value;

        public Card(CardNumber cardNumber, CardType cardType, int _value)
        {
            Number = cardNumber;
            Type = cardType;
            Value = _value;
        }
    }

    public class Deck
    {   
        private readonly int[] cardValueArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };

        public List<Card> Cards { get; private set; }
        public Deck()
        {
            Cards = new List<Card>();

            foreach(CardType type in Enum.GetValues(typeof(CardType)))
            {
                int i = 0;
                foreach (CardNumber num in Enum.GetValues(typeof(CardNumber)))
                {
                    Cards.Add(new Card(num, type, cardValueArray[i]));
                    i++;
                }
            }
        }
    }

    public class Hand
    {
        public List<Card> Cards { get; private set; }
        public Boolean ContainsAce { get; set; }

        public String DisplayScore;

        public int valueOfHand;

        public Hand()
        {
            Cards = new List<Card>();
            ContainsAce = false;
        }

        public int GetValue()
        {
            const int maximumValueForAce = 11;
            const int extraValueOfAce = 10;

            LoopThroughCards();

            if (ContainsAce == true && valueOfHand <= maximumValueForAce)
            {
                valueOfHand += extraValueOfAce;
            }
            return valueOfHand;
        } 

        public string GetDisplayScore()
        {
            LoopThroughCards();        

            if (ContainsAce == true && valueOfHand <= 11)
            {   
                DisplayScore = valueOfHand.ToString() + "/" + (valueOfHand + 10).ToString();
                return DisplayScore;
            } else
            {
                DisplayScore = valueOfHand.ToString();
                return DisplayScore;
            }
        }

        public void LoopThroughCards()
        {
            valueOfHand = 0;
            for (int i = 0; i < Cards.Count; i++)
            {
                //Add score of card to score of hand
                valueOfHand += Cards[i].Value;

                //Keep track of whether hand contains an ace
                if (Cards[i].Value == 1)
                {
                    ContainsAce = true;
                }
            }
        }
    }
}
