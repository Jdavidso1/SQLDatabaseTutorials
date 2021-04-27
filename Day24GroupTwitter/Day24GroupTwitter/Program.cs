using System;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace Day24GroupTwitter
{
    class Program
    {
        static void Main(string[] args)
        {

            SqlConnection connection = new SqlConnection( ... );

            connection.Open();

            //LOTS OF LENGTHY BLOCKS IN HERE, ESPECIALLY FOR SUPER MINOR THINGS - COULD DEFINITELY BENEFIT FROM SOME FUNCTIONS!

            //the user's choice for register, login, quit, tweet, view, etc.
            string userTask = "";

            //conditional for a loop within our main loop exclusive to logged-in users so they can tweet/view without logging in again
            bool loggedIN = false;

            //main loop, primarily for initial invalid entries and returning newly registered users to login/password screen
            while (userTask != "q")
            {
                Console.WriteLine("Would you like to (r)egister, (l)og in, or (q)uit?");
                userTask = Console.ReadLine();

                //never triggers anything else, breaks the loop, goes straight to final connection.close outside the main loop
                if (userTask == "q")
                {
                    Console.WriteLine("\nGoodbye!");
                }
                else if (userTask == "r")
                {
                    //collecting and storing new account info in temporary variables
                    Console.WriteLine("\nPlease create a user name.");
                    string newUser = Console.ReadLine();
                    Console.WriteLine("\nPlease Create a password.");
                    string newPassword = Console.ReadLine();

                    //storing info in the user database
                    SqlCommand addUser = new SqlCommand($"INSERT INTO [Users] (UserName, UserPass) VALUES ('{newUser}', '{newPassword}')", connection);
                    SqlDataReader writer = addUser.ExecuteReader();
                    writer.Close();

                    //thanks user by name, this one is easy, just prints the value in the string variable from writeline/readline
                    Console.WriteLine($"\nWelcome, {newUser}! You have successfully registered. Please return to the main menu and log in.\n");

                }
                else if (userTask == "l")
                {
                    //chose to have them log in by user ID # instead of having them correctly type actual username every time
                    //probably not very practical once our userbase grows
                    Console.WriteLine("\nPlease choose your user login ID Number.\n");

                    //retrieves all users from the user table...
                    SqlCommand viewUsers = new SqlCommand("SELECT * FROM [Users]", connection);
                    SqlDataReader viewer = viewUsers.ExecuteReader();

                    //...prints all the users alongside their user ID# and threw in a slash for distinct separation...
                    while (viewer.Read())
                    {
                        Console.Write(viewer["UserID"] + " / ");
                        Console.WriteLine(viewer["UserName"]);
                    }
                    viewer.Close();
                    Console.WriteLine("");

                    //...then has user select by number
                    int userChoice = Convert.ToInt32(Console.ReadLine());

                    //having user input their password and storing into a string variable to verify momentarily
                    Console.WriteLine("\nPlease enter your password.");
                    string userInput = Console.ReadLine();

                    //retrieving all the info associated with that user ID#...
                    SqlCommand displayName = new SqlCommand($"SELECT * FROM [Users] WHERE UserID = {userChoice}", connection);
                    SqlDataReader displayer = displayName.ExecuteReader();
                    while (displayer.Read())
                    {
                        //comparing user's password attempt against the stored value in the user table
                        if (userInput == (string)displayer["UserPass"])
                        {
                            //if it's good...
                            //welcome the user by name, this one was harder, needed to pull data form the user table as well
                            //probably a better way to do this than inside a reader loop, but didn't get that far
                            Console.WriteLine($"\nHello, {displayer["UserName"]}! You have successfully logged in.");
                            //change this value to true so we can stay in the new loop below to make tweeting and viewing feed convenient
                            loggedIN = true;
                        }
                        else
                        {
                            //kick them back to main menu, should make it so they can try password again
                            //implement a counter so they don't have unlimited tries
                            Console.WriteLine("\nYour password does not match the record for that username. Please try again.\n");
                        }
                    }
                    displayer.Close();

                    //aforementioned loop
                    while (loggedIN == true)
                    {
                        Console.WriteLine("\nDo you want do (t)weet, (v)iew your feed, or (l)og out?");
                        userTask = Console.ReadLine().ToLower();
                        if (userTask == "t")
                        {
                            //pretty straightforward, submitting a new tweet and the associated account via user ID#
                            Console.WriteLine("\nEnter your message. No apostrophes!");
                            string newTweet = Console.ReadLine();
                            SqlCommand addTweet = new SqlCommand($"INSERT INTO [Tweets] (UserID, Message) VALUES ({userChoice}, '{newTweet}')", connection);
                            SqlDataReader writer = addTweet.ExecuteReader();
                            writer.Close();
                            Console.WriteLine("\nMessage submitted!\n");
                        }
                        else if (userTask == "v")
                        {
                            //join command to display tweets by user, ordered by descending tweet number
                            //primary key made them auto-incremented, seems like as good an indicator of chronology as any
                            //assigned the method(?) to a string to put in the sql command since it's a little longer
                            string joinCommand = "SELECT * FROM Tweets JOIN Users ON Tweets.UserID = Users.UserID ORDER BY TweetID DESC";
                            SqlCommand viewFeed = new SqlCommand(joinCommand, connection);
                            SqlDataReader printer = viewFeed.ExecuteReader();
                            Console.WriteLine("");
                            while (printer.Read())
                            {
                                Console.Write(printer["UserName"] + " / ");
                                Console.Write(printer["TweetID"] + " / ");
                                Console.WriteLine(printer["Message"]);
                            }
                            printer.Close();
                        }
                        else if (userTask == "l")
                        {
                            Console.WriteLine("\nReturning to main menu.\n");
                            //breaking the logged in loop, but still in the main loop so returns to the main menu
                            loggedIN = false;
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid option, try again!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid option, try again!");
                }
            }
            connection.Close();
        }
    }
}
