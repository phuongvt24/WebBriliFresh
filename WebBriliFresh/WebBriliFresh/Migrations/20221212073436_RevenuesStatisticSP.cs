using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBriliFresh.Migrations
{
    /// <inheritdoc />
    public partial class RevenuesStatisticSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcSql = @"CREATE OR ALTER PROC GetRevenueStatistic(@fromDate NVARCHAR, @toDate NVARCHAR) AS
                Select OrderDate, sum(orderTotal) As Revenues, sum(orderTotal - (od.Quantity * p.OriginalPrice)) As Benefit
                From [Order] o inner join Order_Details od
                On o.OrderId = od.OrderId
                inner join Product p
                on od.ProID = p.ProID
                where OrderDate >= cast(@fromDate as date) and OrderDate <= cast(@toDate as date)
                group by OrderDate";
            migrationBuilder.Sql(createProcSql);


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropProcSql = "DROP PROC GetRevenueStatistic";
            migrationBuilder.Sql(dropProcSql);

        }
    }
}
