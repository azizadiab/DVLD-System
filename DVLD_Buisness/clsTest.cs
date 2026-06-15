using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsTest
    {

        public enum enMode { AddNew = 0, Update }
        private enMode _Mode = enMode.AddNew;
      public  int TestID { get; set; }
      public  int TestAppointmentID { get; set; }
        public clsTestAppointment TestAppointmentInfo { get; set; }
      public  bool TestResult { get; set; }
      public  string Notes { get; set; }
      public  int CreatedByUserID { get; set; }
    public clsTest()
        {

           this.TestID = -1;
           this.TestAppointmentID = -1;
           this.TestResult = false;
           this.Notes = "";
           this.CreatedByUserID = -1;
            _Mode = enMode.AddNew;
        }

        public clsTest(int TestID, int TestAppointmentID, bool TestResult, string Notes,  int CreatedByUserID)
        {
        this.TestID = TestID;
        this.TestAppointmentID = TestAppointmentID;
        this.TestResult = TestResult;
        this.Notes = Notes;
        this.CreatedByUserID = CreatedByUserID;

            _Mode = enMode.Update; 

        }

        private bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return (this.TestID != -1);
               
        }

        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID,this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

        }

        public bool Save()
        {
            switch(_Mode)
            {

                case enMode.AddNew:
                    if(_AddNewTest())
                    {
                       _Mode= enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateTest();
            }
            return false;
        }

        public static clsTest Find(int TestID)
        {
           int TestAppointmentID = -1, CreatedByUserID = -1;
           string Notes = "";
           bool TestResult = false;

            if(clsTestData.GetTestInfoByID( TestID, ref  TestAppointmentID, ref  TestResult,
                                                ref  Notes, ref  CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            else
            {
                return null;
            }

        }

        public static clsTest FindLastTestPerPersonAndLicenseClass(int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {
            int TestID = -1, TestAppointmentID = -1, CreatedByUserID = -1;
             bool TestResult = false;
            string Notes = "";


            if (clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass( PersonID, LicenseClassID, (int)TestTypeID,
                                                       ref TestID, ref TestAppointmentID, ref TestResult,
                                                       ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            return null;
            
        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }
    }

}
