#include "pipelinestage.h"

#include <QCoreApplication>
#include "simpletracer.h"

PipelineStage::PipelineStage(QObject *parent) :
    QObject(parent)
{
}

void PipelineStage::setState(int state)
{
    if (state != m_state) {
        m_state = state;
        SimpleTracer::writeEvent(this, m_state);
    }
}
