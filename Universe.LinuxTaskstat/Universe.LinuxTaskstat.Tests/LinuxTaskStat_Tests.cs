using System;
using System.Linq;
using System.Diagnostics;
using NUnit.Framework;

namespace Universe.LinuxTaskstat.Tests
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
            Console.WriteLine($"Interop.get_pid(): {Interop.get_pid()}");
            Assert.AreEqual(Process.GetCurrentProcess().Id, Interop.get_pid());
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
            Console.WriteLine($"Interop.get_tid(): {Interop.get_tid()}");
            CollectionAssert.Contains(tids, Interop.get_tid());
        }

        [Test]
        public void IsSupported()
        {
            Console.WriteLine($"LinuxTaskStatReader.IsGetPidSupported: {LinuxTaskStatReader.IsGetPidSupported}");
            Console.WriteLine($"LinuxTaskStatReader.IsGetTidSupported: {LinuxTaskStatReader.IsGetTidSupported}");
            
            Console.WriteLine($"LinuxTaskStatReader.IsGetTaskStatByProcessSupported: {LinuxTaskStatReader.IsGetTaskStatByProcessSupported}");
            Console.WriteLine($"LinuxTaskStatReader.IsGetTaskStatByThreadSupported: {LinuxTaskStatReader.IsGetTaskStatByThreadSupported}");
        }

        [Test]
        public void Show_Taskstat_Version_Raw()
        {
            Console.WriteLine($"Version: {Interop.get_taskstat_version():X16}");
        }
        
        [Test]
        public void Show_Taskstat_PerProcess()
        {
            var stat = LinuxTaskStatReader.GetByProcess(Interop.get_pid());
            Console.WriteLine($"TaskStat HAS VALUE: {stat.HasValue}");
            Assert.IsNotNull(stat, "TaskStat is expected. Probably permissions are missing");
            Console.WriteLine($"VER: {stat.Value.Version}");
        }

        [Test]
        public void Show_Taskstat_PerThread()
        {
            var stat = LinuxTaskStatReader.GetByProcess(Interop.get_pid());
            Console.WriteLine($"TaskStat HAS VALUE: {stat.HasValue}");
            Assert.IsNotNull(stat, "TaskStat is expected. Probably permissions are missing");
            Console.WriteLine($"VER: {stat.Value.Version}");
        }

    }
}