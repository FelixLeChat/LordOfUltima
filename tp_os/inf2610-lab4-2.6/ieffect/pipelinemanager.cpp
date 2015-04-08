#include "pipelinemanager.h"

#include "effects/effects.h"
#include "imageloader.h"
#include "imagesaver.h"
#include "imagequeue.h"
#include "simpletracer.h"
#include "effectstage.h"
#include "effects/effects.h"

#include <QVector>
#include <QProcess>
#include <QDateTime>
#include <QEventLoop>

#if defined(Q_OS_LINUX)
#include "pthread.h"
#elif defined(Q_OS_WIN)
#include "windows.h"
#endif

PipelineManager::PipelineManager(QObject *parent) :
    QObject(parent)
{
}

void PipelineManager::setup(QStringList fx, QDir &input, QDir &output)
{
    // Chargement des images comme première étape du pipeline
    ImageLoader *loader = new ImageLoader(this);
    loader->setName("loader");
    loader->setImageDir(input);
    stageList.append(loader);

    // Préparation des effets
    foreach (QString fxName, fx) {
        if (!effects.hasEffect(fxName)) {
            qDebug() << "unkown effect " << fxName;
            continue;
        }
        EffectStage *fxStage = new EffectStage(this);
        fxStage->setName(fxName);
        fxStage->setEffect(effects.effect(fxName));
        stageList.append(fxStage);
    }

    // Sauvegarde des images comme dernière étape du pipeline
    ImageSaver *saver = new ImageSaver(this);
    saver->setName("saver");
    saver->setOutput(output);
    stageList.append(saver);
}

#if defined(Q_OS_LINUX)
static void *startHelper(void *arg)
{
    PipelineStage *stage = (PipelineStage *) arg;
    qDebug() << "starting " << stage;
    stage->execute();
    return NULL;
}
#elif defined(Q_OS_WIN)
DWORD WINAPI startHelper(void* arg)
{
    PipelineStage *stage = (PipelineStage *) arg;
    stage->execute();
    return 0;
}
#endif

void PipelineManager::launchParallel(int queueSize)
{
    QDateTime now = QDateTime::currentDateTime();
    QString traceDir = QString("trace-%1").arg(now.toString());
    traceDir.replace(":", "_"); // Windows n'aime pas les ":" dans les noms de fichiers
    QDir dir(traceDir);
    dir.mkpath(".");

    // Définir le rank (position de l'étape dans le pipeline)
    for (int i = 0; i < stageList.size(); i++) {
        PipelineStage *stage = stageList.at(i);
        stage->setRank(i);
        SimpleTracer *t = new SimpleTracer(stage);
        QString stageName = QString("%1-0-%2.data")
                .arg(i)
                .arg(stage->name());
        QString path = traceDir + QDir::separator() + stageName;
        t->setPath(path);
        t->init();
    }

    // Connecter les étapes par des objets ImageQueue
    for (int i = 0; i < stageList.size() - 1; i++) {
        PipelineStage *prod = stageList.at(i);
        PipelineStage *cons = stageList.at(i + 1);
        ImageQueue *q = new ImageQueue(this, queueSize);
        qDebug() << "queue " << i << " " << prod << " ---> " << cons;

        // enregistrer les modifications effectuées à cette queue
        SimpleTracer *t = new SimpleTracer(q);
        QString stageName = QString("%1-1-%2-%3.data")
                .arg(i)
                .arg(prod->name())
                .arg(cons->name());
        QString path = traceDir + QDir::separator() + stageName;
        t->setPath(path);
        t->init();

        stageList.at(i)->setProdQueue(q);
        stageList.at(i + 1)->setConsQueue(q);
    }
#if defined(Q_OS_LINUX)
    // Démarrer les fils d'exécutions
    pthread_t *threads = new pthread_t[stageList.size()];
    for (int i = 0; i < stageList.size(); i++) {
        pthread_create(&threads[i], NULL, startHelper, stageList.at(i));
    }

    // Attendre la fin de l'exécution
    for (int i = 0; i < stageList.size(); i++) {
        pthread_join(threads[i], NULL);
    }
    delete[] threads;
#elif defined(Q_OS_WIN)
    HANDLE thread[stageList.size()];
    DWORD thID[stageList.size()];
    for (int i = 0; i < stageList.size(); i++) {
        thread[i] = CreateThread(NULL, 0, startHelper, (void*)stageList.at(i),0, &thID[i]);
        if(thread[i]==NULL)
        {
            printf("CreateThread failed : :(\n");
            return;
        }
    }
    printf("Début attente de la fin des threads \n");
    WaitForMultipleObjects(stageList.size(), thread, true, INFINITE);
    printf("Attente finit :)\n");
    for (int i = 0; i < stageList.size(); i++)
    {
        CloseHandle(thread[i]);
    }
#endif
}
