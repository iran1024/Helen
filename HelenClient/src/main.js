import { createApp, reactive } from 'vue'
import api from './api'
import AppWrapper from './AppWrapper.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'

import PrimeVue from 'primevue/config';
import Avatar from 'primevue/avatar';
import Badge from 'primevue/badge';
import Button from 'primevue/button';
import Calendar from 'primevue/calendar';
import Card from 'primevue/card';
import Carousel from 'primevue/carousel';
import Chart from 'primevue/chart';
import Checkbox from 'primevue/checkbox';
import Chip from 'primevue/chip';
import Chips from 'primevue/chips';
import Column from 'primevue/column';
import ColumnGroup from 'primevue/columngroup';
import ConfirmationService from 'primevue/confirmationservice';
import ConfirmDialog from 'primevue/confirmdialog';
import ConfirmPopup from 'primevue/confirmpopup';
import ContextMenu from 'primevue/contextmenu';
import DataTable from "primevue/datatable";
import Dialog from 'primevue/dialog';
import Dropdown from 'primevue/dropdown';
import InputText from 'primevue/inputtext';
import Knob from 'primevue/knob'
import MultiSelect from 'primevue/multiselect';
import OrganizationChart from 'primevue/organizationchart';
import OverlayPanel from 'primevue/overlaypanel';
import Paginator from 'primevue/paginator';
import ProgressBar from 'primevue/progressbar';
import RadioButton from 'primevue/radiobutton';
import Row from 'primevue/row';
import Ripple from 'primevue/ripple'
import ScrollPanel from "primevue/scrollpanel";
import Slider from 'primevue/slider';
import SpeedDial from 'primevue/speeddial';
import StyleClass from 'primevue/styleclass';
import TabMenu from 'primevue/tabmenu';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';
import Tooltip from 'primevue/tooltip';

import SvgIcon from '@jamescoyle/vue-icon';

import 'primevue/resources/primevue.min.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css'
import './assets/styles/layout.scss'

import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

router.beforeEach(function(to, from, next) {
    window.scrollTo(0, 0);
    next();
});

const app = createApp(AppWrapper);

app.config.globalProperties.$api = api;
app.config.globalProperties.$appState = reactive({ theme: 'lara-light-indigo', darkTheme: false });

app.use(store).use(router);
app.use(PrimeVue, {
    ripple: true,
});
app.use(ConfirmationService);
app.use(ToastService);

app.directive('ripple', Ripple);
app.directive('styleclass', StyleClass);
app.directive('Tooltip', Tooltip);

app.component('Avatar', Avatar);
app.component('Badge', Badge);
app.component('Button', Button);
app.component('Calendar', Calendar);
app.component('Card', Card);
app.component('Carousel', Carousel);
app.component('Chart', Chart);
app.component('Checkbox', Checkbox);
app.component('Chip', Chip);
app.component('Chips', Chips);
app.component('Column', Column);
app.component('ColumnGroup', ColumnGroup);
app.component('ConfirmDialog', ConfirmDialog);
app.component('ConfirmPopup', ConfirmPopup);
app.component('ContextMenu', ContextMenu);
app.component('DataTable', DataTable);
app.component('Dialog', Dialog);
app.component('Dropdown', Dropdown);
app.component('InputText', InputText);
app.component('Knob', Knob);
app.component('MultiSelect', MultiSelect);
app.component('OrganizationChart', OrganizationChart);
app.component('OverlayPanel', OverlayPanel);
app.component('Paginator', Paginator);
app.component('ProgressBar', ProgressBar);
app.component('RadioButton', RadioButton);
app.component('Row', Row);
app.component('ScrollPanel', ScrollPanel);
app.component('Slider', Slider);
app.component('SpeedDial', SpeedDial);
app.component('TabMenu', TabMenu);
app.component('Toast', Toast);

app.component('SvgIcon', SvgIcon);
// <SvgIcon type="mdi" :path="bugIcon(变量)"></SvgIcon>

import { library } from '@fortawesome/fontawesome-svg-core';
import { faBug } from "@fortawesome/free-solid-svg-icons";
import { faFlask } from '@fortawesome/free-solid-svg-icons';
import { faHouseChimney } from '@fortawesome/free-solid-svg-icons';
import { faListUl } from '@fortawesome/free-solid-svg-icons';
import { faNoteSticky } from '@fortawesome/free-solid-svg-icons';
import { faChartLine } from '@fortawesome/free-solid-svg-icons';
import { faCircle } from '@fortawesome/free-solid-svg-icons';
import { faVial } from '@fortawesome/free-solid-svg-icons';
import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import { faGripfire } from '@fortawesome/free-brands-svg-icons';

library.add(...[faBug, faFlask, faHouseChimney, faListUl, faNoteSticky, faChartLine, faCircle, faVial, faCirclePlus, faGripfire]);

app.component('FontAwesomeIcon', FontAwesomeIcon);

app.mount('#app');