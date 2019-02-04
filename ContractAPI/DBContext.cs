using MySql.Data.Entity;
using System.Data.Entity;
using ContractAPI.Models;

namespace DBContext
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ContractDBContext : DbContext
    {
        public DbSet<Contract> Contracts { get; set; }
        public ContractDBContext() : base("Default") { }
    }
}