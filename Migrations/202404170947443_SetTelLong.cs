namespace OniroHotel.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class SetTelLong : DbMigration
    {
        public override void Up()
        {            
            AlterColumn("dbo.Users", "Telephone", c => c.Long());
        }
        
        public override void Down()
        {

        }
    }
}
