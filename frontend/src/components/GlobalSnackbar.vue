<template>
  <v-snackbar
    v-model="show"
    :color="color"
    :timeout="timeout"
    top
  >
    {{ message }}
    <template v-slot:action="{ attrs }">
      <v-btn
        text
        v-bind="attrs"
        @click="closeSnackbar"
      >
        Fechar
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script>
import { mapState, mapMutations } from 'vuex';

export default {
  name: 'GlobalSnackbar',
  computed: {
    ...mapState('snackbar', ['message', 'color', 'timeout']),
    show: {
      get() {
        return this.$store.state.snackbar.show;
      },
      set(value) {
        // Quando o v-snackbar tenta fechar (value se torna false),
        // nós chamamos a mutação para atualizar o estado no Vuex.
        if (!value) {
          this.$store.commit('snackbar/HIDE_SNACKBAR');
        }
      },
    },
  },
  methods: {
    // mapMutations não é estritamente necessário aqui se usarmos o setter de 'show'
    // mas é bom para o botão 'Fechar' explícito.
    ...mapMutations('snackbar', ['HIDE_SNACKBAR']),
    closeSnackbar() {
      this.HIDE_SNACKBAR();
    },
  },
};
</script>
