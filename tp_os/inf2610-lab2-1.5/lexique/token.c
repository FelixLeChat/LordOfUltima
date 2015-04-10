/*
 * token.c
 *
 *  Created on: Aug 26, 2013
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#include <string.h>
#include <glib.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

struct buffer {
    char *data;
    size_t size;
    size_t pos;
    size_t len;
};

static struct timespec delay = { .tv_sec = 0, .tv_nsec = 10000000 };

char *buffer_start(struct buffer *buf) {
    return buf->data + buf->pos;
}

size_t buffer_remains(struct buffer *buf) {
    return buf->size - buf->pos - 1;
}

struct buffer *make_buffer(size_t size) {
    struct buffer *buf = calloc(1, sizeof(struct buffer));
    buf->data = calloc(size, sizeof(char));
    buf->size = size;
    return buf;
}

void free_buffer(struct buffer *buf) {
    free(buf->data);
    free(buf);
}

void buffer_print(struct buffer *buf) {
    printf("size=%lu pos=%lu data=%s\n", buf->size, buf->pos, buf->data);
}

int write_word(char *word, int out_length, int out_string) {
    int ret;
    GString *wordstr = g_string_new(word);
    GString *lowercase = g_string_ascii_down(wordstr);

    fprintf(stderr, "token: (%s,%lu)\n", lowercase->str, lowercase->len);

    ret = write(out_length, &lowercase->len, sizeof(lowercase->len));
    if (ret < 0)
        goto out;

    ret = write(out_string, lowercase->str, lowercase->len);
    if (ret < 0)
        goto out;

    nanosleep(&delay, NULL);
    return 0;
    out:
    g_string_free(lowercase, TRUE);
    g_string_free(wordstr, TRUE);
    fprintf(stderr, "write error\n");
    return -1;
}

int tokenize(GRegex *re, struct buffer *recv, int out_length, int out_string) {
    int x, y, nr, i, error;
    GMatchInfo *match = NULL;
    gchar *word;

    error = 0;
    g_regex_match(re, recv->data, 0, &match);
    while(g_match_info_matches(match) && !error) {
        g_match_info_fetch_pos(match, 1, &x, &y);
        word = g_match_info_fetch(match, 1);
        // copy last word, may be incomplete
        if (y == recv->len) {
            char *tail = recv->data + x;
            strcpy(recv->data, tail);
            recv->pos = strlen(tail);
        } else {
            if ((y - x) > 1) {
                error = write_word(word, out_length, out_string);
            }
        }
        g_free(word);
        g_match_info_next(match, NULL);
    }
    g_match_info_free(match);
    return error;
}

void task_tokenize(int in, int out_length, int out_string) {
    size_t size;
    int i, ret;
    GRegex *re;
    GError *err = NULL;
    struct buffer *recv = make_buffer(100);

    re = g_regex_new("[0-9 \t\n\'\".,]*([a-zA-Z]+)[0-9 \t\n\'\".,]*", 0, 0, &err);
    if (!re) {
        printf("error: %s\n", err->message);
        return;
    }

    while(((size = read(in, buffer_start(recv), buffer_remains(recv))) > 0)) {
        // make sure the string is null-terminated
        recv->len = size + recv->pos;
        recv->data[recv->len] = '\0';
        recv->pos = 0;
        ret = tokenize(re, recv, out_length, out_string);
        if (ret < 0)
            goto out;
    }

    // process residue if any
    if (recv->pos > 0) {
        tokenize(re, recv, out_length, out_string);
    }
    out:
    free_buffer(recv);
    g_regex_unref(re);
    return;
}
