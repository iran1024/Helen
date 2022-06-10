import request from './request.js'

export default {
	
    async getAll() {
		const res = await request.get("/api/Address");

        return res.data;
	},

	async details(id) {
        const res = await request.get("/api/Address/" + id);

        return res.data;
    },

    async create(data) {
        const res = await request.post("/api/Address", data);

        return res.data;
    },

    async edit(data) {
        const res = await request.put("/api/Address", data);

        return res.data;
    },

    async delete(id) {
        const res = await request.delete("/api/Address/" + id);

        return res.data;
    }
}