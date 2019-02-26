using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities.Extensions
{
    public static class IModifiedEntityExtensions
    {
        public static bool IsObjectNull(this IModifiedEntity entity)
        {
            return entity == null;
        }

        public static bool IsEmptyObject(this IModifiedEntity entity)
        {
            return entity.Id == 0;
        }
    }
}