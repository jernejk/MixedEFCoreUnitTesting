namespace MixedDbUnitTests.Persistance.Domain
{
    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChildId { get; set; }
        public virtual Child Child { get; set; }
    }
}
