<script setup>
  const name = "SideBar";
  import { useStore } from 'vuex';
  import { computed, watch } from 'vue';
  import { RouterLink, useRoute } from 'vue-router';
  const route = useRoute();
  const userId = localStorage.getItem('userId');
  const store = useStore();
  watch(
    () => route.meta.hideSideBar,
    (hideSideBar) => {
      store.dispatch('setSidebarVisibility', !hideSideBar);
    },
    { immediate: true } // Trigger on component load
  );
  const isSidebarVisible = computed(() => store.getters.isSidebarVisible);
  const isActiveLink = (routePath) => {
    const route = useRoute();
    return route.path === routePath;
  };
</script>
<template>
  <!-- give the sidebar z-50 class so its higher than the navbar if you want to see the logo -->
  <!-- you will need to add a little "X" button next to the logo in order to close it though -->
  <div v-if="isSidebarVisible" class="w-64 bg-gray-100 border-r min-h-screen h-screen">

    <div class="w-full h-20 border-b flex px-4 items-center mb-8">
      <RouterLink to="/" class="font-semibold text-3xl text-blue-400 pl-4">HUSTask</RouterLink>
    </div>
    <div class="mb-4 px-4">
      <p class="pl-4 text-sm font-semibold mb-1">PERSONAL</p>
      <RouterLink :to="`/${userId}/all-my-tasks`" :class="[
        'w-full flex items-center h-10 pl-4 rounded-lg cursor-pointer text-gray-800', // Text remains gray
        isActiveLink(`/${userId}/all-my-tasks`)
          ? 'bg-gray-200' // Active background
          : 'bg-gray-100 hover:bg-gray-200', // Default and hover background
      ]">
        <svg class="h-6 w-6 fill-current text-blue-400 mr-2" viewBox="0 0 20 20">
          <path stroke="currentColor" stroke-linecap="round" stroke-width="2"
            d="M9 8h10M9 12h10M9 16h10M4.99 8H5m-.02 4h.01m0 4H5" />
        </svg>
        <span>All my tasks</span>
      </RouterLink>
      <RouterLink :to="`/${userId}/next-5-days`" :class="[
        'w-full flex items-center h-10 pl-4 rounded-lg cursor-pointer text-gray-700',  // Text stays the same color
        isActiveLink(`/${userId}/next-5-days`)
          ? 'bg-gray-200'  // Active background (darker)
          : 'bg-gray-100 hover:bg-gray-200', // Default background with hover effect
      ]">
        <svg class="h-6 w-6 fill-current text-blue-400 mr-2" xmlns="http://www.w3.org/2000/svg"
          shape-rendering="geometricPrecision" text-rendering="geometricPrecision" image-rendering="optimizeQuality"
          fill-rule="evenodd" clip-rule="evenodd" viewBox="0 0 512 502.778">
          <path fill-rule="nonzero"
            d="M465.324 502.778H46.678C21.051 502.778 0 481.727 0 456.119V73.082C0 47.454 21.051 26.42 46.679 26.42h69.25v61.016c0 55.473 92.722 55.712 92.722 0V26.42h94.695v61.016c0 56.537 92.722 54.754 92.722 0V26.42h69.247c25.644 0 46.679 21.017 46.679 46.662 0 127.681.006 255.352.006 383.037 0 25.661-21.015 46.659-46.676 46.659zM199.55 409.155l5.998-35.348c13.568 3.284 25.458 4.927 35.67 4.927 10.212 0 18.602-.429 25.172-1.285v-10.712l-16.067-1.714c-17.71-1.857-29.743-4.892-36.098-9.105-6.356-4.213-9.534-11.747-9.534-22.601 0-1.572.714-20.138 2.143-55.701h97.475l-3.213 36.848-50.773-2.999v10.926l12.64 1.499c18.709 2.286 31.635 6.891 38.776 13.818 7.141 6.927 10.711 16.603 10.711 29.029 0 8.998-1.214 16.603-3.642 22.815-2.428 6.213-5.32 10.927-8.676 14.14-3.356 3.213-8.105 5.677-14.247 7.391-6.141 1.714-11.532 2.75-16.174 3.106-4.642.357-10.819.536-18.531.536-18.567 0-35.777-1.857-51.63-5.57zM51.649 487.846h408.686c20.197 0 36.706-16.779 36.706-36.722v-253.07H14.956v253.082c0 20.149 16.49 36.71 36.693 36.71zM327.684 20.488c0-9.956 9.848-20.488 22.024-20.488 12.175 0 22.022 10.532 22.022 20.488v66.948c0 9.963-9.847 18.051-22.022 18.051-12.176 0-22.024-8.088-22.024-18.051V20.488zm-187.417 0c0-9.956 9.848-20.488 22.018-20.488 12.182 0 22.028 10.532 22.028 20.488v66.948c0 9.963-9.846 18.051-22.028 18.051-12.17 0-22.018-8.088-22.018-18.051V20.488z" />
        </svg>

        <span>Next 5 days</span>
      </RouterLink>
      <RouterLink :to="`/${userId}/my-day`" :class="[
        'w-full flex items-center h-10 pl-4 rounded-lg cursor-pointer text-gray-700',  // Text stays the same color
        isActiveLink(`/${userId}/my-day`)
          ? 'bg-gray-200'  // Active background (darker)
          : 'bg-gray-100 hover:bg-gray-200', // Default background with hover effect
      ]">
        <svg class="h-6 w-6 fill-current text-blue-400 mr-2" viewBox="0 0 20 20">
          <path
            d="M17.283,5.549h-5.26V4.335c0-0.222-0.183-0.404-0.404-0.404H8.381c-0.222,0-0.404,0.182-0.404,0.404v1.214h-5.26c-0.223,0-0.405,0.182-0.405,0.405v9.71c0,0.223,0.182,0.405,0.405,0.405h14.566c0.223,0,0.404-0.183,0.404-0.405v-9.71C17.688,5.731,17.506,5.549,17.283,5.549 M8.786,4.74h2.428v0.809H8.786V4.74z M16.879,15.26H3.122v-4.046h5.665v1.201c0,0.223,0.182,0.404,0.405,0.404h1.618c0.222,0,0.405-0.182,0.405-0.404v-1.201h5.665V15.26z M9.595,9.583h0.81v2.428h-0.81V9.583zM16.879,10.405h-5.665V9.19c0-0.222-0.183-0.405-0.405-0.405H9.191c-0.223,0-0.405,0.183-0.405,0.405v1.215H3.122V6.358h13.757V10.405z">
          </path>
        </svg>
        <span>My day</span>
      </RouterLink>
      <RouterLink :to="`/${userId}/my-calendar`" class=" w-full flex items-center text-blue-400 h-10 pl-4 hover:bg-gray-200
        rounded-lg cursor-pointer">
        <svg class="h-6 w-6 fill-current mr-2" viewBox="0 0 20 20">
          <path
            d="M16.557,4.467h-1.64v-0.82c0-0.225-0.183-0.41-0.409-0.41c-0.226,0-0.41,0.185-0.41,0.41v0.82H5.901v-0.82c0-0.225-0.185-0.41-0.41-0.41c-0.226,0-0.41,0.185-0.41,0.41v0.82H3.442c-0.904,0-1.64,0.735-1.64,1.639v9.017c0,0.904,0.736,1.64,1.64,1.64h13.114c0.904,0,1.64-0.735,1.64-1.64V6.106C18.196,5.203,17.461,4.467,16.557,4.467 M17.377,15.123c0,0.453-0.366,0.819-0.82,0.819H3.442c-0.453,0-0.82-0.366-0.82-0.819V8.976h14.754V15.123z M17.377,8.156H2.623V6.106c0-0.453,0.367-0.82,0.82-0.82h1.639v1.23c0,0.225,0.184,0.41,0.41,0.41c0.225,0,0.41-0.185,0.41-0.41v-1.23h8.196v1.23c0,0.225,0.185,0.41,0.41,0.41c0.227,0,0.409-0.185,0.409-0.41v-1.23h1.64c0.454,0,0.82,0.367,0.82,0.82V8.156z">
          </path>
        </svg>
        <span class="text-gray-700">My calender</span>
      </RouterLink>
    </div>

    <div class="mb-4 px-4">
      <p class="pl-4 text-sm font-semibold mb-1">PROJECTS</p>
      <RouterLink :to="`/${userId}/add-project`" :class="[
        'w-full flex items-center h-10 pl-4 rounded-lg cursor-pointer text-gray-700',  // Text stays the same color
        isActiveLink(`/${userId}/add-project`)
          ? 'bg-gray-200'  // Active background (darker)
          : 'bg-gray-100 hover:bg-gray-200', // Default background with hover effect
      ]">
        <svg class="h-6 w-6 fill-current text-blue-400 mr-2" viewBox="0 0 20 20">
          <path
            d="M14.613,10c0,0.23-0.188,0.419-0.419,0.419H10.42v3.774c0,0.23-0.189,0.42-0.42,0.42s-0.419-0.189-0.419-0.42v-3.774H5.806c-0.23,0-0.419-0.189-0.419-0.419s0.189-0.419,0.419-0.419h3.775V5.806c0-0.23,0.189-0.419,0.419-0.419s0.42,0.189,0.42,0.419v3.775h3.774C14.425,9.581,14.613,9.77,14.613,10 M17.969,10c0,4.401-3.567,7.969-7.969,7.969c-4.402,0-7.969-3.567-7.969-7.969c0-4.402,3.567-7.969,7.969-7.969C14.401,2.031,17.969,5.598,17.969,10 M17.13,10c0-3.932-3.198-7.13-7.13-7.13S2.87,6.068,2.87,10c0,3.933,3.198,7.13,7.13,7.13S17.13,13.933,17.13,10">
          </path>
        </svg>
        <span class="text-gray-700">Add Project</span>
      </RouterLink>
      <RouterLink :to="`/${userId}/projects`" :class="[
        'w-full flex items-center h-10 pl-4 rounded-lg cursor-pointer text-gray-700',  // Text stays the same color
        isActiveLink(`/${userId}/projects`)
          ? 'bg-gray-200'  // Active background (darker)
          : 'bg-gray-100 hover:bg-gray-200', // Default background with hover effect
      ]">
        <svg class="h-6 w-6 fill-current text-blue-400 mr-2" viewBox="0 0 20 20">
          <path
            d="M15.396,2.292H4.604c-0.212,0-0.385,0.174-0.385,0.386v14.646c0,0.212,0.173,0.385,0.385,0.385h10.792c0.211,0,0.385-0.173,0.385-0.385V2.677C15.781,2.465,15.607,2.292,15.396,2.292 M15.01,16.938H4.99v-2.698h1.609c0.156,0.449,0.586,0.771,1.089,0.771c0.638,0,1.156-0.519,1.156-1.156s-0.519-1.156-1.156-1.156c-0.503,0-0.933,0.321-1.089,0.771H4.99v-3.083h1.609c0.156,0.449,0.586,0.771,1.089,0.771c0.638,0,1.156-0.518,1.156-1.156c0-0.638-0.519-1.156-1.156-1.156c-0.503,0-0.933,0.322-1.089,0.771H4.99V6.531h1.609C6.755,6.98,7.185,7.302,7.688,7.302c0.638,0,1.156-0.519,1.156-1.156c0-0.638-0.519-1.156-1.156-1.156c-0.503,0-0.933,0.322-1.089,0.771H4.99V3.062h10.02V16.938z M7.302,13.854c0-0.212,0.173-0.386,0.385-0.386s0.385,0.174,0.385,0.386s-0.173,0.385-0.385,0.385S7.302,14.066,7.302,13.854 M7.302,10c0-0.212,0.173-0.385,0.385-0.385S8.073,9.788,8.073,10s-0.173,0.385-0.385,0.385S7.302,10.212,7.302,10 M7.302,6.146c0-0.212,0.173-0.386,0.385-0.386s0.385,0.174,0.385,0.386S7.899,6.531,7.688,6.531S7.302,6.358,7.302,6.146">
          </path>
        </svg>
        <span class="text-gray-700">View Project</span>
      </RouterLink>
    </div>

    <div class="mb-4 px-4">
      <p class="pl-4 text-sm font-semibold mb-1">Configuration</p>
      <RouterLink :to="`/${userId}/account`" :class="[
        'w-full flex items-center h-10 pl-4 rounded-lg cursor-pointer text-gray-700',  // Text stays the same color
        isActiveLink(`/${userId}/account`)
          ? 'bg-gray-200'  // Active background (darker)
          : 'bg-gray-100 hover:bg-gray-200', // Default background with hover effect
      ]">
        <svg class="h-6 w-6 fill-current text-blue-400 mr-2" viewBox="0 0 32 32">
          <path
            d="M16 31C7.729 31 1 24.271 1 16S7.729 1 16 1s15 6.729 15 15-6.729 15-15 15zm0-28C8.832 3 3 8.832 3 16s5.832 13 13 13 13-5.832 13-13S23.168 3 16 3z" />
          <circle cx="16" cy="15.133" r="4.267" />
          <path
            d="M16 30c2.401 0 4.66-.606 6.635-1.671-.425-3.229-3.18-5.82-6.635-5.82s-6.21 2.591-6.635 5.82A13.935 13.935 0 0 0 16 30z" />
        </svg>

        <span class="text-gray-700">Account</span>
      </RouterLink>
      <RouterLink :to="`/${userId}/change-password`" :class="[
        'w-full flex items-center h-10 pl-4 rounded-lg cursor-pointer text-gray-700',  // Text stays the same color
        isActiveLink(`/${userId}/change-password`)
          ? 'bg-gray-200'  // Active background (darker)
          : 'bg-gray-100 hover:bg-gray-200', // Default background with hover effect
      ]">
        <svg class="h-6 w-6 fill-current text-blue-400 mr-2" viewBox="0 0 20 20">
          <path
            d="M17.498,11.697c-0.453-0.453-0.704-1.055-0.704-1.697c0-0.642,0.251-1.244,0.704-1.697c0.069-0.071,0.15-0.141,0.257-0.22c0.127-0.097,0.181-0.262,0.137-0.417c-0.164-0.558-0.388-1.093-0.662-1.597c-0.075-0.141-0.231-0.22-0.391-0.199c-0.13,0.02-0.238,0.027-0.336,0.027c-1.325,0-2.401-1.076-2.401-2.4c0-0.099,0.008-0.207,0.027-0.336c0.021-0.158-0.059-0.316-0.199-0.391c-0.503-0.274-1.039-0.498-1.597-0.662c-0.154-0.044-0.32,0.01-0.416,0.137c-0.079,0.106-0.148,0.188-0.22,0.257C11.244,2.956,10.643,3.207,10,3.207c-0.642,0-1.244-0.25-1.697-0.704c-0.071-0.069-0.141-0.15-0.22-0.257C7.987,2.119,7.821,2.065,7.667,2.109C7.109,2.275,6.571,2.497,6.07,2.771C5.929,2.846,5.85,3.004,5.871,3.162c0.02,0.129,0.027,0.237,0.027,0.336c0,1.325-1.076,2.4-2.401,2.4c-0.098,0-0.206-0.007-0.335-0.027C3.001,5.851,2.845,5.929,2.77,6.07C2.496,6.572,2.274,7.109,2.108,7.667c-0.044,0.154,0.01,0.32,0.137,0.417c0.106,0.079,0.187,0.148,0.256,0.22c0.938,0.936,0.938,2.458,0,3.394c-0.069,0.072-0.15,0.141-0.256,0.221c-0.127,0.096-0.181,0.262-0.137,0.416c0.166,0.557,0.388,1.096,0.662,1.596c0.075,0.143,0.231,0.221,0.392,0.199c0.129-0.02,0.237-0.027,0.335-0.027c1.325,0,2.401,1.076,2.401,2.402c0,0.098-0.007,0.205-0.027,0.334C5.85,16.996,5.929,17.154,6.07,17.23c0.501,0.273,1.04,0.496,1.597,0.66c0.154,0.047,0.32-0.008,0.417-0.137c0.079-0.105,0.148-0.186,0.22-0.256c0.454-0.453,1.055-0.703,1.697-0.703c0.643,0,1.244,0.25,1.697,0.703c0.071,0.07,0.141,0.15,0.22,0.256c0.073,0.098,0.188,0.152,0.307,0.152c0.036,0,0.073-0.004,0.109-0.016c0.558-0.164,1.096-0.387,1.597-0.66c0.141-0.076,0.22-0.234,0.199-0.393c-0.02-0.129-0.027-0.236-0.027-0.334c0-1.326,1.076-2.402,2.401-2.402c0.098,0,0.206,0.008,0.336,0.027c0.159,0.021,0.315-0.057,0.391-0.199c0.274-0.5,0.496-1.039,0.662-1.596c0.044-0.154-0.01-0.32-0.137-0.416C17.648,11.838,17.567,11.77,17.498,11.697 M16.671,13.334c-0.059-0.002-0.114-0.002-0.168-0.002c-1.749,0-3.173,1.422-3.173,3.172c0,0.053,0.002,0.109,0.004,0.166c-0.312,0.158-0.64,0.295-0.976,0.406c-0.039-0.045-0.077-0.086-0.115-0.123c-0.601-0.6-1.396-0.93-2.243-0.93s-1.643,0.33-2.243,0.93c-0.039,0.037-0.077,0.078-0.116,0.123c-0.336-0.111-0.664-0.248-0.976-0.406c0.002-0.057,0.004-0.113,0.004-0.166c0-1.75-1.423-3.172-3.172-3.172c-0.054,0-0.11,0-0.168,0.002c-0.158-0.312-0.293-0.639-0.405-0.975c0.044-0.039,0.085-0.078,0.124-0.115c1.236-1.236,1.236-3.25,0-4.486C3.009,7.719,2.969,7.68,2.924,7.642c0.112-0.336,0.247-0.664,0.405-0.976C3.387,6.668,3.443,6.67,3.497,6.67c1.75,0,3.172-1.423,3.172-3.172c0-0.054-0.002-0.11-0.004-0.168c0.312-0.158,0.64-0.293,0.976-0.405C7.68,2.969,7.719,3.01,7.757,3.048c0.6,0.6,1.396,0.93,2.243,0.93s1.643-0.33,2.243-0.93c0.038-0.039,0.076-0.079,0.115-0.123c0.336,0.112,0.663,0.247,0.976,0.405c-0.002,0.058-0.004,0.114-0.004,0.168c0,1.749,1.424,3.172,3.173,3.172c0.054,0,0.109-0.002,0.168-0.004c0.158,0.312,0.293,0.64,0.405,0.976c-0.045,0.038-0.086,0.077-0.124,0.116c-0.6,0.6-0.93,1.396-0.93,2.242c0,0.847,0.33,1.645,0.93,2.244c0.038,0.037,0.079,0.076,0.124,0.115C16.964,12.695,16.829,13.021,16.671,13.334 M10,5.417c-2.528,0-4.584,2.056-4.584,4.583c0,2.529,2.056,4.584,4.584,4.584s4.584-2.055,4.584-4.584C14.584,7.472,12.528,5.417,10,5.417 M10,13.812c-2.102,0-3.812-1.709-3.812-3.812c0-2.102,1.71-3.812,3.812-3.812c2.102,0,3.812,1.71,3.812,3.812C13.812,12.104,12.102,13.812,10,13.812">
          </path>
        </svg>
        <span class="text-gray-700">Password</span>
      </RouterLink>
      <RouterLink :to="`/${userId}/notices`" :class="[
        'w-full flex items-center h-10 pl-4 rounded-lg cursor-pointer text-gray-700',  // Text stays the same color
        isActiveLink(`/${userId}/notices`)
          ? 'bg-gray-200'  // Active background (darker)
          : 'bg-gray-100 hover:bg-gray-200', // Default background with hover effect
      ]">
        <svg class="h-6 w-6 fill-current text-blue-400 mr-2" viewBox="0 0 20 20">
          <path
            d="M17.592,8.936l-6.531-6.534c-0.593-0.631-0.751-0.245-0.751,0.056l0.002,2.999L5.427,9.075H2.491c-0.839,0-0.162,0.901-0.311,0.752l3.683,3.678l-3.081,3.108c-0.17,0.171-0.17,0.449,0,0.62c0.169,0.17,0.448,0.17,0.618,0l3.098-3.093l3.675,3.685c-0.099-0.099,0.773,0.474,0.773-0.296v-2.965l3.601-4.872l2.734-0.005C17.73,9.688,18.326,9.669,17.592,8.936 M3.534,9.904h1.906l4.659,4.66v1.906L3.534,9.904z M10.522,13.717L6.287,9.48l4.325-3.124l3.088,3.124L10.522,13.717z M14.335,8.845l-3.177-3.177V3.762l5.083,5.083H14.335z">
          </path>
        </svg>
        <span class="text-gray-700">Notices</span>
      </RouterLink>
    </div>
  </div>
</template>
<style scoped>

  /* Sidebar styling with fixed width */
  .SideBar {
    width: 250px;
    /* Fixed sidebar width */
    transition: width 0.3s ease-in-out;
    /* Smooth transition if you want to animate */
  }
</style>