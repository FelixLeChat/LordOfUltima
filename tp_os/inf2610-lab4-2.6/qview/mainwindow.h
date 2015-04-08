#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

class QCustomPlot;

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    void createActions();
    void createMenus();
    void createToolBars();
    void setup(QCustomPlot *customPlot);
    void clearLayout(QLayout *layout, bool deleteWidget = true);
    void loadFile(QString file, QVector<double> &x, QVector<double> &y);
    QCustomPlot *makeGraph(QString name, QVector<double> &x, QVector<double> &y);
    void adjustRange(QList<QCustomPlot*> plotList);
public slots:
    void openFile();
private:
    Ui::MainWindow *ui;
    QMenu *fileMenu;
    QToolBar *fileToolBar;
    QAction *openAction;
    QAction *exitAction;
};

#endif // MAINWINDOW_H
