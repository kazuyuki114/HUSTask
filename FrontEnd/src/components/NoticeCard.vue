<script setup>
    // Define props for the notice object
    const props = defineProps({
        notice: {
            type: Object,
            required: true,
        },
    });
</script>

<template>
    <div class="bg-blue-50 rounded-lg shadow-md p-4 border border-blue-200">
        <!-- Header Section -->
        <div class="flex items-center justify-between border-b pb-2 mb-2">
            <h3 class="text-lg font-semibold text-blue-800">Activity</h3>
            <span class="text-sm text-gray-500">{{ notice.timestamp }}</span>
        </div>

        <!-- Activity Description -->
        <div class="text-gray-700">
            <p>{{ notice.description }}</p>
        </div>
    </div>
</template>

<style scoped>

    /* Add hover effect for better UI */
    div {
        transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    div:hover {
        transform: translateY(-3px);
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }
</style>
