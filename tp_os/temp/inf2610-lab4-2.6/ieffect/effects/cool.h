#ifndef COOL_H
#define COOL_H

#include <QObject>

#include "imageeffect.h"

class Cool : public QObject, public ImageEffect
{
    Q_OBJECT
public:
    explicit Cool(QObject *parent = 0);
    ~Cool() {}
    QImage *apply(QImage *orig);

signals:

public slots:

};

#endif // COOL_H
