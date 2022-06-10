<template>
	<div class="layout-menu-container">
		<SubMenu :items="model" :routable="routable" class="layout-menu" :root="true" @menuitem-click="onMenuItemClick" @keydown="onKeyDown" ref="refSubMenu" />		
	</div>
</template>

<script>
import { ref } from '@vue/reactivity';
import SubMenu from './SubMenu';

export default {
	props: {
		model: Array,
		routable: {
			type: Boolean,
			default: true
		},
	},
	setup() {
		const refSubMenu = ref();
		return {
			refSubMenu,
		}
	},
  methods: {
    onMenuItemClick(event) {
        this.$emit('menuitem-click', event);
    },
		onKeyDown(event) {
			const nodeElement = event.target;
			if (event.code === 'Enter' || event.code === 'Space') {
				nodeElement.click();
				event.preventDefault();
			}
		},
  },
	computed: {
		darkTheme() {
			return this.$appState.darkTheme;
		}
	},
	components: {
		'SubMenu': SubMenu,
	}
}
</script>