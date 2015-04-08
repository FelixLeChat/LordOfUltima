#ifndef WARM_H
#define WARM_H

#include <QObject>

#include "imageeffect.h"

class Warm : public QObject, public ImageEffect
{
    Q_OBJECT
public:
    explicit Warm(QObject *parent = 0);
    ~Warm() {}
    QImage *apply(QImage *orig);

signals:

public slots:

};

#endif // WARM_H
