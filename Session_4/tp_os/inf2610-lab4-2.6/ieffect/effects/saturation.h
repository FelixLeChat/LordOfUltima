#ifndef SATURATION_H
#define SATURATION_H

#include <QObject>

#include "imageeffect.h"

class Saturation : public QObject, public ImageEffect
{
    Q_OBJECT
public:
    explicit Saturation(QObject *parent = 0);
    ~Saturation() {}
    QImage *apply(QImage *img);

signals:

public slots:

};

#endif // SATURATION_H
