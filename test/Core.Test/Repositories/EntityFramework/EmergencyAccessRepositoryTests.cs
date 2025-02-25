﻿using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using Bit.Core.Entities;
using Bit.Core.Test.AutoFixture.Attributes;
using Bit.Core.Test.AutoFixture.EmergencyAccessFixtures;
using Bit.Core.Test.AutoFixture.Relays;
using Bit.Core.Test.AutoFixture.TransactionFixtures;
using Bit.Core.Test.Repositories.EntityFramework.EqualityComparers;
using Xunit;
using EfRepo = Bit.Infrastructure.EntityFramework.Repositories;
using SqlRepo = Bit.Infrastructure.Dapper.Repositories;

namespace Bit.Core.Test.Repositories.EntityFramework
{
    public class EmergencyAccessRepositoryTests
    {
        [CiSkippedTheory, EfEmergencyAccessAutoData]
        public async void CreateAsync_Works_DataMatches(
            EmergencyAccess emergencyAccess,
            List<User> users,
            EmergencyAccessCompare equalityComparer,
            List<EfRepo.EmergencyAccessRepository> suts,
            List<EfRepo.UserRepository> efUserRepos,
            SqlRepo.EmergencyAccessRepository sqlEmergencyAccessRepo,
            SqlRepo.UserRepository sqlUserRepo
            )
        {
            var savedEmergencyAccesss = new List<EmergencyAccess>();
            foreach (var sut in suts)
            {
                var i = suts.IndexOf(sut);

                for (int j = 0; j < users.Count; j++)
                {
                    users[j] = await efUserRepos[i].CreateAsync(users[j]);
                }
                sut.ClearChangeTracking();

                emergencyAccess.GrantorId = users[0].Id;
                emergencyAccess.GranteeId = users[0].Id;
                var postEfEmergencyAccess = await sut.CreateAsync(emergencyAccess);
                sut.ClearChangeTracking();

                var savedEmergencyAccess = await sut.GetByIdAsync(postEfEmergencyAccess.Id);
                savedEmergencyAccesss.Add(savedEmergencyAccess);
            }

            for (int j = 0; j < users.Count; j++)
            {
                users[j] = await sqlUserRepo.CreateAsync(users[j]);
            }

            emergencyAccess.GrantorId = users[0].Id;
            emergencyAccess.GranteeId = users[0].Id;
            var sqlEmergencyAccess = await sqlEmergencyAccessRepo.CreateAsync(emergencyAccess);
            var savedSqlEmergencyAccess = await sqlEmergencyAccessRepo.GetByIdAsync(sqlEmergencyAccess.Id);
            savedEmergencyAccesss.Add(savedSqlEmergencyAccess);

            var distinctItems = savedEmergencyAccesss.Distinct(equalityComparer);
            Assert.True(!distinctItems.Skip(1).Any());
        }
    }
}
