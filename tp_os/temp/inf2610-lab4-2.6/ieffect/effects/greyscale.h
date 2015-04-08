#ifndef GREYSCALE_H
#define GREYSCALE_H

#include <QObject>
#include <QDebug>

#include "imageeffect.h"

/*
 * Source: http://developer.nokia.com/Community/Wiki/Image_editing_techniques_and_algorithms_using_Qt
 */

class GreyScale : public QObject, public ImageEffect
{
    Q_OBJECT
public:
    explicit GreyScale(QObject *parent = 0);
    ~GreyScale() {}
    QImage *apply(QImage *img);
};

#endif // GREYSCALE_H
