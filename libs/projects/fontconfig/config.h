#ifndef _CONFIG_H
#define _CONFIG_H

#define HAVE_MKOSTEMP 1
#define HAVE_RAND 1

#define FC_DEFAULT_FONTS "/usr/share/fonts"
#define FC_CACHEDIR "/tmp/cache"
#define FC_TEMPLATEDIR "/usr/share"

#define FC_GPERF_SIZE_T unsigned int

#define FLEXIBLE_ARRAY_MEMBER 

#define uuid_generate_random uuid_generate
#define SIZEOF_VOID_P 4
#define ALIGNOF_VOID_P 4

#define O_CREAT 0

#include <fcntl.h> 

#endif
