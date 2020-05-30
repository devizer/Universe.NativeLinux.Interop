using System;

namespace Universe.LinuxTaskstats
{
    [Flags]
    public enum TaskStatsErrorAction
    {
        ReturnNull = 0,
        ThrowException = 1,
        VerboseOutput = 2,
    }
}