using System;

namespace Universe.LinuxTaskStats
{
    [Flags]
    public enum TaskStatsErrorAction
    {
        ReturnNull = 0,
        ThrowException = 1,
        VerboseOutput = 2,
    }
}