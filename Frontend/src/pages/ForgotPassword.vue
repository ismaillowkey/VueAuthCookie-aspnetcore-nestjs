<template>
<b-overlay :show="show" rounded="sm" variant="transparent">
    <div class="vue-tempalte">
        <form @submit.prevent="submit">
            <h3>Forgot Password</h3>
 
            <div class="form-group">
                <label>Email address</label>
                <input v-model="email" type="email" class="form-control form-control-lg" />
            </div>
 
            <button type="submit" class="btn btn-dark btn-lg btn-block">Reset password</button>
        </form>
    </div>
</b-overlay>
</template>
 
<script lang="ts">
import { defineComponent } from '@vue/composition-api'
import axios from 'axios'

export default defineComponent({
    data() {
        return {
            email: '' as string,
            show: false as boolean
        }
    },
    methods: {
        async submit() {
            this.show = true;
            await axios.post('auth/forgot', {
                email: this.email
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            }).then((response) => {
                this.$swal({
                    icon: 'success',
                    title: 'success',
                    text: 'check your email to reset password',
                })
                this.$router.push('/')
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
        }
    }
})
</script>