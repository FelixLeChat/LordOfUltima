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
}

ImageQueue::~ImageQueue()
{
}

// Produire
void ImageQueue::enqueue(QImage *item)
{
    WaitForSingleObject(libre, INFINITE);
     qDebug() << "Produit";
    buf[ip] = item;
    ip = (ip+1)%m_capacity;
    if(!ReleaseSemaphore(occupe, 1, NULL))
    {
        qDebug() << "Erreur lors de la libération du sémaphore occupe";
    }
    // tracer la taille de la file lorsqu'elle change
    SimpleTracer::writeEvent(this, 0);
}

// Consommer
QImage *ImageQueue::dequeue()
{
    QImage* item;
    // Si aucun item, regarder si fini
    WaitForSingleObject(occupe, INFINITE);

    qDebug() << "Consomme";
    item = buf[ic];
    ic = (ic+1)%m_capacity;
    if(!ReleaseSemaphore(libre, 1, NULL))
    {
        qDebug() << "Erreur lors de la libération du sémaphore occupe";
    }
    // tracer la taille de la file lorsqu'elle change
    SimpleTracer::writeEvent(this, 0);
    return item;
}
