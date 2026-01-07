using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MVCProniaTask.Confiqurations
{
    public class ProductConfiquration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
            builder.Property(x=>x.Description).IsRequired(false).HasMaxLength(1024);

            builder.Property(x => x.Price).IsRequired().HasPrecision(10, 2);
            builder.HasCheckConstraint("CK_Products_Price", "[Price]>0");

            builder.ToTable(options =>
            {
                options.HasCheckConstraint("CK_Products_Price", "[Price]>0");
            });
            builder.Property(x=>x.SKU).IsRequired().HasMaxLength(64);
            builder.HasIndex(x=> x.SKU).IsUnique();
            builder.Property(x=>x.MainImage).IsRequired().HasMaxLength(256);
            builder.Property(x=>x.HoverImage).IsRequired().HasMaxLength(256);

        }
    }
}
