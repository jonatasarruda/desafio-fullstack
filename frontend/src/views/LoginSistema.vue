<template>
  <v-container class="fill-height primary"  dark fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card class="elevation-16 cor-de-borda" >
          <h1 class="tituloCard">Login</h1>
          <v-card-text>
            <v-form @submit.prevent="enviaLogin">
               <Input
                 label="E-mail"
                 placeHolder="exemplo@exemplo.com"
                 v-model="email"
                 :error-messages="emailErrors"
                 @input="emailErrors = []" 
               ></Input>

              <Input
                label="Senha"
                placeHolder="123456"
                type="password"
                v-model="senha"
                :error-messages="passwordErrors"
                @input="passwordErrors = []" 
              ></Input>
              <v-alert v-if="loginError" type="error" dense class="mt-3">
                {{ loginError }}
              </v-alert>
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="primary" dark @click="enviaLogin" :loading="loading">Entrar</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import Input from '../components/InputFormulario.vue';
import apiService from '@/services/apiService';

export default {
  name: 'LoginSistema',
  components: {
    Input
  },
  data: () => ({
    email: '',
    senha: '',
    emailErrors: [],
    passwordErrors: [],
    loginError: null,
    loading: false,
  }),
  methods: {
    async enviaLogin() {
      this.loading = true;
      this.loginError = null;
      this.emailErrors = [];
      this.passwordErrors = [];
      this.usuarioLogado = false;

      // Validação simples no frontend
      if (!this.email) {
        this.emailErrors.push('E-mail é obrigatório.');
      }
      if (!this.senha) {
        this.passwordErrors.push('Senha é obrigatória.');
      }
      if (this.emailErrors.length || this.passwordErrors.length) {
        this.loading = false;
        return;
      }

      try {
        const response = await apiService.login({
          email: this.email,
          senha: this.senha,
        });

        // Ajuste conforme a estrutura da sua resposta da API
        const token = response.data.token; 
        const user = response.data.email; 
        const id = response.data.id;


        if (token && user) {
          localStorage.setItem('user-token', token);
          localStorage.setItem('user-info', JSON.stringify(user));
          localStorage.setItem('user-id', id) // Armazena info do usuário
          apiService.setAuthHeader(token); // Configura o apiService para usar o token
          
          const redirectPath = this.$route.query.redirect || '/';
          this.$router.push(redirectPath);
        } else {
          this.loginError = 'Resposta inválida do servidor. Tente novamente.';
          console.error('Login response missing token or user info:', response.data);
        }
      } catch (error) {
        if (error.response && error.response.status === 401) {
          this.loginError = 'E-mail ou senha inválidos.';
        } else if (error.response && error.response.data && error.response.data.message) {
          this.loginError = error.response.data.message; // Se sua API envia uma mensagem de erro específica
        } else {
          this.loginError = 'Erro ao tentar fazer login. Verifique sua conexão e tente novamente.';
          console.error('Login error:', error);
        }
      } finally {
        this.loading = false;
      }
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
