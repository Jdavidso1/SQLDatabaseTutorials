using System;
using System.Data.SqlClient;

namespace Day23MusicLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection myConnection = new SqlConnection( ... );

            string repeat = "";
            while (repeat != "n")
            {
                myConnection.Open();

                Console.WriteLine("Would you like to (v)iew albums, add a new (a)lbum, add a (s)ong to an album, or (q)uit?");
                string userTask = Console.ReadLine().ToLower();

                if (userTask == "v")
                {
                    SqlCommand viewAlbums = new SqlCommand("SELECT * FROM [Albums]", myConnection);
                    SqlDataReader reader = viewAlbums.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.Write(reader["AlbumArtist"] + " / ");
                        Console.WriteLine(reader["AlbumTitle"]);
                    }
                    reader.Close();
                }
                else if (userTask == "a")
                {
                    Console.WriteLine("What album would you like to add?");
                    string newAlbumTitle = Console.ReadLine();
                    Console.WriteLine("Enter the artist as well.");
                    string newAlbumArtist = Console.ReadLine();

                    SqlCommand addAlbums = new SqlCommand($"INSERT INTO [Albums] (AlbumTitle, AlbumArtist) VALUES ('{newAlbumTitle}', '{newAlbumArtist}')", myConnection);
                    SqlDataReader writer = addAlbums.ExecuteReader();
                    writer.Close();
                }
                else if (userTask == "s")
                {
                    Console.WriteLine("Which album do you want to add to? Enter the corresponding number:");

                    SqlCommand viewAlbums = new SqlCommand("SELECT * FROM [Albums]", myConnection);
                    SqlDataReader reader = viewAlbums.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.Write(reader["AlbumID"] + " / ");
                        Console.Write(reader["AlbumArtist"] + " / ");
                        Console.WriteLine(reader["AlbumTitle"]);
                    }
                    reader.Close();

                    int userChoice = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("What is the song title you want to add?");
                    string newSongTitle = Console.ReadLine();

                    SqlCommand addSongs = new SqlCommand($"INSERT INTO [Songs] (AlbumID, SongTitle) VALUES ({userChoice},'{newSongTitle}')", myConnection);
                    SqlDataReader writer = addSongs.ExecuteReader();
                    writer.Close();
                }
                myConnection.Close();
            }
        }
    }
}
