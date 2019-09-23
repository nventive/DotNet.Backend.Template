﻿using Xunit;

namespace NV.Templates.Backend.Web.Tests
{
    [CollectionDefinition(CollectionName)]
    public class TestWebApplicationFactoryCollection : ICollectionFixture<TestWebApplicationFactory>
    {
        public const string CollectionName = nameof(TestWebApplicationFactoryCollection);
    }
}
