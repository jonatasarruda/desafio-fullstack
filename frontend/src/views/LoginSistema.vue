<template>
  <v-container class="fill-height primary"  dark fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card class="elevation-16 cor-de-borda" >
          <h1 class="tituloCard">Login</h1>
          <v-card-text>
            <v-form @submit.prevent="handleLogin">
               <Input
                 label="E-mail"
                 placeHolder="exemplo@exemplo.com"
                
               ></Input>

              <Input
                label="Senha"
                placeHolder="123456"
                type="password"
                
              ></Input>
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="primary" dark @click="enviaLogin">Entrar</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import Input from '../components/InputFormulario.vue';

export default {
  name: 'LoginSistema',
  components: {
    Input
  },
  data: () => ({
    email: '',
    password: '',
    emailRules: [
      v => !!v || 'E-mail é obrigatório',
      v => /.+@.+\..+/.test(v) || 'E-mail deve ser válido',
    ],
    passwordRules: [
      v => !!v || 'Senha é obrigatória',
    ],
  }),
  methods: {
    handleLogin() {
      // Lógica de login aqui
      console.log('Email:', this.email, 'Password:', this.password);
      // Exemplo: this.$refs.form.validate() para validar o formulário
    }
  }
};
</script>

<style scoped>

.tituloCard {
  text-align: center;
  margin-bottom: 10px;
  padding-top: 25px;
  
}


</style>
