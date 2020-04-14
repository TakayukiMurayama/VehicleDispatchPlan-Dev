namespace VehicleDispatchPlan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Trainee", "TrainingCourseCd", c => c.String(nullable: false));
            DropColumn("dbo.T_Trainee", "TrainingCd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.T_Trainee", "TrainingCd", c => c.String(nullable: false));
            DropColumn("dbo.T_Trainee", "TrainingCourseCd");
        }
    }
}
