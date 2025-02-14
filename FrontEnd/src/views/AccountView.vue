<script setup>
    import { ref, reactive, onMounted } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';
    import postLog from '@/logs/postLog';

    // State management
    const router = useRouter();
    const userId = localStorage.getItem('userId');
    const token = localStorage.getItem('token');

    // Reactive data for user
    const user = reactive({
        name: '',
        email: '',
        createdAt: '',
        updatedAt: '',
        contact: {
            gender: '',
            occupation: '',
            phone: '',
            notes: '',
            dob: '',
        },
    });

    const isEditing = ref(false); // Toggle edit mode
    const isLoading = ref(false); // Loading state
    const errorMessage = ref('');

    // Fetch user info
    const fetchUserInfo = async () => {
        isLoading.value = true;
        try {
            const response = await axios.get(`http://localhost:5059/api/users/${userId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            Object.assign(user, response.data);
        } catch (error) {
            errorMessage.value = error.response?.data?.message || 'Failed to fetch user info.';
        } finally {
            isLoading.value = false;
        }
    };

    // Update user info
    const updateUserInfo = async () => {
        isLoading.value = true;
        try {
            await axios.put(
                `http://localhost:5059/api/users/${userId}`,
                { ...user, contact: user.contact },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );
            isEditing.value = false;
            await fetchUserInfo(); // Refresh user data

            // Add Log
            const logResponse = await postLog(userId, "Updated your profile");
            console.log('Log posted:', logResponse);

        } catch (error) {
            errorMessage.value = error.response?.data?.message || 'Failed to update user info.';
        } finally {
            isLoading.value = false;
        }
    };

    // Logout function
    const logout = () => {
        localStorage.clear(); // Clear user data
        router.push('/signin'); // Redirect to sign-in
    };

    onMounted(fetchUserInfo);
</script>

<template>
    <div class="account-view">
        <div class="container bg-white shadow-lg rounded-lg p-6">
            <h1 class="text-3xl font-semibold text-gray-700 mb-6 text-center">Account Information</h1>

            <!-- Loading Indicator -->
            <div v-if="isLoading" class="text-center text-gray-500">Loading...</div>

            <!-- Error Message -->
            <div v-if="errorMessage" class="text-center text-red-500">{{ errorMessage }}</div>

            <!-- User Info -->
            <div v-else>
                <div class="grid grid-cols-2 gap-4">
                    <div>
                        <label class="block text-gray-600 font-medium mb-1">Name:</label>
                        <input v-if="isEditing" v-model="user.name" type="text"
                            class="w-full border border-gray-300 rounded px-3 py-2" />
                        <p v-else class="bg-gray-100 rounded px-3 py-2">{{ user.name }}</p>
                    </div>

                    <div>
                        <label class="block text-gray-600 font-medium mb-1">Email:</label>
                        <p class="bg-gray-100 rounded px-3 py-2">{{ user.email }}</p>
                    </div>

                    <div>
                        <label class="block text-gray-600 font-medium mb-1">Gender:</label>
                        <select v-if="isEditing" v-model="user.contact.gender"
                            class="w-full border border-gray-300 rounded px-3 py-2">
                            <option value="">Select Gender</option>
                            <option value="Male">Male</option>
                            <option value="Female">Female</option>
                        </select>
                        <p v-else class="bg-gray-100 rounded px-3 py-2">{{ user.contact.gender }}</p>
                    </div>

                    <div>
                        <label class="block text-gray-600 font-medium mb-1">Occupation:</label>
                        <input v-if="isEditing" v-model="user.contact.occupation" type="text"
                            class="w-full border border-gray-300 rounded px-3 py-2" />
                        <p v-else class="bg-gray-100 rounded px-3 py-2">{{ user.contact.occupation }}</p>
                    </div>

                    <div>
                        <label class="block text-gray-600 font-medium mb-1">Phone:</label>
                        <input v-if="isEditing" v-model="user.contact.phone" type="text"
                            class="w-full border border-gray-300 rounded px-3 py-2" />
                        <p v-else class="bg-gray-100 rounded px-3 py-2">{{ user.contact.phone }}</p>
                    </div>

                    <div>
                        <label class="block text-gray-600 font-medium mb-1">Date of Birth:</label>
                        <input v-if="isEditing" v-model="user.contact.dob" type="date"
                            class="w-full border border-gray-300 rounded px-3 py-2" />
                        <p v-else class="bg-gray-100 rounded px-3 py-2">{{ user.contact.dob }}</p>
                    </div>
                </div>

                <div class="mt-4">
                    <label class="block text-gray-600 font-medium mb-1">Notes:</label>
                    <textarea v-if="isEditing" v-model="user.contact.notes"
                        class="w-full border border-gray-300 rounded px-3 py-2"></textarea>
                    <p v-else class="bg-gray-100 rounded px-3 py-2">{{ user.contact.notes }}</p>
                </div>

                <!-- Action Buttons -->
                <div class="mt-6 flex justify-between">
                    <button v-if="!isEditing" @click="isEditing = true"
                        class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">
                        Edit
                    </button>
                    <button v-if="isEditing" @click="updateUserInfo"
                        class="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600">
                        Save
                    </button>
                    <button v-if="isEditing" @click="isEditing = false"
                        class="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600">
                        Cancel
                    </button>
                    <button @click="logout" class="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600">
                        Log Out
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .account-view {
        max-width: 700px;
        margin: 0 auto;
        padding: 1.5rem;
        background-color: #f9fafb;
    }

    .container {
        background: #fff;
    }
</style>
