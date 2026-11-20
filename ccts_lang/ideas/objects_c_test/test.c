#include <stdint.h>
#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>

//Integers
typedef int8_t      i08;
typedef uint8_t     u08;
typedef int16_t     i16;
typedef uint16_t    u16;
typedef int32_t     i32;
typedef uint32_t    u32;
typedef int64_t     i64;
typedef uint64_t    u64;

typedef char        ch08;

typedef ch08* str;



const u64 FOO_ID = 0;
typedef struct {
    i32 num;
    
} Foo;

i32 S_Foo_get_id(Foo* this){
    return this->num;
}

const u64 BAR_ID = 1;
typedef struct {
    i32 x;
    i32 y;
} Bar;

i32 S_Bar_get_id(Bar* this){
    i32 result  = this->x;
    result <<= 4;
    result += this->y;
}


const u64 FUNC_get_id___i32_ID = 123;


typedef struct {
    u64 ID;
    void* data;
} obj;

void main(){
    obj foo = {FOO_ID, malloc(sizeof(Foo))};
    obj bar = {BAR_ID, malloc(sizeof(Bar))};

    //DEBUG DATA ASSIGNMENT
    ((Foo*)(foo.data))->num = 123;

    ((Bar*)(bar.data))->x = 45;
    ((Bar*)(bar.data))->y = 67;
    //

    
    void* foo_res = call_obj_func(foo, FUNC_get_id___i32_ID );



}
 i32 call_obj_func(obj object, u64 func_ID){
    switch(object.ID){
        case FOO_ID:
            switch (func_ID)            { 
                case FUNC_get_id___i32_ID:
                    return S_Foo_get_id((Foo*) object.data);
                    //...
            }
        //...
    }



}
