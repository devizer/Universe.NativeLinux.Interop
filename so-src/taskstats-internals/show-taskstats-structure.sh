
fields="
version
ac_exitcode
ac_flag
ac_nice
cpu_count
cpu_delay_total
blkio_count
blkio_delay_total
swapin_count
swapin_delay_total
cpu_run_real_total
cpu_run_virtual_total
ac_comm[TS_COMM_LEN]
ac_sched
ac_pad[3]
ac_uid
ac_gid
ac_pid
ac_ppid
ac_btime
ac_etime
ac_utime
ac_stime
ac_minflt
ac_majflt
coremem
virtmem
hiwater_rss
hiwater_vm
read_char
write_char
read_syscalls
write_syscalls
read_bytes
write_bytes
cancelled_write_bytes
nvcsw
nivcsw
ac_utimescaled
ac_stimescaled
cpu_scaled_run_real_total
freepages_count
freepages_delay_total
thrashing_count
thrashing_delay_total
ac_btime64
"

for TASKSTATS_FIELD_NAME in $fields; do
  export TASKSTATS_FIELD_NAME
  envsubst < show-taskstats-offsetof.c > /tmp/field.c
  if gcc -O0 -o /tmp/field.out /tmp/field.c 2>/dev/null; then
    /tmp/field.out
  else
    echo "                         error: $TASKSTATS_FIELD_NAME"
  fi

  # rm -f /tmp/field.*
done