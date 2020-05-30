using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Universe.LinuxTaskStats;

namespace Universe.LinuxTaskStats
{
    public static class TaskStatInterop
    {
        public static TaskStatsErrorAction ErrorAction { get; } = TaskStatsErrorAction.ReturnNull;
        
        private const int TASKSTAT_ENOUGH_SIZE = 1024;
        private const string LibName = "libNativeLinuxInterop";

        [DllImport(LibName)]
        public static extern int get_tid();

        [DllImport(LibName)]
        public static extern int get_pid();

        [DllImport(LibName)]
        public static extern long get_taskstat_version();
        
        [DllImport(LibName)]
        public static extern int get_taskstat(int pid, int tid, IntPtr taskStat, int taskStatSize, int debug);

        internal static Lazy<bool> _IsGetTidSupported = new Lazy<bool>(() =>
        {
            return IsSuccess("syscall(__NR_gettid)", () => get_tid()); 
        });

        internal static Lazy<bool> _IsGetPidSupported = new Lazy<bool>(() =>
        {
            return IsSuccess("getpid()", () => get_pid()); 
        });

        internal static unsafe Lazy<bool> _IsGetTaskStatByProcessSupported = new Lazy<bool>(() =>
        {
            if (!_IsGetPidSupported.Value) return false;
            byte* taskStat = stackalloc byte[TASKSTAT_ENOUGH_SIZE];
            return IsSuccess("get_taskstat(pid)", () => get_taskstat(get_pid(), 0, (IntPtr) taskStat, TASKSTAT_ENOUGH_SIZE, 0)); 
        });

        internal static unsafe Lazy<bool> _IsGetTaskStatByThreadSupported = new Lazy<bool>(() =>
        {
            if (!_IsGetTidSupported.Value) return false;
            byte* taskStat = stackalloc byte[TASKSTAT_ENOUGH_SIZE];
            return IsSuccess("get_taskstat(tid)", () => get_taskstat(0, get_tid(), (IntPtr) taskStat, TASKSTAT_ENOUGH_SIZE, 0)); 
        });

        private static bool IsSuccess(string caption, Action toTry)
        {
            return IsSuccess(caption, () =>
            {
                toTry();
                return true;
            });
        }

        private static bool IsSuccess(string caption, Func<bool> toTry)
        {
            try
            {
                return toTry();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Check failed for '{caption}'. {ex.GetType()}: {ex.Message}");
                return false;
            }
        }
    
    }
}