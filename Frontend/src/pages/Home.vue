<template>
    <div class="container">
        <h1>{{ message }}</h1>
    </div>
</template>

<script>
import { defineComponent } from '@vue/composition-api'
import axios from 'axios'

export default defineComponent({
    name: 'Home',
    data() {
        return {
            message: 'You are not logged in'
        }
    },
    async mounted() {
        try {
            const {data} = await axios.get('auth/user');
            console.log('data')
            console.log(data)
            this.message = `Hi ${data.data.first_name} ${data.data.last_name}`
            await this.$store.dispatch('setAuth', true)
        }
        catch(error) {
            await this.$store.dispatch('setAuth', false)
            this.$router.push('/login')
        }
    }
})
</script>

<style>

</style>