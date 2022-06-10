import request from './request.js'

export default {
  async getBugs() {
    const res = await request.get("/api/bug");

    return res.data;
  },
}