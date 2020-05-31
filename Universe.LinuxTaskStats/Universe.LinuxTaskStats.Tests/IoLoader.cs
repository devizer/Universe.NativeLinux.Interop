using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using KernelManagementJam.Benchmarks;
using NUnit.Framework;

namespace Universe.CpuUsage.Tests
{
    [TestFixture]
    public class IoLoader 
    {
        private static string FileName = "IO-Metrics-Tests-" + Guid.NewGuid().ToString() + ".tmp";

        // Works at home PC and AppVeyor linux,
        // Does not work on multi arch docker container on travis-ci 
        private static bool SkipPosixResourcesUsageAsserts => 
            Environment.GetEnvironmentVariable("SKIP_POSIXRESOURCESUSAGE_ASSERTS") == "True";

        private static void WriteFile(int size)
        {
            Random rnd = new Random(42);
            byte[] content = new byte[size];
            rnd.NextBytes(content);
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 128, FileOptions.WriteThrough))
            {
                fs.Write(content, 0, content.Length);
            }
        }

        private static void ReadFile()
        {
            var bytes = File.ReadAllBytes(FileName);
        }

        [TearDown]
        public static void TearDown_IO_Metrics()
        {
            if (File.Exists(FileName)) File.Delete(FileName);
        }
        

        // Does not work on drone.io in container
        public static void IO_Reads_Test(CpuUsageScope scope)
        {
            if (!PosixResourceUsage.IsSupported) return;
            if (scope == CpuUsageScope.Thread && CrossInfo.ThePlatform != CrossInfo.Platform.Linux) return;

            // Arrange
            var numBytes = 100*1024*1024;
            WriteFile(numBytes);
            LinuxKernelCacheFlusher.Sync();
            
            // Act
            PosixResourceUsage before = PosixResourceUsage.GetByScope(scope).Value;
            ReadFile();
            PosixResourceUsage after = PosixResourceUsage.GetByScope(scope).Value;
            var delta = PosixResourceUsage.Substruct(after, before);
            Console.WriteLine($"Operation: Read {numBytes:n0} bytes. ReadOps = {delta.ReadOps}. WriteOps = {delta.WriteOps}");
            
        }
 
        /*
        [Test]
        [TestCase(CpuUsageScope.Thread)]
        [TestCase(CpuUsageScope.Process)]
        */
        public static void IO_Write_Test(CpuUsageScope scope)
        {
            if (!PosixResourceUsage.IsSupported) return;
            if (scope == CpuUsageScope.Thread && CrossInfo.ThePlatform != CrossInfo.Platform.Linux) return;

            // Arrange: nothing to do
            
            // Act
            PosixResourceUsage before = PosixResourceUsage.GetByScope(scope).Value;
            var numBytes = 100*1024*1024;
            WriteFile(numBytes);
            PosixResourceUsage after = PosixResourceUsage.GetByScope(scope).Value;
            var delta = PosixResourceUsage.Substruct(after, before);
            Console.WriteLine($"Operation: write {numBytes:n0} bytes. ReadOps = {delta.ReadOps}. WriteOps = {delta.WriteOps}");
            
        }

        static bool IsIoReadsWritesSupported()
        {
            try
            {
                return File.Exists("/proc/self/io");
            }
            catch
            {
                // termux on non-rooted android phone. May be supported but we cant know
                return false;
            }
        }
    }
}