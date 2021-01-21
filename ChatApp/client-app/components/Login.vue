<template>
  <v-dialog v-model="dialog" width="20%">
    <template v-slot:activator="{ on, attrs }">
      <v-btn v-if="!login" color="grey" v-bind="attrs" v-on="on">Login</v-btn>
      <v-btn v-else color="grey">Logout</v-btn>
    </template>
    <template>
      <v-card>
        <v-toolbar><v-toolbar-title>Login</v-toolbar-title></v-toolbar>
        <v-container>
          <v-row justify="center" dense>
            <v-col cols="11">
              <v-alert
                v-show="isError"
                dense
                outlined
                prominent
                text
                type="error"
                >{{ errorMessage }}</v-alert
              >
            </v-col>
            <v-col cols="11">
              <v-text-field
                v-model="credential.email"
                label="Email"
                type="email"
              >
              </v-text-field>
            </v-col>
            <v-col cols="11">
              <v-text-field
                v-model="credential.password"
                label="Password"
                type="password"
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-container>
        <v-card-actions>
          <v-container>
            <v-row justify="end">
              <v-col cols="4">
                <v-btn @click="close">Cancel</v-btn>
              </v-col>
              <v-col cols="3">
                <v-btn @click="send">Send</v-btn>
              </v-col>
            </v-row>
          </v-container>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script>
export default {
  name: 'Login.vue',
  data() {
    return {
      errorMessage: '',
      dialog: false,
      credential: {
        email: null,
        password: null,
      },
      login: false,
    }
  },
  computed: {
    isError() {
      return !!this.errorMessage
    },
  },
  mounted() {
    this.login = !!this.$store.getters['context/isAuthenticated']
  },
  methods: {
    async send() {
      if (!this.credential.email && !this.credential.email) {
        this.errorMessage = 'email and password must not be empty.'
        return
      }

      try {
        await this.$store.dispatch('context/login', this.credential)
      } catch {
        this.errorMessage = 'request error occured.'
        return
      }

      this.errorMessage = null
      this.credential.email = null
      this.credential.password = null
      this.dialog = false
      this.login = true
    },
    close() {
      this.dialog = false
    },
  },
}
</script>
