<script setup>
  import { useRoute } from 'vue-router';
  import { computed } from 'vue';
  import NavBar from '@/components/NavBar.vue';
  import SideBar from '@/components/SideBar.vue';

  const route = useRoute();

  // Computed properties to control visibility of NavBar and SideBar
  const showNavBar = computed(() => !route.meta.hideNavBar);
  const showSideBar = computed(() => !route.meta.hideSideBar);
</script>

<template>
  <!-- Main Container -->
  <div class="flex min-h-screen flex-col">
    <!-- NavBar - only visible when showNavBar is true -->
    <NavBar v-if="showNavBar" />

    <!-- Main Content Area -->
    <div class="flex flex-1">
      <SideBar v-if="showSideBar" />
      <div class="flex-1">
        <router-view></router-view>
      </div>
    </div>
  </div>
</template>

<style scoped>

  /* Main Layout */
  .flex {
    display: flex;
  }

  .min-h-screen {
    min-height: 100vh;
  }

  .flex-1 {
    flex: 1;
  }

  .flex-col {
    flex-direction: column;
  }

  .flex-1 {
    flex: 1;
  }

  .SideBar {
    width: 250px;
    min-width: 500px;
  }

  .router-view {
    padding: 20px;
    background-color: #f9f9f9;
  }
</style>
