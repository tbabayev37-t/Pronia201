using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVCProniaTask.Models.Basket;

namespace MVCProniaTask.Confiqurations
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
           builder.HasOne(x=>x.Product).WithMany(b=>b.BasketItems).HasForeignKey(x=>x.ProductId).HasPrincipalKey(x=>x.Id).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.AppUser).WithMany(b => b.BasketItems).HasForeignKey(x => x.AppUserId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(options =>
            {
                options.HasCheckConstraint("CK_BasketItems_Count", "[Count]>0");
            });
        }
    }
}
