using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DBapplication
{
    public class Controller
    {
        DBManager dbMan;

        public Controller()
        {
            dbMan = new DBManager();
        }


        // insert functions here
        public void tesast()
        {
             //test changes;
        }



        public string ValidateAdminLogin(string username, string password)
        {
            // First check if username exists in Accounts table
            string checkUserQuery = $"SELECT COUNT(*) FROM Accounts WHERE Username = '{username}'";
            int userCount = Convert.ToInt32(dbMan.ExecuteScalar(checkUserQuery));

            // If username doesn't exist
            if (userCount == 0)
            {
                return "USER_NOT_FOUND"; // Username not in database
            }

            // Check if this user is an Admin
            string checkAdminQuery = $"SELECT COUNT(*) FROM Administrators WHERE Username = '{username}'";
            int adminCount = Convert.ToInt32(dbMan.ExecuteScalar(checkAdminQuery));

            if (adminCount == 0)
            {
                return "NOT_ADMIN"; // User exists but is not an admin
            }

            // Now check if password matches
            string checkPasswordQuery = $"SELECT Password FROM Accounts WHERE Username = '{username}'";
            object result = dbMan.ExecuteScalar(checkPasswordQuery);

            if (result == null)
            {
                return "ERROR"; // Shouldn't happen if username exists
            }

            string storedPassword = result.ToString();

            // Compare passwords
            if (storedPassword != password)
            {
                return "WRONG_PASSWORD"; // Username exists but password is wrong
            }

            // Everything is correct
            return "SUCCESS";
        }


        public bool AddNewAdmin(string firstName, string lastName, string username, string password)
        {
            // 1. Check if username already exists
            string checkUserQuery = $"SELECT COUNT(*) FROM Accounts WHERE Username = '{username}'";
            int existingUser = Convert.ToInt32(dbMan.ExecuteScalar(checkUserQuery));

            if (existingUser > 0)
            {
                return false;
            }

            // 2. Insert into Accounts table
            string insertAccountQuery = $"INSERT INTO Accounts (Username, Password) VALUES ('{username}', '{password}')";
            int accountRows = dbMan.ExecuteNonQuery(insertAccountQuery);

            if (accountRows == 0) return false;

            // 3. Insert into Administrators table
            string insertAdminQuery = $"INSERT INTO Administrators (First_Name, Last_Name, Username) VALUES ('{firstName}', '{lastName}', '{username}')";
            int adminRows = dbMan.ExecuteNonQuery(insertAdminQuery);

            return adminRows > 0;
        }


    }

}