#ifndef BRIGHTNESS_H
#define BRIGHTNESS_H

#include <QObject>

#include "imageeffect.h"

class Brightness : public QObject, public ImageEffect
{
    Q_OBJECT
public:
    explicit Brightness(QObject *parent = 0);
    ~Brightness() {}
    QImage *apply(QImage *orig);
signals:

public slots:

};

#endif // BRIGHTNESS_H
