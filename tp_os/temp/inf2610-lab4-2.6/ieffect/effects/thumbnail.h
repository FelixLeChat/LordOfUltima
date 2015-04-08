#ifndef SCALE_H
#define SCALE_H

#include "imageeffect.h"

#include <QObject>

class Thumbnail : public QObject, public ImageEffect
{
    Q_OBJECT
public:
    explicit Thumbnail(QObject *parent = 0);
    QImage *apply(QImage *orig);
signals:

public slots:

};

#endif // SCALE_H
