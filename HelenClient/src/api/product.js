import request from './request.js'

export default {
  async getModules() {
    const res = await request.get("/api/product/module");

    return res.data;
  },
}