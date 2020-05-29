using System;
using System.Diagnostics;

namespace Universe.LinuxTaskstat
{
    public class LinuxTaskStatReader
    {
        public static bool IsGetTidSupported => Interop._IsGetTidSupported.Value;
        public static bool IsGetPidSupported => Interop._IsGetPidSupported.Value;
        public static bool IsGetTaskStatByProcessSupported => Interop._IsGetTaskStatByProcessSupported.Value;
        public static bool IsGetTaskStatByThreadSupported => Interop._IsGetTaskStatByThreadSupported.Value;

        public int? GetTaskStatVersion()
        {
            long verRaw = Interop.get_taskstat_version();
            int isOk = (int) (verRaw >> 32);
            int ver = (int) (verRaw & 0xFFFFFFFF);
            if (isOk != 0)
            {
                DebugMessage($"Warning. get_taskstat_version returned error {isOk}");
                return null;
            }

            return ver;
        }
        
        public static LinuxTaskStat? GetByThread(int threadId)
        {
            return Get_Impl(0, threadId);
        }

        public static LinuxTaskStat? GetByProcess(int processId)
        {
            return Get_Impl(processId, 0);
        }

        static unsafe LinuxTaskStat? Get_Impl(int pid, int tid)
        {
            int size = 640;
            byte* taskStat = stackalloc byte[size];

            int isOk = Interop.get_taskstat(pid, tid, (IntPtr) taskStat, size, IsDebug ? 1 : 0);
            if (isOk != 0)
            {
                DebugMessage($"Warning. get_taskstat returned error {isOk}");
                return null;
            }

            LinuxTaskStat ret = new LinuxTaskStat()
            {
                Version = ((short*) taskStat)[0],
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