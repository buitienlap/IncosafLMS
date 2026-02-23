namespace IncosafCMS.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddDiaDiemThucHienColToContractTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contract", "DiaDiemThucHien", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Contract", "DiaDiemThucHien");
        }
    }
}
