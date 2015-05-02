bin_PROGRAMS = multilock

multilock_SOURCES = multilock.c multilock.h
multilock_LDADD = libmultilock.a -lm

noinst_LIBRARIES = libmultilock.a

libmultilock_a_SOURCES = minispinlock.asm minispinlock.h \
	spinlock.c spinlock.h \
	serial.c serial.h \
	semrelay.c semrelay.h \
	mutex.c mutex.h \
	statistics.c statistics.h \
	utils.c utils.h

.asm.o:
	$(NASM) $(NAFLAGS) -g -o $@ $<

EXTRA_DIST = multilock-all.sh
