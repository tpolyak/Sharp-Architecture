using SharpArch.Core.DomainModel;

namespace Tests.DomainModel.Entities {
    public class TestEntity : Entity {
        [DomainSignature]
        public virtual string Name { get; set; }
    }
}