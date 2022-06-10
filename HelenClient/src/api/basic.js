import request from './request'

export default {
	setToken(token) {
		request.defaults.headers.Authorization = `Bearer ${token}`;
	},
	
	async get(url, config) {
		const res = await request.get(url, config);

		return res.data;
	},
	
	async post(url, data, config) {
		const res = await request.post(url, data, config);

		return res.data;
	},
	
	async put(url, data, config) {
		const res = await request.put(url, data, config);

		return res.data;
	},
	
	async delete(url, config) {
		const res = await request.delete(url, config);

		return res.data;
	},
}