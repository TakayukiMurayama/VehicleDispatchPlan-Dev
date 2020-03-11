namespace VehicleDispatchPlan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.M_Agent",
                c => new
                    {
                        AgentCd = c.String(nullable: false, maxLength: 128),
                        AgentName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AgentCd);
            
            CreateTable(
                "dbo.M_CodeMaster",
                c => new
                    {
                        Div = c.String(nullable: false, maxLength: 128),
                        Cd = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.Div, t.Cd });
            
            CreateTable(
                "dbo.M_LodgingFacility",
                c => new
                    {
                        LodgingCd = c.String(nullable: false, maxLength: 128),
                        LodgingName = c.String(nullable: false),
                        PostalNo = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.LodgingCd);
            
            CreateTable(
                "dbo.T_Trainee",
                c => new
                    {
                        TraineeId = c.Int(nullable: false, identity: true),
                        TraineeName = c.String(nullable: false),
                        TrainingCd = c.String(nullable: false),
                        EntrancePlanDate = c.DateTime(nullable: false),
                        LodgingCd = c.String(),
                        AgentCd = c.String(),
                    })
                .PrimaryKey(t => t.TraineeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.T_Trainee");
            DropTable("dbo.M_LodgingFacility");
            DropTable("dbo.M_CodeMaster");
            DropTable("dbo.M_Agent");
        }
    }
}
