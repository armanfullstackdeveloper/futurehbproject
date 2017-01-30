using DataModel.Enums;

namespace DataModel.Entities
{
    public class Role:EntityBase<Role>
    {
        public ERole Id { get; set; }
        public string Name { get; set; }
    }
}
