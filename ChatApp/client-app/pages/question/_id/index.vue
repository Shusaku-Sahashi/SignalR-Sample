<template>
  <v-row justify="center" class="mt-10">
    <v-col cols="7">
      <question-preview :question="question"></question-preview>
    </v-col>
    <v-col cols="7">
      <nuxt-link to="/">Back To Top</nuxt-link>
    </v-col>
  </v-row>
</template>

<script>
export default {
  name: 'QuestionViewer',
  async asyncData({ params, $axios }) {
    const question = await $axios.$get(`/api/question/${params.id}`)
    console.log(question)
    return { question }
  },
}
</script>

<style scoped></style>
