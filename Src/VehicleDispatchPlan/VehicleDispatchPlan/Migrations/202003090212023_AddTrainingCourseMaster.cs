namespace VehicleDispatchPlan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainingCourseMaster : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.M_TrainingCourse",
                c => new
                    {
                        TrainingCourseCd = c.String(nullable: false, maxLength: 128),
                        TrainingCourseName = c.String(nullable: false),
                        GraduationDays = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TrainingCourseCd);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.M_TrainingCourse");
        }
    }
}
