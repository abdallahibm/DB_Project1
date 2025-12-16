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

            // Check if already suspended - USING THE Status COLUMN
            string checkSuspendedQuery = $"SELECT Status FROM Accounts WHERE Username = '{usernameToSuspend}'";
            object statusResult = dbMan.ExecuteScalar(checkSuspendedQuery);

            if (statusResult != null && statusResult.ToString() == "Suspended")
            {
                return "ALREADY_SUSPENDED";
            }

            // Try to suspend - UPDATE Status column instead of inserting into Suspend table
            string suspendQuery = $"UPDATE Accounts SET Status = 'Suspended' WHERE Username = '{usernameToSuspend}'";
            int rowsAffected = dbMan.ExecuteNonQuery(suspendQuery);

            return rowsAffected > 0 ? "SUCCESS" : "ERROR";
        }


        public bool UnsuspendAccount(string usernameToUnsuspend)
        {
            // Check if actually suspended - USING THE Status COLUMN
            string checkQuery = $"SELECT Status FROM Accounts WHERE Username = '{usernameToUnsuspend}'";
            object statusResult = dbMan.ExecuteScalar(checkQuery);

            if (statusResult == null || statusResult.ToString() != "Suspended")
            {
                return false; // Not suspended or user doesn't exist
            }

            // Remove suspension - UPDATE Status column to 'Active'
            string unsuspendQuery = $"UPDATE Accounts SET Status = 'Active' WHERE Username = '{usernameToUnsuspend}'";
            return dbMan.ExecuteNonQuery(unsuspendQuery) > 0;
        }

        public bool DeleteAccount(string usernameToDelete)
        {
            // 1. Check if account exists
            string checkQuery = $"SELECT COUNT(*) FROM Accounts WHERE Username = '{usernameToDelete}'";
            int exists = Convert.ToInt32(dbMan.ExecuteScalar(checkQuery));

            if (exists == 0)
            {
                return false; // User doesn't exist
            }

            // 2. Check if user is an Admin (CANNOT delete another admin)
            string checkAdminQuery = $"SELECT COUNT(*) FROM Administrators WHERE Username = '{usernameToDelete}'";
            int isAdmin = Convert.ToInt32(dbMan.ExecuteScalar(checkAdminQuery));

            if (isAdmin > 0)
            {
                return false; // Cannot delete another admin
            }

            // 3. Check what type of user this is
            string checkMemberQuery = $"SELECT COUNT(*) FROM Members WHERE Username = '{usernameToDelete}'";
            string checkAgencyQuery = $"SELECT COUNT(*) FROM Organizing_Agency WHERE Username = '{usernameToDelete}'";
            string checkUsherQuery = $"SELECT COUNT(*) FROM Ushers WHERE Username = '{usernameToDelete}'";

            int isMember = Convert.ToInt32(dbMan.ExecuteScalar(checkMemberQuery));
            int isAgency = Convert.ToInt32(dbMan.ExecuteScalar(checkAgencyQuery));
            int isUsher = Convert.ToInt32(dbMan.ExecuteScalar(checkUsherQuery));

            // ========== DELETE MEMBER ==========
            if (isMember > 0)
            {
                // Get Member's SSN
                string getSSNQuery = $"SELECT SSN FROM Members WHERE Username = '{usernameToDelete}'";
                string ssn = dbMan.ExecuteScalar(getSSNQuery).ToString();

                // Delete Member's Ratings
                string deleteRatingsQuery = $"DELETE FROM Rating WHERE SSN = '{ssn}'";
                dbMan.ExecuteNonQuery(deleteRatingsQuery);

                // Delete Member's Bookings
                string deleteBookingsQuery = $"DELETE FROM Book WHERE SSN = '{ssn}'";
                dbMan.ExecuteNonQuery(deleteBookingsQuery);

                // Delete Member
                string deleteMemberQuery = $"DELETE FROM Members WHERE Username = '{usernameToDelete}'";
                dbMan.ExecuteNonQuery(deleteMemberQuery);
            }

            // ========== DELETE AGENCY ==========
            else if (isAgency > 0)
            {
                // Get Agency_ID
                string getAgencyIDQuery = $"SELECT Agency_ID FROM Organizing_Agency WHERE Username = '{usernameToDelete}'";
                int agencyID = Convert.ToInt32(dbMan.ExecuteScalar(getAgencyIDQuery));

                // Get all Events by this Agency
                string getEventsQuery = $"SELECT Event_ID FROM Events WHERE Agency_ID = {agencyID}";
                DataTable events = dbMan.ExecuteReader(getEventsQuery);

                if (events != null)
                {
                    foreach (DataRow row in events.Rows)
                    {
                        int eventID = Convert.ToInt32(row["Event_ID"]);

                        // FIRST: Get all Create_Event entries for this event (need Agency_ID and Category)
                        string getCreateEventQuery = $"SELECT Agency_ID, Category FROM Create_Event WHERE Event_ID = {eventID}";
                        DataTable createEvents = dbMan.ExecuteReader(getCreateEventQuery);

                        if (createEvents != null)
                        {
                            foreach (DataRow ceRow in createEvents.Rows)
                            {
                                int ceAgencyID = Convert.ToInt32(ceRow["Agency_ID"]);
                                string category = ceRow["Category"].ToString();

                                // Delete Event_Inventory for this specific Create_Event entry
                                string deleteInventoryQuery = $"DELETE FROM Event_Inventory WHERE Event_ID = {eventID} AND Agency_ID = {ceAgencyID} AND Category = '{category}'";
                                dbMan.ExecuteNonQuery(deleteInventoryQuery);
                            }
                        }

                        // Delete Create_Event for this event
                        string deleteCreateEventQuery = $"DELETE FROM Create_Event WHERE Event_ID = {eventID}";
                        dbMan.ExecuteNonQuery(deleteCreateEventQuery);

                        // Delete Allow_Entry for this event
                        string deleteAllowEntryQuery = $"DELETE FROM Allow_Entry WHERE Event_ID = {eventID}";
                        dbMan.ExecuteNonQuery(deleteAllowEntryQuery);

                        // Get Tickets for this event
                        string getTicketsQuery = $"SELECT Ticket_ID FROM Tickets WHERE Event_ID = {eventID}";
                        DataTable tickets = dbMan.ExecuteReader(getTicketsQuery);

                        if (tickets != null)
                        {
                            foreach (DataRow ticketRow in tickets.Rows)
                            {
                                int ticketID = Convert.ToInt32(ticketRow["Ticket_ID"]);

                                // Delete Bookings for this ticket
                                string deleteBookingsQuery = $"DELETE FROM Book WHERE Ticket_ID = {ticketID}";
                                dbMan.ExecuteNonQuery(deleteBookingsQuery);
                            }

                            // Delete Tickets for this event
                            string deleteTicketsQuery = $"DELETE FROM Tickets WHERE Event_ID = {eventID}";
                            dbMan.ExecuteNonQuery(deleteTicketsQuery);
                        }

                        // Delete Ratings for this event
                        string deleteRatingsQuery = $"DELETE FROM Rating WHERE Event_ID = {eventID}";
                        dbMan.ExecuteNonQuery(deleteRatingsQuery);
                    }

                    // Delete Events by this Agency
                    string deleteEventsQuery = $"DELETE FROM Events WHERE Agency_ID = {agencyID}";
                    dbMan.ExecuteNonQuery(deleteEventsQuery);
                }

                // Delete Agency
                string deleteAgencyQuery = $"DELETE FROM Organizing_Agency WHERE Username = '{usernameToDelete}'";
                dbMan.ExecuteNonQuery(deleteAgencyQuery);
            }

            // ========== DELETE USHER ==========
            else if (isUsher > 0)
            {
                // Get Usher's SSN
                string getSSNQuery = $"SELECT SSN FROM Ushers WHERE Username = '{usernameToDelete}'";
                string ssn = dbMan.ExecuteScalar(getSSNQuery).ToString();

                // Delete Usher's Phones
                string deletePhonesQuery = $"DELETE FROM Usher_Phones WHERE SSN = '{ssn}'";
                dbMan.ExecuteNonQuery(deletePhonesQuery);

                // Delete Usher from Allow_Entry
                string deleteAllowEntryQuery = $"DELETE FROM Allow_Entry WHERE Usher_SSN = '{ssn}'";
                dbMan.ExecuteNonQuery(deleteAllowEntryQuery);

                // Delete Usher
                string deleteUsherQuery = $"DELETE FROM Ushers WHERE Username = '{usernameToDelete}'";
                dbMan.ExecuteNonQuery(deleteUsherQuery);
            }

            // 4. REMOVE SUSPENSION STATUS (NOT Suspend table) - Update Status to 'Active' before deletion
            string updateStatusQuery = $"UPDATE Accounts SET Status = 'Active' WHERE Username = '{usernameToDelete}'";
            dbMan.ExecuteNonQuery(updateStatusQuery);

            // 5. Finally delete from Accounts table
            string deleteAccountQuery = $"DELETE FROM Accounts WHERE Username = '{usernameToDelete}'";
            int rowsAffected = dbMan.ExecuteNonQuery(deleteAccountQuery);

            return rowsAffected > 0;
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