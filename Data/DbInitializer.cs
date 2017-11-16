using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using szakdoga.Models;

namespace szakdoga.Data
{
    public class DbInitializer
    {
        private AppDbContext _context;
        private IConfigurationRoot _cfg;

        public DbInitializer(AppDbContext context, IConfigurationRoot cfg)
        {
            _context = context;
            _cfg = cfg;
        }

        public void Seed()
        {
            CleanAllTables(_context);

            if (!_context.User.Any())
                _context.User.AddRange(Users);
            if (!_context.Query.Any())
                _context.Query.AddRange(Queries);

            _context.SaveChanges();
            if (!_context.Report.Any())
                _context.Report.AddRange(Reports);
            if (!_context.Dashboards.Any())
                _context.Dashboards.AddRange(Dashboards);
            if (!_context.ReportDashboardRel.Any())
                _context.ReportDashboardRel.AddRange(ReportDashboardRels);
            if (!_context.UserDashboardRel.Any())
                _context.UserDashboardRel.AddRange(UserDashboardRels);
            if (!_context.ReportUserRel.Any())
                _context.ReportUserRel.AddRange(ReportUserRels);

            _context.SaveChanges();

            string sourceConn = _cfg.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(sourceConn))
            {
                ExecuteBatchNonQuery(cmdText, conn);
            }
        }

        private static List<User> users;

        private static List<User> Users
        {
            get
            {
                if (users == null)
                    users = new List<User>{
                        new User{ Name="Admin", Password="admin", EmailAddress="asd@asd.com", UserGUID="674a8382-dfb0-41d9-a349-a85599cc0de6"},
                        new User{ Name="Teszt", Password="teszt",EmailAddress="teszt@teszt.com",UserGUID="b12b3382-a250-48ca-9523-cf99b1826600"}
                    };
                return users;
            }
        }

        private static List<ReportUserRel> reportUserRels;

        private static List<ReportUserRel> ReportUserRels
        {
            get
            {
                if (reportUserRels == null)
                    reportUserRels = new List<ReportUserRel>   {
                            new ReportUserRel {  User=Users.FirstOrDefault(x=>x.Name.Equals("Admin")), AuthoryLayer=(int)RepotUserPermissions.CanModify, Report=Reports.FirstOrDefault(x=>x.Name.Equals("Riport1"))},
                            new ReportUserRel {  User=Users.FirstOrDefault(x=>x.Name.Equals("Teszt")), AuthoryLayer=(int)RepotUserPermissions.CanWatch, Report=Reports.FirstOrDefault(x=>x.Name.Equals("Riport2"))}
                                                                };
                return reportUserRels;
            }
        }

        private static List<UserDashboardRel> userDashboardRels;

        private static List<UserDashboardRel> UserDashboardRels
        {
            get
            {
                if (userDashboardRels == null)
                    userDashboardRels = new List<UserDashboardRel>
                            {
                                new UserDashboardRel { User=Users.FirstOrDefault(x=>x.Name.Equals("Admin")), Dashboard=Dashboards.FirstOrDefault(x=>x.Name.Equals("Dashboard1")), AuthoryLayer=(int)DashboardUserPermissions.CanModify},
                                new UserDashboardRel { User=Users.FirstOrDefault(x=>x.Name.Equals("Teszt")), Dashboard=Dashboards.FirstOrDefault(x=>x.Name.Equals("Dashboard2")), AuthoryLayer=(int)DashboardUserPermissions.CanWatch}
                            };
                return userDashboardRels;
            }
        }

        private static List<ReportDashboardRel> reportDashboardRels;

        private static List<ReportDashboardRel> ReportDashboardRels
        {
            get
            {
                if (reportDashboardRels == null)
                    reportDashboardRels = new List<ReportDashboardRel> {
                            new ReportDashboardRel{ Dashboard=Dashboards.FirstOrDefault(x=>x.Name.Equals("Dashboard1")) , Position="1", Report=Reports.FirstOrDefault(x=>x.Name.Equals("Riport1")) },
                            new ReportDashboardRel{ Dashboard=Dashboards.FirstOrDefault(x=>x.Name.Equals("Dashboard2")) , Position="2", Report=Reports.FirstOrDefault(x=>x.Name.Equals("Riport2")) }
                        };
                return reportDashboardRels;
            }
        }

        private static List<Dashboard> dashboards;

        private static List<Dashboard> Dashboards
        {
            get
            {
                if (dashboards == null)
                {
                    dashboards = new List<Dashboard> {
                               new Dashboard{ Name = "Dashboard1", DashBoardGUID = "9b4ea50f-8a05-4025-ab01-0072894691e6", Style = "style" , LastModifier=Users.FirstOrDefault(x=>x.Name.Equals("Admin")), Author=Users.FirstOrDefault(x=>x.Name.Equals("Admin"))},
                               new Dashboard{ Name = "Dashboard2", DashBoardGUID = "2adccadc-7a05-419d-b8c3-9578db9a81dc", Style = "style",LastModifier=Users.FirstOrDefault(x=>x.Name.Equals("Teszt")), Author=Users.FirstOrDefault(x=>x.Name.Equals("Teszt")) } };
                }
                return dashboards;
            }
        }

        private static List<Report> reports;

        private static List<Report> Reports
        {
            get
            {
                if (reports == null)
                {
                    reports = new List<Report> {
                            new Report{Name = "Riport1", Query = Queries.FirstOrDefault(x=>x.Name.Equals("Számlák")), Style = "style json", ReportGUID="b2fc0e93-4260-47bb-9757-e682f077dd27", LastModifier=Users.FirstOrDefault(x=>x.Name.Equals("Admin")), Author=Users.FirstOrDefault(x=>x.Name.Equals("Admin"))},
                            new Report{Name = "Riport2", Query = Queries.FirstOrDefault(x=>x.Name.Equals("Számlák")), Style = "style json", ReportGUID="51adb95f-161b-4473-95ff-e0d6392f5caa" ,LastModifier=Users.FirstOrDefault(x=>x.Name.Equals("Teszt")) , Author=Users.FirstOrDefault(x=>x.Name.Equals("Teszt"))}};
                }
                return reports;
            }
        }

        private static List<Query> queries;

        private static List<Query> Queries
        {
            get
            {
                if (queries == null)
                {
                    queries = new List<Query> {
                            new Query{  SQL = sql1, Name = "Számlák",TranslatedColumnNames=columns, QueryGUID = "3066e94b-ff9e-454c-ab58-6a88436e4b52", NextUpdating = System.DateTime.Now, UpdatePeriod = new TimeSpan(1, 0, 0, 0),ResultTableName="a73bcfb4b6d714f32bedcf68a48a52fc5" } };
                }
                return queries;
            }
        }

        private static string columns = @"{ ""PrimeryKeyColumn"":""Table_95_Field_1"",
                                            ""Columns"":	[
											{ ""Name"": ""Table_95_Field_1"", ""Text"": ""SzámlafejID"", ""Hidden"": ""true"" ,""Type"": ""number"" },
											{ ""Name"": ""Table_95_Field_3"", ""Text"": ""Bizonylat azonosító"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                 	                        { ""Name"": ""Table_95_Field_5"", ""Text"": ""Bizonylattömb"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                             	            { ""Name"": ""Table_95_Field_7"", ""Text"": ""Bizonylatszám"", ""Hidden"": ""false"" ,""Type"": ""string"" },
                                        	{ ""Name"": ""Table_95_Field_9"", ""Text"": ""Stornó bizonylat"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_11"", ""Text"": ""Helyesbítő bizonylat"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_13"", ""Text"": ""Ügyfél"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_15"", ""Text"": ""Ügyfélcím"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_17"", ""Text"": ""Kapcsolattartó"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_19"", ""Text"": ""Cégprofil"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_21"", ""Text"": ""Valuta"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_23"", ""Text"": ""Részlegszám"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_25"", ""Text"": ""Munkaszám"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_27"", ""Text"": ""Projekt"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_29"", ""Text"": ""Bizonylat"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_31"", ""Text"": ""Kelte"", ""Hidden"": ""false"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_33"", ""Text"": ""Teljesítés dátuma"", ""Hidden"": ""false"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_35"", ""Text"": ""Esedékesség"", ""Hidden"": ""false"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_37"", ""Text"": ""Fizetési mód"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_39"", ""Text"": ""Raktár azonosító"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_41"", ""Text"": ""Nettó érték"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_43"", ""Text"": ""Áfa értékű"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_45"", ""Text"": ""Bruttó érték"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_47"", ""Text"": ""Fennmaradó összeg"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_49"", ""Text"": ""Kiegyenlített összeg"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_51"", ""Text"": ""? Kiterjesztő azonosító?"", ""Hidden"": ""false"" ,""Type"": ""string"" },
                                        	{ ""Name"": ""Table_95_Field_53"", ""Text"": ""Felhasználható"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_55"", ""Text"": ""Raktári bevét"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""TopComment"", ""Text"": ""Felső megjegyzés"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Comment"", ""Text"": ""Megjegyzés"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""ReportRepository"", ""Text"": ""Bizonylatkép"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_63"", ""Text"": ""Bizonylat oldalainak száma"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_65"", ""Text"": ""RowVersion"", ""Hidden"": ""true"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_67"", ""Text"": ""Vevő"", ""Hidden"": ""false"" ,""Type"": ""string"" },
                                        	{ ""Name"": ""Table_95_Field_69"", ""Text"": ""Vevő kód"", ""Hidden"": ""false"" ,""Type"": ""string"" },
                                        	{ ""Name"": ""Table_95_Field_71"", ""Text"": ""Rögzítés dátuma"", ""Hidden"": ""false"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_73"", ""Text"": ""Munkatárs"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_75"", ""Text"": ""Logokép"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_77"", ""Text"": ""Teljes bizonylatnév"", ""Hidden"": ""false"" ,""Type"": ""string"" },
                                        	{ ""Name"": ""Table_95_Field_79"", ""Text"": ""Végső termék"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_81"", ""Text"": ""Végső terméknév"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_83"", ""Text"": ""Lockolt"", ""Hidden"": ""true"" ,""Type"": ""boolean"" },
                                        	{ ""Name"": ""Table_95_Field_85"", ""Text"": ""Áfa nélkül"", ""Hidden"": ""false"" ,""Type"": ""boolean"" },
                                        	{ ""Name"": ""Table_95_Field_87"", ""Text"": ""Nyelv"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_89"", ""Text"": ""Kiegyenlítés dátuma"", ""Hidden"": ""false"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_91"", ""Text"": ""ReportDesign"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_93"", ""Text"": ""IntrastatTransactionCode"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_95"", ""Text"": ""IntrastatDeliveryTermCode"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_97"", ""Text"": ""IntrastatModeOfTransportCode"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_99"", ""Text"": ""IntrastatCountryNonEU"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_101"", ""Text"": ""IntrastatCountryEU"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_103"", ""Text"": ""Nettó súly"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_105"", ""Text"": ""Bruttó súly"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_107"", ""Text"": ""Bizonylat státusza"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_109"", ""Text"": ""SettlementPeriodDate"", ""Hidden"": ""false"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_111"", ""Text"": ""Eszámla"", ""Hidden"": ""false"" ,""Type"": ""boolean"" },
                                        	{ ""Name"": ""Table_95_Field_113"", ""Text"": ""EDI számla"", ""Hidden"": ""false"" ,""Type"": ""boolean"" },
                                        	{ ""Name"": ""Table_95_Field_115"", ""Text"": ""Házipénztár"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_117"", ""Text"": ""Kiegyenlített összeg"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_119"", ""Text"": ""Fennmaradó összeg"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_121"", ""Text"": ""Bizonylat sablon"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_123"", ""Text"": ""Nyomtatás dátuma"", ""Hidden"": ""false"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_125"", ""Text"": ""Távoli nyomtatás hatálya"", ""Hidden"": ""false"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_127"", ""Text"": ""Távoli nyomtatás dátuma"", ""Hidden"": ""false"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_129"", ""Text"": ""Egységár tizedesjegyek"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_131"", ""Text"": ""Nettó érték tizedes jegyek"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_133"", ""Text"": ""ÁFA érték tizedes jegyek"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_135"", ""Text"": ""Bruttó érték tizedes jegyek"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_137"", ""Text"": ""GrandTotalDigits"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_139"", ""Text"": ""Valuta szorzó"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_141"", ""Text"": ""Szállítási cím"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_143"", ""Text"": ""Szállítási mód"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_145"", ""Text"": ""PaymentAccounting"", ""Hidden"": ""false"" ,""Type"": ""boolean"" },
                                        	{ ""Name"": ""InheretedTopComment"", ""Text"": ""Örökölt felső megjegyzés"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""InheretedBottomComment"", ""Text"": ""Örökölt alső megjegyzés"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_151"", ""Text"": ""SalesOpportunity"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_153"", ""Text"": ""DetailGroupByType"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_155"", ""Text"": ""RetentionWarranty"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_157"", ""Text"": ""RetentionPaymentDate"", ""Hidden"": ""true"" ,""Type"": ""date"" },
                                        	{ ""Name"": ""Table_95_Field_159"", ""Text"": ""AppliedPriceRule"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_161"", ""Text"": ""AccountingStockMovementType"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_163"", ""Text"": ""Webshop azonosító"", ""Hidden"": ""false"" ,""Type"": ""string"" },
                                        	{ ""Name"": ""Description"", ""Text"": ""Leírás"", ""Hidden"": ""false"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_167"", ""Text"": ""ActualCurrencyRate"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_169"", ""Text"": ""DiscountMode"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_171"", ""Text"": ""FulfilledCertificate"", ""Hidden"": ""true"" ,""Type"": ""boolean"" },
                                        	{ ""Name"": ""Table_95_Field_173"", ""Text"": ""CustomerStockInIntermediate"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_175"", ""Text"": ""MadeWithCashRegister"", ""Hidden"": ""true"" ,""Type"": ""boolean"" },
                                        	{ ""Name"": ""NAVRepository"", ""Text"": ""NAVRepository,"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_179"", ""Text"": ""SecQuantity"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_181"", ""Text"": ""SecQuantityUnit"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_183"", ""Text"": ""SecQuantityDigits"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_185"", ""Text"": ""ConversionRate"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_187"", ""Text"": ""QuantityUnit"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_189"", ""Text"": ""QuantityDigits"", ""Hidden"": ""true"" ,""Type"": ""number"" },
                                        	{ ""Name"": ""Table_95_Field_191"", ""Text"": ""Magánszemély"", ""Hidden"": ""true"",""Type"":""boolean"" }
                                        	]
}";

        private static string sql1 = @"SELECT  TBL271.[Id] as  Table_95_Field_1,
                                   TBL271.[VoucherType] as  Table_95_Field_3,
                                   TBL271.[VoucherSequence] as  Table_95_Field_5,
                                   TBL271.[VoucherNumber] as  Table_95_Field_7,
                                   TBL271.[CancelledCustomerStockOut] as  Table_95_Field_9,
                                   TBL271.[ModifiedCustomerStockOut] as  Table_95_Field_11,
                                   TBL271.[Customer] as  Table_95_Field_13,
                                   TBL271.[CustomerAddress] as  Table_95_Field_15,
                                   TBL271.[CustomerContact] as  Table_95_Field_17,
                                   TBL271.[CompanyProfile] as  Table_95_Field_19,
                                   TBL271.[Currency] as  Table_95_Field_21,
                                   TBL271.[CurrencyRate] as  Table_95_Field_23,
                                   TBL271.[Division] as  Table_95_Field_25,
                                   TBL271.[JobNumber] as  Table_95_Field_27,
                                   TBL271.[Business] as  Table_95_Field_29,
                                   TBL271.[VoucherDate] as  Table_95_Field_31,
                                   TBL271.[FulfillmentDate] as  Table_95_Field_33,
                                   TBL271.[PaymentDate] as  Table_95_Field_35,
                                   TBL271.[PaymentMethod] as  Table_95_Field_37,
                                   TBL271.[Stock] as  Table_95_Field_39,
                                   TBL271.[NetValue] as  Table_95_Field_41,
                                   TBL271.[VatValue] as  Table_95_Field_43,
                                   TBL271.[GrossValue] as  Table_95_Field_45,
                                   TBL271.[PayableValue] as  Table_95_Field_47,
                                   TBL271.[AcquitValue] as  Table_95_Field_49,
                                   TBL271.[ExternalId] as  Table_95_Field_51,
                                   TBL271.[AvailableGrossValue] as  Table_95_Field_53,
                                   TBL271.[CustomerStockIn] as  Table_95_Field_55,
                                   NULL TopComment,
                                   NULL Comment,
                                   NULL ReportRepository,
                                   TBL271.[ReportPageCount] as  Table_95_Field_63,
                                   TBL271.[RowVersion] as  Table_95_Field_65,
                                   TBL271.[CustomerNameDisplay] as  Table_95_Field_67,
                                   TBL271.[CustomerCodeDisplay] as  Table_95_Field_69,
                                   TBL271.[CreationDateTime] as  Table_95_Field_71,
                                   TBL271.[Owner] as  Table_95_Field_73,
                                   TBL271.[LogoImage] as  Table_95_Field_75,
                                   TBL271.[ReportFullName] as  Table_95_Field_77,
                                   TBL271.[FinishedProduct] as  Table_95_Field_79,
                                   TBL271.[FinishedProductQuantity] as  Table_95_Field_81,
                                   TBL271.[Locked] as  Table_95_Field_83,
                                   TBL271.[WithoutVat] as  Table_95_Field_85,
                                   TBL271.[Language] as  Table_95_Field_87,
                                   TBL271.[AcquitedDate] as  Table_95_Field_89,
                                   TBL271.[ReportDesign] as  Table_95_Field_91,
                                   TBL271.[IntrastatTransactionCode] as  Table_95_Field_93,
                                   TBL271.[IntrastatDeliveryTermCode] as  Table_95_Field_95,
                                   TBL271.[IntrastatModeOfTransportCode] as  Table_95_Field_97,
                                   TBL271.[IntrastatCountryNonEU] as  Table_95_Field_99,
                                   TBL271.[IntrastatCountryEU] as  Table_95_Field_101,
                                   TBL271.[NetWeight] as  Table_95_Field_103,
                                   TBL271.[GrossWeight] as  Table_95_Field_105,
                                   TBL271.[CustomerStockOutStatus] as  Table_95_Field_107,
                                   TBL271.[SettlementPeriodDate] as  Table_95_Field_109,
                                   TBL271.[EVoucher] as  Table_95_Field_111,
                                   TBL271.[EDIVoucher] as  Table_95_Field_113,
                                   TBL271.[CashRegisterProfile] as  Table_95_Field_115,
                                   TBL271.[PurchasePrice] as  Table_95_Field_117,
                                   TBL271.[PriceGap] as  Table_95_Field_119,
                                   TBL271.[VoucherTemplate] as  Table_95_Field_121,
                                   TBL271.[PrintDateTime] as  Table_95_Field_123,
                                   TBL271.[RemoteValidToDate] as  Table_95_Field_125,
                                   TBL271.[RemotePrintDateTime] as  Table_95_Field_127,
                                   TBL271.[UnitPriceDigits] as  Table_95_Field_129,
                                   TBL271.[NetValueDigits] as  Table_95_Field_131,
                                   TBL271.[VatValueDigits] as  Table_95_Field_133,
                                   TBL271.[GrossValueDigits] as  Table_95_Field_135,
                                   TBL271.[GrandTotalDigits] as  Table_95_Field_137,
                                   TBL271.[CurrencyRateforVatSummary] as  Table_95_Field_139,
                                   TBL271.[DeliveryAddress] as  Table_95_Field_141,
                                   TBL271.[TransportMode] as  Table_95_Field_143,
                                   TBL271.[PaymentAccounting] as  Table_95_Field_145,
                                   NULL InheretedTopComment,
                                   NULL InheretedBottomComment,
                                   TBL271.[SalesOpportunity] as  Table_95_Field_151,
                                   TBL271.[DetailGroupByType] as  Table_95_Field_153,
                                   TBL271.[RetentionWarranty] as  Table_95_Field_155,
                                   TBL271.[RetentionPaymentDate] as  Table_95_Field_157,
                                   TBL271.[AppliedPriceRule] as  Table_95_Field_159,
                                   TBL271.[AccountingStockMovementType] as  Table_95_Field_161,
                                   TBL271.[WebShopId] as  Table_95_Field_163,
                                   NULL Description,
                                   TBL271.[ActualCurrencyRate] as  Table_95_Field_167,
                                   TBL271.[DiscountMode] as  Table_95_Field_169,
                                   TBL271.[FulfilledCertificate] as  Table_95_Field_171,
                                   TBL271.[CustomerStockInIntermediate] as  Table_95_Field_173,
                                   TBL271.[MadeWithCashRegister] as  Table_95_Field_175,
                                   NULL NAVRepository,
                                   TBL271.[SecQuantity] as  Table_95_Field_179,
                                   TBL271.[SecQuantityUnit] as  Table_95_Field_181,
                                   TBL271.[SecQuantityDigits] as  Table_95_Field_183,
                                   TBL271.[ConversionRate] as  Table_95_Field_185,
                                   TBL271.[QuantityUnit] as  Table_95_Field_187,
                                   TBL271.[QuantityDigits] as  Table_95_Field_189,
                                   TBL271.[PrivateCustomer] as  Table_95_Field_191
                                    FROM dbo.[CustomerStockOut] as  TBL271
                                    WHERE TBL271.[Vouchertype]=1";

        private static string cmdText = @"USE [ReportDB]
                                            GO
                                            SET ANSI_NULLS ON
                                            GO
                                            SET QUOTED_IDENTIFIER ON
                                            GO
                                            if exists(select 1 from sys.tables where name='a73bcfb4b6d714f32bedcf68a48a52fc5' and type='U')
                                            drop table a73bcfb4b6d714f32bedcf68a48a52fc5
                                            GO
                                            CREATE TABLE [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5](
                                                [Table_95_Field_1] [int] IDENTITY(1,1) NOT NULL,
                                                [Table_95_Field_3] [int] NOT NULL,
                                                [Table_95_Field_5] [int] NULL,
                                                [Table_95_Field_7] [nvarchar](80) NOT NULL,
                                                [Table_95_Field_9] [int] NULL,
                                                [Table_95_Field_11] [int] NULL,
                                                [Table_95_Field_13] [int] NULL,
                                                [Table_95_Field_15] [int] NULL,
                                                [Table_95_Field_17] [int] NULL,
                                                [Table_95_Field_19] [int] NULL,
                                                [Table_95_Field_21] [int] NULL,
                                                [Table_95_Field_23] [decimal](18, 6) NULL,
                                                [Table_95_Field_25] [int] NULL,
                                                [Table_95_Field_27] [int] NULL,
                                                [Table_95_Field_29] [int] NULL,
                                                [Table_95_Field_31] [datetime] NOT NULL,
                                                [Table_95_Field_33] [datetime] NOT NULL,
                                                [Table_95_Field_35] [datetime] NULL,
                                                [Table_95_Field_37] [int] NULL,
                                                [Table_95_Field_39] [int] NULL,
                                                [Table_95_Field_41] [decimal](18, 6) NOT NULL,
                                                [Table_95_Field_43] [decimal](18, 6) NOT NULL,
                                                [Table_95_Field_45] [decimal](18, 6) NOT NULL,
                                                [Table_95_Field_47] [decimal](18, 6) NOT NULL,
                                                [Table_95_Field_49] [decimal](18, 6) NOT NULL,
                                                [Table_95_Field_51] [nvarchar](80) NULL,
                                                [Table_95_Field_53] [decimal](18, 6) NULL,
                                                [Table_95_Field_55] [int] NULL,
                                                [TopComment] [int] NULL,
                                                [Comment] [int] NULL,
                                                [ReportRepository] [int] NULL,
                                                [Table_95_Field_63] [int] NULL,
                                                [Table_95_Field_65] [datetime] NOT NULL,
                                                [Table_95_Field_67] [nvarchar](140) NULL,
                                                [Table_95_Field_69] [nvarchar](80) NULL,
                                                [Table_95_Field_71] [datetime] NOT NULL,
                                                [Table_95_Field_73] [int] NULL,
                                                [Table_95_Field_75] [int] NULL,
                                                [Table_95_Field_77] [nvarchar](100) NULL,
                                                [Table_95_Field_79] [int] NULL,
                                                [Table_95_Field_81] [decimal](18, 6) NULL,
                                                [Table_95_Field_83] [bit] NOT NULL,
                                                [Table_95_Field_85] [bit] NOT NULL,
                                                [Table_95_Field_87] [int] NULL,
                                                [Table_95_Field_89] [datetime] NULL,
                                                [Table_95_Field_91] [int] NULL,
                                                [Table_95_Field_93] [int] NULL,
                                                [Table_95_Field_95] [int] NULL,
                                                [Table_95_Field_97] [int] NULL,
                                                [Table_95_Field_99] [int] NULL,
                                                [Table_95_Field_101] [int] NULL,
                                                [Table_95_Field_103] [decimal](18, 6) NULL,
                                                [Table_95_Field_105] [decimal](18, 6) NULL,
                                                [Table_95_Field_107] [int] NULL,
                                                [Table_95_Field_109] [datetime] NULL,
                                                [Table_95_Field_111] [bit] NOT NULL,
                                                [Table_95_Field_113] [bit] NOT NULL,
                                                [Table_95_Field_115] [int] NULL,
                                                [Table_95_Field_117] [decimal](18, 6) NULL,
                                                [Table_95_Field_119] [decimal](18, 6) NULL,
                                                [Table_95_Field_121] [int] NULL,
                                                [Table_95_Field_123] [datetime] NULL,
                                                [Table_95_Field_125] [datetime] NULL,
                                                [Table_95_Field_127] [datetime] NULL,
                                                [Table_95_Field_129] [int] NULL,
                                                [Table_95_Field_131] [int] NULL,
                                                [Table_95_Field_133] [int] NULL,
                                                [Table_95_Field_135] [int] NULL,
                                                [Table_95_Field_137] [int] NULL,
                                                [Table_95_Field_139] [decimal](18, 6) NULL,
                                                [Table_95_Field_141] [int] NULL,
                                                [Table_95_Field_143] [int] NULL,
                                                [Table_95_Field_145] [bit] NOT NULL,
                                                [InheretedTopComment] [int] NULL,
                                                [InheretedBottomComment] [int] NULL,
                                                [Table_95_Field_151] [int] NULL,
                                                [Table_95_Field_153] [int] NOT NULL,
                                                [Table_95_Field_155] [decimal](18, 6) NULL,
                                                [Table_95_Field_157] [datetime] NULL,
                                                [Table_95_Field_159] [int] NULL,
                                                [Table_95_Field_161] [int] NULL,
                                                [Table_95_Field_163] [nvarchar](50) NULL,
                                                [Description] [int] NULL,
                                                [Table_95_Field_167] [decimal](18, 6) NULL,
                                                [Table_95_Field_169] [int] NOT NULL,
                                                [Table_95_Field_171] [bit] NOT NULL,
                                                [Table_95_Field_173] [int] NULL,
                                                [Table_95_Field_175] [bit] NOT NULL,
                                                [NAVRepository] [int] NULL,
                                                [Table_95_Field_179] [decimal](18, 6) NULL,
                                                [Table_95_Field_181] [int] NULL,
                                                [Table_95_Field_183] [int] NOT NULL,
                                                [Table_95_Field_185] [decimal](18, 6) NULL,
                                                [Table_95_Field_187] [int] NULL,
                                                [Table_95_Field_189] [int] NOT NULL,
                                                [Table_95_Field_191] [bit] NOT NULL
                                            ) ON [PRIMARY]
                                            GO
                                            SET IDENTITY_INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ON
                                            GO
                                            INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ([Table_95_Field_1], [Table_95_Field_3], [Table_95_Field_5], [Table_95_Field_7], [Table_95_Field_9], [Table_95_Field_11], [Table_95_Field_13], [Table_95_Field_15], [Table_95_Field_17], [Table_95_Field_19], [Table_95_Field_21], [Table_95_Field_23], [Table_95_Field_25], [Table_95_Field_27], [Table_95_Field_29], [Table_95_Field_31], [Table_95_Field_33], [Table_95_Field_35], [Table_95_Field_37], [Table_95_Field_39], [Table_95_Field_41], [Table_95_Field_43], [Table_95_Field_45], [Table_95_Field_47], [Table_95_Field_49], [Table_95_Field_51], [Table_95_Field_53], [Table_95_Field_55], [TopComment], [Comment], [ReportRepository], [Table_95_Field_63], [Table_95_Field_65], [Table_95_Field_67], [Table_95_Field_69], [Table_95_Field_71], [Table_95_Field_73], [Table_95_Field_75], [Table_95_Field_77], [Table_95_Field_79], [Table_95_Field_81], [Table_95_Field_83], [Table_95_Field_85], [Table_95_Field_87], [Table_95_Field_89], [Table_95_Field_91], [Table_95_Field_93], [Table_95_Field_95], [Table_95_Field_97], [Table_95_Field_99], [Table_95_Field_101], [Table_95_Field_103], [Table_95_Field_105], [Table_95_Field_107], [Table_95_Field_109], [Table_95_Field_111], [Table_95_Field_113], [Table_95_Field_115], [Table_95_Field_117], [Table_95_Field_119], [Table_95_Field_121], [Table_95_Field_123], [Table_95_Field_125], [Table_95_Field_127], [Table_95_Field_129], [Table_95_Field_131], [Table_95_Field_133], [Table_95_Field_135], [Table_95_Field_137], [Table_95_Field_139], [Table_95_Field_141], [Table_95_Field_143], [Table_95_Field_145], [InheretedTopComment], [InheretedBottomComment], [Table_95_Field_151], [Table_95_Field_153], [Table_95_Field_155], [Table_95_Field_157], [Table_95_Field_159], [Table_95_Field_161], [Table_95_Field_163], [Description], [Table_95_Field_167], [Table_95_Field_169], [Table_95_Field_171], [Table_95_Field_173], [Table_95_Field_175], [NAVRepository], [Table_95_Field_179], [Table_95_Field_181], [Table_95_Field_183], [Table_95_Field_185], [Table_95_Field_187], [Table_95_Field_189], [Table_95_Field_191]) VALUES (1, 1, 9, N'SZA00001/2017', NULL, NULL, 1, 1, NULL, 1, 1, CAST(1.000000 AS Decimal(18, 6)), NULL, NULL, NULL, CAST(0x0000A80D00000000 AS DateTime), CAST(0x0000A80D00000000 AS DateTime), CAST(0x0000A80D00000000 AS DateTime), 1, 1, CAST(7000.000000 AS Decimal(18, 6)), CAST(1890.000000 AS Decimal(18, 6)), CAST(8890.000000 AS Decimal(18, 6)), CAST(8890.000000 AS Decimal(18, 6)), CAST(8890.000000 AS Decimal(18, 6)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000A80D00BAF340 AS DateTime), N'Teszt Elek', NULL, CAST(0x0000A80D00BAF22C AS DateTime), 1, NULL, NULL, NULL, NULL, 0, 0, NULL, CAST(0x0000A80D00000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), 2, NULL, 0, 0, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(7000.000000 AS Decimal(18, 6)), 3, NULL, NULL, NULL, 2, 0, 0, 0, 0, CAST(1.000000 AS Decimal(18, 6)), 2, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1.000000 AS Decimal(18, 6)), 0, 0, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
                                            GO
                                            INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ([Table_95_Field_1], [Table_95_Field_3], [Table_95_Field_5], [Table_95_Field_7], [Table_95_Field_9], [Table_95_Field_11], [Table_95_Field_13], [Table_95_Field_15], [Table_95_Field_17], [Table_95_Field_19], [Table_95_Field_21], [Table_95_Field_23], [Table_95_Field_25], [Table_95_Field_27], [Table_95_Field_29], [Table_95_Field_31], [Table_95_Field_33], [Table_95_Field_35], [Table_95_Field_37], [Table_95_Field_39], [Table_95_Field_41], [Table_95_Field_43], [Table_95_Field_45], [Table_95_Field_47], [Table_95_Field_49], [Table_95_Field_51], [Table_95_Field_53], [Table_95_Field_55], [TopComment], [Comment], [ReportRepository], [Table_95_Field_63], [Table_95_Field_65], [Table_95_Field_67], [Table_95_Field_69], [Table_95_Field_71], [Table_95_Field_73], [Table_95_Field_75], [Table_95_Field_77], [Table_95_Field_79], [Table_95_Field_81], [Table_95_Field_83], [Table_95_Field_85], [Table_95_Field_87], [Table_95_Field_89], [Table_95_Field_91], [Table_95_Field_93], [Table_95_Field_95], [Table_95_Field_97], [Table_95_Field_99], [Table_95_Field_101], [Table_95_Field_103], [Table_95_Field_105], [Table_95_Field_107], [Table_95_Field_109], [Table_95_Field_111], [Table_95_Field_113], [Table_95_Field_115], [Table_95_Field_117], [Table_95_Field_119], [Table_95_Field_121], [Table_95_Field_123], [Table_95_Field_125], [Table_95_Field_127], [Table_95_Field_129], [Table_95_Field_131], [Table_95_Field_133], [Table_95_Field_135], [Table_95_Field_137], [Table_95_Field_139], [Table_95_Field_141], [Table_95_Field_143], [Table_95_Field_145], [InheretedTopComment], [InheretedBottomComment], [Table_95_Field_151], [Table_95_Field_153], [Table_95_Field_155], [Table_95_Field_157], [Table_95_Field_159], [Table_95_Field_161], [Table_95_Field_163], [Description], [Table_95_Field_167], [Table_95_Field_169], [Table_95_Field_171], [Table_95_Field_173], [Table_95_Field_175], [NAVRepository], [Table_95_Field_179], [Table_95_Field_181], [Table_95_Field_183], [Table_95_Field_185], [Table_95_Field_187], [Table_95_Field_189], [Table_95_Field_191]) VALUES (2, 1, 9, N'SZA00002/2017', NULL, NULL, 1, 1, NULL, 1, 1, CAST(1.000000 AS Decimal(18, 6)), NULL, NULL, NULL, CAST(0x0000A80D00000000 AS DateTime), CAST(0x0000A80D00000000 AS DateTime), CAST(0x0000A80D00000000 AS DateTime), 1, 1, CAST(4500.000000 AS Decimal(18, 6)), CAST(1215.000000 AS Decimal(18, 6)), CAST(5715.000000 AS Decimal(18, 6)), CAST(5715.000000 AS Decimal(18, 6)), CAST(5715.000000 AS Decimal(18, 6)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000A80D00BB0C64 AS DateTime), N'Teszt Elek', NULL, CAST(0x0000A80D00BB0C29 AS DateTime), 1, NULL, NULL, NULL, NULL, 0, 0, NULL, CAST(0x0000A80D00000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), 2, NULL, 0, 0, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(4500.000000 AS Decimal(18, 6)), 3, NULL, NULL, NULL, 2, 0, 0, 0, 0, CAST(1.000000 AS Decimal(18, 6)), 2, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1.000000 AS Decimal(18, 6)), 0, 0, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
                                            GO
                                            INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ([Table_95_Field_1], [Table_95_Field_3], [Table_95_Field_5], [Table_95_Field_7], [Table_95_Field_9], [Table_95_Field_11], [Table_95_Field_13], [Table_95_Field_15], [Table_95_Field_17], [Table_95_Field_19], [Table_95_Field_21], [Table_95_Field_23], [Table_95_Field_25], [Table_95_Field_27], [Table_95_Field_29], [Table_95_Field_31], [Table_95_Field_33], [Table_95_Field_35], [Table_95_Field_37], [Table_95_Field_39], [Table_95_Field_41], [Table_95_Field_43], [Table_95_Field_45], [Table_95_Field_47], [Table_95_Field_49], [Table_95_Field_51], [Table_95_Field_53], [Table_95_Field_55], [TopComment], [Comment], [ReportRepository], [Table_95_Field_63], [Table_95_Field_65], [Table_95_Field_67], [Table_95_Field_69], [Table_95_Field_71], [Table_95_Field_73], [Table_95_Field_75], [Table_95_Field_77], [Table_95_Field_79], [Table_95_Field_81], [Table_95_Field_83], [Table_95_Field_85], [Table_95_Field_87], [Table_95_Field_89], [Table_95_Field_91], [Table_95_Field_93], [Table_95_Field_95], [Table_95_Field_97], [Table_95_Field_99], [Table_95_Field_101], [Table_95_Field_103], [Table_95_Field_105], [Table_95_Field_107], [Table_95_Field_109], [Table_95_Field_111], [Table_95_Field_113], [Table_95_Field_115], [Table_95_Field_117], [Table_95_Field_119], [Table_95_Field_121], [Table_95_Field_123], [Table_95_Field_125], [Table_95_Field_127], [Table_95_Field_129], [Table_95_Field_131], [Table_95_Field_133], [Table_95_Field_135], [Table_95_Field_137], [Table_95_Field_139], [Table_95_Field_141], [Table_95_Field_143], [Table_95_Field_145], [InheretedTopComment], [InheretedBottomComment], [Table_95_Field_151], [Table_95_Field_153], [Table_95_Field_155], [Table_95_Field_157], [Table_95_Field_159], [Table_95_Field_161], [Table_95_Field_163], [Description], [Table_95_Field_167], [Table_95_Field_169], [Table_95_Field_171], [Table_95_Field_173], [Table_95_Field_175], [NAVRepository], [Table_95_Field_179], [Table_95_Field_181], [Table_95_Field_183], [Table_95_Field_185], [Table_95_Field_187], [Table_95_Field_189], [Table_95_Field_191]) VALUES (4, 1, 9, N'SZA00003/2017', NULL, NULL, 2, 3, NULL, 1, 1, CAST(1.000000 AS Decimal(18, 6)), NULL, NULL, NULL, CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), 1, 1, CAST(160.000000 AS Decimal(18, 6)), CAST(43.000000 AS Decimal(18, 6)), CAST(203.000000 AS Decimal(18, 6)), CAST(205.000000 AS Decimal(18, 6)), CAST(203.000000 AS Decimal(18, 6)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000A82400EAE279 AS DateTime), N'Kis Sándor', N'11222', CAST(0x0000A82400EAE0D2 AS DateTime), 1, NULL, NULL, NULL, NULL, 0, 0, NULL, CAST(0x0000A82400000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), 2, NULL, 0, 0, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(160.000000 AS Decimal(18, 6)), 3, NULL, NULL, NULL, 2, 0, 0, 0, 0, CAST(1.000000 AS Decimal(18, 6)), 3, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1.000000 AS Decimal(18, 6)), 0, 0, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
                                            GO
                                            INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ([Table_95_Field_1], [Table_95_Field_3], [Table_95_Field_5], [Table_95_Field_7], [Table_95_Field_9], [Table_95_Field_11], [Table_95_Field_13], [Table_95_Field_15], [Table_95_Field_17], [Table_95_Field_19], [Table_95_Field_21], [Table_95_Field_23], [Table_95_Field_25], [Table_95_Field_27], [Table_95_Field_29], [Table_95_Field_31], [Table_95_Field_33], [Table_95_Field_35], [Table_95_Field_37], [Table_95_Field_39], [Table_95_Field_41], [Table_95_Field_43], [Table_95_Field_45], [Table_95_Field_47], [Table_95_Field_49], [Table_95_Field_51], [Table_95_Field_53], [Table_95_Field_55], [TopComment], [Comment], [ReportRepository], [Table_95_Field_63], [Table_95_Field_65], [Table_95_Field_67], [Table_95_Field_69], [Table_95_Field_71], [Table_95_Field_73], [Table_95_Field_75], [Table_95_Field_77], [Table_95_Field_79], [Table_95_Field_81], [Table_95_Field_83], [Table_95_Field_85], [Table_95_Field_87], [Table_95_Field_89], [Table_95_Field_91], [Table_95_Field_93], [Table_95_Field_95], [Table_95_Field_97], [Table_95_Field_99], [Table_95_Field_101], [Table_95_Field_103], [Table_95_Field_105], [Table_95_Field_107], [Table_95_Field_109], [Table_95_Field_111], [Table_95_Field_113], [Table_95_Field_115], [Table_95_Field_117], [Table_95_Field_119], [Table_95_Field_121], [Table_95_Field_123], [Table_95_Field_125], [Table_95_Field_127], [Table_95_Field_129], [Table_95_Field_131], [Table_95_Field_133], [Table_95_Field_135], [Table_95_Field_137], [Table_95_Field_139], [Table_95_Field_141], [Table_95_Field_143], [Table_95_Field_145], [InheretedTopComment], [InheretedBottomComment], [Table_95_Field_151], [Table_95_Field_153], [Table_95_Field_155], [Table_95_Field_157], [Table_95_Field_159], [Table_95_Field_161], [Table_95_Field_163], [Description], [Table_95_Field_167], [Table_95_Field_169], [Table_95_Field_171], [Table_95_Field_173], [Table_95_Field_175], [NAVRepository], [Table_95_Field_179], [Table_95_Field_181], [Table_95_Field_183], [Table_95_Field_185], [Table_95_Field_187], [Table_95_Field_189], [Table_95_Field_191]) VALUES (5, 1, 9, N'SZA00004/2017', NULL, NULL, 2, 3, NULL, 1, 1, CAST(1.000000 AS Decimal(18, 6)), NULL, NULL, NULL, CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), 1, 1, CAST(160.000000 AS Decimal(18, 6)), CAST(43.000000 AS Decimal(18, 6)), CAST(203.000000 AS Decimal(18, 6)), CAST(205.000000 AS Decimal(18, 6)), CAST(203.000000 AS Decimal(18, 6)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000A82400EAF7B3 AS DateTime), N'Kis Sándor', N'11222', CAST(0x0000A82400EAF761 AS DateTime), 1, NULL, NULL, NULL, NULL, 0, 0, NULL, CAST(0x0000A82400000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), 2, NULL, 0, 0, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(160.000000 AS Decimal(18, 6)), 3, NULL, NULL, NULL, 2, 0, 0, 0, 0, CAST(1.000000 AS Decimal(18, 6)), 3, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1.000000 AS Decimal(18, 6)), 0, 0, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
                                            GO
                                            INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ([Table_95_Field_1], [Table_95_Field_3], [Table_95_Field_5], [Table_95_Field_7], [Table_95_Field_9], [Table_95_Field_11], [Table_95_Field_13], [Table_95_Field_15], [Table_95_Field_17], [Table_95_Field_19], [Table_95_Field_21], [Table_95_Field_23], [Table_95_Field_25], [Table_95_Field_27], [Table_95_Field_29], [Table_95_Field_31], [Table_95_Field_33], [Table_95_Field_35], [Table_95_Field_37], [Table_95_Field_39], [Table_95_Field_41], [Table_95_Field_43], [Table_95_Field_45], [Table_95_Field_47], [Table_95_Field_49], [Table_95_Field_51], [Table_95_Field_53], [Table_95_Field_55], [TopComment], [Comment], [ReportRepository], [Table_95_Field_63], [Table_95_Field_65], [Table_95_Field_67], [Table_95_Field_69], [Table_95_Field_71], [Table_95_Field_73], [Table_95_Field_75], [Table_95_Field_77], [Table_95_Field_79], [Table_95_Field_81], [Table_95_Field_83], [Table_95_Field_85], [Table_95_Field_87], [Table_95_Field_89], [Table_95_Field_91], [Table_95_Field_93], [Table_95_Field_95], [Table_95_Field_97], [Table_95_Field_99], [Table_95_Field_101], [Table_95_Field_103], [Table_95_Field_105], [Table_95_Field_107], [Table_95_Field_109], [Table_95_Field_111], [Table_95_Field_113], [Table_95_Field_115], [Table_95_Field_117], [Table_95_Field_119], [Table_95_Field_121], [Table_95_Field_123], [Table_95_Field_125], [Table_95_Field_127], [Table_95_Field_129], [Table_95_Field_131], [Table_95_Field_133], [Table_95_Field_135], [Table_95_Field_137], [Table_95_Field_139], [Table_95_Field_141], [Table_95_Field_143], [Table_95_Field_145], [InheretedTopComment], [InheretedBottomComment], [Table_95_Field_151], [Table_95_Field_153], [Table_95_Field_155], [Table_95_Field_157], [Table_95_Field_159], [Table_95_Field_161], [Table_95_Field_163], [Description], [Table_95_Field_167], [Table_95_Field_169], [Table_95_Field_171], [Table_95_Field_173], [Table_95_Field_175], [NAVRepository], [Table_95_Field_179], [Table_95_Field_181], [Table_95_Field_183], [Table_95_Field_185], [Table_95_Field_187], [Table_95_Field_189], [Table_95_Field_191]) VALUES (6, 1, 9, N'SZA00005/2017', NULL, NULL, 2, 3, NULL, 1, 1, CAST(1.000000 AS Decimal(18, 6)), NULL, NULL, NULL, CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), 1, 1, CAST(400.000000 AS Decimal(18, 6)), CAST(108.000000 AS Decimal(18, 6)), CAST(508.000000 AS Decimal(18, 6)), CAST(510.000000 AS Decimal(18, 6)), CAST(508.000000 AS Decimal(18, 6)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000A82400EB0450 AS DateTime), N'Kis Sándor', N'11222', CAST(0x0000A82400EB03F9 AS DateTime), 1, NULL, NULL, NULL, NULL, 0, 0, NULL, CAST(0x0000A82400000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), 2, NULL, 0, 0, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(400.000000 AS Decimal(18, 6)), 3, NULL, NULL, NULL, 2, 0, 0, 0, 0, CAST(1.000000 AS Decimal(18, 6)), 3, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1.000000 AS Decimal(18, 6)), 0, 0, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
                                            GO
                                            INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ([Table_95_Field_1], [Table_95_Field_3], [Table_95_Field_5], [Table_95_Field_7], [Table_95_Field_9], [Table_95_Field_11], [Table_95_Field_13], [Table_95_Field_15], [Table_95_Field_17], [Table_95_Field_19], [Table_95_Field_21], [Table_95_Field_23], [Table_95_Field_25], [Table_95_Field_27], [Table_95_Field_29], [Table_95_Field_31], [Table_95_Field_33], [Table_95_Field_35], [Table_95_Field_37], [Table_95_Field_39], [Table_95_Field_41], [Table_95_Field_43], [Table_95_Field_45], [Table_95_Field_47], [Table_95_Field_49], [Table_95_Field_51], [Table_95_Field_53], [Table_95_Field_55], [TopComment], [Comment], [ReportRepository], [Table_95_Field_63], [Table_95_Field_65], [Table_95_Field_67], [Table_95_Field_69], [Table_95_Field_71], [Table_95_Field_73], [Table_95_Field_75], [Table_95_Field_77], [Table_95_Field_79], [Table_95_Field_81], [Table_95_Field_83], [Table_95_Field_85], [Table_95_Field_87], [Table_95_Field_89], [Table_95_Field_91], [Table_95_Field_93], [Table_95_Field_95], [Table_95_Field_97], [Table_95_Field_99], [Table_95_Field_101], [Table_95_Field_103], [Table_95_Field_105], [Table_95_Field_107], [Table_95_Field_109], [Table_95_Field_111], [Table_95_Field_113], [Table_95_Field_115], [Table_95_Field_117], [Table_95_Field_119], [Table_95_Field_121], [Table_95_Field_123], [Table_95_Field_125], [Table_95_Field_127], [Table_95_Field_129], [Table_95_Field_131], [Table_95_Field_133], [Table_95_Field_135], [Table_95_Field_137], [Table_95_Field_139], [Table_95_Field_141], [Table_95_Field_143], [Table_95_Field_145], [InheretedTopComment], [InheretedBottomComment], [Table_95_Field_151], [Table_95_Field_153], [Table_95_Field_155], [Table_95_Field_157], [Table_95_Field_159], [Table_95_Field_161], [Table_95_Field_163], [Description], [Table_95_Field_167], [Table_95_Field_169], [Table_95_Field_171], [Table_95_Field_173], [Table_95_Field_175], [NAVRepository], [Table_95_Field_179], [Table_95_Field_181], [Table_95_Field_183], [Table_95_Field_185], [Table_95_Field_187], [Table_95_Field_189], [Table_95_Field_191]) VALUES (7, 1, 9, N'SZA00006/2017', NULL, NULL, 2, 3, NULL, 1, 1, CAST(1.000000 AS Decimal(18, 6)), NULL, NULL, NULL, CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), 1, 1, CAST(400.000000 AS Decimal(18, 6)), CAST(108.000000 AS Decimal(18, 6)), CAST(508.000000 AS Decimal(18, 6)), CAST(510.000000 AS Decimal(18, 6)), CAST(508.000000 AS Decimal(18, 6)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000A82400EB0B9C AS DateTime), N'Kis Sándor', N'11222', CAST(0x0000A82400EB0B5A AS DateTime), 1, NULL, NULL, NULL, NULL, 0, 0, NULL, CAST(0x0000A82400000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), 2, NULL, 0, 0, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(400.000000 AS Decimal(18, 6)), 3, NULL, NULL, NULL, 2, 0, 0, 0, 0, CAST(1.000000 AS Decimal(18, 6)), 3, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1.000000 AS Decimal(18, 6)), 0, 0, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
                                            GO
                                            INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ([Table_95_Field_1], [Table_95_Field_3], [Table_95_Field_5], [Table_95_Field_7], [Table_95_Field_9], [Table_95_Field_11], [Table_95_Field_13], [Table_95_Field_15], [Table_95_Field_17], [Table_95_Field_19], [Table_95_Field_21], [Table_95_Field_23], [Table_95_Field_25], [Table_95_Field_27], [Table_95_Field_29], [Table_95_Field_31], [Table_95_Field_33], [Table_95_Field_35], [Table_95_Field_37], [Table_95_Field_39], [Table_95_Field_41], [Table_95_Field_43], [Table_95_Field_45], [Table_95_Field_47], [Table_95_Field_49], [Table_95_Field_51], [Table_95_Field_53], [Table_95_Field_55], [TopComment], [Comment], [ReportRepository], [Table_95_Field_63], [Table_95_Field_65], [Table_95_Field_67], [Table_95_Field_69], [Table_95_Field_71], [Table_95_Field_73], [Table_95_Field_75], [Table_95_Field_77], [Table_95_Field_79], [Table_95_Field_81], [Table_95_Field_83], [Table_95_Field_85], [Table_95_Field_87], [Table_95_Field_89], [Table_95_Field_91], [Table_95_Field_93], [Table_95_Field_95], [Table_95_Field_97], [Table_95_Field_99], [Table_95_Field_101], [Table_95_Field_103], [Table_95_Field_105], [Table_95_Field_107], [Table_95_Field_109], [Table_95_Field_111], [Table_95_Field_113], [Table_95_Field_115], [Table_95_Field_117], [Table_95_Field_119], [Table_95_Field_121], [Table_95_Field_123], [Table_95_Field_125], [Table_95_Field_127], [Table_95_Field_129], [Table_95_Field_131], [Table_95_Field_133], [Table_95_Field_135], [Table_95_Field_137], [Table_95_Field_139], [Table_95_Field_141], [Table_95_Field_143], [Table_95_Field_145], [InheretedTopComment], [InheretedBottomComment], [Table_95_Field_151], [Table_95_Field_153], [Table_95_Field_155], [Table_95_Field_157], [Table_95_Field_159], [Table_95_Field_161], [Table_95_Field_163], [Description], [Table_95_Field_167], [Table_95_Field_169], [Table_95_Field_171], [Table_95_Field_173], [Table_95_Field_175], [NAVRepository], [Table_95_Field_179], [Table_95_Field_181], [Table_95_Field_183], [Table_95_Field_185], [Table_95_Field_187], [Table_95_Field_189], [Table_95_Field_191]) VALUES (8, 1, 9, N'SZA00007/2017', NULL, NULL, 1, 1, NULL, 1, 1, CAST(1.000000 AS Decimal(18, 6)), NULL, NULL, NULL, CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), 1, 1, CAST(7000.000000 AS Decimal(18, 6)), CAST(1890.000000 AS Decimal(18, 6)), CAST(8890.000000 AS Decimal(18, 6)), CAST(8890.000000 AS Decimal(18, 6)), CAST(8890.000000 AS Decimal(18, 6)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000A82400EB1303 AS DateTime), N'Teszt Elek', NULL, CAST(0x0000A82400EB12B4 AS DateTime), 1, NULL, NULL, NULL, NULL, 0, 0, NULL, CAST(0x0000A82400000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), 2, NULL, 0, 0, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(7000.000000 AS Decimal(18, 6)), 3, NULL, NULL, NULL, 2, 0, 0, 0, 0, CAST(1.000000 AS Decimal(18, 6)), 2, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1.000000 AS Decimal(18, 6)), 0, 0, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
                                            GO
                                            INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ([Table_95_Field_1], [Table_95_Field_3], [Table_95_Field_5], [Table_95_Field_7], [Table_95_Field_9], [Table_95_Field_11], [Table_95_Field_13], [Table_95_Field_15], [Table_95_Field_17], [Table_95_Field_19], [Table_95_Field_21], [Table_95_Field_23], [Table_95_Field_25], [Table_95_Field_27], [Table_95_Field_29], [Table_95_Field_31], [Table_95_Field_33], [Table_95_Field_35], [Table_95_Field_37], [Table_95_Field_39], [Table_95_Field_41], [Table_95_Field_43], [Table_95_Field_45], [Table_95_Field_47], [Table_95_Field_49], [Table_95_Field_51], [Table_95_Field_53], [Table_95_Field_55], [TopComment], [Comment], [ReportRepository], [Table_95_Field_63], [Table_95_Field_65], [Table_95_Field_67], [Table_95_Field_69], [Table_95_Field_71], [Table_95_Field_73], [Table_95_Field_75], [Table_95_Field_77], [Table_95_Field_79], [Table_95_Field_81], [Table_95_Field_83], [Table_95_Field_85], [Table_95_Field_87], [Table_95_Field_89], [Table_95_Field_91], [Table_95_Field_93], [Table_95_Field_95], [Table_95_Field_97], [Table_95_Field_99], [Table_95_Field_101], [Table_95_Field_103], [Table_95_Field_105], [Table_95_Field_107], [Table_95_Field_109], [Table_95_Field_111], [Table_95_Field_113], [Table_95_Field_115], [Table_95_Field_117], [Table_95_Field_119], [Table_95_Field_121], [Table_95_Field_123], [Table_95_Field_125], [Table_95_Field_127], [Table_95_Field_129], [Table_95_Field_131], [Table_95_Field_133], [Table_95_Field_135], [Table_95_Field_137], [Table_95_Field_139], [Table_95_Field_141], [Table_95_Field_143], [Table_95_Field_145], [InheretedTopComment], [InheretedBottomComment], [Table_95_Field_151], [Table_95_Field_153], [Table_95_Field_155], [Table_95_Field_157], [Table_95_Field_159], [Table_95_Field_161], [Table_95_Field_163], [Description], [Table_95_Field_167], [Table_95_Field_169], [Table_95_Field_171], [Table_95_Field_173], [Table_95_Field_175], [NAVRepository], [Table_95_Field_179], [Table_95_Field_181], [Table_95_Field_183], [Table_95_Field_185], [Table_95_Field_187], [Table_95_Field_189], [Table_95_Field_191]) VALUES (9, 1, 9, N'SZA00008/2017', NULL, NULL, 1, 1, NULL, 1, 1, CAST(1.000000 AS Decimal(18, 6)), NULL, NULL, NULL, CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), 1, 1, CAST(7200.000000 AS Decimal(18, 6)), CAST(1944.000000 AS Decimal(18, 6)), CAST(9144.000000 AS Decimal(18, 6)), CAST(9145.000000 AS Decimal(18, 6)), CAST(9144.000000 AS Decimal(18, 6)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000A82400EB200E AS DateTime), N'Teszt Elek', NULL, CAST(0x0000A82400EB1FEA AS DateTime), 1, NULL, NULL, NULL, NULL, 0, 0, NULL, CAST(0x0000A82400000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), 2, NULL, 0, 0, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(7200.000000 AS Decimal(18, 6)), 3, NULL, NULL, NULL, 2, 0, 0, 0, 0, CAST(1.000000 AS Decimal(18, 6)), 2, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1.000000 AS Decimal(18, 6)), 0, 0, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
                                            GO
                                            INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] ([Table_95_Field_1], [Table_95_Field_3], [Table_95_Field_5], [Table_95_Field_7], [Table_95_Field_9], [Table_95_Field_11], [Table_95_Field_13], [Table_95_Field_15], [Table_95_Field_17], [Table_95_Field_19], [Table_95_Field_21], [Table_95_Field_23], [Table_95_Field_25], [Table_95_Field_27], [Table_95_Field_29], [Table_95_Field_31], [Table_95_Field_33], [Table_95_Field_35], [Table_95_Field_37], [Table_95_Field_39], [Table_95_Field_41], [Table_95_Field_43], [Table_95_Field_45], [Table_95_Field_47], [Table_95_Field_49], [Table_95_Field_51], [Table_95_Field_53], [Table_95_Field_55], [TopComment], [Comment], [ReportRepository], [Table_95_Field_63], [Table_95_Field_65], [Table_95_Field_67], [Table_95_Field_69], [Table_95_Field_71], [Table_95_Field_73], [Table_95_Field_75], [Table_95_Field_77], [Table_95_Field_79], [Table_95_Field_81], [Table_95_Field_83], [Table_95_Field_85], [Table_95_Field_87], [Table_95_Field_89], [Table_95_Field_91], [Table_95_Field_93], [Table_95_Field_95], [Table_95_Field_97], [Table_95_Field_99], [Table_95_Field_101], [Table_95_Field_103], [Table_95_Field_105], [Table_95_Field_107], [Table_95_Field_109], [Table_95_Field_111], [Table_95_Field_113], [Table_95_Field_115], [Table_95_Field_117], [Table_95_Field_119], [Table_95_Field_121], [Table_95_Field_123], [Table_95_Field_125], [Table_95_Field_127], [Table_95_Field_129], [Table_95_Field_131], [Table_95_Field_133], [Table_95_Field_135], [Table_95_Field_137], [Table_95_Field_139], [Table_95_Field_141], [Table_95_Field_143], [Table_95_Field_145], [InheretedTopComment], [InheretedBottomComment], [Table_95_Field_151], [Table_95_Field_153], [Table_95_Field_155], [Table_95_Field_157], [Table_95_Field_159], [Table_95_Field_161], [Table_95_Field_163], [Description], [Table_95_Field_167], [Table_95_Field_169], [Table_95_Field_171], [Table_95_Field_173], [Table_95_Field_175], [NAVRepository], [Table_95_Field_179], [Table_95_Field_181], [Table_95_Field_183], [Table_95_Field_185], [Table_95_Field_187], [Table_95_Field_189], [Table_95_Field_191]) VALUES (10, 1, 9, N'SZA00009/2017', NULL, NULL, 2, 3, NULL, 1, 1, CAST(1.000000 AS Decimal(18, 6)), NULL, NULL, NULL, CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82400000000 AS DateTime), CAST(0x0000A82C00000000 AS DateTime), 2, 1, CAST(680.000000 AS Decimal(18, 6)), CAST(183.000000 AS Decimal(18, 6)), CAST(863.000000 AS Decimal(18, 6)), CAST(863.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0x0000A82400EB3EB5 AS DateTime), N'Kis Sándor', N'11222', CAST(0x0000A82400EB3ECB AS DateTime), 1, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(0.000000 AS Decimal(18, 6)), 2, NULL, 0, 0, NULL, CAST(0.000000 AS Decimal(18, 6)), CAST(680.000000 AS Decimal(18, 6)), 3, NULL, NULL, NULL, 2, 0, 0, 0, 0, CAST(1.000000 AS Decimal(18, 6)), 3, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1.000000 AS Decimal(18, 6)), 0, 0, NULL, 0, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
                                            GO
                                            SET IDENTITY_INSERT [dbo].[a73bcfb4b6d714f32bedcf68a48a52fc5] OFF
                                            GO";

        private static void CleanAllTables(AppDbContext context)
        {
            context.ReportDashboardRel.RemoveRange(context.ReportDashboardRel.ToList());
            context.ReportUserRel.RemoveRange(context.ReportUserRel.ToList());
            context.UserDashboardRel.RemoveRange(context.UserDashboardRel.ToList());
            context.Dashboards.RemoveRange(context.Dashboards.ToList());
            context.Report.RemoveRange(context.Report.ToList());
            context.UserJwtMap.RemoveRange(context.UserJwtMap.ToList());
            context.User.RemoveRange(context.User.ToList());
            context.Query.RemoveRange(context.Query.ToList());
            context.SaveChanges();
        }

        private static void ExecuteBatchNonQuery(string sql, SqlConnection conn)
        {
            string sqlBatch = string.Empty;
            SqlCommand cmd = new SqlCommand(string.Empty, conn);
            conn.Open();
            try
            {
                foreach (string line in sql.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.ToUpperInvariant().Trim() == "GO")
                    {
                        cmd.CommandText = sqlBatch;
                        cmd.ExecuteNonQuery();
                        sqlBatch = string.Empty;
                    }
                    else
                    {
                        sqlBatch += line + "\n";
                    }
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
}