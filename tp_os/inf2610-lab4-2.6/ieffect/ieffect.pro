#-------------------------------------------------
#
# Project created by QtCreator 2013-10-19T14:10:08
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = ieffect
TEMPLATE = app

include(effects/effects.pri)

SOURCES += main.cpp\
    imagequeue.cpp \
    imageloader.cpp \
    imagesaver.cpp \
    pipelinemanager.cpp \
    pipelinestage.cpp \
    effectstage.cpp \
    simpletracer.cpp

HEADERS  += \
    imagequeue.h \
    imageloader.h \
    imagesaver.h \
    pipelinemanager.h \
    pipelinestage.h \
    effectstage.h \
    simpletracer.h

FORMS    +=

OTHER_FILES += \
    effects/effects.pri
