namespace VehicleDispatchPlan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumnName1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Trainee", "AttendTypeCd", c => c.String(nullable: false));
            DropColumn("dbo.T_Trainee", "AttendType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.T_Trainee", "AttendType", c => c.String(nullable: false));
            DropColumn("dbo.T_Trainee", "AttendTypeCd");
        }
    }
}
