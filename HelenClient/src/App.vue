<template>
  
    <div :class="containerClass" @click="onWrapperClick" @routed="onRouted">
      <AppTopbar @menu-toggle="onMenuToggle" />
      <div class="layout-sidebar scroll-bar" @click="onSidebarClick">
        <Menu :model="menu" :routable="true" @menuitem-click="onMenuItemClick" />
      </div>

      <div :class="mainContainerClass">
        <div class="layout-main">
          <router-view  @routed="onRouted"/>
        </div>
        <AppFooter />
      </div>

      <AppConfig :layoutMode="layoutMode" @layout-change="onLayoutChange" hidden />
      <transition name="layout-mask">
        <div
          class="layout-mask p-component-overlay"
          v-if="mobileMenuActive || overlayMenuActive"
        ></div>
      </transition>
    </div>

</template>

<script>
import AppTopbar from "./AppTopbar.vue";
import Menu from "./components/Menu.vue";
import AppConfig from "./AppConfig.vue";
import AppFooter from "./AppFooter.vue";

export default {
  emits: ["change-theme"],
  data() {
    return {
      layoutMode: "static",
      forceOverlay: false,
      forceOverlayHasMenu: false,
      staticMenuInactive: false,
      overlayMenuActive: false,   
      mobileMenuActive: false,
      menu: [
        {
          label: "",
          items: [
            {
              label: "控制台",
              icon: ["fas", "house-chimney"],
              to: "/dashboard",
            },
            {
              label: "测试",
              icon: ["fas", "flask"],              
              items: [
                {
                  label: "测试主页",
                  icon: ["fas", "vial"],
                  to: "/testhome",                  
                },
                {
                  label: "Bug",
                  icon: ["fas", "bug"],
                  to: "/bug",
                  overlayMenu: true,
                },
                {
                  label: "测试用例",
                  icon: ["fas", "list-ul"],
                  to: "/testcase",
                  overlayMenu: true,
                },
                {
                  label: "测试单",
                  icon: ["fas", "note-sticky"],
                  to: "/testtask",
                  overlayMenu: true,
                },
                {
                  label: "测试报告",
                  icon: ["fas", "chart-line"],
                  to: "/testreport",
                  overlayMenu: true,
                },
              ],
            },
          ],
        },
        {
          label: "Live Demo",
          items: [
            {
              label: "Free Blocks",
              icon: ["fas", "bug"],
              to: "/blocks",
              badge: "NEW",
            },
            {
              label: "All Blocks",
              icon: ["fas", "bug"],
              url: "https://www.primefaces.org/primeblocks-vue",
              target: "_blank",
            },
          ],
        },
        {
          label: "Get Started",
          items: [
            {
              label: "Documentation",
              icon: ["fas", "bug"],
              command: () => {
                window.location = "#/documentation";
              },
            },
            {
              label: "View Source",
              icon: ["fas", "bug"],
              command: () => {
                window.location = "https://github.com/primefaces/sakai-vue";
              },
            },
          ],
        },
      ],
    };
  },
  watch: {
    $route() {
      this.menuActive = false;
      this.$toast.removeAllGroups();
    },   
  },
  methods: {
    onWrapperClick() {
      if (!this.menuClick) {        
        this.overlayMenuActive = false;
        this.mobileMenuActive = false;
      }

      this.menuClick = false;
    },
    onMenuToggle() {
      this.menuClick = true;

      if (this.isDesktop()) {        
          if (this.forceOverlay === true) {
            if(this.mobileMenuActive === true) {
              this.overlayMenuActive = true;
            }

            this.overlayMenuActive = !this.overlayMenuActive;
					  this.mobileMenuActive = false;
          } 
          else if (this.layoutMode === 'static' && this.forceOverlay === false) {
              this.staticMenuInactive = !this.staticMenuInactive;
          }
      } else {
        this.mobileMenuActive = !this.mobileMenuActive;
      }

      event.preventDefault();
    },
    onSidebarClick() {
      this.menuClick = true;
    },
    onMenuItemClick(event) {
      if (event.item && !event.item.items) {
        this.overlayMenuActive = false;        
        this.mobileMenuActive = false;
      }
    },
    onLayoutChange(layoutMode) {
      this.layoutMode = layoutMode;
    },
    addClass(element, className) {
      if (element.classList) element.classList.add(className);
      else element.className += " " + className;
    },
    removeClass(element, className) {
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
    isDesktop() {
      return window.innerWidth >= 992;
    },
    onRouted(overlay, menu) {      
      if (overlay === true)
        this.forceOverlay = true;
      else if (overlay === false)
        this.forceOverlay = false;
      if (menu === true) {
       this.forceOverlayHasMenu = true;
      }                      
      else if (menu === false) {
        this.forceOverlayHasMenu = false;  
      }
    }
  },
  computed: {
    containerClass() {
      return [
        "layout-wrapper",
        {          
          'layout-overlay': this.forceOverlay === true,
          "layout-static": this.layoutMode === "static" && this.forceOverlay === false,
          "layout-static-sidebar-inactive":
            this.staticMenuInactive && this.layoutMode === "static" && this.forceOverlay === false,          
          "layout-mobile-sidebar-active": this.mobileMenuActive,
          "layout-overlay-sidebar-active": this.overlayMenuActive && this.forceOverlay === true,
          "p-input-filled": this.$primevue.config.inputStyle === "filled",
          "p-ripple-disabled": this.$primevue.config.ripple === false,
        },
      ];
    },
    mainContainerClass() {
      return [
        "layout-main-container",
        {
          "layout-container-offset": this.forceOverlayHasMenu === true,
        },
      ];
    },
    logo() {
      return this.$appState.darkTheme ? "images/logo.png" : "images/logo.png";
    },
  },
  beforeUpdate() {
    if (this.mobileMenuActive)
      this.addClass(document.body, "body-overflow-hidden");
    else this.removeClass(document.body, "body-overflow-hidden");
  },
  mounted() {
    window.onresize = (event) => {
      if (document.body.clientWidth <= 991 && this.overlayMenuActive){
        this.overlayMenuActive = false;   
      }
    };
  },
  components: {
    AppTopbar: AppTopbar,
    Menu: Menu,
    AppConfig: AppConfig,
    AppFooter: AppFooter,
  },
};
</script>

<style lang="scss">
@import "./App.scss";
</style>