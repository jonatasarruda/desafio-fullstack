// src/store/modules/snackbar.js
const state = {
  show: false,
  message: '',
  color: 'info',
  timeout: 3000, 
};

const mutations = {
  SHOW_SNACKBAR(state, payload) {
    state.message = payload.message;
    state.color = payload.color || 'info';
    state.timeout = payload.timeout === undefined ? 3000 : payload.timeout; 
    state.show = true;
  },
  HIDE_SNACKBAR(state) {
    state.show = false;

  },
};

const actions = {
  showSnackbar({ commit }, payload) {
    commit('SHOW_SNACKBAR', payload);
  },

};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
};
