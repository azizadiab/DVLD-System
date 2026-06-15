using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public class clsLocalDrivingLicenseApplicationData
    {
        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID, ref int ApplicationID,
            ref int LicenseClassID)
        {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"select * from LocalDrivingLicenseApplications
                        Where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];

                } else
                {
                    isFound = false;
                }

                reader.Close();

            } catch (Exception ex)
            {
                //
                isFound = false;


            }
            finally
            {

                connection.Close();
            }

            return isFound;
        }

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(
          int ApplicationID, ref int LocalDrivingLicenseApplicationID,
          ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);

            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

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

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"select * from LocalDrivingLicenseApplications_View
                           Order By ApplicationDate Desc";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);

                }

                reader.Close();

            }
            catch (Exception ex)
            {
                //

            }
            finally
            {

                connection.Close();
            }

            return dt;
        }


        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            //this function will return the new person id if succeeded and -1 if not.
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"insert into LocalDrivingLicenseApplications(ApplicationID, LicenseClassID)
                            Values(@ApplicationID, @LicenseClassID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int inserted))
                {
                    LocalDrivingLicenseApplicationID = inserted;
                }
            } catch (Exception ex)
            {
                //
            }
            finally
            {
                connection.Close();
            }
            return LocalDrivingLicenseApplicationID;
        }


        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID,
                                                                 int LicenseClassID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"Update LocalDrivingLicenseApplications
                             Set ApplicationID = @ApplicationID , LicenseClassID= @LicenseClassID
                             Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                rowsAffected = command.ExecuteNonQuery();
            } catch (Exception ex)
            {

                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);
        }


        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)

        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"Delete From LocalDrivingLicenseApplications                       
                             Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


            try
            {
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);

        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool isFound = false;

            string query = @" SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";

            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    con.Open();
                    Object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        isFound = true;
                    }

                }
                catch (Exception ex)
                {
                    isFound = false;

                }

            }
            return isFound;
        }


        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            Byte TotalTrialsPerTest = 0;

            string query = @" SELECT TotalTrialsPerTest = count(TestID)
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";

            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    con.Open();
                    object result = cmd.ExecuteNonQuery();

                    if (result != null && byte.TryParse( result.ToString(), out byte Trials))
                    {
                        TotalTrialsPerTest = Trials;
                    }

                }
                catch (Exception ex)
                {
                    //

                }

            }
            return (byte)TotalTrialsPerTest;


        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;

            string query = @"SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)  
                            AND(TestAppointments.TestTypeID = @TestTypeID) and isLocked=0
                            ORDER BY TestAppointments.TestAppointmentID desc";
                 

            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    con.Open();
                    Object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        Result = true;
                    }

                }
                catch (Exception ex)
                {
                    Result = false;

                }

            }
            return Result;


        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result= false;

            string query = @" SELECT top 1 TestResult
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";

            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    con.Open();
                    Object result = cmd.ExecuteScalar();
                    if (result != null && bool.TryParse(result.ToString(), out bool returnedResult))
                    {
                        Result = returnedResult;
                    }

                }
                catch (Exception ex)
                {
                   //

                }

            }
            return Result;
        }


    }

    }

