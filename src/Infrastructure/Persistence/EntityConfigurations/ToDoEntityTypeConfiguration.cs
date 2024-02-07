using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class ToDoEntityTypeConfiguration : BaseEntityTypeConfiguration<ToDo>
{
	public override void Configure(EntityTypeBuilder<ToDo> builder)
	{
		builder
			.Property(e => e.Name)
			.HasMaxLength(50);

		builder
			.Property(e => e.Description)
			.HasMaxLength(50);

		base.Configure(builder);
	}
}