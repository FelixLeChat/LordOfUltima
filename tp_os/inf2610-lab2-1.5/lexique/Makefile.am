bin_PROGRAMS = lexique remplissage

AM_CFLAGS = $(GLIB_CFLAGS)

remplissage_SOURCES = remplissage.c

lexique_SOURCES = lexique.c
lexique_LDADD = liblexique.a $(GLIB_LIBS)

noinst_LIBRARIES = liblexique.a

liblexique_a_SOURCES = frequency.c frequency.h token.c token.h

EXTRA_DIST = text-galaxy text-linux text-small
