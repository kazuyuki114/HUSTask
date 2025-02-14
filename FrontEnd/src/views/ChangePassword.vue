<script setup>
    import { ref } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';
    import postLog from '@/logs/postLog';

    const currentPassword = ref('');
    const newPassword = ref('');
    const confirmPassword = ref('');
    const errorMessage = ref('');
    const successMessage = ref('');
    const isLoading = ref(false);
    const router = useRouter();
    const myUserID = localStorage.getItem('userId');

    // Function to handle password change
    const changePassword = async () => {
        // Check if the new passwords match
        if (newPassword.value !== confirmPassword.value) {
            errorMessage.value = 'Passwords do not match.';
            return;
        }

        const token = localStorage.getItem('token'); // Retrieve token from local storage

        if (!token) {
            errorMessage.value = 'No authorization token found.';
            return;
        }

        // Set the headers for the request
        const headers = {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        };

        try {
            isLoading.value = true;
            const response = await axios.put(
                'http://localhost:5059/api/auth/change-password',
                {
                    userId: myUserID,
                    currentPassword: currentPassword.value,
                    newPassword: newPassword.value,
                },
                { headers }
            );

            successMessage.value = 'Password updated successfully!';
            errorMessage.value = '';
            isLoading.value = false;
            // Add Log
            const logResponse = await postLog(Number(myUserID), "Your password was changed");
            console.log('Log posted:', logResponse);
            // Wait for 10 seconds and then redirect to the sign-in page
            setTimeout(() => {
                localStorage.clear(); // Clear the user data from localStorage (optional)
                router.push('/signin');
            }, 3000); // Delay for 3 seconds
        } catch (error) {
            isLoading.value = false;
            errorMessage.value =
                error.response?.data?.message || 'Failed to change password.';
            successMessage.value = '';
        }
    };

    // Function to handle logout
    const logout = () => {
        localStorage.clear();
        router.push('/signin');
    };
</script>


<template>
    <div class="change-password-view max-w-lg mx-auto mt-10 p-6 bg-white rounded shadow-md">
        <h1 class="text-2xl font-bold text-center mb-6">Change Password</h1>

        <!-- Loading Indicator -->
        <div v-if="isLoading" class="text-gray-500 text-center mb-4">Loading...</div>

        <!-- Success Message -->
        <div v-if="successMessage" class="bg-green-100 text-green-800 p-4 rounded mb-4">
            {{ successMessage }}
        </div>

        <!-- Error Message -->
        <div v-if="errorMessage" class="bg-red-100 text-red-800 p-4 rounded mb-4">
            {{ errorMessage }}
        </div>

        <!-- Password Change Form -->
        <form @submit.prevent="changePassword">
            <div class="mb-4">
                <label for="currentPassword" class="block text-gray-700 font-medium mb-2">Current Password</label>
                <input id="currentPassword" v-model="currentPassword" type="password"
                    class="border rounded w-full p-2 focus:outline-none focus:ring focus:ring-blue-200" required />
            </div>

            <div class="mb-4">
                <label for="newPassword" class="block text-gray-700 font-medium mb-2">New Password</label>
                <input id="newPassword" v-model="newPassword" type="password"
                    class="border rounded w-full p-2 focus:outline-none focus:ring focus:ring-blue-200" required />
            </div>

            <div class="mb-4">
                <label for="confirmPassword" class="block text-gray-700 font-medium mb-2">Confirm New Password</label>
                <input id="confirmPassword" v-model="confirmPassword" type="password"
                    class="border rounded w-full p-2 focus:outline-none focus:ring focus:ring-blue-200" required />
            </div>

            <div class="flex justify-between items-center">
                <button type="submit" class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition">
                    Change Password
                </button>
                <button type="button" @click="logout"
                    class="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition">
                    Log Out
                </button>
            </div>
        </form>
    </div>
</template>

<style scoped>
    .change-password-view {
        background-color: #f9f9f9;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }
</style>
