#!/bin/sh
FILES="configure multilock/multilock-all.sh tests/test-interblocage.sh tests/test-multilock.sh tests/test-lexique.sh"

for F in $FILES; do
	chmod +x $F
done
