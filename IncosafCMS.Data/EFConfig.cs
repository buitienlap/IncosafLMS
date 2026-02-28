using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Data.Identity.Models;
using IncosafCMS.Core.DomainModels.Identity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace IncosafCMS.Data
{
    public class EfConfig
    {
        public static void ConfigureEf(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Customer>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Accreditation>()
            //    .Property(e => e.Id)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Accreditation>().HasOptional(p => p.AccTask).WithMany(x => x.Accreditations).HasForeignKey(key => key.AccTask_Id);
            
            //modelBuilder.Entity<Contract>().ToTable("Contract")
            //    .Property(e => e.Id)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Province>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // added by lapbt
            modelBuilder.Entity<ProvinceFull>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<v_ProvinceDistrict>()
                .HasKey(e => e.Ma_QH);

            modelBuilder.Entity<Department>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<EmployeePosition>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            modelBuilder.Entity<AppUser>()
               .HasMany(s => s.Roles)
               .WithOptional().HasForeignKey(x => x.UserId);


            modelBuilder.Entity<ApplicationIdentityUser>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationIdentityRole>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationIdentityUserClaim>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ApplicationPermissionRole>().HasKey(r => new { r.RoleId, r.PermissionId });
            //modelBuilder.Entity<ApplicationPermissionRole>().HasRequired(r => r.Permission).WithRequiredPrincipal().WillCascadeOnDelete(true);

            modelBuilder.Entity<ApplicationIdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<ApplicationIdentityUserLogin>().HasKey(r => new { r.ProviderKey, r.UserId });

            modelBuilder.Entity<ApplicationUserLogin>().HasKey<int>(l => l.UserId);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<AppPermission>().HasKey<int>(r => r.Id);

            modelBuilder.Entity<AppPermission>()
               .HasMany(s => s.Roles)
               .WithOptional()
               .WillCascadeOnDelete(true);
            //modelBuilder.Entity<ApplicationPermissionRole>().HasKey(r => new { r.RoleId, r.PermissionId });
            
            modelBuilder.Entity<CourseCategory>()
                .ToTable("CourseCategory")
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Question>()
                .ToTable("Question")
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Answer>()
                .ToTable("Answer")
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}