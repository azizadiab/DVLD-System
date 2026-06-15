using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    

    public class clsCountryData
    {
    
        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            SqlConnection Connaction = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = "select * from Countries order by CountryName";
            SqlCommand Command = new SqlCommand(query, Connaction);

            try
            {
                Connaction.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }

            finally
            {
                Connaction.Close();
            }
            return dt;
        }

        public static bool GetCountryInfoByID(int ID, ref string CountrName)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = "Select * from Countries where CountryID= @CountryID";
            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@CountryID", ID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                if(Reader.Read())
                {
                    // The record was found
                    IsFound = true;
                  
                    CountrName = (string)Reader["CountryName"];
                }
                else
                {
                    // The record was not found
                    IsFound = false;
                }
                Reader.Close();
            }catch(Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }


        public static bool GetCountryInfoByName(string CountrName, ref int ID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = "Select * from Countries where CountryName= @CountryName";
            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@CountryName", CountrName);
            try
            {
                Connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                if (Reader.Read())
                {
                    // The record was found

                    IsFound = true;

                    ID = (int)Reader["CountryID"];
                }
                else
                {
                    // The record was not found

                    IsFound = false;
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }

    }
}
