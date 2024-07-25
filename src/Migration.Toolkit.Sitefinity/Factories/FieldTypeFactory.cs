using System.Reflection;

using Migration.Toolkit.Sitefinity.Core;
using Migration.Toolkit.Sitefinity.Core.Factories;
using Migration.Toolkit.Sitefinity.FieldTypes;

namespace Migration.Toolkit.Sitefinity.Factories;
internal class FieldTypeFactory : IFieldTypeFactory
{
    private readonly List<IFieldType> fieldTypes = [];

    public FieldTypeFactory(IServiceProvider serviceProvider)
    {
        foreach (var fieldType in GetTypes())
        {
            if (serviceProvider.GetService(fieldType) is not IFieldType createdFieldType)
            {
                continue;
            }

            fieldTypes.Add(createdFieldType);
        }
    }

    public IFieldType CreateFieldType(string? fieldType)
    {
        if (fieldType == null)
        {
            return new TextFieldType();
        }

        var foundFieldType = fieldTypes.Find(x => x.SitefinityWidgetTypeName.Equals(fieldType));

        if (foundFieldType == null)
        {
            return new TextFieldType();
        }

        return foundFieldType;
    }

    public static IEnumerable<Type> GetTypes()
    {
        var assemblies = Assembly.Load("Migration.Toolkit.Sitefinity");

        return assemblies.GetExportedTypes().Where(y => y.IsClass && !y.IsAbstract && !y.IsGenericType && !y.IsNested && typeof(IFieldType).IsAssignableFrom(y));

    }
}
