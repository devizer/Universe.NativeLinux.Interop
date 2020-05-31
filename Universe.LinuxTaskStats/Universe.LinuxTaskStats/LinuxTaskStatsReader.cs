using System;
using System.Diagnostics;

namespace Universe.LinuxTaskStats
{
    public class LinuxTaskStatsReader
    {
        public static bool IsGetTidSupported => TaskStatInterop._IsGetTidSupported.Value;
        public static bool IsGetPidSupported => TaskStatInterop._IsGetPidSupported.Value;
        public static bool IsGetTaskStatByProcessSupported => TaskStatInterop._IsGetTaskStatByProcessSupported.Value;
        public static bool IsGetTaskStatByThreadSupported => TaskStatInterop._IsGetTaskStatByThreadSupported.Value;

        public int? GetTaskStatVersion()
        {
            long verRaw = TaskStatInterop.get_taskstats_version();
            int isOk = (int) (verRaw >> 32);
            int ver = (int) (verRaw & 0xFFFFFFFF);
            if (isOk != 0)
            {
                if ((TaskStatInterop.ErrorAction & TaskStatsErrorAction.VerboseOutput) != 0)
                    DebugMessage($"Warning. get_taskstat_version() failed. {new TaskStatInteropException(isOk).Message}");
                
                if ((TaskStatInterop.ErrorAction & TaskStatsErrorAction.ThrowException) != 0)
                    throw new TaskStatInteropException(isOk);
                
                return null;
            }

            return ver;
        }
        
        public static Universe.LinuxTaskStats.LinuxTaskStats? GetByThread(int threadId)
        {
            return Get_Impl(0, threadId);
        }

        public static Universe.LinuxTaskStats.LinuxTaskStats? GetByProcess(int processId)
        {
            return Get_Impl(processId, 0);
        }

        static unsafe Universe.LinuxTaskStats.LinuxTaskStats? Get_Impl(int pid, int tid)
        {
            int size = 640;
            byte* taskStat = stackalloc byte[size];

            var isVerboseOutput = (TaskStatInterop.ErrorAction & TaskStatsErrorAction.VerboseOutput) != 0;
            int isOk = TaskStatInterop.get_taskstats(pid, tid, (IntPtr) taskStat, size, IsDebug || isVerboseOutput? 1 : 0);
            if (isOk != 0)
            {
                if (isVerboseOutput)
                    DebugMessage($"Warning. get_taskstat() failed. {new TaskStatInteropException(isOk).Message}");
                
                if ((TaskStatInterop.ErrorAction & TaskStatsErrorAction.ThrowException) != 0)
                    throw new TaskStatInteropException(isOk);
                
                return null;
            }

            var version = *(short*) taskStat;
            LinuxTaskStats ret = new LinuxTaskStats()
            {
                Version = version,
                Nice = taskStat[9],
                
                // blkio_count, blkio_delay_total
                BlockIoCount = *(long*)(taskStat + 32),
                BlockIoDelay = *(long*)(taskStat + 40),
                
                // swapin_count, swapin_delay_total
                SwapinCount = *(long*)(taskStat + 48),
                SwapinDelay = *(long*)(taskStat + 56),
                
                UserId = *(int*)(taskStat + 120),
                GroupId = *(int*)(taskStat + 124),
                Pid = *(int*)(taskStat + 128),
                ParentPId = *(int*)(taskStat + 132),
                
                BeginTime32 = *(int*)(taskStat + 136),
                ElapsedTime = *(long*)(taskStat + 144),

                // utime, stime
                UserTime = *(long*)(taskStat + 152),
                KernelTime = *(long*)(taskStat + 160),
                
                // minflt, majflt
                MinorPageFaults = *(long*)(taskStat + 168),
                MajorPageFaults = *(long*)(taskStat + 176),

                ReadBytes = *(long*)(taskStat + 216),
                WriteBytes = *(long*)(taskStat + 224),
                ReadSysCalls = *(long*)(taskStat + 232),
                WriteSysCalls = *(long*)(taskStat + 240),
                ReadBlockBackedBytes = *(long*)(taskStat + 248),
                WriteBlockBackedBytes = *(long*)(taskStat + 256),
                
                // nvcsw, nivcsw
                VoluntaryContextSwitches = *(long*)(taskStat + 272),
                InvoluntaryContextSwitches = *(long*)(taskStat + 280),
                
                // ac_utimescaled, ac_stimescaled, cpu_scaled_run_real_total
                UserTimeSmtScaled = *(long*)(taskStat + 288),
                KernelTimeSmtScaled = *(long*)(taskStat + 296),
                RealTimeSmtScaled = *(long*)(taskStat + 304),

                // freepages_count, freepages_delay_total
                MemoryPageReclaimCounter = *(long*)(taskStat + 312),
                MemoryPageReclaimDelay = *(long*)(taskStat + 320),
                
                // thrashing_count, thrashing_delay_total 
                MemoryPageTrashingCounter = version >= 9 ? *(long*)(taskStat + 328) : (long?) null,
                MemoryPageTrashingDelay = version >= 9 ? *(long*)(taskStat + 336) : (long?) null,
                
                // ac_btime64                 
                BeginTime64 = version >= 10 ? *(long*)(taskStat + 344) : (long?) null,
                
                
            };

            return ret;
        }

        [Conditional(("DEBUG"))]
        static void DebugMessage(string message)
        {
#if DEBUG && (NETCOREAPP || NETSTANDARD2_0)
                Console.WriteLine(message);
#endif
            
        }

#if DEBUG
        private const bool IsDebug = true;
#else
        private const bool IsDebug = false;
#endif

    }
}