using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnderstandingDependencies.Api.Models;
using UnderstandingDependencies.Api.Repositories;

namespace UnderstandingDependencies.Api.Tests.Unit;

public class FakeUserRepository : IUserRepository
{
    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult((Enumerable.Empty<User>()));
    }
}