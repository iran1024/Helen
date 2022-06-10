<template>     
  <div class="layout-sidebar-sub scroll-bar ">
    <Menu :model="modulesMenu" :routable="false" @menuitem-click="onMenuItemClick" ref="refModulesMenu" />
  </div>
  <div class="layout-mobile-module-block">
    <Button id="mobile-module-menubtn" icon="pi pi-ellipsis-h" class="p-button-rounded p-button-text p-button" style="color:#6c757d" v-tooltip.right="'模块菜单'"
			v-styleclass="{ selector: '@next', enterClass: 'hidden', enterActiveClass: 'scalein', 
			leaveToClass: 'hidden', leaveActiveClass: 'fadeout', hideOnOutsideClick: true}" />
    <ul class="layout-mobile-module-menu hidden lg:flex origin-top scroll-bar">
      <li v-for="(item,index) in modules" :key="index">
        <button class="layout-mobile-button" @click="onMobileMenuItemClick($event, item, index)">
          <FontAwesomeIcon :icon="['fas', 'circle']" class="text-bluegray-600" size="xs" fixed-width style="margin-left: -2px;margin-right: 8px;" />
          <span>{{item}}</span>
        </button>        
      </li>
		</ul>
  </div>
  <div class="grid">
    <div class="col-12">
     <div class="card">
       <DataTable :value="bugs" :paginator="true" class="p-datatable-bugs dt-shadow" :rows="10" dataKey="id" sortField="Id" :sort-order="-1" removableSort :rowHover="true" v-model:filters="filters" filterDisplay="menu" :loading="loading" paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,20,50]" currentPageReportTemplate="显示 {first} ~ {last} 个Bug，共计 {totalRecords} 个Bug" :globalFilterFields="['Id', 'Module.Name', 'Title', 'Creator.Username', 'CreatedTime', 'Assignment.Username', 'Resolution', 'ActiveCount']" responsiveLayout="scroll" stateStorage="local" stateKey="dt-state-bug-local" @state-restore="onStateRestore" >
        <template #header>
          <div class="flex justify-content-between">
            <span class="p-input-icon-left">
              <i class="pi pi-search" />
              <InputText v-model="filters['global'].value" placeholder="关键词查询" style="border-radius: 12px 0 0 0;margin-left: -1px;backgroud-color:#f8f9fa;" class="tb-header-boundary-element" />              
            </span>
            <Button type="button" style="border-radius: 0 12px 0 0;" class="hoverbtn tb-header-boundary-element" @click="createNew">
              <FontAwesomeIcon :icon="['fas', 'circle-plus']" class="text-white" size="1x" fixed-width />
              <span class="ml-2 font-bold text-base line-height-1" style="">新建</span>
            </Button>
          </div>
        </template>
        <template #empty>
          没有符合要求的Bug。
        </template>
        <template #loading>
          加载Bug数据中，请稍等。
        </template>
        <Column header="ID" field="Id" :sortable="true" style="width: 4%;" headerStyle="white-space:nowrap;">
          <template #body="{data}">
            <span style="text-align: center">{{data.Id}}</span>
          </template>          
        </Column>
        <Column header="模块" field="Module.Name" :filterMatchModeOptions="modulesFilterOpts" :showFilterMenu="false" style="width: 3%;" headerStyle="white-space:nowrap;"  bodyStyle="white-space:nowrap;">
          <template #body="{data}">
            <span class="text-bluegray-300 text-sm">{{data.Module.Name}}</span>
          </template>
          <template #filter="{filterModel}">                        
            <Dropdown v-model="filterModel.value" :options="modules" placeholder="任意" class="p-column-filter" panelClass="popup-scrollable-panel" :showClear="true">
                <template #value="slotProps">
                    <span :class="'customer-badge status-' + slotProps.value" v-if="slotProps.value">{{slotProps.value}}</span>
                    <span v-else>{{slotProps.placeholder}}</span>
                </template>
                <template #option="slotProps">
                    <span>{{slotProps.option}}</span>
                </template>
            </Dropdown>                        
          </template>
        </Column>
        <Column header="Bug标题" field="Title" :filterMatchModeOptions="titleFilterOpts" style="max-width:22.5rem;" bodyClass="dt-td" headerStyle="white-space:nowrap;">
          <template #body="{data}">
            <span v-tooltip.top='`${data.Title}`'>{{data.Title}}</span>
          </template>
          <template #filter="{filterModel}">            
            <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="根据标题查询" />
          </template>
        </Column>
        <Column header="创建者" sortable filterField="Creator.Username" sortField="Creator.Username" :showFilterMatchModes="false" :filterMenuStyle="{'min-width': '13rem','max-width': '21rem'}" bodyClass="dt-td" style="width:5%;" headerStyle="white-space:nowrap;">
          <template #body="{data}">
            <img :src="data.Creator.Avatar" width="30" style="vertical-align: middle;border-radius: 30px;margin-right: 8px;" />
            <span class="image-text" style="vertical-align: middle">{{data.Creator.Username}}</span>
          </template>
          <template #filter="{filterModel}">
            <div class="mb-3 font-bold">选择用户</div>
            <MultiSelect v-model="filterModel.value" :options="users" optionLabel="Username" optionValue="Username" :filter="true" placeholder="任意" :showToggleAll="false" panelClass="popup-scrollable-panel" class="p-column-filter" emptyFilterMessage="无匹配项" >
              <template #value="slotProps">                        
                <div class="inline-flex align-items-center py-1 px-2 bg-primary text-primary mr-2" style="border-radius:12px" v-for="option of slotProps.value" :key="option.Id">
                  <img :src="getUserAvatar(option)" width="18" style="vertical-align: middle;border-radius: 18px;margin-right: 5px;" />
                  <div style="line-height: 18px">{{option}}</div>
                </div>                
              </template>
              <template #option="slotProps">
                <div class="p-multiselect-user-option">
                  <Avatar :image="slotProps.option.Avatar" class="mr-2" style="vertical-align: middle" shape="circle" />
                  <span class="image-text" style="vertical-align: middle">{{slotProps.option.Username}}</span>
                </div>
              </template>
            </MultiSelect>
          </template>
        </Column>
        <Column header="创建日期" field="CreatedTime" :filterMatchModeOptions="createdtimeFilterOpts" filterMatchMode="customDateIs" headerStyle="white-space:nowrap;"  sortable dataType="date" style="width:5%;">
          <template #body="{data}">
            {{formatDate(new Date(data.CreatedTime))}}
          </template>
          <template #filter="{filterModel}">
            <Calendar v-model="filterModel.value" dateFormat="yy-mm-dd" placeholder="yyyy-mm-dd" />
          </template>
        </Column>
        <Column header="指派给" sortable filterField="Assignment.Username" sortField="Assignment.Username" :showFilterMatchModes="false" :filterMenuStyle="{'min-width': '16rem','max-width': '21rem'}" bodyClass="dt-td" style="width:5%;" headerStyle="white-space:nowrap;">
          <template #body="{data}">
            <img :src="data.Assignment.Avatar" width="30" style="vertical-align: middle;border-radius: 30px;margin-right: 8px;" />
            <span class="image-text" style="vertical-align: middle">{{data.Assignment.Username}}</span>
          </template>
          <template #filter="{filterModel}">
            <div class="mb-3 font-bold">选择用户</div>
            <MultiSelect v-model="filterModel.value" :options="users" optionLabel="Username" optionValue="Username" :filter="true" placeholder="任意" panelClass="popup-scrollable-panel" class="p-column-filter" emptyFilterMessage="无匹配项" >
              <template #value="slotProps">
                <div class="inline-flex align-items-center py-1 px-2 bg-primary text-primary mr-2" style="border-radius:12px" v-for="option of slotProps.value" :key="option.Id">                  
                  <img :src="getUserAvatar(option)" width="18" style="vertical-align: middle;border-radius: 18px;margin-right: 5px;" />
                  <div style="line-height: 18px">{{option}}</div>
                </div>                
              </template>
              <template #option="slotProps">
                <div class="p-multiselect-user-option">
                  <Avatar :image="slotProps.option.Avatar" class="mr-2" style="vertical-align: middle" shape="circle" />
                  <span class="image-text" style="vertical-align: middle">{{slotProps.option.Username}}</span>
                </div>
              </template>
            </MultiSelect>
          </template>
        </Column>
        <Column header="解决方案" field="Resolution" :filterMatchModeOptions="resolutionFilterOpts" sortable style="width:4%;" headerStyle="white-space:nowrap;">
          <template #body="{data}">
            <span :class="'resolution-badge status-' + resolutionsMapper[data.Resolution]">{{data.Resolution}}</span>
          </template>
          <template #filter="{filterModel}">
            <Dropdown v-model="filterModel.value" :options="resolutions" optionValue="name" placeholder="任意" panelClass="popup-scrollable-panel" class="p-column-filter" :showClear="true">
                <template #value="slotProps">
                    <span v-if="slotProps.value" :class="'resolution-badge status-' + resolutionsMapper[slotProps.value]">{{slotProps.value}}</span>
                    <span v-else>{{slotProps.placeholder}}</span>
                </template>
                <template #option="slotProps">
                    <span :class="'resolution-badge status-' + slotProps.option.id">{{slotProps.option.name}}</span>
                </template>
            </Dropdown>                        
          </template>          
        </Column>
        <Column header="激活次数" field="ActiveCount" :sortable="true" :showFilterMatchModes="false" style="width: 4%" headerStyle="white-space:nowrap;">
          <template #body="{data}">
            <ProgressBar v-tooltip.top="{value: data.ActiveCount}" :value="data.ActiveCount * 5" :showValue="false" style="height:.5rem"></ProgressBar>
          </template>
          <template #filter={filterModel}>
              <Slider v-model="filterModel.value" range class="m-3" max="20"></Slider>
              <div class="flex align-items-center justify-content-between px-2">
                  <span>{{filterModel.value ? filterModel.value[0] : 0}}</span>
                  <span>{{filterModel.value ? filterModel.value[1] : 20}}</span>
              </div>
          </template>
        </Column>        
      </DataTable>
      <Dialog v-model:visible="bugDialog" :style="{width: '1000px'}" class="p-fluid" header="新建Bug" @hide="hideDialog" maximizable>
        <div class="field grid">
          <div class="col-8">
            <label for="Title" class="text-lg">标题</label>
            <InputText id="Title" v-model.trim="bug.Title" required="true" autofocus class="mt-2" :class="{'p-invalid': submitted && !bug.Title}" />
            <small id="title-validate" class="p-error" v-if="submitted && !bug.Title">需要填写Bug标题</small>            	
          </div>
          <div class="col-2">
            <label for="Severity" class="text-lg nowrap">严重程度</label>
            <Dropdown v-model="bug.Severity" id="Severity" :options="severities" class="mt-2">
              <template #value="slotProps">
                <span v-if="slotProps.value" :class="'severity-badge status-' + slotProps.value">{{slotProps.value}}</span>
                <span v-else class="severity-badge status-3">3</span>
              </template>
              <template #option="slotProps">
                  <span :class="'severity-badge status-' + slotProps.option">{{slotProps.option}}</span>
              </template>
            </Dropdown>            
          </div>
          <div class="col-2">
            <label for="Priority" class="text-lg nowrap">优先级</label>
            <Dropdown v-model="bug.Priority" id="Priority" :options="priorities" class="mr-2 mt-2">
              <template #value="slotProps">
                <span v-if="slotProps.value" :class="'priority-badge status-' + slotProps.value">{{slotProps.value}}</span>  
                <span v-else class="priority-badge status-3">3</span>                
              </template>
              <template #option="slotProps">
                  <span :class="'priority-badge status-' + slotProps.option">{{slotProps.option}}</span>
              </template>
            </Dropdown>
          </div>
        </div>
        <div class="field grid">
          <div class="col-3">
            <label for="Product" class="text-lg">所属产品</label>     
            <Dropdown v-model="bug.Product" id="Product" :options="products" class="mt-2">
            </Dropdown>       
          </div>
           <div class="col-3">
            <label for="Module" class="text-lg">所属模块</label>     
            <Dropdown  v-model="bug.Module" id="Module" :options="modules" class="mt-2">
            </Dropdown>       
          </div>
           <div class="col-3">
            <label for="AffectVersions" class="text-lg">影响版本</label>     
            <Dropdown  v-model="bug.AffectVersions" id="AffectVersions" :options="versions" class="mt-2">
            </Dropdown>       
          </div>
           <div class="col-3">
            <label for="Assignment" class="text-lg">当前指派</label>     
            <Dropdown  v-model="bug.Assignment" id="Assignment" :options="users" class="mt-2">
            </Dropdown>       
          </div>
        </div>
         <div class="field grid">
          <div class="col-3">
            <label for="Type" class="text-lg">Bug类型</label>     
            <Dropdown v-model="bug.Type" id="Type" :options="types" class="mt-2">
            </Dropdown>       
          </div>
           <div class="col-3">
            <label for="OS" class="text-lg">操作系统</label>     
            <Dropdown  v-model="bug.OS" id="OS" :options="oses" class="mt-2">
            </Dropdown>       
          </div>
           <div class="col-3">
            <label for="Browser" class="text-lg">浏览器</label>     
            <Dropdown  v-model="bug.Browser" id="Browser" :options="browsers" class="mt-2">
            </Dropdown>       
          </div>
           <div class="col-3">
            <label for="Assignment" class="text-lg">截止日期</label>     
            <Calendar class="mt-2"/>     
          </div>
        </div>

        <template #footer>
          <Button label="暂存" icon="pi pi-times" class="p-button-text" @click="temporaryStorage"/>
          <Button label="提交" icon="pi pi-check" class="p-button-text" @click="submitBug"/>
        </template>
      </Dialog>
     </div>
    </div>      
  </div>
  <transition name="layout-mask">
    <div class="layout-mask p-component-overlay" v-if="modalTag" ></div>
  </transition>
  <transition name="moudle-menu-mask">
    <div class="moudle-menu-mask p-component-overlay" v-if="masktag"></div>
  </transition>
</template>

<script>
import Menu from "../components/Menu";
import { getCurrentInstance, nextTick, onMounted, onUnmounted, ref } from 'vue';
import { FilterMatchMode, FilterOperator, FilterService } from 'primevue/api';
 
export default {
  components: {
    Menu: Menu,
  },
  emits: ["routed"],
  setup(props, {emit}) {    
    const api = getCurrentInstance().appContext?.app.config.globalProperties.$api;

    emit('routed', true, true);

    const modulesMenu = ref([{label: '', items: [{label: '所有模块', icon: ['fas', 'circle']}]}]);
    const bugs = ref();
    const modules = ref();
    const users = ref();
    const filters = ref({
      'global': { value: null, matchMode: FilterMatchMode.CONTAINS },    
      'Module.Name': {
        operator: FilterOperator.OR,
        constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
      },
      'Title': {
        operator: FilterOperator.AND,
        constraints: [{ value: null, matchMode: FilterMatchMode.CONTAINS }],
      },
      'Creator.Username': { value: null, matchMode: FilterMatchMode.IN },
      'CreatedTime': {
        operator: FilterOperator.AND,
        constraints: [{ value: null, matchMode: "customDateIs" }],
      },
      'Assignment.Username': { value: null, matchMode: FilterMatchMode.IN },
      'Resolution': {
        operator: FilterOperator.OR,
        constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
      },
      'ActiveCount':  { value: null, matchMode: FilterMatchMode.BETWEEN },
    });
    const modulesFilterOpts = [
      { label: "等于", value: FilterMatchMode.EQUALS },
      { label: "不等于", value: FilterMatchMode.NOT_EQUALS },
    ];
    const titleFilterOpts = [
      { label: "从...开始", value: FilterMatchMode.STARTS_WITH },
      { label: "包含", value: FilterMatchMode.CONTAINS },
      { label: "不包含", value: FilterMatchMode.NOT_CONTAINS },
      { label: "由...结束", value: FilterMatchMode.ENDS_WITH }
    ];
    const resolutionFilterOpts = [
      { label: "等于", value: FilterMatchMode.EQUALS },
      { label: "不等于", value: FilterMatchMode.NOT_EQUALS },
    ];
    const createdtimeFilterOpts = [
      { label: "日期是", value: "customDateIs" },
      { label: "日期不是", value: "customDateNotIs" },
      { label: "在...之前", value: "customDateBefore" },
      { label: "在...之后", value: "customDateAfter" },
    ];
    const resolutions = ref([
      {id: 1, name: "已解决"},
      {id: 2, name: "重复Bug"},
      {id: 3, name: "设计如此"},
      {id: 4, name: "不予解决"},
      {id: 5, name: "延期处理"},
      {id: 6, name: "外部原因"},
      {id: 7, name: "无法重现"},
    ]);
    const resolutionsMapper = {
      "已解决": 1,
      "重复Bug": 2,
      "设计如此": 3,
      "不予解决": 4,
      "延期处理": 5,
      "外部原因": 6,
      "无法重现": 7,
    };  
    const formatDate = (value) => {
      return value.toLocaleDateString('zh', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        hour12: false,
      }).replace(/\//g, '-');
		};

    const filterDateIs = (value, filter) => {
      if (filter === undefined || filter === null || (typeof filter === 'string' && filter.trim() === '')) {
        return true;
      }

      if (value === undefined || value === null) {
        return false;
      }

      return value.split(' ')[0] === formatDate(filter).split(' ')[0];
    };
    const filterDateIsNot = (value, filter) => {
      if (filter === undefined || filter === null || (typeof filter === 'string' && filter.trim() === '')) {
        return true;
      }

      if (value === undefined || value === null) {
        return false;
      }

      return value.split(' ')[0] !== formatDate(filter).split(' ')[0];
    };
    const filterDateBefore = (value, filter) => {
      if (filter === undefined || filter === null || (typeof filter === 'string' && filter.trim() === '')) {
        return true;
      }

      if (value === undefined || value === null) {
        return false;
      }

      return new Date(value.split(' ')[0]) < new Date(formatDate(filter).split(' ')[0]);
    };
    const filterDateAfter = (value, filter) => {
      if (filter === undefined || filter === null || (typeof filter === 'string' && filter.trim() === '')) {
        return true;
      }

      if (value === undefined || value === null) {
        return false;
      }

      return new Date(value.split(' ')[0]) > new Date(formatDate(filter).split(' ')[0]);
    };

    FilterService.register("customDateIs", filterDateIs);
    FilterService.register("customDateNotIs", filterDateIsNot);
    FilterService.register("customDateBefore", filterDateBefore);
    FilterService.register("customDateAfter", filterDateAfter);
            
    const loading = ref(true);

    const refModulesMenu = ref();
    const previousClickedElement = ref(null);
    const addClass = (element, className) => {
      if (element.classList) element.classList.add(className);
      else element.className += " " + className;
    };
    const removeClass = (element, className) => {      
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
    };
    const findSpanTag = (e) => {
      if (e.__vnode.type === "span") {
        return e;
      } else {        
        let result = null;
        Array.from(e.children).forEach(element => {    
          if (element.__vnode.type === "span"){            
            result = element;
          }
        });
        return result;
      }                
    };
    const activeMenuItem = (e) => {
      if (previousClickedElement.value != e) {
          removeClass(
            previousClickedElement.value,
            "exact-active"
          );
        }
        if (e.__vnode.type != "span") {
          let spanTag = findSpanTag(e);                  
          addClass(spanTag, "exact-active");
          previousClickedElement.value = spanTag;
        } else {
          addClass(e, "exact-active");
          previousClickedElement.value = e;
        }
    };
    const activeMenuItemByContent = (item) => {
      Array.from(document.getElementsByClassName('layout-mobile-module-menu')[0].children).forEach(element => {
        let target = element.children[0].children[1];
        
        if (target.innerHTML === item){
          activeMenuItem(target);
        }
      });
    };
    const setModulesFilter = (module) => {  
      filters.value = {
        'global': { value: null, matchMode: FilterMatchMode.CONTAINS },    
        'Module.Name': {
          operator: FilterOperator.OR,
          constraints: [{ value: module === '所有模块' ? null : module, matchMode: FilterMatchMode.EQUALS }],
        },
        'Title': {
          operator: FilterOperator.AND,
          constraints: [{ value: null, matchMode: FilterMatchMode.CONTAINS }],
        },
        'Creator.Username': { value: null, matchMode: FilterMatchMode.IN },
        'CreatedTime': {
          operator: FilterOperator.AND,
          constraints: [{ value: null, matchMode: "customDateIs" }],
        },
        'Assignment.Username': { value: null, matchMode: FilterMatchMode.IN },
        'Resolution': {
          operator: FilterOperator.OR,
          constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
        },
        'ActiveCount':  { value: null, matchMode: FilterMatchMode.BETWEEN },
      }
    };

    const onMobileMenuItemClick = (event, item, index) => { 
      if ((event && (index || index === 0)) && document.body.clientWidth <= 991)
        document.getElementById('mobile-module-menubtn').click();        
      let e = event?.srcElement;      
      if (index === undefined) {                   
        activeMenuItemByContent(item.label ? item.label : item);
      }
      else {        
        setModulesFilter(item);
        activeMenuItem(e);
        triggerMeunChildComponentEvent(event, item, index);        
      }
    };
    const onMenuItemClick = (eventData) => {      
      if (eventData.self) {
        setModulesFilter(eventData.item.label);
      }
      if (eventData.item) {
        onMobileMenuItemClick(eventData.originalEvent, eventData.item);        
      }
    };
    const triggerMeunChildComponentEvent = (event, item) => {     
      refModulesMenu.value.refSubMenu.onMenuItemClick(event, item);
    }

    const masktag = ref(false);
    const mutaitonCallback = (mutationsList, observer) => {      
      for (let mutation of mutationsList) {        
        if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
          let originNode = mutation.target;          
          if (originNode.className.indexOf('hidden') !== -1) {          
            masktag.value = false;
            return;
          } else {
            masktag.value = true;
            return;
          }
        }
      }
    };
    const observer = ref(new MutationObserver(mutaitonCallback));

    window.onresize = (event) => {
      console.log("todo://可视窗口大小监控有问题，后续跟进", masktag.value, modalTag.value);
      if (document.body.clientWidth > 991){         
        let target = document.getElementsByClassName('layout-mobile-module-menu')[0];
        if (!target.classList?.contains('hidden')) {
          target.classList.add('hidden');          
        }         
      }
    };
    
    const stateModuleFilterValue = ref();
    const onStateRestore = async (event) => {      
      let moduleFilterConstraintsValue = event.filters['Module.Name'].constraints[0].value;
      if (moduleFilterConstraintsValue !== null) {
        stateModuleFilterValue.value = moduleFilterConstraintsValue;        
      }
    };

    const getUserAvatar = (username) => {
      let user = users.value.find(user => user.Username === username)

      return user.Avatar;
    };
    const modalTag = ref(false);
    const bugDialog = ref(false);
    const bug = ref({});
    const submitted = ref(false);
    const createNew = () => {
      modalTag.value = true;
      bugDialog.value = true;
      submitted.value = false;
    };
    const hideDialog = () => {
      modalTag.value = false;
      bugDialog.value = false;
      submitted.value = false;
    };
    const submitBug = () => {
      submitted.value = true;
    };
    const severities = ref([1,2,3]);
    const priorities = ref([1,2,3]);
    
    onMounted(async() => {      
      if (stateModuleFilterValue.value !== undefined) {
        await nextTick(setTimeout(() => {
          let menu = document.getElementsByClassName('layout-sidebar-sub')[0].children[0].children[0].children[0].children[1];

          Array.from(menu.children).forEach(el => {
            if (el.children[0].children[1].innerHTML === stateModuleFilterValue.value) {
              el.children[0].click();
            }
          });          
        }, 100));          
      }
      modules.value = await api.product.getModules();
      modules.value.forEach(element => {
        modulesMenu.value[0].items.push({label: `${element}`, icon: ['fas', 'circle']});
      });
      modules.value.unshift('所有模块');

      bugs.value = await api.bug.getBugs();
      users.value = await api.user.getUsers();

      loading.value = false;    
      
      // 配置dom观察器
      const targetNode = document.getElementsByClassName('layout-mobile-module-menu')[0];
      const config = { attributes: true };
      
      observer.value.observe(targetNode, config);
    });    
            
    onUnmounted(async() => {
      observer.value.disconnect();
    });

    return {
      api,
      modules,
      modulesMenu,
      bugs,
      users,
      filters,
      resolutions,  
      resolutionsMapper,      
      loading, 
      formatDate,
      modulesFilterOpts,
      titleFilterOpts,
      resolutionFilterOpts,
      createdtimeFilterOpts,            
      onMenuItemClick,
      refModulesMenu,
      onMobileMenuItemClick,      
      masktag,
      observer,
      onStateRestore,
      getUserAvatar,
      modalTag,
      bugDialog,
      createNew,
      bug,
      submitted,
      hideDialog,
      submitBug,
      severities,
      priorities
    };
  },
};
</script>

<style lang="scss" scoped>
@import '../assets/styles/sass/badges.scss';
@import '../assets/styles/sass/_bug.scss';

img {
  vertical-align: middle;
}
::v-deep(.p-paginator) {
  .p-paginator-current {
    margin-left: auto;
  }
}

::v-deep(.p-progressbar) {
  height: .5rem;
  background-color: #D8DADC;

  .p-progressbar-value {
    background-color: #607D8B;
  }
}

::v-deep(.p-datepicker) {
  min-width: 25rem;

  td {
    font-weight: 400;
  }
}

::v-deep(.p-datatable.p-datatable-bugs) {   
  table-layout: fixed;
  word-break: break-all;  
  
  .p-datatable-tbody > tr {
    &:hover {
      background-color: #e9ecef;
      transition: background-color .6s;
    }
  }

  .p-datatable-tbody > tr > td {    
    font-weight: 500;
    cursor: pointer;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }

  .p-datatable-header {    
    height: 42px;  
    padding: 0;
  }

  .p-datatable-thead > tr > th {
    color: #575454;
    -webkit-user-select: none;
    user-select: none;    
    &:hover {
      background-color: #e9ecef;
      transition: background-color .6s;
    }
  }
}

::v-deep(.card) {
  padding: 0;  
}

::v-deep(.p-datatable .p-datatable-header) {
  border-radius: 12px 12px 0 0;
}

::v-deep(.p-datatable .p-paginator-bottom) {
  border-radius: 12px;
}
</style>

<style>
.dt-td:hover {
  color: var(--blue-300);
}

span.exact-active {
  font-weight: 700;
  color: var(--primary-color);
}

.dt-shadow {
  border-radius: 12px;
  box-shadow: 0 8px 16px 0 rgb(133 133 133 / 28%);
}

.tb-header-boundary-element {  
  padding: 0.76rem 0.75rem;    
  margin-top: -1px; 
  -webkit-user-select: none;
  user-select: none;
}

.hoverbtn.tb-header-boundary-element:hover {
  
}

.nowrap {
   white-space: nowrap;    
}

.p-dialog {
  border-radius: 12px;
}

.p-dialog .p-dialog-header {
  border-top-right-radius: 12px;
  border-top-left-radius: 12px;
}

.p-dialog .p-dialog-footer {
  border-bottom-right-radius: 12px;
  border-bottom-left-radius: 12px;
}
</style>