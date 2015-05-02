#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QVector>
#include <QFileDialog>
#include <QDebug>
#include "qcustomplot.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    createActions();
    createMenus();
    createToolBars();
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::createActions()
{
    openAction = new QAction(tr("&Open"), this);
    connect(openAction, SIGNAL(triggered()), this, SLOT(openFile()));

    exitAction = new QAction(tr("E&xit"), this);
    exitAction->setShortcut(tr("Ctrl+Q"));
    connect(exitAction, SIGNAL(triggered()), this, SLOT(close()));
}

void MainWindow::createMenus()
{
    fileMenu = ui->menuBar->addMenu(tr("&File"));
    fileMenu->addAction(openAction);
    fileMenu->addSeparator();
    fileMenu->addAction(exitAction);
    fileMenu->show();
    ui->menuBar->show();
}

void MainWindow::createToolBars()
{
    ui->mainToolBar->addAction(openAction);
}

void MainWindow::openFile()
{
    QStringList fileList = QFileDialog::getOpenFileNames(this, tr("Open data files"), tr("*.data"));
    if (fileList.isEmpty())
        return;
    clearLayout(ui->verticalLayout);
    QList<QCustomPlot*> plotList;
    foreach(QString file, fileList) {
        QVector<double> x, y;
        loadFile(file, x, y);
        QFileInfo f(file);
        QCustomPlot *plot = makeGraph(f.fileName(), x, y);
        ui->verticalLayout->addWidget(plot);
        plotList.append(plot);
    }
    adjustRange(plotList);
}

/*
 * http://stackoverflow.com/questions/4272196/qt-remove-all-widgets-from-layout
 */
void MainWindow::clearLayout(QLayout *layout, bool deleteWidgets)
{
    while (QLayoutItem* item = layout->takeAt(0)) {
        if (deleteWidgets) {
            if (QWidget* widget = item->widget())
                delete widget;
        }
        if (QLayout* childLayout = item->layout())
            clearLayout(childLayout, deleteWidgets);
        delete item;
    }
}

void MainWindow::loadFile(QString file, QVector<double> &time, QVector<double> &size)
{
    QFile f(file);
    f.open(QIODevice::ReadOnly | QIODevice::Text);
    QTextStream in(&f);

    QString line;
    forever {
        line = in.readLine();
        if (line.isNull())
            break;
        QStringList items = line.split(";");
        if (items.size() != 2)
            continue;
        time.append(items.at(0).toDouble() / 1000000000.0);
        size.append(items.at(1).toDouble());
    }
}

QCustomPlot *MainWindow::makeGraph(QString name, QVector<double> &x, QVector<double> &y)
{
    qDebug() << name << " data size: " << x.size();
    QCustomPlot *plot = new QCustomPlot(this);
    plot->addGraph();
    QCPGraph *graph = plot->graph(0);
    graph->setLineStyle(QCPGraph::lsStepLeft);
    graph->setBrush(QBrush(Qt::red));
    graph->setData(x, y);

    // Add title
    QCPItemText *title = new QCPItemText(plot);
    plot->addItem(title);
    title->setPositionAlignment(Qt::AlignTop|Qt::AlignHCenter);
    title->position->setType(QCPItemPosition::ptAxisRectRatio);
    title->position->setCoords(0.5, 0);
    title->setText(name);
    title->setFont(QFont(font().family(), 12));

    // Add info label
    QCPItemText *infoLabel = new QCPItemText(plot);
    plot->addItem(infoLabel);
    infoLabel->setPositionAlignment(Qt::AlignTop|Qt::AlignLeft);
    infoLabel->position->setType(QCPItemPosition::ptAxisRectRatio);
    infoLabel->position->setCoords(0.1, 0);
    infoLabel->setText(QString("start: %1\nend: %2").arg(x.first()).arg(x.last()));
    infoLabel->setFont(QFont(font().family(), 8));
    return plot;
}

void MainWindow::adjustRange(QList<QCustomPlot*> plotList)
{

    double xmax = 0.0;
    double ymax = 0.0;
    foreach (QCustomPlot *plot, plotList) {
        const QCPDataMap *map = plot->graph(0)->data();
        double x = map->keys().last();
        if (x > xmax)
            xmax = x;

        QListIterator<QCPData> itr(map->values());
        while(itr.hasNext()) {
            double y = itr.next().value;
            if (y > ymax)
                ymax = y;
        }
        ymax += 1;
    }

    qDebug() << xmax << " " << ymax;

    foreach (QCustomPlot *plot, plotList) {
        plot->xAxis->setRange(0, xmax);
        plot->yAxis->setRange(0, ymax);
        plot->replot();
    }
}
