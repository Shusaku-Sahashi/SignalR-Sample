<template>
  <div class="message-container">
    <div class="message-title">Title</div>
    <v-row
      class="message-wrapper ma-2"
      ref="multipane"
      align="end"
      no-gutters
      @mousedown="onMouseDown"
    >
      <v-col cols="12" class="mt-1 message-display">foo</v-col>
      <div class="multipane-resizer" />
      <v-col cols="12" class="mt-0 message-sender">hoge</v-col>
    </v-row>
  </div>
</template>
<script>
export default {
  layout: 'message',
  methods: {
    // ここを参考に作成 : https://github.com/yansern/vue-multipane
    onMouseDown({ target: resizer, pageX: initialPageX, pageY: initialPageY }) {
      if (resizer.className && resizer.className.match('multipane-resizer')) {
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

        const MIN_NEXT_PANE_SIZE = 8
        const MAX_NEXT_PANE_SIZE = 18
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
          }
        }

        const onMouseUp = () => {
          previousPane.style.height =
            // clientHeight: 表示域の縦幅を取得する。
            // https://syncer.jp/javascript-reference/element/clientwidth
            getResizeHeight(previousPane.clientHeight) + '%'
          nextPane.style.height = getResizeHeight(nextPane.clientHeight) + '%'

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
  height: 90%;
}
.message-title {
  height: 8%;
  background-color: orangered;
}
.message-display {
  height: 90%;
}
.message-sender {
  height: 8%;
  background-color: green;
}
.multipane-resizer {
  width: 100%;
  height: 3px;
  cursor: row-resize;
}
</style>
