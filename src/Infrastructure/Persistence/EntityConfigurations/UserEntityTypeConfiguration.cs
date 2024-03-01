using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<User>
{
	public override void Configure(EntityTypeBuilder<User> builder)
	{
		#region Properties

		builder
			.Property(e => e.FirstName)
			.HasMaxLength(100)
			.IsRequired();

		builder
			.Property(e => e.LastName)
			.HasMaxLength(100);

		builder
			.Property(e => e.Email)
			.HasMaxLength(150)
			.IsRequired();

		builder
			.Property(e => e.NtUser)
			.HasMaxLength(10)
			.IsRequired();

		builder
			.Property(e => e.PrivacyPolicyApproval)
			.HasDefaultValue(false)
			.IsRequired();

		builder
			.Property(e => e.UserAgreementApproval)
			.HasDefaultValue(false)
			.IsRequired();

		#endregion

		#region Relationships

		builder
			.HasOne(e => e.Role)
			.WithMany()
			.HasForeignKey(e => e.RoleId)
			.IsRequired();

		#endregion

		#region Indexes

		builder.HasIndex(e => e.NtUser).IsUnique();
		builder.HasIndex(e => e.Email).IsUnique();

		#endregion

		base.Configure(builder);
	}
}