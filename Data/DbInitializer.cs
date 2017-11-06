using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            }
        }

        private static List<User> users;

        private static List<User> Users
        {
            get
            {
                if (users == null)
                    users = new List<User>{
                        new User{ Name="Admin", Password="admin", EmailAddress="asd@asd.com", GUID=CreateGUID.GetGUID()},
                        new User{ Name="Teszt", Password="teszt",EmailAddress="teszt@teszt.com",GUID=CreateGUID.GetGUID()}
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
                               new Dashboard{ Name = "Dashboard1", GUID = CreateGUID.GetGUID(), Style = "style" },
                               new Dashboard{ Name = "Dashboard2", GUID = CreateGUID.GetGUID(), Style = "style" } };
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
                            new Report{Name = "Riport1", Query = Queries.FirstOrDefault(x=>x.Name.Equals("Számlák")), Style = "style json", GUID=CreateGUID.GetGUID() },
                            new Report{Name = "Riport2", Query = Queries.FirstOrDefault(x=>x.Name.Equals("Számlák")), Style = "style json", GUID=CreateGUID.GetGUID() } };
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
                            new Query{  SQL = sql1, Name = "Számlák", GUID = CreateGUID.GetGUID(), NextUpdating = System.DateTime.Now, UpdatePeriod = new TimeSpan(1, 0, 0, 0) } };
                }
                return queries;
            }
        }

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
        WHERE (TBL271.[Id] = 3)";

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
    }
}