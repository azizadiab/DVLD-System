using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Net.NetworkInformation;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {

        public static bool GetTestAppointmentByID(int TestAppointmentID,
            ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked,
            ref int RetakeTestApplicationID)
        {
            bool isFound = false;
            
            using(SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using(SqlCommand cmd = new SqlCommand("SP_Get_TestAppointment_ByID", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            TestTypeID = (int)reader["TestTypeID"];
                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees =Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = (bool)reader["IsLocked"];
                            if (reader["RetakeTestApplicationID"] == DBNull.Value)
                            {
                                RetakeTestApplicationID = -1;
                            }else
                            RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];

                        }else
                        {
                            isFound = false;
                        }
                    }

                }catch(Exception ex)
                {
                    isFound = false;

                }
            }

            return isFound;

        }

        public static bool GetLastTestAppointment( int LocalDrivingLicenseApplicationID,
             int TestTypeID,  ref int LastTestAppointmentID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked,
            ref int RetakeTestApplicationID)
        {
            bool isFound = false;
          
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetLastTestAppointment", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                // TODO: Add Parameters
                try
                {

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            //TestTypeID = (int)reader["TestTypeID"];
                            //LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            LastTestAppointmentID = (int)reader["TestAppointmentID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = (bool)reader["IsLocked"];
                            if (reader["RetakeTestApplicationID"] == DBNull.Value)
                            {
                                RetakeTestApplicationID = -1;
                            }
                            else
                                RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];

                        }
                        else
                        {
                            isFound = false;
                        }

                    }
                }catch(Exception ex)
                {
                    isFound = false;
                }

                }
            return isFound;


        }


        public static DataTable GetAllTestAppointments()
        {

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Get_AllTestAppointments", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }catch(Exception ex)
                {
                    return null;
                }

            }

            return dt;

        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID,
         int TestTypeID)
            {


            DataTable dt = new DataTable();


            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Get_ApplicationTestAppointmentsPerTestType", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                       
                            dt.Load(reader);
                        
                    }


                }catch(Exception ex)
                {
                    return null;
                }                
            }

            return dt;


        }

        public static int AddTestPoinppt(
             int TestTypeID,  int LocalDrivingLicenseApplicationID,
             DateTime AppointmentDate,  float PaidFees,  int CreatedByUserID,  bool IsLocked,
             int RetakeTestApplicationID)
        {

            int newTestId = -1;
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Add_TestAppointment", con))
            {
                              
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                cmd.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
                cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                cmd.Parameters.AddWithValue("@IsLocked", IsLocked);
                cmd.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

                var outputIdParam = new SqlParameter("@newTestAppointment", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                try
                {

                    con.Open();

                    cmd.ExecuteNonQuery();
                    if (outputIdParam.Value != DBNull.Value)
                    {
                        newTestId = Convert.ToInt32(outputIdParam.Value);
                    }
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
            return newTestId;

        }


        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
             DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool IsLocked,
             int RetakeTestApplicationID)
        {
            int rowEffect = 0;
            
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Update_Table", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
                cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                cmd.Parameters.AddWithValue("@IsLocked", IsLocked);
                cmd.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

              

                try
                {

                    con.Open();
                    rowEffect = cmd.ExecuteNonQuery();
                   
                }catch(Exception ex)
                {
                    return false;
                }
            }
            return rowEffect > 0;
        }


        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Get_TestID", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {

                    con.Open();

                    object result = cmd.ExecuteScalar();

                   if (result != DBNull.Value  && int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestID = insertedID;
                    }                     
                    
                }catch(Exception ex)
                {
                   throw;
                }
            }

            return TestID;

        }

    }


}
