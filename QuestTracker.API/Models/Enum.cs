using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Models
{
    public enum ApplicationType
    {
        JavaScript = 0,
        NativeConfidential = 1
    }

    public enum SmartListChoice
    {
        Auto = 0,
        Visible = 1,
        Hidden = 2
    }

    public enum TimeDelayType
    {
        Day = 0,
        Week = 1,
        Month = 2,
        Year = 3
    }
}