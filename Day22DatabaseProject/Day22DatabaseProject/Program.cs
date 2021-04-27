using System;
using System.Data.SqlClient;

namespace Day22DatabaseProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // sourcing the location of our database
            SqlConnection myConnection = new SqlConnection( ... );

            // opening the connection for our database, remember to close
            myConnection.Open();

            string repeat = "";
            while (repeat != "n")
            {
                Console.WriteLine("Would you like to (v)iew the menu, (a)dd an item, or (e)dit an item?");
                string userTask = Console.ReadLine().ToLower();

                if (userTask == "v")
                {
                    Console.WriteLine("Here's the full menu! \n");

                    // establish a command named "showData" in the same way as class/function to be used in the future
                    SqlCommand showData = new SqlCommand("SELECT * FROM [Table]", myConnection);
                    // call on the database to read and use that command
                    SqlDataReader reader = showData.ExecuteReader();
                    // while there is data to be read... loop goes through the data called much like "each"
                    while (reader.Read())
                    {
                        // looking at "reader" variable which now just refers to the data set we want pulled, then print positions
                        Console.Write(reader["Item"] + " ");
                        Console.Write(reader["ItemCategory"] + " ");
                        Console.WriteLine(reader["Price"]);
                    }
                    // remember to close any open connections or readers
                    reader.Close();
                }
                else if (userTask == "a")
                {
                    Console.WriteLine("What is the item you want to add?");
                    string item = Console.ReadLine();
                    Console.WriteLine("Is that an appetizer, entree, side, or drink?");
                    string itemCategory = Console.ReadLine();
                    Console.WriteLine("What's the price on that?");
                    string price = Console.ReadLine();

                    // another command, this time named "inputData", uses string interpolation with '$' and {} to add input to DB
                    SqlCommand inputData = new SqlCommand($"INSERT INTO [Table] (Item, ItemCategory, Price) VALUES ('{item}', '{itemCategory}', '{price}')", myConnection);
                    // execute the command again, then close
                    SqlDataReader writer = inputData.ExecuteReader();
                    writer.Close();
                }
                else if (userTask == "e")
                {
                    Console.WriteLine("What item do you want to change?");
                    string itemEdit = Console.ReadLine();

                    string keepEditing = "";
                    while (keepEditing != "n")
                    {
                        Console.WriteLine("Do you want to change the (n)ame, the (c)ourse, or the (p)rice?");
                        string editType = Console.ReadLine().ToLower();

                        if (editType == "n")
                        {
                            Console.WriteLine("Enter the new name.");
                            string newName = Console.ReadLine();
                            SqlCommand inputName = new SqlCommand($"UPDATE [Table] SET Item = '{newName}' WHERE Item = '{itemEdit}'", myConnection);
                            SqlDataReader updater = inputName.ExecuteReader();
                            itemEdit = newName;
                            updater.Close();
                        }
                        else if (editType == "c")
                        {
                            Console.WriteLine("Enter the new course type.");
                            string newCourse = Console.ReadLine();
                            SqlCommand inputCourse = new SqlCommand($"UPDATE [Table] SET ItemCategory = '{newCourse}' WHERE Item = '{itemEdit}'", myConnection);
                            SqlDataReader updater = inputCourse.ExecuteReader();
                            updater.Close();
                        }
                        else if (editType == "p")
                        {
                            Console.WriteLine("Enter the new price.");
                            string newPrice = Console.ReadLine();
                            SqlCommand inputPrice = new SqlCommand($"UPDATE [Table] SET Price = '{newPrice}' WHERE Item = '{itemEdit}'", myConnection);
                            SqlDataReader updater = inputPrice.ExecuteReader();
                            updater.Close();
                        }
                        Console.WriteLine($"Make another change to {itemEdit}? y/n");
                        keepEditing = Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Sorry, invalid option.");
                }
                Console.WriteLine("Would you like to perform another function? y/n");
                repeat = Console.ReadLine().ToLower();
            }
            Console.WriteLine("Goodbye!");

            // close the whole connection
            myConnection.Close();
        }
    }
}
