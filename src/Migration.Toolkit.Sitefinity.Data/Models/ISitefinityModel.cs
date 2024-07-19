namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model to ensure that all Sitefinity models have an Id property.
/// </summary>
public interface ISitefinityModel
{
    Guid Id { get; }
}
