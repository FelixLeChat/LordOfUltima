#!/bin/sh

REPEAT=100000000
AMOUNT=10000000000
LIBS="serial fork pthread pth"
EVENTS="page-faults,context-switches,L1-dcache-load-misses"
ADDITIONAL_COLUMNS="lib,user time,system time,elapsed time"
CSV_FILE="perf.csv"

# Add events header to CSV file (overwrite if existing)
echo "$ADDITIONAL_COLUMNS,$EVENTS" > $CSV_FILE

# For each execution, append event counters to CSV file
echo "Calculating statistics for the following events:"
echo $EVENTS
for lib in $LIBS; do
	CMD="./banque --lib $lib --repeat $REPEAT --amount $AMOUNT"
	echo $CMD
    # The awk script takes all the event counters and prints them on a single line, comma-seperated
    PERF_RESULTS=$(perf stat -e $EVENTS -x , $CMD 2>&1 >/dev/null | awk 'BEGIN{FS=","} /^[^#]/{s = s $1","} END{print substr(s, 0, length(s)-1)}')
    TIME_RESULTS=$(/usr/bin/time -f "%U,%S,%E" $CMD 2>&1 >/dev/null)
    echo "$lib,$TIME_RESULTS,$PERF_RESULTS" >> $CSV_FILE
done
echo "Results were written in $CSV_FILE"
