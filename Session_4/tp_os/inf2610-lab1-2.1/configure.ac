AC_INIT([INF2610-LAB1], 2.1)
AC_CONFIG_SRCDIR([banque/banque.c])
AC_CONFIG_MACRO_DIR([m4])
AM_CONFIG_HEADER([config.h])
AM_INIT_AUTOMAKE([])

LT_INIT

AC_CHECK_HEADERS(sys/types.h unistd.h fcntl.h strings.h pthread.h time.h errno.h stdarg.h limits.h signal.h stdlib.h)
AC_CHECK_HEADERS(inttypes.h math.h)
AC_CHECK_LIB(pthread, pthread_create)
AC_CHECK_PTH(1.2.0)
PKG_CHECK_MODULES(GLIB, [glib-2.0])

# be silent by default
AM_SILENT_RULES([yes])

CFLAGS="-Wall -g -O0 -fno-inline"

AC_PROG_LIBTOOL
AC_PROG_CC
AC_PROG_CXX
AM_PROG_CC_C_O
AC_PROG_RANLIB
AC_CONFIG_FILES([Makefile
    chaine/Makefile
    banque/Makefile])
AC_OUTPUT
