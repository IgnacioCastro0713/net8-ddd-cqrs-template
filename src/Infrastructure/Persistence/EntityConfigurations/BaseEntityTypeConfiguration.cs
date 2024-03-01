using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public abstract class BaseEntityTypeConfiguration<TAuditableEntity> : IEntityTypeConfiguration<TAuditableEntity>
	where TAuditableEntity : BaseAuditableEntity
{
	public virtual void Configure(EntityTypeBuilder<TAuditableEntity> builder)
	{
		#region Properties

		builder
			.Property(entity => entity.CreatedAt)
			.HasColumnType("datetime")
			.IsRequired(false);

		builder
			.Property(entity => entity.CreatedBy)
			.IsRequired(false);

		builder
			.Property(entity => entity.UpdatedAt)
			.HasColumnType("datetime")
			.IsRequired(false);

		builder
			.Property(entity => entity.UpdatedBy)
			.IsRequired(false);

		builder
			.Property(e => e.DeletedAt)
			.HasColumnType("datetime")
			.IsRequired(false);

		builder
			.Property(entity => entity.DeletedBy)
			.IsRequired(false);

		#endregion

		#region SoftDelete

		builder.HasQueryFilter(e => e.DeletedAt == null);

		#endregion

		#region Relationships

		builder
			.HasOne<User>()
			.WithMany()
			.HasForeignKey(e => e.CreatedBy)
			.IsRequired(false);

		builder
			.HasOne<User>()
			.WithMany()
			.HasForeignKey(e => e.UpdatedBy)
			.IsRequired(false);

		builder
			.HasOne<User>()
			.WithMany()
			.HasForeignKey(e => e.DeletedBy)
			.IsRequired(false);

		#endregion
	}
}