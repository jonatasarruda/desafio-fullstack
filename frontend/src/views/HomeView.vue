<template>
  <div class="home">
    <hello-world />
    <v-btn color="primary" class="mt-5"> Botão Vuetify </v-btn>
  </div>
</template>

<script>
  import HelloWorld from '../components/HelloWorld'

  export default {
    name: 'HomeView',

    components: {
      HelloWorld,
    },
  }
</script>

<style scoped>
.home {
  text-align: center; /* Para centralizar o botão */
}
.mt-5 {
  margin-top: 20px; /* Apenas para espaçamento */
}
</style>