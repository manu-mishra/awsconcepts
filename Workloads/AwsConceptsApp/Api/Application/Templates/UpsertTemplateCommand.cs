using Domain;

namespace Application.Templates
{
    public class UpsertTemplateCommand : IRequest<TemplateDto>
    {

        public UpsertTemplateCommand(TemplateDto dto)
        {
            this.Dto = dto;
        }
        public TemplateDto Dto { get; }
    }
    public class UpsertTemplateCommandHandler
        : IRequestHandler<UpsertTemplateCommand, TemplateDto>
    {
        private readonly IEntityRepository<Template> repository;
        private readonly IMapper mapper;
        private readonly IIdentity identity;

        public UpsertTemplateCommandHandler(
            IEntityRepository<Template> Repository,
            IMapper Mapper,
            IIdentity Identity)
        {
            repository = Repository;
            mapper = Mapper;
            identity = Identity;
        }
        public async Task<TemplateDto> Handle(UpsertTemplateCommand request, CancellationToken cancellationToken)
        {
            Template entity = mapper.Map<Template>(request.Dto);
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id ??= Guid.NewGuid().ToString();
                await repository.Create(entity, cancellationToken);
            }
            else
                await repository.Update(entity, cancellationToken);
            return mapper.Map<TemplateDto>(entity);
        }
    }
}
