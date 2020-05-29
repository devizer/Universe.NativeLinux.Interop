using System;

namespace Universe.LinuxTaskstat
{
    public class LinuxTaskStatReader
    {
        public static bool IsGetTidSupported => Interop._IsGetTidSupported.Value;
        public static bool IsGetPidSupported => Interop._IsGetPidSupported.Value;
        public static bool IsGetTaskStatByProcessSupported => Interop._IsGetTaskStatByProcessSupported.Value;
        public static bool IsGetTaskStatByThreadSupported => Interop._IsGetTaskStatByThreadSupported.Value;

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

            int isOk = Interop.get_taskstat(pid, tid, (IntPtr) taskStat, size, 0);
            if (isOk != 0)
            {
#if DEBUG && NETCOREAPP
                Console.WriteLine($"Warning. get_taskstat returned error {isOk}");
#endif
                return null;
            }

            LinuxTaskStat ret = new LinuxTaskStat()
            {
                Version = ((short*) taskStat)[0],
            };

            return ret;
        }

    }
}