<script setup>
  import { ref } from 'vue';
  import { RouterLink, useRouter } from 'vue-router';
  import axios from 'axios';
  import { useToast } from 'vue-toastification';
  import postLog from '@/logs/postLog';

  const router = useRouter();
  const toast = useToast();

  const signUpForm = ref({
    name: '',
    email: '',
    password: '',
    gender: '',
    occupation: '',
    phone: '',
    dob: '',
    notes: ''
  });

  const isValidEmail = ref(false);

  const checkEmailValidity = async () => {
    try {
      const response = await axios.get(`http://localhost:5059/api/Auth/verify-email?email=${signUpForm.value.email}`);
      console.log(response.data);

      // Extract correct status field
      const status = response.data.data?.status;
      if (status === "valid") {
        isValidEmail.value = true;
      } else {
        isValidEmail.value = false;
      }
    } catch (error) {
      console.error("Error verifying email", error);
      isValidEmail.value = false;
    }
  };

  const handleSignUp = async () => {
    try {
      // Check email validity
      await checkEmailValidity();
      if (isValidEmail.value === false) {
        toast.error("Invalid email address");
        return;
      }
      // Sign up
      const response = await axios.post('http://localhost:5059/api/Users/signup', signUpForm.value);
      console.log(response.data);
      signUpForm.value = {
        name: '',
        email: '',
        password: '',
        gender: '',
        occupation: '',
        phone: '',
        dob: '',
        notes: ''
      };
      // Add Log
      const logResponse = await postLog(response.data.userId, "Created your profile");
      console.log('Log posted:', logResponse);

      // Redirect to sign-in page
      router.push('/signin');
      toast.success('Sign up successfully!');
    } catch (error) {
      console.error('Error during sign-up:', error);
      toast.error('Error signing up a new account!');
    }
  };
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="w-full max-w-md p-6 bg-white shadow-md rounded-lg">
      <h2 class="text-2xl font-bold mb-6 text-center">Sign Up</h2>

      <form @submit.prevent="handleSignUp">
        <!-- Name -->
        <div class="mb-4">
          <label for="name" class="block text-sm font-medium text-gray-700 mb-1">
            Name
          </label>
          <input v-model="signUpForm.name" type="text" id="name"
            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" required />
        </div>

        <!-- Email -->
        <div class="mb-4">
          <label for="email" class="block text-sm font-medium text-gray-700 mb-1">
            Email
          </label>
          <input v-model="signUpForm.email" type="email" id="email"
            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" required />
        </div>

        <!-- Password -->
        <div class="mb-4">
          <label for="password" class="block text-sm font-medium text-gray-700 mb-1">
            Password
          </label>
          <input v-model="signUpForm.password" type="password" id="password"
            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" required />
        </div>

        <!-- Gender -->
        <div class="mb-4">
          <label for="gender" class="block text-sm font-medium text-gray-700 mb-1">
            Gender
          </label>
          <select v-model="signUpForm.gender" id="gender"
            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
            <option value="">Select Gender</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
          </select>
        </div>

        <!-- Occupation -->
        <div class="mb-4">
          <label for="occupation" class="block text-sm font-medium text-gray-700 mb-1">
            Occupation
          </label>
          <input v-model="signUpForm.occupation" type="text" id="occupation"
            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" />
        </div>

        <!-- Phone -->
        <div class="mb-4">
          <label for="phone" class="block text-sm font-medium text-gray-700 mb-1">
            Phone
          </label>
          <input v-model="signUpForm.phone" type="text" id="phone"
            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" />
        </div>

        <!-- Date of Birth -->
        <div class="mb-4">
          <label for="dob" class="block text-sm font-medium text-gray-700 mb-1">
            Date of Birth
          </label>
          <input v-model="signUpForm.dob" type="date" id="dob"
            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" />
        </div>

        <!-- Notes -->
        <div class="mb-4">
          <label for="notes" class="block text-sm font-medium text-gray-700 mb-1">
            Notes
          </label>
          <textarea v-model="signUpForm.notes" id="notes"
            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" rows="4"></textarea>
        </div>

        <!-- Submit Button -->
        <button type="submit"
          class="w-32 bg-gradient-to-r from-blue-400 to-blue-600 text-white py-2 rounded-lg mx-auto block focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-cyan-500 mt-4 mb-6">
          Sign Up
        </button>
      </form>

      <div class="mt-6 text-center">
        <p class="text-sm text-gray-600">
          Already have an account?
          <RouterLink to="/signin" class="text-blue-500 hover:underline">
            Sign In
          </RouterLink>
        </p>
      </div>
    </div>
  </div>
</template>

<style scoped>
  /* Optional: Add responsive styles */
</style>
