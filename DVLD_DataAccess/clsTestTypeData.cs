using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {

        public static bool GetTestTypeByID(int TestTypeID, ref string TestTypeTitle,
                                           ref string TestTypeDescription, ref decimal TestTypeFees)
        {
            bool isFound = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand Command = new SqlCommand("SP_GetTestTypeInfoByID", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            isFound = true;

                            TestTypeTitle = (string)Reader["TestTypeTitle"];
                            TestTypeDescription = (string)Reader["TestTypeDescription"];
                            TestTypeFees = (decimal)Reader["TestTypeFees"];
                        }
                        else
                        {
                            isFound = false;
                        }

                    }

                } catch (Exception ex)
                {

                    throw new Exception("An Error occurred While retrieving Test Types", ex);

                }

                return isFound;

            }

        }

        public static DataTable GetAllTestTypes()
        {

            DataTable dt = new DataTable();

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand Command = new SqlCommand("SP_GetALLTestTypes", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;

                try
                {
                    Connection.Open();
                

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            dt.Load(Reader);
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("An Error occurred While retrieving Test Types.", ex);

                }
                return dt;


            }


        }

        public static int AddNewTestType(string TestTypeTitle, string TestTypeDescription,
                                           decimal TestTypeFees)
        {

            int TestTypeID = -1;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand Command = new SqlCommand("SP_AddNewTestType", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
                Command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
                Command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
                try
                {
                    Connection.Open();
                    object Result = Command.ExecuteScalar();
                    if (Result != null && int.TryParse(Result.ToString(), out int insertID))
                    {
                        TestTypeID = insertID;
                    }

                }catch(Exception ex)
                {
                    throw new Exception("An Error occurred While Add New Test Type", ex);

                }
          
            }

            return TestTypeID;
        }

        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle,
                                         string TestTypeDescription, decimal TestTypeFees)
        {
            int RowEffect = 0;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand Command = new SqlCommand("SP_UpdateTestType", Connection))
            {

                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
                Command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
                Command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);

                try
                {
                    Connection.Open();
                    RowEffect = Command.ExecuteNonQuery();
                  
                }catch(Exception ex)
                {
                    throw new Exception("An Error occurred While Add New Test Type", ex);

                }

            }

            return (RowEffect > 0);

        }

    }
}
