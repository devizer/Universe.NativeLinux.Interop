using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

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
            Console.WriteLine($"Version: {TaskStatInterop.get_taskstat_version():X16}");
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

    }
}