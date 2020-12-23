using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack
{
    public partial class Form1 : Form
    {

        private Hand _playerHand;
        private Hand _dealerHand;

        private List<PictureBox> pictureBoxList = new List<PictureBox>();

        private const int cardPlacementX = 30;
        private const int cardPlacementDealerY = 50;
        private const int cardPlacementPlayerY = 313;
        private const int cardOffset = 50;

        private const int maximumValueOfHand = 21;

        private Deck _myDeck;

        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            NewGame();
            pictureBoxList = new List<PictureBox>();
        }

        public void NewGame()
        {
            //Remove Picture Boxes and reset list
            for (int i = 0; i < pictureBoxList.Count; i++)
            {
                Controls.Remove(pictureBoxList[i]);
            }

            //Create new deck
            _myDeck = new Deck();

            //Reset hands
            _playerHand = new Hand();
            _dealerHand = new Hand();

            //Draw cards out of this deck
            DrawRandomCard(_playerHand.Cards, cardPlacementX + cardOffset * _playerHand.Cards.Count, cardPlacementPlayerY);
            DrawRandomCard(_dealerHand.Cards, cardPlacementX + cardOffset * _dealerHand.Cards.Count, cardPlacementDealerY);
            DrawRandomCard(_playerHand.Cards, cardPlacementX + cardOffset * _playerHand.Cards.Count, cardPlacementPlayerY);          
            CreatePictureBox("back", "deck", cardPlacementX + cardOffset * _dealerHand.Cards.Count, cardPlacementDealerY);

            //Calculate and Display Scores
            lblPlayerScore.Text = "Player: " + _playerHand.GetDisplayScore();
            lblDealerScore.Text = "Dealer: " + _dealerHand.GetDisplayScore();

            //Enable Buttons
            btnHit.Enabled = true;
            btnStand.Enabled = true;

            //Check for immediate blackjack
            CheckForBlackJack(_playerHand);
        }

        //Creates PictureBox for playing cards
        public void CreatePictureBox(string number, string type, int LocationX, int LocationY)
        {
            //Picturebox Size
            const int pictureBoxSizeX = 118;
            const int pictureBoxSizeY = 194;

            //Create new picturebox
            PictureBox pictureBox = new PictureBox
            {
                //Set the new picturebox its properties
                Name = $"{number} of {type}",
                Location = new Point(LocationX, LocationY),
                Size = new Size(pictureBoxSizeX, pictureBoxSizeY),
                SizeMode = PictureBoxSizeMode.StretchImage,
                ImageLocation = $@"Deck\{number}_of_{type}.png",
                BorderStyle = BorderStyle.FixedSingle
            };

            //Add the new picturebox to form and list           
            Controls.Add(pictureBox);
            pictureBoxList.Add(pictureBox);

            //Bring it to the front so it properly overlaps past cards
            pictureBox.BringToFront();

        }

        //Add Random Card to one of the hands
        public void DrawRandomCard(List<Card> hand, int pictureBoxX, int pictureBoxY)
        {
            Card myCard = _myDeck.Cards[rnd.Next(0, _myDeck.Cards.Count - 1)];
            CreatePictureBox(Convert.ToString(myCard.Number), Convert.ToString(myCard.Type), pictureBoxX, pictureBoxY);

            //Adds card to player's or dealer's hand
            hand.Add(myCard);

            //Removes card from deck
            _myDeck.Cards.Remove(myCard);
        }

        //Check for a blackjack
        private void CheckForBlackJack(Hand hand)
        {
            const int valueOfPotentialBlackjack = 10;

            //Check if the player has a score of 21
            if (hand.ContainsAce == true && _playerHand.GetValue() == maximumValueOfHand)
            {

                //Check if the dealer is holding either an ace or a card worth 10
                if (_dealerHand.ContainsAce || _dealerHand.GetValue() == valueOfPotentialBlackjack)
                {
                    //ADD: Give player option for Even Money (1.5x or 1x)

                    DrawRandomCard(_dealerHand.Cards, cardPlacementX + cardOffset * _dealerHand.Cards.Count, cardPlacementDealerY);
                    lblDealerScore.Text = "Dealer: " + _dealerHand.GetDisplayScore();
                    if (_dealerHand.GetValue() == maximumValueOfHand)
                    {
                        MessageBox.Show("Draw :-/");
                        NewGame();
                        return;
                    }
                }
                MessageBox.Show("Blackjack! You win :D");
                NewGame();
            }
        }

        private void BtnHit_Click(object sender, EventArgs e)
        {
            List<Card> Cards = _playerHand.Cards;
            DrawRandomCard(Cards, cardPlacementX + cardOffset * Cards.Count, cardPlacementPlayerY);
            lblPlayerScore.Text = "Player: " + _playerHand.GetDisplayScore();

            if (_playerHand.GetValue() > maximumValueOfHand)
            {
                MessageBox.Show("Bust :(");
                NewGame();
            }
        }

        private void BtnStand_Click(object sender, EventArgs e)
        {
            const int minimumValueOfDealer = 16;

            btnHit.Enabled = false;
            btnStand.Enabled = false;

            int valueOfDealerHand = _dealerHand.GetValue();

            //Check is if the value of the dealer's hand is 16 or lower
            while (valueOfDealerHand <= minimumValueOfDealer)
            {

                //Draw a card, update the display and update the value of the dealers hand
                DrawRandomCard(_dealerHand.Cards, cardPlacementX + cardOffset * _dealerHand.Cards.Count, cardPlacementDealerY);
                lblDealerScore.Text = "Dealer: " + _dealerHand.GetDisplayScore(); 
                valueOfDealerHand = _dealerHand.GetValue();
            }

            int valueOfPlayerHand = _playerHand.GetValue();

            if (valueOfDealerHand > maximumValueOfHand || valueOfDealerHand < valueOfPlayerHand)
            {
                MessageBox.Show("You win :)");
            }
            else if (valueOfDealerHand == valueOfPlayerHand)
            {
                MessageBox.Show("Stand off :-/");
            }
            else if (valueOfDealerHand > valueOfPlayerHand)
            {
                MessageBox.Show("You lose :(");
            }         
            NewGame();
        }
    }
}