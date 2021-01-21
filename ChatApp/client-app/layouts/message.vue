<template>
  <v-app>
    <app-navigation-bar></app-navigation-bar>
    <v-navigation-drawer app clipped>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="title">
            <v-row justify="space-between" align="center">
              <v-col cols="auto"> Rooms </v-col>
              <v-col cols="auto">
                <v-btn icon><v-icon>mdi-plus-circle-outline</v-icon></v-btn>
              </v-col>
            </v-row>
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-divider></v-divider>
      <v-list dense nav>
        <v-list-item v-for="item in items" :key="item.title" link>
          <v-list-item-icon>
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-icon>

          <v-list-item-content>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-main>
      <nuxt />
    </v-main>
  </v-app>
</template>

<script>
export default {
  layout: 'empty',
}
</script>

<style scoped></style>
