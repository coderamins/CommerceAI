using System.Text.RegularExpressions;

namespace CommerceAI.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required.");

        var normalized = value.Trim().ToLowerInvariant();

        if (!Regex.IsMatch(normalized,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email.");

        return new Email(normalized);
    }

    public bool Equals(Email? other)
        => other is not null && Value == other.Value;

    public override bool Equals(object? obj)
        => Equals(obj as Email);

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value;

    public static implicit operator string(Email email)
        => email.Value;
}