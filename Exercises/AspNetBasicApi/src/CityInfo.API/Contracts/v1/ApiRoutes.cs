using System.Runtime.CompilerServices;

namespace CityInfo.API.Contracts.v1;

public static class ApiRoutesV1
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = $"{Root}/{Version}";

    public static class Cities
    {
        public static class GetAll
        {
            public const string Url = Base + "/cities";
        }

        public static class Get
        {
            public const string Url = Base + "/cities/{cityId:guid}";
            public static string UrlFor(Guid cityId) => Url.ReplaceCityId(cityId);
        }

        public static class Create
        {
            public const string Url = Base + "/cities";
        }
        
        public static class Update
        {
            public const string Url = Base + "/cities/{cityId:guid}";
            public static string UrlFor(Guid cityId) => Url.ReplaceCityId(cityId);
        }

        public static class Delete
        {
            public const string Url = Base + "/cities/{cityId:guid}";
            public static string UrlFor(Guid cityId) => Url.ReplaceCityId(cityId);
        }
    }
    public static class PointsOfInterest
    {
        public static class GetAll
        {
            public const string Url = Base + "/cities/{cityId:guid}/pointsofinterest";
            public static string UrlFor(Guid cityId) => Url.ReplaceCityId(cityId);
        }

        public static class Get
        {
            public const string Url = Base + "/cities/{cityId:guid}/pointsofinterest/{pointOfInterestId:guid}";
            public static string UrlFor(Guid cityId, Guid pointOfInterestId) =>
                Url.ReplaceCityId(cityId).ReplacePointOfInterestId(pointOfInterestId);
        }

        public static class Create
        {
            public const string Url = Base + "/cities/{cityId:guid}/pointsofinterest";
            public static string UrlFor(Guid cityId) => Url.ReplaceCityId(cityId);
        }

        public static class Update
        {
            public const string Url = Base + "/cities/{cityId:guid}/pointsofinterest/{pointOfInterestId:guid}";
            public static string UrlFor(Guid cityId, Guid pointOfInterestId) =>
                Url.ReplaceCityId(cityId).ReplacePointOfInterestId(pointOfInterestId);        }

        public static class Delete
        {
            public const string Url = Base + "/cities/{cityId:guid}/pointsofinterest/{pointOfInterestId:guid}";
            public static string UrlFor(Guid cityId, Guid pointOfInterestId) =>
                Url.ReplaceCityId(cityId).ReplacePointOfInterestId(pointOfInterestId);
        }
        
    }

    private static string ReplaceCityId(this string s, Guid cityId) => s.Replace("{cityId:guid}", cityId.ToString());
    private static string ReplacePointOfInterestId(this string s, Guid pointOfInterestId) => s.Replace("{pointOfInterestId:guid}", pointOfInterestId.ToString());
}