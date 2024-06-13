using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clicker.Security.DAL.Data.Configurations;

public class RoleEntityConfiguration:  IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder) {
        builder.HasData(
            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User", NormalizedName = "User".ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString()},
            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "Admin".ToUpper(),ConcurrencyStamp = Guid.NewGuid().ToString() });
    }
}