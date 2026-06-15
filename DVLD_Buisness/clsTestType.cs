using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsTestType
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode = enMode.AddNew;

        public enum enTestType { VisionTest = 1,  WrittenTest = 2, StreetTest = 3 }

        public clsTestType.enTestType ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fees { get; set; }


        public clsTestType()
        {
            this.ID = clsTestType.enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
            Mode = enMode.AddNew;
        }

        public clsTestType(clsTestType.enTestType ID, string Title, string Description, decimal Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.Fees = Fees;
            Mode = enMode.Update;

        }

        public  bool _AddNewTestType()
        {
            this.ID = (clsTestType.enTestType)clsTestTypeData.AddNewTestType(this.Title, this.Description, this.Fees);
            return (this.Title != "");


        }

        public  bool _UpdateTestType()
        {

            return clsTestTypeData.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees);

        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewTestType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }else
                    {
                        return false;
                    }

                case enMode.Update:
                        return _UpdateTestType();
                            
                        }
            return false;

        }

        public static clsTestType Find(clsTestType.enTestType ID)
        {

            string Title = "", Description = "";
            decimal Fees = 0;
            if(clsTestTypeData.GetTestTypeByID((int)ID, ref Title,  ref Description,  ref Fees))
            {

                return new clsTestType(ID,  Title,  Description,  Fees);

            }else
            {
                return null;
            }
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();

        }


    }
}
