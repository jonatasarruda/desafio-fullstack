import api from './api.js'

async function obterTodos(rota){
    return await api.get(`${rota}`)
    .then(response => {
        return response.data})
    .catch(error => error.message)
}

async function obterPorId(rota, id){
    return await api.get(`${rota,id}`)
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
    atualizar
}