using CommerceAI.Domain.Common;
using CommerceAI.Domain.ValueObjects;

namespace CommerceAI.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; private set; }

    public Email Email { get; private set; }

    public string PasswordHash { get; private set; }

    private User() { }

    public User(
        string fullName,
        Email email,
        string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name required.");

        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void ChangeEmail(Email email)
    {
        Email = email;
        Update();
    }
}
