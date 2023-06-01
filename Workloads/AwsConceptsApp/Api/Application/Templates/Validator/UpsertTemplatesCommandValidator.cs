namespace Application.Templates.Validator
{
    internal class UpsertTemplatesCommandValidator : AbstractValidator<UpsertTemplateCommand>
    {
        public UpsertTemplatesCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull();
        }
    }
}
