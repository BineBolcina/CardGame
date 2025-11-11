using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Spomin
{
    public partial class Form1 : Form
    {
        Game game;
        AIPlayer ai;
        List<PictureBox> pictureBoxes;
        private bool isBusy = false;
        private bool isAITurn = false;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            game = new Game("Player");
            ai = new AIPlayer();

            pictureBoxes = new List<PictureBox>()
            {
                pictureBox1, pictureBox2, pictureBox3, pictureBox4,
                pictureBox5, pictureBox6, pictureBox7, pictureBox8,
                pictureBox9, pictureBox10, pictureBox11, pictureBox12,
                pictureBox13, pictureBox14, pictureBox15, pictureBox16
            };

            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                PictureBox pb = pictureBoxes[i];
                pb.Image = Image.FromFile("images/Cover.jpg");
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Click += pictureBox_Click;
                pb.Tag = game.Deck.Cards[i];
            }

            UpdateScoreLabels();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (isBusy || isAITurn) return;
            FlipCard(sender as PictureBox, isFromAI: false);
        }

        private void FlipCard(PictureBox pb, bool isFromAI)
        {
            if (pb == null) return;
            Card card = pb.Tag as Card;
            if (card == null || card.IsFlipped || card.IsMatched) return;

            pb.Image = Image.FromFile($"images/{card.Name}.jpg");
            card.IsFlipped = true;

            int index = pictureBoxes.IndexOf(pb);
            ai.RememberCard(card.Name, index);

            if (game.FirstSelected == null)
            {
                game.FirstSelected = card;
            }
            else
            {
                if (game.CheckMatch(game.FirstSelected, card))
                {
                    // uspešen par
                    if (isFromAI)
                        game.AiScore++;
                    else
                        game.Player1.Score++;

                    // odstranimo iz AI spomina
                    ai.RemoveMatchedPair(card.Name);

                    UpdateScoreLabels();

                    game.FirstSelected = null;

                    if (!isFromAI)
                        AIMove();

                    CheckGameOver();
                }
                else
                {
                    isBusy = true;
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 1000;
                    timer.Tick += (s, ev) =>
                    {
                        foreach (PictureBox p in pictureBoxes)
                        {
                            Card c = p.Tag as Card;
                            if (!c.IsMatched)
                            {
                                c.IsFlipped = false;
                                p.Image = Image.FromFile("images/Cover.jpg");
                            }
                        }
                        game.FirstSelected = null;
                        isBusy = false;

                        if (!isFromAI)
                            AIMove();

                        timer.Stop();
                    };
                    timer.Start();
                }
            }
        }

        private void AIMove()
        {
            if (game.Deck.Cards.All(c => c.IsMatched)) return;

            isAITurn = true;

            var move = ai.ChooseMove(game.Deck.Cards);
            int idx1 = move.Item1;
            int idx2 = move.Item2;

            System.Windows.Forms.Timer aiTimer = new System.Windows.Forms.Timer();
            aiTimer.Interval = 700;
            int step = 0;
            aiTimer.Tick += (s, ev) =>
            {
                if (step == 0)
                {
                    FlipCard(pictureBoxes[idx1], isFromAI: true);
                    step++;
                }
                else if (step == 1)
                {
                    FlipCard(pictureBoxes[idx2], isFromAI: true);
                    step++;
                }
                else
                {
                    isAITurn = false;
                    aiTimer.Stop();
                }
            };
            aiTimer.Start();
        }

        private void UpdateScoreLabels()
        {
            lblScorePlayer.Text = $"Igralec: {game.Player1.Score}";
            lblScoreAi.Text = $"AI: {game.AiScore}";
        }

        private void CheckGameOver()
        {
            if (game.Deck.Cards.All(c => c.IsMatched))
            {
                string winner;
                if (game.Player1.Score > game.AiScore)
                    winner = "Igralec zmaga!";
                else if (game.Player1.Score < game.AiScore)
                    winner = "AI zmaga!";
                else
                    winner = "Neodločeno!";

                MessageBox.Show($"Konec igre!\nIgralec: {game.Player1.Score}\nAI: {game.AiScore}\n{winner}", "Konec igre");
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            string message = "Memory igra s kartami:\n\n" +
                             "1. Klikni karto, da jo obrneš.\n" +
                             "2. Obrni dve karti, da najdeš ujemajoči par.\n" +
                             "3. Če se karti ujemata, ostaneta obrnjeni.\n" +
                             "4. Če se ne ujemata, se po 1 sekundi obrnejo nazaj.\n" +
                             "5. Igra se konča, ko so vse karte ujemajoče.\n" +
                             "6. Cilj je najti čim več parov.";

            MessageBox.Show(message, "Kako igrati", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
