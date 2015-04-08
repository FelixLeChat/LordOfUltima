#include "effects.h"

#include <QStringList>

#include "blur.h"
#include "brightness.h"
#include "cool.h"
#include "greyscale.h"
#include "saturation.h"
#include "sharpen.h"
#include "thumbnail.h"
#include "warm.h"

Effects::Effects(QObject *parent) :
    QObject(parent)
{
    map = new QHash<QString, ImageEffect*>();
    map->insert("blur", new Blur());
    map->insert("brightness", new Brightness());
    map->insert("cool", new Cool());
    map->insert("greyscale", new GreyScale());
    map->insert("saturation", new Saturation());
    map->insert("sharpen", new Sharpen());
    map->insert("thumbnail", new Thumbnail());
    map->insert("warm", new Warm());
}

Effects::~Effects()
{
    qDeleteAll(map->begin(), map->end());
    delete map;
}

ImageEffect *Effects::effect(QString name)
{
    return map->value(name);
}

QStringList Effects::effectList()
{
    return map->keys();
}

bool Effects::hasEffect(QString name)
{
    return map->contains(name);
}
