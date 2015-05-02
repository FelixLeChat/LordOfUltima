/*
 * serial.h
 *
 *  Created on: 2013-08-19
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#ifndef SERIAL_H_
#define SERIAL_H_

void *serial_worker(void *data);
void serial_init(struct experiment *data);
void serial_done(struct experiment *data);

#endif /* SERIAL_H_ */
