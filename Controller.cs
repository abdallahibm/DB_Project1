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

        public void LoadEventComboBoxInAdmin(ComboBox comboBoxEventNames)
        {
            // Create DBManager directly
            DBManager dbMan = new DBManager();

            // Query to get event names
            string query = "SELECT Name FROM Events ORDER BY Name";

            DataTable events = dbMan.ExecuteReader(query);

            // Clear existing items
            comboBoxEventNames.Items.Clear();

            if (events != null && events.Rows.Count > 0)
            {
                // Add each event name to combobox
                foreach (DataRow row in events.Rows)
                {
                    string eventName = row["Name"].ToString();
                    comboBoxEventNames.Items.Add(eventName);
                }

                // Select first item
                comboBoxEventNames.SelectedIndex = 0;
            }
            else
            {
                comboBoxEventNames.Items.Add("No events found");
                comboBoxEventNames.SelectedIndex = 0;
            }
        }

        public string SuspendAccount(int adminID, string usernameToSuspend)
        {
            // Return different strings for different cases

            // Check if user exists
            string checkUserQuery = $"SELECT COUNT(*) FROM Accounts WHERE Username = '{usernameToSuspend}'";
            int userExists = Convert.ToInt32(dbMan.ExecuteScalar(checkUserQuery));

            if (userExists == 0)
            {
                return "USER_NOT_FOUND";
            }

            // Check if user is an Admin
            string checkAdminQuery = $"SELECT COUNT(*) FROM Administrators WHERE Username = '{usernameToSuspend}'";
            int isAdmin = Convert.ToInt32(dbMan.ExecuteScalar(checkAdminQuery));

            if (isAdmin > 0)
            {
                return "CANNOT_SUSPEND_ADMIN";
            }

            // Check if already suspended
            string checkSuspendedQuery = $"SELECT COUNT(*) FROM Suspend WHERE Username = '{usernameToSuspend}'";
            int alreadySuspended = Convert.ToInt32(dbMan.ExecuteScalar(checkSuspendedQuery));

            if (alreadySuspended > 0)
            {
                return "ALREADY_SUSPENDED";
            }

            // Try to suspend
            string suspendQuery = $"INSERT INTO Suspend (Admin_ID, Username) VALUES ({adminID}, '{usernameToSuspend}')";
            int rowsAffected = dbMan.ExecuteNonQuery(suspendQuery);

            return rowsAffected > 0 ? "SUCCESS" : "ERROR";
        }


        public bool UnsuspendAccount(string usernameToUnsuspend)
        {
            // Check if actually suspended
            string checkQuery = $"SELECT COUNT(*) FROM Suspend WHERE Username = '{usernameToUnsuspend}'";
            int isSuspended = Convert.ToInt32(dbMan.ExecuteScalar(checkQuery));

            if (isSuspended == 0)
            {
                return false; // Not suspended
            }

            // Remove from Suspend table
            string unsuspendQuery = $"DELETE FROM Suspend WHERE Username = '{usernameToUnsuspend}'";
            return dbMan.ExecuteNonQuery(unsuspendQuery) > 0;
        }

        public int GetAdminID(string username)
        {
            string query = $"SELECT Admin_ID FROM Administrators WHERE Username = '{username}'";
            object result = dbMan.ExecuteScalar(query);

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            return -1; // Return -1 if not found (error)
        }



        public string Login_Member(string username, string password)
        {
            string query = "SELECT M.SSN " +
                           "FROM Members M, Accounts A " +
                           "WHERE M.Username = A.Username " +
                           "AND A.Username = '" + username + "' " +
                           "AND A.Password = '" + password + "';";

            object result = dbMan.ExecuteScalar(query);

            if (result == null)
                return null;
            else
                return result.ToString();
        }



        public int Insert_New_Member(string fName, string lName, string date, string gender,
                                     string ssn, string email, string phone, string nationality,
                                     string username, string password)
        {

            string accountQuery = "INSERT INTO Accounts (Username, Password) " +
                                  "VALUES ('" + username + "', '" + password + "');";

            int result1 = dbMan.ExecuteNonQuery(accountQuery);

            if (result1 == 0) return 0; 

            string memberQuery = "INSERT INTO Members " +
                                 "(SSN, First_Name, Last_Name, Email, Phone_Number, Gender, Date_Of_Birth, Nationality, Username) " +
                                 "VALUES ('" + ssn + "', '" + fName + "', '" + lName + "', '" + email + "', '" +
                                 phone + "', '" + gender + "', '" + date + "', '" + nationality + "', '" + username + "');";
            return dbMan.ExecuteNonQuery(memberQuery);
        }

        public bool checkuser(string username)
        {
           
            string query = "SELECT COUNT(*) FROM Accounts WHERE Username = '" + username + "';";
            object result = dbMan.ExecuteScalar(query);
            int count = Convert.ToInt32(result);
            if(count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Checkemail(string email)
        {
            string query = "SELECT COUNT(*) FROM Members WHERE Email = '" + email + "';";

            object result = dbMan.ExecuteScalar(query);
            int count = Convert.ToInt32(result);

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkssn(string ssn)
        {
            string query= "SELECT COUNT(*) FROM Members WHERE SSN = '" + ssn + "';";
            object result = dbMan.ExecuteScalar(query);
            int count = Convert.ToInt32(result);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }

}