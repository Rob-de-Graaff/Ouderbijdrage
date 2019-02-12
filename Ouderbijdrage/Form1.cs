using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ouderbijdrage
{
    public partial class Form1 : Form
    {
        private Subscription _newSubscription1,
            _newSubscription2;
        private List<Subscription> _subscriptions;
        private List<int> _agesList = new List<int>();
        private double _priceTotal = 50;
        //private double basicAmount = 50;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _subscriptions = new List<Subscription>();

            _newSubscription1 = new Subscription("kind < 10", 25);
            _newSubscription2 = new Subscription("kind >= 10", 37);

            _subscriptions.Add(_newSubscription1);
            _subscriptions.Add(_newSubscription2);

            // Fills labels
            int counterSubscriptions = 0;
            foreach (Label labelSubscription in panelSubscriptions.Controls)
            {
                //if (control.GetType() == typeof(Label))
                labelSubscription.Text = _subscriptions[counterSubscriptions].Name;
                counterSubscriptions++;
            }

            // Fills labels
            int counterPrices = 0;
            foreach (Label labelPrice in panelPrices.Controls)
            {
                labelPrice.Text = _subscriptions[counterPrices].Price.ToString();
                counterPrices++;
            }

            // Fills numericupdowns
            int counterMaximum = 3;
            foreach (NumericUpDown control in panelAmount.Controls)
            {
                control.Maximum = counterMaximum;
                counterMaximum--;
            }

            // Fills numericupdowns
            foreach (NumericUpDown control in panelYear.Controls)
            {
                control.Maximum = DateTime.Today.Year;
                control.Minimum = DateTime.Today.Year - 100;
                control.Value = DateTime.Today.Year;
            }

            foreach (NumericUpDown control in panelMonth.Controls)
            {
                control.Maximum = DateTime.Today.Month;
                control.Value = DateTime.Today.Month;
            }

            foreach (NumericUpDown control in panelDay.Controls)
            {
                control.Maximum = DateTime.Today.Day;
                control.Value = DateTime.Today.Day;
            }

            labelPriceTotal.Text = $@"€ {Math.Round(_priceTotal, 2):0.00},-";
            labelTicketsTotal.Text = $@"aantal: {numericUpDownAmount1.Value + numericUpDownAmount2.Value}";
        }

        private void ButtonCalculate_Click(object sender, EventArgs e)
        {
            int searchHits10Min;
            int searchHits10Plus;

            if (CheckSelection())
            {
                if (ValidateDate())
                {
                    if (numericUpDownAmount1.Value > 0)
                    {
                        searchHits10Min = (int)numericUpDownAmount1.Value;
                        
                        IEnumerable<int> ageSelection10Min = 
                            from intergers in _agesList
                            where intergers < 10
                            select intergers;

                        if (ageSelection10Min.Count() == searchHits10Min && ageSelection10Min.Count() != 0)
                        {
                            _priceTotal += _newSubscription1.Price * (double)numericUpDownAmount1.Value;
                        }
                    }

                    if (numericUpDownAmount2.Value > 0)
                    {
                        searchHits10Plus = (int)numericUpDownAmount1.Value;
                        
                        IEnumerable<int> ageSelection10Plus = 
                            from intergers in _agesList
                            where intergers >= 10
                            select intergers;

                        if (ageSelection10Plus.Count() == searchHits10Plus && ageSelection10Plus.Count() != 0)
                        {
                            _priceTotal += _newSubscription2.Price * (double)numericUpDownAmount2.Value;
                        }
                    }

                    //if (priceTotal < 50)
                    //{
                    //    priceTotal = 50;
                    //}

                    // Checks if family has 1 child, if so gives discount.
                    if (numericUpDownAmount1.Value == 1 && numericUpDownAmount2.Value == 0 ||
                        numericUpDownAmount1.Value == 0 && numericUpDownAmount2.Value == 1)
                    {
                        _priceTotal = _priceTotal * 0.75;
                    }

                    labelPriceTotal.Text = $@"€ {Math.Round(_priceTotal, 2):0.00},-";
                    labelTicketsTotal.Text = $@"aantal: {numericUpDownAmount1.Value + numericUpDownAmount2.Value}";
                }
                else
                {
                    MessageBox.Show($@"The selected day value is incorrect");
                }
            }
            else
            {
                MessageBox.Show($@"Max 3 kids of < 10, max 2 kids of >= 10");
            }
            
        }

        private bool CheckSelection()
        {
            int kidsCounter = 0;
            // Checks which checkboxes are checked and counts them
            // Use .oftype when there are multiple control types in a panel
            foreach (CheckBox control in panelLayout.Controls.OfType<CheckBox>())
            {
                if (control.Checked)
                {
                    kidsCounter++;
                }
            }

            if ((numericUpDownAmount1.Value + numericUpDownAmount2.Value) == kidsCounter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateDate()
        {
            // Checks day input
            if (checkBox1.Checked && numericUpDownAmount1.Value == 3 || numericUpDownAmount1.Value == 2 ||
                numericUpDownAmount1.Value == 1 || numericUpDownAmount2.Value == 2 || numericUpDownAmount2.Value == 1)
            {
                int checkDay = DateTime.DaysInMonth((int)numericUpDownYear1.Value, (int)numericUpDownMonth1.Value);
                if ((int)numericUpDownDay1.Value <= checkDay)
                {
                    int age1;
                    DateTime dateOfBirth1 = new DateTime((int) numericUpDownYear1.Value,
                        (int) numericUpDownMonth1.Value, (int) numericUpDownDay1.Value);
                    age1 = CalculateAge(dateOfBirth1);
                    _agesList.Add(age1);
                }
                else
                {
                    MessageBox.Show($@"The selected day value is incorrect");
                    return false;
                }
            }
            else
            {
                MessageBox.Show($@"Max 3 kids < 10, max 2 kids >= 10");
                return false;
            }

            if (checkBox2.Checked && numericUpDownAmount1.Value == 3 || numericUpDownAmount1.Value == 2 ||
                numericUpDownAmount2.Value == 2)
            {
                int checkDay = DateTime.DaysInMonth((int)numericUpDownYear2.Value, (int)numericUpDownMonth2.Value);
                if ((int)numericUpDownDay2.Value <= checkDay)
                {
                    int age2;
                    DateTime dateOfBirth2 = new DateTime((int) numericUpDownYear2.Value,
                        (int) numericUpDownMonth2.Value, (int) numericUpDownDay2.Value);
                    age2 = CalculateAge(dateOfBirth2);
                    _agesList.Add(age2);
                }
                else
                {
                    MessageBox.Show($@"The selected day value is incorrect");
                    return false;
                }
            }

            if (checkBox3.Checked  && numericUpDownAmount1.Value == 3)
            {
                int checkDay = DateTime.DaysInMonth((int)numericUpDownYear3.Value, (int)numericUpDownMonth3.Value);
                if ((int)numericUpDownDay3.Value <= checkDay)
                {
                    int age3;
                    DateTime dateOfBirth3 = new DateTime((int) numericUpDownYear3.Value,
                        month: (int) numericUpDownMonth3.Value, day: (int) numericUpDownDay3.Value);
                    age3 = CalculateAge(dateOfBirth3);
                    _agesList.Add(age3);
                }
                else
                {
                    MessageBox.Show($@"The selected day value is incorrect");
                    return false;
                }
            }

            if (checkBox4.Checked && numericUpDownAmount2.Value == 2 || numericUpDownAmount2.Value == 1)
            {
                int checkDay = DateTime.DaysInMonth((int)numericUpDownYear4.Value, (int)numericUpDownMonth4.Value);
                if ((int)numericUpDownDay4.Value <= checkDay)
                {
                    int age4;
                    DateTime dateOfBirth4 = new DateTime((int) numericUpDownYear4.Value,
                        (int) numericUpDownMonth4.Value, (int) numericUpDownDay4.Value);
                    age4 = CalculateAge(dateOfBirth4);
                    _agesList.Add(age4);
                }
                else
                {
                    MessageBox.Show($@"The selected day value is incorrect");
                    return false;
                }
            }

            if (checkBox5.Checked  && numericUpDownAmount2.Value == 2)
            {
                int checkDay = DateTime.DaysInMonth((int)numericUpDownYear5.Value, (int)numericUpDownMonth5.Value);
                if ((int)numericUpDownDay5.Value <= checkDay)
                {
                    int age5;
                    DateTime dateOfBirth5 = new DateTime((int) numericUpDownYear5.Value,
                        (int) numericUpDownMonth5.Value, (int) numericUpDownDay5.Value);
                    age5 = CalculateAge(dateOfBirth5);
                    _agesList.Add(age5);
                }
                else
                {
                    MessageBox.Show($@"The selected day value is incorrect");
                    return false;
                }
            }

            return true;
        }

        private static int CalculateAge(DateTime dateOfBirth)  
        {  
            int age = DateTime.Now.Year - dateOfBirth.Year;  
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)  
                age = age - 1;  
  
            return age;  
        }
    }
}
