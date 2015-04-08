#ifndef SIMPLETRACER_H
#define SIMPLETRACER_H

#include <QString>
#include <QObject>
#include <QMutex>

class QTextStream;
class QElapsedTimer;

class SimpleTracer : public QObject
{
    Q_OBJECT
    Q_PROPERTY(QString path READ path WRITE setPath)
public:
    explicit SimpleTracer(QObject *parent = 0);
    ~SimpleTracer();
    void init();
    void close();
    bool isInitialized() { return m_isInitialized; }
    QString path() { return m_path; }
    void setPath(QString path) { m_path = path; }
    void writeEvent(int value);
    static void writeEvent(QObject *parent, int value);
private:
    QString m_path;
    QTextStream *m_out;
    QElapsedTimer *m_timer;
    bool m_isInitialized;
    QMutex m_lock;
    static QHash<QObject*, SimpleTracer*> s_traces;
};

#endif // SIMPLETRACER_H
