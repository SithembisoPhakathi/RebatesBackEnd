using Microsoft.AspNetCore.Mvc;
using Rebates1.Models;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Reflection.PortableExecutable;
using iText.Kernel.Pdf;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Rebates1.Data;

namespace Rebates1.Controllers
{
    public class AcknowledgeController : Controller
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;
        List<RebatesViewModel> RebatesViewModels = new();
        List<AdminValuer> adminValuers = new();
        List<ApplicationStatistics> applicationStatistics = new();
        SqlConnection conn = new();
        SqlCommand com = new();
        SqlDataReader dr;
        EmailHelper emailHelper = new EmailHelper();

        private readonly IConverter _pdfConverter;

        public AcknowledgeController(IConfiguration config)
        {
            _config = config;
            conn.ConnectionString = _config.GetConnectionString("defaultConnection");
        }

        public IActionResult Index()
        {
            if (applicationStatistics.Count > 0)
            {
                applicationStatistics.Clear();
            }
            try
            {
                conn.Open();
                com.Connection = conn;
                com.CommandText = "SELECT COUNT(Rebate_Type) AS NumberOfApplication, Rebate_Type FROM [Objection].[dbo].[Rebate_Infos] GROUP BY Rebate_Type";

                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    applicationStatistics.Add(new ApplicationStatistics
                    {
                        RebateType = dr["Rebate_Type"].ToString(),
                        NumberOfApplications = (int)dr["NumberOfApplication"]
                    });
                }

                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }

            var User = this.User.Identity.Name;
            TempData["currentUser"] = User;

            if (adminValuers.Count > 0)
            {
                adminValuers.Clear();
            }
            try
            {
                conn.Open();
                com.Connection = conn;
                com.CommandText = "EXEC GetRebatesUserByUsername @Username";

                com.Parameters.AddWithValue("@Username", User);

                dr = com.ExecuteReader();

                while (dr.Read())
                {
                    adminValuers.Add(new AdminValuer
                    {
                        Username = dr["Username"].ToString(),
                        Role = dr["Role"].ToString(),
                        Sector = dr["Sector"].ToString(),
                        EmailAddress = dr["Email_Address"].ToString(),
                        Surname = dr["Surname"].ToString(),
                        FirstName = dr["First_name"].ToString(),
                        PhoneNo = dr["Phone_No"].ToString(),
                    });
                }

                conn.Close();
                if (adminValuers.Count > 0)
                {
                    foreach (var admin in adminValuers)
                    {
                        TempData["currentRole"] = admin.Role;
                        TempData["currentUserRole"] = "Admin";
                        TempData["currentUserSector"] = admin.Sector;
                        TempData["currentUserEmail"] = admin.EmailAddress;
                        TempData["currentUserSurname"] = admin.Surname;
                        TempData["currentUserFirstname"] = admin.FirstName;
                        TempData["currentUserPhoneNo"] = admin.PhoneNo;
                    }
                }
                //else
                //{
                //    return PartialView("_Error403");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }

            TempData.Keep("currentEmail");
            TempData.Keep("currentRole");
            TempData.Keep("currentUserRole");
            TempData.Keep("currentUserSector");
            TempData.Keep("currentUserEmail");
            TempData.Keep("currentUserSurname");
            TempData.Keep("currentUserFirstname");
            TempData.Keep("currentUserPhoneNo");
            TempData.Keep("currentUsername");

            return View(applicationStatistics);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Acknowledge(string? userName)
        {
            var user = HttpContext.User;
            //var userName = user.Identity.Name;

            if (RebatesViewModels.Count > 0)
            {
                RebatesViewModels.Clear();
            }

            try
            {
                conn.Open();
                com.Connection = conn;
                com.CommandText = "EXEC GetRebateInfos @CurrentUserFullName";

                string currentUserFullName = $"{TempData["currentUserFirstname"]} {TempData["currentUserSurname"]}";

                com.Parameters.AddWithValue("@CurrentUserFullName", currentUserFullName);

                dr = com.ExecuteReader();

                while (dr.Read())
                {
                    RebatesViewModels.Add(new RebatesViewModel()
                    {
                        Rebate_No = dr["Rebate_No"].ToString(),
                        AccountNumber = dr["AccountNumber"].ToString(),
                        IDNumber = dr["IDNumber"].ToString(),
                        Rebate_Type = dr["Rebate_Type"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        Surname = dr["Surname"].ToString(),
                        FirstNames = dr["FirstNames"].ToString(),
                        Status = dr["Status"].ToString(),
                        DateOfSubmission = dr["DateOfSubmission"].ToString(),
                        Email = dr["Email"].ToString(),
                        UserID_SAP_Number = dr["UserID SAP Number"].ToString()
                    });
                }
                conn.Close();

                ViewBag.Rebates = RebatesViewModels.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("Acknowledge", new { userName = userName });
        }

        public IActionResult ViewData(string? RebateType, string? RebateNo)
        {
            if (RebateType == "Pensioner Rebate 70")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;
                    com.CommandText = "EXEC [Objection].[dbo].[ViewRebateForm] @RebateNo = '" + RebateNo + "'";     //SELECT * FROM FileTest where Rebate_Ref_files

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel()
                        {
                            Rebate_No = dr["Rebate_No"].ToString(),
                            Rebate_Type = dr["Rebate_Type"].ToString(),
                            AccountNumber = dr["AccountNumber"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            MaritalStatus = dr["MaritalStatus"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            FirstNames = dr["FirstNames"].ToString(),
                            IDNumber = dr["IDNumber"].ToString(),
                            PassportNumber = dr["PassportNumber"].ToString(),
                            SpouseSurname = dr["SpouseSurname"].ToString(),
                            SpouseFirstNames = dr["SpouseFirstNames"].ToString(),
                            SpouseIDNumber = dr["SpouseIDNumber"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            CitySuburb = dr["CitySuburb"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PostalAddress = dr["PostalAddress"].ToString(),
                            PostalAddressCitySuburb = dr["PostalAddressCitySuburb"].ToString(),
                            PostalAddressPostalCode = dr["PostalAddressPostalCode"].ToString(),
                            HomeTel = dr["HomeTel"].ToString(),
                            CellNo = dr["CellNo"].ToString(),
                            WorkTel = dr["WorkTel"].ToString(),
                            Email = dr["Email"].ToString(),
                            OccupyMentionedProperty = dr["OccupyMentionedProperty"].ToString(),
                            StandNumber = dr["StandNumber"].ToString(),
                            PortionNumber = dr["PortionNumber"].ToString(),
                            Suburb = dr["Suburb"].ToString(),
                            NameOfBodyCorporate = dr["NameOfBodyCorporate"].ToString(),
                            UnitNumberDoorNumber = dr["UnitNumberDoorNumber"].ToString(),
                            DoorNumber = dr["DoorNumber"].ToString(),
                            //FilePath1 = dr["FilePath1"].ToString() 
                        });
                    }
                    conn.Close();

                    ViewBag.SeventyYears = RebatesViewModels.ToList();

                    foreach (var items in ViewBag.SeventyYears)
                    {
                        TempData["Rebate_No"] = @items.Rebate_No;
                        TempData["AccountNumber"] = @items.AccountNumber;
                        TempData["IDNumber"] = @items.IDNumber;
                        TempData["PassportNumber"] = @items.PassportNumber;
                        TempData["Gender"] = @items.Gender;
                        TempData["MaritalStatus"] = @items.MaritalStatus;
                        TempData["Surname"] = @items.Surname;
                        TempData["FirstNames"] = @items.FirstNames;
                        TempData["SpouseSurname"] = @items.SpouseSurname;
                        TempData["SpouseFirstNames"] = @items.SpouseFirstNames;
                        TempData["SpouseIDNumber"] = @items.SpouseIDNumber;
                        TempData["StreetAddress"] = @items.StreetAddress;
                        TempData["CitySuburb"] = @items.CitySuburb;
                        TempData["PostalCode"] = @items.PostalCode;
                        TempData["PostalAddress"] = @items.PostalAddress;
                        TempData["PostalAddressCitySuburb"] = @items.PostalAddressCitySuburb;
                        TempData["PostalAddressPostalCode"] = @items.PostalAddressPostalCode;
                        TempData["HomeTel"] = @items.HomeTel;
                        TempData["CellNo"] = @items.CellNo;
                        TempData["WorkTel"] = @items.WorkTel;
                        TempData["Email"] = @items.Email;
                        TempData["OccupyMentionedProperty"] = @items.OccupyMentionedProperty;
                        TempData["StandNumber"] = @items.StandNumber;
                        TempData["PortionNumber"] = @items.PortionNumber;
                        TempData["Suburb"] = @items.Suburb;
                        TempData["NameOfBodyCorporate"] = @items.NameOfBodyCorporate;
                        TempData["UnitNumberDoorNumber"] = @items.UnitNumberDoorNumber;
                        TempData["DoorNumber"] = @items.DoorNumber;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("_ViewForm70", RebatesViewModels);
            }

            if (RebateType == "Pensioner Rebate 60")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;
                    com.CommandText = "EXEC [Objection].[dbo].[ViewRebateForm] @RebateNo = '" + RebateNo + "'";
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel()
                        {
                            Rebate_No = dr["Rebate_No"].ToString(),
                            Rebate_Type = dr["Rebate_Type"].ToString(),
                            AccountNumber = dr["AccountNumber"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            MaritalStatus = dr["MaritalStatus"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            FirstNames = dr["FirstNames"].ToString(),
                            IDNumber = dr["IDNumber"].ToString(),
                            PassportNumber = dr["PassportNumber"].ToString(),
                            SpouseSurname = dr["SpouseSurname"].ToString(),
                            SpouseFirstNames = dr["SpouseFirstNames"].ToString(),
                            SpouseIDNumber = dr["SpouseIDNumber"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            CitySuburb = dr["CitySuburb"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PostalAddress = dr["PostalAddress"].ToString(),
                            PostalAddressCitySuburb = dr["PostalAddressCitySuburb"].ToString(),
                            PostalAddressPostalCode = dr["PostalAddressPostalCode"].ToString(),
                            HomeTel = dr["HomeTel"].ToString(),
                            CellNo = dr["CellNo"].ToString(),
                            WorkTel = dr["WorkTel"].ToString(),
                            Email = dr["Email"].ToString(),
                            OccupyMentionedProperty = dr["OccupyMentionedProperty"].ToString(),
                            StandNumber = dr["StandNumber"].ToString(),
                            PortionNumber = dr["PortionNumber"].ToString(),
                            Suburb = dr["Suburb"].ToString(),
                            NameOfBodyCorporate = dr["NameOfBodyCorporate"].ToString(),
                            UnitNumberDoorNumber = dr["UnitNumberDoorNumber"].ToString(),
                            DoorNumber = dr["DoorNumber"].ToString(),
                            SalaryIncomeApplicant = dr["SalaryIncomeApplicant"].ToString(),
                            InterestOnInvestmentsApplicant = dr["InterestOnInvestmentsApplicant"].ToString(),
                            MonthlyPensionApplicant = dr["MonthlyPensionApplicant"].ToString(),
                            StateDisabilityAllowanceApplicant = dr["StateDisabilityAllowanceApplicant"].ToString(),
                            OtherIncomeApplicant = dr["OtherIncomeApplicant"].ToString(),
                            SalaryIncomeSpouse = dr["SalaryIncomeSpouse"].ToString(),
                            InterestOnInvestmentsSpouse = dr["InterestOnInvestmentsSpouse"].ToString(),
                            MonthlyPensionSpouse = dr["MonthlyPensionSpouse"].ToString(),
                            StateDisabilityAllowanceSpouse = dr["StateDisabilityAllowanceSpouse"].ToString(),
                            OtherIncomeSpouse = dr["OtherIncomeSpouse"].ToString()
                        });
                    }
                    conn.Close();

                    ViewBag.SixtyYears = RebatesViewModels.ToList();

                    foreach (var items in ViewBag.SixtyYears)
                    {
                        TempData["Rebate_No"] = @items.Rebate_No;
                        TempData["AccountNumber"] = @items.AccountNumber;
                        TempData["IDNumber"] = @items.IDNumber;
                        TempData["PassportNumber"] = @items.PassportNumber;
                        TempData["Gender"] = @items.Gender;
                        TempData["MaritalStatus"] = @items.MaritalStatus;
                        TempData["Surname"] = @items.Surname;
                        TempData["FirstNames"] = @items.FirstNames;
                        TempData["SpouseSurname"] = @items.SpouseSurname;
                        TempData["SpouseFirstNames"] = @items.SpouseFirstNames;
                        TempData["SpouseIDNumber"] = @items.SpouseIDNumber;
                        TempData["StreetAddress"] = @items.StreetAddress;
                        TempData["CitySuburb"] = @items.CitySuburb;
                        TempData["PostalCode"] = @items.PostalCode;
                        TempData["PostalAddress"] = @items.PostalAddress;
                        TempData["PostalAddressCitySuburb"] = @items.PostalAddressCitySuburb;
                        TempData["PostalAddressPostalCode"] = @items.PostalAddressPostalCode;
                        TempData["HomeTel"] = @items.HomeTel;
                        TempData["CellNo"] = @items.CellNo;
                        TempData["WorkTel"] = @items.WorkTel;
                        TempData["Email"] = @items.Email;
                        TempData["OccupyMentionedProperty"] = @items.OccupyMentionedProperty;
                        TempData["StandNumber"] = @items.StandNumber;
                        TempData["PortionNumber"] = @items.PortionNumber;
                        TempData["Suburb"] = @items.Suburb;
                        TempData["NameOfBodyCorporate"] = @items.NameOfBodyCorporate;
                        TempData["UnitNumberDoorNumber"] = @items.UnitNumberDoorNumber;
                        TempData["DoorNumber"] = @items.DoorNumber;
                        TempData["SalaryIncomeApplicant"] = @items.SalaryIncomeApplicant;
                        TempData["InterestOnInvestmentsApplicant"] = @items.InterestOnInvestmentsApplicant;
                        TempData["MonthlyPensionApplicant"] = @items.MonthlyPensionApplicant;
                        TempData["StateDisabilityAllowanceApplicant"] = @items.StateDisabilityAllowanceApplicant;
                        TempData["OtherIncomeApplicant"] = @items.OtherIncomeApplicant;
                        TempData["SalaryIncomeSpouse"] = @items.SalaryIncomeSpouse;
                        TempData["InterestOnInvestmentsSpouse"] = @items.InterestOnInvestmentsSpouse;
                        TempData["MonthlyPensionSpouse"] = @items.MonthlyPensionSpouse;
                        TempData["StateDisabilityAllowanceSpouse"] = @items.StateDisabilityAllowanceSpouse;
                        TempData["OtherIncomeSpouse"] = @items.OtherIncomeSpouse;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("_ViewForm60", RebatesViewModels);
            }

            if (RebateType == "High Density Rebate" || RebateType == "Public Benefit Organisation Rebate" || RebateType == "Disaster Rebate")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;
                    com.CommandText = "EXEC [Objection].[dbo].[ViewRebateForm] @RebateNo = '" + RebateNo + "'";

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel()
                        {
                            Rebate_No = dr["Rebate_No"].ToString(),
                            Rebate_Type = dr["Rebate_Type"].ToString(),
                            Status = dr["Status"].ToString(),
                            AccountNumber = dr["AccountNumber"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            MaritalStatus = dr["MaritalStatus"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            FirstNames = dr["FirstNames"].ToString(),
                            IDNumber = dr["IDNumber"].ToString(),
                            PassportNumber = dr["PassportNumber"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            CitySuburb = dr["CitySuburb"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PostalAddress = dr["PostalAddress"].ToString(),
                            PostalAddressCitySuburb = dr["PostalAddressCitySuburb"].ToString(),
                            PostalAddressPostalCode = dr["PostalAddressPostalCode"].ToString(),
                            HomeTel = dr["HomeTel"].ToString(),
                            CellNo = dr["CellNo"].ToString(),
                            WorkTel = dr["WorkTel"].ToString(),
                            Email = dr["Email"].ToString(),
                            AccountNo_MultipleUnits = dr["AccountNo_MultipleUnits"].ToString(),
                            Contact_CellNo = dr["Contact_CellNo"].ToString(),
                            SchemeBuilding = dr["SchemeBuilding"].ToString()
                        });
                    }
                    conn.Close();

                    ViewBag.HighDensity = RebatesViewModels.ToList();

                    foreach (var items in ViewBag.HighDensity)
                    {
                        TempData["Rebate_No"] = @items.Rebate_No;
                        TempData["RebateType"] = @items.Rebate_Type;
                        TempData["Status"] = @items.Status;
                        TempData["AccountNumber"] = @items.AccountNumber;
                        TempData["IDNumber"] = @items.IDNumber;
                        TempData["PassportNumber"] = @items.PassportNumber;
                        TempData["Gender"] = @items.Gender;
                        TempData["MaritalStatus"] = @items.MaritalStatus;
                        TempData["Surname"] = @items.Surname;
                        TempData["FirstNames"] = @items.FirstNames;
                        TempData["SpouseSurname"] = @items.SpouseSurname;
                        TempData["SpouseFirstNames"] = @items.SpouseFirstNames;
                        TempData["SpouseIDNumber"] = @items.SpouseIDNumber;
                        TempData["StreetAddress"] = @items.StreetAddress;
                        TempData["CitySuburb"] = @items.CitySuburb;
                        TempData["PostalCode"] = @items.PostalCode;
                        TempData["PostalAddress"] = @items.PostalAddress;
                        TempData["PostalAddressCitySuburb"] = @items.PostalAddressCitySuburb;
                        TempData["PostalAddressPostalCode"] = @items.PostalAddressPostalCode;
                        TempData["HomeTel"] = @items.HomeTel;
                        TempData["CellNo"] = @items.CellNo;
                        TempData["WorkTel"] = @items.WorkTel;
                        TempData["Email"] = @items.Email;
                        TempData["AccountNo_MultipleUnits"] = @items.AccountNo_MultipleUnits;
                        TempData["Contact_CellNo"] = @items.Contact_CellNo;
                        TempData["SchemeBuilding"] = @items.SchemeBuilding;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("_ViewFormDensity", RebatesViewModels);
            }

            if (RebateType == "Child Headed Household Rebate")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;
                    com.CommandText = "EXEC [Objection].[dbo].[ViewRebateForm] @RebateNo = '" + RebateNo + "'";

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel()
                        {
                            Rebate_No = dr["Rebate_No"].ToString(),
                            Rebate_Type = dr["Rebate_Type"].ToString(),
                            AccountNumber = dr["AccountNumber"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            MaritalStatus = dr["MaritalStatus"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            FirstNames = dr["FirstNames"].ToString(),
                            IDNumber = dr["IDNumber"].ToString(),
                            PassportNumber = dr["PassportNumber"].ToString(),
                            SpouseSurname = dr["SpouseSurname"].ToString(),
                            SpouseFirstNames = dr["SpouseFirstNames"].ToString(),
                            SpouseIDNumber = dr["SpouseIDNumber"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            CitySuburb = dr["CitySuburb"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PostalAddress = dr["PostalAddress"].ToString(),
                            PostalAddressCitySuburb = dr["PostalAddressCitySuburb"].ToString(),
                            PostalAddressPostalCode = dr["PostalAddressPostalCode"].ToString(),
                            HomeTel = dr["HomeTel"].ToString(),
                            CellNo = dr["CellNo"].ToString(),
                            WorkTel = dr["WorkTel"].ToString(),
                            Email = dr["Email"].ToString(),
                            OccupyMentionedProperty = dr["OccupyMentionedProperty"].ToString(),
                            StandNumber = dr["StandNumber"].ToString(),
                            PortionNumber = dr["PortionNumber"].ToString(),
                            Suburb = dr["Suburb"].ToString(),
                            NameOfBodyCorporate = dr["NameOfBodyCorporate"].ToString(),
                            UnitNumberDoorNumber = dr["UnitNumberDoorNumber"].ToString(),
                            DoorNumber = dr["DoorNumber"].ToString(),
                            NameSurname1 = dr["NameSurname1"].ToString(),
                            NameSurname2 = dr["NameSurname2"].ToString(),
                            NameSurname3 = dr["NameSurname3"].ToString(),
                            NameSurname4 = dr["NameSurname4"].ToString(),
                            IDNo1 = dr["IDNo1"].ToString(),
                            IDNo2 = dr["IDNo2"].ToString(),
                            IDNo3 = dr["IDNo3"].ToString(),
                            IDNo4 = dr["IDNo4"].ToString(),
                            HouseUnits = dr["HouseUnits"].ToString()
                        });
                    }
                    conn.Close();

                    ViewBag.ChildHHH = RebatesViewModels.ToList();

                    foreach (var items in ViewBag.ChildHHH)
                    {
                        TempData["Rebate_No"] = @items.Rebate_No;
                        TempData["AccountNumber"] = @items.AccountNumber;
                        TempData["IDNumber"] = @items.IDNumber;
                        TempData["PassportNumber"] = @items.PassportNumber;
                        TempData["Gender"] = @items.Gender;
                        TempData["MaritalStatus"] = @items.MaritalStatus;
                        TempData["Surname"] = @items.Surname;
                        TempData["FirstNames"] = @items.FirstNames;
                        TempData["SpouseSurname"] = @items.SpouseSurname;
                        TempData["SpouseFirstNames"] = @items.SpouseFirstNames;
                        TempData["SpouseIDNumber"] = @items.SpouseIDNumber;
                        TempData["StreetAddress"] = @items.StreetAddress;
                        TempData["CitySuburb"] = @items.CitySuburb;
                        TempData["PostalCode"] = @items.PostalCode;
                        TempData["PostalAddress"] = @items.PostalAddress;
                        TempData["PostalAddressCitySuburb"] = @items.PostalAddressCitySuburb;
                        TempData["PostalAddressPostalCode"] = @items.PostalAddressPostalCode;
                        TempData["HomeTel"] = @items.HomeTel;
                        TempData["CellNo"] = @items.CellNo;
                        TempData["WorkTel"] = @items.WorkTel;
                        TempData["Email"] = @items.Email;
                        TempData["OccupyMentionedProperty"] = @items.OccupyMentionedProperty;
                        TempData["StandNumber"] = @items.StandNumber;
                        TempData["PortionNumber"] = @items.PortionNumber;
                        TempData["Suburb"] = @items.Suburb;
                        TempData["NameOfBodyCorporate"] = @items.NameOfBodyCorporate;
                        TempData["UnitNumberDoorNumber"] = @items.UnitNumberDoorNumber;
                        TempData["DoorNumber"] = @items.DoorNumber;
                        TempData["NameSurname1"] = @items.NameSurname1;
                        TempData["NameSurname2"] = @items.NameSurname2;
                        TempData["NameSurname3"] = @items.NameSurname3;
                        TempData["NameSurname4"] = @items.NameSurname4;
                        TempData["IDNo1"] = @items.IDNo1;
                        TempData["IDNo2"] = @items.IDNo2;
                        TempData["IDNo3"] = @items.IDNo3;
                        TempData["IDNo4"] = @items.IDNo4;
                        TempData["HouseUnits"] = @items.HouseUnits;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("_ViewFormChildHeadedHH", RebatesViewModels);
            }

            if (RebateType == "Life Rights Rebate")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;
                    com.CommandText = "EXEC [Objection].[dbo].[ViewRebateForm] @RebateNo = '" + RebateNo + "'";

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel()
                        {
                            Rebate_No = dr["Rebate_No"].ToString(),
                            Rebate_Type = dr["Rebate_Type"].ToString(),
                            AccountNumber = dr["AccountNumber"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            MaritalStatus = dr["MaritalStatus"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            FirstNames = dr["FirstNames"].ToString(),
                            IDNumber = dr["IDNumber"].ToString(),
                            PassportNumber = dr["PassportNumber"].ToString(),
                            SpouseSurname = dr["SpouseSurname"].ToString(),
                            SpouseFirstNames = dr["SpouseFirstNames"].ToString(),
                            SpouseIDNumber = dr["SpouseIDNumber"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            CitySuburb = dr["CitySuburb"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PostalAddress = dr["PostalAddress"].ToString(),
                            PostalAddressCitySuburb = dr["PostalAddressCitySuburb"].ToString(),
                            PostalAddressPostalCode = dr["PostalAddressPostalCode"].ToString(),
                            HomeTel = dr["HomeTel"].ToString(),
                            CellNo = dr["CellNo"].ToString(),
                            WorkTel = dr["WorkTel"].ToString(),
                            Email = dr["Email"].ToString(),
                            OccupyMentionedProperty = dr["OccupyMentionedProperty"].ToString(),
                            StandNumber = dr["StandNumber"].ToString(),
                            PortionNumber = dr["PortionNumber"].ToString(),
                            Suburb = dr["Suburb"].ToString(),
                            NameOfBodyCorporate = dr["NameOfBodyCorporate"].ToString(),
                            UnitNumberDoorNumber = dr["UnitNumberDoorNumber"].ToString(),
                            DoorNumber = dr["DoorNumber"].ToString(),
                            AccountNo_MultipleUnits = dr["AccountNo_MultipleUnits"].ToString(),
                            Contact_CellNo = dr["Contact_CellNo"].ToString(),
                            SchemeBuilding = dr["SchemeBuilding"].ToString()
                        });
                    }
                    conn.Close();

                    ViewBag.LifeRights = RebatesViewModels.ToList();

                    foreach (var items in ViewBag.LifeRights)
                    {
                        TempData["Rebate_No"] = @items.Rebate_No;
                        TempData["AccountNumber"] = @items.AccountNumber;
                        TempData["IDNumber"] = @items.IDNumber;
                        TempData["PassportNumber"] = @items.PassportNumber;
                        TempData["Gender"] = @items.Gender;
                        TempData["MaritalStatus"] = @items.MaritalStatus;
                        TempData["Surname"] = @items.Surname;
                        TempData["FirstNames"] = @items.FirstNames;
                        TempData["SpouseSurname"] = @items.SpouseSurname;
                        TempData["SpouseFirstNames"] = @items.SpouseFirstNames;
                        TempData["SpouseIDNumber"] = @items.SpouseIDNumber;
                        TempData["StreetAddress"] = @items.StreetAddress;
                        TempData["CitySuburb"] = @items.CitySuburb;
                        TempData["PostalCode"] = @items.PostalCode;
                        TempData["PostalAddress"] = @items.PostalAddress;
                        TempData["PostalAddressCitySuburb"] = @items.PostalAddressCitySuburb;
                        TempData["PostalAddressPostalCode"] = @items.PostalAddressPostalCode;
                        TempData["HomeTel"] = @items.HomeTel;
                        TempData["CellNo"] = @items.CellNo;
                        TempData["WorkTel"] = @items.WorkTel;
                        TempData["Email"] = @items.Email;
                        TempData["OccupyMentionedProperty"] = @items.OccupyMentionedProperty;
                        TempData["StandNumber"] = @items.StandNumber;
                        TempData["PortionNumber"] = @items.PortionNumber;
                        TempData["Suburb"] = @items.Suburb;
                        TempData["NameOfBodyCorporate"] = @items.NameOfBodyCorporate;
                        TempData["UnitNumberDoorNumber"] = @items.UnitNumberDoorNumber;
                        TempData["DoorNumber"] = @items.DoorNumber;
                        TempData["AccountNo_MultipleUnits"] = @items.AccountNo_MultipleUnits;
                        TempData["Contact_CellNo"] = @items.Contact_CellNo;
                        TempData["SchemeBuilding"] = @items.SchemeBuilding;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("_LifeRights", RebatesViewModels);
            }

            if (RebateType == "Sports Club Rebate" || RebateType == "Protection of Animals")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;
                    com.CommandText = "EXEC [Objection].[dbo].[ViewRebateForm] @RebateNo = '" + RebateNo + "'";

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel()
                        {
                            Rebate_No = dr["Rebate_No"].ToString(),
                            Rebate_Type = dr["Rebate_Type"].ToString(),
                            AccountNumber = dr["AccountNumber"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            MaritalStatus = dr["MaritalStatus"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            FirstNames = dr["FirstNames"].ToString(),
                            IDNumber = dr["IDNumber"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            CitySuburb = dr["CitySuburb"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PostalAddress = dr["PostalAddress"].ToString(),
                            PostalAddressCitySuburb = dr["PostalAddressCitySuburb"].ToString(),
                            PostalAddressPostalCode = dr["PostalAddressPostalCode"].ToString(),
                            HomeTel = dr["HomeTel"].ToString(),
                            CellNo = dr["CellNo"].ToString(),
                            WorkTel = dr["WorkTel"].ToString(),
                            Email = dr["Email"].ToString(),
                            AccountNo_MultipleUnits = dr["AccountNo_MultipleUnits"].ToString(),
                            Contact_CellNo = dr["Contact_CellNo"].ToString(),
                            SchemeBuilding = dr["SchemeBuilding"].ToString(),
                            ErfandTownship = dr["ErfandTownship"].ToString(),
                            NameOfTheOrganisation = dr["NameOfTheOrganisation"].ToString(),
                            RegistrationNumber = dr["RegistrationNumber"].ToString(),
                            NameSurnameDesignation = dr["NameSurnameDesignation"].ToString(),
                            RegisteredWithSARS = dr["RegisteredWithSARS"].ToString()

                        });
                    }
                    conn.Close();

                    ViewBag.SportsClub = RebatesViewModels.ToList();

                    foreach (var items in ViewBag.SportsClub)
                    {
                        TempData["Rebate_No"] = @items.Rebate_No;
                        TempData["AccountNumber"] = @items.AccountNumber;
                        TempData["IDNumber"] = @items.IDNumber;
                        TempData["Gender"] = @items.Gender;
                        TempData["MaritalStatus"] = @items.MaritalStatus;
                        TempData["Surname"] = @items.Surname;
                        TempData["FirstNames"] = @items.FirstNames;
                        TempData["SpouseSurname"] = @items.SpouseSurname;
                        TempData["SpouseFirstNames"] = @items.SpouseFirstNames;
                        TempData["SpouseIDNumber"] = @items.SpouseIDNumber;
                        TempData["StreetAddress"] = @items.StreetAddress;
                        TempData["CitySuburb"] = @items.CitySuburb;
                        TempData["PostalCode"] = @items.PostalCode;
                        TempData["PostalAddress"] = @items.PostalAddress;
                        TempData["PostalAddressCitySuburb"] = @items.PostalAddressCitySuburb;
                        TempData["PostalAddressPostalCode"] = @items.PostalAddressPostalCode;
                        TempData["HomeTel"] = @items.HomeTel;
                        TempData["CellNo"] = @items.CellNo;
                        TempData["WorkTel"] = @items.WorkTel;
                        TempData["Email"] = @items.Email;
                        TempData["AccountNo_MultipleUnits"] = @items.AccountNo_MultipleUnits;
                        TempData["Contact_CellNo"] = @items.Contact_CellNo;
                        TempData["SchemeBuilding"] = @items.SchemeBuilding;
                        TempData["ErfandTownship"] = @items.ErfandTownship;
                        TempData["NameOfTheOrganisation"] = @items.NameOfTheOrganisation;
                        TempData["RegistrationNumber"] = @items.RegistrationNumber;
                        TempData["NameSurnameDesignation"] = @items.NameSurnameDesignation;
                        TempData["RegisteredWithSARS"] = @items.RegisteredWithSARS;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("_SportsClub", RebatesViewModels);
            }

            if (RebateType == "Heritage Sites Rebate")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;
                    com.CommandText = "EXEC [Objection].[dbo].[ViewRebateForm] @RebateNo = '" + RebateNo + "'";

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel()
                        {
                            Rebate_No = dr["Rebate_No"].ToString(),
                            Rebate_Type = dr["Rebate_Type"].ToString(),
                            AccountNumber = dr["AccountNumber"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            MaritalStatus = dr["MaritalStatus"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            FirstNames = dr["FirstNames"].ToString(),
                            IDNumber = dr["IDNumber"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            CitySuburb = dr["CitySuburb"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PostalAddress = dr["PostalAddress"].ToString(),
                            PostalAddressCitySuburb = dr["PostalAddressCitySuburb"].ToString(),
                            PostalAddressPostalCode = dr["PostalAddressPostalCode"].ToString(),
                            HomeTel = dr["HomeTel"].ToString(),
                            CellNo = dr["CellNo"].ToString(),
                            WorkTel = dr["WorkTel"].ToString(),
                            Email = dr["Email"].ToString(),
                            AccountNo_MultipleUnits = dr["AccountNo_MultipleUnits"].ToString(),
                            Contact_CellNo = dr["Contact_CellNo"].ToString(),
                            SchemeBuilding = dr["SchemeBuilding"].ToString(),
                            ErfandTownship = dr["ErfandTownship"].ToString(),
                            NameOfTheOrganisation = dr["NameOfTheOrganisation"].ToString(),
                            RegistrationNumber = dr["RegistrationNumber"].ToString(),
                            NameSurnameDesignation = dr["NameSurnameDesignation"].ToString(),
                            RegisteredWithSARS = dr["RegisteredWithSARS"].ToString(),
                            NameOfApplicant = dr["NameOfApplicant"].ToString(),
                            NameOfHeritageProperty = dr["NameOfHeritageProperty"].ToString(),
                            BriefDescriptionHS = dr["BriefDescriptionHS"].ToString(),
                            NationalHeritageSite = dr["NationalHeritageSite"].ToString(),
                            ProvincialHeritageSite = dr["ProvincialHeritageSite"].ToString(),
                            HeritageArea = dr["HeritageArea"].ToString(),
                            ProvisionalProtection = dr["ProvisionalProtection"].ToString(),
                            Other = dr["Other"].ToString(),
                            SpecifyOther = dr["SpecifyOther"].ToString(),
                            NumberDateGovernmentNotice = dr["NumberDateGovernmentNotice"].ToString(),
                            DetailsGazette = dr["DetailsGazette"].ToString(),
                            FormerProtection = dr["FormerProtection"].ToString()
                        });
                    }
                    conn.Close();

                    ViewBag.HeritageSites = RebatesViewModels.ToList();

                    foreach (var items in ViewBag.HeritageSites)
                    {
                        TempData["Rebate_No"] = @items.Rebate_No;
                        TempData["AccountNumber"] = @items.AccountNumber;
                        TempData["IDNumber"] = @items.IDNumber;
                        TempData["Gender"] = @items.Gender;
                        TempData["MaritalStatus"] = @items.MaritalStatus;
                        TempData["Surname"] = @items.Surname;
                        TempData["FirstNames"] = @items.FirstNames;
                        TempData["SpouseSurname"] = @items.SpouseSurname;
                        TempData["SpouseFirstNames"] = @items.SpouseFirstNames;
                        TempData["SpouseIDNumber"] = @items.SpouseIDNumber;
                        TempData["StreetAddress"] = @items.StreetAddress;
                        TempData["CitySuburb"] = @items.CitySuburb;
                        TempData["PostalCode"] = @items.PostalCode;
                        TempData["PostalAddress"] = @items.PostalAddress;
                        TempData["PostalAddressCitySuburb"] = @items.PostalAddressCitySuburb;
                        TempData["PostalAddressPostalCode"] = @items.PostalAddressPostalCode;
                        TempData["HomeTel"] = @items.HomeTel;
                        TempData["CellNo"] = @items.CellNo;
                        TempData["WorkTel"] = @items.WorkTel;
                        TempData["Email"] = @items.Email;
                        TempData["AccountNo_MultipleUnits"] = @items.AccountNo_MultipleUnits;
                        TempData["Contact_CellNo"] = @items.Contact_CellNo;
                        TempData["SchemeBuilding"] = @items.SchemeBuilding;
                        TempData["ErfandTownship"] = @items.ErfandTownship;
                        TempData["NameOfTheOrganisation"] = @items.NameOfTheOrganisation;
                        TempData["RegistrationNumber"] = @items.RegistrationNumber;
                        TempData["NameSurnameDesignation"] = @items.NameSurnameDesignation;
                        TempData["RegisteredWithSARS"] = @items.RegisteredWithSARS;
                        TempData["NameOfApplicant"] = @items.NameOfApplicant;
                        TempData["NameOfHeritageProperty"] = @items.NameOfHeritageProperty;
                        TempData["BriefDescriptionHS"] = @items.BriefDescriptionHS;
                        TempData["NationalHeritageSite"] = @items.NationalHeritageSite;
                        TempData["ProvincialHeritageSite"] = @items.ProvincialHeritageSite;
                        TempData["HeritageArea"] = @items.HeritageArea;
                        TempData["ProvisionalProtection"] = @items.ProvisionalProtection;
                        TempData["Other"] = @items.Other;
                        TempData["SpecifyOther"] = @items.SpecifyOther;
                        TempData["NumberDateGovernmentNotice"] = @items.NumberDateGovernmentNotice;
                        TempData["DetailsGazette"] = @items.DetailsGazette;
                        TempData["FormerProtection"] = @items.FormerProtection;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("_HeritageSites", RebatesViewModels);
            }

            if (RebateType == "Education Rebate")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;
                    com.CommandText = "EXEC [Objection].[dbo].[ViewRebateForm] @RebateNo = '" + RebateNo + "'";

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel()
                        {
                            Rebate_No = dr["Rebate_No"].ToString(),
                            Rebate_Type = dr["Rebate_Type"].ToString(),
                            AccountNumber = dr["AccountNumber"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            MaritalStatus = dr["MaritalStatus"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            FirstNames = dr["FirstNames"].ToString(),
                            IDNumber = dr["IDNumber"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            CitySuburb = dr["CitySuburb"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PostalAddress = dr["PostalAddress"].ToString(),
                            PostalAddressCitySuburb = dr["PostalAddressCitySuburb"].ToString(),
                            PostalAddressPostalCode = dr["PostalAddressPostalCode"].ToString(),
                            HomeTel = dr["HomeTel"].ToString(),
                            CellNo = dr["CellNo"].ToString(),
                            WorkTel = dr["WorkTel"].ToString(),
                            Email = dr["Email"].ToString(),
                            AccountNo_MultipleUnits = dr["AccountNo_MultipleUnits"].ToString(),
                            Contact_CellNo = dr["Contact_CellNo"].ToString(),
                            SchemeBuilding = dr["SchemeBuilding"].ToString(),
                            ErfandTownship = dr["ErfandTownship"].ToString(),
                            NameOfTheOrganisation = dr["NameOfTheOrganisation"].ToString(),
                            RegistrationNumber = dr["RegistrationNumber"].ToString(),
                            NameSurnameDesignation = dr["NameSurnameDesignation"].ToString(),
                            RegisteredWithSARS = dr["RegisteredWithSARS"].ToString(),
                            NameOfApplicant = dr["NameOfApplicant"].ToString(),
                            NameOfHeritageProperty = dr["NameOfHeritageProperty"].ToString(),
                            BriefDescriptionHS = dr["BriefDescriptionHS"].ToString(),
                            NationalHeritageSite = dr["NationalHeritageSite"].ToString(),
                            ProvincialHeritageSite = dr["ProvincialHeritageSite"].ToString(),
                            HeritageArea = dr["HeritageArea"].ToString(),
                            ProvisionalProtection = dr["ProvisionalProtection"].ToString(),
                            Other = dr["Other"].ToString(),
                            SpecifyOther = dr["SpecifyOther"].ToString(),
                            NumberDateGovernmentNotice = dr["NumberDateGovernmentNotice"].ToString(),
                            DetailsGazette = dr["DetailsGazette"].ToString(),
                            FormerProtection = dr["FormerProtection"].ToString(),
                            OperationTime = dr["OperationTime"].ToString(),
                            ContactPersonWithCOJStatements = dr["ContactPersonWithCOJStatements"].ToString(),
                            LargeOrganisation = dr["LargeOrganisation"].ToString(),
                            LargeOrganisationDescription = dr["LargeOrganisationDescription"].ToString(),
                            NoPermantStaff = dr["NoPermantStaff"].ToString(),
                            FulltimeScholars = dr["FulltimeScholars"].ToString(),
                            NoOfScholarsPayingFull = dr["NoOfScholarsPayingFull"].ToString(),
                            NoOfScholarsNotPaying = dr["NoOfScholarsNotPaying"].ToString(),
                            EstimatedRevenuePayingFees = dr["EstimatedRevenuePayingFees"].ToString(),
                            EstimatedRevenueNotPayingFees = dr["EstimatedRevenueNotPayingFees"].ToString(),
                            ExpectedIncome1 = dr["ExpectedIncome1"].ToString(),
                            ExpectedIncome2 = dr["ExpectedIncome2"].ToString(),
                            ExpectedIncome3 = dr["ExpectedIncome3"].ToString(),
                            ExpectedIncome4 = dr["ExpectedIncome4"].ToString(),
                            EIPastYear1 = dr["EIPastYear1"].ToString(),
                            EIPastYear2 = dr["EIPastYear2"].ToString(),
                            EIPastYear3 = dr["EIPastYear3"].ToString(),
                            EIPastYear4 = dr["EIPastYear4"].ToString(),
                            EICurrentYear1 = dr["EICurrentYear1"].ToString(),
                            EICurrentYear2 = dr["EICurrentYear2"].ToString(),
                            EICurrentYear3 = dr["EICurrentYear3"].ToString(),
                            EICurrentYear4 = dr["EICurrentYear4"].ToString(),
                            EINextYear1 = dr["EINextYear1"].ToString(),
                            EINextYear2 = dr["EINextYear2"].ToString(),
                            EINextYear3 = dr["EINextYear3"].ToString(),
                            EINextYear4 = dr["EINextYear4"].ToString(),
                            Expediture1 = dr["Expediture1"].ToString(),
                            Expediture2 = dr["Expediture2"].ToString(),
                            Expediture3 = dr["Expediture3"].ToString(),
                            ExpediturePastYear1 = dr["ExpediturePastYear1"].ToString(),
                            ExpediturePastYear2 = dr["ExpediturePastYear2"].ToString(),
                            ExpediturePastYear3 = dr["ExpediturePastYear3"].ToString(),
                            ExpeditureCurrentYear1 = dr["ExpeditureCurrentYear1"].ToString(),
                            ExpeditureCurrentYear2 = dr["ExpeditureCurrentYear2"].ToString(),
                            ExpeditureCurrentYear3 = dr["ExpeditureCurrentYear3"].ToString(),
                            ExpeditureNextYear1 = dr["ExpeditureNextYear1"].ToString(),
                            ExpeditureNextYear2 = dr["ExpeditureNextYear2"].ToString(),
                            ExpeditureNextYear3 = dr["ExpeditureNextYear3"].ToString(),
                            SalaryPastYear = dr["SalaryPastYear"].ToString(),
                            SalaryCurrentYear = dr["SalaryCurrentYear"].ToString(),
                            SalaryNextYear = dr["SalaryNextYear"].ToString(),
                            BonusPastYear = dr["BonusPastYear"].ToString(),
                            BonusCurrentYear = dr["BonusCurrentYear"].ToString(),
                            BonusNextYear = dr["BonusNextYear"].ToString()
                        });
                    }
                    conn.Close();

                    ViewBag.Education = RebatesViewModels.ToList();

                    foreach (var items in ViewBag.Education)
                    {
                        TempData["Rebate_No"] = @items.Rebate_No;
                        TempData["AccountNumber"] = @items.AccountNumber;
                        TempData["IDNumber"] = @items.IDNumber;
                        TempData["Gender"] = @items.Gender;
                        TempData["MaritalStatus"] = @items.MaritalStatus;
                        TempData["Surname"] = @items.Surname;
                        TempData["FirstNames"] = @items.FirstNames;
                        TempData["SpouseSurname"] = @items.SpouseSurname;
                        TempData["SpouseFirstNames"] = @items.SpouseFirstNames;
                        TempData["SpouseIDNumber"] = @items.SpouseIDNumber;
                        TempData["StreetAddress"] = @items.StreetAddress;
                        TempData["CitySuburb"] = @items.CitySuburb;
                        TempData["PostalCode"] = @items.PostalCode;
                        TempData["PostalAddress"] = @items.PostalAddress;
                        TempData["PostalAddressCitySuburb"] = @items.PostalAddressCitySuburb;
                        TempData["PostalAddressPostalCode"] = @items.PostalAddressPostalCode;
                        TempData["HomeTel"] = @items.HomeTel;
                        TempData["CellNo"] = @items.CellNo;
                        TempData["WorkTel"] = @items.WorkTel;
                        TempData["Email"] = @items.Email;
                        TempData["AccountNo_MultipleUnits"] = @items.AccountNo_MultipleUnits;
                        TempData["Contact_CellNo"] = @items.Contact_CellNo;
                        TempData["SchemeBuilding"] = @items.SchemeBuilding;
                        TempData["ErfandTownship"] = @items.ErfandTownship;
                        TempData["NameOfTheOrganisation"] = @items.NameOfTheOrganisation;
                        TempData["RegistrationNumber"] = @items.RegistrationNumber;
                        TempData["NameSurnameDesignation"] = @items.NameSurnameDesignation;
                        TempData["RegisteredWithSARS"] = @items.RegisteredWithSARS;
                        TempData["NameOfApplicant"] = @items.NameOfApplicant;
                        TempData["NameOfHeritageProperty"] = @items.NameOfHeritageProperty;
                        TempData["BriefDescriptionHS"] = @items.BriefDescriptionHS;
                        TempData["NationalHeritageSite"] = @items.NationalHeritageSite;
                        TempData["ProvincialHeritageSite"] = @items.ProvincialHeritageSite;
                        TempData["HeritageArea"] = @items.HeritageArea;
                        TempData["ProvisionalProtection"] = @items.ProvisionalProtection;
                        TempData["Other"] = @items.Other;
                        TempData["SpecifyOther"] = @items.SpecifyOther;
                        TempData["NumberDateGovernmentNotice"] = @items.NumberDateGovernmentNotice;
                        TempData["DetailsGazette"] = @items.DetailsGazette;
                        TempData["FormerProtection"] = @items.FormerProtection;
                        TempData["OperationTime"] = @items.OperationTime;
                        TempData["ContactPersonWithCOJStatements"] = @items.ContactPersonWithCOJStatements;
                        TempData["LargeOrganisation"] = @items.LargeOrganisation;
                        TempData["LargeOrganisationDescription"] = @items.LargeOrganisationDescription;
                        TempData["NoPermantStaff"] = @items.NoPermantStaff;
                        TempData["FulltimeScholars"] = @items.FulltimeScholars;
                        TempData["NoOfScholarsPayingFull"] = @items.NoOfScholarsPayingFull;
                        TempData["NoOfScholarsNotPaying"] = @items.NoOfScholarsNotPaying;
                        TempData["EstimatedRevenuePayingFees"] = @items.EstimatedRevenuePayingFees;
                        TempData["EstimatedRevenueNotPayingFees"] = @items.EstimatedRevenueNotPayingFees;
                        TempData["ExpectedIncome1"] = @items.ExpectedIncome1;
                        TempData["ExpectedIncome2"] = @items.ExpectedIncome2;
                        TempData["ExpectedIncome3"] = @items.ExpectedIncome3;
                        TempData["ExpectedIncome4"] = @items.ExpectedIncome4;
                        TempData["EIPastYear1"] = @items.EIPastYear1;
                        TempData["EIPastYear2"] = @items.EIPastYear2;
                        TempData["EIPastYear3"] = @items.EIPastYear3;
                        TempData["EIPastYear4"] = @items.EIPastYear4;
                        TempData["EICurrentYear1"] = @items.EICurrentYear1;
                        TempData["EICurrentYear2"] = @items.EICurrentYear2;
                        TempData["EICurrentYear3"] = @items.EICurrentYear3;
                        TempData["EICurrentYear4"] = @items.EICurrentYear4;
                        TempData["EINextYear1"] = @items.EINextYear1;
                        TempData["EINextYear2"] = @items.EINextYear2;
                        TempData["EINextYear3"] = @items.EINextYear3;
                        TempData["EINextYear4"] = @items.EINextYear4;
                        TempData["Expediture1"] = @items.Expediture1;
                        TempData["Expediture2"] = @items.Expediture2;
                        TempData["Expediture3"] = @items.Expediture3;
                        TempData["ExpediturePastYear1"] = items.ExpediturePastYear1;
                        TempData["ExpediturePastYear2"] = items.ExpediturePastYear2;
                        TempData["ExpediturePastYear3"] = items.ExpediturePastYear3;
                        TempData["ExpeditureCurrentYear1"] = items.ExpeditureCurrentYear1;
                        TempData["ExpeditureCurrentYear2"] = items.ExpeditureCurrentYear2;
                        TempData["ExpeditureCurrentYear3"] = items.ExpeditureCurrentYear3;
                        TempData["ExpeditureNextYear1"] = items.ExpeditureNextYear1;
                        TempData["ExpeditureNextYear2"] = items.ExpeditureNextYear2;
                        TempData["ExpeditureNextYear3"] = items.ExpeditureNextYear3;
                        TempData["SalaryPastYear"] = items.SalaryPastYear;
                        TempData["SalaryCurrentYear"] = items.SalaryCurrentYear;
                        TempData["SalaryNextYear"] = items.SalaryNextYear;
                        TempData["BonusPastYear"] = items.BonusPastYear;
                        TempData["BonusCurrentYear"] = items.BonusCurrentYear;
                        TempData["BonusNextYear"] = items.BonusNextYear;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("_Education", RebatesViewModels);
            }

            return View();
        }

        public IActionResult ApproveReject(string? RebateNo, string? approval, string? approverComment, string? RebateType, string? approverComment4)
        {
            var user = HttpContext.User;
            var userName = user.Identity.Name;
            var AccountNumber = TempData["AccountNumber"];

            if (approval == "Approved")
            {
                //update
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;

                    string currentUserFirstname = TempData["currentUserFirstname"].ToString();
                    string currentUserSurname = TempData["currentUserSurname"].ToString();

                    com.CommandText = "EXEC UpdateRebateInfoApprove @RebateNo, @ApproverComment, @ApproverComment4, @CurrentUserFirstname, @CurrentUserSurname";
                    // Add parameters
                    com.Parameters.AddWithValue("@RebateNo", string.IsNullOrEmpty(RebateNo) ? DBNull.Value : RebateNo);
                    com.Parameters.AddWithValue("@ApproverComment", string.IsNullOrEmpty(approverComment) ? DBNull.Value : approverComment);
                    com.Parameters.AddWithValue("@ApproverComment4", string.IsNullOrEmpty(approverComment4) ? DBNull.Value : approverComment4);
                    com.Parameters.AddWithValue("@CurrentUserFirstname", string.IsNullOrEmpty(currentUserFirstname) ? DBNull.Value : currentUserFirstname);
                    com.Parameters.AddWithValue("@CurrentUserSurname", string.IsNullOrEmpty(currentUserSurname) ? DBNull.Value : currentUserSurname);

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel
                        {
                           
                        });
                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //Insert Into Rebate Audit Table 
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }

                try
                {
                    conn.Open();
                    com.Connection = conn;

                    string currentUserFullName = TempData["currentUserFirstname"].ToString() + ' ' + TempData["currentUserSurname"].ToString();

                    com.CommandText = "EXEC InsertRebateAuditApprove @RebateNo, @AccountNumber, @ApproverFullName, @ApproverComment";

                    com.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                    com.Parameters.AddWithValue("@ApproverFullName", currentUserFullName);

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel
                        {

                        });
                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                string emailAddress = TempData["Email"]?.ToString();
                string rebateNo = TempData["Rebate_No"]?.ToString();

                emailHelper.SendEmailNotificationRebateApprove(TempData["Email"].ToString(), TempData["Rebate_No"].ToString());               

                //Delete
                //if (RebatesViewModels.Count > 0)
                //{
                //    RebatesViewModels.Clear();
                //}
                //try
                //{
                //    conn.Open();
                //    com.Connection = conn;
                //    com.CommandText = "DELETE FROM [Objection].[dbo].[Rebate_Info_Final] where Rebate_No = '" + RebateNo + "'";
                //    dr = com.ExecuteReader();
                //    while (dr.Read())
                //    {
                //        RebatesViewModels.Add(new RebatesViewModel
                //        {

                //        });
                //    }
                //    conn.Close();

                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}

                return RedirectToAction("Display");

                //return RedirectToAction("Acknowledge");
            }

            if (approval == "Rejected")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;

                    string currentUserFirstname = TempData["currentUserFirstname"].ToString();
                    string currentUserSurname = TempData["currentUserSurname"].ToString();

                    com.CommandText = "EXEC UpdateRebateInfoAdminReject @RebateNo, @ApproverComment, @ApproverComment4, @CurrentUserFirstname, @CurrentUserSurname";

                    com.Parameters.AddWithValue("@RebateNo", string.IsNullOrEmpty(RebateNo) ? DBNull.Value : RebateNo);
                    com.Parameters.AddWithValue("@ApproverComment", string.IsNullOrEmpty(approverComment) ? DBNull.Value : approverComment);
                    com.Parameters.AddWithValue("@ApproverComment4", string.IsNullOrEmpty(approverComment4) ? DBNull.Value : approverComment4);
                    com.Parameters.AddWithValue("@CurrentUserFirstname", string.IsNullOrEmpty(currentUserFirstname) ? DBNull.Value : currentUserFirstname);
                    com.Parameters.AddWithValue("@CurrentUserSurname", string.IsNullOrEmpty(currentUserSurname) ? DBNull.Value : currentUserSurname);                    

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel
                        {

                        });
                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //Insert Rebate Audit Table
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;

                    string currentUserFullName = TempData["currentUserFirstname"].ToString() + ' ' + TempData["currentUserSurname"].ToString();

                    com.CommandText = "EXEC InsertRebateAuditAdminReject @RebateNo, @AccountNumber, @ApproverFullName, @ApproverComment";

                    com.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                    com.Parameters.AddWithValue("@ApproverFullName", currentUserFullName);

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel
                        {

                        });
                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                emailHelper.SendEmailNotificationRebateReject(TempData["Email"].ToString(), TempData["Rebate_No"].ToString());

                //Delete
                //if (RebatesViewModels.Count > 0)
                //{
                //    RebatesViewModels.Clear();
                //}
                //try
                //{
                //    conn.Open();
                //    com.Connection = conn;
                //    com.CommandText = "DELETE FROM [Objection].[dbo].[Rebate_Info_Final] where Rebate_No = '" + RebateNo + "'";
                //    dr = com.ExecuteReader();
                //    while (dr.Read())
                //    {
                //        RebatesViewModels.Add(new RebatesViewModel
                //        {

                //        });
                //    }
                //    conn.Close();

                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}

                return RedirectToAction("Display");

                //return RedirectToAction("Acknowledge");
            }

            if (approval == "Pending Approval")
            {
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;

                    string currentUserFirstname = TempData["currentUserFirstname"].ToString();
                    string currentUserSurname = TempData["currentUserSurname"].ToString();

                    com.CommandText = "EXEC UpdateRebateInfoPendingApproval @RebateNo, @ApproverComment, @CurrentUserFirstname, @CurrentUserSurname";

                    com.Parameters.AddWithValue("@RebateNo", RebateNo);
                    com.Parameters.AddWithValue("@ApproverComment", approverComment);
                    com.Parameters.AddWithValue("@CurrentUserFirstname", currentUserFirstname);
                    com.Parameters.AddWithValue("@CurrentUserSurname", currentUserSurname);

                    if (!string.IsNullOrEmpty(approverComment4))
                    {
                        com.Parameters.AddWithValue("@ApproverComment4", approverComment4);
                    }
                    else
                    {
                        // Use DBNull.Value for SQL NULL
                        com.Parameters.AddWithValue("@ApproverComment4", DBNull.Value);
                    }

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel
                        {

                        });
                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //Insert Rebate Audit Table
                if (RebatesViewModels.Count > 0)
                {
                    RebatesViewModels.Clear();
                }
                try
                {
                    conn.Open();
                    com.Connection = conn;

                    string currentUserFullName = TempData["currentUserFirstname"].ToString() + ' ' + TempData["currentUserSurname"].ToString();

                    com.CommandText = "EXEC InsertRebateAuditPendingApproval @RebateNo, @AccountNumber, @ApproverFullName, @ApproverComment";

                    com.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                    com.Parameters.AddWithValue("@ApproverFullName", currentUserFullName);

                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        RebatesViewModels.Add(new RebatesViewModel
                        {

                        });
                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //emailHelper.SendEmailNotificationRebateReject(TempData["Email"].ToString(), TempData["Rebate_No"].ToString());

                //Delete
                //if (RebatesViewModels.Count > 0)
                //{
                //    RebatesViewModels.Clear();
                //}
                //try
                //{
                //    conn.Open();
                //    com.Connection = conn;
                //    com.CommandText = "DELETE FROM [Objection].[dbo].[Rebate_Info_Final] where Rebate_No = '" + RebateNo + "'";
                //    dr = com.ExecuteReader();
                //    while (dr.Read())
                //    {
                //        RebatesViewModels.Add(new RebatesViewModel
                //        {

                //        });
                //    }
                //    conn.Close();

                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}

                return RedirectToAction("Display");

                //return RedirectToAction("Acknowledge");
            }

            return View();
        }

        public IActionResult Display()
        {
            return View();
        }

        public IActionResult DownloadFiles(string? RebateNo)
        {
            string folderPath = @"E:\\RebateData\\" + RebateNo + "";

            string[] pdfFilePaths = Directory.GetFiles(folderPath, "*.pdf");

            using (var memoryStream = new MemoryStream())
            {
                using (var combinedPdf = new PdfDocument(new PdfWriter(memoryStream)))
                {
                    foreach (var pdfFilePath in pdfFilePaths)
                    {
                        using (var pdfDocument = new PdfDocument(new PdfReader(pdfFilePath)))
                        {
                            pdfDocument.CopyPagesTo(1, pdfDocument.GetNumberOfPages(), combinedPdf);
                        }
                    }
                }

                byte[] combinedPdfData = memoryStream.ToArray();

                //memoryStream.Position = 0;

                using (var finalMemoryStream = new MemoryStream())
                {
                    //memoryStream.CopyTo(finalMemoryStream);

                    finalMemoryStream.Position = 0;

                    return File(combinedPdfData, "application/pdf", "" + RebateNo + " Files.pdf");
                }
            }
        }

        //Starting with Acknowledgement Letter
        public IActionResult DownloadFiles1(string? RebateNo) 
        {
            string folderPath = @"E:\\RebateData\\" + RebateNo + "";

            string startingFileName = folderPath.Contains("Acknowledgement").ToString();           

            string[] pdfFilePaths = Directory.GetFiles(folderPath, "*.pdf")
            .OrderBy(filePath => filePath.Contains("Acknowledgement") ? 0 : 1) 
            .ToArray();

            using (var memoryStream = new MemoryStream())
            {
                using (var combinedPdf = new PdfDocument(new PdfWriter(memoryStream)))
                {
                    foreach (var pdfFilePath in pdfFilePaths)
                    {
                        using (var pdfDocument = new PdfDocument(new PdfReader(pdfFilePath)))
                        {
                            pdfDocument.CopyPagesTo(1, pdfDocument.GetNumberOfPages(), combinedPdf);
                        }
                    }
                }

                byte[] combinedPdfData = memoryStream.ToArray();

                //memoryStream.Position = 0;

                using (var finalMemoryStream = new MemoryStream())
                {
                    //memoryStream.CopyTo(finalMemoryStream);

                    finalMemoryStream.Position = 0;

                    return File(combinedPdfData, "application/pdf", "" + RebateNo + " Files.pdf");
                }
            }
        }

        public IActionResult AdminTasks()
        {
            if (applicationStatistics.Count > 0)
            {
                applicationStatistics.Clear();
            }
            try
            {
                conn.Open();
                com.Connection = conn;
                com.CommandText = "SELECT COUNT(Status) Rebates_To_Processed, [UserID SAP Number] FROM [Objection].[dbo].[Rebate_Infos] WHERE status IN ('Acknowledge', 'Auto Reject') GROUP BY [UserID SAP Number]";

                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    applicationStatistics.Add(new ApplicationStatistics
                    {
                        Name = dr["UserID SAP Number"].ToString(),
                        NumberOfApplications = (int)dr["Rebates_To_Processed"],
                    });
                }

                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }

            return View(applicationStatistics);
        }

        public async Task<IActionResult> PBOAcknowledge(string? userName)
        {
            var user = HttpContext.User;
            //var userName = user.Identity.Name;

            if (RebatesViewModels.Count > 0)
            {
                RebatesViewModels.Clear();
            }

            try
            {
                conn.Open();
                com.Connection = conn;
                com.CommandText = "EXEC GetRebateInfos_PBO";

                dr = com.ExecuteReader();

                while (dr.Read())
                {
                    RebatesViewModels.Add(new RebatesViewModel()
                    {
                        Rebate_No = dr["Rebate_No"].ToString(),
                        AccountNumber = dr["AccountNumber"].ToString(),
                        IDNumber = dr["IDNumber"].ToString(),
                        Rebate_Type = dr["Rebate_Type"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        Surname = dr["Surname"].ToString(),
                        FirstNames = dr["FirstNames"].ToString(),
                        Status = dr["Status"].ToString(),
                        DateOfSubmission = dr["DateOfSubmission"].ToString(),
                        Email = dr["Email"].ToString(),
                        UserID_SAP_Number = dr["UserID SAP Number"].ToString()
                    });
                }
                conn.Close();

                ViewBag.Rebates = RebatesViewModels.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("PBOAcknowledge", new { userName = userName });
        }

        public async Task<IActionResult> PBOPending(string? userName)
        {
            var user = HttpContext.User;
            //var userName = user.Identity.Name;

            if (RebatesViewModels.Count > 0)
            {
                RebatesViewModels.Clear();
            }

            try
            {
                conn.Open();
                com.Connection = conn;
                com.CommandText = "EXEC GetRebateInfos_PBO_Pending";

                dr = com.ExecuteReader();

                while (dr.Read())
                {
                    RebatesViewModels.Add(new RebatesViewModel()
                    {
                        Rebate_No = dr["Rebate_No"].ToString(),
                        AccountNumber = dr["AccountNumber"].ToString(),
                        IDNumber = dr["IDNumber"].ToString(),
                        Rebate_Type = dr["Rebate_Type"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        Surname = dr["Surname"].ToString(),
                        FirstNames = dr["FirstNames"].ToString(),
                        Status = dr["Status"].ToString(),
                        DateOfSubmission = dr["DateOfSubmission"].ToString(),
                        Email = dr["Email"].ToString(),
                        UserID_SAP_Number = dr["UserID SAP Number"].ToString()
                    });
                }
                conn.Close();

                ViewBag.Rebates = RebatesViewModels.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("PBOPending", new { userName = userName });
        }
    }
}
