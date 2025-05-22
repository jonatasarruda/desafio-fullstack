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

  if (to.matched.some(record => record.meta.requiresAuth)) { 
    if (!isAuthenticated) { 
      next({
        path: '/login',
        query: { redirect: to.fullPath } 
      });
    } else {
      next();
    }
  } else if (to.matched.some(record => record.meta.guest)) { 
    if (isAuthenticated) { 
      next({ path: '/' }); 
    } else { 
      next();
    }
  } else { 
    next();
  }
});

export default router
