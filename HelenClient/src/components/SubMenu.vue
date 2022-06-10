<template>
  <ul v-if="items">
    <template v-for="(item, i) of items">
      <li
        v-if="visible(item) && !item.separator"
        :key="item.label || i"
        :class="[
          {
            'layout-menuitem-category': root,
            'active-menuitem': activeIndex === i && !item.disabled && !item.to,
          },
        ]"
        role="none"
      >
        <template v-if="root">
          <div class="layout-menuitem-root-text" :aria-label="item.label">
            {{ item.label }}
          </div>
          <submenu
            :items="visible(item) && item.items"
            :routable="routable"
            @menuitem-click="$emit('menuitem-click', $event)"
          ></submenu>
        </template>
        <template v-else>
          <router-link class="menuitemtag"
            v-if="item.to"
            :to="item.to"
            :class="[item.class, 'p-ripple', { 'p-disabled': item.disabled }]"
            :style="item.style"
            @click="onMenuItemClick($event, item, i)"
            :target="item.target"
            :aria-label="item.label"
            exact
            role="menuitem"
            v-ripple
          >
            <FontAwesomeIcon
              :icon="item.icon"
              class="text-bluegray-600"
              size="1x"
              fixed-width
            />
            <span>{{ item.label }}</span>
            <i
              v-if="item.items"
              class="pi pi-fw pi-angle-down menuitem-toggle-icon"
            ></i>
            <Badge v-if="item.badge" :value="item.badge"></Badge>
          </router-link>
          <a class="menuitemtag"
            v-if="!item.to"
            :href="item.url || '#'"
            :style="item.style"
            :class="[item.class, 'p-ripple', { 'p-disabled': item.disabled }]"
            @click="onMenuItemClick($event, item, i)"
            :target="item.target"
            :aria-label="item.label"
            role="menuitem"
            v-ripple
          >
            <FontAwesomeIcon
              :icon="item.icon"
              class="text-bluegray-600"
              size="1x"
              fixed-width
            />
            <span>{{ item.label }}</span>
            <i
              v-if="item.items"
              class="pi pi-fw pi-angle-down menuitem-toggle-icon"
            ></i>
            <Badge v-if="item.badge" :value="item.badge"></Badge>
          </a>
          <transition name="layout-submenu-wrapper">
            <submenu
              v-show="activeIndex === i"
              :items="visible(item) && item.items"
              :routable="routable"
              @menuitem-click="$emit('menuitem-click', $event)"
            ></submenu>
          </transition>
        </template>
      </li>
      <li
        class="p-menu-separator"
        :style="item.style"
        v-if="visible(item) && item.separator"
        :key="'separator' + i"
        role="separator"
      ></li>
    </template>
  </ul>
</template>
<script>

export default {
  name: "submenu",
  props: {
    items: Array,
    routable: {
      type: Boolean,
      default: true,
    },
    root: {
      type: Boolean,
      default: false,
    },
  },  
  data() {
    return {
      activeIndex: null,      
    };
  },
  methods: {
    onMenuItemClick(event, item, index) {          
      if (item.disabled) {
        event.preventDefault();
        return;
      }

      if (!item.to && !item.url) {
        event.preventDefault();
      }

      if (item.command) {
        item.command({ originalEvent: event, item: item });
      }

      if (index === undefined) {
        if (!this.routable) {          
          let staticModuleMenu = document.getElementsByClassName('layout-sidebar-sub')[0].children[0].children[0].children[0].children[1];          
          Array.from(staticModuleMenu.children).forEach(element => {
            let target = element.children[0];
            if (target.ariaLabel === item){              
              this.activeMenuItem(target);
            }
          });
        }
      } else{
        this.activeIndex = index === this.activeIndex ? null : index;

        if (!this.routable) {
          let e = event.srcElement;
          this.activeMenuItem(e);
        }
      }

      this.$emit("menuitem-click", {
        originalEvent: event,
        item: item,
        self: index !== undefined
      });           
    },
    visible(item) {
      return typeof item.visible === "function"
        ? item.visible()
        : item.visible !== false;
    },
    activeMenuItem(e) {
      let staticModuleMenu = document.getElementsByClassName('layout-sidebar-sub')[0].children[0].children[0].children[0].children[1]; 

      Array.from(staticModuleMenu.children).forEach(element => {
        let target = element.children[0];
        if (target.className.indexOf('router-link-exact-active') !== -1){
          this.removeClass(target, 'router-link-exact-active');
        }
      });

      if (e.__vnode.type != "a") {
        let aTag = this.findATag(e);
        this.addClass(aTag, "router-link-exact-active");        
      } else {
        this.addClass(e, "router-link-exact-active");        
      }
    },
    addClass(element, className) {
      if (element.classList) element.classList.add(className);
      else element.className += " " + className;
    },
    removeClass(element, className) {
      if (element === null) return;
      if (element.classList) element.classList.remove(className);
      else
        element.className = element.className.replace(
          new RegExp(
            "(^|\\b)" + className.split(" ").join("|") + "(\\b|$)",
            "gi"
          ),
          " "
        );
    },
    findATag(e) {
      if (e.__vnode.type === "a") {
        return e;
      } else if (e.parentElement.__vnode.type === "a") {
        return e.parentElement;
      } else {
        return this.findATag(e.parentElement);
      }
    },
  },
};
</script>