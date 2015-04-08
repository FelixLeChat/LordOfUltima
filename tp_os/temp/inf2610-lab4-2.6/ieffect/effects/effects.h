#ifndef EFFECTS_H
#define EFFECTS_H

#include <QObject>
#include <QHash>

class ImageEffect;

class Effects : public QObject
{
    Q_OBJECT
public:
    explicit Effects(QObject *parent = 0);
    ~Effects();
    ImageEffect *effect(QString name);
    QStringList effectList();
    bool hasEffect(QString name);
signals:

public slots:

private:
    QHash<QString, ImageEffect*> *map;
};

#endif // EFFECTS_H
