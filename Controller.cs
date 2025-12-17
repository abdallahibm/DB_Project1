using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
            
            string checkUserQuery = $"SELECT COUNT(*) FROM Accounts WHERE Username = '{username}'";
            int userCount = Convert.ToInt32(dbMan.ExecuteScalar(checkUserQuery));

           
            if (userCount == 0)
            {
                return "USER_NOT_FOUND"; 
            }

            
            string checkAdminQuery = $"SELECT COUNT(*) FROM Administrators WHERE Username = '{username}'";
            int adminCount = Convert.ToInt32(dbMan.ExecuteScalar(checkAdminQuery));

            if (adminCount == 0)
            {
                return "NOT_ADMIN"; 
            }

            
            string checkPasswordQuery = $"SELECT Password FROM Accounts WHERE Username = '{username}'";
            object result = dbMan.ExecuteScalar(checkPasswordQuery);

            if (result == null)
            {
                return "ERROR"; 
            }

            string storedPassword = result.ToString();

            
            if (storedPassword != password)
            {
                return "WRONG_PASSWORD"; 
            }

            
            return "SUCCESS";
        }


        public bool AddNewAdmin(string firstName, string lastName, string username, string password)
        {
           
            string checkUserQuery = $"SELECT COUNT(*) FROM Accounts WHERE Username = '{username}'";
            int existingUser = Convert.ToInt32(dbMan.ExecuteScalar(checkUserQuery));

            if (existingUser > 0)
            {
                return false;
            }

            
            string insertAccountQuery = $"INSERT INTO Accounts (Username, Password) VALUES ('{username}', '{password}')";
            int accountRows = dbMan.ExecuteNonQuery(insertAccountQuery);

            if (accountRows == 0) return false;

            
            string insertAdminQuery = $"INSERT INTO Administrators (First_Name, Last_Name, Username) VALUES ('{firstName}', '{lastName}', '{username}')";
            int adminRows = dbMan.ExecuteNonQuery(insertAdminQuery);

            return adminRows > 0;
        }



        public string SuspendAccount(int adminID, string usernameToSuspend)
        {
            

           
            string checkUserQuery = $"SELECT COUNT(*) FROM Accounts WHERE Username = '{usernameToSuspend}'";
            int userExists = Convert.ToInt32(dbMan.ExecuteScalar(checkUserQuery));

            if (userExists == 0)
            {
                return "USER_NOT_FOUND";
            }

            
            string checkAdminQuery = $"SELECT COUNT(*) FROM Administrators WHERE Username = '{usernameToSuspend}'";
            int isAdmin = Convert.ToInt32(dbMan.ExecuteScalar(checkAdminQuery));

            if (isAdmin > 0)
            {
                return "CANNOT_SUSPEND_ADMIN";
            }

            
            string checkSuspendedQuery = $"SELECT Status FROM Accounts WHERE Username = '{usernameToSuspend}'";
            object statusResult = dbMan.ExecuteScalar(checkSuspendedQuery);

            if (statusResult != null && statusResult.ToString() == "Suspended")
            {
                return "ALREADY_SUSPENDED";
            }

            
            string suspendQuery = $"UPDATE Accounts SET Status = 'Suspended', Suspending_Admin_ID = {adminID} WHERE Username = '{usernameToSuspend}'";
            int rowsAffected = dbMan.ExecuteNonQuery(suspendQuery);

            return rowsAffected > 0 ? "SUCCESS" : "ERROR";
        }


        public bool UnsuspendAccount(string usernameToUnsuspend)
        {
           
            string checkQuery = $"SELECT Status FROM Accounts WHERE Username = '{usernameToUnsuspend}'";
            object statusResult = dbMan.ExecuteScalar(checkQuery);

            if (statusResult == null || statusResult.ToString() != "Suspended")
            {
                return false; 
            }

            
            string unsuspendQuery = $"UPDATE Accounts SET Status = 'Active', Suspending_Admin_ID = NULL WHERE Username = '{usernameToUnsuspend}'";
            return dbMan.ExecuteNonQuery(unsuspendQuery) > 0;
        }

        public bool DeleteAccount(string usernameToDelete)
        {
            
            string checkQuery = $"SELECT COUNT(*) FROM Accounts WHERE Username = '{usernameToDelete}'";
            int exists = Convert.ToInt32(dbMan.ExecuteScalar(checkQuery));

            if (exists == 0)
            {
                return false; 
            }

            
            string checkAdminQuery = $"SELECT COUNT(*) FROM Administrators WHERE Username = '{usernameToDelete}'";
            int isAdmin = Convert.ToInt32(dbMan.ExecuteScalar(checkAdminQuery));

            if (isAdmin > 0)
            {
                return false; 
            }

           
            string checkMemberQuery = $"SELECT COUNT(*) FROM Members WHERE Username = '{usernameToDelete}'";
            string checkAgencyQuery = $"SELECT COUNT(*) FROM Organizing_Agency WHERE Username = '{usernameToDelete}'";
            string checkUsherQuery = $"SELECT COUNT(*) FROM Ushers WHERE Username = '{usernameToDelete}'";

            int isMember = Convert.ToInt32(dbMan.ExecuteScalar(checkMemberQuery));
            int isAgency = Convert.ToInt32(dbMan.ExecuteScalar(checkAgencyQuery));
            int isUsher = Convert.ToInt32(dbMan.ExecuteScalar(checkUsherQuery));

            
            if (isMember > 0)
            {
                
                string getSSNQuery = $"SELECT SSN FROM Members WHERE Username = '{usernameToDelete}'";
                string ssn = dbMan.ExecuteScalar(getSSNQuery).ToString();

                
                string deleteRatingsQuery = $"DELETE FROM Rating WHERE SSN = '{ssn}'";
                dbMan.ExecuteNonQuery(deleteRatingsQuery);

                
                string deleteBookingsQuery = $"DELETE FROM Book WHERE SSN = '{ssn}'";
                dbMan.ExecuteNonQuery(deleteBookingsQuery);

              
                string deleteMemberQuery = $"DELETE FROM Members WHERE Username = '{usernameToDelete}'";
                dbMan.ExecuteNonQuery(deleteMemberQuery);
            }

            
            else if (isAgency > 0)
            {
               
                string getAgencyIDQuery = $"SELECT Agency_ID FROM Organizing_Agency WHERE Username = '{usernameToDelete}'";
                int agencyID = Convert.ToInt32(dbMan.ExecuteScalar(getAgencyIDQuery));

                
                string getEventsQuery = $"SELECT DISTINCT CE.Event_ID FROM Create_Event CE WHERE CE.Agency_ID = {agencyID}";
                DataTable events = dbMan.ExecuteReader(getEventsQuery);

                if (events != null)
                {
                    foreach (DataRow row in events.Rows)
                    {
                        int eventID = Convert.ToInt32(row["Event_ID"]);

                        
                        string getCreateEventQuery = $"SELECT Agency_ID, Category FROM Create_Event WHERE Event_ID = {eventID}";
                        DataTable createEvents = dbMan.ExecuteReader(getCreateEventQuery);

                        if (createEvents != null)
                        {
                            foreach (DataRow ceRow in createEvents.Rows)
                            {
                                int ceAgencyID = Convert.ToInt32(ceRow["Agency_ID"]);
                                string category = ceRow["Category"].ToString();

                                
                                string deleteInventoryQuery = $"DELETE FROM Event_Inventory WHERE Event_ID = {eventID} AND Agency_ID = {ceAgencyID} AND Category = '{category}'";
                                dbMan.ExecuteNonQuery(deleteInventoryQuery);
                            }
                        }

                       
                        string deleteCreateEventQuery = $"DELETE FROM Create_Event WHERE Event_ID = {eventID}";
                        dbMan.ExecuteNonQuery(deleteCreateEventQuery);

                        
                        string deleteAllowEntryQuery = $"DELETE FROM Allow_Entry WHERE Event_ID = {eventID}";
                        dbMan.ExecuteNonQuery(deleteAllowEntryQuery);

                        
                        string getTicketsQuery = $"SELECT Ticket_ID FROM Tickets WHERE Event_ID = {eventID}";
                        DataTable tickets = dbMan.ExecuteReader(getTicketsQuery);

                        if (tickets != null)
                        {
                            foreach (DataRow ticketRow in tickets.Rows)
                            {
                                int ticketID = Convert.ToInt32(ticketRow["Ticket_ID"]);

                                
                                string deleteBookingsQuery = $"DELETE FROM Book WHERE Ticket_ID = {ticketID}";
                                dbMan.ExecuteNonQuery(deleteBookingsQuery);
                            }

                            
                            string deleteTicketsQuery = $"DELETE FROM Tickets WHERE Event_ID = {eventID}";
                            dbMan.ExecuteNonQuery(deleteTicketsQuery);
                        }

                        
                        string deleteRatingsQuery = $"DELETE FROM Rating WHERE Event_ID = {eventID}";
                        dbMan.ExecuteNonQuery(deleteRatingsQuery);
                    }

                    
                    string deleteOrphanEventsQuery = $"DELETE FROM Events WHERE Event_ID NOT IN (SELECT Event_ID FROM Create_Event)";
                    dbMan.ExecuteNonQuery(deleteOrphanEventsQuery);
                }

                
                string deleteAgencyQuery = $"DELETE FROM Organizing_Agency WHERE Username = '{usernameToDelete}'";
                dbMan.ExecuteNonQuery(deleteAgencyQuery);
            }

            
            else if (isUsher > 0)
            {
                
                string getSSNQuery = $"SELECT SSN FROM Ushers WHERE Username = '{usernameToDelete}'";
                string ssn = dbMan.ExecuteScalar(getSSNQuery).ToString();

                
                string deletePhonesQuery = $"DELETE FROM Usher_Phones WHERE SSN = '{ssn}'";
                dbMan.ExecuteNonQuery(deletePhonesQuery);

               
                string deleteAllowEntryQuery = $"DELETE FROM Allow_Entry WHERE Usher_SSN = '{ssn}'";
                dbMan.ExecuteNonQuery(deleteAllowEntryQuery);

               
                string deleteUsherQuery = $"DELETE FROM Ushers WHERE Username = '{usernameToDelete}'";
                dbMan.ExecuteNonQuery(deleteUsherQuery);
            }

            
            string updateStatusQuery = $"UPDATE Accounts SET Status = 'Active', Suspending_Admin_ID = NULL WHERE Username = '{usernameToDelete}'";
            dbMan.ExecuteNonQuery(updateStatusQuery);

            
            string deleteAccountQuery = $"DELETE FROM Accounts WHERE Username = '{usernameToDelete}'";
            int rowsAffected = dbMan.ExecuteNonQuery(deleteAccountQuery);

            return rowsAffected > 0;
        }

        public string GetAgencyForEvent(string eventName)
        {
            
            string query = $"SELECT TOP 1 OA.Name FROM Events E, Create_Event CE, Organizing_Agency OA WHERE E.Event_ID = CE.Event_ID AND CE.Agency_ID = OA.Agency_ID AND E.Name = '" + eventName + "'";

            object result = dbMan.ExecuteScalar(query);
            return result != null ? result.ToString() : "Not Found";
        }

        public DataTable GetAvailableUshers()
        {
            
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
            
            string getSSNQuery = "SELECT SSN FROM Ushers WHERE Username = '" + usherUsername + "'";
            object ssnResult = dbMan.ExecuteScalar(getSSNQuery);

            if (ssnResult == null)
            {
                return false; 
            }

            string usherSSN = ssnResult.ToString();

            
            string checkQuery = "SELECT COUNT(*) FROM Allow_Entry WHERE Usher_SSN = '" + usherSSN + "' AND Event_ID = " + eventID;
            int alreadyAssigned = Convert.ToInt32(dbMan.ExecuteScalar(checkQuery));

            if (alreadyAssigned > 0)
            {
                return false; 
            }

            
            string getEventDateTimeQuery = "SELECT TOP 1 Date_And_Time FROM Create_Event WHERE Event_ID = " + eventID;
            object eventDateTimeResult = dbMan.ExecuteScalar(getEventDateTimeQuery);

            if (eventDateTimeResult == null)
            {
                return false; 
            }

            DateTime eventDateTime = Convert.ToDateTime(eventDateTimeResult);
            string sqlDateTime = eventDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            
            string insertQuery = "INSERT INTO Allow_Entry (Usher_SSN, Event_ID, Entry_DateTime) VALUES ('" + usherSSN + "', " + eventID + ", '" + sqlDateTime + "')";

            return dbMan.ExecuteNonQuery(insertQuery) > 0;
        }

        
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
            return -1; 
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
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //void Just_For_Testing()
        //{






        //}

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
            string query = "SELECT COUNT(*) FROM Members WHERE SSN = '" + ssn + "';";
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
        public DataTable getavalaibleevents()
        {

            string query = "SELECT DISTINCT E.Event_ID, E.Name FROM Events E JOIN Create_Event CE ON E.Event_ID = CE.Event_ID WHERE CE.Status = 'Approved';";

            return dbMan.ExecuteReader(query);
        }

        public DataTable getticketcategories(int eventID)
        {

            string query = "SELECT Category, Price FROM Create_Event WHERE Event_ID = " + eventID + " AND Status = 'Approved';";

            return dbMan.ExecuteReader(query);
        }
        public string getmemberssn(string username)
        {
            string query = "SELECT SSN FROM Members WHERE Username = '" + username + "'";
            object result = dbMan.ExecuteScalar(query);
            if (result == null)
                return null;
            else
                return result.ToString();

        }

        public int checkticketavailability(int eventID, string category)
        {
            string query = "SELECT Available_Tickets FROM Create_Event WHERE Event_ID = " + eventID + " AND Category = '" + category + "'";

            object result = dbMan.ExecuteScalar(query);
            if (result == null)
                return 0;
            else
                return Convert.ToInt32(result);
        }

        
        public string BookTickets(string username, int eventID, string category, int count, string paymentMethod)
        {
            string ssn = getmemberssn(username);
            if (ssn == null)
            {
                return "Error: User SSN not found.";
            }

            int available = checkticketavailability(eventID, category);
            if (available < count)
            {
                return "Not enough tickets. Only " + available + " left.";
            }

            for (int i = 0; i < count; i++)
            {
                string ticketQuery = "INSERT INTO Tickets (Event_ID, Payment_Method, Category) VALUES (" + eventID + ", '" + paymentMethod + "', '" + category + "'); SELECT SCOPE_IDENTITY();";
                int newTicketID = Convert.ToInt32(dbMan.ExecuteScalar(ticketQuery));
                string bookQuery = "INSERT INTO Book (SSN, Ticket_ID, Status, Number_Of_Tickets) VALUES ('" + ssn + "', " + newTicketID + ", 'Active', 1)";

                dbMan.ExecuteNonQuery(bookQuery);

            }

            string updateQuery = "UPDATE Create_Event SET Available_Tickets = Available_Tickets - " + count + " WHERE Event_ID = " + eventID + " AND Category = '" + category + "'";

            int result = dbMan.ExecuteNonQuery(updateQuery);

            if (result > 0)
                return "Success";
            else
                return "Error updating inventory";
        }
        public DataTable GetMemberBookingsDetailed(string username)
        {
            string query = "SELECT DISTINCT " +
                           "E.Name, " +
                           "E.Event_ID, " +
                           "CE.Category, " +
                           "CE.Price, " +


                           "(" +
                               "SELECT COUNT(*) " +
                               "FROM Tickets T, Book B, Members M " +
                               "WHERE T.Ticket_ID = B.Ticket_ID " +
                               "AND B.SSN = M.SSN " +
                               "AND M.Username = '" + username + "' " +
                               "AND T.Event_ID = E.Event_ID " +
                               "AND T.Category = CE.Category" +
                           ") AS TicketCount, " +
                           "(" +
                               "CE.Price * " +
                               "(" +
                                   "SELECT COUNT(*) " +
                                   "FROM Tickets T, Book B, Members M " +
                                   "WHERE T.Ticket_ID = B.Ticket_ID " +
                                   "AND B.SSN = M.SSN " +
                                   "AND M.Username = '" + username + "' " +
                                   "AND T.Event_ID = E.Event_ID " +
                                   "AND T.Category = CE.Category" +
                               ")" +
                           ") AS TotalCost " +

                           "FROM Events E, Create_Event CE " +
                           "WHERE E.Event_ID = CE.Event_ID " +

                           "AND (" +
                               "SELECT COUNT(*) " +
                               "FROM Tickets T, Book B, Members M " +
                               "WHERE T.Ticket_ID = B.Ticket_ID " +
                               "AND B.SSN = M.SSN " +
                               "AND M.Username = '" + username + "' " +
                               "AND T.Event_ID = E.Event_ID " +
                               "AND T.Category = CE.Category" +
                           ") > 0";

            return dbMan.ExecuteReader(query);
        }
        public string DeleteMemberBooking(string username, int eventID, string category, int count)
        {
           
            string getTicketsQuery = "SELECT T.Ticket_ID " +
                                     "FROM Tickets T, Book B, Members M " +
                                     "WHERE T.Ticket_ID = B.Ticket_ID " +
                                     "AND B.SSN = M.SSN " +
                                     "AND M.Username = '" + username + "' " +
                                     "AND T.Event_ID = " + eventID + " " +
                                     "AND T.Category = '" + category + "'";

            DataTable ticketsToDelete = dbMan.ExecuteReader(getTicketsQuery);

            if (ticketsToDelete == null || ticketsToDelete.Rows.Count == 0)
            {
                return "Error: No tickets found to delete.";
            }

            foreach (DataRow row in ticketsToDelete.Rows)
            {
                int ticketID = Convert.ToInt32(row["Ticket_ID"]);

               
                dbMan.ExecuteNonQuery("DELETE FROM Book WHERE Ticket_ID = " + ticketID);

                dbMan.ExecuteNonQuery("DELETE FROM Tickets WHERE Ticket_ID = " + ticketID);
            }

            string updateInventory = "UPDATE Create_Event " +
                                     "SET Available_Tickets = Available_Tickets + " + count + " " +
                                     "WHERE Event_ID = " + eventID + " AND Category = '" + category + "'";

            int result = dbMan.ExecuteNonQuery(updateInventory);

            return result > 0 ? "Success" : "Error updating inventory";
        }
        public bool issuspended(string user)
        {

            string query = "SELECT Status FROM Accounts WHERE Username = '" + user + "'";
        
            object result = dbMan.ExecuteScalar(query);
            string s = Convert.ToString(result);
            if(s== "Suspended")
            {
                return true;
            }
            return false;
        }

        public void testc()
        {
            //test conflict;
        }

    }
}