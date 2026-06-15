using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;

namespace DVLD_DataAccess
{
    public class clsDriverData
    {
        public static bool GetDriverInfoByDriverID(int DriverID,
            ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {

            bool isFound = false;
            string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@DriverID", DriverID);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            PersonID = (int)reader["PersonID"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            CreatedDate = (DateTime)reader["CreatedDate"];
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

        public static bool GetDriverInfoByPersonID(int PersonID,
            ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {

            bool isFound = false;
            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            DriverID = (int)reader["DriverID"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            CreatedDate = (DateTime)reader["CreatedDate"];
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

        public static DataTable GetAllDrivers()
        {

            DataTable dt = new DataTable();
            string query = "SELECT * FROM Drivers_View order by FullName";
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;

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
                    throw;
                }
               
            }

            return dt;


        }

        public static int AddNewDriver(int PersonID,  int CreatedByUserID)
        {
            int DriverID = -1;
            string query= @"Insert Into Drivers (PersonID,CreatedByUserID,CreatedDate)
                            Values (@PersonID,@CreatedByUserID,@CreatedDate) ;
                             SELECT SCOPE_IDENTITY(); ";

            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PersonID", PersonID);
                cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
               

                try
                {

                con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) 
                     DriverID = insertedID;

                }catch(Exception ex)
                {
                    throw;
                }

                return DriverID;
            }

            

        }

        public static bool UpdateDriver(int DriverID, int PersonID,
              int CreatedByUserID)
        {
            int rowsAffected = 0;
            string query = @"Update Drivers Set PersonID = @PersonID,
                                  CreatedByUserID = @CreatedByUserID,
                                   Where DriverID = @DriverID";
                                
            using (SqlConnection con = new SqlConnection(clsDataAccessStrings.connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@DriverID", DriverID);
                cmd.Parameters.AddWithValue("@PersonID", PersonID);
                cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                  
                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }

                return rowsAffected > 0;
            }


        }
    }
}
