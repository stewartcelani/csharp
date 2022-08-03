using CityInfo.API.Contracts.v1.Requests.Queries;

namespace CityInfo.API.Services;

public interface IUriService
{
    Uri GetCitiesUri(PaginationQuery? paginationQuery = null);
}