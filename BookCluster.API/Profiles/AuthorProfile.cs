using AutoMapper;

namespace BookCluster.API.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Domain.Entities.Author, Models.Author>();
            CreateMap<Models.Author, Domain.Entities.Author>();
        }
    }
}
