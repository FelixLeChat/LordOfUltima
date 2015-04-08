#ifndef SHARPEN_H
#define SHARPEN_H

#include <QObject>

#include "imageeffect.h"

class Sharpen : public QObject, public ImageEffect
{
    Q_OBJECT
public:
    explicit Sharpen(QObject *parent = 0);
    ~Sharpen() {}
    QImage *apply(QImage *orig);
signals:

public slots:

};

#endif // SHARPEN_H
