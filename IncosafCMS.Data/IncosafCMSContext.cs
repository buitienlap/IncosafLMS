using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Logging;
using IncosafCMS.Data.Identity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using IncosafCMS.Core.DomainModels.Identity;



namespace IncosafCMS.Data
{
    public class IncosafCMSContext : IdentityDbContext<ApplicationIdentityUser, ApplicationIdentityRole, int, ApplicationIdentityUserLogin, ApplicationIdentityUserRole, ApplicationIdentityUserClaim>, IEntitiesContext
    {
        private ObjectContext _objectContext;
        private DbTransaction _transaction;
        private static readonly object Lock = new object();
        private static bool _databaseInitialized;
        //public DbSet<Equipment> Equipments { get; set; }//thêm 4.7.2025
        public DbSet<Accreditation> Accreditations { get; set; }//thêm 4.7.2025
        //public DbSet<AppUser> AppUsers { get; set; }//thêm 4.7.2025

        //public DbSet<StampSerial> StampSerials { get; set; } //thêm 29.7.2025
        //public DbSet<v_ActTurnOver> v_ActTurnOver { get; set; }
        //public DbSet<v_ActPayment> v_ActPayment { get; set; }
        public DbSet<Contract> Contracts { get; set; } // thêm dòng này
        //public DbSet<OriginalSpec> OriginalSpecs { get; set; } //thêm 24.9.2025
        //public DbSet<Contract_Doc> Contract_Docs { get; set; } //thêm 26.9.2025
        //public DbSet<Department> Departments { get; set; } //thêm 4.11.2025
        //public DbSet<SanLuongDK> SanLuongDK { get; set; } //thêm 4.11.2025
        public DbSet<AppUser> AppUser { get; set; } //thêm 5.11.2025
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        // Course entity
        public DbSet<Course> Courses { get; set; }
        public DbSet<ExamAssignment> ExamAssignments { get; set; }
        public IncosafCMSContext()
            : base("AppContext")
        {
        }

        public IncosafCMSContext(string nameOrConnectionString, ILogger logger)
            : base(nameOrConnectionString)
        {
            if (logger != null)
            {
                Database.Log = logger.Log;
            }

            if (_databaseInitialized)
            {
                return;
            }
            lock (Lock)
            {               

                if (!_databaseInitialized)
                {
                    // Set the database intializer which is run once during application start
                    // This seeds the database with admin user credentials and admin role
                    return;
                    Database.SetInitializer(new ApplicationDbInitializer());
                    //this.Configuration.LazyLoadingEnabled = false;
                    _databaseInitialized = true;
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<IncosafCMSContext>(null);
            base.OnModelCreating(modelBuilder);
            EfConfig.ConfigureEf(modelBuilder);
           

            // v_ActTurnOver có khóa composite {ma_hd, so_ct}
            //modelBuilder.Entity<v_ActTurnOver>()
            //    .HasKey(e => new { e.ma_hd, e.so_ct });

            //// v_ActPayment cũng có thể như vậy nếu cần
            //modelBuilder.Entity<v_ActPayment>()
            //    .HasKey(e => new { e.ma_hd, e.so_ct });
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Added);
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Modified);
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Deleted);
        }

        public void BeginTransaction()
        {
            _objectContext = ((IObjectContextAdapter)this).ObjectContext;
            if (_objectContext.Connection.State == ConnectionState.Open)
            {
                return;
            }
            _objectContext.Connection.Open();
            _transaction = _objectContext.Connection.BeginTransaction();
        }

        public int Commit()
        {
            var saveChanges = SaveChanges();
            _transaction.Commit();
            return saveChanges;
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public Task<int> CommitAsync()
        {
            var saveChangesAsync = SaveChangesAsync();
            _transaction.Commit();
            return saveChangesAsync;
        }

        private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : BaseEntity
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = entityState;
        }

        private DbEntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var dbEntityEntry = Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }
            return dbEntityEntry;
        }

        public DbContext ToDbContext()
        {
            return (DbContext)this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_objectContext != null && _objectContext.Connection.State == ConnectionState.Open)
                {
                    _objectContext.Connection.Close();
                }
                if (_objectContext != null)
                {
                    _objectContext.Dispose();
                }
                if (_transaction != null)
                {
                    _transaction.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
