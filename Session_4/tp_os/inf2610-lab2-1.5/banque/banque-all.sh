#!/bin/sh

REPEAT=100000000
AMOUNT=10000000000
LIBS="serial fork pthread pth"
for lib in $LIBS; do
	CMD="./banque --lib $lib --repeat $REPEAT --amount $AMOUNT"
	echo $CMD
	$CMD
done
