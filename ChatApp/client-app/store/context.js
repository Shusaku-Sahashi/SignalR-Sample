const CookieJwtKey = 'so-signal.jwt.cookie'

export const state = {
  profile: {},
  jwtToken: null,
}

export const getters = {
  isAuthenticated(state) {
    console.log(state.profile.name && state.profile.email)
    return state.profile.name && state.profile.email
  },
}

export const mutations = {
  setProfile(state, profile) {
    state.profile = profile
  },
  setJwt(state, jwtToken) {
    state.jwtToken = jwtToken
    console.log(jwtToken)
    this.$cookies.set(CookieJwtKey, jwtToken, {
      path: '/',
      maxAge: 60 * 60 * 24 * 7,
      sameSite: 'strict',
    })
    console.log(this.$cookies.getAll())
  },
  restoreJwt(state) {
    const jwt = this.$cookies.get(CookieJwtKey)
    if (!jwt) return false

    state.jwtToken = jwt
    return true
  },
}

export const actions = {
  async login({ commit }, credentials) {
    const res = await this.$axios.$post('account/token', credentials)
    const profile = res
    const token = res.token
    delete profile.token

    commit('setProfile', profile)
    commit('setJwt', token)
  },
}
