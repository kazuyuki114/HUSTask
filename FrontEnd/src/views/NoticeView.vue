<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import NoticeCard from '@/components/NoticeCard.vue';

    // Get currentUser
    const userId = Number(localStorage.getItem('userId'));

    // State to store the logs
    const notices = ref([]);
    const loading = ref(true);
    const error = ref(false);

    // Fetch logs from the backend
    const fetchLogs = async () => {
        try {
            loading.value = true;
            error.value = false; // Reset error state
            const response = await axios.get(`http://localhost:5059/api/Log/${userId}`);
            notices.value = response.data;
        } catch (err) {
            console.error('Error fetching logs:', err);
            error.value = true;
        } finally {
            loading.value = false;
        }
    };

    // Fetch logs when the component is mounted
    onMounted(() => {
        fetchLogs();
    });
</script>

<template>
    <div class="p-4 space-y-4">
        <h2 class="text-xl font-semibold text-gray-800">User Notices</h2>

        <!-- Loading State -->
        <div v-if="loading" class="text-gray-500">
            Loading notices...
        </div>

        <!-- Error State -->
        <div v-if="error" class="text-red-500">
            Failed to load notices.
        </div>

        <!-- No Notices State -->
        <div v-if="!loading && !error && notices.length === 0" class="text-gray-500">
            No notices available.
        </div>

        <!-- Notices List -->
        <div v-if="!loading && !error && notices.length > 0" class="space-y-4">
            <NoticeCard v-for="notice in notices" :key="notice.id" :notice="notice" />
        </div>
    </div>
</template>

<style scoped>
    /* Add optional styles */
</style>
