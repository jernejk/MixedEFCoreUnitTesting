using System;

namespace MixedDbUnitTests.Persistance.Domain
{
    public class TestData
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool IsDeleted { get; set; }

        public ChildData Child { get; set; }

        public int ChildId { get; set; }
    }
}
