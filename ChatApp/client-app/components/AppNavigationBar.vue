<template>
  <v-app-bar app clipped-left>
    <v-app-bar-title class="wrap-text">miniOS</v-app-bar-title>
    <v-toolbar-items class="ma-md-4">
      <v-btn
        v-for="link in links"
        :key="link.name"
        :to="link.path"
        text
        nuxt
        class="mx-md-1"
        >{{ link.name }}</v-btn
      >
    </v-toolbar-items>
    <v-spacer />
    <v-row justify="end" align="center">
      <v-col cols="auto">
        {{ profile.name }}
      </v-col>
      <v-col cols="auto">
        <login />
      </v-col>
    </v-row>
  </v-app-bar>
</template>

<script>
// mapGettersにはvuexのimportが必要。
import { mapGetters, mapState } from 'vuex'
import Login from '../components/Login.vue'

export default {
  name: 'AppNavigationBar',
  components: { Login },
  data() {
    return {
      links: [
        { name: 'Home', path: '/' },
        { name: 'Message', path: '/message' },
      ],
    }
  },
  mounted() {},
  computed: {
    ...mapGetters('context', ['isAuthenticated']),
    ...mapState('context', ['profile']),
  },
}
</script>

<style scoped></style>
