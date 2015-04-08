#-------------------------------------------------
#
# Project created by QtCreator 2013-11-05T12:07:12
#
#-------------------------------------------------

QT       += testlib testlib

TARGET = tst_teststest
CONFIG   += console
CONFIG   -= app_bundle

TEMPLATE = app

SOURCES += $$PWD/../ieffect/simpletracer.cpp

HEADERS += $$PWD/../ieffect/simpletracer.h

SOURCES += tst_teststest.cpp
DEFINES += SRCDIR=\\\"$$PWD/\\\"

INCLUDEPATH += $$PWD/../ieffect
DEPENDPATH += $$PWD/../ieffect
