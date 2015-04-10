/*
 * frequency.c
 *
 *  Created on: Aug 26, 2013
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <glib.h>

/*
 * Fonctions pour collections
 */

struct entry {
    char *word;
    int count;
};

void display_entry(gpointer val, gpointer data) {
    int ret;
    char *str;
    struct entry *ent = val;

    if (strlen(ent->word) < 3)
        return;
    ret = asprintf(&str, "%-5d %s\n", ent->count, ent->word);
    if (ret < 0)
        return;
    ret = write(GPOINTER_TO_INT(data), str, ret);
    free(str);
    if (ret < 0)
        return;
}

void add_word(GHashTable *freq, char *word) {
    if (!g_hash_table_contains(freq, word)) {
        struct entry *value = calloc(1, sizeof(struct entry));
        value->word = g_strdup(word);
        g_hash_table_insert(freq, g_strdup(word), value);
    }
    struct entry *value = g_hash_table_lookup(freq, word);
    value->count++;
}

// sort by decreasing order
gint entry_compare(gconstpointer a, gconstpointer b, gpointer data) {
    const struct entry *self  = a;
    const struct entry *other = b;
    if (self->count > other->count)
        return -1;
    if (self->count < other->count)
        return 1;
    return 0;
}

void sort_entry(gpointer key, gpointer val, gpointer data) {
    struct entry *e = val;
    GSequence *seq = data;
    g_sequence_insert_sorted(seq, e, entry_compare, NULL);
}

void display_results(GHashTable *freq, int out)
{
    GSequence *seq = g_sequence_new(NULL);
    g_hash_table_foreach(freq, sort_entry, seq);
    g_sequence_foreach(seq, display_entry, GINT_TO_POINTER(out));
    g_sequence_free(seq);
}

void free_entry(gpointer entry) {
    struct entry *e = entry;
    free(e->word);
    free(e);
}

void task_frequency(int in_length, int in_string, int out) {
    int ret;
    long len;
    char *word;
    GHashTable *freq = g_hash_table_new_full(g_str_hash,  g_str_equal, g_free, free_entry);

    do {
        ret = read(in_length, &len, sizeof(len));
        if (ret <= 0)
            break;
        if (len < 0) {
            printf("frequency is stopping\n");
            break;
        }
        word = malloc((len + 1) * sizeof(char));
        ret = read(in_string, word, len);
        if (ret <= 0)
            break;
        word[len] = '\0';
        add_word(freq, word);
        free(word);
    } while(1);

    display_results(freq, out);
    g_hash_table_destroy(freq);
}
