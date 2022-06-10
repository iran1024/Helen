import request from './request.js'

export default {
  async getUsers() {
    const res = await request.get("/api/user");

    return res.data;
  },
}