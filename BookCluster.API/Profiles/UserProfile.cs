using AutoMapper;

namespace BookCluster.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Domain.Entities.User, Models.User>();
            CreateMap<Models.User, Domain.Entities.User>();
        }
    }
}
