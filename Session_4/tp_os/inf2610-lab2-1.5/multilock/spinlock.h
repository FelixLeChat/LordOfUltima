/*
 * spinlock.h
 *
 *  Created on: 2013-08-19
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#ifndef SPINLOCK_H_
#define SPINLOCK_H_

#include "multilock.h"

void *spinlock_worker(void *data);
void spinlock_init(struct experiment *exp);
void spinlock_done(struct experiment *exp);

#endif /* SPINLOCK_H_ */
