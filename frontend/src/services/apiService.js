import api from './api.js'


const setAuthHeader = (token) => {
  if (token) {
    api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  } else {
    delete api.defaults.headers.common['Authorization'];
  }
};

const initialToken = localStorage.getItem('user-token');
if (initialToken) {
  setAuthHeader(initialToken);
}

async function login(credentials) {
// O endpoint de login geralmente não requer token de autorização

return api.post('/usuarios/login', credentials); // Ajuste o endpoint '/auth/login' conforme sua API
}

async function logout() {
// Se sua API tiver um endpoint de logout para invalidar o token no servidor:
// return apiClient.post('/auth/logout');
return Promise.resolve(); // Se não houver endpoint de logout no backend
}

async function obterTodos(rota){
    return await api.get(`${rota}`)
    .then(response => {
        return response.data})
    .catch(error => error.message)
}

async function obterPorId(rota, id){
    return await api.get(`${rota}/${id}`)
    .then(response => {
        return response.data})
    .catch(error => error.message)
}

async function obterPorNome(rota, nome){
    return await api.get(`${rota,nome}`)
    .then(response => {
        return response.data})
    .catch(error => error.message)
}

async function cadastrarNovo(rota, entidade){
    return await api.post(`${rota}`, entidade)
    .then(response => {
        return response.data})
    .catch(error => error.message)
}

async function atualizar(rota, id, entidade){
    return await api.put(`${rota}/${id}`, entidade)
    .then(response => {
        return response.data})
    .catch(error => error.message)
}




export default {
    obterTodos,
    obterPorId,
    obterPorNome,
    cadastrarNovo,
    atualizar,
    login,
    logout,
    setAuthHeader
}