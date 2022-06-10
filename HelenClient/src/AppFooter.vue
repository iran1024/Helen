<template>
	<div class="layout-footer">
		<img alt="Logo" :src="footerImage()" height="20" class="mr-2" />
		<span class="font-medium ml-2">Copyright © 艾普阳科技（深圳）有限公司</span>
	</div>
</template>

<script>
	export default {
		name: "AppFooter",
		methods: {
			footerImage() {
				return this.$appState.darkTheme ? 'images/logo.png' : 'images/logo.png';
			}
		},
		computed: {
			darkTheme() {
				return this.$appState.darkTheme;
			}
		}
	}
</script>