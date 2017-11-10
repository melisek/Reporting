using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using szakdoga.Models;

namespace szakdoga.Data
{
    public class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            //TODO: megnézni h ezzel mért nem működik //AppDbContext context = applicationBuilder.ApplicationServices.GetRequiredService<AppDbContext>();
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                AppDbContext context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                CleanAllTables(context);

                if (!context.User.Any())
                    context.User.AddRange(Users);
                if (!context.Query.Any())
                    context.Query.AddRange(Queries);

                context.SaveChanges();
                if (!context.Report.Any())
                    context.Report.AddRange(Reports);
                if (!context.Dashboards.Any())
                    context.Dashboards.AddRange(Dashboards);
                if (!context.ReportDashboardRel.Any())
                    context.ReportDashboardRel.AddRange(ReportDashboardRels);
                if (!context.UserDashboardRel.Any())
                    context.UserDashboardRel.AddRange(UserDashboardRels);
                if (!context.ReportUserRel.Any())
                    context.ReportUserRel.AddRange(ReportUserRels);

                context.SaveChanges();

                //TODO: nem szép incializálás- ÍRD ÁT A SAJÁTODRA !!!
                string sourceConn = " Data Source = localhost\\SQL2012ST; Initial Catalog = kszstart_demo;MultipleActiveResultSets=True; Persist Security Info = True; User ID = Admin; Password = admin";
                using (SqlConnection conn = new SqlConnection(sourceConn))
                {
                    ExecuteBatchNonQuery(cmdText, conn);
                }
            }
        }

        private static List<User> users;

        private static List<User> Users
        {
            get
            {
                if (users == null)
                    users = new List<User>{
                        new User{ Name="Admin", Password="admin", EmailAddress="asd@asd.com", GUID="674a8382-dfb0-41d9-a349-a85599cc0de6"},
                        new User{ Name="Teszt", Password="teszt",EmailAddress="teszt@teszt.com",GUID="b12b3382-a250-48ca-9523-cf99b1826600"}
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
                               new Dashboard{ Name = "Dashboard1", GUID = "9b4ea50f-8a05-4025-ab01-0072894691e6", Style = "style" },
                               new Dashboard{ Name = "Dashboard2", GUID = "2adccadc-7a05-419d-b8c3-9578db9a81dc", Style = "style" } };
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
                            new Report{Name = "Riport1", Query = Queries.FirstOrDefault(x=>x.Name.Equals("Számlák")), Style = "style json", GUID="b2fc0e93-4260-47bb-9757-e682f077dd27" },
                            new Report{Name = "Riport2", Query = Queries.FirstOrDefault(x=>x.Name.Equals("Számlák")), Style = "style json", GUID="51adb95f-161b-4473-95ff-e0d6392f5caa" } };
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
                            new Query{  SQL = sql1, Name = "Számlák",TranslatedColumnNames=columns, GUID = "3066e94b-ff9e-454c-ab58-6a88436e4b52", NextUpdating = System.DateTime.Now, UpdatePeriod = new TimeSpan(1, 0, 0, 0) } };
                }
                return queries;
            }
        }

        private static string columns = @"{ name: ""Table_95_Field_1"", text: ""Számlafej ID"", hidden: false }
                                        { name: ""Table_95_Field_3"", text: ""Bizonylat azonosító"", hidden: true }
                                        { name: ""Table_95_Field_5"", text: ""Bizonylattömb"", hidden: true }
                                        { name: ""Table_95_Field_7"", text: ""Bizonylatszám"", hidden: true }
                                        { name: ""Table_95_Field_9"", text: ""Stornó bizonylat"", hidden: false }
                                        { name: ""Table_95_Field_11"", text: ""Helyesbítő bizonylat"", hidden: false }
                                        { name: ""Table_95_Field_13"", text: ""Ügyfél"", hidden: true }
                                        { name: ""Table_95_Field_15"", text: ""Ügyfélcím"", hidden: true }
                                        { name: ""Table_95_Field_17"", text: ""Kapcsolattartó"", hidden: true }
                                        { name: ""Table_95_Field_19"", text: ""Cégprofil"", hidden: true }
                                        { name: ""Table_95_Field_21"", text: ""Valuta"", hidden: true }
                                        { name: ""Table_95_Field_23"", text: ""Részlegszám"", hidden: true }
                                        { name: ""Table_95_Field_25"", text: ""Munkaszám"", hidden: true }
                                        { name: ""Table_95_Field_27"", text: ""Projekt"", hidden: true }
                                        { name: ""Table_95_Field_29"", text: ""Bizonylat"", hidden: true }
                                        { name: ""Table_95_Field_31"", text: ""Kelte"", hidden: true }
                                        { name: ""Table_95_Field_33"", text: ""Teljesítés dátuma"", hidden: true }
                                        { name: ""Table_95_Field_35"", text: ""Esedékesség"", hidden: true }
                                        { name: ""Table_95_Field_37"", text: ""Fizetési mód"", hidden: true }
                                        { name: ""Table_95_Field_39"", text: ""Raktár azonosító"", hidden: true }
                                        { name: ""Table_95_Field_41"", text: ""Nettó érték"", hidden: true }
                                        { name: ""Table_95_Field_43"", text: ""Áfa értékű"", hidden: true }
                                        { name: ""Table_95_Field_45"", text: ""Bruttó érték"", hidden: true }
                                        { name: ""Table_95_Field_47"", text: ""Fennmaradó összeg"", hidden: true }
                                        { name: ""Table_95_Field_49"", text: ""Kiegyenlített összeg"", hidden: true }
                                        { name: ""Table_95_Field_51"", text: ""? Kiterjesztő azonosító?"", hidden: true }
                                        { name: ""Table_95_Field_53"", text: ""Felhasználható"", hidden: true }
                                        { name: ""Table_95_Field_55"", text: ""Raktári bevét"", hidden: true }
                                        { name: ""TopComment"", text: ""Felső megjegyzés"", hidden: true }
                                        { name: ""Comment"", text: ""Megjegyzés"", hidden: true }
                                        { name: ""ReportRepository"", text: ""Bizonylatkép"", hidden: false }
                                        { name: ""Table_95_Field_63"", text: ""Bizonylat oldalainak száma"", hidden: false }
                                        { name: ""Table_95_Field_65"", text: ""RowVersion"", hidden: false }
                                        { name: ""Table_95_Field_67"", text: ""Vevő"", hidden: true }
                                        { name: ""Table_95_Field_69"", text: ""Vevő kód"", hidden: true }
                                        { name: ""Table_95_Field_71"", text: ""Rögzítés dátuma"", hidden: true }
                                        { name: ""Table_95_Field_73"", text: ""Munkatárs"", hidden: true }
                                        { name: ""Table_95_Field_75"", text: ""Logokép"", hidden: false }
                                        { name: ""Table_95_Field_77"", text: ""Teljes bizonylatnév"", hidden: true }
                                        { name: ""Table_95_Field_79"", text: ""Végső termék"", hidden: false }
                                        { name: ""Table_95_Field_81"", text: ""Végső terméknév"", hidden: false }
                                        { name: ""Table_95_Field_83"", text: ""Lockolt"", hidden: false }
                                        { name: ""Table_95_Field_85"", text: ""Áfa nélkül"", hidden: true }
                                        { name: ""Table_95_Field_87"", text: ""Nyelv"", hidden: true }
                                        { name: ""Table_95_Field_89"", text: ""Kiegyenlítés dátuma"", hidden: true }
                                        { name: ""Table_95_Field_91"", text: ""ReportDesign"", hidden: true }
                                        { name: ""Table_95_Field_93"", text: ""IntrastatTransactionCode"", hidden: false }
                                        { name: ""Table_95_Field_95"", text: ""IntrastatDeliveryTermCode"", hidden: false }
                                        { name: ""Table_95_Field_97"", text: ""IntrastatModeOfTransportCode"", hidden: false }
                                        { name: ""Table_95_Field_99"", text: ""IntrastatCountryNonEU"", hidden: false }
                                        { name: ""Table_95_Field_101"", text: ""IntrastatCountryEU"", hidden: false }
                                        { name: ""Table_95_Field_103"", text: ""Nettó súly"", hidden: true }
                                        { name: ""Table_95_Field_105"", text: ""Bruttó súly"", hidden: true }
                                        { name: ""Table_95_Field_107"", text: ""Bizonylat státusza"", hidden: true }
                                        { name: ""Table_95_Field_109"", text: ""SettlementPeriodDate"", hidden: true }
                                        { name: ""Table_95_Field_111"", text: ""Eszámla"", hidden: true }
                                        { name: ""Table_95_Field_113"", text: ""EDI számla"", hidden: true }
                                        { name: ""Table_95_Field_115"", text: ""Házipénztár"", hidden: true }
                                        { name: ""Table_95_Field_117"", text: ""Kiegyenlített összeg"", hidden: true }
                                        { name: ""Table_95_Field_119"", text: ""Fennmaradó összeg"", hidden: true }
                                        { name: ""Table_95_Field_121"", text: ""Bizonylat sablon"", hidden: true }
                                        { name: ""Table_95_Field_123"", text: ""Nyomtatás dátuma"", hidden: true }
                                        { name: ""Table_95_Field_125"", text: ""Távoli nyomtatás hatálya"", hidden: true }
                                        { name: ""Table_95_Field_127"", text: ""Távoli nyomtatás dátuma"", hidden: true }
                                        { name: ""Table_95_Field_129"", text: ""Egységár tizedesjegyek"", hidden: true }
                                        { name: ""Table_95_Field_131"", text: ""Nettó érték tizedes jegyek"", hidden: true }
                                        { name: ""Table_95_Field_133"", text: ""ÁFA érték tizedes jegyek"", hidden: true }
                                        { name: ""Table_95_Field_135"", text: ""Bruttó érték tizedes jegyek"", hidden: true }
                                        { name: ""Table_95_Field_137"", text: ""GrandTotalDigits"", hidden: false }
                                        { name: ""Table_95_Field_139"", text: ""Valuta szorzó"", hidden: false }
                                        { name: ""Table_95_Field_141"", text: ""Szállítási cím"", hidden: true }
                                        { name: ""Table_95_Field_143"", text: ""Szállítási mód"", hidden: true }
                                        { name: ""Table_95_Field_145"", text: ""PaymentAccounting"", hidden: true }
                                        { name: ""InheretedTopComment"", text: ""Örökölt felső megjegyzés"", hidden: false }
                                        { name: ""InheretedBottomComment"", text: ""Örökölt alső megjegyzés"", hidden: false }
                                        { name: ""Table_95_Field_151"", text: ""SalesOpportunity"", hidden: false }
                                        { name: ""Table_95_Field_153"", text: ""DetailGroupByType"", hidden: false }
                                        { name: ""Table_95_Field_155"", text: ""RetentionWarranty"", hidden: false }
                                        { name: ""Table_95_Field_157"", text: ""RetentionPaymentDate"", hidden: false }
                                        { name: ""Table_95_Field_159"", text: ""AppliedPriceRule"", hidden: false }
                                        { name: ""Table_95_Field_161"", text: ""AccountingStockMovementType"", hidden: false }
                                        { name: ""Table_95_Field_163"", text: ""Webshop azonosító"", hidden: true }
                                        { name: ""Description"", text: ""Leírás"", hidden: true }
                                        { name: ""Table_95_Field_167"", text: ""ActualCurrencyRate"", hidden: false }
                                        { name: ""Table_95_Field_169"", text: ""DiscountMode"", hidden: false }
                                        { name: ""Table_95_Field_171"", text: ""FulfilledCertificate"", hidden: false }
                                        { name: ""Table_95_Field_173"", text: ""CustomerStockInIntermediate"", hidden: false }
                                        { name: ""Table_95_Field_175"", text: ""MadeWithCashRegister"", hidden: false }
                                        { name: ""NAVRepository"", text: ""NAVRepository,"", hidden: false }
                                        { name: ""Table_95_Field_179"", text: ""SecQuantity"", hidden: false }
                                        { name: ""Table_95_Field_181"", text: ""SecQuantityUnit"", hidden: false }
                                        { name: ""Table_95_Field_183"", text: ""SecQuantityDigits"", hidden: false }
                                        { name: ""Table_95_Field_185"", text: ""ConversionRate"", hidden: false }
                                        { name: ""Table_95_Field_187"", text: ""QuantityUnit"", hidden: false }
                                        { name: ""Table_95_Field_189"", text: ""QuantityDigits"", hidden: false }
                                        { name: ""Table_95_Field_191"", text: ""Magánszemély"", hidden: true }";

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