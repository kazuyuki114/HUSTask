import '@/assets/main.css';
import { createApp } from 'vue'
import App from './App.vue'
import 'primeicons/primeicons.css';
import router from './router';
import Toast from 'vue-toastification'; 
import 'vue-toastification/dist/index.css';
import store from './store/store';


const app = createApp(App);
// Define smooth-scroll directive
app.directive('smooth-scroll', {
    mounted(el) {
        el.addEventListener('click', (e) => {
            e.preventDefault();
            const targetId = el.getAttribute('href').substring(1);
            const targetElement = document.getElementById(targetId);
            if (targetElement) {
                targetElement.scrollIntoView({ behavior: 'smooth' });
            }
        });
    },
});
app.use(router);
app.use(Toast); 
app.use(store);
app.mount('#app');

