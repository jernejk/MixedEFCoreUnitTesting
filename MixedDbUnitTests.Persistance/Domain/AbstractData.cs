namespace MixedDbUnitTests.Persistance.Domain
{
    public abstract class AbstractData
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
