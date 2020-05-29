using System;
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