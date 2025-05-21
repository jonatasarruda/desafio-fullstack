// src/store/modules/snackbar.js
const state = {
  show: false,
  message: '',
  color: 'info', // Default color: 'success', 'info', 'warning', 'error'
  timeout: 3000, // Default timeout in ms
};

const mutations = {
  SHOW_SNACKBAR(state, payload) {
    state.message = payload.message;
    state.color = payload.color || 'info';
    state.timeout = payload.timeout === undefined ? 3000 : payload.timeout; // Allow timeout 0 for persistent
    state.show = true;
  },
  HIDE_SNACKBAR(state) {
    state.show = false;
    // Não resetamos a mensagem/cor aqui para que não pisquem se o snackbar for reaberto rapidamente
  },
};

const actions = {
  showSnackbar({ commit }, payload) {
    commit('SHOW_SNACKBAR', payload);
  },
  // HIDE_SNACKBAR é geralmente chamado pelo próprio componente v-snackbar através do v-model
};

export default {
  namespaced: true, // Importante para evitar conflitos de nome
  state,
  mutations,
  actions,
};
