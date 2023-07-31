using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DailyDairyManagementSystem
{
    internal class Program
    {
        static string connectionString = @"Data Source=DESKTOP-QHL4H1I;Initial Catalog=DailyDairyManagementSystem;Integrated Security=True";

        static void Main(string[] args)
        {
            bool isExit = false;
            while (!isExit)
            {
                string heading = "\nDaily Dairy Management Application";
                char[] headingChar = heading.ToCharArray();
                Console.WriteLine(headingChar);
                for (int i = 1; i < headingChar.Length; i++)
                {
                    if (headingChar[i] == ' ')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("=");
                    }
                }

                Console.WriteLine();             
                Console.WriteLine("Please choose a choice \n 1.Create dairy \n 2.View dairy  \n 3.Update dairy \n 4.Delete dairy \n 5.Search \n 6.Book Mark \n 7.Filter by date \n 8.Exit");
                Console.Write("\nEnter a choice: ");
                string userChoice = Console.ReadLine();
                switch (userChoice)
                {
                    case "1":
                        validationInCreation();
                        break;
                    case "2":
                        ViewDairy();
                        break;
                    case "3":
                        validationInUpdation();                  
                        break;
                    case "4":
                        DeleteDairy();
                        break;
                    case "5":
                        Search();
                        break;
                    case "6":
                        BookMark();
                        break;
                    case "7":
                        FilterByDate();
                        break;
                    case "8":
                        Console.WriteLine("Ready to exit!");
                        isExit = true;
                        break;
                    default:
                        Console.WriteLine("\nPlease enter a valid input.");
                        break;

                }
               

            }
        }


        #region Validation in creation
        static void validationInCreation()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string heading = "\nCreate Your Daily Dairy Entry with Ease!";
                char[] headingChar = heading.ToCharArray();
                Console.WriteLine(headingChar);
                for (int i = 1; i < headingChar.Length; i++)
                {
                    if (headingChar[i] == ' ')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("=");
                    }
                }
                Console.WriteLine("\n");
                Console.Write("Title: ");
                string title = Console.ReadLine();
                if (Regex.IsMatch(title, @"^[A-Za-z\s]*$"))
                {
                    if (title == "")
                    {
                        title = DateTime.Now.ToString();
                    }
                    Console.Write("Description: ");
                    string description = Console.ReadLine();
                    if (Regex.IsMatch(description, @"^[A-Za-z\s]*$"))
                    {
                        Console.Write("Book Mark (yes/no): ");
                        string bookMark = Console.ReadLine();
                        if (Regex.IsMatch(bookMark, @"^[a-zA-Z]+$"))
                        {
                            CreateDairy(title, description, bookMark);
                        }
                        else
                        {
                            Console.WriteLine("Book mark value is not in correct format! Please try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Description value is not in correct format! Please try again.");
                    }

                }
                else
                {
                    Console.WriteLine("Title value is not in correct format! Please try again.");
                }
            }
        }
        #endregion

        #region Dairy creation
        static void CreateDairy(string title, string description, string bookMark)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DairyDetailsCRUD", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", "Add");
                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@DateTommarow", DateTime.Today.AddDays(1));
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@BookMark", bookMark);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine("\nDairy created sucessfully...");

                    SqlParameter reminderParameter = new SqlParameter("@Reminder", SqlDbType.VarChar, 100);
                    reminderParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(reminderParameter);
                    command.ExecuteNonQuery();
                    string reminderValue = command.Parameters["@Reminder"].Value.ToString();
                    Console.WriteLine("\nMessage: Tommarow is " + reminderValue+".");
                }
               




            }

        }
        #endregion

        #region Dairy view
        static void ViewDairy()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string heading = "\nYour Dairy";
                char[] headingChar = heading.ToCharArray();
                Console.WriteLine(headingChar);
                for (int i = 1; i < headingChar.Length; i++)
                {
                    if (headingChar[i] == ' ')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("=");
                    }
                }
                Console.WriteLine("\n");
                using (SqlCommand command = new SqlCommand("ViewDairy", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Console.WriteLine("\nId: " + dataReader.GetValue(0).ToString());
                            Console.WriteLine("Date: " + dataReader.GetValue(1).ToString());
                            Console.WriteLine("Title: " + dataReader.GetValue(2).ToString());
                            Console.WriteLine("Description: " + dataReader.GetValue(3).ToString());
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
        #endregion

        #region Validation in updation
        static void validationInUpdation()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string heading = "\nYour Dairy";
                char[] headingChar = heading.ToCharArray();
                Console.WriteLine(headingChar);
                for (int i = 1; i < headingChar.Length; i++)
                {
                    if (headingChar[i] == ' ')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("=");
                    }
                }
                Console.WriteLine("\n");
                using (SqlCommand command = new SqlCommand("ViewDairy", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Console.WriteLine("\nId: " + dataReader.GetValue(0).ToString());
                            Console.WriteLine("Date: " + dataReader.GetValue(1).ToString());
                            Console.WriteLine("Title: " + dataReader.GetValue(2).ToString());
                            Console.WriteLine("Description: " + dataReader.GetValue(3).ToString());
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine("\n");
                Console.Write("Enter the id to be updated: ");
                string userId = Console.ReadLine();
                if (Regex.IsMatch(userId, "^[1-9]"))
                {
                    Console.Write("Title: ");
                    string title = Console.ReadLine();
                    if (Regex.IsMatch(title, @"^[A-Za-z\s]*$"))
                    {
                        if (title == "")
                        {
                            title = DateTime.Now.ToString();
                        }
                        Console.Write("Description: ");
                        string description = Console.ReadLine();
                        if (Regex.IsMatch(description, @"^[A-Za-z\s]*$"))
                        {
                            Console.Write("Book Mark (yes/no): ");
                            string bookMark = Console.ReadLine();
                            if (Regex.IsMatch(bookMark, @"^[a-zA-Z]+$"))
                            {
                                UpdateDairy(userId, title, description, bookMark);
                            }
                            else
                            {
                                Console.WriteLine("Book mark value is not in correct format! Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Description value is not in correct format! Please try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Title value is not in correct format! Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Id is not in correct format! Please try again.");

                }
            }
        }
        #endregion

        #region Dairy updation
        static void UpdateDairy(string userId, string title, string description, string bookMark)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DairyDetailsCRUD", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", userId);
                    command.Parameters.AddWithValue("@Action", "Update");
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@BookMark", bookMark);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine("Dairy updated sucessfully...");
                }
            }
        }
        #endregion

        #region Dairy deletion
        static void DeleteDairy()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string heading = "\nYour Dairy";
                char[] headingChar = heading.ToCharArray();
                Console.WriteLine(headingChar);
                for (int i = 1; i < headingChar.Length; i++)
                {
                    if (headingChar[i] == ' ')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("=");
                    }
                }
                Console.WriteLine("\n");
                using (SqlCommand command = new SqlCommand("ViewDairy", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", "View");
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Console.WriteLine("\nId: " + dataReader.GetValue(0).ToString());
                            Console.WriteLine("Date: " + dataReader.GetValue(1).ToString());
                            Console.WriteLine("Title: " + dataReader.GetValue(2).ToString());
                            Console.WriteLine("Description: " + dataReader.GetValue(3).ToString());
                            Console.WriteLine();
                        }
                    }
                }
                Console.Write("Please enter the Id to be deleted: ");
                string userId = Console.ReadLine();
                using (SqlCommand command = new SqlCommand("DairyDetailsCRUD", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", "Delete");
                    command.Parameters.AddWithValue("@Id", userId);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine("Dairy deleted successfully...");
                }
            }
        }
        #endregion

        #region Book mark
        static void BookMark()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string heading = "\nSelect By Book Mark";
                char[] headingChar = heading.ToCharArray();
                Console.WriteLine(headingChar);
                for (int i = 1; i < headingChar.Length; i++)
                {
                    if (headingChar[i] == ' ')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("=");
                    }
                }
                Console.WriteLine("\n");
                using (SqlCommand command = new SqlCommand("ViewDairy", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchKey", "yes");
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Console.WriteLine("\nId: " + dataReader.GetValue(0).ToString());
                            Console.WriteLine("Date: " + dataReader.GetValue(1).ToString());
                            Console.WriteLine("Title: " + dataReader.GetValue(2).ToString());
                            Console.WriteLine("Description: " + dataReader.GetValue(3).ToString());
                            Console.WriteLine("Book Mark: " + dataReader.GetValue(4).ToString());
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
        #endregion

        #region Dairy search
        static void Search()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string heading = "\nSearch";
                char[] headingChar = heading.ToCharArray();
                Console.WriteLine(headingChar);
                for (int i = 1; i < headingChar.Length; i++)
                {
                    if (headingChar[i] == ' ')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("=");
                    }
                }
                Console.WriteLine();
                Console.Write("Enter the word: ");
                string userInput = Console.ReadLine();

                using (SqlCommand command = new SqlCommand("ViewDairy", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchKey", userInput);
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Console.WriteLine("\nId: " + dataReader.GetValue(0).ToString());
                            Console.WriteLine("Date: " + dataReader.GetValue(1).ToString());
                            Console.WriteLine("Title: " + dataReader.GetValue(2).ToString());
                            Console.WriteLine("Description: " + dataReader.GetValue(3).ToString());
                            Console.WriteLine("Book Mark: " + dataReader.GetValue(4).ToString());
                            Console.WriteLine();
                        }

                    }
                }
            }

        }
        #endregion

        #region Dairy filter by date
        static void FilterByDate()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.Write("Enter a starting date (e.g. dd/mm/yyyy): ");
                string firstDateString = Console.ReadLine();
                Console.Write("Enter an ending date (e.g. dd/mm/yyyy): ");
                string secondDateString = Console.ReadLine();
                Console.WriteLine();
                string heading = $"Filter dairy from {firstDateString} to {secondDateString}";
                char[] headingChar = heading.ToCharArray();
                Console.WriteLine(headingChar);
                for (int i = 1; i < headingChar.Length; i++)
                {
                    if (headingChar[i] == ' ')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("=");
                    }
                }

                DateTime firstDate;
                DateTime secondDate;

                if (DateTime.TryParseExact(firstDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out firstDate) &&
                    DateTime.TryParseExact(secondDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out secondDate))
                {
                    using (SqlCommand command = new SqlCommand("ViewDairy", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "filter");
                        command.Parameters.AddWithValue("@FirstDate", firstDate);
                        command.Parameters.AddWithValue("@SecondDate", secondDate);
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Console.WriteLine("\nId: " + dataReader.GetValue(0).ToString());
                                Console.WriteLine("Date: " + dataReader.GetValue(1).ToString());
                                Console.WriteLine("Title: " + dataReader.GetValue(2).ToString());
                                Console.WriteLine("Description: " + dataReader.GetValue(3).ToString());
                                Console.WriteLine("Book Mark: " + dataReader.GetValue(4).ToString());
                                Console.WriteLine();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid date format. Please use the dd/mm/yyyy format.");
                }
            }
        }
        #endregion


    }
}
      