<script setup>
    import MemberOverviewSection from '@/components/MemberOverviewSection.vue';
    import ProjectTaskSection from '@/components/ProjectTaskSection.vue';
    import { useRoute } from 'vue-router';
    const route = useRoute();
    const memberId = Number(route.params.memberId);
</script>

<template>
    <div class="min-h-screen bg-gray-100 p-6">
        <!-- Member Overview Section -->
        <div class="w-full bg-white shadow-md rounded-lg mb-6 p-6">
            <MemberOverviewSection :userId="memberId" />
        </div>

        <!-- Project Task Section -->
        <div class="w-full bg-white shadow-md rounded-lg p-6">
            <ProjectTaskSection />
        </div>
    </div>
</template>

<style scoped>
    /* Optional custom styles if needed */
</style>
