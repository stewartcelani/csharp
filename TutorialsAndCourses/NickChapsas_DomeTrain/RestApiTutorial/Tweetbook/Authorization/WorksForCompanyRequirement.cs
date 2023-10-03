using Microsoft.AspNetCore.Authorization;

namespace Tweetbook.Authorization;

public class WorksForCompanyRequirement : IAuthorizationRequirement
{
    public WorksForCompanyRequirement(string domainName)
    {
        DomainName = domainName;
    }

    public string DomainName { get; }
}