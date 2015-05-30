namespace Tests.SharpArch.Domain.Reflection
{
    using FluentAssertions;
    using global::SharpArch.Domain.Reflection;
    using NUnit.Framework;

    [TestFixture]
    public class TypePropertyDescriptorCacheTests
    {
        [SetUp]
        public void SetUp()
        {
            _cache = new TypePropertyDescriptorCache();
        }

        private TypePropertyDescriptorCache _cache;

        [Test]
        public void Clear_Should_ClearTheCache()
        {
            _cache.GetOrAdd(GetType(), () => new TypePropertyDescriptor(GetType(), null));
            _cache.Clear();
            _cache.Find(GetType()).Should().BeNull();
        }

        [Test]
        public void Find_Should_ReturnNullForMissingDescriptor()
        {
            _cache.Find(typeof (TypePropertyDescriptorCache)).Should().BeNull();
        }

        [Test]
        public void GetOrAdd_Should_AddMissingItemToCache()
        {
            var type = GetType();
            var descriptor = new TypePropertyDescriptor(type, null);
            _cache.GetOrAdd(type, () => descriptor).Should().BeSameAs(descriptor);
        }
    }
}
