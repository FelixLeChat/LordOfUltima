#ifndef PIPELINESTAGE_H
#define PIPELINESTAGE_H

#include <QDebug>
#include <QImage>

#include "imagequeue.h"

class PipelineStage : public QObject
{
    Q_OBJECT
    Q_PROPERTY(ImageQueue* cons READ consQueue WRITE setConsQueue)
    Q_PROPERTY(ImageQueue* prod READ prodQueue WRITE setProdQueue)
    Q_PROPERTY(QString name READ name WRITE setName)
    Q_PROPERTY(int rank READ rank WRITE setRank)

public:
    static const int waiting = 0x0;
    static const int running = 0x1;

    explicit PipelineStage(QObject *parent = 0);
    virtual void execute() = 0;
    QString name() { return m_name; }
    void setName(QString name ) { m_name = name; }
    int rank() { return m_rank; }
    void setRank(int rank) { m_rank = rank; }
    ImageQueue *consQueue() { return m_cons; }
    ImageQueue *prodQueue() { return m_prod; }
    void setConsQueue(ImageQueue *_queue) { m_cons = _queue; }
    void setProdQueue(ImageQueue *_queue) { m_prod = _queue; }
    int state() const { return m_state; }

signals:
    void stateChanged(int newState);
    void done();
public slots:
    void setState(int state);
protected:
    QString m_name;
    int m_rank;
    int m_state;
    ImageQueue *m_cons;
    ImageQueue *m_prod;
};

#endif // PIPELINESTAGE_H
