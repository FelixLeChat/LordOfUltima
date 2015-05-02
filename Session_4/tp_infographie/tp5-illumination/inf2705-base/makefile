CXXFLAGS += -I/usr/X11/include -I/opt/local/include -W -Wall -Wno-unused-parameter -Wno-deprecated-declarations
LDFLAGS += -L/usr/X11/lib -L/opt/local/lib

SRC = main.cpp chargertex.cpp fctsUtiles.cpp
all : main.exe
run: main.exe
	./main.exe
main.exe : $(SRC:.cpp=.o)
	$(CXX) $(LDFLAGS) -o$@ $(SRC:.cpp=.o) -lglut -lGLU -lGL -lGLEW

SOL = main_solution.cpp chargertex.cpp fctsUtiles.cpp
sol : main_solution.exe
runs: main_solution.exe
	./main_solution.exe
main_solution.exe : $(SOL:.cpp=.o)
	$(CXX) $(LDFLAGS) -o $@ $(SOL:.cpp=.o) -lglut -lGLU -lGL -lGLEW

clean:
	-\rm *.o *.exe

chargertex.o: chargertex.cpp chargertex.h bitmap.h
fctsUtiles.o: fctsUtiles.cpp fctsUtiles.h
main.o: main.cpp varglob.h fctsUtiles.h chargertex.h
main_solution.o: main_solution.cpp varglob.h fctsUtiles.h chargertex.h
