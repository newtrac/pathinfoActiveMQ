#include <iostream>
using namespace std;

int main(int argc, char* argv[])
{   
    // Test compiling mode
    if (sizeof(void*) == 8) cout << "Compiling 64-bits" << endl;
    else cout << "Compiling 32-bits" << endl;

    return 0;
}