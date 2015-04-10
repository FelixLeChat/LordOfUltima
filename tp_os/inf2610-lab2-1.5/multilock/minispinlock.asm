;
; Implementation minimale d'un spinlock
;
SECTION .data

SECTION .text
global mini_spin_lock
global mini_spin_unlock

mini_spin_lock:              ; adresse du verrou dans rdi
    mov rcx, 1               ; valeur du verrou occupe
mini_spin_lock_retry:
	xor rax, rax             ; remettre a zero rax
	lock cmpxchg [rdi], rcx  ; compare la valeur [rdi] avec rax
                             ; si ([rdi] == rax) i.e. verrou occupe
                             ;   ZF = 1, [rdi] = rcx
                             ; sinon
                             ;   ZF = 0, rax = [rdi]
    jnz mini_spin_lock_retry ; si (ZF == 0), recommence
    ret                      ; verrou obtenu

mini_spin_unlock:            ; adresse du verrou dans rdi
    mov qword [rdi], 0       ; libere le verrou
    ret
