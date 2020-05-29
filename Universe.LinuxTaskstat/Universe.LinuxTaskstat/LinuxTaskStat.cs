namespace Universe.LinuxTaskstat
{
    public struct LinuxTaskStat
    {
        public short Version { get; set; }
        
        public short Nice { get; set; }
        
        public long BlockIoCount { get; set; }
        public long BlockIoDelay { get; set; }
        
        public long SwapinCount { get; set; }
        public long SwapinDelay { get; set; }
        
        public long UserTime { get; set; }
        public long KernelTime { get; set; }
        
        public long MinorPageFaults { get; set; }
        public long MajorPageFaults { get; set; }
        
        // .../io
        public long ReadBytes { get; set; }
        public long WriteBytes { get; set; }
        public long ReadSysCalls { get; set; }
        public long WriteSysCalls { get; set; }
        
        // TASKSTATS_HAS_IO_ACCOUNTING defined
        public long ReadBlockBackedBytes { get; set; }
        public long WriteBlockBackedBytes { get; set; }
        
        public long VoluntaryContextSwitches { get; set; }
        public long InvoluntaryContextSwitches { get; set; }
        
        public long UserTimeSmtScaled { get; set; }
        public long KernelTimeSmtScaled { get; set; }
        public long RealTimeSmtScaled { get; set; }
        
        public long MemoryPageReclaimCounter { get; set; }
        public long MemoryPageReclaimDelay { get; set; }
        // End V7
        
        public long? MemoryPageTrashingCounter { get; set; }
        public long? MemoryPageTrashingDelay { get; set; }
        // End V9
        
        /* v10: 64-bit btime to avoid overflow */
        public long? BeginTime { get; set; }
        
    }
}