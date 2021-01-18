import mitt from 'mitt'
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

// Event Busを使用しようと思ったが、vue 3.0から Vue.$on が廃止され、公式で mitt, tiny-emitter を使用するようにドキュメントに記載してあるので、それに倣った。
// https://v3.vuejs.org/guide/migration/events-api.html#migration-strategy
const emitter = mitt()
let manuallyClosed = false
let connection = null
let started = false

export default ({ $axios }, inject) => {
  // questionHubをpluginとしてNuxtにインジェクトする。これにより、Nuxtコード上でcontextから呼び出しが可能になる。
  inject('questionHub', {
    on: emitter.on,
    emit: emitter.emit,
    start: async (jwtToken) =>
      await startSignalr($axios.defaults.baseURL, jwtToken),
    stop: () => stopSignalr(),
    questionOpened(questionId) {
      if (!started) return
      try {
        return connection.invoke('JoinQuestionGroup', questionId)
      } catch (e) {
        console.error(e)
      }
    },
    questionClosed(questionId) {
      if (!started) return
      try {
        return connection.invoke('LeaveQuestionGroup', questionId)
      } catch (e) {
        console.error(e)
      }
    },
    sendMessage(message) {
      if (!started) return
      try {
        return connection.invoke('SendLiveChatMessage', message)
      } catch (e) {
        console.error(e)
      }
    },
  })
}

async function startSignalr(url, jwtToken) {
  connection = new HubConnectionBuilder()
    .withUrl(
      `${url}/question-hub`,
      jwtToken ? { accessTokenFactory: () => jwtToken } : null
    )
    .configureLogging(LogLevel.Information)
    .build()

  async function start() {
    const MAX_TRY = 10
    const RETRY_INTERVAL = 5000

    for (let i = 0; i < MAX_TRY; i++) {
      try {
        await connection.start()
        started = true
        break
      } catch (e) {
        console.error('Failed to connect with hub', e)
        if (i === MAX_TRY - 1) {
          console.error(`${MAX_TRY} times failed.`)
        }

        // retry on interval.
        await new Promise((resolve, reject) =>
          setTimeout(resolve, RETRY_INTERVAL)
        )
      }
    }
  }

  // define events
  connection.on('QuestionAdded', (question) => {
    emitter.emit('question-added', question)
  })

  connection.onclose(async () => {
    if (!started) return
    if (!manuallyClosed) await start()
  })

  await start()
}

async function stopSignalr() {
  if (!started) return

  manuallyClosed = true
  await connection.stop()
  started = false
}
