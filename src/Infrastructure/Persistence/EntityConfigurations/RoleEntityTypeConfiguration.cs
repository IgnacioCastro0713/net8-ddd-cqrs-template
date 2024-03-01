using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class RoleEntityTypeConfiguration : BaseEntityTypeConfiguration<Role>
{
	public override void Configure(EntityTypeBuilder<Role> builder)
	{
		#region Properties

		builder
			.Property(e => e.Name)
			.HasMaxLength(50)
			.IsRequired();

		builder
			.Property(e => e.Description)
			.HasMaxLength(100);

		#endregion

		base.Configure(builder);
	}
}