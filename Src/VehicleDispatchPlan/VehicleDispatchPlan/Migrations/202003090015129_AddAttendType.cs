namespace VehicleDispatchPlan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttendType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Trainee", "AttendType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_Trainee", "AttendType");
        }
    }
}
