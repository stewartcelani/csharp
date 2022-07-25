using CityInfo.API.Domain;
using CityInfo.API.Mappers;

namespace CityInfo.API.Data;

public static class SeedData
{
    public static async Task SeedDataAsync(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
        if (dbContext.City.Any()) return;
        
        var cities = new List<City>()
        {
            new ()
            {
                Id = new Guid("BC6E490A-7E14-42FD-94EB-7D3282CE889D"),
                Name = "Adelaide",
                Description = "Adelaide is the capital city of South Australia, the state's largest city and the fifth-most populous city of Australia. 'Adelaide' may refer to either Greater Adelaide or the Adelaide city centre. The demonym Adelaidean is used to denote the city and the residents of Adelaide.",
                PointsOfInterest = new List<PointOfInterest>()
                {
                    new ()
                    {
                        Id = new Guid("ACF0EFC6-332C-45E4-B733-21E93733AAFD"),
                        Name = "Botanical Gardens",
                        Description = "The Adelaide Botanic Garden is a 51-hectare public garden at the north-east corner of the Adelaide city centre, in the Adelaide Park Lands. It encompasses a fenced garden on North Terrace and behind it the Botanic Park. Work was begun on the site in 1855, with its official opening to the public on 4 October 1857."
                    },
                    new ()
                    {
                        Id = new Guid("23B26D79-E9E1-4625-BB33-F93DD399C8B2"),
                        Name = "Mount Lofty Summit",
                        Description = "Mount Lofty Summit, the majestic peak of the Mount Lofty Ranges in the Adelaide Hills, provides spectacular panoramic views across Adelaide's city skyline to the coast. Each year more than 350,000 people visit the peak which rises more than 710 metres above sea level."
                    }
                }
            },
            new ()
            {
                Id = new Guid("38DDA0D4-6F38-4041-A0CD-D8DD972DEB9E"),
                Name = "Melbourne",
                Description = "Melbourne is the capital and most-populous city of the Australian state of Victoria, and the second-most populous city in both Australia and Oceania.",
                PointsOfInterest = new List<PointOfInterest>()
                {
                    new ()
                    {
                        Id = new Guid("839A6F32-2F97-4D3E-8F42-8957754BD5D1"),
                        Name = "Melbourne Cricket Ground",
                        Description = "The Melbourne Cricket Ground, also known locally as 'The G', is an Australian sports stadium located in Yarra Park, Melbourne, Victoria. Founded and managed by the Melbourne Cricket Club, it is the largest stadium in the Southern Hemisphere, the 11th largest globally, and the second largest cricket ground by capacity."
                    },
                    new ()
                    {
                        Id = new Guid("D6F7FD77-042E-4F4F-9CE1-72D8845C4EE7"),
                        Name = "Phillip Island",
                        Description = "Phillip Island is an Australian island about 125 km south-southeast of Melbourne, Victoria. The island is named after Governor Arthur Phillip, the first Governor of New South Wales, by explorer and seaman George Bass, who sailed in an whaleboat, arriving from Sydney on 5 January 1798."
                    }
                }
            },
            new ()
            {
                Id = new Guid("11501C0B-D518-4BC3-8546-B813E68DED44"),
                Name = "Sydney",
                Description = "Sydney is the capital city of the state of New South Wales, and the most populous city in both Australia and Oceania. Located on Australia's east coast, the metropolis surrounds Sydney Harbour and extends about 70 km on its periphery towards the Blue Mountains to the west, Hawkesbury to the north, the Royal National Park to the south and Macarthur to the south-west.",
                PointsOfInterest = new List<PointOfInterest>()
                {
                    new ()
                    {
                        Id = new Guid("0B489629-431C-49D7-9BAF-CE7E75E1CF6F"),
                        Name = "Sydney Opera House",
                        Description = "The Sydney Opera House is a multi-venue performing arts centre in Sydney. Located on the banks of Sydney Harbour, it is widely regarded as one of the world's most famous and distinctive buildings and a masterpiece of 20th century architecture."
                    },
                    new ()
                    {
                        Id = new Guid("4AE3FDA7-6806-4EC6-8E10-C3705F25F31D"),
                        Name = "Manly Beach",
                        Description = "Manly Beach is a beach situated among the Northern Beaches of Sydney, Australia, in Manly, New South Wales. From north to south, the three main sections are Queenscliff, North Steyne, and South Steyne."
                    }
                }
            }
        };

        dbContext.City.AddRange(cities.Select(x => x.ToCityEntity()));
        await dbContext.SaveChangesAsync();
    }
   
}