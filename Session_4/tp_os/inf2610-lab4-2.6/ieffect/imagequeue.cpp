#include <QImage>
#include <QDebug>
#include <QQueue>

#include "imagequeue.h"
#include "simpletracer.h"

ImageQueue::ImageQueue(QObject *parent, int capacity) :
    QObject(parent),
    m_capacity(capacity)
{
    buf = new QImage*[capacity];
    occupe = CreateSemaphore(NULL, 0, capacity, NULL);
    if (occupe == NULL)
    {
        qDebug() << "Creation du sémaphore à échoué :(";
    }
    libre = CreateSemaphore (NULL, capacity, capacity, NULL);
    if (libre == NULL)
    {
        qDebug() << "Creation du sémaphore à échoué :(";
    }
    ip = 0;
    ic = 0;
    taille = 0;
}

ImageQueue::~ImageQueue()
{
}

// Produire
void ImageQueue::enqueue(QImage *item)
{
    WaitForSingleObject(libre, INFINITE);
    buf[ip] = item;
    ip = (ip+1)%m_capacity;
    taille++;
    // tracer la taille de la file lorsqu'elle change
    SimpleTracer::writeEvent(this, taille);
    if(!ReleaseSemaphore(occupe, 1, NULL))
    {
        qDebug() << "Erreur lors de la libération du sémaphore occupe";
    }

}

// Consommer
QImage *ImageQueue::dequeue()
{
    QImage* item;
    // Si aucun item, regarder si fini
    WaitForSingleObject(occupe, INFINITE);
    item = buf[ic];
    ic = (ic+1)%m_capacity;
    taille--;
    // tracer la taille de la file lorsqu'elle change
    SimpleTracer::writeEvent(this, taille);
    if(!ReleaseSemaphore(libre, 1, NULL))
    {
        qDebug() << "Erreur lors de la libération du sémaphore occupe";
    }

    return item;
}
