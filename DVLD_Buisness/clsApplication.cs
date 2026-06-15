using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Buisness.clsApplication;

namespace DVLD_Buisness
{
    public class clsApplication
    {

      public  enum enMode { AddNew=0, Update=1}
        public enMode Mode = enMode.AddNew;
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        }

      public enum enApplicationStatus { New = 1, Cancelled = 2, Completed =3}
        public int ApplicationID  { get; set; }
        
        public int ApplicantPersonID { get; set; }

        public clsPerson PersonInfo { get; set; }
        public string ApplicationFullName
        {
            get
            {

                return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }

        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }

        public clsApplicationType ApplicationTypeInfo;
        public enApplicationStatus ApplicationStatus { get; set; }

        public string StatusText
        {
            get
            {
                switch(ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                       
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }          
            }
        }

        public DateTime LastStatusDate  { get; set; }
        public float PaidFees   { get; set; }
        public int CreatedByUserID   { get; set; }

        public clsUser CreateByUserInfo;


        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }


        public clsApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate,
                       int ApplicationTypeID, enApplicationStatus ApplicationStatus,
                        DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.PersonInfo = clsPerson.Find(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreateByUserInfo = clsUser.FindByPersonID(CreatedByUserID);

            Mode = enMode.Update;

        }

        private bool _AddNewApplicationddNew()
        {
            //call DataAccess Layer 

            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
             (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            return (this.ApplicationID != -1);

        }

        private bool _UpdateApplication()

        {
            //call DataAccess Layer 

            return clsApplicationData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
             (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewApplicationddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                        
                    }else

                    { 
                        return false;
                    }

                case enMode.Update:
                            return _UpdateApplication();

                        }
            return false;
        }

        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID = -1, ApplicationTypeID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            byte ApplicationStatus = 1;
            float PaidFees = 0;

           bool isFound =  clsApplicationData.GetApplicationInfoByID(ApplicationID,
             ref ApplicantPersonID, ref ApplicationDate, ref ApplicationTypeID,
             ref ApplicationStatus, ref LastStatusDate,
             ref PaidFees, ref CreatedByUserID);

             if(isFound)
            {
                return new clsApplication(ApplicationID,
              ApplicantPersonID, ApplicationDate, ApplicationTypeID, (enApplicationStatus)ApplicationStatus, LastStatusDate,
              PaidFees, CreatedByUserID);
            }
            return null;

        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, 2);
        }

        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, 3);
        }

        public bool Delete()
        {

            return clsApplicationData.DeleteApplication(this.ApplicationID);
        }


        public static bool IsApplicationExist(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationID);
        }

       public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }

        public  bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);
        }


        public static int GetActiveApplicationID(int PersonID, clsApplication.enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
            
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, clsApplication.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);

        }

        public int GetActiveApplicationID( clsApplication.enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(this.ApplicantPersonID, (int)ApplicationTypeID);

        }

    }
}
