using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public abstract class BaseEntityTypeConfiguration<TBase> : IEntityTypeConfiguration<TBase>
	where TBase : BaseEntity
{
	public virtual void Configure(EntityTypeBuilder<TBase> builder)
	{
		builder
			.Property(entity => entity.CreatedBy)
			.HasMaxLength(150)
			.IsRequired(false);

		builder
			.Property(entity => entity.UpdatedBy)
			.HasMaxLength(150)
			.IsRequired(false);

		builder
			.Property(entity => entity.CreatedAt)
			.HasColumnType("datetime");

		builder
			.Property(entity => entity.UpdatedAt)
			.HasColumnType("datetime");
	}
}