namespace database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "Category_Id" });
            CreateTable(
                "dbo.CategoryCategories",
                c => new
                    {
                        Category_Id = c.Int(nullable: false),
                        Category_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_Id, t.Category_Id1 })
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id1)
                .Index(t => t.Category_Id)
                .Index(t => t.Category_Id1);
            
            DropColumn("dbo.Categories", "Category_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Category_Id", c => c.Int());
            DropForeignKey("dbo.CategoryCategories", "Category_Id1", "dbo.Categories");
            DropForeignKey("dbo.CategoryCategories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.CategoryCategories", new[] { "Category_Id1" });
            DropIndex("dbo.CategoryCategories", new[] { "Category_Id" });
            DropTable("dbo.CategoryCategories");
            CreateIndex("dbo.Categories", "Category_Id");
            AddForeignKey("dbo.Categories", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
