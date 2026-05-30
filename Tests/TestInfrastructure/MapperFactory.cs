using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Services.Mapping;
using Web.Mapping;

namespace Tests.TestInfrastructure
{
    internal static class MapperFactory
    {
        public static IMapper Create()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BuildingProfile>();
                cfg.AddProfile<EquipmentProfile>();
                cfg.AddProfile<RoomProfile>();
                cfg.AddProfile<RoomEquipmentProfile>();
                cfg.AddProfile<EventTypeProfile>();
                cfg.AddProfile<EventProfile>();
                cfg.AddProfile<ReservationProfile>();
                cfg.AddProfile<BuildingViewModelProfile>();
            }, NullLoggerFactory.Instance);

            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }
    }
}
