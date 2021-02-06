using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Airplane_Seating_App
{
    static class Program
    {
        //variables rows and columns easily changable and won't afect the algorithms
        public static int rowsFirst = 5, columnsFirst = 4;
        public static int rowsSecond = 30, columnsSecond = 6;

        //serialization of the data
        static String retriveFirstClassString = Properties.Settings.Default.firstClassList;
        static String retriveSecondClassString = Properties.Settings.Default.secondClassList;

        //filling of the dictionaries we use during the project that will depend
        //on the function FillDictionary
        public static Dictionary<String, String> firstClassList = FillDictionary(retriveFirstClassString, rowsFirst, columnsFirst);
        public static Dictionary<String, String> secondClassList = FillDictionary(retriveSecondClassString, rowsSecond, columnsSecond);


        //Checks if there is availability for the amount of seats required 
        //and assign them, it depends on the number of passengers and differ between 
        //first class and business class. 
        //It will show a message if it doesn't find available seats.
        public static void checker(Person[] people, int numberPassengers, bool firstClass)
        {
            Dictionary<String, String> list;

            //The first class is determined by a max amount of 2 passengers
            if (firstClass)
            {
                list = firstClassList;
                for (int index = 0; index < list.Count; index++)
                {
                    var item = list.ElementAt(index);
                    var itemKey = item.Key;
                    var itemValue = item.Value;
                    try
                    {
                        var nextItem = list.ElementAt(index + 1);
                        if (itemValue == "Empty" && numberPassengers == 1)
                        {
                            list[itemKey] = people[0].lastName + ", " + people[0].name;
                            firstClassList = list;
                            //Here after the seats are assigned we serialize the dictionary
                            string fcSettingsSafe = JsonConvert.SerializeObject(firstClassList);
                            Properties.Settings.Default.firstClassList = fcSettingsSafe;
                            Properties.Settings.Default.Save();
                            break;

                        }else if(itemValue == "Empty" && nextItem.Value == "Empty")
                        {
                            list[itemKey] = people[0].lastName + ", " + people[0].name;
                            list[nextItem.Key] = people[1].lastName + ", " + people[1].name;
                            firstClassList = list;
                            string fcSettingsSafe = JsonConvert.SerializeObject(firstClassList);
                            Properties.Settings.Default.firstClassList = fcSettingsSafe;
                            Properties.Settings.Default.Save();
                            break;

                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("There are no available seats");
                    }



                }

            }
            //It does the same for business class but for a max of 3 passengers
            else
            {
                //needs to change to something more efficient
                list = secondClassList;
                for (int index = 0; index < list.Count; index++)
                {
                    var item = list.ElementAt(index);
                    var itemKey = item.Key;
                    var itemValue = item.Value;
                    try
                    {
                        var nextItem = list.ElementAt(index + 1);
                        var secondItem = list.ElementAt(index + 2);
                        if (itemValue == "Empty" && numberPassengers == 1)
                        {
                            list[itemKey] = people[0].lastName + ", " + people[0].name;
                           
                            secondClassList = list;
                            string scSettingsSafe = JsonConvert.SerializeObject(secondClassList);
                            Properties.Settings.Default.secondClassList = scSettingsSafe;
                            Properties.Settings.Default.Save();
                            break;

                        }else if (itemValue == "Empty" && nextItem.Value == "Empty" && numberPassengers == 2)
                        {
                            list[itemKey] = people[0].lastName + ", " + people[0].name;
                            list[nextItem.Key] = people[1].lastName + ", " + people[1].name;
                           
                            secondClassList = list;
                            string scSettingsSafe = JsonConvert.SerializeObject(secondClassList);
                            Properties.Settings.Default.secondClassList = scSettingsSafe;
                            Properties.Settings.Default.Save();
                            break;

                        }else if (itemValue == "Empty" && nextItem.Value == "Empty" && secondItem.Value == "Empty")
                        {
                            list[itemKey] = people[0].lastName + ", " + people[0].name;
                            list[nextItem.Key] = people[1].lastName + ", " + people[1].name;
                            list[secondItem.Key] = people[2].lastName + ", " + people[2].name;

                            secondClassList = list;
                            string scSettingsSafe = JsonConvert.SerializeObject(secondClassList);
                            Properties.Settings.Default.secondClassList = scSettingsSafe;
                            Properties.Settings.Default.Save();
                            break;

                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("There are no available seats");
                    }



                }
            }

           // return list;
        }


        

        //I create a filler for the dictionaries i will use to have all the seats ready
        public static Dictionary<String,String> FillDictionary(String list, int rows, int columns)
        {
            var seats = new Dictionary<String, String>()
            { };
            //This will filled the dictionaries if there is previous data 
            if (!String.IsNullOrEmpty(list))
            {
                seats = JsonConvert.DeserializeObject<Dictionary<string, string>>(list);

            }
            //if not it creates a new dictionary based on the columns and rows 
            else
            {
                
                char letter = 'a';
                for (int i = 0; i < rows; i++)
                {
                    char letterLoop = letter;

                    for (int j = 0; j < columns; j++)
                    {
                        String aux = i.ToString() + letterLoop;

                        seats.Add(aux, "Empty");
                        letterLoop = getNextChar(letterLoop);
                    }
                }
            }


            

             return seats;
        }

        //I create a function that allows me to turn the column number into a character
        private static char getNextChar(char c)
        {
            // convert char to ascii
            int ascii = (int)c;
            // get the next ascii
            int nextAscii = ascii + 1;
            // convert ascii to char
            char nextChar = (char)nextAscii;
            return nextChar;
        }

        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


        }
    }

    //Class for the passenger
    public class Person
    {
        public String name { get; set; }
        public String lastName { get; set; }
        public bool firstClass { get; set; }
        public String seat { get; set; }

    }
}
