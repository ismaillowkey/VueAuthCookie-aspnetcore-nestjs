<template>
  <div class="vue-tempalte">
    <!-- Navigation -->
    <nav class="navbar shadow bg-white rounded justify-content-between flex-nowrap flex-row fixed-top">
      <div class="container">
        <router-link class="navbar-brand float-left" to='/'>
           Home
        </router-link>
        <div v-if="!auth">
          <ul  class="nav navbar-nav flex-row float-right">
            <li class="nav-item">
              <router-link class="nav-link pr-3" to="/login">Sign in</router-link>
            </li>
            <li class="nav-item">
              <router-link class="btn btn-outline-primary" to="/signup">Sign up</router-link>
            </li>
          </ul>
        </div>
        <div v-if="auth">
          <!-- <li class="nav-item"> -->
              <a href class="nav-link pr-3" @click="logout">Logout</a>
            <!-- </li> -->
        </div>
      </div>
    </nav>
 
    <!-- Main -->
    <div class="App">
      <div class="vertical-center">
        <div class="inner-block">
          <router-view />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api'
import axios from 'axios'

export default defineComponent({
  data() {
    return {
    }
  },
  computed:{
    auth() {
      return this.$store.state.auth;
    }
  },
  methods: {
    async logout() {
      await axios.post('auth/logout');
      this.$store.dispatch('setAuth', false)
      this.$swal({
          icon: 'success',
          title: 'success',
          text: 'logout success',
      })
    }
  }
})
</script>
