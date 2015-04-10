/*
 * semrelay.h
 *
 *  Created on: 2013-08-19
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#ifndef SEMRELAY_H_
#define SEMRELAY_H_

#include "multilock.h"

void *semrelay_worker(void *ptr);
void semrelay_init(struct experiment *exp);
void semrelay_done(struct experiment *exp);

#endif /* SEMRELAY_H_ */
