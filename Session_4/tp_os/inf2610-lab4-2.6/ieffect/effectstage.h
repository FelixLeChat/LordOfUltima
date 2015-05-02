#ifndef EFFECTSTAGE_H
#define EFFECTSTAGE_H

#include "effects/imageeffect.h"
#include "pipelinestage.h"

#include <QObject>

class EffectStage : public PipelineStage
{
    Q_OBJECT
    Q_PROPERTY(ImageEffect* effect READ effect WRITE setEffect)

public:
    explicit EffectStage(QObject *parent = 0);
    void execute();
    ImageEffect *effect() { return m_effect; }
    void setEffect(ImageEffect *e) { m_effect = e; }
private:
    ImageEffect *m_effect;
};

#endif // EFFECTSTAGE_H
