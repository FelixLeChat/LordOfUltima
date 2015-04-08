#ifndef IMAGESAVER_H
#define IMAGESAVER_H

#include <QObject>
#include <QDir>

#include "pipelinestage.h"
#include "imagequeue.h"

class ImageSaver : public PipelineStage
{
    Q_OBJECT
    Q_PROPERTY(QDir output READ output WRITE setOutput)

public:
    explicit ImageSaver(QObject *parent = 0);
    void execute();
    QDir output() {
        return m_output;
    }
    void setOutput(QDir &dir) {
        m_output = dir;
    }
signals:
public slots:
private:
    QDir m_output;
};

#endif // IMAGESAVER_H
