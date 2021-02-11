<template>
  <div class="message-container">
    <v-toolbar>
      <v-toolbar-title>{{ roomName }}</v-toolbar-title>
    </v-toolbar>
    <v-row
      class="message-wrapper"
      ref="multipane"
      align="end"
      no-gutters
      @mousedown="onMouseDown"
    >
      <v-col cols="12" class="mt-1 message-display-container"
        ><v-row align="end" class="message-display-wrapper">
          <v-col cols="12" class="message-previewer py-0">
            <message-previewer></message-previewer> </v-col></v-row
      ></v-col>
      <div class="multipane-resizer">
        <div class="multipane-resizer-line"></div>
      </div>
      <v-col cols="12" class="mt-2 message-sender-wrapper">
        <v-row
          ><v-col cols="11"
            ><v-textarea
              filled
              no-resize
              outlined
              :rows="messageSender.rows"
              height="3"
              class="ml-10"
            />
          </v-col>
          <v-col cols="1">
            <v-btn class="primary">送信</v-btn>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </div>
</template>
<script>
export default {
  layout: 'message',
  data() {
    return {
      roomName: 'Room Name',
      messageSender: {
        rows: 10,
      },
    }
  },
  methods: {
    // ここを参考に作成 : https://github.com/yansern/vue-multipane
    onMouseDown({ target: resizer, pageX: initialPageX, pageY: initialPageY }) {
      if (resizer.className && resizer.className.match('multipane-resizer')) {
        // lineの方でEventが起きた場合は親要素を詰め直す。
        if (resizer.className.match('multipane-resizer-line')) {
          resizer = resizer.parentNode
        }
        // functionなどでthisの意味が変わってくるので、selfとして入れて定義しておく。
        const self = this
        // $elでコンポネートのルートを参照することが可能。(HTMLツリーの最上位に位置するルートは `html` タグです。
        // https://qiita.com/smkhkc/items/fefe0c6060978846a2b4
        // 下記で ref="multipane" を指定している対象を取得する。
        const container = self.$refs.multipane

        // `multipane-resizer の１つ前, 1つ後の兄弟要素を取得
        const previousPane = resizer.previousElementSibling
        const nextPane = resizer.nextElementSibling

        const { offsetHeight: initialPreviousPaneHeight } = previousPane
        const { offsetHeight: initialNextPaneHeight } = nextPane

        // DOMのstyleを参照することで、CSSの設定を取得することが可能。
        const { addEventListener, removeEventListener } = window

        const getResizeHeight = (initialSize, offset = 0) => {
          return ((initialSize + offset) / container.clientHeight) * 100
        }

        const MIN_NEXT_PANE_SIZE = 10
        const MAX_NEXT_PANE_SIZE = 20
        const onMouseMove = ({ pageY }) => {
          const nextPaneSize = getResizeHeight(
            initialNextPaneHeight,
            -(pageY - initialPageY)
          )

          if (
            MIN_NEXT_PANE_SIZE <= nextPaneSize &&
            nextPaneSize <= MAX_NEXT_PANE_SIZE
          ) {
            previousPane.style.height =
              getResizeHeight(initialPreviousPaneHeight, pageY - initialPageY) +
              '%'
            nextPane.style.height = nextPaneSize + '%'
            this.messageSender.rows = (nextPaneSize - MIN_NEXT_PANE_SIZE) / 3
          }
        }

        const onMouseUp = () => {
          previousPane.style.height =
            // clientHeight: 表示域の縦幅を取得する。
            // https://syncer.jp/javascript-reference/element/clientwidth
            getResizeHeight(previousPane.clientHeight) + '%'
          const nextPaneSize = getResizeHeight(nextPane.clientHeight)
          nextPane.style.height = nextPaneSize + '%'
          this.messageSender.rows = (nextPaneSize - MIN_NEXT_PANE_SIZE) / 3

          removeEventListener('mousemove', onMouseMove)
          removeEventListener('mouseup', onMouseUp)
        }

        addEventListener('mousemove', onMouseMove)
        addEventListener('mouseup', onMouseUp)
      }
    },
  },
}
</script>

<style scoped>
.message-container {
  height: 100%;
}
.message-wrapper {
  height: 85%;
}
.message-display-container {
  height: 90%;
}
.message-display-wrapper {
  height: 100%;
}
.message-sender-wrapper {
  height: 8%;
}
.multipane-resizer {
  width: 100%;
  height: 15px;
  cursor: row-resize;
}
.multipane-resizer-line {
  width: 100%;
  height: 5px;
  transform: translateY(5px);
  background-color: #222222;
}
</style>
