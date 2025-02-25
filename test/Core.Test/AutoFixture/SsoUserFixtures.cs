﻿using AutoFixture;
using Bit.Core.Entities;
using Bit.Core.Test.AutoFixture.EntityFrameworkRepositoryFixtures;
using Bit.Core.Test.AutoFixture.OrganizationFixtures;
using Bit.Core.Test.AutoFixture.UserFixtures;
using Bit.Infrastructure.EntityFramework.Repositories;
using Bit.Test.Common.AutoFixture;
using Bit.Test.Common.AutoFixture.Attributes;

namespace Bit.Core.Test.AutoFixture.SsoUserFixtures
{
    internal class EfSsoUser : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualMembersCustomization());
            fixture.Customizations.Add(new GlobalSettingsBuilder());
            fixture.Customizations.Add(new UserBuilder());
            fixture.Customizations.Add(new OrganizationBuilder());
            fixture.Customize<SsoUser>(composer => composer.Without(ou => ou.Id));
            fixture.Customizations.Add(new EfRepositoryListBuilder<SsoUserRepository>());
            fixture.Customizations.Add(new EfRepositoryListBuilder<UserRepository>());
            fixture.Customizations.Add(new EfRepositoryListBuilder<OrganizationRepository>());
        }
    }

    internal class EfSsoUserAutoDataAttribute : CustomAutoDataAttribute
    {
        public EfSsoUserAutoDataAttribute() : base(new SutProviderCustomization(), new EfSsoUser())
        { }
    }

    internal class InlineEfSsoUserAutoDataAttribute : InlineCustomAutoDataAttribute
    {
        public InlineEfSsoUserAutoDataAttribute(params object[] values) : base(new[] { typeof(SutProviderCustomization),
            typeof(EfSsoUser) }, values)
        { }
    }
}
