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
              <add-question @question-added="onQuestionAdded"></add-question>
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
import QuestionPreview from '@/components/QuestionPreview'
import AddQuestion from '@/components/AddQuestion'

export default {
  components: { QuestionPreview, AddQuestion },
  // asyncDataはcontextを引数に入れて呼び出す。その引数の中には、$axiosがあるので、これを使用可能。
  async asyncData({ $axios }) {
    const questions = await $axios.$get('/api/question')
    if (questions !== false) return { questions }
  },
  data() {
    return {
      dialog: false,
    }
  },
  created() {
    this.$questionHub.on('question-added', this.onQuestionAdded)
  },
  methods: {
    onQuestionAdded(question) {
      if (this.questions.some((q) => q.id === question.id)) return
      this.questions = [question, ...this.questions]
    },
  },
}
</script>
