using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DVLD_Security;

namespace DVLD_DataAccess
{
    public class clsUesrData
    {

        public static bool GetUserInfoByUserID(int UserID, ref int PersonID,  ref string UserName,
                                              ref string PasswordHash, ref string Salt, ref bool IsActive)
        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"Select * From Users  
                         Where UserID= @UserID";
            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                Connection.Open();
               SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.Read())
                {
                    isFound = true;
                   
                    PersonID = (int)Reader["PersonID"];
                    UserName = (string)Reader["UserName"];
                    PasswordHash = (string)Reader["PasswordHash"];
                    Salt = (string)Reader["Salt"];
                    IsActive = (bool)Reader["IsActive"];
                }
                else
                {
                    isFound = false;
                }
                Reader.Close();

            }catch(Exception ex)
            {
                isFound = false;
            }
            finally
            {

                Connection.Close();
            }
            return isFound;

        }


        public static bool GetUserInfoByPersonID(ref int UserID, int PersonID, ref string UserName,
                                         ref string PasswordHash, ref string Salt, ref bool IsActive)
        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"Select * From Users  
                         Where PersonID= @PersonID";
            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    isFound = true;

                    UserID = (int)Reader["UserID"];
                    UserName = (string)Reader["UserName"];
                    PasswordHash = (string)Reader["PasswordHash"];
                    Salt = (string)Reader["Salt"];
                    IsActive = (bool)Reader["IsActive"];
                }
                else
                {
                    isFound = false;
                }
                Reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {

                Connection.Close();
            }
            return isFound;

        }

        public static bool GetUserInfoByUsernameAndPassword(ref int UserID, ref int PersonID, 
                               string UserName, string PasswordHash, ref bool IsActive)

        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);

            try
            {
                Connection.Open();

                // Get the salt from the database.
                string Saltquery = "Select Salt From Users Where UserName=@UserName";
                SqlCommand Command = new SqlCommand(Saltquery, Connection);
                Command.Parameters.AddWithValue("UserName", UserName);
                object SaltResult = Command.ExecuteScalar();
                if(SaltResult == null)
                {
                    return false;
                }
                string salt = SaltResult.ToString();

                //Encrypt the password the user entered with the same salt

                string hashPassword = clsSecurity.Has256Password(PasswordHash, salt);
                //Compare the resulting hash with the store.
                string query = @"Select * From Users  
                         Where UserName= @UserName and PasswordHash = @PasswordHash";

              SqlCommand Command2 = new SqlCommand(query, Connection);
             Command2.Parameters.AddWithValue("@UserName", UserName);
          Command2.Parameters.AddWithValue("@PasswordHash", PasswordHash);
           
                
                SqlDataReader Reader = Command2.ExecuteReader();
                if (Reader.Read())
                {
                    isFound = true;
                    UserID = (int)Reader["UserID"];
                    PersonID = (int)Reader["PersonID"];
                    IsActive = (bool)Reader["IsActive"];
                }
                else
                {
                    isFound = false;
                }
                Reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {

                Connection.Close();
            }
            return isFound;

        }
        public static int AddNewUser(int PersonID, string UserName, string PasswordHash, 
            string Salt, bool IsActive)
        {
            int UserID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);

            SqlCommand Command = new SqlCommand("SP_AddNewUser", Connection);
            Command.CommandType = CommandType.StoredProcedure;
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
            Command.Parameters.AddWithValue("@Salt", Salt);
            Command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }

            } catch (Exception ex)
            {
                //
            }

            finally
            {
                Connection.Close();

            }
            return UserID;

        }
    
        public static bool UpdateUser(int UserID, int PersonID, string UserName,
                                      string PasswordHash, string Salt, bool IsActive)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);

            SqlCommand command = new SqlCommand("SP_UpdateUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
            command.Parameters.AddWithValue("@Salt", Salt);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteUser(int UserID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);

            string query = @"Delete Users 
                                where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static DataTable GetAllUsers()
        {

            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"Select Users.UserID,
                                    Users.PersonID,
  FullName= People.FirstName+' '+People.SecondName+' '+ People.ThirdName+' '+People.LastName,
	                                Users.UserName,
	                                Users.IsActive
                                    From Users inner join 
                                    People on  Users.PersonID= People.PersonID";

            SqlCommand Command = new SqlCommand(query, Connection);
            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                //
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }

        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);

            string query = "SELECT Found=1 FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);

            string query = "SELECT Found=1 FROM Users WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = "Select Found = 1 from Users where PersonID = PersonID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                isFound = Reader.HasRows;

                Reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                Connection.Close();
            }

            return isFound;
        }

        public static bool ChangePassword(int UserID, string PasswordHash)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);

            string query = @"Update  Users  
                            set PasswordHash = @PasswordHash
                            where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


    }

}
