using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplicationData
    {

        public static bool GetApplicationInfoByID(int ApplicationID,
            ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID,
            ref byte ApplicationStatus, ref DateTime LastStatusDate,
            ref float PaidFees, ref int CreatedByUserID)
        {

            bool isFound = false;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))


            using (SqlCommand Command = new SqlCommand("SP_GetApplicationInfoByID", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                try
                {
                    Connection.Open();
                    using (SqlDataReader reader = Command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            ApplicantPersonID = (int)reader["ApplicantPersonID"];
                            ApplicationDate = (DateTime)reader["ApplicationDate"];
                            ApplicationTypeID = (int)reader["ApplicationTypeID"];
                            ApplicationStatus = (byte)reader["ApplicationStatus"];
                            LastStatusDate = (DateTime)reader["LastStatusDate"];
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];

                        }

                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred while retrieving the application", ex);

                }
                return isFound;
            }

        }

        public static DataTable GetAllApplications()
        {
            DataTable de = new DataTable();
            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))


            using (SqlCommand Command = new SqlCommand("SP_GetAllApplications", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                try
                {

                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {

                        if (Reader.HasRows)
                        {
                            de.Load(Reader);

                        }

                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while retrieving application.", ex);
                }

                return de;
            }

        }

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int ApplicationID = 0;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand command = new SqlCommand("SP_AddNewApplication", Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
                command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
                command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
                command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
                command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
                command.Parameters.AddWithValue("PaidFees", @PaidFees);
                command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);

                try
                {

                    Connection.Open();
                    object Result = command.ExecuteScalar();
                    if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                    {
                        ApplicationID = insertedID;
                    }

                }
                catch (Exception ex)
                {

                    //throw new Exception("Error occurred while retrieving Adding", ex);
                }

            }
            return ApplicationID;
        }


        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {

            int rowsAffected = 0;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))


            using (SqlCommand command = new SqlCommand("SP_UpdateApplication", Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
                command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
                command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
                command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
                command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
                command.Parameters.AddWithValue("PaidFees", @PaidFees);
                command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);

                try
                {
                    Connection.Open();
                    rowsAffected = command.ExecuteNonQuery();


                }
                catch (Exception ex)
                {
                    return false;

                    throw new Exception("An Error ocurred whlie Update Application Types. ", ex);


                }

            }

            return (rowsAffected > 0);
        }


        public static bool DeleteApplication(int ApplicationID)
        {
            int rowsAffected = 0;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))


            using (SqlCommand command = new SqlCommand("SP_DeleteApplication", Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ApplicationID", ApplicationID);

                try
                {
                    Connection.Open();
                    rowsAffected = command.ExecuteNonQuery();


                }
                catch (Exception ex)
                {

                    throw new Exception("An Error ocurred whlie Delect Application ID. ", ex);

                }

                return (rowsAffected > 0);

            }

        }

        public static bool IsApplicationExist(int ApplicationID)
        {

            bool isFound = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))


            using (SqlCommand command = new SqlCommand("SP_IsApplicationExist", Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                try
                {
                    Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        isFound = reader.HasRows;
                    }



                }
                catch (Exception ex)
                {

                    throw new Exception("An Error ocurred whlie  Fond Application ID. ", ex);

                }

                return isFound;

            }



        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            //incase the ActiveApplication ID !=-1 return true.
            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);

        }

        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);

            string query = @"Select ActiveApplicationID= ApplicationID From Applications
                          where ApplicantPersonID = @ApplicantPersonID and  ApplicationTypeID = @ApplicationTypeID
                          and ApplicationStatus = 1";

            SqlCommand command = new SqlCommand("SP_UpdateApplication", Connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                Connection.Open();

                object Result = command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int AppId))
                {
                    ActiveApplicationID = AppId;
                }

            }
            catch (Exception ex)
            {
                //
                return ActiveApplicationID;
            }
            finally
            {
                Connection.Close();
            }
            return ActiveApplicationID;
        }


        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);

            string query = @"SELECT ActiveApplicationID=Applications.ApplicationID  
                            From
                            Applications INNER JOIN
                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE ApplicantPersonID = @ApplicantPersonID 
                            and ApplicationTypeID=@ApplicationTypeID 
							and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                            and ApplicationStatus=1";

            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                object Result = command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int AppId))
                {
                    ActiveApplicationID = AppId;
                }

            }
            catch (Exception ex)
            {
                //
                return ActiveApplicationID;
            }
            finally
            {
                Connection.Close();
            }
            return ActiveApplicationID;
        }


        public static bool UpdateStatus(int ApplicationTypeID, short NewStatus)
        {
            int rowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"Update  Applications  
                            set 
                                ApplicationStatus = @NewStatus, 
                                LastStatusDate = @LastStatusDate
                            where ApplicationID =@ApplicationID;";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@NewStatus", NewStatus);
            Command.Parameters.AddWithValue("LastStatusDate", DateTime.Now);

            try
            {
                Connection.Open();
                rowsAffected = Command.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                //
                return false;

            }
            finally
            {

                Connection.Close();

            }

            return (rowsAffected > 0);


        }

    }
}
