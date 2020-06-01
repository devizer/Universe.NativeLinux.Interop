pid=1440
tasks=$(ls -1 /proc/$pid/task)
for task in $tasks; do
  t=$(basename $task)
  sudo ./delays -t $t -d
  echo $t
done
