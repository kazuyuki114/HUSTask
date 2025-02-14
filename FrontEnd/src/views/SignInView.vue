<script setup>
  import { ref } from 'vue';
  import { RouterLink, useRouter } from 'vue-router';
  import axios from 'axios';
  import { useToast } from 'vue-toastification';
  import postLog from '@/logs/postLog';
  const signInForm = ref({
    email: '',
    password: '',
  });

  const showPassword = ref(false); // State to toggle password visibility

  const togglePasswordVisibility = () => {
    showPassword.value = !showPassword.value; // Toggle the visibility
  };

  const router = useRouter();
  const toast = useToast();
  const handleSignIn = async () => {
    try {
      const response = await axios.post('http://localhost:5059/api/Auth/login', {
        email: signInForm.value.email,
        password: signInForm.value.password,
      });

      // Check and extract the required data
      const { token, id } = response.data;

      if (token && id) {
        // Store the JWT token in localStorage
        localStorage.setItem('token', token);
        localStorage.setItem('userId', id);
        // Toast
        toast.success('Sign in completed');
        // Redirect to the dashboard or another route
        router.push(`/${id}/all-my-tasks`);
      } else {
        console.error('Invalid response from the server:', response.data);
        alert('Login failed. Please try again.');
      }

      // Add Log
      const logResponse = await postLog(id, "Sign in");
      console.log('Log posted:', logResponse);

    } catch (error) {
      if (error.response) {
        // Server-side error (e.g., 401 Unauthorized, 400 Bad Request)
        console.error('Error from server:', error.response.data);
        alert(error.response.data || 'Login failed.');
      } else {
        // Client-side or network error
        console.error('An error occurred:', error.message);
        alert('An error occurred. Please try again later.');
      }
    }
  };
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="max-w-md w-full p-6 bg-white rounded-lg shadow-lg">
      <h1 class="text-2xl font-semibold text-center text-gray-500 mt-8 mb-6">
        Sign in
      </h1>
      <form @submit.prevent="handleSignIn">
        <div class="mb-6">
          <label for="email" class="block mb-2 text-sm text-gray-600">Email</label>
          <input type="email" id="email" v-model="signInForm.email"
            class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-cyan-500" required />
        </div>
        <div class="mb-6">
          <label for="password" class="block mb-2 text-sm text-gray-600">Password</label>
          <div class="relative">
            <!-- Password Input Field -->
            <input :type="showPassword ? 'text' : 'password'" id="password" v-model="signInForm.password"
              class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-cyan-500 pr-10"
              required />
            <!-- Toggle Password Visibility Icon -->
            <button type="button" @click="togglePasswordVisibility"
              class="absolute inset-y-0 right-2 flex items-center">
              <svg v-if="!showPassword" xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-600"
                viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"
                stroke-linejoin="round">
                <path d="M1 12s4-7 11-7 11 7 11 7-4 7-11 7-11-7-11-7z" />
                <circle cx="12" cy="12" r="3" />
              </svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-600" viewBox="0 0 24 24"
                fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path
                  d="M13.875 18.825A10.05 10.05 0 0 1 12 19C5 19 1 12 1 12a17.6 17.6 0 0 1 2.362-3.43M6 6l12 12M18.368 15.578A17.48 17.48 0 0 0 23 12s-4-7-11-7a10.05 10.05 0 0 0-4.847 1.193" />
              </svg>
            </button>
          </div>
          <RouterLink to="/forgot-password" class="block text-right text-xs text-blue-600 mt-2">
            Forgot password?
          </RouterLink>
        </div>
        <button type="submit"
          class="w-32 bg-gradient-to-r from-blue-400 to-blue-600 text-white py-2 rounded-lg mx-auto block focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-cyan-500 mt-4 mb-6">
          Sign in
        </button>
      </form>
      <div class="text-center">
        <p class="text-sm">
          You don't have an account?
          <RouterLink to="/signup" class="text-blue-600">Sign up</RouterLink>
        </p>
      </div>
    </div>
  </div>
</template>

<style scoped>
  /* Optional: Add responsive styles */
</style>
