<template>
  <v-card>
    <v-row no-gutters>
      <v-col cols="1" class="mt-4">
        <v-row justify="center">
          <v-col cols="7" class="text-center"
            ><v-icon>mdi-chevron-up</v-icon></v-col
          >
          <v-col cols="7" class="mr-mid-2 text-center">{{
            question.score
          }}</v-col>
          <v-col cols="7" class="text-center"
            ><v-icon>mdi-chevron-up mdi-rotate-180</v-icon></v-col
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
export default {
  name: 'QuestionPreview',
  props: {
    question: {
      type: Object,
      required: true,
    },
  },
  methods: {
    viewQuestion(id) {
      this.$router.push(`/question/${id}`)
    },
  },
}
</script>

<style scoped></style>
