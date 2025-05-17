import ControleAtendimentos from '@/views/ControleAtendimentos.vue'
import ControleClientes from '@/views/ControleClientes.vue'
import ControleUsuarios from '@/views/ControleUsuarios.vue'
import DashboardGeral from '@/views/DashboardGeral.vue'
import Login from '@/views/LoginSistema.vue'

const routes = [
    {
        path: '/login',
        name: 'LoginSistema',
        component: Login,
        title: 'Login',
        meta: { requiredAuth: false,
                showInMenu: false
              }
    },
    {
        path: '/',
        name: 'DashboardGeral',
        component: DashboardGeral,
        title: 'Dashboard',
        meta: { requiredAuth: true,
                showInMenu: true
              }
    },
    {
        path: '/clientes',
        name: 'ControleClientes',
        component: ControleClientes,
        title: 'Clientes',
        meta: { requiredAuth: true,
                showInMenu: true
              }
    },
    {
        path: '/atendimentos',
        name: 'ControleAtendimentos',
        component: ControleAtendimentos,
        title: 'Atendimentos',
        meta: { requiredAuth: true,
                showInMenu: true
              }
    },
    {
        path: '/usuarios',
        name: 'ControleUsuarios',
        component: ControleUsuarios,
        title: 'Usuarios',
        meta: { requiredAuth: true,
                showInMenu: true
              }
    }
]

export default routes