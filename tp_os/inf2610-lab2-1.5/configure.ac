AC_INIT([INF2610-LAB2], 1.4)
AC_CONFIG_SRCDIR([multilock/multilock.c])
AC_CONFIG_MACRO_DIR([m4])
AM_CONFIG_HEADER([config.h])
AM_INIT_AUTOMAKE([])

LT_INIT

AC_CHECK_HEADERS(sys/types.h unistd.h fcntl.h strings.h pthread.h time.h errno.h stdarg.h limits.h signal.h stdlib.h)
AC_CHECK_HEADERS(inttypes.h math.h)
AC_CHECK_LIB(pthread, pthread_create)
PKG_CHECK_MODULES(GLIB, [glib-2.0])

# be silent by default
AM_SILENT_RULES([yes])

AC_PROG_NASM
AC_PROG_LIBTOOL
AC_PROG_CC
AC_PROG_CXX
AM_PROG_CC_C_O
AC_PROG_RANLIB
AC_CONFIG_FILES([Makefile
	lexique/Makefile
    multilock/Makefile
    interblocage/Makefile
    tests/Makefile])
AC_OUTPUT
