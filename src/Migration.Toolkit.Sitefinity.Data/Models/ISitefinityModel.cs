namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model to ensure that all Sitefinity models have an Id property.
/// </summary>
public interface ISitefinityModel
{
    /// <summary>
    /// The unique identifier of the model.
    /// </summary>
    Guid Id { get; }
}
