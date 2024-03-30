using Microsoft.EntityFrameworkCore.Migrations;

namespace AzureDevopsTracker.Migrations
{
    public partial class FunctionToTimeInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var rawFunction = @"CREATE OR ALTER FUNCTION ToTime (@seconds int)  
								RETURNS varchar(50)  
								AS BEGIN  
									declare @seconds_in_a_day int = 86400;
									declare @seconds_in_an_hour int = 3600;
									declare @seconds_in_a_minute int = 60;

									declare @text_to_return varchar(50) = '';
									if(@seconds >= @seconds_in_a_day)
									begin
										declare @completed_days int = @seconds/@seconds_in_a_day;
										select @text_to_return = CONCAT(@text_to_return, @completed_days, ' day(s)')
										select @seconds = @seconds - (@seconds_in_a_day * @completed_days)
									end

									if(@seconds >= @seconds_in_an_hour)
									begin
										declare @completed_hours int = @seconds/@seconds_in_an_hour;
										select @text_to_return = CONCAT(@text_to_return, ' ', @completed_hours, ' hour(s)')
										select @seconds = @seconds - (@seconds_in_an_hour * @completed_hours)
									end

									if(@seconds >= @seconds_in_a_minute)
									begin
										declare @completed_minutes int = @seconds/@seconds_in_a_minute;
										select @text_to_return = CONCAT(@text_to_return, ' ', @completed_minutes, ' minute(s)')
										select @seconds = @seconds - (@seconds_in_a_minute * @completed_minutes)
									end

									return LTRIM(RTRIM(@text_to_return));
								END";
            migrationBuilder.Sql(rawFunction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropFunction = @$"DROP FUNCTION IF EXISTS dbo.ToTime";
            migrationBuilder.Sql(dropFunction);
        }
    }
}
