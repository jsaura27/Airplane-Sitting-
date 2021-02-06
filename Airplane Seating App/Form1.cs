using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Airplane_Seating_App
{
    public partial class Form1 : Form
    {

        //Variables
        public int passengers = 0;
        public bool firstClass;
        public bool secondClass;
        public bool noClass = false;
        public bool warning = false;
        Person[] people = new Person[3];
        Dictionary<String, String> list;

        public Form1()
        {
            InitializeComponent();

            //Initialization of the columns that will show the seats 
            listView1.View = View.Details;
            listView1.Columns.Add("Seat", 100);
            listView1.Columns.Add("Name", 100);
        }


        //Passanges Labels
        private void NameLabel_Click(object sender, EventArgs e)
        {
            
        }

        private void lastNameLabel_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        //Textboxes
        //Name1 Input
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            
        }
        //LastName1 Input
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        //Name2 Input
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        //LastName2 Input
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        //Name3 Input
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }
        //LastName 3 Input
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //ComboBox where I fill the 3 booleans that later on will show which class are each passenger
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text == "First Class")
            {
                firstClass = true;
                secondClass = false;
                noClass = false;
            }else if(comboBox1.Text == "Second Class")
            {
                secondClass = true;
                firstClass = false;
                noClass = false;
            }
            else
            {
                noClass = true;
            }
        }


        //Button that will execute the adding process
        private void addButton_Click(object sender, EventArgs e)
        {

            //In this 3 if operations I am adding to an array the passengers filled by the clients
            textBoxReader(textBox1.Text, textBox2.Text);
            textBoxReader(textBox4.Text, textBox3.Text);
            textBoxReader(textBox6.Text, textBox5.Text);
            



            //Here I take care of scenarios I don't want happening 
            if (firstClass && passengers>2)
            {
                MessageBox.Show("You can only add 2 First Class Passangers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }else if(noClass)
            {
                MessageBox.Show("You have to choose a class first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }else if (warning)
            {
                MessageBox.Show("You have to fill both name and lastname.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            //If none of the scenarios arise the program proceeds 
            else
            {

                Program.checker(people, passengers, firstClass);
                passengers = 0;
                
            }

        }
        void textBoxReader(String textbox1, String textbox2)
        {
            if (!string.IsNullOrEmpty(textbox1) && !string.IsNullOrEmpty(textbox2))
            {
                var person = new Person();
                person.name = textbox1;
                person.lastName = textbox2;
                person.firstClass = firstClass;

                people[passengers] = person;
                passengers++;


            }else if (string.IsNullOrEmpty(textbox1) && !string.IsNullOrEmpty(textbox2)){
                warning = true;
            }
            else if (!string.IsNullOrEmpty(textbox1) && string.IsNullOrEmpty(textbox2))
            {
                warning = true;
            }
            else
            {

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        //Button that shows the list of seats taken or not 
        private void buttonShowSeats_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            //Here depending on the boolean, it will show the list you require
            if (firstClass)
            {
                //Serialization of the passengers data
                String retriveFirstClassString = Properties.Settings.Default.firstClassList;
                list = JsonConvert.DeserializeObject<Dictionary<string, string>>(retriveFirstClassString);
                

                foreach (KeyValuePair<string, string> kvp in list)
                {

                    string[] arr = new string[2];
                    ListViewItem itm;
                    //add items to ListView
                    arr[0] = kvp.Key;
                    arr[1] = kvp.Value;
                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);


                }
            }
            else if (secondClass)
            {
                String retriveSecondClassString = Properties.Settings.Default.secondClassList;
                list = JsonConvert.DeserializeObject<Dictionary<string, string>>(retriveSecondClassString);
                
                foreach (KeyValuePair<string, string> kvp in list)
                {

                    string[] arr = new string[2];
                    ListViewItem itm;
                    //add items to ListView
                    arr[0] = kvp.Key;
                    arr[1] = kvp.Value;
                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);


                }
            }
            else
            {
                MessageBox.Show("You have to select a class to show first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }

            
        }

        //This button would do the same than the previous one but on an alphabetical order
        //But I decided that it will only show the seats taken not all of them.
        private void buttonNameOrder_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            List<Person> passList = new List<Person>();

            if (firstClass)
            {
                String retriveFirstClassString = Properties.Settings.Default.firstClassList;
                list = JsonConvert.DeserializeObject<Dictionary<string, string>>(retriveFirstClassString);

                //This foreach goes through all the dictionary and adds all the passangers in an rray 
                foreach (KeyValuePair<string, string> kvp in list)
                {

                    Person passanger = new Person();
                    if (kvp.Value == "Empty")
                    {
                        passanger.lastName = kvp.Value;
                        passanger.name = "";
                        passanger.seat = kvp.Key;
                        passList.Add(passanger);
                    }
                    else
                    {
                        int position = kvp.Value.IndexOf(",");
                        String aux = kvp.Value;
                        Console.WriteLine(aux);
                        passanger.lastName = kvp.Value.Substring(0, position);
                        passanger.name = kvp.Value.Substring(position + 1);
                        passanger.seat = kvp.Key;
                        passList.Add(passanger);
                    }


                }
                //Here sorts alphabetically all the passangers and then we show them on the listview
                List<Person> SortedList = passList.OrderBy(o => o.lastName).ToList();

                for (int i = 0; i < SortedList.Count; i++)
                {

                    string[] arr = new string[2];
                    ListViewItem itm;
                    //add items to ListView
                    arr[0] = SortedList[i].seat;
                    if (SortedList[i].lastName == "Empty")
                    {
                        arr[1] = "";

                    }
                    else
                    {
                        arr[1] = SortedList[i].lastName + "," + SortedList[i].name;
                        itm = new ListViewItem(arr);
                        listView1.Items.Add(itm);
                    }



                }
            }
            else if (secondClass)
            {
                String retriveSecondClassString = Properties.Settings.Default.secondClassList;
                list = JsonConvert.DeserializeObject<Dictionary<string, string>>(retriveSecondClassString);

                foreach (KeyValuePair<string, string> kvp in list)
                {

                    Person passanger = new Person();
                    if (kvp.Value == "Empty")
                    {
                        passanger.lastName = kvp.Value;
                        passanger.name = "";
                        passanger.seat = kvp.Key;
                        passList.Add(passanger);
                    }
                    else
                    {
                        int position = kvp.Value.IndexOf(",");
                        String aux = kvp.Value;
                        Console.WriteLine(aux);
                        passanger.lastName = kvp.Value.Substring(0, position);
                        passanger.name = kvp.Value.Substring(position + 1);
                        passanger.seat = kvp.Key;
                        passList.Add(passanger);
                    }


                }
                List<Person> SortedList = passList.OrderBy(o => o.lastName).ToList();

                for (int i = 0; i < SortedList.Count; i++)
                {

                    string[] arr = new string[2];
                    ListViewItem itm;
                    //add items to ListView
                    arr[0] = SortedList[i].seat;
                    if (SortedList[i].lastName == "Empty")
                    {
                        arr[1] = "";

                    }
                    else
                    {
                        arr[1] = SortedList[i].lastName + "," + SortedList[i].name;
                        itm = new ListViewItem(arr);
                        listView1.Items.Add(itm);
                    }



                }
            }
            else
            {
                MessageBox.Show("You have to select a class to show first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }


            
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
        }
    }
}
