using System;

namespace Universe.LinuxTaskstat
{
    [Flags]
    public enum TaskStatErrorAction
    {
        ReturnNull = 0,
        ThrowException = 1,
        VerboseOutput = 2,
    }
}