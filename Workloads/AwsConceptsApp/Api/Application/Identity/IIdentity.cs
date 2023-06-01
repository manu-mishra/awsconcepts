namespace Application.Identity
{
    public interface IIdentity
    {
        string Id { get; }
        string GivenName { get; }
        string Surname { get; }
        string NickName { get; }
        string Email { get; }
        bool Verified { get; }
    }
}
