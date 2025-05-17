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
        meta: { requiredAuth: false }
    },
    {
        path: '/',
        name: 'Dashboard',
        component: DashboardGeral,
        title: 'Dashboard',
        meta: { requiredAuth: true }
    },
    {
        path: '/clientes',
        name: 'Clientes',
        component: ControleClientes,
        title: 'Clientes',
        meta: { requiredAuth: true }
    },
    {
        path: '/atendimentos',
        name: 'Atendimentos',
        component: ControleAtendimentos,
        title: 'Atendimentos',
        meta: { requiredAuth: true }
    },
    {
        path: '/usuarios',
        name: 'Usuarios',
        component: ControleUsuarios,
        title: 'Usuarios',
        meta: { requiredAuth: true }
    }
]

export default routes