import axios from 'axios';
import Vue from 'vue'
import Vuex, { Commit } from 'vuex'
import createPersistedState from "vuex-persistedstate";

Vue.use(Vuex)

export default new Vuex.Store({
  plugins: [createPersistedState()],
  state: {
    auth: false
  },
  mutations: {
    setAuth: (state: {auth: boolean}, auth: boolean) => state.auth = auth
    
  },
  actions: {
    setAuth: ({commit}: {commit: Commit}, auth: boolean) => commit('setAuth', auth),
    refreshToken: async ({ state, commit }) => {
      const refreshUrl = "auth/refreshToken";
      try {
        await axios
          .get(refreshUrl)
          .then(response => {
            if (response.status === 200) {
              console.log('refresh token has refreshed')
            }
          });
      } catch (e) {
        console.log('refresh token error')
        console.log(e);
      }
    },
  },
  modules: {
  }
})
