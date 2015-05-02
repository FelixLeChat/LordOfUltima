/*
 * stack_tp_provider.h
 *
 *  Created on: 2013-09-16
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */


#undef TRACEPOINT_PROVIDER
#define TRACEPOINT_PROVIDER stack

#undef TRACEPOINT_INCLUDE_FILE
#define TRACEPOINT_INCLUDE_FILE ./stack_tp_provider.h

#ifdef __cplusplus
#extern "C"{
#endif /*__cplusplus */

#if !defined(_STACK_TP_PROVIDER_H) || defined(TRACEPOINT_HEADER_MULTI_READ)
#define _STACK_TP_PROVIDER_H
/*
 * Add this to allow programs to call "tracepoint(...):
 */
#include <lttng/tracepoint.h>

/**/
TRACEPOINT_EVENT(
        stack,
        entry,
        TP_ARGS(unsigned long, rsp),
        TP_FIELDS(
                ctf_integer(unsigned long, rsp, rsp)
        )
)

#endif /* _STACK_TP_PROVIDER_H */
#include <lttng/tracepoint-event.h>

#ifdef __cplusplus
}
#endif /*__cplusplus */
