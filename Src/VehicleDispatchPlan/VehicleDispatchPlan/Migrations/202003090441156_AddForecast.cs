namespace VehicleDispatchPlan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForecast : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_Forecast",
                c => new
                    {
                        Year = c.String(nullable: false, maxLength: 128),
                        Period = c.String(nullable: false, maxLength: 128),
                        InstructorAmt = c.Int(nullable: false),
                        ClassQty = c.Int(nullable: false),
                        LodgingRatio = c.Double(nullable: false),
                        NotDrivingRatio = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.Year, t.Period });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.T_Forecast");
        }
    }
}
