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
            
            Console.WriteLine($"LinuxTaskStatReader.IsGetPidSupported: {LinuxTaskStatReader.IsGetPidSupported}");
            Console.WriteLine($"LinuxTaskStatReader.IsGetPidSupported: {LinuxTaskStatReader.IsGetPidSupported}");
        }
    }
}