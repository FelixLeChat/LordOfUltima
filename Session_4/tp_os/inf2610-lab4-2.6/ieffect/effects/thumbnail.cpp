#include "thumbnail.h"

#include <QImage>

Thumbnail::Thumbnail(QObject *parent) :
    QObject(parent)
{
}

QImage *Thumbnail::apply(QImage *origin)
{
    QImage img = origin->scaled(32, 32, Qt::KeepAspectRatio, Qt::SmoothTransformation);
    return new QImage(img);
}
