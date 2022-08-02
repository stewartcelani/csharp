using CityInfo.API.Contracts.Requests.Queries;

namespace CityInfo.API.Services;

public interface IUriService
{
    Uri GetCitiesUri(PaginationQuery? paginationQuery = null);
}