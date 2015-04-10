#!/bin/sh
FILES="configure banque/banque-all.sh banque/banque-perf.sh"

for F in $FILES; do
	chmod +x $F
done
