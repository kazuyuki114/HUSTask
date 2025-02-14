<script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios'; // Import Axios
  import { useRouter, useRoute } from 'vue-router';
  import logo from '@/assets/img/logo.png'
  const isActiveLink = (routePath) => {
    const route = useRoute();
    return route.path === routePath;
  };

  const isAuthenticated = ref(false);
  const userName = ref('');
  const router = useRouter();
  const userId = localStorage.getItem('userId');
  const token = localStorage.getItem('token');
  const checkAuthState = async () => {

    if (userId && token) {
      isAuthenticated.value = true;
      try {
        // Use Axios to fetch user information
        const response = await axios.get(`http://localhost:5059/api/users/${userId}`, {
          headers: {
            Authorization: `Bearer ${token}`, // Include token in Authorization header
          },
        });
        if (response.data && response.data.name) {
          userName.value = response.data.name; // Extract Name from response
        } else {
          console.error('Name field is missing in the API response.');
        }
      } catch (error) {
        console.error('Error fetching user info:', error);
        isAuthenticated.value = false;
      }
    } else {
      isAuthenticated.value = false;
    }
  };

  const logout = () => {
    localStorage.removeItem('userId');
    localStorage.removeItem('token');
    isAuthenticated.value = false;
    router.push('/signin');
  };

  // Fetch authentication state when component mounts
  onMounted(() => {
    checkAuthState();
  });
</script>

<template>
  <nav v-if="true" class="bg-blue-600 border-b border-blue-900">
    <div class="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
      <div class="flex h-20 items-center justify-between">
        <div class="flex flex-1 items-center justify-center md:items-stretch md:justify-start">
          <!-- Logo -->
          <RouterLink class="flex flex-shrink-0 items-center mr-4" to="/">
            <img class="h-10 w-auto" v-bind:src="logo" alt="Vue Jobs" />
            <span class="hidden md:block text-white text-2xl font-bold ml-2">HUSTask</span>
          </RouterLink>

          <div class="md:ml-auto">
            <div class="flex space-x-2">
              <template v-if="isAuthenticated">
                <RouterLink :to="`/${userId}/account`" class="flex items-center text-white font-bold px-3 py-2">
                  <!-- SVG Icon -->
                  <svg class="h-6 w-6 fill-current text-white mr-2" viewBox="0 0 32 32">
                    <path
                      d="M16 31C7.729 31 1 24.271 1 16S7.729 1 16 1s15 6.729 15 15-6.729 15-15 15zm0-28C8.832 3 3 8.832 3 16s5.832 13 13 13 13-5.832 13-13S23.168 3 16 3z" />
                    <circle cx="16" cy="15.133" r="4.267" />
                    <path
                      d="M16 30c2.401 0 4.66-.606 6.635-1.671-.425-3.229-3.18-5.82-6.635-5.82s-6.21 2.591-6.635 5.82A13.935 13.935 0 0 0 16 30z" />
                  </svg>
                  <!-- User Greeting -->
                  <span>Welcome, {{ userName || 'Guest' }}</span>
                </RouterLink>
                <div @click="logout" class="cursor-pointer flex items-center">
                  <!-- Logout SVG -->
                  <svg class="h-4 w-4 fill-current text-white" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 52 52"
                    enable-background="new 0 0 52 52" xml:space="preserve">
                    <g>
                      <path
                        d="M21,48.5v-3c0-0.8-0.7-1.5-1.5-1.5h-10C8.7,44,8,43.3,8,42.5v-33C8,8.7,8.7,8,9.5,8h10
                        C20.3,8,21,7.3,21,6.5v-3C21,2.7,20.3,2,19.5,2H6C3.8,2,2,3.8,2,6v40c0,2.2,1.8,4,4,4h13.5C20.3,50,21,49.3,21,48.5z" />
                      <path d="M49.6,27c0.6-0.6,0.6-1.5,0-2.1L36.1,11.4c-0.6-0.6-1.5-0.6-2.1,0l-2.1,2.1c-0.6,0.6-0.6,1.5,0,2.1l5.6,5.6
                        c0.6,0.6,0.2,1.7-0.7,1.7H15.5c-0.8,0-1.5,0.6-1.5,1.4v3c0,0.8,0.7,1.6,1.5,1.6h21.2c0.9,0,1.3,1.1,0.7,1.7l-5.6,5.6
                        c-0.6,0.6-0.6,1.5,0,2.1l2.1,2.1c0.6,0.6,1.5,0.6,2.1,0L49.6,27z" />
                    </g>
                  </svg>
                </div>
              </template>
              <template v-else>
                <RouterLink to="/"
                  :class="['text-white', 'hover:bg-gray-900 hover:text-white', 'rounded-md', 'px-3', 'py-2']">
                  Home
                </RouterLink>
                <RouterLink to="/signin"
                  :class="['text-white', 'hover:bg-gray-900 hover:text-white', 'rounded-md', 'px-3', 'py-2']">
                  Sign In
                </RouterLink>
                <RouterLink to="/signup"
                  :class="['text-white', 'hover:bg-gray-900 hover:text-white', 'rounded-md', 'px-3', 'py-2']">
                  Sign Up
                </RouterLink>
              </template>
            </div>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>
