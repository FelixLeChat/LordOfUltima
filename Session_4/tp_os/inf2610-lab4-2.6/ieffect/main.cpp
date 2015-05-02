#include <QApplication>

#include <QtGui>
#include <QtCore>
#include <QRegExp>

#include <iostream>
#include "effects/effects.h"
#include "effects/imageeffect.h"
#include "imagequeue.h"
#include "imageloader.h"
#include "imagesaver.h"
#include "pipelinemanager.h"
#include "effectstage.h"

class Opts {
public:
    Opts() : queueSize(1) {}
    int parseArguments(QStringList &args) {
        int ret = 0;
        for (int i = 0; i < args.size() - 1; i++) {
            QString arg = args.at(i);
            QString opt = args.at(i+1);
            if (arg.compare("--effects") == 0) {
                fx = opt.split(",");
            } else if (arg.compare("--input") == 0) {
                inputDir = QDir(opt);
            } else if (arg.compare("--output") == 0) {
                outputDir = QDir(opt);
            } else if (arg.compare("--queue-size") == 0) {
                queueSize = opt.toInt();
            }
        }

        qDebug() << "effects    : " << fx;
        qDebug() << "queue-size : " << queueSize;
        qDebug() << "input      : " << inputDir.absolutePath();
        qDebug() << "output     : " << outputDir.absolutePath();

        if (fx.isEmpty()) {
            qDebug() << "No effects to apply";
            ret = -1;
        }

        Effects effects;
        bool unkownEffect = false;
        foreach (QString fxName, fx) {
            if (!effects.hasEffect(fxName)) {
                qDebug() << "unkown effect " << fxName;
                unkownEffect = true;
            }
        }

        if (unkownEffect) {
            qDebug() << "one or more unkown effect, aborting";
            ret = -1;
        }

        return ret;
    }
    QStringList fx;
    QDir inputDir;
    QDir outputDir;
    int queueSize;
};

class MainLoop : public QThread {
public:
    QApplication *app;
    void run() {
        app->exec();
    }
};

int main(int argc, char *argv[])
{
    (void) argc;
    (void) argv;
    QApplication app(argc, argv);
    app.setApplicationName(app.translate("main", "iEffect"));

    // Traitement des arguments
    Opts opts;
    QStringList args = QApplication::arguments();
    if (opts.parseArguments(args) < 0)
        return -1;

    // Objet principal de gestion du pipeline
    PipelineManager manager;
    manager.setup(opts.fx, opts.inputDir, opts.outputDir);
    // Exécution (bloquant)
    manager.launchParallel(opts.queueSize);
    return 0;
}
