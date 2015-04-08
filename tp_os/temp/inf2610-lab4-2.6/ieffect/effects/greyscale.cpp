#include "greyscale.h"

#include <QImage>
#include <QColor>

GreyScale::GreyScale(QObject *parent) :
    QObject(parent)
{
}

QImage *GreyScale::apply(QImage *origin)
{
    QImage *newImage = new QImage(origin->width(), origin->height(), QImage::Format_ARGB32);

    QColor oldColor;

    for(int x = 0; x < newImage->width(); x++){
        for(int y = 0; y < newImage->height(); y++){
            oldColor = QColor(origin->pixel(x, y));
            int average = (oldColor.red() + oldColor.green() + oldColor.blue()) / 3;
            newImage->setPixel(x, y, qRgb(average, average, average));
        }
    }

    return newImage;
}
