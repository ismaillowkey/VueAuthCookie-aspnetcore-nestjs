<template>
  <b-overlay :show="show" rounded="sm" variant="transparent">
    <div class="vue-tempalte">
      <form @submit.prevent="submit">
        <h3>Sign Up</h3>
        <div class="row">
          <div class="col">
            <div class="form-group">
              <label>Firts Name</label>
              <input
                v-model="firstName"
                type="text"
                class="form-control form-control-xs"
                required
              />
            </div>
          </div>
          <div class="col">
            <div class="form-group">
              <label>Last Name</label>
              <input
                v-model="lastName"
                type="text"
                class="form-control form-control-xs"
                required
              />
            </div>
          </div>
        </div>
        <div class="form-group">
          <label>Email address</label>
          <input
            v-model="email"
            type="email"
            class="form-control form-control-xs"
            required
          />
        </div>

        <div class="form-group">
          <label for="inputsm">Password</label>
          <input
            v-model="password"
            type="password"
            class="form-control form-control-xs"
            required
          />
        </div>

        <div class="form-group">
          <label>Password Confirm</label>
          <input
            v-model="passwordConfirm"
            type="password"
            class="form-control form-control-xs"
            required
          />
        </div>

        <button type="submit" class="btn btn-dark btn-lg btn-block">
          Sign Up
        </button>

        <p class="forgot-password text-right">
          Already registered
          <router-link :to="{ name: 'login' }">sign in?</router-link>
        </p>
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
      show: false as boolean,
      firstName: "" as string,
      lastName: "" as string,
      email: "" as string,
      password: "" as string,
      passwordConfirm: "" as string,
    };
  },
  methods: {
    async submit() {
      this.show = true;
      if (this.password !== this.passwordConfirm) {
        this.$swal({
          icon: "error",
          title: "Oops...",
          text: "Password do not match",
        });
      } else {
        await axios
          .post("auth/register", {
            first_name: this.firstName,
            last_name: this.lastName,
            email: this.email,
            password: this.password,
            password_confirm: this.passwordConfirm,
          })
          // eslint-disable-next-line @typescript-eslint/no-unused-vars
          .then((response) => {
            this.$swal({
              icon: "success",
              title: "success",
              text: "Register success, please login",
            });
            this.$router.push("/login");
          })
          .catch((error) => {
            console.log('error')
            console.log(error.response)
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
      }
      this.show = false;
    },
  },
});
</script>