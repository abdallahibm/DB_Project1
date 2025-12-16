using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            string checkSuspendedQuery = $"SELECT Status FROM Accounts WHERE Username = '{usernameToSuspend}'";
            object statusResult = dbMan.ExecuteScalar(checkSuspendedQuery);

            if (statusResult != null && statusResult.ToString() == "Suspended")
            {
                return "ALREADY_SUSPENDED";
            }

            // Try to suspend - UPDATE Status AND Suspending_Admin_ID
            string suspendQuery = $"UPDATE Accounts SET Status = 'Suspended', Suspending_Admin_ID = {adminID} WHERE Username = '{usernameToSuspend}'";
            int rowsAffected = dbMan.ExecuteNonQuery(suspendQuery);

            return rowsAffected > 0 ? "SUCCESS" : "ERROR";
        }


        public bool UnsuspendAccount(string usernameToUnsuspend)
        {
            // Check if actually suspended
            string checkQuery = $"SELECT Status FROM Accounts WHERE Username = '{usernameToUnsuspend}'";
            object statusResult = dbMan.ExecuteScalar(checkQuery);

            if (statusResult == null || statusResult.ToString() != "Suspended")
            {
                return false; // Not suspended or user doesn't exist
            }

            // Remove suspension - UPDATE Status AND clear Suspending_Admin_ID
            string unsuspendQuery = $"UPDATE Accounts SET Status = 'Active', Suspending_Admin_ID = NULL WHERE Username = '{usernameToUnsuspend}'";
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

                // Get all Events by this Agency (through Create_Event table)
                string getEventsQuery = $"SELECT DISTINCT CE.Event_ID FROM Create_Event CE WHERE CE.Agency_ID = {agencyID}";
                DataTable events = dbMan.ExecuteReader(getEventsQuery);

                if (events != null)
                {
                    foreach (DataRow row in events.Rows)
                    {
                        int eventID = Convert.ToInt32(row["Event_ID"]);

                        // FIRST: Get all Create_Event entries for this event
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

                    // Delete Events (if they have no other agencies)
                    string deleteOrphanEventsQuery = $"DELETE FROM Events WHERE Event_ID NOT IN (SELECT Event_ID FROM Create_Event)";
                    dbMan.ExecuteNonQuery(deleteOrphanEventsQuery);
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

            // 4. NEW: Clear Suspension status and Suspending_Admin_ID before deletion
            string updateStatusQuery = $"UPDATE Accounts SET Status = 'Active', Suspending_Admin_ID = NULL WHERE Username = '{usernameToDelete}'";
            dbMan.ExecuteNonQuery(updateStatusQuery);

            // 5. Finally delete from Accounts table
            string deleteAccountQuery = $"DELETE FROM Accounts WHERE Username = '{usernameToDelete}'";
            int rowsAffected = dbMan.ExecuteNonQuery(deleteAccountQuery);

            return rowsAffected > 0;
        }

        public string GetAgencyForEvent(string eventName)
        {
            // NEW: Get Agency from Create_Event table (Events table no longer has Agency_ID)
            string query = $"SELECT TOP 1 OA.Name FROM Events E, Create_Event CE, Organizing_Agency OA WHERE E.Event_ID = CE.Event_ID AND CE.Agency_ID = OA.Agency_ID AND E.Name = '" + eventName + "'";

            object result = dbMan.ExecuteScalar(query);
            return result != null ? result.ToString() : "Not Found";
        }

        public DataTable GetAvailableUshers()
        {
            // Specify which table's Username to use (U.Username from Ushers table)
            string query = "SELECT U.Username FROM Ushers U, Accounts A WHERE U.Username = A.Username AND A.Status = 'Active' ORDER BY U.Username";

            return dbMan.ExecuteReader(query);
        }

        public int GetEventID(string eventName)
        {
            string query = "SELECT Event_ID FROM Events WHERE Name = '" + eventName + "'";
            object result = dbMan.ExecuteScalar(query);

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            return -1;
        }

        public bool ApproveEvent(int eventID, int adminID)
        {
            string query = "UPDATE Create_Event SET Status = 'Approved' WHERE Event_ID = " + eventID;
            return dbMan.ExecuteNonQuery(query) > 0;
        }

        public bool DeclineEvent(int eventID, int adminID)
        {
            string query = "UPDATE Create_Event SET Status = 'Declined' WHERE Event_ID = " + eventID;
            return dbMan.ExecuteNonQuery(query) > 0;
        }

        public bool AssignUsherToEvent(string usherUsername, int eventID)
        {
            // Get usher SSN from username
            string getSSNQuery = "SELECT SSN FROM Ushers WHERE Username = '" + usherUsername + "'";
            object ssnResult = dbMan.ExecuteScalar(getSSNQuery);

            if (ssnResult == null)
            {
                return false; // Usher not found
            }

            string usherSSN = ssnResult.ToString();

            // Check if already assigned
            string checkQuery = "SELECT COUNT(*) FROM Allow_Entry WHERE Usher_SSN = '" + usherSSN + "' AND Event_ID = " + eventID;
            int alreadyAssigned = Convert.ToInt32(dbMan.ExecuteScalar(checkQuery));

            if (alreadyAssigned > 0)
            {
                return false; // Already assigned
            }

            // Get the event date/time from Create_Event table
            string getEventDateTimeQuery = "SELECT TOP 1 Date_And_Time FROM Create_Event WHERE Event_ID = " + eventID;
            object eventDateTimeResult = dbMan.ExecuteScalar(getEventDateTimeQuery);

            if (eventDateTimeResult == null)
            {
                return false; // Event not found in Create_Event
            }

            DateTime eventDateTime = Convert.ToDateTime(eventDateTimeResult);
            string sqlDateTime = eventDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            // Insert with the EVENT date/time
            string insertQuery = "INSERT INTO Allow_Entry (Usher_SSN, Event_ID, Entry_DateTime) VALUES ('" + usherSSN + "', " + eventID + ", '" + sqlDateTime + "')";

            return dbMan.ExecuteNonQuery(insertQuery) > 0;
        }

        // 7. Get Event Status from Create_Event table
        public string GetEventStatus(string eventName)
        {
            string query = "SELECT TOP 1 CE.Status FROM Create_Event CE, Events E WHERE CE.Event_ID = E.Event_ID AND E.Name = '" + eventName + "'";

            object result = dbMan.ExecuteScalar(query);
            return result != null ? result.ToString() : "Unknown";
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