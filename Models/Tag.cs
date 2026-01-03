using MVCProniaTask.Models.Command;

namespace MVCProniaTask.Models
{
    public class Tag:BaseEntity
    {
        public string name { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; } = [];
    }
}
