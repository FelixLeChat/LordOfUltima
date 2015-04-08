#include <QString>
#include <QtTest>
#include <QDebug>

#include "simpletracer.h"
#include "pipelinestage.h"

class TestsTest : public QObject
{
    Q_OBJECT

public:
    TestsTest();

signals:
    void testInt(int value);
    void testState(int state);

private Q_SLOTS:
    void testCase1();
};

TestsTest::TestsTest()
{
}

void TestsTest::testCase1()
{
    static const QString outfile = "out.log";
    SimpleTracer tracer;
    tracer.setPath(outfile);
    tracer.init();
    connect(this, SIGNAL(testInt(int)),
            &tracer, SLOT(writeEvent(int)));
    connect(this, SIGNAL(testState(int)),
            &tracer, SLOT(writeEvent(int)));
    for (int i = 0; i < 10; i++) {
        emit testInt(42 + i);
        emit testState(PipelineStage::running);
        emit testState(PipelineStage::waiting);
    }
    tracer.close();
    QFile trace(outfile);
    QVERIFY2(trace.size() > 0, "Failure");
}

QTEST_APPLESS_MAIN(TestsTest)

#include "tst_teststest.moc"
