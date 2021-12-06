	
<template>
  <b-overlay :show="show" rounded="sm" variant="transparent">
    <div class="vue-tempalte">
      <form @submit.prevent="submit">
        <h3>Sign In</h3>

        <div class="form-group">
          <label>Email</label>
          <input
            v-model="email"
            type="email"
            class="form-control form-control-md"
            required
          />
        </div>

        <div class="form-group">
          <label>Password</label>
          <input
            v-model="password"
            type="password"
            class="form-control form-control-md"
            required
          />
        </div>

        <button type="submit" class="btn btn-dark btn-lg btn-block">
          Sign In
        </button>

        <p class="forgot-password text-right mt-2 mb-4">
          <router-link to="/forgot-password">Forgot password ?</router-link>
        </p>

        <div class="social-icons">
          <ul>
            <li>
              <a href="#"><i class="fa fa-google"></i></a>
            </li>
            <li>
              <a href="#"><i class="fa fa-facebook"></i></a>
            </li>
            <li>
              <a href="#"><i class="fa fa-twitter"></i></a>
            </li>
          </ul>
        </div>
      </form>
    </div>
  </b-overlay>
</template>
 
<script lang='ts'>
import axios from "axios";
import { defineComponent } from "@vue/composition-api";

export default defineComponent({
  data() {
    return {
      email: "" as string,
      password: "" as string,
      show: false as boolean,
    };
  },
  methods: {
    async submit() {
      this.show = true;
      await axios
        .post("auth/login", {
          email: this.email,
          password: this.password,
        })
        .then((response) => {
          this.$swal({
            icon: "success",
            title: "success",
            text: `welcome ${response.data.data.first_name} ${response.data.data.last_name}`,
          });
          this.$router.push("/");
        })
        .catch((error) => {
          if (error.response) {
            this.$swal({
              icon: "error",
              title: "Oops...",
              text: error.response.data.message,
            });
          } else if (error.request) {
            // "The request was made but no response was received"
            this.$swal({
              icon: "error",
              title: "Oops...",
              text: "Network error, cannot connect to server, please try again",
            });
          } else {
            // "Something happened in setting up the request that triggered an Error"
            this.$swal({
              icon: "error",
              title: "Oops...",
              text: error.message,
            });
          }
        });
      this.show = false;
    },
  },
});
</script>