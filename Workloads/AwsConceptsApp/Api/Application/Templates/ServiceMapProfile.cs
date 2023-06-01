using Domain;

namespace Application.Templates
{
    public class ServiceMapProfile : Profile
    {
        public ServiceMapProfile()
        {
            CreateMap<Template, TemplateDto>().ForAllMembers(x => x.AllowNull());
            CreateMap<TemplateDto, Template>().ForAllMembers(x => x.AllowNull());
        }
    }
}
