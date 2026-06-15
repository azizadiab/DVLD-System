using System;
using System.Linq;
using System.Data;
using DVLD_DataAccess;


namespace DVLD_Buisness
{
    public class clsPerson
    {

     public enum eMode { Addnew = 0, Update = 1};
     public eMode Mode = eMode.Addnew ;
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }
        }
        
        public DateTime DateOfBirth { get; set; }
        public short Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }

        public clsCountry CountryInfo;

        private string _ImagePath;
        public string ImagePath 
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

        private clsPerson(int ID, string NationalNo, string FirstName, string SecondName,
        string ThirdName, string LastName, DateTime DateOfBirth, short Gendor,
        string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            this.PersonID = ID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Phone = Phone;
            this.Email = Email;
            this.Address = Address;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountry.Find(NationalityCountryID);
            
            Mode = eMode.Update;
        }

        public clsPerson()
        {
            this.PersonID = -1;
            //this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
         //this.Gendor = 1;
            this.Phone = "";
            this.Email = "";
            this.Address = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";
            Mode = eMode.Addnew;
        }

     
        private bool _AddNewPerson()
        {
            //call DataAccess Layer 
            this.PersonID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName, 
             this.SecondName, ThirdName, this.LastName, this.DateOfBirth, this.Gendor,
             this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
            return (this.PersonID != -1);

        }
       
        private bool _UpdatePerson()
        {
            //call DataAccess Layer 
            return clsPersonData.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName,
          this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor,
         this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);

        }

        public bool Save()
        {
            switch (Mode)
            {
                case eMode.Addnew:
                    if (_AddNewPerson())
                    {
                        Mode = eMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case eMode.Update:
                    return _UpdatePerson();

            }
            return false;
        }



        public static clsPerson Find(int ID)
        {
            string NationalNo = "", FirstName = " ", SecondName = "", ThirdName = "",
                LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";

            short Gendor = 0;
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;

            bool IsFound = clsPersonData.GetPersonInfoByID(ID, ref NationalNo, ref FirstName, ref SecondName,
            ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone,
            ref Email, ref NationalityCountryID, ref ImagePath);
            
            if (IsFound)
            {
                //we return new object of that person with the right data
                return new clsPerson(ID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                    DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);

            }
            else
            {
                return null;
            }

        }

        public static clsPerson Find(string NationalNo)

        {
           
            string FirstName = " ", SecondName = "", ThirdName = "",
                LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";

            short Gendor = 0;
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1, ID = -1;

            bool IsFound = clsPersonData.GetPersonInfoByNationalNo(ref ID, NationalNo, ref FirstName, 
                ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor, ref Address,
                ref Phone,ref Email, ref NationalityCountryID, ref ImagePath);
            if(IsFound)
            {
                
                    return new clsPerson(ID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                        DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }

        }

        public static DataTable GetPeople()
        {
            return clsPersonData.GetAllPeople();

        }
        public static bool DelectePerson(int Id)
        {
            return clsPersonData.DeletPerson(Id);
        }

        public static bool isPersonExist(int ID)
        {
            return clsPersonData.IsPersonExist(ID);
        }

        public static bool isPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }

    }
}
