#ifndef IMAGEEFFECT_H
#define IMAGEEFFECT_H

#include <QDebug>

class QImage;

class ImageEffect
{
public:
    ImageEffect();
    virtual ~ImageEffect() {}
    virtual QImage *apply(QImage *orig) {
        (void) orig;
        qDebug() << "this function should never be called";
        return NULL;
    }
};

#endif // IMAGEEFFECT_H
