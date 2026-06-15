using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLicenseClassData
    {
     
        
        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref string ClassName, ref string ClassDescription,
                                                    ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand Command = new SqlCommand("SP_GetLicenseClassInfoByID", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                try
                {
                    Connection.Open();

                    using(SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if(Reader.Read())
                        {
                            isFound = true;
                            ClassName = (string)Reader["ClassName"];
                            ClassDescription = (string)Reader["ClassDescription"];
                            MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                            ClassFees = (float)Reader["ClassFees"];
                        }else
                        {
                            isFound = false;
                        }
                    }


                }catch(Exception ex)
                {
                    //throw new Exception("Error occurred while Get the License class", ex);
                }

            }
            return isFound;

        }

        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID,  ref string ClassDescription,
                                                   ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand Command = new SqlCommand("SP_GetLicenseClassInfoByClassName", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ClassName", ClassName);
                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            isFound = true;
                            LicenseClassID = (int)Reader["LicenseClassID"];
                            ClassDescription = (string)Reader["ClassDescription"];
                            MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                            ClassFees = (float)Reader["ClassFees"];
                        }
                        else
                        {
                            isFound = false;
                        }
                    }


                }
                catch (Exception ex)
                {
                    //
                }

            }
            return isFound;

        }

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();


            using (SqlConnection Connection= new SqlConnection(clsDataAccessStrings.connectionString))
                using(SqlCommand Command= new SqlCommand("SP_GetAllLicenseClasses", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                try
                {
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            dt.Load(Reader);
                        }

                    }
                    

                }catch (Exception ex)
                {
                    throw new Exception("Error occurred while retrieving the License class", ex);
                }

                return dt;
            }
        }

        public static int AddNewLicenseClass( string ClassName, string ClassDescription,
                                                     byte MinimumAllowedAge,  byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand Command = new SqlCommand("SP_AddNewLicenseClass", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ClassName", ClassName);
                Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                Command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                Command.Parameters.AddWithValue("@ClassFees", ClassFees);
                var outputIdParam = new SqlParameter("@@NewLicenseClassID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                Command.Parameters.Add(outputIdParam);

                try
                {
                    Connection.Open();
                     Command.ExecuteNonQuery();
                    if(outputIdParam.Value != DBNull.Value)
                    {
                        LicenseClassID = Convert.ToInt32(outputIdParam.Value);

                    }

                }catch(Exception ex)
                {

                    throw new Exception("Error occurred while Add New License class", ex);
                }

                return LicenseClassID;
            }
            
        }


        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, byte MinimumAllowedAge,
                                                          byte DefaultValidityLength, float ClassFees)
        {
            int rowsAffected = 0;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand Command = new SqlCommand("SP_UpdateLicenseClass", Connection))
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);
                Command.Parameters.AddWithValue("ClassName", ClassName);
                Command.Parameters.AddWithValue("MinimumAllowedAge", MinimumAllowedAge);
                Command.Parameters.AddWithValue("DefaultValidityLength", DefaultValidityLength);
                Command.Parameters.AddWithValue("ClassFees", ClassFees);
                try
                {
                    Connection.Open();
                    rowsAffected = Command.ExecuteNonQuery();


                }catch(Exception ex)
                {
                    throw new Exception("Error occurred while Update a License class", ex);

                }

                return rowsAffected > 0;
            }


        }

    }
}
