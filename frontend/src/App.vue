<template>
  <v-app>
      <MenuSistema v-if="usuarioLogado"></MenuSistema>
    <v-main>
      <router-view/>
    </v-main>
    <GlobalSnackbar/>
  </v-app>
</template>

<script>
import MenuSistema from '../src/components/MenuSistema.vue';
import GlobalSnackbar from '../src/components/GlobalSnackbar.vue';



export default {
  name: 'App',
  components: {
    MenuSistema,
    GlobalSnackbar
  },

  data: () => ({
    usuarioLogado: false,
  }),
  methods: {
    verificarStatusLogin() {
      const token = localStorage.getItem('user-token');
      this.usuarioLogado = !!token; // Define true se token existir, false caso contrário
    }
  },
  created() {
    this.verificarStatusLogin(); // Verifica ao criar o componente
  },
  watch: {
    '$route'() {
      // Verifica o status do login toda vez que a rota muda
      this.verificarStatusLogin();
    }
  }
};
</script>
