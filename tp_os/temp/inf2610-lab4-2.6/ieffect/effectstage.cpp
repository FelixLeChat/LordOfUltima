#include "effectstage.h"

EffectStage::EffectStage(QObject *parent) :
    PipelineStage(parent)
{
}

void EffectStage::execute()
{
    int i = 0;
    setState(PipelineStage::waiting);
    forever {
        QImage *img = m_cons->dequeue();
        if (img == 0) {
            m_prod->enqueue(0);
            break;
        }
        qDebug() << "effx " << m_name << i++;
        setState(PipelineStage::running);
        QImage *out = m_effect->apply(img);
        setState(PipelineStage::waiting);
        m_prod->enqueue(out);
        delete img;
    }
}
