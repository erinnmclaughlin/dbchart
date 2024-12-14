namespace DbChart;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RelationshipDescriptionAttribute(string description) : Attribute
{
    public string Description { get; } = description;
}