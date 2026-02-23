namespace IncosafCMS.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddAutoAccreNumberTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutoAccreNumber",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Dept = c.String(nullable: false, maxLength: 10),
                    Prefix = c.String(nullable: false, maxLength: 10),
                    AccreNumber = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.AutoAccreNumber");
        }
    }
}
