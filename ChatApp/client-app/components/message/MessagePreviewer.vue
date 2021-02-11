<template>
  <v-card>
    <v-card>
      <v-row justify="start" no-gutters>
        <v-col cols="1" class="message-preview-user-icon-wrapper">
          <v-avatar><v-img :src="icon"></v-img></v-avatar>
        </v-col>
        <v-col cols="11" class="message-preview-detail-container">
          <v-row
            justify="start"
            class="message-preview-detail-wrapper"
            no-gutters
          >
            <v-col class="message-preview-user-name" cols="12">
              <v-card-title class="pa-0">Inoue Taro</v-card-title>
            </v-col>
            <v-col class="message-preview-content" cols="12">
              <v-card-text class="ps-0"> {{ messageContent }} </v-card-text>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-card>
  </v-card>
</template>

<script>
export default {
  name: 'messagePreviewer',
  data() {
    return {
      icon: '/favicon.ico',
      messageContent: `
      今日暇？
      なに？
      `,
    }
  },
}
</script>

<style scoped>
.message-preview-user-icon-wrapper {
  display: flex;
  justify-content: center;
}
</style>
