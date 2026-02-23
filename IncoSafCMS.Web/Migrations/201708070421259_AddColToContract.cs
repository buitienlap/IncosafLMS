namespace IncosafCMS.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddColToContract : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contract", "NguoiLienHe", c => c.String());
            AddColumn("dbo.Contract", "DienThoaiNguoiLienHe", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Contract", "NguoiLienHe");
            DropColumn("dbo.Contract", "DienThoaiNguoiLienHe");
        }
    }
}
