<template>
  <v-app>
      <MenuSistema v-if="usuarioLogado"></MenuSistema>
    <v-main>
      <router-view/>
    </v-main>
  </v-app>
</template>

<script>
import MenuSistema from '../src/components/MenuSistema.vue';


export default {
  name: 'App',
  components: {
    MenuSistema
  },

  data: () => ({
    usuarioLogado: false,
  }),

};
</script>
