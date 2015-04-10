bin_PROGRAMS = randaddr dummy

AM_CFLAGS = -fno-omit-frame-pointer -rdynamic -fno-optimize-sibling-calls

randaddr_SOURCES = randaddr.c

dummy_SOURCES = dummy.c
dummy_LDADD = libdummy.la -ldl

lib_LTLIBRARIES = libdummy.la
libdummy_la_SOURCES = libdummy.c libdummy.h
libdummy_la_LIBADD = -ldl
