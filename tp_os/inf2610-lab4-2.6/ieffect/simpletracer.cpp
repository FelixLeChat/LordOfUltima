#include <QFile>
#include <QDebug>
#include <QTextStream>
#include <QElapsedTimer>

#include "simpletracer.h"

QHash<QObject*, SimpleTracer*> SimpleTracer::s_traces;

SimpleTracer::SimpleTracer(QObject *parent) :
    QObject(parent)
{
    m_isInitialized = false;
    m_timer = new QElapsedTimer();
    SimpleTracer::s_traces.insert(parent, this);
}

SimpleTracer::~SimpleTracer()
{
    delete m_timer;
    if (m_isInitialized)
        delete m_out;
    SimpleTracer::s_traces.remove(parent());
}

void SimpleTracer::init()
{
    QFile *f = new QFile(this);
    f->setFileName(m_path);
    if (!f->open(QIODevice::WriteOnly | QIODevice::Truncate | QIODevice::Text))
        return;
    m_out = new QTextStream(f);
    m_timer->start();
    m_isInitialized = true;
}

void SimpleTracer::close()
{
    if (m_isInitialized) {
        m_out->flush();
    }
    m_isInitialized = false;
}

void SimpleTracer::writeEvent(int value)
{
    if (m_isInitialized) {
        m_lock.lock();
        *m_out << m_timer->nsecsElapsed() << ";" << value << endl;
        m_lock.unlock();
    }
}

void SimpleTracer::writeEvent(QObject *parent, int value)
{
    SimpleTracer *tracer = SimpleTracer::s_traces[parent];
    if (tracer)
        tracer->writeEvent(value);
}
