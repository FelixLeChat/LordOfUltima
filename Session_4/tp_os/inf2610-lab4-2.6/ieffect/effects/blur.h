#ifndef BLUR_H
#define BLUR_H

#include <QObject>
#include <QDebug>

#include "imageeffect.h"

class Blur : public QObject, public ImageEffect
{
    Q_OBJECT
public:
    explicit Blur(QObject *parent = 0);
    ~Blur() {}
    QImage *apply(QImage *orig);
signals:

public slots:

};

#endif // BLUR_H
