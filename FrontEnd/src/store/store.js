import { createStore } from 'vuex';

export default createStore({
  state: {
    showSidebar: true,
    showNavbar: true,
  },
  mutations: {
    toggleSidebar(state) {
      state.showSidebar = !state.showSidebar;
    },
    toggleNavbar(state) {
      state.showNavbar = !state.showNavbar;
    },
    setSidebarVisibility(state, visibility) {
      state.showSidebar = visibility;
    },
    setNavbarVisibility(state, visibility) {
      state.showNavbar = visibility;
    },
  },
  actions: {
    toggleSidebar({ commit }) {
      commit('toggleSidebar');
    },
    toggleNavbar({ commit }) {
      commit('toggleNavbar');
    },
    setSidebarVisibility({ commit }, visibility) {
      commit('setSidebarVisibility', visibility);
    },
    setNavbarVisibility({ commit }, visibility) {
      commit('setNavbarVisibility', visibility);
    },
  },
  getters: {
    isSidebarVisible: (state) => state.showSidebar,
    isNavbarVisible: (state) => state.showNavbar,
  },
});
