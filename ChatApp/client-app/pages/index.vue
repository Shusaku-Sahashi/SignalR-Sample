<template>
  <v-container>
    <v-row justify="center">
      <v-col cols="auto">
        <h1>This total looks like Stack Overflow</h1>
      </v-col>
      <v-col cols="4">
        <div class="action-buttons">
          <v-row justify="end">
            <v-col cols="auto">
              <v-btn
                class="primary text-capitalize"
                :disabled="!isAuthenticated"
                ><v-icon class="mr-1">mdi-help-circle</v-icon>Add
                Question</v-btn
              >
            </v-col>
          </v-row>
        </div>
      </v-col>
    </v-row>

    <v-row v-for="question in questions" :key="question.id" justify="center">
      <v-col cols="7">
        <question-preview :question="question"></question-preview>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapGetters } from 'vuex'
import QuestionPreview from '~/components/QuestionPreview'

export default {
  components: { QuestionPreview },
  data() {
    return {
      questions: [],
    }
  },
  computed: {
    ...mapGetters('context', ['isAuthenticated']),
  },
  async created() {
    const questions = await this.$axios.$get('/api/question')
    if (questions !== false) this.questions = questions
  },
}
</script>
