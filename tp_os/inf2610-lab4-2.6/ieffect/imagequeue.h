#ifndef IMAGEQUEUE_H
#define IMAGEQUEUE_H

#include <QObject>
#include "windows.h"

class QImage;

class ImageQueue : public QObject
{
    Q_OBJECT
public:
    explicit ImageQueue(QObject *parent = 0, int capacity = 4);
    ~ImageQueue();
    void enqueue(QImage *item);
    QImage *dequeue();
    bool isEmpty();
private:
    int m_capacity;
    HANDLE occupe;
    HANDLE libre;
    int ic;
    int ip;
    QImage** buf;
    int taille;
};

#endif // IMAGEQUEUE_H
