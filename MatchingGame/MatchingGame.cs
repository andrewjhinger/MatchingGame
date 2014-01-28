using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class MatchingGame : Form
    {
        private Button[,] _buttons;         // Two-dimensional array of Button controls
        private int _buttonRows = 4;        // Number of rows (and columns) of Button controls

        private Button _firstButton;        // First selected Button
        private bool _picking = false;      // Flag to indicate if we've selected first Button

        public MatchingGame()
        {
            // Create Button controls
            createButtons();

            // Now initialize components
            InitializeComponent();

            MatchingGame_Layout();

            // Set the card letters
            setCards();
        }

        private void createButtons()
        {
            // Dimension two-dimensional array
            _buttons = new Button[_buttonRows, _buttonRows];

            // Loop through each row and column of buttons
            for (int row = 0; row < _buttonRows; row++)
            {
                for (int col = 0; col < _buttonRows; col++)
                {
                    // Create the new Button, setting Button properties
                    Button button = new Button()
                    {
                        Text = "?",
                        Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold),
                        ForeColor = Color.Black
                    };
                    // Set the current array element to reference the new Button
                    _buttons[row, col] = button;

                    // Add an event handler for this Button (same one for all of the Button controls)
                    _buttons[row, col].Click += new EventHandler(buttonClickHandler);
                }
            }
        }

        private void buttonClickHandler(object sender, EventArgs e)
        {
            // Get the Button control we're responding to
            Button button = (Button)sender;

            // Make sure we aren't clicking on the same button both times
            // or we're selecting a button that's already part of a pair
            if (button.Text == "?")
            {
                if (!_picking)
                {
                    // First pick
                    _firstButton = button;
                    button.Text = button.Tag.ToString();
                    button.ForeColor = Color.Red;
                }
                else if (_firstButton.Tag.ToString() == button.Tag.ToString())
                {
                    // Matched
                    button.Text = button.Tag.ToString();
                    button.ForeColor = Color.Green;
                    _firstButton.ForeColor = Color.Green;
                }
                else
                {
                    // Second pick failed to match
                    button.Text = button.Tag.ToString();
                    button.ForeColor = Color.Red;

                    // We need to refresh the Button before sleeping or the Button
                    // text change will not be visible
                    button.Refresh();

                    // Pause for player to see the selected Button text
                    System.Threading.Thread.Sleep(1000);

                    // Change Button controls back to default text and colors
                    _firstButton.Text = "?";
                    button.Text = "?";
                    button.ForeColor = Color.Black;
                    _firstButton.ForeColor = Color.Black;
                }

                // Flip our picking flag
                _picking = !_picking;
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Exit the application
            this.Close();
        }

        private void MatchingGame_Layout()
        {
            // Get the height and width for each Button using the inside height and
            // width of the Panel control

            mainPanel.Controls.Clear();

            int buttonWidth = mainPanel.ClientRectangle.Width / _buttonRows;
            int buttonHeight = mainPanel.ClientRectangle.Height / _buttonRows;

            // Loop through and set the location and dimensions of each Button control
            for (int row = 0; row < _buttonRows; row++)
            {
                for (int col = 0; col < _buttonRows; col++)
                {
                    _buttons[row, col].SetBounds(buttonWidth * col, buttonHeight * row, buttonWidth, buttonHeight);
                }
            }

            // Add the Button controls to the Panel Controls collection
            if (mainPanel.Controls.Count == 0)
            {
                for (int row = 0; row < _buttonRows; row++)
                {
                    for (int col = 0; col < _buttonRows; col++)
                    {
                        mainPanel.Controls.Add(_buttons[row, col]);
                    }
                }
            }
        }

        private void setCards()
        {
            string _alphabet = "ABCDEFDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDGHABCDEFGH";
            Random random = new Random();

            // Loop through each Button
            for (int row = 0; row < _buttonRows; row++)
            {
                for (int col = 0; col < _buttonRows; col++)
                {
                    // Find a random number into card text string
                    int position = random.Next(_alphabet.Length);

                    // Extract the card text
                    string cardText = _alphabet[position].ToString();

                    // Store the card text in the Tag property of our Button control
                    ((Button)_buttons[row, col]).Tag = cardText;

                    // Use a temporary string to create a new card text string excluding
                    // the previously used card text
                    string holdString = "";
                    if (position > 0)
                        holdString = _alphabet.Substring(0, position);
                    if (position < _alphabet.Length)
                        holdString += _alphabet.Substring(position + 1, _alphabet.Length - position - 1);

                    // Replace previous card text string with the temporary card text string
                    _alphabet = holdString;
                }
            }
        }

        private void restartGame()
        {
            createButtons();
            MatchingGame_Layout();
            setCards();
            this.PerformLayout();
            this.Refresh();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            restartGame();
        }

        private void x2GridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _buttonRows = 2;
            restartGame();
        }

        private void x4GridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _buttonRows = 4;
            restartGame();
        }

        private void x6GridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _buttonRows = 6;
            restartGame();
        }
    }
}
