#ifndef CCTS_STD
#define CCTS_STD

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


//Object
typedef uint64_t    t_oid; //Object ID

typedef struct {
    t_oid TYPE_ID;
    void* INSTANCE;
} OBJECT;





#endif