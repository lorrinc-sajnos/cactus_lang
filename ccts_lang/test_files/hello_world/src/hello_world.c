#include "hello_world.h"


i32 main() {
    return hello_world_$$_main();
}


i32 hello_world_$$_main() {
	hello_world_$$_print_ln("Hello, world!");
	return  (0);

}

void hello_world_$$_print_ln(ch08* str) {
	printf("%s\n",str);

}


