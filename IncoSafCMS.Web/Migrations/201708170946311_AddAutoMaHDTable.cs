namespace IncosafCMS.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddAutoMaHDTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutoMaHD",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Prefix = c.String(nullable: false, maxLength: 10),
                    ContractNumber = c.Int(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.AutoMaHD");
        }
    }
}
