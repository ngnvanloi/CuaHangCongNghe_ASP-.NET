namespace Code_First___Technology_Products.ModelsMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductDetails",
                c => new
                    {
                        DetailID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        ScreenSize = c.String(),
                        BatteryCapacity = c.String(),
                        CameraResolution = c.String(),
                        Ram = c.Int(nullable: false),
                        Rom = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DetailID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductDetails", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductDetails", new[] { "ProductID" });
            DropTable("dbo.ProductDetails");
        }
    }
}
