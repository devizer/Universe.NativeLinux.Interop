using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Universe.CpuUsage;
using Universe.CpuUsage.Tests;

namespace Universe.LinuxTaskStats.Tests
{
    public class LinuxTaskStat_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Pid()
        {
            Console.WriteLine($"Process.GetCurrentProcess().Id: {Process.GetCurrentProcess().Id}");
            Console.WriteLine($"Interop.get_pid(): {TaskStatInterop.get_pid()}");
            Assert.AreEqual(Process.GetCurrentProcess().Id, TaskStatInterop.get_pid());
        }

        [Test]
        public void Test_Tid()
        {
            var tids = Process.GetCurrentProcess().Threads
                .OfType<ProcessThread>()
                .Select(x => x.Id)
                .OrderBy(x => x)
                .ToArray();
            
            Console.WriteLine($"Process.GetCurrentProcess().Threads: {string.Join(", ", tids)}");
            Console.WriteLine($"Interop.get_tid(): {TaskStatInterop.get_tid()}");
            CollectionAssert.Contains(tids, TaskStatInterop.get_tid());
        }

        [Test]
        public void IsSupported()
        {
            Console.WriteLine($"LinuxTaskStatReader.IsGetPidSupported: {LinuxTaskStatsReader.IsGetPidSupported}");
            Console.WriteLine($"LinuxTaskStatReader.IsGetTidSupported: {LinuxTaskStatsReader.IsGetTidSupported}");
            
            Console.WriteLine($"LinuxTaskStatReader.IsGetTaskStatByProcessSupported: {LinuxTaskStatsReader.IsGetTaskStatByProcessSupported}");
            Console.WriteLine($"LinuxTaskStatReader.IsGetTaskStatByThreadSupported: {LinuxTaskStatsReader.IsGetTaskStatByThreadSupported}");
        }

        [Test]
        public void Show_Taskstat_Version_Raw()
        {
            Console.WriteLine($"Version: {TaskStatInterop.get_taskstats_version():X16}");
        }
        
        [Test]
        public void Show_Taskstat_PerProcess()
        {
            var stat = LinuxTaskStatsReader.GetByProcess(TaskStatInterop.get_pid());
            Console.WriteLine($"TaskStat HAS VALUE: {stat.HasValue}");
            Assert.IsNotNull(stat, "TaskStat is expected. Probably permissions are missing");
            Console.WriteLine($"VER: {stat.Value.Version}");
        }

        [Test]
        public void Show_Taskstat_PerThread()
        {
            var stat = LinuxTaskStatsReader.GetByProcess(TaskStatInterop.get_pid());
            Console.WriteLine($"TaskStat HAS VALUE: {stat.HasValue}");
            Assert.IsNotNull(stat, "TaskStat is expected. Probably permissions are missing");
            Console.WriteLine($"VER: {stat.Value.Version}");
        }

        [Test]
        public void Show_Pairs_For_All_Processes()
        {
            List<LinuxTaskStats> pairs = new List<LinuxTaskStats>();
            var processes = Process.GetProcesses();
            foreach (var p in processes)
            {
                var stat1 = LinuxTaskStatsReader.GetByProcess(p.Id);
                var stat2 = LinuxTaskStatsReader.GetByThread(p.Id);
                if (stat1.HasValue) pairs.Add(stat1.Value);
                if (stat2.HasValue) pairs.Add(stat2.Value);
            }
            
            IEnumerable<LinuxTaskStats?> allProcesses = Process.GetProcesses().Select(x => LinuxTaskStatsReader.GetByProcess(x.Id));
            Console.WriteLine($"ALL PAIRS. My PID is {Process.GetCurrentProcess().Id}");
            Console.WriteLine(pairs.ToDebugString());
        }

        [Test]
        public void Show_All_Processes()
        {
            IEnumerable<LinuxTaskStats?> allProcesses = Process.GetProcesses().Select(x => LinuxTaskStatsReader.GetByProcess(x.Id));
            Console.WriteLine($"ALL PROCESSES. My PID is {Process.GetCurrentProcess().Id}");
            Console.WriteLine(allProcesses.ToDebugString());
        }
        
        [Test]
        public void Show_All_Threads_Of_Current_Process()
        {
            CpuLoader.Run(1, 3000, true);
            IoLoader.IO_Reads_Test(CpuUsageScope.Thread);
            IoLoader.IO_Write_Test(CpuUsageScope.Thread);
            IoLoader.TearDown_IO_Metrics();
            
            var tids = Process.GetCurrentProcess().Threads
                .OfType<ProcessThread>()
                .Select(x => x.Id)
                .OrderBy(x => x)
                .ToArray();
            
            DumpThreadsByProcess(Process.GetCurrentProcess().Id);
        }

        [Test]
        public void Show_All_Threads_Of_ALL_Process()
        {
            Directory.CreateDirectory("bin");
            foreach (var p in Process.GetProcesses())
            {
                File.WriteAllText($"bin/pid-{p.Id}", DumpThreadsByProcess(p.Id), Encoding.ASCII);
            }
            
        }

        private static string DumpThreadsByProcess(int processId)
        {
            StringBuilder ret = new StringBuilder();

            var processName = Process.GetProcessById(processId).ProcessName;
            ret.AppendLine($"Process #{processId} '{processName}'");
            var tids = GetTasksByPid(processId); 

            var allThreads = tids.Select(LinuxTaskStatsReader.GetByThread).ToArray();
            ret.AppendLine($"ALL THREADS Of (pid={processId}) PROCESS: {string.Join(", ", tids)}");
            ret.AppendLine(allThreads.ToDebugString() + Environment.NewLine);
            return ret.ToString();
        }

        static int[] GetTasksByPid(int pid)
        {
            try
            {
                var dirs = new DirectoryInfo($"/proc/{pid}/task").GetDirectories();
                return dirs
                    .Select(x => x.Name)
                    .Where(x => int.TryParse(x, out var tmp))
                    .Select(x => Int32.Parse(x))
                    .ToArray();
            }
            catch
            {
                return new int[0];
            }
        }
    }
}