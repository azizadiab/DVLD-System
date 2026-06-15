using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{

 
    public class clsApplicationType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;



        public int ID { get; set; }

        public string Title { get; set; }
        public  decimal Fees { get; set; }

        public clsApplicationType()
        {

            this.ID = -1;
            this.Title = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }

        public clsApplicationType(int ApplicationTypeID, string ApplicationTypeTitel, decimal ApplicationFees)
        {

            this.ID = ApplicationTypeID;
            this.Title = ApplicationTypeTitel;
            this.Fees = ApplicationFees;
            Mode = enMode.Update;

        }


        public static clsApplicationType Find(int ID)
        {
            string Title = "";
            decimal Fees = 0;

            if( clsApplicationTypeData.GetApplicationTypesByID(ID, ref Title, ref Fees))
            {

                return new clsApplicationType(ID, Title, Fees);
            }

            return null;
        }

        public static DataTable GetAllApplicationTypes()
        {

            return clsApplicationTypeData.GetAllApplicationTypes();
        }


        private bool _AddNewApplicationType()
        {


            this.ID = clsApplicationTypeData.AddNewApplicationType(this.Title, this.Fees);
            return this.ID != 0;
        }

        private bool _UpdateApplicationTypes()
        {

            return clsApplicationTypeData.UpdateApplicationTypes(ID, Title, Fees);


        }

       public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                if (_AddNewApplicationType())
            {
                        Mode = enMode.Update;
                        return true;

                    }else
                    {
                        return false;
                    }

                  case enMode.Update:
                      return _UpdateApplicationTypes();

              }

            return false;
        }

    }
}
