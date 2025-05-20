import Vue from 'vue'
import VueRouter from 'vue-router'
import routes from './routes'

Vue.use(VueRouter)

const router = new VueRouter({
  mode: 'history',
  routes
})

router.beforeEach((to, from, next) => {
  const isAuthenticated = !!localStorage.getItem('user-token');

  if (to.matched.some(record => record.meta.requiresAuth)) { // Se a rota requer autenticação
    if (!isAuthenticated) { // E o usuário não está autenticado
      next({
        path: '/login',
        query: { redirect: to.fullPath } // Salva a rota que o usuário tentou acessar
      });
    } else { // Se está autenticado, permite o acesso
      next();
    }
  } else if (to.matched.some(record => record.meta.guest)) { // Se é uma rota para convidados (ex: login)
    if (isAuthenticated) { // E o usuário já está autenticado
      next({ path: '/' }); // Redireciona para a página principal de autenticados
    } else { // Se não está autenticado, permite o acesso à rota de convidado
      next();
    }
  } else { // Para rotas que não têm meta (públicas ou não especificadas)
    next();
  }
});

export default router
