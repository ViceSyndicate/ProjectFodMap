using ProjectFodMap.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace ProjectFodMap.Data
{
    /// <summary>
    /// needs : https://sqlite.org/download.html 
    /// Universal Windows Platform
    /// VSIX package for Universal Windows Platform development using Visual Studio 2015.
    /// 
    /// Precompiled Binaries for .NET might work too.
    /// </summary>
    /// 
    public class MealsManager
    {
        public static string conString = "Data Source=C:\\SQLite\\ProjectFodMap.db";
        public static void SetupDB()
        {
            // Check if Directory where we save db exists. If it doesn't. Create it.
            if (!Directory.Exists("C:\\SQLite"))
            {
                Directory.CreateDirectory("C:\\SQLite");
            }

            // Check if db exists. If it doesnt, Create it, create tables and fill it with some data.
            if (!System.IO.File.Exists("C:\\SQLite\\ProjectFodMap.db"))
            {
                Console.WriteLine("Trying to write database to: C:\\SQLite\\ProjectFodMap.db");
                System.Data.SQLite.SQLiteConnection.CreateFile("C:\\SQLite\\ProjectFodMap.db");

                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(conString))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();

                        com.CommandText = @"CREATE TABLE Meals(
                                      MealID INTEGER PRIMARY KEY AUTOINCREMENT,
                                      MealName CHAR(40) NOT NULL,
                                      MealIngredients VARCHAR(max) NOT NULL,
                                      MealInstructions VARCHAR(max),
                                      MealNotes VARCHAR(max),
                                      MealRisks VARCHAR(max)
                                    );";

                        com.ExecuteNonQuery();

                        con.Close();
                    }
                }
                InsertTestMeals();
            }
        }

        // part of SetupDB Function.
        public static void InsertTestMeals()
        {
            using (SQLiteConnection con = new SQLiteConnection(conString))
            {
                con.Open();
                string query = @"INSERT INTO
  Meals(
    MealName,
    MealIngredients,
    MealInstructions
  )
VALUES(
    'Baked Potato',
    'Baked Potato Butter Ham',
    'Make 2 cross cuts in the potato and put in oven at 200c for 2 hours.'
  );
INSERT INTO
  Meals(
    MealName,
    MealIngredients,
    MealInstructions
  )
VALUES(
    'Wook',
    'Rice Shrimp Corn Chicken Haricots Egg Carrot',
    'Boil the rice and veggies together, Cook eggs separately when rice is cooking.Throw it in a wook when done and add meat of choice and some oil.'
  );
";
                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    //cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public static List<ViewModels.MealListingVM> GetAllMeals()
        {
            SetupDB();
            // This list of meals gets filled up with data from each row from the Meals table (code below)
            List<ViewModels.MealListingVM> results = new List<ViewModels.MealListingVM>();
            // System.Data.SQLite.SQLiteDataReader dataReader = 

            using (SQLiteConnection con = new SQLiteConnection(conString))
            {
                con.Open();

                string query = "SELECT MealID, MealName, MealIngredients FROM Meals";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ViewModels.MealListingVM meal = new ViewModels.MealListingVM();

                                meal.MealID = Convert.ToInt32(dataReader["MealID"]);
                                meal.MealName = Convert.ToString(dataReader["MealName"]);
                                meal.MealIngredients = Convert.ToString(dataReader["MealIngredients"]);
                                // Add meals to result list which we send to Controller
                                results.Add(meal);
                            }
                        }
                    }
                }
            }
            return results;
        }

        public static List<ViewModels.MealListingVM> GetMealsByName(string searchString)
        {
            List<ViewModels.MealListingVM> resultList = new List<ViewModels.MealListingVM>();

            using (SQLiteConnection con = new SQLiteConnection(conString))
            {
                con.Open();

                // This SQL search query could be improved a lot.
                string query = "SELECT MealID, MealName, MealIngredients FROM Meals WHERE MealName LIKE '%" + searchString + "%'" +
                    " OR MealIngredients LIKE '%" + searchString + "%'";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ViewModels.MealListingVM meal = new ViewModels.MealListingVM();

                                meal.MealID = Convert.ToInt32(dataReader["MealID"]);
                                meal.MealName = Convert.ToString(dataReader["MealName"]);
                                meal.MealIngredients = Convert.ToString(dataReader["MealIngredients"]);
                                // Add meals to result list which we send to Controller
                                resultList.Add(meal);
                            }
                        }
                    }
                }
                return resultList;
            }
        }

        public static Meal GetMealDetails(int id)
        {
            Meal MealData = new Meal();

            using (SQLiteConnection con = new SQLiteConnection(conString))
            {
                con.Open();

                string query = "SELECT * FROM Meals " +
                      "WHERE MealID LIKE " + id;

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                MealData.MealID = Convert.ToInt32(dataReader["MealID"]);
                                MealData.MealName = Convert.ToString(dataReader["MealName"]);
                                MealData.MealIngredients = Convert.ToString(dataReader["MealIngredients"]);
                                MealData.MealInstructions = Convert.ToString(dataReader["MealInstructions"]);
                                MealData.MealNotes = Convert.ToString(dataReader["MealNotes"]);
                                MealData.MealRisks = Convert.ToString(dataReader["MealRisks"]);
                            }
                        }
                    }
                }

                return MealData;
            }
        }

        public void EditMealDetails(int id, Meal meal)
        {
            using (SQLiteConnection con = new SQLiteConnection(conString))
            {

                // I make a bunch of sql calls here to populate the meal.
                // I didn't know how to make one call and populate the Meal object with all the data.
                using (SQLiteCommand cmd = new SQLiteCommand(con))
                {
                    con.Open();
                    if (meal.MealName == null)
                        meal.MealName = "";
                    cmd.CommandText = "UPDATE Meals SET MealName = @MealName WHERE mealID = " + id;
                    cmd.Parameters.Add("@MealName", System.Data.DbType.String);
                    cmd.Parameters["@MealName"].Value = meal.MealName;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine(ex);
                    }

                    if (meal.MealIngredients == null)
                        meal.MealIngredients = "";
                    cmd.CommandText = "UPDATE Meals SET MealIngredients = @MealIngredients WHERE mealID = " + id;
                    cmd.Parameters.Add("@MealIngredients", System.Data.DbType.String);
                    cmd.Parameters["@MealIngredients"].Value = meal.MealIngredients;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    if (meal.MealInstructions == null)
                        meal.MealInstructions = "";
                    cmd.CommandText = "UPDATE Meals SET MealInstructions = @MealInstructions WHERE mealID = " + id;
                    cmd.Parameters.Add("@MealInstructions", System.Data.DbType.String);
                    cmd.Parameters["@MealInstructions"].Value = meal.MealInstructions;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    if (meal.MealNotes == null)
                        meal.MealNotes = "";
                    cmd.CommandText = "UPDATE Meals SET MealNotes = @MealNotes WHERE mealID = " + id;
                    cmd.Parameters.Add("@MealNotes", System.Data.DbType.String);
                    cmd.Parameters["@MealNotes"].Value = meal.MealNotes;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    if (meal.MealRisks == null)
                        meal.MealRisks = "";
                    cmd.CommandText = "UPDATE Meals SET MealRisks = @MealRisks WHERE mealID = " + id;
                    cmd.Parameters.Add("@MealRisks", System.Data.DbType.String);
                    cmd.Parameters["@MealRisks"].Value = meal.MealRisks;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    con.Close();
                }
            }
            return;
        }

        public static void CreateNewMeal(Models.Meal meal)
        {
            using (SQLiteConnection con = new SQLiteConnection(conString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO Meals(MealName, MealIngredients, MealInstructions, MealNotes, MealRisks)" +
                    "VALUES(" +
                    "@MealName," +
                    "@MealIngredients," +
                    "@MealInstructions," +
                    "@MealNotes," +
                    "@MealRisks" +
                    ");");

                string query = sb.ToString();

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.Add("@MealName", System.Data.DbType.String);
                    cmd.Parameters.Add("@MealIngredients", System.Data.DbType.String);
                    cmd.Parameters.Add("@MealInstructions", System.Data.DbType.String);
                    cmd.Parameters.Add("@MealNotes", System.Data.DbType.String);
                    cmd.Parameters.Add("@MealRisks", System.Data.DbType.String);
                    cmd.Parameters["@MealName"].Value = meal.MealName;
                    cmd.Parameters["@MealIngredients"].Value = meal.MealIngredients;
                    cmd.Parameters["@MealInstructions"].Value = meal.MealInstructions;
                    cmd.Parameters["@MealNotes"].Value = meal.MealNotes;
                    cmd.Parameters["@MealRisks"].Value = meal.MealRisks;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    con.Close();
                }
            }
        }

        // the meal parameter is unused here bcause i couldn't figure out how to send the data back from the view to the controller.
        // It's not a must have though so i only delete the meal based of its ID.
        public static void DeleteMeal(int id, Data.ViewModels.MealDataVM meal)
        {
            string query = "DELETE FROM Meals WHERE MealID = @ID;";

            using (SQLiteConnection con = new SQLiteConnection(conString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    // sqlinjection proofing
                    cmd.Parameters.Add("@ID", System.Data.DbType.Int32);
                    cmd.Parameters["@ID"].Value = id;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    con.Close();
                }
            }
        }
    }
}