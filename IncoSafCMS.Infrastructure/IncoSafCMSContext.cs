using IncoSafCMS.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Infrastructure
{
    public class IncoSafCMSContext : DbContext
    {
        public IncoSafCMSContext() : base("name=IncoSafCMSContext")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Custommer> Custommers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Accreditation> Accreditations { get; set; }
        public DbSet<Equiment> Equiments { get; set; }
        public DbSet<EquimentPartion> EquimentPartions { get; set; }
        public DbSet<Core.Task> Tasks { get; set; }
        public DbSet<Standard> Standards { get; set; }
        public DbSet<Specifications> Specificationses { get; set; }
        public DbSet<TechnicalDocument> TechnicalDocuments { get; set; }
        public DbSet<Procedure> Procedures { get; set; }

    }
}
