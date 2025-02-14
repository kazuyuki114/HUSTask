<script setup>
    import { ref, onMounted, watch } from 'vue';
    import axios from 'axios';

    const props = defineProps({
        userId: {
            type: Number,
            required: true,
        },
    });

    const memberInfo = ref(null);
    const errorMessage = ref(null);

    const fetchMemberInfo = async () => {
        try {
            const response = await axios.get(`http://localhost:5059/api/users/${props.userId}`);
            memberInfo.value = response.data;
            errorMessage.value = null; // Clear previous errors
        } catch (error) {
            console.error(error);
            errorMessage.value = error.response?.data?.Message || 'Failed to fetch member information.';
            memberInfo.value = null; // Clear previous data on error
        }
    };

    onMounted(() => {
        fetchMemberInfo();
    });

    watch(() => props.userId, fetchMemberInfo);
</script>

<template>
    <div class="member-overview  bg-white rounded-lg min-screen-w">
        <h2 class="text-3xl font-bold text-gray-800 border-b-2 border-gray-300 pb-4 mb-6">Member Overview</h2>

        <!-- Error Message -->
        <div v-if="errorMessage" class="bg-red-100 text-red-600 font-semibold p-4 rounded-md mb-6">
            {{ errorMessage }}
        </div>

        <!-- Member Details -->
        <div v-else-if="memberInfo" class="member-details space-y-6">
            <!-- User Information -->
            <div>
                <h3 class="text-xl font-semibold text-gray-700 mb-3">User Information</h3>
                <ul class="space-y-2">
                    <li class="text-gray-600"><strong>Name:</strong> {{ memberInfo.name }}</li>
                    <li class="text-gray-600"><strong>Email:</strong> {{ memberInfo.email }}</li>
                    <li class="text-gray-600"><strong>Created At:</strong> {{ new
                        Date(memberInfo.createdAt).toLocaleDateString() }}</li>
                    <li class="text-gray-600"><strong>Updated At:</strong> {{ new
                        Date(memberInfo.updatedAt).toLocaleDateString() }}</li>
                </ul>
            </div>

            <!-- Contact Information -->
            <div v-if="memberInfo.contact" class="contact-details pt-6 border-t border-gray-300">
                <h3 class="text-xl font-semibold text-gray-700 mb-3">Contact Information</h3>
                <ul class="space-y-2">
                    <li class="text-gray-600"><strong>Email:</strong> {{ memberInfo.contact.email }}</li>
                    <li class="text-gray-600"><strong>Gender:</strong> {{ memberInfo.contact.gender }}</li>
                    <li class="text-gray-600"><strong>Occupation:</strong> {{ memberInfo.contact.occupation }}</li>
                    <li class="text-gray-600"><strong>Phone:</strong> {{ memberInfo.contact.phone }}</li>
                    <li class="text-gray-600"><strong>Notes:</strong> {{ memberInfo.contact.notes }}</li>
                    <li class="text-gray-600"><strong>Date of Birth:</strong> {{ new
                        Date(memberInfo.contact.dob).toLocaleDateString() }}</li>
                </ul>
            </div>
            <div v-else class="text-gray-500">
                <p>No contact information available.</p>
            </div>
        </div>

        <!-- Loading State -->
        <div v-else class="text-gray-500">
            <p>Loading member information...</p>
        </div>
    </div>
</template>

<style scoped>
    .member-overview {
        font-family: 'Inter', sans-serif;
    }
</style>
