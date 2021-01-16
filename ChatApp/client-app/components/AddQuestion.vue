<template>
  <v-dialog v-model="dialog" width="30%">
    <template v-slot:activator="{ on, attr }">
      <v-btn
        class="primary text-capitalize"
        :disabled="!isAuthenticated"
        v-on="on"
        v-bind="attr"
        ><v-icon class="mr-1">mdi-help-circle</v-icon>Add Question</v-btn
      >
    </template>
    <v-card>
      <v-card-title>Add Queston</v-card-title>
      <v-container>
        <v-container class="pb-0">
          <v-row justify="center" no-gutters>
            <v-col cols="11">
              <v-text-field
                v-model="question.title"
                placeholder="Title"
                outlined
              ></v-text-field>
            </v-col>
            <v-col cols="11">
              <v-textarea
                v-model="question.body"
                placeholder="Text"
                outlined
                dense
              ></v-textarea>
            </v-col>
          </v-row>
        </v-container>
      </v-container>
      <v-card-actions>
        <v-container>
          <v-row justify="end">
            <v-col cols="auto">
              <v-btn
                @click="send"
                :loading="loading"
                class="primary"
                :disabled="!canSend"
                >Send</v-btn
              >
            </v-col>
            <v-col cols="auto">
              <v-btn @click="cancel">Cancel</v-btn>
            </v-col>
          </v-row>
        </v-container>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'AddQuestion.vue',
  data() {
    return {
      dialog: false,
      loading: false,
      question: {
        title: '',
        body: '',
      },
    }
  },
  computed: {
    canSend() {
      return this.question.title && this.question.body
    },
    ...mapGetters('context', ['isAuthenticated']),
  },
  methods: {
    async send() {
      this.loading = true
      const res = await this.$axios.$post('/api/question', this.question)
      this.$emit('question-added', res)

      this.cancel()
      this.loading = false
    },
    cancel() {
      this.question.title = ''
      this.question.body = ''
      this.dialog = false
    },
  },
}
</script>

<style scoped></style>
