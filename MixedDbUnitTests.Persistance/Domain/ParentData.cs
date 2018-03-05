namespace MixedDbUnitTests.Persistance.Domain
{
    public class ParentData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChildId { get; set; }
        public virtual ChildData Child { get; set; }
    }
}
