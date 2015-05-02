CXXFLAGS += -I/usr/X11/include -I/opt/local/include -W -Wall -Wno-unused-parameter -Wno-deprecated-declarations
LDFLAGS += -L/usr/X11/lib -L/opt/local/lib

SRC = main.cpp 
all : main.exe
run: main.exe
	./main.exe
main.exe : $(SRC:.cpp=.o)
	$(CXX) $(LDFLAGS) -o$@ $(SRC:.cpp=.o) -lglut -lGLU -lGL -lGLEW

SOL = main_solution.cpp 
sol : main_solution.exe
runs: main_solution.exe
	./main_solution.exe
main_solution.exe : $(SOL:.cpp=.o)
	$(CXX) $(LDFLAGS) -o $@ $(SOL:.cpp=.o) -lglut -lGLU -lGL -lGLEW

clean:
	-\rm *.o *.exe

main.o: main.cpp varglob.h float3.h teapot_data.h
main_solution.o: main_solution.cpp varglob.h float3.h teapot_data.h
