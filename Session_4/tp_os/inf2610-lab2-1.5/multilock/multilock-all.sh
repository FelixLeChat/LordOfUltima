#!/bin/sh

./multilock --check --outer 1000 --inner 100 --verbose
./multilock --check --outer 10  --inner 10000 --verbose
