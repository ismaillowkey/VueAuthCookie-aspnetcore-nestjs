	
<template>
  <b-overlay :show="show" rounded="sm" variant="transparent">
    <div class="vue-tempalte">
      <form @submit.prevent="submit">
        <h3>Reset your password</h3>

        <div class="form-group">
          <label>Enter new password</label>
          <input
            v-model="password"
            type="password"
            class="form-control form-control-md"
            required
          />
        </div>

        <div class="form-group">
          <label>Enter again your password</label>
          <input
            v-model="passwordConfirm"
            type="password"
            class="form-control form-control-md"
            required
          />
        </div>

        <button type="submit" class="btn btn-dark btn-lg btn-block">
          Submit
        </button>
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
      password: "" as string,
      passwordConfirm: "" as string,
      show: false as boolean,
    };
  },
  methods: {
    async submit() {
      this.show = true;
      await axios
        .post("auth/reset", {
          email: this.$route.params.email,
          token: this.$route.params.token,
          password: this.password,
          password_confirm: this.passwordConfirm,
        })
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        .then((response) => {
          this.$swal({
            icon: "success",
            title: "success",
            text: "password has been reset, please login",
          });
          this.$router.push("/login");
        })
        .catch((error) => {
          if (error.response) {
            this.$swal({
              icon: "error",
              title: "Oops...",
              text: error.response.data.error,
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