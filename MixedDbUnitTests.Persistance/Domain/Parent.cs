namespace MixedDbUnitTests.Persistance.Domain;

public class Parent
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ChildId { get; set; }
    public virtual Child Child { get; set; }
}
