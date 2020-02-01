import Vue from "vue";
import VueRouter from "vue-router";
import Inventory from "@/views/Inventory.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    name: 'home',
    component: Inventory
  },
  {
    path: '/inventory',
    name: 'inventory',
    component: Inventory
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

export default router;
