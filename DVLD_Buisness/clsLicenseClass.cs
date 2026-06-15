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
    public class clsLicenseClass
    {
       public enum enMode { AddNew = 0, Update = 1 }
       public enMode Mode = enMode.AddNew;

       public int LicenseClassID { get; set; }
       public string ClassName { get; set; }
       public string ClassDescription { get; set; }
       public byte MinimumAllowedAge { get; set; }
       public byte DefaultValidityLength { get; set; }
       public float ClassFees { get; set; }

        public clsLicenseClass()
        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;
            Mode = enMode.AddNew;

        }

        public clsLicenseClass(int LicenseClassID, string ClassName, string ClassDescription,
                                         byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFee)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFee;
            Mode = enMode.Update;

        }
        public static clsLicenseClass FindByID(int LicenseClassID)
        {
            string ClassName = "", ClassDescription = "";
           byte MinimumAllowedAge = 18, DefaultValidityLength = 10;
           float ClassFee = 0;
           if(clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref ClassName, ref ClassDescription,
                                         ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFee))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFee);

            }else

            {
                return null;
            }
            
        }

        private bool _AddNewLicenseClass()
        {
            this.LicenseClassID = clsLicenseClassData.AddNewLicenseClass(this.ClassName, this.ClassDescription,
                                                      this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
            return (this.LicenseClassID != -1);



        }

        private  bool _UpdateLicenseClass()
        {
            return clsLicenseClassData.UpdateLicenseClass(this.LicenseClassID, this.ClassName, this.ClassDescription,
                                                      this.MinimumAllowedAge, this.DefaultValidityLength, this. ClassFees);
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = ""; string ClassDescription = "";
            byte MinimumAllowedAge = 18, DefaultValidityLength = 10;
            float ClassFee = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref ClassName, ref ClassDescription,
                                                   ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFee))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                                                    MinimumAllowedAge, DefaultValidityLength, ClassFee);
            }
            else
            {
                return null;
            }
        }

        public static clsLicenseClass FindByName(string ClassName)
        {
            int LicenseClassID = -1; string ClassDescription = "";
            byte MinimumAllowedAge = 18, DefaultValidityLength = 10;
            float ClassFee = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, ref  ClassDescription,
                                                   ref MinimumAllowedAge, ref  DefaultValidityLength, ref ClassFee))
            {
                return new clsLicenseClass(LicenseClassID, ClassName,  ClassDescription,
                                                    MinimumAllowedAge, DefaultValidityLength, ClassFee);
            }else
            {
                return null;
            }
        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewLicenseClass())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateLicenseClass();
            }
            return false;
        }

    }
}