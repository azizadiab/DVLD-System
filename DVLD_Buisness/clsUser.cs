using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DVLD_Security;

namespace DVLD_Buisness
{
 public class clsUser
    {
      public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;
     
       public int UserID { get; set; }
        public clsPerson PersonInfo { get; set; }
        public int PersonID { get; set; }
        public string  UserName  { get; set; }
        public string Password { get; set; }
        public string  PasswordHash  { get; set; }
        public string  Salt  { get; set; }
        public bool    IsActive    { get; set; }


        public clsUser()
        {
            this.UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.Salt = "";
            this.IsActive = true;
            Mode = enMode.AddNew;

        }


        private clsUser(int UserID, int PersonID, string UserName, string Password, string Salt, bool IsActive)
        {

            this.UserID = UserID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.UserName = UserName;
            this.PasswordHash = Password;
            this.Salt = Salt;
            this.IsActive = IsActive;
            Mode = enMode.Update;

        }

 

        public static clsUser FindByPersonID(int UserID)
        {
            int PersonID = -1;
            bool IsActive = false;
            string UserName = "", PasswordHash = "", Salt="";
            bool IsFound = clsUesrData.GetUserInfoByUserID(UserID, ref PersonID, ref UserName,
                           ref PasswordHash,ref Salt,  ref IsActive);

           if(IsFound)
            {
                //we return new object of that User with the right data
                return new clsUser(UserID, PersonID,  UserName, PasswordHash, Salt,  IsActive);

            }else
            {
                return null;
            }
            
        }

        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = -1;
            bool IsActive = false;
            string UserName = "", PasswordHash = "", Salt="";
            bool IsFound = clsUesrData.GetUserInfoByUserID(UserID, ref PersonID, 
                                ref UserName, ref PasswordHash, ref Salt, ref IsActive);

            if (IsFound)
            {
                //we return new object of that User with the right data
                return new clsUser(UserID, PersonID, UserName, PasswordHash, Salt, IsActive);

            }
            else
            {
                return null;
            }

        }

        public static clsUser FindByUsernameAndPassword(string UserName, string Password)
        {
            int PersonID = -1, UserID = -1;
            bool IsActive = false;
          
            bool IsFound = clsUesrData.GetUserInfoByUsernameAndPassword(ref UserID, ref PersonID,
                           UserName, Password, ref IsActive);

            if (IsFound)
            {

               string Salt = clsSecurity.GenerateSalt();
                String PasswordHash = clsSecurity.Has256Password(Password, Salt);
                return new clsUser(UserID, PersonID, UserName, Password, Salt, IsActive);

            }
            else
            {
                return null;
            }

        }

        private bool _AddNewUser()
        {
            //call DataAccess Layer         
            this.UserID = clsUesrData.AddNewUser(this.PersonID, this.UserName,
                                              this.PasswordHash, this.Salt, this.IsActive);

            return (this.UserID != -1);
                
        }

        private bool _UpdateUser()
        {
            //call DataAccess Layer 
            return clsUesrData.UpdateUser(this.UserID, this.PersonID, this.UserName,
                                              this.PasswordHash, this.Salt, this.IsActive);
        }

        public static bool DeleteUser(int ID)
        {
            return clsUesrData.DeleteUser(ID);
        }

        public bool Save()
        {
         switch(Mode)
            {
                case enMode.AddNew:
                    this.Salt = clsSecurity.GenerateSalt();
                    this.PasswordHash = clsSecurity.Has256Password(this.Password, this.Salt);
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update; 
                        return true;
                       
                    }else
                    {
                        return false;
                    }
                case enMode.Update:
                  
                    return _UpdateUser();
      
            }
            return false;
        }

        public static DataTable GetAllUsers()
        {

            return clsUesrData.GetAllUsers();


        }

        public static bool IsUserExist(int UserID)
        {

            return clsUesrData.IsUserExist(UserID);
        }

        public static bool IsUserExist(string UserName)
        {

            return clsUesrData.IsUserExist(UserName);
        }

        public static bool isUserExistForPersonID(int PersonID)
        {
            return clsUesrData.IsUserExistForPersonID(PersonID);

        }

       
    }
}
