namespace ContractAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_contracts_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                        StartAt = c.DateTime(nullable: false, precision: 0),
                        EndAt = c.DateTime(nullable: false, precision: 0),
                        YearlySalary = c.Int(nullable: false),
                        TransferFee = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Contracts");
        }
    }
}
