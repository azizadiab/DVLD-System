using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplicationTypeData
    {

     public static bool GetApplicationTypesByID( int ApplicationTypeID, ref string Title, ref decimal Fees)
        {

            bool isFound = false;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))


            using (SqlCommand Command = new SqlCommand("SP_GetApplicationTypesByID", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
              
                try
                {
                    Connection.Open();
                    using(SqlDataReader Reader= Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            isFound = true;

                            Title = (string)Reader["ApplicationTypeTitle"];
                            Fees = Convert.ToDecimal(Reader["ApplicationFees"]);
                        }
                        
                    }

                }catch(Exception ex)
                {
                    throw new Exception("Error occurred while retrieving the application type", ex);
                   
                }
                return isFound;
            }

        }


     public static DataTable GetAllApplicationTypes()
        {
            DataTable deApplications = new DataTable();
            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            
           
            using (SqlCommand Command = new SqlCommand("SP_GetAllApplicationTypes", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                try
                {

                   Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {

                        if (Reader.HasRows)
                        {
                            deApplications.Load(Reader);

                        }

                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while retrieving application types.", ex);
                }
              
                return deApplications;
            }

        }


      public static int AddNewApplicationType(string Title, decimal Fees)
        {
            int ApplicationTypeID = 0;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand Command = new SqlCommand("SP_AddNewApplicationType", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                Command.Parameters.AddWithValue("@ApplicationFees", Fees);

                try
                {

                    Connection.Open();
                    object Result = Command.ExecuteScalar();
                    if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                    {
                        ApplicationTypeID = insertedID;
                    }

                }catch(Exception ex)
                {

                    throw new Exception("Error occurred while retrieving Adding", ex);
                }

            }
            return ApplicationTypeID ;
        }



        public static bool UpdateApplicationTypes(int ApplicationTypeID, string Title, decimal Fees)
        {

            int rowsAffected = 0;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))


            using (SqlCommand Command = new SqlCommand("SP_UpdateApplicationTypes", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                Command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                Command.Parameters.AddWithValue("@ApplicationFees", Fees);

                try
                {
                    Connection.Open();
                    rowsAffected  = Command.ExecuteNonQuery();
                    

                }catch(Exception ex)
                {
                    return false;

                    throw new Exception("An Error ocurred whlie Update Application Types. ", ex);
                    

                }

            }

            return (rowsAffected > 0);
        }
    }

    }





