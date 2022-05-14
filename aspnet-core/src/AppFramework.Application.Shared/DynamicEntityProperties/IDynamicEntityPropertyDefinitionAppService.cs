using System.Collections.Generic;

namespace AppFramework.DynamicEntityProperties
{
    public interface IDynamicEntityPropertyDefinitionAppService
    {
        List<string> GetAllAllowedInputTypeNames();

        List<string> GetAllEntities();
    }
}