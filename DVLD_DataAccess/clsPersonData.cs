using System;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Security.Policy;
using System.Runtime.Remoting.Messaging;

namespace DVLD_DataAccess
{
    public class clsPersonData
    {
        public static bool GetPersonInfoByID(int ID, ref string NationalNo, ref string FirstName,
         ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
         ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID,
         ref string ImagePath)

        {
            bool IsFound = false;
            SqlConnection Connaction = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = "Select * From People where PersonID= @PersonID";
            SqlCommand Command = new SqlCommand(query, Connaction);
            Command.Parameters.AddWithValue("@PersonID", ID);
            try
            {
                Connaction.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    NationalNo = (string)reader["NationalNo"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    //ThirdName: allows null in database so we should handle null
                    if (reader["ThirdName"]!= DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }
                    
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (Byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    //Email: allows null in database so we should handle null
                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }
                   
                    NationalityCountryID = (int)reader["NationalityCountryID"];


                     //ImagePath: allows null in database so we should handle null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }
                }
                else
                {
                    // The record was not found
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

                IsFound = false;
            }
            finally
            {
                Connaction.Close();
            }
            return IsFound;
        }



        public static bool GetPersonInfoByNationalNo(ref int ID, string NationalNo, ref string FirstName,
         ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
         ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID,
         ref string ImagePath)

        {
            bool IsFound = false;
            SqlConnection Connaction = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = "Select * From People where NationalNo = @NationalNo";
            SqlCommand Command = new SqlCommand(query, Connaction);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            try
            {
                Connaction.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.Read())
                {
                    // The record was found
                    IsFound = true;
                    ID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    //ThirdName: allows null in database so we should handle null
                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }

                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (Byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    //Email: allows null in database so we should handle null
                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }

                    NationalityCountryID = (int)reader["NationalityCountryID"];


                    //ImagePath: allows null in database so we should handle null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }
                }
                else
                {
                    // The record was not found
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

                IsFound = false;
            }
            finally
            {
                Connaction.Close();
            }
            return IsFound;
        }


        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
               DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email,
                int NationalityCountryID, string ImagePath)
        {
            //this function will return the new person id if succeeded and -1 if not.
            int ID = -1;
            SqlConnection Connaction = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth,
                Gendor,Address,Phone,Email,NationalityCountryID,ImagePath) 
                 VALUES  (@NationalNo,@FirstName,@SecondName,@ThirdName,@LastName,@DateOfBirth,@Gendor,@Address,@Phone,@Email,@NationalityCountryID,@ImagePath)
                 SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connaction);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "" && ThirdName != null)
            {
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else
            {
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
            }
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
            {
                Command.Parameters.AddWithValue("@Email", Email);
            }
            else
            {
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            }

            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);


            if (ImagePath != "" && ImagePath != null)
            {
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }


            try
            {
                Connaction.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    ID = insertedID;

                }

            }
            catch (Exception ex)
            {
                //
            }
            finally
            {
                Connaction.Close();
            }
            return ID;
        }


        public static bool UpdatePerson(int ID, string NationalNo, string FirstName, string SecondName,
      string ThirdName, string LastName,
      DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email,
     int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;
            SqlConnection Connaction = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"UPDATE People SET
                          
                          NationalNo=@NationalNo,
                          FirstName=@FirstName,
                          SecondName=@SecondName,
                          ThirdName=@ThirdName,
                          LastName=@LastName,
                          DateOfBirth=@DateOfBirth,
                          Gendor=@Gendor,
                          Address=@Address,
                          Phone=@Phone,
                          Email=@Email,
                          NationalityCountryID=@NationalityCountryID,
                          ImagePath=@ImagePath
                           where PersonID=@PersonID";

            SqlCommand Command = new SqlCommand(query, Connaction);
            Command.Parameters.AddWithValue("@PersonID", ID);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            if(ThirdName !=""&& ThirdName !=null)
            {
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else
            {
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
            }
           
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);

            if(Email != "" && Email !=null)
            {
                Command.Parameters.AddWithValue("@Email", Email);
            }
            else
            {
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            }
          
            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if(ImagePath !="" && ImagePath  !=null)
            {
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }
           

            try
            {
                Connaction.Open();
                rowsAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // return false;
            }
            finally
            {
                Connaction.Close();
            }
            return (rowsAffected > 0);
        }


        public static bool IsPersonExist(int ID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = "select Found=1 from People where PersonID = @PersonID ";
            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@PersonID ", ID);
            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsFound = reader.HasRows;
                reader.Close();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;

        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = "select Found=1 from People where NationalNo= @NationalNo";
            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsFound = reader.HasRows;
                reader.Close();
                     
            }
            catch(Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }
            return  IsFound;

        }
      
       
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            SqlConnection Connaction = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"Select People.PersonID, People.NationalNo, People.FirstName, People.SecondName,People.ThirdName,
                   People.LastName, People.Gendor,
	               case
	               when People.Gendor= 0 then 'male'
	               ELSE 'Female'
	               End as GendorCaption ,
	               people.Address, People.Email, People.Phone, People.DateOfBirth, People.ImagePath,
	               People.NationalityCountryID, Countries.CountryName
	               from People  INNER JOIN Countries on People.NationalityCountryID= Countries.CountryID
                    order by People.FirstName";
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

        public static bool DeletPerson(int ID)

        {
            int RorwEffect = 0;
            SqlConnection Connaction = new SqlConnection(clsDataAccessStrings.connectionString);
            string query = @"delete People where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(query, Connaction);
            Command.Parameters.AddWithValue("@PersonID", ID);
            try
            {
                Connaction.Open();
                RorwEffect = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connaction.Close();
            }
            return (RorwEffect > 0);
        }

   
    }

}