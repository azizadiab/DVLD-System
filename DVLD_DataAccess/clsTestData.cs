using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestData
    {
        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID, ref bool TestResult,
                                                ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;
            string query = "Select * From Tests Where TestID= @TestID";
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@TestID", TestID);

                try
                {

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];
                            if (reader["Notes"] == DBNull.Value)
                            {
                                Notes = "";
                            }
                            else
                            {
                                Notes = (string)reader["Notes"];
                            }
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                        }
                        else
                        {
                            isFound = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }
            return isFound;
        }
        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int TestTypeID, int LicenseClassID,
                                                       ref int TestID, ref int TestAppointmentID, ref bool TestResult,
                                                       ref string Notes, ref int CreatedByUserID)
        {

            bool isFound = false;
            string query = @"Select top 1 Tests.TestID,Tests.TestAppointmentID, Tests.TestResult, Tests.Notes,
                Tests.CreatedByUserID, Applications.ApplicantPersonID
                from LocalDrivingLicenseApplications inner join Tests inner join 
                TestAppointments on Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                On LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=TestAppointments.LocalDrivingLicenseApplicationID
                inner join Applications on LocalDrivingLicenseApplications.ApplicationID=Applications.ApplicationID
                Where (Applications.ApplicantPersonID = @PersonID)
                And (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                And (TestAppointments.TestTypeID = @TestTypeID)
                Order By Tests.TestAppointmentID DESc";

            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    cmd.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            TestID = (int)reader["TestID"];
                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];
                            if (reader["Notes"] == DBNull.Value)
                            {
                                Notes = "";
                            }
                            else
                            {
                                Notes = (string)reader["Notes"];
                            }
                            CreatedByUserID = (int)reader["CreatedByUserID"];

                        }
                        else
                        {
                            isFound = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;

        }

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();
            string query = "Select *  From Tests order by TestID";
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
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
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return dt;

        }

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {

            int TestID = -1;

            string query = @"Insert into Tests (TestAppointmentID,TestResult, Notes, CreatedByUserID)
                              Values(@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
                                UPDATE TestAppointments 
                                SET IsLocked=1 where TestAppointmentID = @TestAppointmentID;
                               Select SCOPE_IDENTITY()";
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                cmd.Parameters.AddWithValue("@TestResult", TestResult);

                if (Notes != "" && Notes != null)
                    cmd.Parameters.AddWithValue("@Notes", Notes);
                else
                    cmd.Parameters.AddWithValue("@Notes", System.DBNull.Value);
               
                cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        TestID = insertedID;
                }
                catch (Exception ex)
                {
                    throw;

                }

                return TestID;
            }
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;
            string query = @"Update Tests Set 
                          TestAppointmentID = @TestAppointmentID,
                           TestResult = @TestResult,
                           Notes = @Notes, 
                           CreatedByUserID= @CreatedByUserID
                            Where TestID = @TestID";
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@TestID", TestID);
                    cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    cmd.Parameters.AddWithValue("@TestResult", TestResult);
                    cmd.Parameters.AddWithValue("@Notes", Notes);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    return false;
                }

                return rowsAffected > 0;
            }

        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            string query = @"SELECT PassedTestCount = count(TestTypeID)
                         FROM Tests INNER JOIN
                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
						 where LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID and TestResult=1";

            byte PassedTestCount = 0;
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                try
                {
                    con.Open();
                    Object result = cmd.ExecuteScalar();
                   {
                        if (result !=null  && Byte.TryParse(result.ToString(), out byte ptCount))
                        {
                            PassedTestCount = ptCount;
                        }
                   }
                }catch(Exception ex)
                {
                    throw;
                }
                
            }

            return PassedTestCount;


        }


    }
}
