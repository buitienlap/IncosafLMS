namespace IncosafCMS.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddColInternalPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InternalPayment", "IPType", c => c.Int(false, false, 0));
        }

        public override void Down()
        {
            DropColumn("dbo.InternalPayment", "IPType");
        }
    }
}
