<template>
	<div class="layout-topbar">
		<router-link to="/" class="layout-topbar-logo">
			<img alt="Logo" :src="topbarImage()" />
			<span>Helen System</span>
		</router-link>
		<button class="p-link layout-menu-button layout-topbar-button" @click="onMenuToggle">
			<i class="pi pi-bars"></i>
		</button>

		<button class="p-link layout-topbar-menu-button layout-topbar-button"
			v-styleclass="{ selector: '@next', enterClass: 'hidden', enterActiveClass: 'scalein', 
			leaveToClass: 'hidden', leaveActiveClass: 'fadeout', hideOnOutsideClick: true}">
			<i class="pi pi-ellipsis-v"></i>
		</button>
		<ul class="layout-topbar-menu hidden lg:flex origin-top">
			<li>
				<button class="p-link layout-topbar-button" v-tooltip.bottom="'消息'">
					<i class="pi pi-calendar"></i>
					<span>消息</span>
				</button>
			</li>
			<li>
				<button class="p-link layout-topbar-button" v-tooltip.bottom="'设置'">
					<i class="pi pi-cog"></i>
					<span>设置</span>
				</button>
			</li>
			<li>				
				<button class="p-link layout-topbar-button" v-tooltip.bottom="'用户'">
					<i class="pi pi-user"></i>
					<span>用户</span>
				</button>				
			</li>
		</ul>
	</div>
</template>

<script>
export default {
	methods: {
		onMenuToggle(event) {
				this.$emit('menu-toggle', event);
		},
		topbarImage() {
			return this.$appState.darkTheme ? 'images/logo.png' : 'images/logo.png';
		}
	}	
}
</script>