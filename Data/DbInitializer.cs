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
                    context.Query.AddRange(Query1);

                context.SaveChanges();
            }
        }

        static List<User> Users
        {
            get
            {
                return new List<User>
                {
                    new User{ Name="Admin", Password="admin", EmailAddress="asd@asd.com", GUID="123"},
                    new User{ Name="Teszt", Password="teszt",EmailAddress="teszt@teszt.com",GUID="234"}
                };
            }
        }

        static Query Query1 { get => new Query { SQL = asd, Name = "Számlák", GUID = "456", NextUpdating = System.DateTime.Now, UpdatePeriod = new TimeSpan(1, 0, 0, 0) }; }

        private static string asd = @"exec sp_executesql N'SELECT  TBL175.[Id] ''Id'',
       CAST(0 AS BIT) ''Kijelölt'',
       TBL188.[Name] ''CompanyProfile.Name'',
       TBL175.[VoucherNumber] ''CustomerStockOut.VoucherNumber'',
       TBL175.[VoucherDate] ''CustomerStockOut.VoucherDate'',
       TBL175.[VoucherType] ''VoucherType'',
       TBL175.[CustomerNameDisplay] ''CustomerStockOut.CustomerName'',
       TBL175.[CustomerCodeDisplay] ''CustomerStockOut.CustomerCode'',
       CentralAddresTableName_CustomerStockOut_CustomerAddress.[AddressCountry] ''AddressCountry'',
       CentralAddresTableName_CustomerStockOut_CustomerAddress.[AddressCountryState] ''AddressCountryState'',
       CASE
	WHEN (LTRIM(RTRIM(CentralAddresTableName_CustomerStockOut_CustomerAddress.[AddressCity])) <> '''') THEN (CentralAddresTableName_CustomerStockOut_CustomerAddress.[AddressCity] + '', '' + CentralAddresTableName_CustomerStockOut_CustomerAddress.[AddressStreet])
	ELSE CentralAddresTableName_CustomerStockOut_CustomerAddress.[AddressStreet]
END ''CustomerStockOut.CustomerAddress'',
       CentralAddresTableName_CustomerStockOut_CustomerAddress.[CustomerAddress_Name] ''CustomerAddress_Name'',
       TBL175.[Customer] ''CustomerStockOut.Customer'',
       TBL175.[FulfillmentDate] ''CustomerStockOut.FulfillmentDate'',
       TBL182.[Name] ''StockNameDisplay'',
       TBL175.[PaymentDate] ''CustomerStockOut.PaymentDate'',
       TBL175.[SettlementPeriodDate] ''CustomerStockOut.SettlementPeriodDate'',
       TBL177.[Name] ''CustomerStockOut.PaymentMethod'',
       TBL178.[Name] ''CustomerStockOut.Currency'',
       TBL175.[CurrencyRate] ''CustomerStockOut.CurrencyRate'',
       TBL175.[NetValue] ''NetValue'',
       TBL175.[VatValue] ''VatValue'',
       TBL175.[GrossValue] ''GrossValue'',
       CASE
	WHEN (TBL175.[VoucherType] = @Param126) THEN TBL175.[GrossValue]
	ELSE TBL175.[AcquitValue]
END ''AcquitValue'',
       TBL175.[GrossValue] - TBL175.[AcquitValue] ''RemainingAmount'',
       TBL175.[AcquitedDate] ''CustomerStockOut.AcquitedDate'',
       TBL175.[AvailableGrossValue] ''AvailableGrossValue'',
       TBL175.[ExternalId] ''CustomerStockOut.ExternalId'',
       ROUND(CAST(CAST(TBL175.[NetValue] AS DOUBLE PRECISION) * CAST(TBL175.[CurrencyRate] AS DOUBLE PRECISION) AS DECIMAL(18, 6)),0) ''NetValue (HUF)'',
       ROUND(CAST(CAST(TBL175.[VatValue] AS DOUBLE PRECISION) * CAST(TBL175.[CurrencyRate] AS DOUBLE PRECISION) AS DECIMAL(18, 6)),0) ''VatValue (HUF)'',
       ROUND(ROUND(CAST(CAST(TBL175.[NetValue] AS DOUBLE PRECISION) * CAST(TBL175.[CurrencyRate] AS DOUBLE PRECISION) AS DECIMAL(18, 6)),0) + ROUND(CAST(CAST(TBL175.[VatValue] AS DOUBLE PRECISION) * CAST(TBL175.[CurrencyRate] AS DOUBLE PRECISION) AS DECIMAL(18, 6)),0),0) ''GrossValue (HUF)'',
       TBL189.[Name] ''CustomerBid.TransportMode'',
       TBL179.[Name] ''CustomerStockOut.Division'',
       TBL180.[Name] ''CustomerStockOut.JobNumber'',
       TBL181.[Subject] ''CustomerStockOut.Business'',
       CASE
	WHEN TBL175.[PrintDateTime] IS NOT NULL THEN CAST(1 AS BIT)
	ELSE CAST(0 AS BIT)
END ''IsPrinted'',
       TBL175.[EVoucher] ''CustomerStockOut.EVoucher'',
       TBL175.[EDIVoucher] ''CustomerStockOut.EDIVoucher'',
       TBL175.[PurchasePrice] ''PurchasePrice'',
       TBL175.[PriceGap] ''PriceGap'',
       CASE
	WHEN (COALESCE(ABS(TBL175.[PurchasePrice]), @Param127) = @Param128) THEN NULL
	ELSE ROUND(CAST(CAST(ROUND(CAST(CAST(COALESCE(TBL175.[PriceGap], @Param129) AS DOUBLE PRECISION) / CAST(COALESCE(ABS(TBL175.[PurchasePrice]), @Param130) AS DOUBLE PRECISION) AS DECIMAL(18, 6)),3) AS DOUBLE PRECISION) * CAST(100 AS DOUBLE PRECISION) AS DECIMAL(18, 6)),1)
END ''Árrés (%)'',
       TBL175.[CustomerStockOutStatus] ''CustomerStockOut.InvoiceStatus'',
       TBL175.[Locked] ''CustomerStockOut.Locked'',
       TBL175.[RemoteValidToDate] ''RemoteValidToDate'',
       TBL175.[RemotePrintDateTime] ''RemotePrintDateTime'',
       TBL175.[RetentionPaymentDate] ''RetentionPaymentDate'',
       TBL175.[RetentionWarranty] ''RetentionWarranty'',
       TBL178.[Id] ''Currency.Id'',
       TBL178.[IsPost] ''Currency.IsPost'',
       TBL178.[Sign] ''Currency.Sign'',
       TBL175.[NetValueDigits] ''NetValueDigits'',
       TBL175.[VatValueDigits] ''VatValueDigits'',
       TBL175.[GrossValueDigits] ''GrossValueDigits'',
       TBL175.[UnitPriceDigits] ''UnitPriceDigits'',
       TBL175.[GrandTotalDigits] ''GrandTotalDigits'',
       TBL187.[Name] ''Owner'',
       TBL175.[NetWeight] ''NetWeight'',
       TBL175.[GrossWeight] ''GrossWeight'',
       TBL175.[FulfilledCertificate] ''FulfilledCertificate'',
       TBL188.[Name] ''Telephely''
FROM dbo.[CustomerStockOut] TBL175
LEFT JOIN (SELECT  TBL190.[Id] ''CustomerId'',
       LTRIM(RTRIM(COALESCE(TBL192.[Name], ''''))) ''AddressCountry'',
       LTRIM(RTRIM(COALESCE(TBL193.[Name], ''''))) ''AddressCountryState'',
       LTRIM(RTRIM(COALESCE(TBL190.[AddressName], ''''))) ''CustomerAddress_Name'',
       LTRIM(RTRIM(LTRIM(RTRIM((LTRIM(RTRIM(COALESCE(TBL190.[Zip], ''''))) + '' '' + LTRIM(RTRIM(COALESCE(TBL190.[City], '''')))))))) ''AddressCity'',
       REPLACE(REPLACE(REPLACE(LTRIM(RTRIM((LTRIM(RTRIM(COALESCE(TBL190.[Street], ''''))) + '' '' + LTRIM(RTRIM(COALESCE(TBL191.[Name], ''''))) + '' '' + LTRIM(RTRIM(COALESCE(TBL190.[Number], ''''))) + '' '' + LTRIM(RTRIM(COALESCE(TBL190.[Building], ''''))) + '' '' + LTRIM(RTRIM(COALESCE(TBL190.[Staircase], ''''))) + '' '' + LTRIM(RTRIM(COALESCE(TBL190.[Floor], ''''))) + '' '' + LTRIM(RTRIM(COALESCE(TBL190.[Door], '''')))))),''  '','' ''),''  '','' ''),''  '','' '') ''AddressStreet''
FROM dbo.[CustomerAddress] TBL190
LEFT JOIN dbo.[PublicDomain] TBL191 ON (TBL190.[PublicDomain] = TBL191.[Id]) 
LEFT JOIN dbo.[Country] TBL192 ON (TBL190.[Country] = TBL192.[Id]) 
LEFT JOIN dbo.[CountryState] TBL193 ON (TBL190.[CountryState] = TBL193.[Id])) CentralAddresTableName_CustomerStockOut_CustomerAddress ON (TBL175.[CustomerAddress] = CentralAddresTableName_CustomerStockOut_CustomerAddress.[CustomerId]) 
LEFT JOIN dbo.[Customer] TBL176 ON (TBL175.[Customer] = TBL176.[Id]) 
LEFT JOIN dbo.[PaymentMethod] TBL177 ON (TBL175.[PaymentMethod] = TBL177.[Id]) 
LEFT JOIN dbo.[Currency] TBL178 ON (TBL175.[Currency] = TBL178.[Id]) 
LEFT JOIN dbo.[TransportMode] TBL189 ON (TBL189.[Id] = TBL175.[TransportMode]) 
LEFT JOIN dbo.[Division] TBL179 ON (TBL175.[Division] = TBL179.[Id]) 
LEFT JOIN dbo.[JobNumber] TBL180 ON (TBL175.[JobNumber] = TBL180.[Id]) 
LEFT JOIN dbo.[Business] TBL181 ON (TBL175.[Business] = TBL181.[Id]) 
LEFT JOIN dbo.[Stock] TBL182 ON (TBL175.[Stock] = TBL182.[Id]) 
LEFT JOIN dbo.[CompanyProfile] TBL188 ON (TBL175.[CompanyProfile] = TBL188.[Id]) 
LEFT JOIN dbo.[User] TBL187 ON (TBL187.[Id] = TBL175.[Owner]) 
WHERE TBL175.[VoucherType] IN (1, 12, 14, 28) AND TBL175.[Id] NOT IN (SELECT DISTINCT TBL212.[CustomerStockOut] ''Table_97_Field_3''
FROM dbo.[CustomerStockOutStockOutR] TBL213
INNER JOIN dbo.[CustomerStockOutDetail] TBL212 ON (TBL212.[Id] = TBL213.[CustomerStockOutDetail]) 
WHERE (TBL213.[VoucherType] = @Param131) AND (TBL213.[Cancelled] = @Param132)) AND (TBL175.[PaymentMethod] = @Param133) AND (TBL176.[Id] = @Param134)
ORDER BY TBL175.[Id] ASC',N'@Param126 int,@Param128 int,@Param127 decimal(1,0),@Param129 decimal(1,0),@Param130 decimal(1,0),@Param131 int,@Param132 bit,@Param133 int,@Param134 int',@Param126=28,@Param128=0,@Param127=0,@Param129=0,@Param130=0,@Param131=28,@Param132=0,@Param133=1,@Param134=1";

        private static void CleanAllTables(AppDbContext context)
        {
            context.ReportDashboardRel.RemoveRange(context.ReportDashboardRel.ToList());
            context.ReportUserRel.RemoveRange(context.ReportUserRel.ToList());
            context.UserDashboardRel.RemoveRange(context.UserDashboardRel.ToList());
            context.Dashboards.RemoveRange(context.Dashboards.ToList());
            context.Report.RemoveRange(context.Report.ToList());
            context.User.RemoveRange(context.User.ToList());
            context.Query.RemoveRange(context.Query.ToList());
        }
    }
}