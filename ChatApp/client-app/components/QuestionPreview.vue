<template>
  <v-card>
    <v-row no-gutters>
      <v-col cols="1" class="mt-4">
        <v-row justify="center" no-gutters>
          <v-col cols="7" class="text-center"
            ><v-btn
              icon
              small
              :disabled="!isAuthenticated"
              @click="onUpVote(question.id)"
              ><v-icon>mdi-chevron-up</v-icon></v-btn
            ></v-col
          >
          <v-col cols="7" class="text-center">{{ question.score }}</v-col>
          <v-col cols="7" class="text-center"
            ><v-btn
              icon
              small
              :disabled="!isAuthenticated"
              @click="onDownVote(question.id)"
              ><v-icon>mdi-chevron-up mdi-rotate-180</v-icon></v-btn
            ></v-col
          >
        </v-row>
      </v-col>
      <v-col cols="11">
        <v-card-title>
          <nuxt-link :to="`/question/${question.id}`">{{
            question.title
          }}</nuxt-link>
        </v-card-title>
        <v-card-text>
          <div v-html="this.$md.render(question.body)"></div>
        </v-card-text>
      </v-col>
    </v-row>
  </v-card>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'QuestionPreview',
  props: {
    question: {
      type: Object,
      required: true,
    },
  },
  created() {
    this.$questionHub.on('score-changed', this.onScoreChange)
  },
  computed: {
    ...mapGetters('context', ['isAuthenticated']),
  },
  methods: {
    onUpVote(id) {
      this.$axios.$patch(`api/question/${id}/upvote`)
    },
    onDownVote(id) {
      this.$axios.$patch(`api/question/${id}/downvote`)
    },
    onScoreChange({ questionId, score }) {
      if (this.question.id !== questionId) return
      this.question.score = score
    },
  },
}
</script>

<style scoped></style>
