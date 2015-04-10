TESTS_ENVIRONMENT = \
  PATH='$(abs_top_builddir)/src$(PATH_SEPARATOR)'"$$PATH" \
  abs_top_builddir='$(abs_top_builddir)' \
  abs_top_srcdir='$(abs_top_srcdir)' \
  LANG=en_US

AM_CFLAGS = -I$(top_builddir)/src

INCLUDES = -I$(top_srcdir)/src

check_SCRIPTS = test-lexique.sh test-interblocage.sh test-multilock.sh
TESTS = $(check_PROGRAMS) $(check_SCRIPTS)

EXTRA_DIST = $(check_SCRIPTS)
